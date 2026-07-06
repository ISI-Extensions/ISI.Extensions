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
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Shared;

namespace ISI.Extensions.AspNetCore
{
	public interface ITypedAuthorizeAttributeGetPolicy
	{
		string Policy { get; }
	}

	public interface ITypedAuthorizeAttributeGetRoles
	{
		string Roles { get; }
	}

	public interface ITypedAuthorizeAttributeGetAuthenticationSchemes
	{
		string AuthenticationSchemes { get; }
	}


	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class TypedAuthorizeAttribute : Attribute, Microsoft.AspNetCore.Authorization.IAuthorizeData
	{
		protected Type GetPolicyType { get; set; }
		protected ITypedAuthorizeAttributeGetPolicy GetPolicy { get; set; } = null;
		
		protected Type GetRolesType { get; set; }
		protected ITypedAuthorizeAttributeGetRoles GetRoles { get; set; } = null;

		protected Type GetAuthenticationSchemesType { get; set; }
		protected ITypedAuthorizeAttributeGetAuthenticationSchemes GetAuthenticationSchemes { get; set; } = null;

		public TypedAuthorizeAttribute(Type getPolicyType, Type getRolesType = null, Type getAuthenticationSchemesType = null)
		{
			GetPolicyType = getPolicyType;
			GetRolesType = getRolesType;
			GetAuthenticationSchemesType = getAuthenticationSchemesType;
		}

		private string _policy = null;
		public string Policy
		{
			get
			{
				if (_policy == null)
				{
					if (GetPolicyType == null)
					{
						return null;
					}

					GetPolicy = ISI.Extensions.ServiceLocator.Current.GetService(GetPolicyType) as ITypedAuthorizeAttributeGetPolicy;

					_policy = GetPolicy?.Policy;
				}

				return _policy;
			}
			set => _policy = value;
		}

		private string _roles = null;
		public string Roles
		{
			get
			{
				if (_roles == null)
				{
					if (GetRolesType == null)
					{
						return null;
					}

					GetRoles = ISI.Extensions.ServiceLocator.Current.GetService(GetRolesType) as ITypedAuthorizeAttributeGetRoles;

					_roles = GetRoles?.Roles;
				}

				return _roles;
			}
			set => _roles = value;
		}

		private string _authenticationSchemes = null;
		public string AuthenticationSchemes
		{
			get
			{
				if (_authenticationSchemes == null)
				{
					if (GetAuthenticationSchemesType == null)
					{
						return null;
					}

					GetAuthenticationSchemes = ISI.Extensions.ServiceLocator.Current.GetService(GetAuthenticationSchemesType) as ITypedAuthorizeAttributeGetAuthenticationSchemes;

					_authenticationSchemes = GetAuthenticationSchemes?.AuthenticationSchemes;
				}

				return _authenticationSchemes;
			}
			set => _authenticationSchemes = value;
		}
	}
}
