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
using global::MassTransit;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.MessageBus.MassTransit
{
	public partial class MessageBus
	{
		private static readonly object _publishRequestClientWrapperCacheLock = new();
		private static readonly Dictionary<string, IPublishRequestClientWrapper> _publishRequestClientWrapperCache = new();

		private IPublishRequestClientWrapper GetPublishRequestClientWrapper(Type requestType, Type responseType, TimeSpan? timeout, TimeSpan? timeToLive = null)
		{
			timeout ??= Configuration.DefaultResponseTimeOut ?? DefaultResponseTimeOut;
			timeToLive ??= Configuration.DefaultResponseTimeToLive;

			if(!Configuration.CachePublishRequestClientWrapper)
			{
				var publishRequestClientWrapperType = typeof(PublishRequestClientWrapper<,>).MakeGenericType(requestType, responseType);

				return Activator.CreateInstance(publishRequestClientWrapperType, timeout.GetValueOrDefault(), timeToLive) as IPublishRequestClientWrapper;
			}

			var key = string.Format("{0}|{1}|{2}|{3}", requestType.FullName, responseType.FullName, timeout.GetValueOrDefault().Ticks, timeToLive.GetValueOrDefault().Ticks);
			
			if (!_publishRequestClientWrapperCache.TryGetValue(key, out var publishRequestClientWrapper))
			{
				lock (_publishRequestClientWrapperCacheLock)
				{
					if (!_publishRequestClientWrapperCache.TryGetValue(key, out publishRequestClientWrapper))
					{
						var publishRequestClientWrapperType = typeof(PublishRequestClientWrapper<,>).MakeGenericType(requestType, responseType);

						publishRequestClientWrapper = Activator.CreateInstance(publishRequestClientWrapperType, Configuration, Logger, timeout.GetValueOrDefault(), timeToLive) as IPublishRequestClientWrapper;

						_publishRequestClientWrapperCache.Add(key, publishRequestClientWrapper);
					}
				}
			}

			return publishRequestClientWrapper;
		}

		private interface IPublishRequestClientWrapper
		{

		}

		private interface IPublishRequestClientWrapper<TResponse> : IPublishRequestClientWrapper
			where TResponse : class
		{
			Task<TResponse> GetResponse(IBus busControl, object request, IEnumerable<MessageBusMessageHeader> headers, System.Threading.CancellationToken cancellationToken);
		}

		private class PublishRequestClientWrapper<TRequest, TResponse> : IPublishRequestClientWrapper<TResponse>
			where TRequest : class
			where TResponse : class
		{
			protected ISI.Extensions.MessageBus.Configuration Configuration { get; }
			protected Microsoft.Extensions.Logging.ILogger Logger { get; }

			private readonly object _clientLock = new();
			private global::MassTransit.IRequestClient<TRequest> _client = null;

			private readonly TimeSpan _timeout;
			private readonly TimeSpan? _timeToLive;

			public PublishRequestClientWrapper(ISI.Extensions.MessageBus.Configuration configuration, Microsoft.Extensions.Logging.ILogger logger, TimeSpan timeout, TimeSpan? timeToLive)
			{
				Configuration = configuration;
				Logger = logger;

				_timeout = timeout;
				_timeToLive = timeToLive;
			}

			public async Task<TResponse> GetResponse(IBus busControl, object request, IEnumerable<MessageBusMessageHeader> headers, System.Threading.CancellationToken cancellationToken)
			{
				if (_client == null)
				{
					if (Configuration.LogPublishRequestClient)
					{
						Logger.LogInformation($"PublishRequestClientWrapper-Get Lock for ${typeof(TRequest).Name}");
					}
					lock (_clientLock)
					{
						if (Configuration.LogPublishRequestClient)
						{
							Logger.LogInformation($"PublishRequestClientWrapper-Get CreateRequestClient for ${typeof(TRequest).Name}");
						}
						_client ??= busControl.CreateRequestClient<TRequest>(_timeout);
					}
				}

				if (Configuration.LogPublishRequestClient)
				{
					Logger.LogInformation($"PublishRequestClientWrapper-Get Create for ${typeof(TRequest).Name}");
				}
				using (var busRequest = _client.Create(request as TRequest, cancellationToken))
				{
					if (Configuration.LogPublishRequestClient)
					{
						Logger.LogInformation($"PublishRequestClientWrapper-Get UseExecute for ${typeof(TRequest).Name}");
					}
					busRequest.UseExecute(context =>
					{
						var operationKey = UpdateRequest(context.Message);

						context.Headers.Set(ISI.Extensions.MessageBus.AbstractMessageBus.RequestTimeOutHeaderKey, _timeout.Formatted(TimeSpanExtensions.TimeSpanFormat.Precise));
						context.Headers.Set(ISI.Extensions.Diagnostics.OperationKeyHeaderKey, operationKey);
						
						foreach (var header in headers ?? [])
						{
							context.Headers.Set(header.Key, header.Value);
						}

						context.CorrelationId = NewId.NextGuid();
					});

					if (_timeToLive.HasValue)
					{
						busRequest.TimeToLive = _timeToLive.Value;
					}

					if (Configuration.LogPublishRequestClient)
					{
						Logger.LogInformation($"PublishRequestClientWrapper-Get GetResponse for ${typeof(TRequest).Name}");
					}

					var busResponse = await busRequest.GetResponse<TResponse>().ConfigureAwait(false);

					var response = busResponse.Message;

					if (Configuration.LogPublishRequestClient)
					{
						Logger.LogInformation($"PublishRequestClientWrapper-Get \"Return\" for ${typeof(TRequest).Name}");
					}

					return response;
				}
			}
		}
	}
}