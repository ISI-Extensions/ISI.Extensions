#region Copyright & License
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Configuration;
using DTOs = ISI.Extensions.Repository.DataTransferObjects.RepositorySetupApi;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Repository.Cosmos
{
	public partial class RepositorySetupApi
	{
		public DTOs.SetStepResponse SetStep(int stepId)
		{
			var response = new DTOs.SetStepResponse();

			SetStepAsync(stepId).Wait();

			return response;
		}

		private async Task SetStepAsync(int stepId)
		{
			try
			{
				await GetClient().ReadDocumentCollectionAsync(Microsoft.Azure.Documents.Client.UriFactory.CreateDocumentCollectionUri(DatabaseName, DatabaseMigrationStepTableName));
			}
			catch (Microsoft.Azure.Documents.DocumentClientException documentClientException) when (documentClientException.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
				await GetClient().CreateDocumentCollectionAsync(
					Microsoft.Azure.Documents.Client.UriFactory.CreateDatabaseUri(DatabaseName),
					new()
					{
						Id = DatabaseMigrationStepTableName
					},
					new()
					{
						OfferThroughput = 1000
					});
			}

			var record = new DatabaseMigrationStepRecord()
			{
				DatabaseMigrationStepUuid = Guid.NewGuid(),
				StepId = stepId,
				CompletedDateTimeUtc = DateTimeStamper.CurrentDateTimeUtc(),
				CompletedByKey = CompletedBy,
			};

			await GetClient().CreateDocumentAsync(Microsoft.Azure.Documents.Client.UriFactory.CreateDocumentCollectionUri(DatabaseName, DatabaseMigrationStepTableName), record);
		}
	}
}