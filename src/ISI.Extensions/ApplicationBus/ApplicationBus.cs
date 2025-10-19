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

namespace ISI.Extensions
{
	public class ApplicationBus : IApplicationBus
	{
		private IDictionary<Type, Func<object, object>> _allBusFunctions = null;
		protected IDictionary<Type, Func<object, object>> AllBusFunctions => _allBusFunctions ??= new Dictionary<Type, Func<object, object>>();

		private IDictionary<Type, Func<object, object>> _unnamedBusFunctions = null;
		protected IDictionary<Type, Func<object, object>> UnnamedBusFunctions => _unnamedBusFunctions ??= new Dictionary<Type, Func<object, object>>();

		private IDictionary<string, IDictionary<Type, Func<object, object>>> _namedBusFunctions = null;
		protected IDictionary<string, IDictionary<Type, Func<object, object>>> NamedBusFunctions => _namedBusFunctions ??= new Dictionary<string, IDictionary<Type, Func<object, object>>>();

		private IDictionary<Type, IList<Action<object>>> _allBusActions = null;
		protected IDictionary<Type, IList<Action<object>>> AllBusActions => _allBusActions ??= new Dictionary<Type, IList<Action<object>>>();

		private IDictionary<Type, IList<Action<object>>> _unnamedBusActions = null;
		protected IDictionary<Type, IList<Action<object>>> UnnamedBusActions => _unnamedBusActions ??= new Dictionary<Type, IList<Action<object>>>();

		private IDictionary<string, IDictionary<Type, IList<Action<object>>>> _namedBusActions = null;
		protected IDictionary<string, IDictionary<Type, IList<Action<object>>>> NamedBusActions => _namedBusActions ??= new Dictionary<string, IDictionary<Type, IList<Action<object>>>>();

		private IDictionary<Type, IList<Action<object, object>>> _allBusEvents = null;
		protected IDictionary<Type, IList<Action<object, object>>> AllBusEvents => _allBusEvents ??= new Dictionary<Type, IList<Action<object, object>>>();

		private IDictionary<Type, IList<Action<object, object>>> _unnamedBusEvents = null;
		protected IDictionary<Type, IList<Action<object, object>>> UnnamedBusEvents => _unnamedBusEvents ??= new Dictionary<Type, IList<Action<object, object>>>();

		private IDictionary<string, IDictionary<Type, IList<Action<object, object>>>> _namedBusEvents = null;
		protected IDictionary<string, IDictionary<Type, IList<Action<object, object>>>> NamedBusEvents => _namedBusEvents ??= new Dictionary<string, IDictionary<Type, IList<Action<object, object>>>>();

		public void Subscribe<TRequest, TResponse>(Func<TRequest, TResponse> function, bool includeNamed = false)
			where TRequest : class
			where TResponse : class
		{
			var requestType = typeof(TRequest);

			if (includeNamed && UnnamedBusFunctions.Any(f => f.Key == requestType))
			{
				throw new("Cannot subscribe to All, is already subscribed as unnamed");
			}

			if (includeNamed && NamedBusFunctions.Values.Any(v => v.Any(f => f.Key == requestType)))
			{
				throw new("Cannot subscribe to All, is already subscribed as named");
			}

			var busFunctions = (includeNamed ? AllBusFunctions : UnnamedBusFunctions);

			if (busFunctions.Any(f => f.Key == requestType))
			{
				throw new("Cannot subscribe, is already subscribed");
			}

			busFunctions.Add(requestType, request => function(request as TRequest));
		}

		public void Subscribe<TRequest, TResponse>(string name, Func<TRequest, TResponse> function)
			where TRequest : class
			where TResponse : class
		{
			var requestType = typeof(TRequest);

			if (AllBusFunctions.Any(f => f.Key == requestType))
			{
				throw new("Cannot subscribe, is already subscribed as all");
			}

			if (!NamedBusFunctions.TryGetValue(name, out var functionMaps))
			{
				functionMaps = new Dictionary<Type, Func<object, object>>();
				NamedBusFunctions.Add(name, functionMaps);
			}

			if (functionMaps.Any(f => f.Key == requestType))
			{
				throw new("Cannot subscribe, is already subscribed");
			}

			functionMaps.Add(requestType, request => function(request as TRequest));
		}

		public void Subscribe<TRequest>(Action<TRequest> action, bool includeNamed = false)
			where TRequest : class
		{
			var requestType = typeof(TRequest);

			var busActions = (includeNamed ? AllBusActions : UnnamedBusActions);

			if (!busActions.TryGetValue(requestType, out var actions))
			{
				actions = new List<Action<object>>();
				busActions.Add(requestType, actions);
			}

			actions.Add(request => action(request as TRequest));
		}

		public void Subscribe<TRequest>(string name, Action<TRequest> action)
			where TRequest : class
		{
			if (!NamedBusActions.TryGetValue(name, out var actionMaps))
			{
				actionMaps = new Dictionary<Type, IList<Action<object>>>();
				NamedBusActions.Add(name, actionMaps);
			}

			var requestType = typeof(TRequest);

			if (!actionMaps.TryGetValue(requestType, out var actions))
			{
				actions = new List<Action<object>>();
				actionMaps.Add(requestType, actions);
			}

			actions.Add(request => action(request as TRequest));
		}

