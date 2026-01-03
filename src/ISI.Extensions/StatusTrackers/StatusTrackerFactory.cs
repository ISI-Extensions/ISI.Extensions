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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.StatusTrackers
{
	public class MemoryStatusTrackerFactory : StatusTrackerFactory<StatusTracker>
	{
	}

	public class StatusTrackerFactory<TStatusTracker> : IStatusTrackerFactory
		where TStatusTracker : IStatusTracker, new()
	{
		protected readonly Dictionary<string, TStatusTracker> StatusTrackers = new(StringComparer.InvariantCultureIgnoreCase);
		protected readonly Dictionary<string, (bool Successful, DateTime FinishedDateTimeUtc)> FinishedStatusTrackers = new(StringComparer.InvariantCultureIgnoreCase);
		
		public bool TryGetStatusTracker(string statusTrackerKey, out IStatusTracker statusTracker)
		{
			if (StatusTrackers.TryGetValue(statusTrackerKey, out var existingStatusTracker))
			{
				statusTracker = existingStatusTracker;
				return true;
			}

			statusTracker = null;
			return false;
		}

		public IStatusTracker CreateStatusTracker(string statusTrackerKey)
		{
			var expiredStatusTrackerKeys = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

			foreach (var finishedStatusTracker in FinishedStatusTrackers)
			{
				if (finishedStatusTracker.Value.FinishedDateTimeUtc < DateTime.UtcNow.AddDays(-4))
				{
					expiredStatusTrackerKeys.Add(finishedStatusTracker.Key);
				}
			}

			foreach (var expiredStatusTrackerKey in expiredStatusTrackerKeys)
			{
				StatusTrackers.Remove(expiredStatusTrackerKey);
				FinishedStatusTrackers.Remove(expiredStatusTrackerKey);
			}

			var statusTracker = new TStatusTracker();

			statusTracker.SetOnFinished(successful => FinishedStatusTrackers[statusTrackerKey] = (Successful: successful, FinishedDateTimeUtc: DateTime.UtcNow));

			return statusTracker;
		}

		public bool IsRunning(string statusTrackerKey) => (StatusTrackers.ContainsKey(statusTrackerKey) && !FinishedStatusTrackers.ContainsKey(statusTrackerKey));

		public bool IsFinished(string statusTrackerKey) => FinishedStatusTrackers.ContainsKey(statusTrackerKey);

		public bool IsSuccessful(string statusTrackerKey)
		{
			if (FinishedStatusTrackers.TryGetValue(statusTrackerKey, out var isFinishedResponse))
			{
				return isFinishedResponse.Successful;
			}

			return false;
		}

		public bool TryStatusTrackerGetKeyValue(string statusTrackerKey, string key, out string value)
		{
			if (StatusTrackers.TryGetValue(statusTrackerKey, out var statusTracker))
			{
				if (statusTracker.KeyValues.TryGetValue(key, out value))
				{
					return true;
				}
			}

			value = null;
			return false;
		}

		public IEnumerable<string> GetActiveStatusTrackerKeys()
		{
			var statusTrackerKeys = new HashSet<string>(StatusTrackers.Keys);
			statusTrackerKeys.RemoveWhere(statusTrackerKey => FinishedStatusTrackers.ContainsKey(statusTrackerKey));

			return statusTrackerKeys;
		}

		public IStatusTrackerSnapshot GetStatusTrackerSnapshot(string statusTrackerKey)
		{
			if (StatusTrackers.TryGetValue(statusTrackerKey, out var statusTracker))
			{
				return new StatusTrackerSnapshot(statusTracker);
			}

			return new StatusTrackerSnapshot();
		}

		public IStatusTrackerSnapshot[] GetStatusTrackerSnapshots(IEnumerable<string> statusTrackerKeys)
		{
			var statusTrackers = new List<IStatusTrackerSnapshot>();

			foreach (var statusTrackerKey in statusTrackerKeys)
			{
				var statusTracker = GetStatusTrackerSnapshot(statusTrackerKey);

				if (statusTracker != null)
				{
					statusTrackers.Add(statusTracker);
				}
			}

			return statusTrackers.ToArray();
		}

		public IStatusTrackerSnapshot[] GetActiveStatusTrackerSnapshots() => GetStatusTrackerSnapshots(GetActiveStatusTrackerKeys());

		public void DeleteStatusTracker(string statusTrackerKey)
		{
			StatusTrackers.Remove(statusTrackerKey);
			FinishedStatusTrackers.Remove(statusTrackerKey);
		}
	}
}
