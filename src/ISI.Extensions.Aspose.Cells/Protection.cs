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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Aspose.Extensions;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Cells
	{
		public class Protection : ISI.Extensions.SpreadSheets.IProtection
		{
			internal readonly ISI.Extensions.SpreadSheets.IWorksheet _worksheet = null;
			internal readonly global::Aspose.Cells.Protection _protection = null;

			internal Protection(ISI.Extensions.SpreadSheets.IWorksheet worksheet)
			{
				_worksheet = worksheet;
				_protection = worksheet.GetAsposeWorksheet().Protection;
			}

			public void Copy(ISI.Extensions.SpreadSheets.IProtection source)
			{
				_protection.Copy(source.GetAsposeProtection());
			}

			public bool AllowDeletingColumn
			{
				get => _protection.AllowDeletingColumn;
				set => _protection.AllowDeletingColumn = value;
			}

			public bool AllowDeletingRow
			{
				get => _protection.AllowDeletingRow;
				set => _protection.AllowDeletingRow = value;
			}

			public bool AllowFiltering
			{
				get => _protection.AllowFiltering;
				set => _protection.AllowFiltering = value;
			}

			public bool AllowFormattingCell
			{
				get => _protection.AllowFormattingCell;
				set => _protection.AllowFormattingCell = value;
			}

			public bool AllowFormattingColumn
			{
				get => _protection.AllowFormattingColumn;
				set => _protection.AllowFormattingColumn = value;
			}

			public bool AllowFormattingRow
			{
				get => _protection.AllowFormattingRow;
				set => _protection.AllowFormattingRow = value;
			}

			public bool AllowInsertingColumn
			{
				get => _protection.AllowInsertingColumn;
				set => _protection.AllowInsertingColumn = value;
			}

			public bool AllowInsertingHyperlink
			{
				get => _protection.AllowInsertingHyperlink;
				set => _protection.AllowInsertingHyperlink = value;
			}

			public bool AllowInsertingRow
			{
				get => _protection.AllowInsertingRow;
				set => _protection.AllowInsertingRow = value;
			}

			public bool AllowSorting
			{
				get => _protection.AllowSorting;
				set => _protection.AllowSorting = value;
			}

			public bool AllowUsingPivotTable
			{
				get => _protection.AllowUsingPivotTable;
				set => _protection.AllowUsingPivotTable = value;
			}

			public bool AllowEditingContent
			{
				get => _protection.AllowEditingContent;
				set => _protection.AllowEditingContent = value;
			}

			public bool AllowEditingObject
			{
				get => _protection.AllowEditingObject;
				set => _protection.AllowEditingObject = value;
			}

			public bool AllowEditingScenario
			{
				get => _protection.AllowEditingScenario;
				set => _protection.AllowEditingScenario = value;
			}

			public string Password
			{
				get => _protection.Password;
				set => _protection.Password = value;
			}

			public bool IsProtectedWithPassword => _protection.IsProtectedWithPassword;

			public bool VerifyPassword(string password) => _protection.VerifyPassword(password);

			public bool AllowSelectingLockedCell
			{
				get => _protection.AllowSelectingLockedCell;
				set => _protection.AllowSelectingLockedCell = value;
			}

			public bool AllowSelectingUnlockedCell
			{
				get => _protection.AllowSelectingUnlockedCell;
				set => _protection.AllowSelectingUnlockedCell = value;
			}
		}
	}
}