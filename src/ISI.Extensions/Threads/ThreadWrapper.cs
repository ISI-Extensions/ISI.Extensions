#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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

namespace ISI.Extensions
{
	public partial class Threads
	{
		public abstract class ThreadWrapper : IThreadWrapper
		{
			protected Microsoft.Extensions.Logging.ILogger Logger { get; }

			private System.Threading.Thread _thread = null;
			private System.Threading.CancellationTokenSource _cancellationTokenSource = null;

			public bool IsActive { get; private set; }

			protected ThreadWrapper(
				Microsoft.Extensions.Logging.ILogger logger)
			{
				Logger = logger;
				OnAppExit += Finish;
			}

			public virtual void Execute()
			{
				_thread = new(Start);
				_cancellationTokenSource = new();

				IsActive = true;

				//_thread.Start(System.Diagnostics.Trace.CorrelationManager);
				_thread.Start();
			}

			protected void Start()
			{
				var operationKey = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens);

				System.Diagnostics.Trace.CorrelationManager.StartLogicalOperation(operationKey);

				// Set thread name to same as operationKey so that log4net will record the id for each log entry
				if (string.IsNullOrWhiteSpace(System.Threading.Thread.CurrentThread.Name))
				{
					System.Threading.Thread.CurrentThread.Name = operationKey;
				}

				Run(_cancellationTokenSource.Token);

				IsActive = false;

				System.Diagnostics.Trace.CorrelationManager.StopLogicalOperation();
			}

			protected abstract void Run(System.Threading.CancellationToken cancellationToken);
			//while (!cancellationToken.IsCancellationRequested)
			//{

			//}

			public void Finish()
			{
				if (_thread != null)
				{
					_cancellationTokenSource?.Cancel();
					if (!_thread.Join(10000))
					{
						Abort();
					}
					else
					{
						_thread = null;
					}
				}

				_cancellationTokenSource?.Dispose();
				_cancellationTokenSource = null;
			}

			public void Abort()
			{
				_cancellationTokenSource?.Cancel();

				_thread?.Abort();
				_thread = null;

				_cancellationTokenSource?.Dispose();
				_cancellationTokenSource = null;
			}
		}
	}
}