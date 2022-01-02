#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
using System.Text;
using System.Runtime.Serialization;
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class JsonSerialization_Tests
	{
		public interface IRequestStep
		{

		}

		[DataContract]
		[ISI.Extensions.Serialization.SerializerContractUuid("b7edf765-58f0-4fb5-ab5a-8b87d48d1515")]
		public class RequestStepA : IRequestStep
		{
			[DataMember(Name = "stepName", EmitDefaultValue = false)]
			public string StepName { get; set; }

			[DataMember(Name = "instructionA", EmitDefaultValue = false)]
			public string InstructionA { get; set; }
		}

		[DataContract]
		[ISI.Extensions.Serialization.SerializerContractUuid("c125d01a-f070-415a-bb7b-06bb01fe6084")]
		public class RequestStepB : IRequestStep
		{
			[DataMember(Name = "stepName", EmitDefaultValue = false)]
			public string StepName { get; set; }

			[DataMember(Name = "instructionB", EmitDefaultValue = false)]
			public string InstructionB { get; set; }
		}

		[DataContract]
		public class Request
		{
			[DataMember(Name = "steps", EmitDefaultValue = false)]
			public IRequestStep[] Steps { get; set; }
		}

		[Test]
		public void JsonSerialization_Test()
		{
			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
			var configuration = configurationBuilder.Build();

			var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
				.AddOptions()
				.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configuration);

			services.AddAllConfigurations(configuration);

			var serviceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configuration);

			var jsonSerializer = new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer();

			var request = new Request()
			{
				Steps = new IRequestStep[]
				{
					new RequestStepA()
					{
						StepName = "A",
						InstructionA = "Goto B",
					}, 
					new RequestStepB()
					{
						StepName = "B",
						InstructionB = "Goto A",
					}, 
				}
			};


			var serializedRequest = jsonSerializer.Serialize(typeof(Request), request, true);

			var deserializedRequest = jsonSerializer.Deserialize(typeof(Request), serializedRequest);
		}
	}
}
