#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using System.Runtime.Serialization;

namespace ISI.Extensions.Slack.SerializableModels
{
	[ISI.Extensions.Serialization.SerializerObjectType("message")]
	[DataContract]
	public class WebHookEventMessage : IWebHookEvent
	{
		[DataMember(Name = "client_msg_id", EmitDefaultValue = false)]
		public string ClientMsgId { get; set; }

		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "text", EmitDefaultValue = false)]
		public string Text { get; set; }

		[DataMember(Name = "user", EmitDefaultValue = false)]
		public string User { get; set; }

		[DataMember(Name = "ts", EmitDefaultValue = false)]
		public string Ts { get; set; }

		[DataMember(Name = "team", EmitDefaultValue = false)]
		public string Team { get; set; }

		[DataMember(Name = "blocks", EmitDefaultValue = false)]
		public WebHookEventMessageBlock[] Blocks { get; set; }

		[DataMember(Name = "channel", EmitDefaultValue = false)]
		public string Channel { get; set; }

		[DataMember(Name = "event_ts", EmitDefaultValue = false)]
		public string EventTimeStamp { get; set; }

		[DataMember(Name = "channel_type", EmitDefaultValue = false)]
		public string ChannelType { get; set; }
	}

	[DataContract]
	public class WebHookEventMessageBlock
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "block_id", EmitDefaultValue = false)]
		public string BlockId { get; set; }

		[DataMember(Name = "elements", EmitDefaultValue = false)]
		public WebHookEventMessageElement[] Elements { get; set; }
	}

	[DataContract]
	public class WebHookEventMessageElement
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "elements", EmitDefaultValue = false)]
		public WebHookEventMessageElement1[] Elements { get; set; }
	}

	[DataContract]
	public class WebHookEventMessageElement1
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "text", EmitDefaultValue = false)]
		public string Text { get; set; }
	}

	[DataContract]
	public class WebHookEventAuthorization
	{
		[DataMember(Name = "enterprise_id", EmitDefaultValue = false)]
		public object EnterpriseId { get; set; }

		[DataMember(Name = "team_id", EmitDefaultValue = false)]
		public string TeamId { get; set; }

		[DataMember(Name = "user_id", EmitDefaultValue = false)]
		public string UserId { get; set; }

		[DataMember(Name = "is_bot", EmitDefaultValue = false)]
		public bool IsBot { get; set; }

		[DataMember(Name = "is_enterprise_install", EmitDefaultValue = false)]
		public bool IsEnterpriseInstall { get; set; }
	}
}