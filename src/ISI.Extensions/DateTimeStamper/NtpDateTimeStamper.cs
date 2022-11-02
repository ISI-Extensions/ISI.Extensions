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
using System.Text;

namespace ISI.Extensions.DateTimeStamper
{
	public class NtpDateTimeStamper : IDateTimeStamper
	{
		protected ISI.Extensions.DateTimeStamper.Configuration Configuration { get; }

		private static readonly object _sourceRefreshLock = new();

		private static DateTime _sourceLastRefresh = DateTime.MinValue;
		private static TimeSpan _sourceToLocalMachineOffset;

		[ThreadStatic]
		internal static DateTimeBubble _DateTimeBubble = null;

		public NtpDateTimeStamper(
			ISI.Extensions.DateTimeStamper.Configuration configuration)
		{
			Configuration = configuration;
		}

		public DateTime CurrentDateTime()
		{
			return CurrentDateTime(true);
		}

		public DateTime CurrentDateTimeUtc()
		{
			return _DateTimeBubble?.UtcNow ?? DateTime.SpecifyKind(CurrentDateTime(false), DateTimeKind.Utc);
		}

		public ISI.Extensions.DateTimeStamper.DateTimeBubble DateTimeBubble()
		{
			return _DateTimeBubble ??= new(CurrentDateTime(false), CurrentDateTime(true), () => { _DateTimeBubble = null; });
		}

		public ISI.Extensions.DateTimeStamper.DateTimeBubble DateTimeBubble(ISI.Extensions.DateTimeStamper.DateTimeBubble dateTimeBubble)
		{
			return (_DateTimeBubble = new(dateTimeBubble.UtcNow, dateTimeBubble.Now, () => { _DateTimeBubble = null; }));
		}

		private DateTime CurrentDateTime(bool useLocalDateTime)
		{
			if (_DateTimeBubble == null)
			{
				if (_sourceLastRefresh + Configuration.NtpServerRefreshInterval < DateTime.Now)
				{
					lock (_sourceRefreshLock)
					{
						if (_sourceLastRefresh + Configuration.NtpServerRefreshInterval < DateTime.Now)
						{
							try
							{
								_sourceToLocalMachineOffset = DateTime.UtcNow - GetNetworkTime();
							}
#pragma warning disable CS0168 // Variable is declared but never used
							catch (Exception exception)
#pragma warning restore CS0168 // Variable is declared but never used
							{
							}

							_sourceLastRefresh = DateTime.Now;
						}
					}
				}

				return DateTime.SpecifyKind((useLocalDateTime ? DateTime.Now : DateTime.UtcNow) + _sourceToLocalMachineOffset, (useLocalDateTime ? DateTimeKind.Local : DateTimeKind.Utc));
			}

			return (useLocalDateTime ? _DateTimeBubble.Now : _DateTimeBubble.UtcNow);
		}

		//http://stackoverflow.com/questions/1193955/how-to-query-an-ntp-server-using-c

		private DateTime GetNetworkTime()
		{
			// NTP message size - 16 bytes of the digest (RFC 2030)
			var ntpData = new byte[48];

			//Setting the Leap Indicator, Version Number and Mode values
			ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

			var addresses = System.Net.Dns.GetHostEntry(Configuration.NtpServer).AddressList;

			//The UDP port number assigned to NTP is 123
			var ipEndPoint = new System.Net.IPEndPoint(addresses[0], 123);
			//NTP uses UDP
			var socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);

			socket.Connect(ipEndPoint);

			//Stops code hang if NTP is blocked
			socket.ReceiveTimeout = 3000;

			socket.Send(ntpData);
			socket.Receive(ntpData);
			socket.Close();

			//Offset to get to the "Transmit Timestamp" field (time at which the reply 
			//departed the server for the client, in 64-bit timestamp format."
			const byte serverReplyTime = 40;

			//Get the seconds part
			ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

			//Get the seconds fraction
			ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

			//Convert From big-endian to little-endian
			intPart = SwapEndianness(intPart);
			fractPart = SwapEndianness(fractPart);

			var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

			//**UTC** time
			var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

			return networkDateTime;
		}

		// stackoverflow.com/a/3294698/162671
		private static uint SwapEndianness(ulong x)
		{
			return (uint)(((x & 0x000000ff) << 24) + ((x & 0x0000ff00) << 8) + ((x & 0x00ff0000) >> 8) + ((x & 0xff000000) >> 24));
		}
	}
}
