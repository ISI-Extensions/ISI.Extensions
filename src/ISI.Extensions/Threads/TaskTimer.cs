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
using Microsoft.Extensions.Logging;

namespace ISI.Extensions
{
	public partial class Threads
	{
		public class TaskTimer
		{
			public delegate void OnTimerEvent();

			protected Microsoft.Extensions.Logging.ILogger Logger { get; }

			protected System.Threading.Thread TaskTimerThread { get; private set; }
			protected System.Threading.AutoResetEvent _autoResetEvent { get; private set; }

			protected bool RunImmediately { get; set; }
			protected TimeSpan? StartupDelayTimeSpan { get; set; }

			protected System.Threading.Thread TaskTimerWorkerThread { get; private set; } = null;
			protected TimeSpan? MaxProcessTimeSpan { get; }
			protected TimeSpan TaskTimerInterval { get; }
			protected TimeSpan? ExceptionWaitTimeSpan { get; }
			protected object StartLock { get; } = new();
			public bool Started { get; private set; }
			public bool StopTimer { get; private set; }

			public event OnTimerEvent OnTimer = null;

			public TaskTimer(Microsoft.Extensions.Logging.ILogger logger, TimeSpan interval)
				: this(logger, interval, null, null, null)
			{
			}

			public TaskTimer(Microsoft.Extensions.Logging.ILogger logger, TimeSpan interval, OnTimerEvent onTimer)
				: this(logger, interval, null, null, onTimer)
			{
			}

			public TaskTimer(Microsoft.Extensions.Logging.ILogger logger, TimeSpan interval, TimeSpan? exceptionWaitTimeSpan)
				: this(logger, interval, exceptionWaitTimeSpan, null, null)
			{
			}

			public TaskTimer(Microsoft.Extensions.Logging.ILogger logger, TimeSpan interval, TimeSpan? exceptionWaitTimeSpan, OnTimerEvent onTimer)
				: this(logger, interval, exceptionWaitTimeSpan, null, onTimer)
			{
			}

			public TaskTimer(Microsoft.Extensions.Logging.ILogger logger, TimeSpan interval, TimeSpan? exceptionWaitTimeSpan, TimeSpan? maxProcessTimeSpan)
				: this(logger, interval, exceptionWaitTimeSpan, maxProcessTimeSpan, null)
			{
			}

			public TaskTimer(Microsoft.Extensions.Logging.ILogger logger, TimeSpan interval, TimeSpan? exceptionWaitTimeSpan, TimeSpan? maxProcessTimeSpan, OnTimerEvent onTimer)
				: this(logger, interval, exceptionWaitTimeSpan, maxProcessTimeSpan, null, onTimer)
			{
			}

			public TaskTimer(Microsoft.Extensions.Logging.ILogger logger, TimeSpan interval, TimeSpan? exceptionWaitTimeSpan, TimeSpan? maxProcessTimeSpan, TimeSpan? startupDelayTimeSpan, OnTimerEvent onTimer)
			{
				Logger = logger;
				TaskTimerInterval = interval;
				StartupDelayTimeSpan = startupDelayTimeSpan;
				ExceptionWaitTimeSpan = exceptionWaitTimeSpan;
				MaxProcessTimeSpan = maxProcessTimeSpan;

				if (onTimer != null)
				{
					OnTimer += onTimer;
				}

				ISI.Extensions.Threads.OnAppExit += Stop;
			}

			public virtual void Start(bool runImmediately = false)
			{
				if (!Started)
				{
					lock (StartLock)
					{
						if (!Started)
						{
							RunImmediately = runImmediately;
							Started = true;
							Wakeup();
						}
					}
				}
			}

			public virtual void Stop()
			{
				Stop(TimeSpan.FromMilliseconds(10000));
			}

			public virtual void Stop(TimeSpan joinTimeout)
			{
				if ((TaskTimerThread != null) && !StopTimer)
				{
					StopTimer = true;

					if (TaskTimerThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
					{
						_autoResetEvent.Set();
						//TaskTimerThread.Interrupt();
					}

					_autoResetEvent = null;
					if ((TaskTimerThread.ThreadState != System.Threading.ThreadState.Unstarted))
					{
						TaskTimerThread.Join(joinTimeout);

						TaskTimerWorkerThread?.Abort();
						TaskTimerWorkerThread = null;
					}

					TaskTimerThread = null;
				}

				Started = false;
			}

			public virtual void Wakeup()
			{
				if (OnTimer != null)
				{
					_autoResetEvent ??= new(false);

					TaskTimerThread ??= new(ThreadProcess);

					if ((TaskTimerThread.ThreadState == System.Threading.ThreadState.Unstarted) || (TaskTimerThread.ThreadState == System.Threading.ThreadState.Stopped))
					{
						TaskTimerThread.Start();
					}
					else
					{
						_autoResetEvent.Set();
					}
				}
			}

			protected virtual void ThreadProcess()
			{
				while (!StopTimer)
				{
					if (StartupDelayTimeSpan.HasValue)
					{
						var startupDelayTimeSpan = StartupDelayTimeSpan.Value;

						StartupDelayTimeSpan = null;

						_autoResetEvent.WaitOne(startupDelayTimeSpan);
					}
					else if (OnTimer != null)
					{
						try
						{
							if (RunImmediately)
							{
								if (MaxProcessTimeSpan.HasValue)
								{
									TaskTimerWorkerThread = new(() => OnTimer());

									TaskTimerWorkerThread.Start();

									if (!TaskTimerWorkerThread.Join(MaxProcessTimeSpan.Value))
									{
										TaskTimerWorkerThread.Abort();
									}

									TaskTimerWorkerThread = null;
								}
								else
								{
									OnTimer();
								}
							}
							else
							{
								RunImmediately = true;
							}

							if (!StopTimer)
							{
								_autoResetEvent.WaitOne(TaskTimerInterval);
							}
						}
						catch (Exception exception)
						{
							Logger?.LogError(exception, exception.Message);

							if (ExceptionWaitTimeSpan != null)
							{
								_autoResetEvent.WaitOne(ExceptionWaitTimeSpan.GetValueOrDefault());
							}
						}
					}
				}
			}
		}
	}
}
