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

namespace ISI.Extensions.Locks
{
	public class FileLock : ISI.Extensions.Locks.ILock
	{
		public string FileName { get; }
		public string DirectoryName { get; }
		protected System.IO.Stream FileStream { get; private set; }

		public FileLock(string fileName, TimeSpan? retryInterval = null, TimeSpan? failInterval = null, Action onWaitingForLock = null, Action<string> onCreatingLock = null)
		{
			FileName = GetLockFileName(fileName);
			DirectoryName = System.IO.Path.GetDirectoryName(FileName);
			FileStream = null;

			CreateFileStream(retryInterval.GetValueOrDefault(TimeSpan.FromSeconds(5)), failInterval, onWaitingForLock, onCreatingLock);
		}

		protected virtual void CreateFileStream(TimeSpan retryInterval, TimeSpan? failInterval, Action onWaitingForLock = null, Action<string> onCreatingLock = null)
		{
			onCreatingLock?.Invoke(FileName);

			var autoResetEvent = new System.Threading.AutoResetEvent(false);

			var now = DateTime.UtcNow;

			while (FileStream == null)
			{
				try
				{
					FileStream = new System.IO.FileStream(FileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None, 4096, System.IO.FileOptions.DeleteOnClose);
				}
				catch (System.IO.IOException exception)
				{
					if (failInterval.HasValue && (DateTime.UtcNow > now + failInterval.Value))
					{
						throw new ISI.Extensions.Locks.LockException(string.Format("Failed to get lock on \"{0}\"", FileName), exception);
					}

					onWaitingForLock?.Invoke();

					System.IO.Directory.CreateDirectory(DirectoryName);

					var fileSystemWatcher = new System.IO.FileSystemWatcher(DirectoryName)
					{
						EnableRaisingEvents = true
					};

					fileSystemWatcher.Changed += (o, fileSystemEvent) =>
						{
							if (System.IO.Path.GetFullPath(fileSystemEvent.FullPath) == DirectoryName)
							{
								autoResetEvent.Set();
							}
						};

					autoResetEvent.WaitOne(retryInterval);
				}
			}
		}

		protected string GetLockFileName(string fileName)
		{
			return string.Format("{0}.lock", fileName);
		}

		public void Dispose()
		{
			FileStream?.Dispose();
			FileStream = null;
		}

		public static void Lock(string fileName, Action action, TimeSpan? retryInterval = null, TimeSpan? failInterval = null, Action onWaitingForLock = null, Action<string> onCreatingLock = null)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				throw new NullReferenceException("fileName cannot be null or empty");
			}

			if (action == null)
			{
				throw new NullReferenceException("action cannot be null");
			}

			using (new FileLock(fileName, retryInterval, failInterval, onWaitingForLock, onCreatingLock))
			{
				action();
			}
		}
	}
}
