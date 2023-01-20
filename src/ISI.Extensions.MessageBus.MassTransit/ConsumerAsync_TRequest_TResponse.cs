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
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using MassTransit;

namespace ISI.Extensions.MessageBus.MassTransit
{
	public class ConsumerAsync<TRequest, TResponse> : AbstractConsumer<TRequest>
		where TRequest : class
		where TResponse : class
	{
		private readonly Func<TRequest, Task<TResponse>> _processor;
		private readonly ISI.Extensions.MessageBus.OnError<TRequest, TResponse> _onError;

		public ConsumerAsync(Func<TRequest, Task<TResponse>> processor, ISI.Extensions.MessageBus.OnError<TRequest, TResponse> onError = null)
		{
			_processor = processor;
			_onError = onError;
		}

		public override async Task Consume(global::MassTransit.ConsumeContext<TRequest> context)
		{
			TResponse response = null;
			try
			{
				SetTrackingKeys(context);

				BeginRequest();

				response = await _processor(context.Message);

				if ((context.Message is ISI.Extensions.MessageBus.ICorrelatedBy<Guid> requestCorrelatedByGuid) && (response is ISI.Extensions.MessageBus.ICorrelatedBy<Guid> responseCorrelatedByGuid))
				{
					responseCorrelatedByGuid.CorrelationId = requestCorrelatedByGuid.CorrelationId;
				}
				else if ((context.Message is ISI.Extensions.MessageBus.ICorrelatedBy<int> requestCorrelatedByInt) && (response is ISI.Extensions.MessageBus.ICorrelatedBy<int> responseCorrelatedByInt))
				{
					responseCorrelatedByInt.CorrelationId = requestCorrelatedByInt.CorrelationId;
				}
				else if ((context.Message is ISI.Extensions.MessageBus.ICorrelatedBy<long> requestCorrelatedByLong) && (response is ISI.Extensions.MessageBus.ICorrelatedBy<long> responseCorrelatedByLong))
				{
					responseCorrelatedByLong.CorrelationId = requestCorrelatedByLong.CorrelationId;
				}

				await context.RespondAsync<TResponse>(response);

				EndRequest();
			}
			catch (Exception exception)
			{
				var serializedRequest = context.ReceiveContext.GetBodyStream().TextReadToEnd();

				if (_onError == null)
				{
					var message = string.Format("{0}\nSerializedRequest:\n{1}", exception.Message, serializedRequest);

					exception = new ISI.Extensions.MessageBus.MessageBusException(message, exception);

					System.Diagnostics.Trace.TraceError(exception.ErrorMessageFormatted());

					throw exception;
				}

				var onErrorResponse = _onError(serializedRequest, context.Message, response, exception);

				//if (!ISI.Extensions.MessageBus.Configuration.Current.TrapHandlerException && (onErrorResponse == null) || !onErrorResponse.Handled)
				//{
				//	throw;
				//}

				throw;
			}
		}
	}
}
