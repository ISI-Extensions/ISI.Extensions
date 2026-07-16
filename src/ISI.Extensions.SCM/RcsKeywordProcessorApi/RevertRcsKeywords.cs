#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.RcsKeywordProcessorApi;
using SerializableDTOs = ISI.Extensions.Scm.SerializableModels.RcsKeywords;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Scm
{
	public partial class RcsKeywordProcessorApi
	{
		public DTOs.RevertRcsKeywordsResponse RevertRcsKeywords(DTOs.IRevertRcsKeywordsRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.RevertRcsKeywordsResponse();

			switch (request)
			{
				case DTOs.RevertRcsKeywordsFromCacheRequest revertRcsKeywordsFromCacheRequest:
				{
					var rcsKeywordsCacheSettings = GetRcsKeywordsCacheSettings(null) ?? new();

					var rcsKeywordsRepositories = rcsKeywordsCacheSettings?.RcsKeywordsRepositories?.ToNullCheckedDictionary(rcsKeywordsRepository => rcsKeywordsRepository.SourceDirectory, _ => _, StringComparer.InvariantCultureIgnoreCase, NullCheckDictionaryResult.Empty);

					if (rcsKeywordsRepositories.TryGetValue(revertRcsKeywordsFromCacheRequest.SourceDirectory, out var rcsKeywordsRepository))
					{
						Parallel.ForEach(rcsKeywordsRepository.RcsKeywordsFiles ?? [], rcsKeywordsFile =>
						{
							logger.LogInformation($"Reverting {System.IO.Path.GetFileName(rcsKeywordsFile.SourceFullName)}");

							var content = System.IO.File.ReadAllText(rcsKeywordsFile.ContentFullName);

							System.IO.File.Delete(rcsKeywordsFile.SourceFullName);

							System.IO.File.WriteAllText(rcsKeywordsFile.SourceFullName, content);

							System.IO.File.Delete(rcsKeywordsFile.ContentFullName);
						});

						rcsKeywordsRepositories.Remove(revertRcsKeywordsFromCacheRequest.SourceDirectory);
					}

					rcsKeywordsCacheSettings.RcsKeywordsRepositories = rcsKeywordsRepositories.Values.ToArray();

					SetRcsKeywordsCacheSettings(null, rcsKeywordsCacheSettings);
				}
					break;
				
				case DTOs.RevertRcsKeywordsRequest revertRcsKeywordsRequest:
					Parallel.ForEach(revertRcsKeywordsRequest.ModifiedFiles ?? [], modifiedFile =>
					{
						logger.LogInformation($"Reverting {System.IO.Path.GetFileName(modifiedFile.FullName)}");

						System.IO.File.Delete(modifiedFile.FullName);
						System.IO.File.WriteAllText(modifiedFile.FullName, modifiedFile.OriginalContent);
					});
					break;
				
				default:
					throw new ArgumentOutOfRangeException(nameof(request));
			}


			return response;
		}
	}
}