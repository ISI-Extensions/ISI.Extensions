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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Telephony.Calls
{
	[DataContract]
	public enum Language
	{
		[ISI.Extensions.Enum("da-DK")] DaDk,
		[ISI.Extensions.Enum("de-DE")] DeDe,
		[ISI.Extensions.Enum("en-AU")] EnAu,
		[ISI.Extensions.Enum("en-CA")] EnCa,
		[ISI.Extensions.Enum("en-GB")] EnGb,
		[ISI.Extensions.Enum("en-IN")] EnIn,
		[ISI.Extensions.Enum("en-US")] EnUs,
		[ISI.Extensions.Enum("ca-ES")] CaEs,
		[ISI.Extensions.Enum("es-ES")] EsEs,
		[ISI.Extensions.Enum("es-MX")] EsMx,
		[ISI.Extensions.Enum("fi-FI")] FiFi,
		[ISI.Extensions.Enum("fr-CA")] FrCa,
		[ISI.Extensions.Enum("fr-FR")] FrFr,
		[ISI.Extensions.Enum("it-IT")] ItIt,
		[ISI.Extensions.Enum("ja-JP")] JaJp,
		[ISI.Extensions.Enum("ko-KR")] KoKr,
		[ISI.Extensions.Enum("nb-NO")] NbNo,
		[ISI.Extensions.Enum("nl-NL")] NlNl,
		[ISI.Extensions.Enum("pl-PL")] PlPl,
		[ISI.Extensions.Enum("pt-BR")] PtBr,
		[ISI.Extensions.Enum("pt-PT")] PtPt,
		[ISI.Extensions.Enum("ru-RU")] RuRu,
		[ISI.Extensions.Enum("sv-SE")] SvSe,
		[ISI.Extensions.Enum("zh-CN")] ZhCn,
		[ISI.Extensions.Enum("zh-HK")] ZhHk,
		[ISI.Extensions.Enum("zh-TW")] ZhTw,
	}
}