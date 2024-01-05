using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using ISI.Extensions.TypeLocator.Extensions;
using SerializableEntitiesDTOs = ISI.Extensions.JsonJwt.SerializableEntities;

namespace ISI.Extensions.JsonJwt
{
	public partial class JwtEncoder
	{
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }
		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; }

		private static IDictionary<string, ISI.Extensions.JsonJwt.JwtAlgorithms.IJwtAlgorithm> _jwtAlgorithms = null;
		protected IDictionary<string, ISI.Extensions.JsonJwt.JwtAlgorithms.IJwtAlgorithm> JwtAlgorithms => _jwtAlgorithms ??= TypeLocator.Container.LocalContainer.GetImplementations<ISI.Extensions.JsonJwt.JwtAlgorithms.IJwtAlgorithm>().ToDictionary(jwtAlgorithm => jwtAlgorithm.AlgorithmType, _ => _, StringComparer.InvariantCultureIgnoreCase);


		public static readonly System.Text.RegularExpressions.Regex RegexJws = new("^[A-Za-z0-9-_]+\\.[A-Za-z0-9-_]*\\.[A-Za-z0-9-_]*$", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100.0));

		public JwtEncoder(
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer)
		{
			DateTimeStamper = dateTimeStamper;
			JsonSerializer = jsonSerializer;
		}
	}
}