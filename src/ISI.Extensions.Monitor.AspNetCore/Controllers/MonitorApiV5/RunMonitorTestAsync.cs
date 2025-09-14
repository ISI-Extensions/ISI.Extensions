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
using ISI.Extensions.AspNetCore.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.Monitor.AspNetCore.Models.MonitorApiV5.SerializableModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Monitor.AspNetCore.Controllers
{
	public partial class MonitorApiV5Controller
	{
		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Post))]
		[Microsoft.AspNetCore.Authorization.AllowAnonymous]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.MonitorApiV5.RouteNames.RunMonitorTest, typeof(Routes.MonitorApiV5), "run-monitor-test")]
		[Microsoft.AspNetCore.Mvc.ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, Type = typeof(ISI.Extensions.Monitor.SerializableModels.MonitorTestSerializableResponse))]
		[Microsoft.AspNetCore.Mvc.ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, Type = typeof(DTOs.RunMonitorTestExceptionResponse))]
		[Microsoft.AspNetCore.Mvc.ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, Type = typeof(DTOs.RunMonitorTestNotFoundResponse))]
		public async Task<Microsoft.AspNetCore.Mvc.IActionResult> RunMonitorTestAsync(string name, ISI.Extensions.AspNetCore.AllRequestParameters parameters, string failedHttpStatus, System.Threading.CancellationToken cancellationToken = default)
		{
			var monitorTests = MonitorApi.ListMonitorTests(new()).MonitorTests;

			var monitorTest = monitorTests.FirstOrDefault(monitorTest => string.Equals(monitorTest.Name, name, StringComparison.InvariantCultureIgnoreCase));

			if (monitorTest == null)
			{
				var response = new DTOs.RunMonitorTestNotFoundResponse()
				{
					Passed = false,
					Note = string.Format("Monitor Test: \"{0}\" not found", name)
				};

				return NotFound(response);
			}

			try
			{
				monitorTest.Model = monitorTest.ConstructMonitorTestModel(parameters.TryGetValue);

				var monitorTestResponse = monitorTest.Execute();

				var response = monitorTestResponse.GetSerializableResponse();

				if (response.Passed)
				{
					return Ok(response);
				}

				var content = Content(JsonSerializer.Serialize(response), ISI.Extensions.MimeType.GetMimeType("json"));
				content.StatusCode = failedHttpStatus.ToInt(400);

				return content;
			}
			catch (Exception exception)
			{
				var response = new DTOs.RunMonitorTestExceptionResponse();

				try
				{
					response.StartupParameterValues = monitorTest.GetModelProperties().ToNullCheckedArray(property =>
					{
						object value = property.GetValue(monitorTest.Model);

						var parameter = new ISI.Extensions.Monitor.SerializableModels.MonitorTestSerializableResponseStartupParameterValue()
						{
							Name = property.Name,
							Value = (value == null ? null : (property.PropertyType.IsArray ? string.Join(",", ((IEnumerable<object>)value).Select(v => string.Format("{0}", v))) : string.Format("{0}", value)))
						};

						return parameter;
					});
				}
				catch
				{
				}

				response.Passed = false;

				response.Exception = exception.NullCheckedConvert(Convert);

				return BadRequest(response);
			}

			return BadRequest();
		}
	}
}