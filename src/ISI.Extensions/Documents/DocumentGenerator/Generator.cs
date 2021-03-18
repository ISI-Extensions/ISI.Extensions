#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using ISI.Extensions.Extensions;
using ISI.Extensions.TypeLocator.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;

namespace ISI.Extensions.Documents.DocumentGenerator
{
	public class Generator : IGenerator
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public Generator(
			Microsoft.Extensions.Logging.ILogger logger)
		{
			Logger = logger;
		}

		#region ContentGenerators
		private static object _contentGeneratorsLock = new object();
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

							var genericContentGeneratorInterfaceType = typeof(IContentGenerator<>);

							var contentGeneratorTypes = ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementationTypes<IContentGenerator>();

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
									}) as IContentGenerator;

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

		protected virtual IContentGenerator<TModel> GetContentGenerator<TModel>(IModel model)
			where TModel : class, IModel
		{
			var modelType = typeof(TModel);

			if (modelType.IsInterface)
			{
				if (model == null)
				{
					throw new Exception("Cannot determine modelType, thus cannot determine which Content Generator to use");
				}

				modelType = model.GetType();
			}

			if (!ContentGenerators.TryGetValue(modelType, out var contentGeneratorType))
			{
				throw new GeneratorNotFoundException(string.Format("Cannot find Content Generator for \"{0}\"\nRegistered Content Generators:\n{1}", modelType.FullName, string.Join(Environment.NewLine, ContentGenerators.Select(cg => cg.Key.AssemblyQualifiedNameWithoutVersion()))));
			}

			if (!(ISI.Extensions.ServiceLocator.Current.GetService(contentGeneratorType, () => new ISI.Extensions.DependencyInjection.RegistrationDeclarationByMapToType()
			{
				MapToType = contentGeneratorType,
				ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton,
			}) is IContentGenerator<TModel> contentGenerator))
			{
				throw new GeneratorCannotBeCreatedException(string.Format("Cannot not create instance of \"{0}\"", contentGeneratorType.FullName));
			}

			return contentGenerator;
		}

		public virtual void GenerateDocument<TModel>(TModel model, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			where TModel : class, IModel
		{
			var contentGenerator = GetContentGenerator<TModel>(model);

			contentGenerator.GenerateDocument(model, documentProperties, printerName, documentStream, fileFormat);
		}

		public virtual void GenerateDocument<TModel>(IEnumerable<TModel> models, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			where TModel : class, IModel
		{
			var contentGenerator = GetContentGenerator<TModel>(null);

			contentGenerator.GenerateDocument(models, documentProperties, printerName, documentStream, fileFormat);
		}

		public virtual void GenerateDocument<TModel>(System.IO.Stream templateStream, TModel model, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			where TModel : class, IModel
		{
			var contentGenerator = GetContentGenerator<TModel>(model);

			contentGenerator.GenerateDocument(templateStream, model, documentProperties, printerName, documentStream, fileFormat);
		}

		public virtual void GenerateDocument<TModel>(System.IO.Stream templateStream, IEnumerable<TModel> models, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			where TModel : class, IModel
		{
			var contentGenerator = GetContentGenerator<TModel>(null);

			contentGenerator.GenerateDocument(templateStream, models, documentProperties, printerName, documentStream, fileFormat);
		}
	}
}
