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

namespace ISI.Extensions.MessageBus.AzureServiceBus
{
	public abstract class AbstractConsumer<TRequest, TResponse> : AbstractConsumer<TRequest>
		where TRequest : class
		where TResponse : class
	{
		protected static readonly Type ResponseType = typeof(TResponse);
		protected static readonly string ResponseTypeAssemblyQualifiedNameWithoutVersion = ResponseType.AssemblyQualifiedNameWithoutVersion();

		protected AbstractConsumer(
			System.IServiceProvider serviceProvider,
			string connectionString,
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer)
			: base(serviceProvider, connectionString, jsonSerializer)
		{
		}

		protected void UpdateResponse(TRequest request, TResponse response)
		{
			if ((request is ISI.Extensions.MessageBus.ICorrelatedBy<Guid> requestCorrelatedByGuid) && (response is ISI.Extensions.MessageBus.ICorrelatedBy<Guid> responseCorrelatedByGuid))
			{
				responseCorrelatedByGuid.CorrelationId = requestCorrelatedByGuid.CorrelationId;
			}
			else if ((request is ISI.Extensions.MessageBus.ICorrelatedBy<int> requestCorrelatedByInt) && (response is ISI.Extensions.MessageBus.ICorrelatedBy<int> responseCorrelatedByInt))
			{
				responseCorrelatedByInt.CorrelationId = requestCorrelatedByInt.CorrelationId;
			}
			else if ((request is ISI.Extensions.MessageBus.ICorrelatedBy<long> requestCorrelatedByLong) && (response is ISI.Extensions.MessageBus.ICorrelatedBy<long> responseCorrelatedByLong))
			{
				responseCorrelatedByLong.CorrelationId = requestCorrelatedByLong.CorrelationId;
			}
		}

		protected Microsoft.Azure.ServiceBus.Message GetMessageEnvelope(Microsoft.Azure.ServiceBus.Message requestMessage, TResponse response)
		{
			var responseType = typeof(TResponse);

			var serializedMessage = _jsonSerializer.Serialize(responseType, response, true);

			return new(Encoding.UTF8.GetBytes(serializedMessage))
			{
				MessageId = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				Label = ResponseTypeAssemblyQualifiedNameWithoutVersion,
				ReplyToSessionId = requestMessage.SessionId,
			};
		}
	}
}
