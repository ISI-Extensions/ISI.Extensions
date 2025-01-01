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
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions.AspNetCore.DataAnnotations
{
	public class DataAnnotationsValidationRules
	{
		private static readonly Dictionary<string, IDataAnnotationsValidationRule> ValidationRules = new();

		static DataAnnotationsValidationRules()
		{
			try
			{
				var localContainer = TypeLocator.Container.LocalContainer;

				var dataAnnotationsValidationRules = localContainer.GetImplementations<IDataAnnotationsValidationRule>();

				foreach (var dataAnnotationsValidationRule in dataAnnotationsValidationRules)
				{
					var name = dataAnnotationsValidationRule.GetType().Name;

					if (!ValidationRules.ContainsKey(name))
					{
						ValidationRules.Add(name, dataAnnotationsValidationRule);
					}
				}
			}
			catch (TypeInitializationException exception)
			{
				var message = new StringBuilder();

				if (exception.InnerException is System.Reflection.ReflectionTypeLoadException reflectionTypeLoadException)
				{
					foreach (var loaderException in reflectionTypeLoadException.LoaderExceptions)
					{
						message.AppendFormat("TypeLoader: {1}{0}", Environment.NewLine, loaderException.Message);
					}

					throw new Exception(message.ToString(), exception);
				}

				throw;
			}
			catch (System.Reflection.ReflectionTypeLoadException exception)
			{
				var message = new StringBuilder();

				foreach (var loaderException in exception.LoaderExceptions)
				{
					message.AppendFormat("TypeLoader: {1}{0}", Environment.NewLine, loaderException.Message);
				}

				throw new Exception(message.ToString(), exception);
			}
		}

		public static string JavaScriptUnobtrusiveValidationRules()
		{
			var result = new System.Text.StringBuilder();

			foreach (var validationRule in ValidationRules)
			{
				result.AppendLine(validationRule.Value.GetJavaScriptUnobtrusiveValidationRule());
			}

			return result.ToString();
		}
	}
}
