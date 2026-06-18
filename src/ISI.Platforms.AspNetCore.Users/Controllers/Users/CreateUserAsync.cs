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

namespace ISI.Platforms.AspNetCore.Users.Controllers
{
	public partial class UsersController
	{
		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Get))]
		[UsersAuthorize]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.Users.RouteNames.CreateUser, typeof(Routes.Users), "create-user")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> CreateUserAsync(System.Threading.CancellationToken cancellationToken = default)
		{
			Microsoft.AspNetCore.Mvc.IActionResult result = null;

			var viewModel = new Models.Users.CreateUserModel()
			{
				CreateUser = new Models.Users.EditModels.CreateUserEditModel(),
			};

			return result ?? View(T4Files.Views.Users.CreateUser_cshtml, viewModel);
		}

		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Post))]
		[UsersAuthorize]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.Users.RouteNames.CreateUser, typeof(Routes.Users), "create-user")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> CreateUserSaveAsync(Models.Users.EditModels.CreateUserEditModel createUser, System.Threading.CancellationToken cancellationToken = default)
		{
			Microsoft.AspNetCore.Mvc.IActionResult result = null;

			var viewModel = new Models.Users.CreateUserModel()
			{
				CreateUser = createUser,
			};

			var createUserResponse = await AuthenticationApi.CreateNewUserAsync(new()
			{
				FirstName = createUser.FirstName,
				LastName = createUser.LastName,
				EmailAddress = createUser.EmailAddress,
				RequestedByUserKey = GetUserUuid(),
			}, cancellationToken);

			var user = createUserResponse.User;

			if (user != null)
			{
				result = RedirectToRoute(Routes.Users.RouteNames.UserWithUserUuid, new { userUuid = user.UserUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens) });
			}

			return result ?? View(T4Files.Views.Users.CreateUser_cshtml, viewModel);
		}
	}
}