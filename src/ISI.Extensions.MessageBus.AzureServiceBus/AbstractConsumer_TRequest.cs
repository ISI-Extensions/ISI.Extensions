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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.MessageBus.AzureServiceBus
{
	public abstract class AbstractConsumer<TRequest>
		where TRequest : class
	{
		protected static readonly Type RequestType = typeof(TRequest);
		protected static readonly string RequestTypeAssemblyQualifiedNameWithoutVersion = RequestType.AssemblyQualifiedNameWithoutVersion();

		protected readonly System.IServiceProvider _serviceProvider;
		protected readonly string _connectionString;
		protected readonly ISI.Extensions.JsonSerialization.IJsonSerializer _jsonSerializer;

		protected AbstractConsumer(
			System.IServiceProvider serviceProvider,
			string connectionString,
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer)
		{
			_serviceProvider = serviceProvider;
			_connectionString = connectionString;
			_jsonSerializer = jsonSerializer;
		}

		protected string OperationKey
		{
			get
			{
				if (System.Diagnostics.Trace.CorrelationManager.LogicalOperationStack.Count > 0)
				{
					return System.Diagnostics.Trace.CorrelationManager.LogicalOperationStack.Peek().ToString();
				}

				return string.Format("{0:D}", Guid.NewGuid());
			}
		}

		protected RequestContext<TRequest> GetRequestContext(Microsoft.Azure.ServiceBus.Message requestMessage)
		{
			var serializeRequest = Encoding.UTF8.GetString(requestMessage.Body);
			var request = _jsonSerializer.Deserialize(Type.GetType(requestMessage.Label), serializeRequest);

			return new(requestMessage, serializeRequest, request);
		}

		protected virtual void BeginRequest()
		{
			System.Diagnostics.Trace.TraceInformation(string.Format("BeginRequest RequestType: \"{0}\", OperationKey: \"{1}\"", RequestTypeAssemblyQualifiedNameWithoutVersion, OperationKey));
		}

		protected virtual void EndRequest()
		{
			System.Diagnostics.Trace.TraceInformation(string.Format("EndRequest RequestType: \"{0}\", OperationKey: \"{1}\"", RequestTypeAssemblyQualifiedNameWithoutVersion, OperationKey));
		}

		protected virtual void SetTrackingKeys(IRequestContext requestContext)
		{
			var operationKey = requestContext.MessageEnvelope?.SessionId ?? string.Empty;

			if (requestContext.Request is ISI.Extensions.IHasOperationKey hasOperationKey)
			{
				operationKey = hasOperationKey.OperationKey;
			}

			if (string.IsNullOrWhiteSpace(operationKey))
			{
				operationKey = string.Format("{0:D}", Guid.NewGuid());
			}

			operationKey = string.Format("{0}:{1}", operationKey, Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens));

			System.Diagnostics.Trace.CorrelationManager.StartLogicalOperation(operationKey);

			// Set thread name to same as operationKey so that log4net will record the id for each log entry
			if (string.IsNullOrWhiteSpace(System.Threading.Thread.CurrentThread.Name))
			{
				System.Threading.Thread.CurrentThread.Name = operationKey;
			}
		}

		public abstract Task Consume(Microsoft.Azure.ServiceBus.Message requestMessage);
	}
}
