using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.VisualStudio
{
	public partial class VisualStudioSettings
	{
		public static readonly string[] DefaultExcludePathFilters = new[]
		{
			".vs",
			".git",
			".svn",
			".nuget",
			"obj",
			"Resources",
			"packages",
			"_ReSharper.Caches",
			"ISI.CMS",
			"ISI.DataBus",
			"ISI.DocumentStorage",
			"ISI.Emails",
			"ISI.Gravity",
			"ISI.Journal",
			"ISI.Libraries",
			"ISI.Licenses",
			"ISI.Monitor",
			"ISI.Monitor.Tester",
			"ISI.Monitor.Watcher",
			"ISI.Scheduler",
			"ISI.Scripts",
			"ISI.SecureDataStore",
			"ISI.Services",
			"ISI.Telephony",
			"ISI.Tracing",
			"ISI.Worker",
			"ISI.Wrapper",
			"ICS.Libraries",
			"ICS.CMS",
			"ICS.Libraries",
			"ICS.Licenses",
			"ICS.Scripts",
			"ICS.Services",
		};

		private static ISI.Extensions.Serialization.ISerialization _serialization = null;
		protected static ISI.Extensions.Serialization.ISerialization Serialization => _serialization ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Serialization.ISerialization>();

		protected string SettingsFileName { get; }

		public VisualStudioSettings()
		{
			var configurationDirectory = System.IO.Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), "ISI.Extensions");

			System.IO.Directory.CreateDirectory(configurationDirectory);

			SettingsFileName = System.IO.Path.Combine(configurationDirectory, "visualStudio.settings.json");
		}
	}
}