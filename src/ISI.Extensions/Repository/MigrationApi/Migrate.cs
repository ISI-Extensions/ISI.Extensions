#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions.Repository
{
	public partial class MigrationApi
	{
		public ISI.Extensions.Repository.DataTransferObjects.MigrationApi.MigrateResponse Migrate(IsItOkToRunMigrationTool isItOkToRunMigrationTool = null)
		{
			var response = new ISI.Extensions.Repository.DataTransferObjects.MigrationApi.MigrateResponse();

			isItOkToRunMigrationTool ??= _ => true;

			var migrationStepTypes = ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementationTypes<IMigrationStep>().ToArray();

			var migrationSteps = new Dictionary<int, Type>();

			foreach (var migrationStepType in migrationStepTypes)
			{
				var migrationStepNumber = migrationStepType.Name.Split(new[] { '_' })[1].ToInt();

				if (migrationSteps.TryGetValue(migrationStepNumber, out var duplicateMigrationStepType))
				{
					throw new Exception(string.Format("Both Steps: \"{0}\" and \"{1}\" have the same StepNumber: {2}", duplicateMigrationStepType.Name, migrationStepType.Name, migrationStepNumber));
				}

				migrationSteps.Add(migrationStepNumber, migrationStepType);
			}

			if (migrationSteps.Any())
			{
				var defaultRepositorySetupApi = RepositorySetupApiFactory.GetRepositorySetupApi(migrationSteps.First().Value);
				var lastStepId = defaultRepositorySetupApi.GetLatestStep().StepId;

				if (migrationSteps.Any(migrationStep => migrationStep.Key > lastStepId) && isItOkToRunMigrationTool(defaultRepositorySetupApi))
				{
					foreach (var migrationStep in migrationSteps.Where(migrationStep => migrationStep.Key > lastStepId).OrderBy(migrationStep => migrationStep.Key))
					{
						if (!(ServiceProvider.GetService(migrationStep.Value) is IMigrationStep migrationStepInstance))
						{
							throw new Exception("Can't create step");
						}

						Console.WriteLine("{1} => Started {0}", migrationStep.Value.Name, DateTime.Now.Formatted(DateTimeExtensions.DateTimeFormat.DateTime));

						migrationStepInstance.Execute(RepositorySetupApiFactory.GetRepositorySetupApi(migrationStep.Value));

						defaultRepositorySetupApi.SetStep(migrationStep.Key);

						Console.WriteLine("{1} => Finished {0}", migrationStep.Value.Name, DateTime.Now.Formatted(DateTimeExtensions.DateTimeFormat.DateTime));
					}
				}
			}

			return response;
		}
	}
}
