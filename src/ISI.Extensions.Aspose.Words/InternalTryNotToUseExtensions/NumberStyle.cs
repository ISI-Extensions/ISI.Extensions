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

namespace ISI.Extensions.Aspose.InternalTryNotToUseExtensions
{
	public static partial class WordsExtensions
	{
		public static ISI.Extensions.Documents.NumberStyle ToNumberStyle(this global::Aspose.Words.NumberStyle numberStyle)
		{
			switch (numberStyle)
			{
				case global::Aspose.Words.NumberStyle.None: return ISI.Extensions.Documents.NumberStyle.None;
				case global::Aspose.Words.NumberStyle.Custom: return ISI.Extensions.Documents.NumberStyle.Custom;
				case global::Aspose.Words.NumberStyle.Arabic: return ISI.Extensions.Documents.NumberStyle.Arabic;
				case global::Aspose.Words.NumberStyle.UppercaseRoman: return ISI.Extensions.Documents.NumberStyle.UppercaseRoman;
				case global::Aspose.Words.NumberStyle.LowercaseRoman: return ISI.Extensions.Documents.NumberStyle.LowercaseRoman;
				case global::Aspose.Words.NumberStyle.UppercaseLetter: return ISI.Extensions.Documents.NumberStyle.UppercaseLetter;
				case global::Aspose.Words.NumberStyle.LowercaseLetter: return ISI.Extensions.Documents.NumberStyle.LowercaseLetter;
				case global::Aspose.Words.NumberStyle.Ordinal: return ISI.Extensions.Documents.NumberStyle.Ordinal;
				case global::Aspose.Words.NumberStyle.Number: return ISI.Extensions.Documents.NumberStyle.Number;
				case global::Aspose.Words.NumberStyle.OrdinalText: return ISI.Extensions.Documents.NumberStyle.OrdinalText;
				case global::Aspose.Words.NumberStyle.Hex: return ISI.Extensions.Documents.NumberStyle.Hex;
				case global::Aspose.Words.NumberStyle.ChicagoManual: return ISI.Extensions.Documents.NumberStyle.ChicagoManual;
				case global::Aspose.Words.NumberStyle.Kanji: return ISI.Extensions.Documents.NumberStyle.Kanji;
				case global::Aspose.Words.NumberStyle.KanjiDigit: return ISI.Extensions.Documents.NumberStyle.KanjiDigit;
				case global::Aspose.Words.NumberStyle.AiueoHalfWidth: return ISI.Extensions.Documents.NumberStyle.AiueoHalfWidth;
				case global::Aspose.Words.NumberStyle.IrohaHalfWidth: return ISI.Extensions.Documents.NumberStyle.IrohaHalfWidth;
				case global::Aspose.Words.NumberStyle.ArabicFullWidth: return ISI.Extensions.Documents.NumberStyle.ArabicFullWidth;
				case global::Aspose.Words.NumberStyle.ArabicHalfWidth: return ISI.Extensions.Documents.NumberStyle.ArabicHalfWidth;
				case global::Aspose.Words.NumberStyle.KanjiTraditional: return ISI.Extensions.Documents.NumberStyle.KanjiTraditional;
				case global::Aspose.Words.NumberStyle.KanjiTraditional2: return ISI.Extensions.Documents.NumberStyle.KanjiTraditional2;
				case global::Aspose.Words.NumberStyle.NumberInCircle: return ISI.Extensions.Documents.NumberStyle.NumberInCircle;
				case global::Aspose.Words.NumberStyle.DecimalFullWidth: return ISI.Extensions.Documents.NumberStyle.DecimalFullWidth;
				case global::Aspose.Words.NumberStyle.Aiueo: return ISI.Extensions.Documents.NumberStyle.Aiueo;
				case global::Aspose.Words.NumberStyle.Iroha: return ISI.Extensions.Documents.NumberStyle.Iroha;
				case global::Aspose.Words.NumberStyle.LeadingZero: return ISI.Extensions.Documents.NumberStyle.LeadingZero;
				case global::Aspose.Words.NumberStyle.Bullet: return ISI.Extensions.Documents.NumberStyle.Bullet;
				case global::Aspose.Words.NumberStyle.Ganada: return ISI.Extensions.Documents.NumberStyle.Ganada;
				case global::Aspose.Words.NumberStyle.Chosung: return ISI.Extensions.Documents.NumberStyle.Chosung;
				case global::Aspose.Words.NumberStyle.GB1: return ISI.Extensions.Documents.NumberStyle.GB1;
				case global::Aspose.Words.NumberStyle.GB2: return ISI.Extensions.Documents.NumberStyle.GB2;
				case global::Aspose.Words.NumberStyle.GB3: return ISI.Extensions.Documents.NumberStyle.GB3;
				case global::Aspose.Words.NumberStyle.GB4: return ISI.Extensions.Documents.NumberStyle.GB4;
				case global::Aspose.Words.NumberStyle.Zodiac1: return ISI.Extensions.Documents.NumberStyle.Zodiac1;
				case global::Aspose.Words.NumberStyle.Zodiac2: return ISI.Extensions.Documents.NumberStyle.Zodiac2;
				case global::Aspose.Words.NumberStyle.Zodiac3: return ISI.Extensions.Documents.NumberStyle.Zodiac3;
				case global::Aspose.Words.NumberStyle.TradChinNum1: return ISI.Extensions.Documents.NumberStyle.TradChinNum1;
				case global::Aspose.Words.NumberStyle.TradChinNum2: return ISI.Extensions.Documents.NumberStyle.TradChinNum2;
				case global::Aspose.Words.NumberStyle.TradChinNum3: return ISI.Extensions.Documents.NumberStyle.TradChinNum3;
				case global::Aspose.Words.NumberStyle.TradChinNum4: return ISI.Extensions.Documents.NumberStyle.TradChinNum4;
				case global::Aspose.Words.NumberStyle.SimpChinNum1: return ISI.Extensions.Documents.NumberStyle.SimpChinNum1;
				case global::Aspose.Words.NumberStyle.SimpChinNum2: return ISI.Extensions.Documents.NumberStyle.SimpChinNum2;
				case global::Aspose.Words.NumberStyle.SimpChinNum3: return ISI.Extensions.Documents.NumberStyle.SimpChinNum3;
				case global::Aspose.Words.NumberStyle.SimpChinNum4: return ISI.Extensions.Documents.NumberStyle.SimpChinNum4;
				case global::Aspose.Words.NumberStyle.HanjaRead: return ISI.Extensions.Documents.NumberStyle.HanjaRead;
				case global::Aspose.Words.NumberStyle.HanjaReadDigit: return ISI.Extensions.Documents.NumberStyle.HanjaReadDigit;
				case global::Aspose.Words.NumberStyle.Hangul: return ISI.Extensions.Documents.NumberStyle.Hangul;
				case global::Aspose.Words.NumberStyle.Hanja: return ISI.Extensions.Documents.NumberStyle.Hanja;
				case global::Aspose.Words.NumberStyle.Hebrew1: return ISI.Extensions.Documents.NumberStyle.Hebrew1;
				case global::Aspose.Words.NumberStyle.Arabic1: return ISI.Extensions.Documents.NumberStyle.Arabic1;
				case global::Aspose.Words.NumberStyle.Hebrew2: return ISI.Extensions.Documents.NumberStyle.Hebrew2;
				case global::Aspose.Words.NumberStyle.Arabic2: return ISI.Extensions.Documents.NumberStyle.Arabic2;
				case global::Aspose.Words.NumberStyle.HindiLetter1: return ISI.Extensions.Documents.NumberStyle.HindiLetter1;
				case global::Aspose.Words.NumberStyle.HindiLetter2: return ISI.Extensions.Documents.NumberStyle.HindiLetter2;
				case global::Aspose.Words.NumberStyle.HindiArabic: return ISI.Extensions.Documents.NumberStyle.HindiArabic;
				case global::Aspose.Words.NumberStyle.HindiCardinalText: return ISI.Extensions.Documents.NumberStyle.HindiCardinalText;
				case global::Aspose.Words.NumberStyle.ThaiLetter: return ISI.Extensions.Documents.NumberStyle.ThaiLetter;
				case global::Aspose.Words.NumberStyle.ThaiArabic: return ISI.Extensions.Documents.NumberStyle.ThaiArabic;
				case global::Aspose.Words.NumberStyle.ThaiCardinalText: return ISI.Extensions.Documents.NumberStyle.ThaiCardinalText;
				case global::Aspose.Words.NumberStyle.VietCardinalText: return ISI.Extensions.Documents.NumberStyle.VietCardinalText;
				case global::Aspose.Words.NumberStyle.NumberInDash: return ISI.Extensions.Documents.NumberStyle.NumberInDash;
				case global::Aspose.Words.NumberStyle.LowercaseRussian: return ISI.Extensions.Documents.NumberStyle.LowercaseRussian;
				case global::Aspose.Words.NumberStyle.UppercaseRussian: return ISI.Extensions.Documents.NumberStyle.UppercaseRussian;
				default:
					throw new ArgumentOutOfRangeException(nameof(numberStyle), numberStyle, null);
			}
		}

