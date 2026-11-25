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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonJwt.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SerializableDTOs = ISI.Services.StatusTracker.SerializableModels.MessageBus.StatusTrackerApiV1;

namespace ISI.Services.StatusTracker.SerializableModels.MessageBus.StatusTrackerApiV1
{
	[DataContract]
	public class GetStatusTrackerSnapshotRequest
	{
		[DataMember(Name = "statusTrackerKey", EmitDefaultValue = false)]
		public string StatusTrackerKey { get; set; }
	}

	[DataContract]
	public class GetStatusTrackerSnapshotResponse
	{
		[DataMember(Name = "statusTrackerSnapshot", EmitDefaultValue = false)]
		public StatusTrackerSnapshot StatusTrackerSnapshot { get; set; }
	}

	[DataContract]
	public class StatusTrackerSnapshot
	{
		[DataMember(Name = "caption", EmitDefaultValue = false)]
		public string Caption { get; set; }

		[DataMember(Name = "percent", EmitDefaultValue = false)]
		public int Percent { get; set; }

		[DataMember(Name = "logEntries", EmitDefaultValue = false)]
		public StatusTrackerLogEntry[] LogEntries { get; set; }
	}

	[DataContract]
	public class StatusTrackerLogEntry
	{
		[DataMember(Name = "dateTimeStamp", EmitDefaultValue = false)]
		public string __DateTimeStamp { get => DateTimeStamp.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise); set => DateTimeStamp = value.ToDateTimeUtc(); }
		[IgnoreDataMember]
		public DateTime DateTimeStamp { get; set; }

		[DataMember(Name = "logEntryLevelUuid", EmitDefaultValue = false)]
		public Guid __LogEntryLevel { get => LogEntryLevel.GetUuid(); set => LogEntryLevel = ISI.Extensions.Enum<ISI.Extensions.StatusTrackers.LogEntryLevel>.ParseUuid(__LogEntryLevel); }
		[IgnoreDataMember]
		public ISI.Extensions.StatusTrackers.LogEntryLevel LogEntryLevel { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }
	}
}

namespace ISI.Extensions.Tests.MessageBus
{
	public partial class MassTransit_Tests
	{
		[Test]
		public void GetStatusTrackerSnapshot_Test()
		{
			var configuration = ServiceProvider.GetRequiredService<ISI.Extensions.MessageBus.Configuration>();
			configuration.ConnectionString = "rabbitmq://wrsrabbitmqscm01.westriversystems.com";
			configuration.DefaultChannel.ChannelPath = "ISI.Extensions.Tests";

			MessageBus.Build(ServiceProvider);

			MessageBus.StartAsync().GetAwaiter().GetResult();

			var serviceRequest = new SerializableDTOs.GetStatusTrackerSnapshotRequest()
			{
				StatusTrackerKey = "01ccb869-7330-4be3-b33a-88a06119a045",
			};

			var cancellationTokenSource = new System.Threading.CancellationTokenSource();

			var headers = new ISI.Extensions.MessageBus.MessageBusMessageHeaderCollection();

			headers.AddBearerAuthentication("e6ba6c61-21ae-46ad-9ad8-466608f7f9763855f9fa-9032-4ac6-9c96-d2779c596ba131d22b09-5886-41e3-86e2-5cb53872af8d");

			var serviceResponse = MessageBus.PublishAsync<SerializableDTOs.GetStatusTrackerSnapshotRequest, SerializableDTOs.GetStatusTrackerSnapshotResponse>(serviceRequest, headers, cancellationToken: cancellationTokenSource.Token).GetAwaiter().GetResult();

			MessageBus.StopAsync(cancellationTokenSource.Token).GetAwaiter().GetResult();
		}
	}
}