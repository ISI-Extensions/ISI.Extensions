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
using System.Runtime.Serialization;
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.JsonJwt.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Runtime.Serialization;

namespace ISI.Extensions.Tests.MessageBus
{
	public partial class MassTransit_Tests
	{
		[DataContract]
		public class GetInterfaceRequest
		{
			[DataMember(Name = "interface", EmitDefaultValue = false)]
			public IInterface Interface { get; set; }
		}

		[DataContract]
		public class GetInterfaceResponse
		{
			[DataMember(Name = "interface", EmitDefaultValue = false)]
			public IInterface Interface { get; set; }
		}

		public interface IInterface
		{
			
		}
		
		[DataContract]
		[ISI.Extensions.Serialization.SerializerContractUuid("6cdb12fb-6963-46a4-bd70-3a075f0311cb")]
		public class InterfaceA : IInterface
		{
			[DataMember(Name = "key", EmitDefaultValue = false)]
			public string Key { get; set; }

			[DataMember(Name = "value", EmitDefaultValue = false)]
			public string Value { get; set; }
		}
		
		[Test]
		public void Interface_Test()
		{
			var configuration = ServiceProvider.GetRequiredService<ISI.Extensions.MessageBus.Configuration>();
			configuration.ConnectionString = "rabbitmq://localhost";
			configuration.DefaultChannel.ChannelPath = "ISI.Extensions.Tests";

			MessageBus.Build(ServiceProvider, AddSubscriptions);

			void AddSubscriptions(ISI.Extensions.MessageBus.IMessageBusConfigurator messageBusConfigurator)
			{
				messageBusConfigurator.Subscribe<GetInterfaceRequest, GetInterfaceResponse>(async (request, token) =>
				{
					var response = new GetInterfaceResponse();

					Console.WriteLine(request.GetType().FullName);
					Console.WriteLine(request.Interface.GetType().FullName);
					response.Interface = request.Interface;
					Console.WriteLine(response.GetType().FullName);
					Console.WriteLine(response.Interface.GetType().FullName);

					return response;
				});
			}

			MessageBus.StartAsync().GetAwaiter().GetResult();

			var request = new GetInterfaceRequest()
			{
				Interface = new InterfaceA()
				{
					Key = "A",
					Value = "VA",
				},
			};

			Console.WriteLine(request.GetType().FullName);
			Console.WriteLine(request.Interface.GetType().FullName);
			Console.WriteLine();

			var cancellationTokenSource = new System.Threading.CancellationTokenSource();

			try
			{
				var response = MessageBus.PublishAsync<GetInterfaceRequest, GetInterfaceResponse>(request, timeout: TimeSpan.FromSeconds(5), cancellationToken: cancellationTokenSource.Token).GetAwaiter().GetResult();
			
				Console.WriteLine();
				Console.WriteLine(response.GetType().FullName);
				Console.WriteLine(response.Interface.GetType().FullName);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
				//throw;
			}


			MessageBus.StopAsync().GetAwaiter().GetResult();
		}
	}
}