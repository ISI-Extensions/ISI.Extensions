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
using ISI.Extensions.AspNetCore;
using ISI.Extensions.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;

namespace ISI.Platforms.ServiceApplication.Test
{
	public partial class Routes
	{
		public partial class Public : IHasUrlRoute
		{
			string IHasUrlRoute.UrlRoot => UrlRoot;

#pragma warning disable 649
			public class RouteNames : IRouteNames
			{
				[RouteName] public const string Index = "Index-74c48f8e-220d-4564-8fdc-83aff1c48d69";
				[RouteName] public const string Login = "Login-f6b5ca63-51d5-4323-ba3c-b831c653e3a1";
				[RouteName] public const string AuthorizationTest = "AuthorizationTest-39a851c1-3dbb-4b4e-94b1-d5e5fa17fd2a";
				//${RouteNames}
			}
#pragma warning restore 649

			internal static readonly string UrlRoot = Routes.UrlRoot;
		}
	}
}