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
using ISI.Extensions.Extensions;

//Straight from Aspose .......
namespace ISI.Extensions.Aspose
{
	public partial class Words
	{
		internal static class XpsPrintHelper
		{
			public static void Print(global::Aspose.Words.Document document, string printerName, string jobName = null, bool isWait = false)
			{
				if (document == null)
				{
					throw new ArgumentNullException(nameof(document));
				}

				if (string.IsNullOrWhiteSpace(jobName))
				{
					jobName = "document";
				}

				// Use Aspose.Words to convert the document to XPS and store in a memory stream.
				using (var stream = new System.IO.MemoryStream())
				{
					document.Save(stream, global::Aspose.Words.SaveFormat.Xps);
					
					stream.Rewind();

					Print(stream, printerName, jobName, isWait);
				}
			}

			private static void Print(System.IO.Stream stream, string printerName, string jobName, bool isWait)
			{
				if (stream == null)
				{
					throw new ArgumentNullException(nameof(stream));
				}

				if (printerName == null)
				{
					throw new ArgumentNullException(nameof(printerName));
				}

				// Create an event that we will wait on until the job is complete.
				var completionEvent = CreateEvent(IntPtr.Zero, true, false, null);
				if (completionEvent == IntPtr.Zero)
				{
					throw new System.ComponentModel.Win32Exception();
				}

				try
				{
					StartJob(printerName, jobName, completionEvent, out var job, out var jobStream);

					CopyJob(stream, job, jobStream);

					if (isWait)
					{
						WaitForJob(completionEvent);
						CheckJobStatus(job);
					}
				}
				finally
				{
					if (completionEvent != IntPtr.Zero)
					{
						CloseHandle(completionEvent);
					}
				}
			}

			private static void StartJob(string printerName, string jobName, IntPtr completionEvent, out IXpsPrintJob job, out IXpsPrintJobStream jobStream)
			{
				var result = StartXpsPrintJob(printerName, jobName, null, IntPtr.Zero, completionEvent, null, 0, out job, out jobStream, IntPtr.Zero);

				if (result != 0)
				{
					throw new System.ComponentModel.Win32Exception(result);
				}
			}

			private static void CopyJob(System.IO.Stream stream, IXpsPrintJob job, IXpsPrintJobStream jobStream)
			{
				try
				{
					var buff = new byte[4096];
					while (true)
					{
						var read = (uint)stream.Read(buff, 0, buff.Length);
						if (read == 0)
							break;

						jobStream.Write(buff, read, out var written);

						if (read != written)
						{
							throw new Exception("Failed to copy data to the print job stream.");
						}
					}

					// Indicate that the entire document has been copied.
					jobStream.Close();
				}
				catch (Exception)
				{
					// Cancel the job if we had any trouble submitting it.
					job.Cancel();
					throw;
				}
			}

			private static void WaitForJob(IntPtr completionEvent)
			{
				const int INFINITE = -1;

				switch (WaitForSingleObject(completionEvent, INFINITE))
				{
					case WAIT_RESULT.WAIT_OBJECT_0:
						// Expected result, do nothing.
						break;

					case WAIT_RESULT.WAIT_FAILED:
						throw new System.ComponentModel.Win32Exception();

					default:
						throw new Exception("Unexpected result when waiting for the print job.");
				}
			}

			private static void CheckJobStatus(IXpsPrintJob job)
			{
				job.GetJobStatus(out var jobStatus);
				switch (jobStatus.completion)
				{
					case XPS_JOB_COMPLETION.XPS_JOB_COMPLETED:
						// Expected result, do nothing.
						break;

					case XPS_JOB_COMPLETION.XPS_JOB_FAILED:
						throw new System.ComponentModel.Win32Exception(jobStatus.jobStatus);

					default:
						throw new Exception("Unexpected print job status.");
				}
			}

			[System.Runtime.InteropServices.DllImport("XpsPrint.dll", EntryPoint = "StartXpsPrintJob")]
			private static extern int StartXpsPrintJob(
				[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
				string printerName,
				[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
				string jobName,
				[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
				string outputFileName,
				IntPtr progressEvent, // HANDLE
				IntPtr completionEvent, // HANDLE
				[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPArray)]
				byte[] printablePagesOn,
				uint printablePagesOnCount,
				out IXpsPrintJob xpsPrintJob,
				out IXpsPrintJobStream documentStream,
				IntPtr printTicketStream); // This is actually "out IXpsPrintJobStream", but we don't use it and just want to pass null, hence IntPtr.

			[System.Runtime.InteropServices.DllImport("Kernel32.dll", SetLastError = true)]
			private static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

			[System.Runtime.InteropServices.DllImport("Kernel32.dll", SetLastError = true, ExactSpelling = true)]
			private static extern WAIT_RESULT WaitForSingleObject(IntPtr handle, int milliseconds);

			[System.Runtime.InteropServices.DllImport("Kernel32.dll", SetLastError = true)]
			[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
			private static extern bool CloseHandle(IntPtr hObject);
		}

		/// <summary>
		/// This interface definition is HACKED.
		/// 
		/// It appears that the IID for IXpsPrintJobStream specified in XpsPrint.h as 
		/// MIDL_INTERFACE("7a77dc5f-45d6-4dff-9307-d8cb846347ca") is not correct and the RCW cannot return it.
		/// But the returned object returns the parent ISequentialStream inteface successfully.
		/// 
		/// So the hack is that we obtain the ISequentialStream interface but work with it as 
		/// with the IXpsPrintJobStream interface. 
		/// </summary>
		[System.Runtime.InteropServices.Guid("0C733A30-2A1C-11CE-ADE5-00AA0044773D")] // This is IID of ISequenatialSteam.
		[System.Runtime.InteropServices.InterfaceType(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIUnknown)]
		interface IXpsPrintJobStream
		{
			// ISequentualStream methods.
			void Read([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPArray)] byte[] pv, uint cb, out uint pcbRead);

			void Write([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPArray)] byte[] pv, uint cb, out uint pcbWritten);

			// IXpsPrintJobStream methods.
			void Close();
		}

		[System.Runtime.InteropServices.Guid("5ab89b06-8194-425f-ab3b-d7a96e350161")]
		[System.Runtime.InteropServices.InterfaceType(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIUnknown)]
		interface IXpsPrintJob
		{
			void Cancel();
			void GetJobStatus(out XPS_JOB_STATUS jobStatus);
		}

		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
		struct XPS_JOB_STATUS
		{
			public UInt32 jobId;
			public Int32 currentDocument;
			public Int32 currentPage;
			public Int32 currentPageTotal;
			public XPS_JOB_COMPLETION completion;
			public Int32 jobStatus; // UInt32
		};

		enum XPS_JOB_COMPLETION
		{
			XPS_JOB_IN_PROGRESS = 0,
			XPS_JOB_COMPLETED = 1,
			XPS_JOB_CANCELLED = 2,
			XPS_JOB_FAILED = 3
		}

		enum WAIT_RESULT
		{
			WAIT_OBJECT_0 = 0,
			WAIT_ABANDONED = 0x80,
			WAIT_TIMEOUT = 0x102,
			WAIT_FAILED = -1 // 0xFFFFFFFF
		}
	}
}