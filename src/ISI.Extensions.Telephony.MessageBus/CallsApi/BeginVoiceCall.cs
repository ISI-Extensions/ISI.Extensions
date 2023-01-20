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
using DTOs = ISI.Extensions.Telephony.DataTransferObjects.CallsApi;
using MESSAGEQUEUE = ISI.Extensions.Telephony.MessageBus.Calls;

namespace ISI.Extensions.Telephony.MessageBus
{
	public partial class CallsApi
	{
		public DTOs.BeginVoiceCallResponse BeginVoiceCall(DTOs.BeginVoiceCallRequest request)
		{
			var response = new DTOs.BeginVoiceCallResponse();

			var messageBusRequest = new MESSAGEQUEUE.DataTransferObjects.BeginVoiceCallRequest()
			{
				HandlerKey = request.HandlerKey,
				HandlerJobKey = request.HandlerJobKey,
				CallKey = request.CallKey,
				From = request.From,
				To = request.To,
			};

			var messageBusResponse = MessageBus.PublishAsync<MESSAGEQUEUE.DataTransferObjects.BeginVoiceCallRequest, MESSAGEQUEUE.DataTransferObjects.BeginVoiceCallResponse>(messageBusRequest).GetAwaiter().GetResult();

			response.AccountKey = messageBusResponse.AccountKey;
			response.SessionKey = messageBusResponse.SessionKey;
			response.From = messageBusResponse.From;
			response.To = messageBusResponse.To;
			response.CallStatus = ISI.Extensions.Enum<ISI.Extensions.Telephony.Calls.CallStatus?>.Convert(messageBusResponse.CallStatus);
			response.Direction = ISI.Extensions.Enum<ISI.Extensions.Telephony.Calls.Direction?>.Convert(messageBusResponse.Direction);
			response.Price = messageBusResponse.Price;
			response.CreatedDateTimeUtc = messageBusResponse.CreatedDateTimeUtc;
			response.UpdatedDateTimeUtc = messageBusResponse.UpdatedDateTimeUtc;
			response.Success = messageBusResponse.Success;

			return response;
		}
	}
}