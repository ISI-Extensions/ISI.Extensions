using System;
using System.Collections.Generic;
using System.Text;
using DTOs = ISI.Extensions.Security.DataTransferObjects.SecurityApi;

namespace ISI.Extensions.Security
{
	public interface ISecurityApi
	{
		DTOs.GetCurrentDomainNameResponse GetCurrentDomainName(DTOs.GetCurrentDomainNameRequest request);
		DTOs.AuthenticateUserResponse AuthenticateUser(DTOs.AuthenticateUserRequest request);
		DTOs.ListUsersResponse ListUsers(DTOs.ListUsersRequest request);
		DTOs.ListRolesResponse ListRoles(DTOs.ListRolesRequest request);
		DTOs.GetUsersResponse GetUsers(DTOs.GetUsersRequest request);
	}
}
