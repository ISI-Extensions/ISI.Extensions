﻿#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using System.Runtime.Serialization;
using System.Text;
using ISI.Extensions.Extensions;
using ISI.Extensions.VisualStudio.SerializableModels;

namespace ISI.Extensions.VisualStudio
{
	public class ProjectReference
	{
		public const string ClipboardTokenHeader = nameof(ProjectReference);

		public string Name { get; set; }
		public string Path { get; set; }
		public Guid? ProjectUuid { get; set; }
		public override string ToString() => Name;

		public string GetClipboardToken() => string.Format("{0}|{1}|{2}|{3}", ClipboardTokenHeader, Name, Path, ProjectUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));

		public static bool TryParseClipboardToken(string clipboardToken, out ProjectReference projectReference)
		{
			var clippedItem = clipboardToken.Split(['|'], StringSplitOptions.None);

			if ((clippedItem.Length >= 2) && string.Equals(clippedItem[0], ClipboardTokenHeader, StringComparison.InvariantCultureIgnoreCase))
			{
				projectReference = new()
				{
					Name = clippedItem[1],
					Path = (clippedItem.Length >= 3 ? clippedItem[2] : null),
					ProjectUuid = (clippedItem.Length >= 4 ? clippedItem[3].ToGuidNullable() : null),
				};

				return true;
			}

			projectReference = null;

			return false;
		}
	}
}
