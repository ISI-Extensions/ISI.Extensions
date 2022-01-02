#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public partial class IO
	{
		public class FileNameMask
		{
			internal const string FilePrefix = "file:";

			public enum FileNameMaskType
			{
				StringReplacement,
				DateTimeMask,
				KeyValue
			}

			public string Key { get; } = null;
			public FileNameMaskType MaskType { get; } = FileNameMaskType.StringReplacement;
			public string ReplacementValue { get; } = null;
			public Func<string> GetReplacementValue { get; } = null;
			public string Description { get; } = null;

			public FileNameMask()
			{

			}

			public FileNameMask(string key, FileNameMaskType maskType, string replacementValue, string description)
			{
				Key = key;
				MaskType = maskType;
				ReplacementValue = replacementValue;
				Description = description;
			}

			public FileNameMask(string key, FileNameMaskType maskType, Func<string> getReplacementValue, string description)
			{
				Key = key;
				MaskType = maskType;
				GetReplacementValue = getReplacementValue;
				Description = description;
			}

			public string Process(string value, Func<DateTime?> dateTimeStamp)
			{
				if (value.IndexOf(Key) >= 0)
				{
					switch (MaskType)
					{
						case FileNameMaskType.StringReplacement:
							value = value.Replace(Key, (GetReplacementValue == null ? ReplacementValue : GetReplacementValue()));
							break;

						case FileNameMaskType.DateTimeMask:
							var replacementValue = ReplacementValue;
							if ((dateTimeStamp != null) && (value.IndexOf(Key) >= 0))
							{
								replacementValue = dateTimeStamp().GetValueOrDefault().ToString(replacementValue);
							}
							else
							{
								replacementValue = string.Empty;
							}

							value = value.Replace(Key, replacementValue);
							break;

						case FileNameMaskType.KeyValue:
							var keys = new HashSet<string>();

							var pattern = "\\" + Key.TrimEnd(":}") + "\\:(?<key>[^\\}]+)\\}";

							var regex = new System.Text.RegularExpressions.Regex(pattern);

							var match = regex.Match(value);

							while (match.Success)
							{
								keys.Add(match.Groups["key"].Value);

								match = match.NextMatch();
							}

							var keyType = Key.TrimStart("{").TrimEnd("}");

							if (keys.Any())
							{
								if (string.Equals(keyType, FilePrefix, StringComparison.InvariantCultureIgnoreCase))
								{
									foreach (var key in keys)
									{
										var fileName = ISI.Extensions.IO.Path.GetFileNameDeMasked(key.Trim());

										if (System.IO.File.Exists(fileName))
										{
											replacementValue = System.IO.File.ReadAllText(fileName);

											value = value.Replace(string.Format("{{{0}{1}}}", FilePrefix, key), replacementValue);
										}
									}
								}
							}

							break;
					}
				}

				return value;
			}
		}
	}
}