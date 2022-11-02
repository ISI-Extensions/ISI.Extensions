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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Svn.DataTransferObjects.MessageParser;

namespace ISI.Extensions.Svn
{
	public partial class MessageParser
	{
		public DTOs.ParseMessageResponse ParseMessage(string message)
		{
			var response = new DTOs.ParseMessageResponse();

			if (!string.IsNullOrWhiteSpace(message))
			{
				bool mightBeJiraTicket(string key)
				{
					var pieces = key.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);

					if ((pieces.Length == 2) && (pieces[1].ToInt() > 0) && (pieces[0].Length <= 4) && string.Equals(pieces[0], pieces[0].ToUpper(), StringComparison.Ordinal))
					{
						return true;
					}

					return false;
				}

				var tracTicketNumbers = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
				var jiraIssueIdOrKeys = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

				var parsedMessage = new DTOs.ParsedMessage();

				var parsedMessages = new List<DTOs.ParsedMessage>();
				parsedMessages.Add(parsedMessage);

				var words = new Queue<string>(message.Replace("(", " (").Replace("#", " #").Split(new[] { ' ', ':', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries));

				string cleanTicketNumber(string ticketNumber) => ticketNumber.Trim(' ', '#', ',');

				while (words.Any())
				{
					var word = words.Dequeue();

					if (TicketActions.TryGetValue(word, out var ticketAction))
					{
						if (parsedMessage.Action.HasValue)
						{
							parsedMessage.TracTicketNumbers = tracTicketNumbers.ToArray();
							tracTicketNumbers.Clear();

							parsedMessage.JiraIssueIdOrKeys = jiraIssueIdOrKeys.ToArray();
							jiraIssueIdOrKeys.Clear();

							parsedMessage = new();
							parsedMessages.Add(parsedMessage);
						}

						parsedMessage.Action = ticketAction;
					}
					else if (word.StartsWith("#") && mightBeJiraTicket(word))
					{
						jiraIssueIdOrKeys.Add(cleanTicketNumber(word));
					}
					else if (word.StartsWith("#"))
					{
						tracTicketNumbers.Add(cleanTicketNumber(word));
					}
					else if (word.StartsWith("(") && word.EndsWith(")") && double.TryParse(word.Trim(' ', '(', ')', '\t'), out var hours))
					{
						parsedMessage.TimeSpent = TimeSpan.FromHours(hours);
					}
					else if (mightBeJiraTicket(word))
					{
						jiraIssueIdOrKeys.Add(cleanTicketNumber(word));
					}
				}

				parsedMessage.TracTicketNumbers = tracTicketNumbers.ToArray();
				parsedMessage.JiraIssueIdOrKeys = jiraIssueIdOrKeys.ToArray();

				response.ParsedMessages = parsedMessages.ToArray();
			}

			return response;
		}
	}
}