		public static global::Aspose.Words.NumberStyle ToNumberStyle(this ISI.Extensions.Documents.NumberStyle numberStyle)
		{
			switch (numberStyle)
			{
				case ISI.Extensions.Documents.NumberStyle.None: return global::Aspose.Words.NumberStyle.None;
				case ISI.Extensions.Documents.NumberStyle.Custom: return global::Aspose.Words.NumberStyle.Custom;
				case ISI.Extensions.Documents.NumberStyle.Arabic: return global::Aspose.Words.NumberStyle.Arabic;
				case ISI.Extensions.Documents.NumberStyle.UppercaseRoman: return global::Aspose.Words.NumberStyle.UppercaseRoman;
				case ISI.Extensions.Documents.NumberStyle.LowercaseRoman: return global::Aspose.Words.NumberStyle.LowercaseRoman;
				case ISI.Extensions.Documents.NumberStyle.UppercaseLetter: return global::Aspose.Words.NumberStyle.UppercaseLetter;
				case ISI.Extensions.Documents.NumberStyle.LowercaseLetter: return global::Aspose.Words.NumberStyle.LowercaseLetter;
				case ISI.Extensions.Documents.NumberStyle.Ordinal: return global::Aspose.Words.NumberStyle.Ordinal;
				case ISI.Extensions.Documents.NumberStyle.Number: return global::Aspose.Words.NumberStyle.Number;
				case ISI.Extensions.Documents.NumberStyle.OrdinalText: return global::Aspose.Words.NumberStyle.OrdinalText;
				case ISI.Extensions.Documents.NumberStyle.Hex: return global::Aspose.Words.NumberStyle.Hex;
				case ISI.Extensions.Documents.NumberStyle.ChicagoManual: return global::Aspose.Words.NumberStyle.ChicagoManual;
				case ISI.Extensions.Documents.NumberStyle.Kanji: return global::Aspose.Words.NumberStyle.Kanji;
				case ISI.Extensions.Documents.NumberStyle.KanjiDigit: return global::Aspose.Words.NumberStyle.KanjiDigit;
				case ISI.Extensions.Documents.NumberStyle.AiueoHalfWidth: return global::Aspose.Words.NumberStyle.AiueoHalfWidth;
				case ISI.Extensions.Documents.NumberStyle.IrohaHalfWidth: return global::Aspose.Words.NumberStyle.IrohaHalfWidth;
				case ISI.Extensions.Documents.NumberStyle.ArabicFullWidth: return global::Aspose.Words.NumberStyle.ArabicFullWidth;
				case ISI.Extensions.Documents.NumberStyle.ArabicHalfWidth: return global::Aspose.Words.NumberStyle.ArabicHalfWidth;
				case ISI.Extensions.Documents.NumberStyle.KanjiTraditional: return global::Aspose.Words.NumberStyle.KanjiTraditional;
				case ISI.Extensions.Documents.NumberStyle.KanjiTraditional2: return global::Aspose.Words.NumberStyle.KanjiTraditional2;
				case ISI.Extensions.Documents.NumberStyle.NumberInCircle: return global::Aspose.Words.NumberStyle.NumberInCircle;
				case ISI.Extensions.Documents.NumberStyle.DecimalFullWidth: return global::Aspose.Words.NumberStyle.DecimalFullWidth;
				case ISI.Extensions.Documents.NumberStyle.Aiueo: return global::Aspose.Words.NumberStyle.Aiueo;
				case ISI.Extensions.Documents.NumberStyle.Iroha: return global::Aspose.Words.NumberStyle.Iroha;
				case ISI.Extensions.Documents.NumberStyle.LeadingZero: return global::Aspose.Words.NumberStyle.LeadingZero;
				case ISI.Extensions.Documents.NumberStyle.Bullet: return global::Aspose.Words.NumberStyle.Bullet;
				case ISI.Extensions.Documents.NumberStyle.Ganada: return global::Aspose.Words.NumberStyle.Ganada;
				case ISI.Extensions.Documents.NumberStyle.Chosung: return global::Aspose.Words.NumberStyle.Chosung;
				case ISI.Extensions.Documents.NumberStyle.GB1: return global::Aspose.Words.NumberStyle.GB1;
				case ISI.Extensions.Documents.NumberStyle.GB2: return global::Aspose.Words.NumberStyle.GB2;
				case ISI.Extensions.Documents.NumberStyle.GB3: return global::Aspose.Words.NumberStyle.GB3;
				case ISI.Extensions.Documents.NumberStyle.GB4: return global::Aspose.Words.NumberStyle.GB4;
				case ISI.Extensions.Documents.NumberStyle.Zodiac1: return global::Aspose.Words.NumberStyle.Zodiac1;
				case ISI.Extensions.Documents.NumberStyle.Zodiac2: return global::Aspose.Words.NumberStyle.Zodiac2;
				case ISI.Extensions.Documents.NumberStyle.Zodiac3: return global::Aspose.Words.NumberStyle.Zodiac3;
				case ISI.Extensions.Documents.NumberStyle.TradChinNum1: return global::Aspose.Words.NumberStyle.TradChinNum1;
				case ISI.Extensions.Documents.NumberStyle.TradChinNum2: return global::Aspose.Words.NumberStyle.TradChinNum2;
				case ISI.Extensions.Documents.NumberStyle.TradChinNum3: return global::Aspose.Words.NumberStyle.TradChinNum3;
				case ISI.Extensions.Documents.NumberStyle.TradChinNum4: return global::Aspose.Words.NumberStyle.TradChinNum4;
				case ISI.Extensions.Documents.NumberStyle.SimpChinNum1: return global::Aspose.Words.NumberStyle.SimpChinNum1;
				case ISI.Extensions.Documents.NumberStyle.SimpChinNum2: return global::Aspose.Words.NumberStyle.SimpChinNum2;
				case ISI.Extensions.Documents.NumberStyle.SimpChinNum3: return global::Aspose.Words.NumberStyle.SimpChinNum3;
				case ISI.Extensions.Documents.NumberStyle.SimpChinNum4: return global::Aspose.Words.NumberStyle.SimpChinNum4;
				case ISI.Extensions.Documents.NumberStyle.HanjaRead: return global::Aspose.Words.NumberStyle.HanjaRead;
				case ISI.Extensions.Documents.NumberStyle.HanjaReadDigit: return global::Aspose.Words.NumberStyle.HanjaReadDigit;
				case ISI.Extensions.Documents.NumberStyle.Hangul: return global::Aspose.Words.NumberStyle.Hangul;
				case ISI.Extensions.Documents.NumberStyle.Hanja: return global::Aspose.Words.NumberStyle.Hanja;
				case ISI.Extensions.Documents.NumberStyle.Hebrew1: return global::Aspose.Words.NumberStyle.Hebrew1;
				case ISI.Extensions.Documents.NumberStyle.Arabic1: return global::Aspose.Words.NumberStyle.Arabic1;
				case ISI.Extensions.Documents.NumberStyle.Hebrew2: return global::Aspose.Words.NumberStyle.Hebrew2;
				case ISI.Extensions.Documents.NumberStyle.Arabic2: return global::Aspose.Words.NumberStyle.Arabic2;
				case ISI.Extensions.Documents.NumberStyle.HindiLetter1: return global::Aspose.Words.NumberStyle.HindiLetter1;
				case ISI.Extensions.Documents.NumberStyle.HindiLetter2: return global::Aspose.Words.NumberStyle.HindiLetter2;
				case ISI.Extensions.Documents.NumberStyle.HindiArabic: return global::Aspose.Words.NumberStyle.HindiArabic;
				case ISI.Extensions.Documents.NumberStyle.HindiCardinalText: return global::Aspose.Words.NumberStyle.HindiCardinalText;
				case ISI.Extensions.Documents.NumberStyle.ThaiLetter: return global::Aspose.Words.NumberStyle.ThaiLetter;
				case ISI.Extensions.Documents.NumberStyle.ThaiArabic: return global::Aspose.Words.NumberStyle.ThaiArabic;
				case ISI.Extensions.Documents.NumberStyle.ThaiCardinalText: return global::Aspose.Words.NumberStyle.ThaiCardinalText;
				case ISI.Extensions.Documents.NumberStyle.VietCardinalText: return global::Aspose.Words.NumberStyle.VietCardinalText;
				case ISI.Extensions.Documents.NumberStyle.NumberInDash: return global::Aspose.Words.NumberStyle.NumberInDash;
				case ISI.Extensions.Documents.NumberStyle.LowercaseRussian: return global::Aspose.Words.NumberStyle.LowercaseRussian;
				case ISI.Extensions.Documents.NumberStyle.UppercaseRussian: return global::Aspose.Words.NumberStyle.UppercaseRussian;
				default:
					throw new ArgumentOutOfRangeException(nameof(numberStyle), numberStyle, null);
			}
		}
	}
}