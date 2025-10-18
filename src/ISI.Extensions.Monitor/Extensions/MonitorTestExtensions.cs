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

namespace ISI.Extensions.Extensions
{
	public static class MonitorTestExtensions
	{
		public static TResponse CreateResponse<TResponse>(this ISI.Extensions.Monitor.IMonitorTest monitorTest)
			where TResponse : ISI.Extensions.Monitor.IMonitorTestResponse, new()
		{
			var response = new TResponse()
			{
				StartupParameterValues = monitorTest.GetModelProperties().ToNullCheckedArray(property =>
				{
					var value = property.GetValue(monitorTest.Model);

					var parameter = new ISI.Extensions.Monitor.MonitorTestResponseStartupParameterValue()
					{
						Name = property.Name,
						Value = (value == null ? null : (property.PropertyType.IsArray ? string.Join(",", ((IEnumerable<object>)value).Select(v => $"{v}")) : $"{value}"))
					};

					return parameter as ISI.Extensions.Monitor.IMonitorTestResponseStartupParameterValue;
				})
			};

			return response;
		}

		public class MonitorTestParameterInformation
		{
			public string Name { get; set; }
			public Type Type { get; set; }
			public string DefaultValue { get; set; }
		}

		public static MonitorTestParameterInformation[] GetParameterInformations(this ISI.Extensions.Monitor.IMonitorTest monitorTest)
		{
			var model = Activator.CreateInstance(monitorTest.ModelType);

			return monitorTest.GetModelProperties().ToNullCheckedArray(property =>
			{
				var defaultValue = property.GetValue(model);

				var parameter = new MonitorTestParameterInformation()
				{
					Name = property.Name,
					Type = property.PropertyType,
					DefaultValue = (defaultValue == null ? null : (property.PropertyType.IsArray ? string.Join(",", ((IEnumerable<object>) defaultValue).Select(v => $"{v}")) : $"{defaultValue}"))
				};

				return parameter;
			});
		}

		public delegate bool ConstructMonitorTestModelTryGetParameterValueDelegate(string key, out string value);
		public static object ConstructMonitorTestModel(this ISI.Extensions.Monitor.IMonitorTest monitorTest, ConstructMonitorTestModelTryGetParameterValueDelegate tryGetParameterValue)
		{
			var model = Activator.CreateInstance(monitorTest.ModelType);

			foreach (var monitorTestModelProperty in monitorTest.GetModelProperties())
			{
				if (tryGetParameterValue(monitorTestModelProperty.Name, out var value))
				{
					monitorTestModelProperty.SetValueFromString(model, value);
				}
			}

			return model;
		}
	}
}
