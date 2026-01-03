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
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions.Emails.EmailMessageGenerator
{
	public class EmailMessageGenerator : IEmailMessageGenerator
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public EmailMessageGenerator(
			Microsoft.Extensions.Logging.ILogger logger)
		{
			Logger = logger;
		}

		#region ContentGenerators
		private static object _contentGeneratorsLock = new();
		private static Dictionary<Type, Type> _contentGenerators = null;

		private Dictionary<Type, Type> ContentGenerators
		{
			get
			{
				if (_contentGenerators == null)
				{
					lock (_contentGeneratorsLock)
					{
						if (_contentGenerators == null)
						{
							var __contentGenerators = new Dictionary<Type, Type>();

							var genericContentGeneratorInterfaceType = typeof(IEmailMessageContentGenerator<>);

							var localContainer = ISI.Extensions.TypeLocator.Container.LocalContainer;

							var contentGeneratorTypes = localContainer.GetImplementationTypes<IMessageMessageContentGenerator>();

							foreach (var contentGeneratorType in contentGeneratorTypes)
							{
								Type modelType = null;

								var genericContentGeneratorType = contentGeneratorType.GetInterfaces().FirstOrDefault(type => type.IsGenericType && (type.GetGenericTypeDefinition() == genericContentGeneratorInterfaceType));

								if (genericContentGeneratorType != null)
								{
									modelType = genericContentGeneratorType.GetGenericArguments().FirstOrDefault();
								}

								if (modelType == null)
								{
									var contentGenerator = ISI.Extensions.ServiceLocator.Current.GetService(contentGeneratorType, () => new ISI.Extensions.DependencyInjection.RegistrationDeclarationByMapToType()
									{
										MapToType = contentGeneratorType,
										ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton,
									}) as IMessageMessageContentGenerator;

									modelType = contentGenerator.ModelType;
								}

								__contentGenerators.Add(modelType, contentGeneratorType);
							}

							_contentGenerators = __contentGenerators;
						}
					}
				}

				return _contentGenerators;
			}
		}
		#endregion

		protected virtual async Task<IEmailMessageContentGenerator<TModel>> GetContentGeneratorAsync<TModel>(IEmailMessageContentGeneratorModel model, System.Threading.CancellationToken cancellationToken = default)
			where TModel : class, IEmailMessageContentGeneratorModel
		{
			var modelType = typeof(TModel);

			if (modelType.IsInterface)
			{
				modelType = model.GetType();
			}

			if (!ContentGenerators.ContainsKey(modelType))
			{
				throw new EmailMessageGeneratorNotFoundException($"Cannot find Content Generator for \"{modelType.FullName}\"\nRegistered Content Generators:\n{string.Join("\n", ContentGenerators.Select(cg => cg.Key.AssemblyQualifiedNameWithoutVersion()))}");
			}

			var contentGeneratorType = ContentGenerators[modelType];

			if (contentGeneratorType == null)
			{
				throw new EmailMessageGeneratorNotFoundException($"Cannot find Content Generator for \"{modelType.FullName}\"\nRegistered Content Generators:\n{string.Join("\n", ContentGenerators.Select(cg => cg.Key.AssemblyQualifiedNameWithoutVersion()))}");
			}

			if (!(ISI.Extensions.ServiceLocator.Current?.GetService(contentGeneratorType, () => new ISI.Extensions.DependencyInjection.RegistrationDeclarationByMapToType()
			{
				MapToType = contentGeneratorType,
				ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton,
			}) is IEmailMessageContentGenerator<TModel> contentGenerator))
			{
				throw new EmailMessageGeneratorCannotBeCreatedException($"Cannot not create instance of \"{contentGeneratorType.FullName}\" (111)");
			}

			return contentGenerator;
		}

		public virtual async Task<ISI.Extensions.Emails.IEmailMailMessage> GenerateEmailMessageAsync<TModel>(TModel model, System.Threading.CancellationToken cancellationToken = default)
			where TModel : class, IEmailMessageContentGeneratorModel
		{
			return await GenerateEmailMessageAsync<ISI.Extensions.Emails.EmailMailMessage, TModel>(model, cancellationToken);
		}

		public virtual async Task<TResult> GenerateEmailMessageAsync<TResult, TModel>(TModel model, System.Threading.CancellationToken cancellationToken = default)
			where TModel : class, IEmailMessageContentGeneratorModel
			where TResult : ISI.Extensions.Emails.IEmailMailMessage, new()
		{
			return await GenerateEmailMessageAsync<TResult, TModel>(model, new(), cancellationToken);
		}

		public virtual async Task<TResult> GenerateEmailMessageAsync<TResult, TModel>(TModel model, TResult instance, System.Threading.CancellationToken cancellationToken = default)
			where TModel : class, IEmailMessageContentGeneratorModel
			where TResult : ISI.Extensions.Emails.IEmailMailMessage
		{
			var emailMailMessage = default(TResult);

			var contentGenerator = (await GetContentGeneratorAsync<TModel>(model, cancellationToken));

			if (!(model is ISI.Extensions.Culture.IHasCultureKey cultureModel))
			{
				emailMailMessage = (await contentGenerator.GenerateEmailMessageAsync(model, instance, cancellationToken));
			}
			else
			{
				using (new ISI.Extensions.Culture.CultureBubble(cultureModel))
				{
					if (string.IsNullOrEmpty(cultureModel.CultureKey))
					{
						cultureModel.CultureKey = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower();
					}

					emailMailMessage = (await contentGenerator.GenerateEmailMessageAsync(model, instance, cancellationToken));
				}
			}

			return emailMailMessage;
		}
	}
}
