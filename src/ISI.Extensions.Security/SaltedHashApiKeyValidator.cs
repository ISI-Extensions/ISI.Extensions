#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions.Security
{
	public class SaltedHashApiKeyValidator : IApiKeyValidator
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		protected Guid DefaultSaltedHashGeneratorTypeUuid = ISI.Extensions.Crypto.Pbkdf2SaltedHashGenerator.SaltedHashGeneratorTypeUuid.ToGuid();

		protected IDictionary<Guid, ISI.Extensions.Crypto.ISaltedHashGenerator> SaltedHashGeneratorsBySaltedHashGeneratorTypeUuid { get; }

		public SaltedHashApiKeyValidator(
			System.IServiceProvider serviceProvider,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Logger = logger;
			DateTimeStamper = dateTimeStamper;

			SaltedHashGeneratorsBySaltedHashGeneratorTypeUuid = ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementations<ISI.Extensions.Crypto.ISaltedHashGenerator>(serviceProvider).ToDictionary(saltedHashGenerator => saltedHashGenerator.SaltedHashGeneratorTypeUuid, _ => _);
		}

		public Guid GetApiKeyUuid(string apiKeyToValidate) => apiKeyToValidate.Split(':').NullCheckedFirstOrDefault()?.ToGuidNullable() ?? Guid.Empty;

		public bool TrySetHashedApiKey(IApiKey apiKey, out string apiKeyToValidate)
		{
			if (apiKey.ApiKeyUuid == Guid.Empty)
			{
				apiKey.ApiKeyUuid = Guid.NewGuid();
			}

			apiKey.SaltedHashGeneratorTypeUuid = DefaultSaltedHashGeneratorTypeUuid;

			if (SaltedHashGeneratorsBySaltedHashGeneratorTypeUuid.TryGetValue(apiKey.SaltedHashGeneratorTypeUuid, out var saltedHashGenerator))
			{
				apiKeyToValidate = string.Format("{0}:{1}-{2}-{3}", apiKey.ApiKeyUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens), Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens), Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens), Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens));

				apiKey.ApiKeySalt = saltedHashGenerator.GenerateNewCryptoSalt();

				apiKey.HashedApiKey = saltedHashGenerator.GeneratedHashedValue(apiKeyToValidate, apiKey.ApiKeySalt);

				return true;
			}

			apiKeyToValidate = null;

			return false;
		}

		public bool TryValidateApiKey(GetApiKeyDelegate getApiKey, string apiKeyToValidate, out IApiKey apiKey)
		{
			var apiKeyUuid = GetApiKeyUuid(apiKeyToValidate);

			if (apiKeyUuid == Guid.Empty)
			{
				apiKey = null;

				return false;
			}

			apiKey = getApiKey(apiKeyUuid);

			if (apiKey == null)
			{
				return false;
			}

			return TryValidateApiKey(apiKey, apiKeyToValidate);
		}

		public bool TryValidateApiKey(IApiKey apiKey, string apiKeyToValidate)
		{
			if (SaltedHashGeneratorsBySaltedHashGeneratorTypeUuid.TryGetValue(apiKey.SaltedHashGeneratorTypeUuid, out var saltedHashGenerator))
			{
				return saltedHashGenerator.ValidateValue(apiKeyToValidate, apiKey.ApiKeySalt, apiKey.HashedApiKey);
			}

			throw new Exception("SaltedHashGenerator not found");
		}
	}
}