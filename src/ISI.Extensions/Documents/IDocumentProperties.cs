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

namespace ISI.Extensions.Documents
{
	public interface IDocumentProperties
	{
		string Subject { get; set; }
		string Author { get; set; }
		string Keywords { get; set; }
		string Title { get; set; }
	}

	/*
	http://www.aspose.com/api/net/cells/aspose.cells/workbook/properties/builtindocumentproperties
	Subject
	Author
	Keywords
	Comments
	Template
	Last Author
	Revision Number
	Application Name
	Last Print Date
	Creation Date
	Last Save Time
	Total Editing Time
	Number of Pages
	Number of Words
	Number of Characters
	Security
	Category
	Format
	Manager
	Company
	Number of Bytes
	Number of Lines
	Number of Paragraphs
	Number of Slides
	Number of Notes
	Number of Hidden Slides
	Number of Multimedia Clips
	*/

	/// <summary>
	/// Does not implement Creator, Producer, Title
	/// </summary>
	public interface ISpreadSheetDocumentProperties : IDocumentProperties
	{
		string Comments { get; set; }
		string Template { get; set; }
		string RevisionNumber { get; set; }
		string ApplicationName { get; set; }
		string Category { get; set; }
		string Manager { get; set; }
		string Company { get; set; }
	}

	/*
	http://www.aspose.com/docs/display/wordsnet/9.2.1.5+Get+Document+Properties
	Document doc = new Document("Get Document Properties.doc");
	foreach (DocumentProperty prop in doc.BuiltInDocumentProperties)
	{
			Console.WriteLine(prop.Name+": "+ prop.Value);
	} 
	SummaryInformation summaryInfo = new SummaryInformation(new PropertySet(new FileStream("Get Document Properties.doc", FileMode.Open)));
	Console.WriteLine(summaryInfo.ApplicationName);
	Console.WriteLine(summaryInfo.Author);
	Console.WriteLine(summaryInfo.Comments);
	Console.WriteLine(summaryInfo.CharCount);
	Console.WriteLine(summaryInfo.EditTime);
	Console.WriteLine(summaryInfo.Keywords);
	Console.WriteLine(summaryInfo.LastAuthor);
	Console.WriteLine(summaryInfo.PageCount);
	Console.WriteLine(summaryInfo.RevNumber);
	Console.WriteLine(summaryInfo.Security);
	Console.WriteLine(summaryInfo.Subject);
	Console.WriteLine(summaryInfo.Template);
	*/
	public interface IWordDocumentProperties : IDocumentProperties
	{

	}

	/*
	http://www.aspose.com/docs/display/pdfnet/set+document+information
	Aspose.Pdf.Generator.Pdf pdf1 = new Aspose.Pdf.Generator.Pdf(); 
	pdf1.Author = "Nayyer Shahbaz"; 
	pdf1.Creator = "Aspose.Pdf"; 
	pdf1.Keywords = "Hello World"; 
	pdf1.Subject = "Example"; 
	pdf1.Title = "Example";
	*/
	public interface IPdfDocumentProperties : IDocumentProperties
	{
		bool ObfuscateFieldNames { get; }
	}
}