		public void Subscribe<TRequest, TResponse>(Action<TRequest, TResponse> action, bool includeNamed = false)
			where TRequest : class
			where TResponse : class
		{
			var requestType = typeof(TRequest);

			var busEvents = (includeNamed ? AllBusEvents : UnnamedBusEvents);

			if (!busEvents.TryGetValue(requestType, out var events))
			{
				events = new List<Action<object, object>>();
				busEvents.Add(requestType, events);
			}

			events.Add((request, response) => action(request as TRequest, response as TResponse));
		}

		public void Subscribe<TRequest, TResponse>(string name, Action<TRequest, TResponse> action)
			where TRequest : class
			where TResponse : class
		{
			var requestType = typeof(TRequest);

			if (!NamedBusEvents.TryGetValue(name, out var eventMaps))
			{
				eventMaps = new Dictionary<Type, IList<Action<object, object>>>();
				NamedBusEvents.Add(name, eventMaps);
			}

			if (!eventMaps.TryGetValue(requestType, out var events))
			{
				events = new List<Action<object, object>>();
				eventMaps.Add(requestType, events);
			}

			events.Add((request, response) => action(request as TRequest, response as TResponse));
		}

		public TResponse Publish<TRequest, TResponse>(TRequest request)
			where TRequest : class
			where TResponse : class
		{
			var response = default(TResponse);

			var requestType = typeof(TRequest);

			{
				if (!AllBusFunctions.TryGetValue(requestType, out var function))
				{
					if (!UnnamedBusFunctions.TryGetValue(requestType, out function))
					{
						response = null;
					}
				}

				if (function != null)
				{
					response = function(request) as TResponse;
				}
			}

			{
				if (AllBusActions.TryGetValue(requestType, out var actions))
				{
					if (actions != null)
					{
						foreach (var action in actions)
						{
							action(request);
						}
					}
				}
				if (UnnamedBusActions.TryGetValue(requestType, out actions))
				{
					if (actions != null)
					{
						foreach (var action in actions)
						{
							action(request);
						}
					}
				}
			}

			{
				if (AllBusEvents.TryGetValue(requestType, out var events))
				{
					if (events != null)
					{
						foreach (var @event in events)
						{
							@event(request, response);
						}
					}
				}
				if (UnnamedBusEvents.TryGetValue(requestType, out events))
				{
					if (events != null)
					{
						foreach (var @event in events)
						{
							@event(request, response);
						}
					}
				}
			}

			return response;
		}

		public TResponse Publish<TRequest, TResponse>(string name, TRequest request)
			where TRequest : class
			where TResponse : class
		{
			var response = default(TResponse);

			var requestType = typeof(TRequest);

			{
				if (!AllBusFunctions.TryGetValue(requestType, out var function))
				{
					if (!NamedBusFunctions.TryGetValue(name, out var functionMaps))
					{
						functionMaps = new Dictionary<Type, Func<object, object>>();
						NamedBusFunctions.Add(name, functionMaps);
					}

					if (!functionMaps.TryGetValue(requestType, out function))
					{
						response = null;
					}
				}

				if (function != null)
				{
					response = function(request) as TResponse;
				}
			}

			{
				if (!AllBusActions.TryGetValue(requestType, out var actions))
				{
					if (!NamedBusActions.TryGetValue(name, out var actionMaps))
					{
						actionMaps = new Dictionary<Type, IList<Action<object>>>();
						NamedBusActions.Add(name, actionMaps);
					}

					if (!actionMaps.TryGetValue(requestType, out actions))
					{
						actions = null;
					}
				}

				if (actions != null)
				{
					foreach (var action in actions)
					{
						action(request);
					}
				}
			}

			{
				if (AllBusEvents.TryGetValue(requestType, out var events))
				{
					if (events != null)
					{
						foreach (var @event in events)
						{
							@event(request, response);
						}
					}
				}

				if (!NamedBusEvents.TryGetValue(name, out var actionMaps))
				{
					actionMaps = new Dictionary<Type, IList<Action<object, object>>>();
					NamedBusEvents.Add(name, actionMaps);
				}
				if (actionMaps.TryGetValue(requestType, out events))
				{
					if (events != null)
					{
						foreach (var @event in events)
						{
							@event(request, response);
						}
					}
				}

			}

			return response;
		}

		public void Publish<TRequest>(TRequest request)
			where TRequest : class
		{
			var requestType = typeof(TRequest);

			if (AllBusActions.TryGetValue(requestType, out var actions))
			{
				if (actions != null)
				{
					foreach (var action in actions)
					{
						action(request);
					}
				}
			}

			if (UnnamedBusActions.TryGetValue(requestType, out actions))
			{
				if (actions != null)
				{
					foreach (var action in actions)
					{
						action(request);
					}
				}
			}
		}

		public void Publish<TRequest>(string name, TRequest request)
			where TRequest : class
		{
			var requestType = typeof(TRequest);

			if (AllBusActions.TryGetValue(requestType, out var actions))
			{
				if (actions != null)
				{
					foreach (var action in actions)
					{
						action(request);
					}
				}
			}

			if (!NamedBusActions.TryGetValue(name, out var actionMaps))
			{
				actionMaps = new Dictionary<Type, IList<Action<object>>>();
				NamedBusActions.Add(name, actionMaps);
			}

			if (actionMaps.TryGetValue(requestType, out actions))
			{
				if (actions != null)
				{
					foreach (var action in actions)
					{
						action(request);
					}
				}
			}
		}
	}
}
