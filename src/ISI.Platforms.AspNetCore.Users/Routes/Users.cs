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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ISI.Extensions.AspNetCore;
using ISI.Extensions.AspNetCore.Extensions;

namespace ISI.Platforms.AspNetCore.Users
{
	public partial class Routes
	{
		public partial class Users : IHasUrlRoute
		{
			string IHasUrlRoute.UrlRoot => UrlRoot;
			
			#pragma warning disable 649
			public class RouteNames : IRouteNames
			{
				[RouteName] public const string Index = "Index-1dfbf177-fce2-460f-b87a-bf6c7fe7f2fb";
				[RouteName] public const string User = "User-339dcbcd-7fa4-4cf1-a261-d5a788da4877";
				[RouteName] public const string UserWithUserUuid = "UserWithUserUuid-1faf37f4-f9f3-49eb-aed1-287cd9534b4c";
				[RouteName] public const string DeactivateUserAuthentication = "DeactivateUserAuthentication-1ec0ee62-8832-4dd0-9d69-75a49bef1d1b";
				[RouteName] public const string AddActiveDirectoryUserAuthentication = "AddActiveDirectoryUserAuthentication-27af9cdf-52ff-43fc-95a9-8becd9fe1ebb";
				[RouteName] public const string AddActiveDirectoryUserAuthenticationWithUserUuid = "AddActiveDirectoryUserAuthenticationWithUserUuid-a78e9536-cbbe-46b5-8f43-62fd5c2b66bf";
				[RouteName] public const string AddUserNamePasswordUserAuthentication = "AddUserNamePasswordUserAuthentication-4ef18554-4288-4a54-9e22-b249ae7500da";
				[RouteName] public const string AddUserNamePasswordUserAuthenticationWithUserUuid = "AddUserNamePasswordUserAuthenticationWithUserUuid-0588ceab-8291-44cb-a300-31bec7063a5d";
				[RouteName] public const string ReactivateUserAuthentication = "ReactivateUserAuthentication-3da5441e-1be3-4b49-a501-e478ec28217f";
				[RouteName] public const string CreateUserFromActiveDirectory = "CreateUserFromActiveDirectory-6f5d4fa2-faad-40cd-ae05-ab844d5b733d";
				[RouteName] public const string CreateUser = "CreateUser-15b0d758-8610-4840-843a-d26038394653";
				[RouteName] public const string GetUsers = "GetUsers-9f229ddc-b636-4f57-84b2-d4f96b67acdc";
				[RouteName] public const string AddApiKey = "AddApiKey-ff34d328-4afb-4895-a8c1-8f2021685333";
				[RouteName] public const string AddApiKeyWithUserUuid = "AddApiKeyWithUserUuid-c641f742-8349-463d-91b3-679405571d37";
				[RouteName] public const string DeactivateApiKey = "DeactivateApiKey-87700f45-845c-4a99-8460-3f4b37ce6b40";
				[RouteName] public const string ReactivateApiKey = "ReactivateApiKey-aa95f38d-e2b7-44c9-ae91-ebdfd16d7b7e";
				[RouteName] public const string RegenerateApiKey = "RegenerateApiKey-13ad5556-ec42-4c55-978a-b5783d03e0c2";
				[RouteName] public const string RegenerateApiKeyWithApiKeyUuid = "RegenerateApiKeyWithApiKeyUuid-411a3ec6-33fe-4d69-9599-ba10e9f8b984";
				//${RouteNames}
			}
#pragma warning restore 649

			internal static readonly string UrlRoot = Routes.UrlRoot + "users/";
		}
	}
}