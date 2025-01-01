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
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace ISI.Platforms.AspNetCore.Extensions
{
	public static class WebApplicationExtensions
	{
		public static void ConfigureWebApplication(this Microsoft.AspNetCore.Builder.WebApplication webApplication, ServiceApplicationContext context)
		{
			webApplication.UseDefaultFiles();

			//var rootDirectory = ISI.Extensions.IO.Path.GetRootBinDirectory(Startup.Context.RootAssembly);
			
		
			//if (string.Equals(System.IO.Path.GetFileName(rootDirectory), "bin", StringComparison.InvariantCultureIgnoreCase))
			//{
			//	rootDirectory = System.IO.Path.GetDirectoryName(rootDirectory);
			//}
			//var wwwroot = System.IO.Path.Combine(rootDirectory, "wwwroot");

			//webApplication.UseStaticFiles(new Microsoft.AspNetCore.Builder.StaticFileOptions()
			//{
			//	FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(wwwroot)
			//});

			webApplication.UseStaticFiles();

			webApplication.UseRouting();

			webApplication.UseAuthentication();
			webApplication.UseAuthorization();


			if (webApplication.Environment.IsDevelopment())
			{
				webApplication.UseDeveloperExceptionPage();
			}

			webApplication.UseEndpoints(endpointRouteBuilder =>
			{
				endpointRouteBuilder.MapControllers();
				context.WebStartupUseEndpoints?.Invoke(endpointRouteBuilder);
			});

			

			context.ConfigureWebApplication?.Invoke(webApplication);
		}
	}
}
