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
using System.Threading.Tasks;

namespace ISI.Extensions.Extensions
{
	public static class TaskExtensions
	{
		public static void Forget(this Task task)
		{
			// note: this code is inspired by a tweet from Ben Adams: https://twitter.com/ben_a_adams/status/1045060828700037125
			// Only care about tasks that may fault (not completed) or are faulted,
			// so fast-path for SuccessfullyCompleted and Canceled tasks.
			if (!task.IsCompleted || task.IsFaulted)
			{
				// use "_" (Discard operation) to remove the warning IDE0058: Because this call is not awaited, execution of the current method continues before the call is completed
				// https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/discards?WT.mc_id=DT-MVP-5003978#a-standalone-discard
				_ = ForgetAwaited(task);
			}

			// Allocate the async/await state machine only when needed for performance reasons.
			// More info about the state machine: https://blogs.msdn.microsoft.com/seteplia/2017/11/30/dissecting-the-async-methods-in-c/?WT.mc_id=DT-MVP-5003978
			async static Task ForgetAwaited(Task task)
			{
				try
				{
					// No need to resume on the original SynchronizationContext, so use ConfigureAwait(false)
					await task.ConfigureAwait(false);
				}
				catch
				{
					// Nothing to do here
				}
			}
		}
	}
}
