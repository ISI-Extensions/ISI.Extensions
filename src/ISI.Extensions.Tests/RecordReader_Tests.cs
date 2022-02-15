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
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class TextParser_Tests
	{
		[Test]
		public void CsvTextParser_Test()
		{
			var data = "42003012450000,Permian Basin,32.1636734,-102.3899460,,1962-01-25T00:00:00Z,USA,ANDREWS,FASKEN OIL AND RANCH,2021-09-01T00:30:18Z,2013-02-21T13:00:43Z,2021-06-19T14:19:33Z,0,INEZ,1993-01-01T00:00:00Z,2200 N 1760 E,G&MMB&A,41 T2N,33,263,,0,,,a38d526b-bd37-4f64-814a-32a893c5b8ec,1702616,0,,,,32.1636734,\"FEE \"\"Q\"\"\",133915,-102.3899460,12500,,FASKEN OIL AND RANCH,2004-08-23T00:00:00Z,549786,,,CONSOLIDATED,,\"FASKEN OIL AND RANCH, LTD.\",INEZ (DEEP),\"FASKEN OIL AND RANCH, LTD.\",Vertical,ACTIVE - PRODUCING,GAS WELL,3279324,1977-05-14T00:00:00Z,TX,,,,Mature,\"LINESTRING (-102.39 32.1637, -102.39 32.1637, -102.39 32.1637, -102.39 32.1637)\",,VERTICAL,a38d526b-bd37-4f64-814a-32a893c5b8ec,\"FEE \"\"Q\"\" 3U\",3 U,Active Producer,CONDENSATE";

			var parser = ISI.Extensions.TextParserFactory.GetTextParser(TextParserFactory.TextDelimiter.CommaSeparatedValues);

			var context = parser.CreateTextParserContext();

			var values = parser.Read(context, data);

			Assert.True(string.Equals(data, values.Source, StringComparison.Ordinal));
		}

		[Test]
		public void CsvDataReader_Test()
		{
			var fullName = @"E:\data\Well Data\20210915\TX\HeaderRecord.csv";

			using (var stream = System.IO.File.OpenRead(fullName))
			{
				var columns = ISI.Extensions.Columns.ColumnInfoCollection<Record>.GetDefault();

				var parser = ISI.Extensions.RecordParserFactory<Record>.GetRecordParserByFileName(fullName, columns, new[] { ISI.Extensions.RecordParserFactory<Record>.GetHeadersOnFirstLineHandler() });

				var reader = new ISI.Extensions.RecordReaders.RecordParserReader<Record>(stream, parser);

				var records = reader.ToArray();
			}
		}

		public class Record
		{
			public string API { get; set; }
			public string Basin { get; set; }
			public string Bhlatitude { get; set; }
			public string Bhlongitude { get; set; }
			public string Comments { get; set; }
			public DateTime CompletionDate { get; set; }
			public string Country { get; set; }
			public string County { get; set; }
			public string CurrentOperator { get; set; }
			public string DateCatalogued { get; set; }
			public string DateCreated { get; set; }
			public string DateLastModified { get; set; }
			public string DrillingFloorElevation { get; set; }
			public string Field { get; set; }
			public DateTime FirstProdDate { get; set; }
			public string Footages { get; set; }
			public string GrId1 { get; set; }
			public string GrId2 { get; set; }
			public string GrId3 { get; set; }
			public string GrId4 { get; set; }
			public string GrId5 { get; set; }
			public string GroundElevation { get; set; }
			public string HeelLatitude { get; set; }
			public string HeelLongitude { get; set; }
			public int Id { get; set; }
			public int InternalId { get; set; }
			public string KellyBushingElevation { get; set; }
			public string KopLatitude { get; set; }
			public string KopLongitude { get; set; }
			public string LateralLength { get; set; }
			public string Latitude { get; set; }
			public string Lease { get; set; }
			public int LeaseId { get; set; }
			public string Longitude { get; set; }
			public string MeasuredDepth { get; set; }
			public string OffshoreWaterDepth { get; set; }
			public string OriginalOperator { get; set; }
			public DateTime PermitDate { get; set; }
			public string PermitNumber { get; set; }
			public string Play { get; set; }
			public DateTime PlugDate { get; set; }
			public string PrimaryFormation { get; set; }
			public string Quarter { get; set; }
			public string ReportedCurrentOperator { get; set; }
			public string ReportedField { get; set; }
			public string ReportedOriginalOperator { get; set; }
			public string ReportedWellboreProfile { get; set; }
			public string ReportedWellStatus { get; set; }
			public string ReportedWellType { get; set; }
			public int SimpleId { get; set; }
			public DateTime SpudDate { get; set; }
			public string State { get; set; }
			public int StateId { get; set; }
			public string StateIdDesc { get; set; }
			public DateTime TestDate { get; set; }
			public string ThermalMaturity { get; set; }
			public string Trajectory { get; set; }
			public string TrueVerticalDepth { get; set; }
			public string WellBoreProfile { get; set; }
			public int WellId { get; set; }
			public string WellName { get; set; }
			public string WellNumber { get; set; }
			public string WellStatus { get; set; }
			public string WellType { get; set; }
		}
	}
}