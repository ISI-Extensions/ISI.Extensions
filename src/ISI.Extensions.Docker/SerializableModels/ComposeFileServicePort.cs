using System;
using System.Collections.Generic;
using System.Text;
using ISI.Extensions.Extensions;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileServicePort
	{
		[YamlMember(Alias = "target", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public int Target { get; set; }

		[YamlMember(Alias = "published", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public int Published { get; set; }

		[YamlMember(Alias = "protocol", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Protocol { get; set; }

		[YamlMember(Alias = "mode", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Mode { get; set; }

		public string RawValue { get; set; }
		public bool IsYaml { get; set; }
		public int? StartIndex { get; set; }
		public int? EndIndex { get; set; }
	}

	public class ComposeFileServicePortTypeConverter : IYamlTypeConverter
	{
		private static readonly System.Text.RegularExpressions.Regex _portRegex = new(@"(?:\s*)(?<published>\d+)(?:\:)(?<target>\d+)(?:/|\\)?(?<protocol>[a-z]+)?(?:\s*)");

		public bool Accepts(Type type) => (type == typeof(ComposeFileServicePort));

		public object ReadYaml(YamlDotNet.Core.IParser parser, Type type, ObjectDeserializer rootDeserializer)
		{
			var composeFileServicePort = new ComposeFileServicePort
			{
				StartIndex = (int?)(parser.Current as YamlDotNet.Core.Events.NodeEvent)?.Start.Index
			};

			if (parser.TryConsume<YamlDotNet.Core.Events.MappingStart>(out _))
			{
				ParseMapping(composeFileServicePort, parser); // We're parsing a YAML object
			}
			else if (parser.Current is YamlDotNet.Core.Events.Scalar scalar && _portRegex.Match(scalar.Value) is { Success: true } portMatch)
			{
				composeFileServicePort.Published = portMatch.Groups["published"].Value.ToInt();
				composeFileServicePort.Target = portMatch.Groups["target"].Value.ToInt();
				composeFileServicePort.Protocol = portMatch.Groups["protocol"]?.Value ?? string.Empty;

				composeFileServicePort.EndIndex = (int?)scalar?.End.Index;

				parser.MoveNext();
			}


			return composeFileServicePort;
		}

		private static void ParseMapping(ComposeFileServicePort composeFileServicePort, IParser parser)
		{
			// Read all the key-value pairs until we reached the end of the YAML object
			while (!parser.Accept<YamlDotNet.Core.Events.MappingEnd>(out _))
			{
				var key = parser.Consume<YamlDotNet.Core.Events.Scalar>();
				var value = parser.Consume<YamlDotNet.Core.Events.Scalar>();

				if (string.Equals(key.Value, nameof(ComposeFileServicePort.Target), StringComparison.InvariantCultureIgnoreCase))
				{
					composeFileServicePort.Target = value.Value.ToInt();
				}
				else if (string.Equals(key.Value, nameof(ComposeFileServicePort.Published), StringComparison.InvariantCultureIgnoreCase))
				{
					composeFileServicePort.Published = value.Value.ToInt();
				}
				else if (string.Equals(key.Value, nameof(ComposeFileServicePort.Protocol), StringComparison.InvariantCultureIgnoreCase))
				{
					composeFileServicePort.Protocol = value.Value;
				}
				else if (string.Equals(key.Value, nameof(ComposeFileServicePort.Mode), StringComparison.InvariantCultureIgnoreCase))
				{
					composeFileServicePort.Mode = value.Value;
				}
			}

			composeFileServicePort.IsYaml = true;

			composeFileServicePort.EndIndex = (int?)(parser.Current as YamlDotNet.Core.Events.MappingEnd)?.End.Index;

			parser.MoveNext();
		}

		private static readonly char[] KeyCharactersThatRequireQuotes = [' ', '/', '\\', '~', ':', '$', '{', '}'];

		public void WriteYaml(YamlDotNet.Core.IEmitter emitter, object value, Type type, ObjectSerializer serializer)
		{
			//var composeFileServicePort = (ComposeFileServicePort)value;

			//// We start a new YAML object
			//emitter.Emit(new YamlDotNet.Core.Events.MappingStart(AnchorName.Empty, TagName.Empty, isImplicit: true, YamlDotNet.Core.Events.MappingStyle.Block));

			//foreach (var entry in composeFileServicePort)
			//{
			//	// We try to determine if the value needs to be quoted if it contains special characters
			//	var keyScalar = entry.Key.IndexOfAny(KeyCharactersThatRequireQuotes) >= 0
			//		? new YamlDotNet.Core.Events.Scalar(AnchorName.Empty, TagName.Empty, entry.Key, ScalarStyle.DoubleQuoted, isPlainImplicit: false, isQuotedImplicit: true)
			//		: new YamlDotNet.Core.Events.Scalar(AnchorName.Empty, TagName.Empty, entry.Key, ScalarStyle.Plain, isPlainImplicit: true, isQuotedImplicit: false);

			//	// Write the key, then the value
			//	emitter.Emit(keyScalar);
			//	emitter.Emit(new YamlDotNet.Core.Events.Scalar(AnchorName.Empty, TagName.Empty, entry.Value, ScalarStyle.DoubleQuoted, isPlainImplicit: false, isQuotedImplicit: true));
			//}

			//// We end the YAML object
			//emitter.Emit(new YamlDotNet.Core.Events.MappingEnd());
		}
	}
}
