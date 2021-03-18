<#+
		readonly string Namespace = "$rootnamespace$";
		
		readonly bool IsWebRoot = false;
		readonly bool BuildT4Files = true;
		readonly string FilesSubClassName = "T4Files";
		readonly bool BuildT4Links = false;
		readonly string LinksSubClassName = "T4Links";
		readonly bool BuildT4Embedded = false;
		readonly string EmbeddedSubClassName = "T4Embedded";
		readonly bool BuildT4Resources = false;
		readonly string ResourcesSubClassName = "T4Resources";

		readonly string AreasRootFolder = "Areas";
		readonly string ViewsFolder = "Views";

		readonly string[] LocalContentFileFolders = new string[] 
				{
						"StyleSheets",
						"JavaScripts",
						"Scripts",
						"Images",
						"Content",
						"Templates",
						"DocumentTemplates",
						"EmailTemplates",
						"DocumentGenerators",
						"ReportGenerators",
						"EmailGenerators",
						"CommunicationGenerators"
				};

		readonly string[] LocalContentFileIgnoreFileExtensions = new string[] 
				{
						".cs",
						".vb",
				};

		static readonly List<string> RootFileNames = new List<string>();

		static readonly Dictionary<System.Text.RegularExpressions.Regex, string> LocalContentFileNameReplacements = new Dictionary<System.Text.RegularExpressions.Regex, string>();
#>