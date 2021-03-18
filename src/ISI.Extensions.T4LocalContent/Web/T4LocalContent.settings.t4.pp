<#+
		readonly string Namespace = "$rootnamespace$";
		
		readonly bool IsWebRoot = true;
		readonly bool BuildT4Files = true;
		readonly string FilesSubClassName = "T4Files";
		readonly bool BuildT4Links = true;
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
				};

		readonly string[] LocalContentFileIgnoreFileExtensions = new string[] 
				{
						".cs",
						".vb",
				};

		static readonly List<string> RootFileNames = new List<string>()
				{
						"favicon.ico",
						"favicon.png"
				};

		static readonly Dictionary<System.Text.RegularExpressions.Regex, string> LocalContentFileNameReplacements = new Dictionary<System.Text.RegularExpressions.Regex, string>()
				{
						{ new System.Text.RegularExpressions.Regex(@"^(jquery-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.js)$"), "jquery.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(jquery-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.min\.js)$"), "jquery.min.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(jquery-migrate-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.js)$"), "jquery-migrate.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(jquery-migrate-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.min\.js)$"), "jquery-migrate.min.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(jquery-ui-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.js)$"), "jquery-ui.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(jquery-ui-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.min\.js)$"), "jquery-ui.min.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(jquery.signalR-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.js)$"), "jquery.signalR.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(jquery.signalR-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.min\.js)$"), "jquery.signalR.min.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(jquery.maskedinput-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.js)$"), "jquery.maskedinput.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(jquery.maskedinput-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.min\.js)$"), "jquery.maskedinput.min.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(knockout-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.js)$"), "knockout.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(knockout-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.min\.js)$"), "knockout.min.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(modernizr-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.js)$"), "modernizr.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(modernizr-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.min\.js)$"), "modernizr.min.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(jquery.signalR-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.js)$"), "jquery.signalR.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(jquery.signalR-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.min\.js)$"), "jquery.signalR.min.js"},
						{ new System.Text.RegularExpressions.Regex(@"^(jquery-ui-(?:\d+)\.(?:\d+)(?:\.(?:\d+))?\.custom.css)$"), "jquery-ui.custom.css"}
				};
#>