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
using DTOs = ISI.Extensions.Security.DataTransferObjects.AuthenticationApi;

namespace ISI.Extensions.Security
{
	public interface IAuthenticationApi
	{
		Task<DTOs.ListUserAuthenticationTypesResponse> ListUserAuthenticationTypesAsync(DTOs.ListUserAuthenticationTypesRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.CreateUserAuthenticationResponse> CreateUserAuthenticationAsync(DTOs.CreateUserAuthenticationRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.SetUserAuthenticationResponse> SetUserAuthenticationAsync(DTOs.SetUserAuthenticationRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.ReactivateUserAuthenticationResponse> ReactivateUserAuthenticationAsync(DTOs.ReactivateUserAuthenticationRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.AddUserAuthenticationResponse> AddUserAuthenticationAsync(DTOs.AddUserAuthenticationRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.DeactivateUserAuthenticationResponse> DeactivateUserAuthenticationAsync(DTOs.DeactivateUserAuthenticationRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.FindUserAuthenticationsByUserUuidsResponse> FindUserAuthenticationsByUserUuidsAsync(DTOs.FindUserAuthenticationsByUserUuidsRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.FindUserAuthenticationsByUserAuthenticationUuidsResponse> FindUserAuthenticationsByUserAuthenticationUuidsAsync(DTOs.FindUserAuthenticationsByUserAuthenticationUuidsRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.FindUserAuthenticationsByUserAuthenticationRecoveryUuidsResponse> FindUserAuthenticationsByUserAuthenticationRecoveryUuidsAsync(DTOs.FindUserAuthenticationsByUserAuthenticationRecoveryUuidsRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.SetUserAuthenticationPasswordResponse> SetUserAuthenticationPasswordByUserAuthenticationRecoveryUuidAsync(DTOs.SetUserAuthenticationPasswordByUserAuthenticationRecoveryUuidRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.SetUserAuthenticationPasswordResponse> SetUserAuthenticationPasswordByUserAuthenticationUuidAsync(DTOs.SetUserAuthenticationPasswordByUserAuthenticationUuidRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.SetUserAuthenticationPasswordResponse> SetUserAuthenticationPasswordByUserAuthenticationTypeUuidUserUuidAsync(DTOs.SetUserAuthenticationPasswordByUserAuthenticationTypeUuidUserUuidRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.ForgotMyPasswordResponse> ForgotMyPasswordAsync(DTOs.ForgotMyPasswordRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.ValidateAuthenticationResponse> ValidateAuthenticationAsync(DTOs.ValidateAuthenticationRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.FindUserAuthenticationsByUserNamesResponse> FindUserAuthenticationsByUserNamesAsync(DTOs.FindUserAuthenticationsByUserNamesRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.CreateUserAuthenticationRecoveryResponse> CreateUserAuthenticationRecoveryAsync(DTOs.CreateUserAuthenticationRecoveryRequest request, System.Threading.CancellationToken cancellationToken = default);
	}

	public interface IAuthenticationApiWithUser : IAuthenticationApi
	{
		Task<DTOs.FindUserAuthenticationsResponse> FindUserAuthenticationsAsync(DTOs.FindUserAuthenticationsRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.AuthenticateUserNamePasswordResponse> AuthenticateUserNamePasswordAsync(DTOs.AuthenticateUserNamePasswordRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.ValidateApiKeyResponse> ValidateApiKeyAsync(DTOs.ValidateApiKeyRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.SetUsersResponse> SetUsersAsync(DTOs.SetUsersRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.CreateNewUserResponse> CreateNewUserAsync(DTOs.CreateNewUserRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.CreateUserFromActiveDirectoryResponse> CreateUserFromActiveDirectoryAsync(DTOs.CreateUserFromActiveDirectoryRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.CreateNewApiKeyResponse> CreateNewApiKeyAsync(DTOs.CreateNewApiKeyRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.SetApiKeysResponse> SetApiKeysAsync(DTOs.SetApiKeysRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.ListUsersResponse> ListUsersAsync(DTOs.ListUsersRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.GetUsersResponse> GetUsersAsync(DTOs.GetUsersRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.GetApiKeysResponse> GetApiKeysAsync(DTOs.GetApiKeysRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.FindApiKeysByUserUuidResponse> FindApiKeysByUserUuidAsync(DTOs.FindApiKeysByUserUuidRequest request, System.Threading.CancellationToken cancellationToken = default);
		Task<DTOs.RegenerateApiKeyResponse> RegenerateApiKeyAsync(DTOs.RegenerateApiKeyRequest request, System.Threading.CancellationToken cancellationToken = default);
	}
}
