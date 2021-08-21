#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.MessageBus
{
	public class MessageBusBuildRequestCollection : List<MessageBusBuildRequest>
	{
		public static ISI.Extensions.MessageBus.Configuration Configuration { get; set; }
		public static IDictionary<string, ISI.Extensions.MessageBus.Configuration.ChannelConfiguration> NamedChannelConfigurations { get; set; }

		public MessageBusBuildRequest DefaultChannelMessageBusBuildRequest { get; } = new MessageBusBuildRequest(null, Array.Empty<Action<IMessageBusConfigurator>>())
		{
			ChannelPath = Configuration.DefaultChannel.ChannelPath,
			ConcurrentConsumerLimit = Configuration.DefaultChannel.ConcurrentConsumerLimit,
			RetryLimit = Configuration.DefaultChannel.RetryLimit,
			RetryInterval = Configuration.DefaultChannel.RetryInterval,
			AutoDelete = Configuration.DefaultChannel.AutoDelete,
			PurgeOnStartup = Configuration.DefaultChannel.PurgeOnStartup,
		};

		public void Add(IEnumerable<Action<IMessageBusConfigurator>> addSubscriptions)
		{
			DefaultChannelMessageBusBuildRequest.AddSubscriptions.AddRange(addSubscriptions);
		}

		public void Add(Action<IMessageBusConfigurator> addSubscriptions)
		{
			Add(new[] { addSubscriptions });
		}

		public void Add(Func<IEnumerable<IMessageBusBuildRequest>> addSubscriptions)
		{
			foreach (var messageBusBuildRequest in addSubscriptions())
			{
				Add(messageBusBuildRequest);
			}
		}

		public void AddUsingNamedChannelConfiguration(string channelConfigurationName, IEnumerable<Action<IMessageBusConfigurator>> addSubscriptions)
		{
			AddUsingNamedChannelConfiguration(channelConfigurationName, channelConfigurationName, addSubscriptions);
		}
		public void AddUsingNamedChannelConfiguration(string channelConfigurationName, string channelName, IEnumerable<Action<IMessageBusConfigurator>> addSubscriptions)
		{
			var messageBusBuildRequest = new NamedChannelConfigurationMessageBusBuildRequest()
			{
				ChannelConfigurationName = channelConfigurationName,
				ChannelName = channelName,
			};

			messageBusBuildRequest.AddSubscriptions.AddRange(addSubscriptions);

			Add(messageBusBuildRequest);
		}

		public void AddUsingNamedChannelConfigurationWithChannelPathSuffix(string channelConfigurationName, string channelPathSuffix, IEnumerable<Action<IMessageBusConfigurator>> addSubscriptions)
		{
			AddUsingNamedChannelConfigurationWithChannelPathSuffix(channelConfigurationName, channelConfigurationName, channelPathSuffix, addSubscriptions);
		}
		public void AddUsingNamedChannelConfigurationWithChannelPathSuffix(string channelConfigurationName, string channelName, string channelPathSuffix, IEnumerable<Action<IMessageBusConfigurator>> addSubscriptions)
		{
			var messageBusBuildRequest = new NamedChannelConfigurationChannelPathSuffixMessageBusBuildRequest()
			{
				ChannelConfigurationName = channelConfigurationName,
				ChannelName = channelName,
				ChannelPathSuffix = channelPathSuffix,
			};

			messageBusBuildRequest.AddSubscriptions.AddRange(addSubscriptions);

			Add(messageBusBuildRequest);
		}

		public void AddUsingChannelPath(string channelName, string channelPath, IEnumerable<Action<IMessageBusConfigurator>> addSubscriptions, int? concurrentConsumerLimit = null, int retryLimit = 0, bool autoDelete = false, bool purgeOnStartup = false)
		{
			var messageBusBuildRequest = new ChannelPathMessageBusBuildRequest()
			{
				ChannelName = channelName,
				ChannelPath = channelPath,
				ConcurrentConsumerLimit = concurrentConsumerLimit,
				RetryLimit = retryLimit,
				AutoDelete = autoDelete,
				PurgeOnStartup = purgeOnStartup,
			};

			messageBusBuildRequest.AddSubscriptions.AddRange(addSubscriptions);

			Add(messageBusBuildRequest);
		}

		public void AddUsingChannelPathSuffix(string channelName, string channelPathSuffix, IEnumerable<Action<IMessageBusConfigurator>> addSubscriptions, int? concurrentConsumerLimit = null, int retryLimit = 0, bool autoDelete = false, bool purgeOnStartup = false)
		{
			var messageBusBuildRequest = new ChannelPathSuffixMessageBusBuildRequest()
			{
				ChannelName = channelName,
				ChannelPathSuffix = channelPathSuffix,
				ConcurrentConsumerLimit = concurrentConsumerLimit,
				RetryLimit = retryLimit,
				AutoDelete = autoDelete,
				PurgeOnStartup = purgeOnStartup,
			};

			messageBusBuildRequest.AddSubscriptions.AddRange(addSubscriptions);

			Add(messageBusBuildRequest);
		}

		public void Add(IMessageBusBuildRequest messageBusBuildRequest)
		{
			switch (messageBusBuildRequest)
			{
				case DefaultMessageBusBuildRequest defaultMessageBusBuildRequest:
					DefaultChannelMessageBusBuildRequest.AddSubscriptions.AddRange(defaultMessageBusBuildRequest.AddSubscriptions);
					break;

				case NamedChannelConfigurationMessageBusBuildRequest namedChannelConfigurationMessageBusBuildRequest:
					{
						if (NamedChannelConfigurations.TryGetValue(namedChannelConfigurationMessageBusBuildRequest.ChannelConfigurationName, out var namedChannelConfiguration))
						{
							Add(new MessageBusBuildRequest(namedChannelConfigurationMessageBusBuildRequest.ChannelName, namedChannelConfigurationMessageBusBuildRequest.AddSubscriptions)
							{
								ChannelPath = namedChannelConfiguration.ChannelPath,
								ConcurrentConsumerLimit = namedChannelConfiguration.ConcurrentConsumerLimit,
								RetryLimit = namedChannelConfiguration.RetryLimit,
								RetryInterval = namedChannelConfiguration.RetryInterval,
								AutoDelete = namedChannelConfiguration.AutoDelete,
								PurgeOnStartup = namedChannelConfiguration.PurgeOnStartup,
							});
						}
						else
						{
							throw new Exception(string.Format("Named Channel Configuration: \"{0}\" not found", namedChannelConfigurationMessageBusBuildRequest.ChannelConfigurationName));
						}

						break;
					}

				case NamedChannelConfigurationChannelPathSuffixMessageBusBuildRequest namedChannelConfigurationChannelPathSuffixMessageBusBuildRequest:
					{
						if (NamedChannelConfigurations.TryGetValue(namedChannelConfigurationChannelPathSuffixMessageBusBuildRequest.ChannelConfigurationName, out var namedChannelConfiguration))
						{
							Add(new MessageBusBuildRequest(namedChannelConfigurationChannelPathSuffixMessageBusBuildRequest.ChannelName, namedChannelConfigurationChannelPathSuffixMessageBusBuildRequest.AddSubscriptions)
							{
								ChannelPath = string.Format("{0}{1}{2}", namedChannelConfiguration.ChannelPath, (namedChannelConfigurationChannelPathSuffixMessageBusBuildRequest.ChannelPathSuffix.StartsWith("-") ? string.Empty : "-"), namedChannelConfigurationChannelPathSuffixMessageBusBuildRequest.ChannelPathSuffix),
								ConcurrentConsumerLimit = namedChannelConfiguration.ConcurrentConsumerLimit,
								RetryLimit = namedChannelConfiguration.RetryLimit,
								RetryInterval = namedChannelConfiguration.RetryInterval,
								AutoDelete = namedChannelConfiguration.AutoDelete,
								PurgeOnStartup = namedChannelConfiguration.PurgeOnStartup,
							});
						}
						else
						{
							throw new Exception(string.Format("Named Channel Configuration: \"{0}\" not found", namedChannelConfigurationChannelPathSuffixMessageBusBuildRequest.ChannelConfigurationName));
						}

						break;
					}

				case ChannelPathMessageBusBuildRequest channelPathMessageBusBuildRequest:
					{
						Add(new MessageBusBuildRequest(channelPathMessageBusBuildRequest.ChannelName, channelPathMessageBusBuildRequest.AddSubscriptions)
						{
							ChannelPath = channelPathMessageBusBuildRequest.ChannelPath,
							ConcurrentConsumerLimit = channelPathMessageBusBuildRequest.ConcurrentConsumerLimit ?? Configuration.DefaultChannel.ConcurrentConsumerLimit,
							RetryLimit = channelPathMessageBusBuildRequest.RetryLimit ?? Configuration.DefaultChannel.RetryLimit,
							RetryInterval = channelPathMessageBusBuildRequest.RetryInterval ?? Configuration.DefaultChannel.RetryInterval,
							AutoDelete = channelPathMessageBusBuildRequest.AutoDelete ?? Configuration.DefaultChannel.AutoDelete,
							PurgeOnStartup = channelPathMessageBusBuildRequest.PurgeOnStartup ?? Configuration.DefaultChannel.PurgeOnStartup,
						});

						break;
					}

				case ChannelPathSuffixMessageBusBuildRequest channelPathSuffixMessageBusBuildRequest:
					{
						Add(new MessageBusBuildRequest(channelPathSuffixMessageBusBuildRequest.ChannelName, channelPathSuffixMessageBusBuildRequest.AddSubscriptions)
						{
							ChannelPath = string.Format("{0}{1}{2}", Configuration.DefaultChannel.ChannelPath, (channelPathSuffixMessageBusBuildRequest.ChannelPathSuffix.StartsWith("-") ? string.Empty : "-"), channelPathSuffixMessageBusBuildRequest.ChannelPathSuffix),
							ConcurrentConsumerLimit = channelPathSuffixMessageBusBuildRequest.ConcurrentConsumerLimit ?? Configuration.DefaultChannel.ConcurrentConsumerLimit,
							RetryLimit = channelPathSuffixMessageBusBuildRequest.RetryLimit ?? Configuration.DefaultChannel.RetryLimit,
							RetryInterval = channelPathSuffixMessageBusBuildRequest.RetryInterval ?? Configuration.DefaultChannel.RetryInterval,
							AutoDelete = channelPathSuffixMessageBusBuildRequest.AutoDelete ?? Configuration.DefaultChannel.AutoDelete,
							PurgeOnStartup = channelPathSuffixMessageBusBuildRequest.PurgeOnStartup ?? Configuration.DefaultChannel.PurgeOnStartup,
						});

						break;
					}
			}
		}

		public new void Add(MessageBusBuildRequest messageBusBuildRequest)
		{
			var existingMessageBusStartUpRequest = this.FirstOrDefault(x => string.Equals(x.ChannelPath, messageBusBuildRequest.ChannelPath, StringComparison.InvariantCulture));

			if (existingMessageBusStartUpRequest != null)
			{
				if (!existingMessageBusStartUpRequest.IsSameDefinition(messageBusBuildRequest))
				{
					throw new Exception(string.Format("Duplicate definitions for ChannelPath: \"{0}\"", messageBusBuildRequest.ChannelPath));
				}

				existingMessageBusStartUpRequest.MergeIn(messageBusBuildRequest);
			}
			else
			{
				base.Add(messageBusBuildRequest);
			}
		}
	}
}
