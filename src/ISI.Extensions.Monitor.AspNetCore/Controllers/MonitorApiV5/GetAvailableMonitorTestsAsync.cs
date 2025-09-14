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
		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Get))]
		[Microsoft.AspNetCore.Authorization.AllowAnonymous]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.MonitorApiV5.RouteNames.GetAvailableMonitorTests, typeof(Routes.MonitorApiV5), "get-available-monitor-tests")]
		[Microsoft.AspNetCore.Mvc.ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, Type = typeof(DTOs.GetAvailableMonitorTestsResponse))]
		public async Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAvailableMonitorTestsAsync(System.Threading.CancellationToken cancellationToken = default)
		{
			var response = new DTOs.GetAvailableMonitorTestsResponse();

			var monitorTests = MonitorApi.ListMonitorTests(new ()).MonitorTests;

			response.AvailableMonitorTests = monitorTests.ToNullCheckedArray(monitorTest => new Models.MonitorApiV5.SerializableModels.AvailableMonitorTest()
			{
				Name = monitorTest.Name,
				Url = Url.GenerateRouteUrl(Routes.MonitorApiV5.RouteNames.RunMonitorTest, new { name = monitorTest.Name }),
				Parameters = monitorTest.GetParameterInformations().ToNullCheckedArray(parameter => new Models.MonitorApiV5.SerializableModels.AvailableMonitorTestParameter()
				{
					Name = parameter.Name,
					Type = parameter.Type.Name,
					DefaultValue = parameter.DefaultValue
				})
			});

			return Ok(response);
		}
	}
}