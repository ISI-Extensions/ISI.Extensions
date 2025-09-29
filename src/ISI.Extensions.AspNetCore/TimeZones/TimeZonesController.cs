﻿#region Copyright & License
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

namespace ISI.Extensions.AspNetCore
{
	public class TimeZonesController : Microsoft.AspNetCore.Mvc.Controller
	{
		public class Routes
		{
			public const string TimeZonesJavascript_js = "TimeZonesJavascript_js-61d08795-fad3-4b10-b5b1-5aa3fcef787e";
		}

		public class Urls
		{
			public const string TimeZonesJavascript_js = "/javascripts/time-zones-javascript.js";
		}

		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public TimeZonesController(
			Microsoft.Extensions.Logging.ILogger logger)
		{
			Logger = logger;
		}


		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Get))]
		[Microsoft.AspNetCore.Authorization.AllowAnonymous]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.TimeZonesJavascript_js, Urls.TimeZonesJavascript_js)]
		[Microsoft.AspNetCore.Mvc.ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> TimeZonesJavascriptAsync()
		{
			var content = new StringBuilder();

			content.AppendLine(@"
/*
MIT License 

Copyright (c) 2012 Jon Nylander, project maintained at 
https://bitbucket.org/pellepim/jstimezonedetect

Permission is hereby granted, free of charge, to any person obtaining a copy 
of this software and associated documentation files (the ""Software""), to deal 
in the Software without restriction, including without limitation the rights to 
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
of the Software, and to permit persons to whom the Software is furnished to 
do so, subject to the following conditions: 

The above copyright notice and this permission notice shall be included in 
all copies or substantial portions of the Software. 

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
THE SOFTWARE.
*/

/*jslint undef: true */
/*global console, exports*/

(function(root) {
	/**
	 * Namespace to hold all the code for timezone detection.
	 */
	var jstz = (function() {
		'use strict';
		var HEMISPHERE_SOUTH = 's',
		    /**
					 * Gets the offset in minutes from UTC for a certain date.
					 * @@param {Date} date
					 * @@returns {Number}
					 */
		    get_date_offset = function(date) {
			    var offset = -date.getTimezoneOffset();
			    return (offset !== null ? offset : 0);
		    },
		    get_date = function(year, month, date) {
			    var d = new Date();
			    if (year !== undefined) {
				    d.setFullYear(year);
			    }
			    d.setDate(date);
			    d.setMonth(month);
			    return d;
		    },
		    get_january_offset = function(year) {
			    return get_date_offset(get_date(year, 0, 2));
		    },
		    get_june_offset = function(year) {

			    return get_date_offset(get_date(year, 5, 2));
		    },
		    /**
					 * Private method.
					 * Checks whether a given date is in daylight savings time.
					 * If the date supplied is after august, we assume that we're checking
					 * for southern hemisphere DST.
					 * @@param {Date} date
					 * @@returns {Boolean}
					 */
		    date_is_dst = function(date) {
			    var base_offset = ((date.getMonth() > 7 ? get_june_offset(date.getFullYear())
				    : get_january_offset(date.getFullYear()))),
			        date_offset = get_date_offset(date);


			    return (base_offset - date_offset) !== 0;
		    },
		    /**
					 * This function does some basic calculations to create information about
					 * the user's timezone.
					 *
					 * Returns a key that can be used to do lookups in jstz.olson.timezones.
					 *
					 * @@returns {String}
					 */

		    lookup_key = function() {
			    var january_offset = get_january_offset(),
			        june_offset = get_june_offset(),
			        diff = get_january_offset() - get_june_offset();

			    if (diff < 0) {
				    return january_offset + "",1"";
			    } else if (diff > 0) {
				    return june_offset + "",1,"" + HEMISPHERE_SOUTH;
			    }

			    return january_offset + "",0"";
		    },
		    /**
					 * Uses get_timezone_info() to formulate a key to use in the olson.timezones dictionary.
					 *
					 * Returns a primitive object on the format:
					 * {'timezone': TimeZone, 'key' : 'the key used to find the TimeZone object'}
					 *
					 * @@returns Object
					 */
		    determine = function() {
			    var key = lookup_key();
			    return new jstz.TimeZone(jstz.olson.timezones[key]);
		    },					
		    /**
					 * This object contains information on when daylight savings starts for
					 * different timezones.
					 *
					 * The list is short for a reason. Often we do not have to be very specific
					 * to single out the correct timezone. But when we do, this list comes in
					 * handy.
					 *
					 * Each value is a date denoting when daylight savings starts for that timezone.
					 */
		    dst_start_for = function(tz_name) {

			    var ru_pre_dst_change = new Date(2010, 6, 15, 1, 0, 0, 0), // In 2010 Russia had DST, this allows us to detect Russia :)
			        dst_starts = {
				        'America/Denver': new Date(2011, 2, 13, 3, 0, 0, 0),
				        'America/Mazatlan': new Date(2011, 3, 3, 3, 0, 0, 0),
				        'America/Chicago': new Date(2011, 2, 13, 3, 0, 0, 0),
				        'America/Mexico_City': new Date(2011, 3, 3, 3, 0, 0, 0),
				        'America/Asuncion': new Date(2012, 9, 7, 3, 0, 0, 0),
				        'America/Santiago': new Date(2012, 9, 3, 3, 0, 0, 0),
				        'America/Campo_Grande': new Date(2012, 9, 21, 5, 0, 0, 0),
				        'America/Montevideo': new Date(2011, 9, 2, 3, 0, 0, 0),
				        'America/Sao_Paulo': new Date(2011, 9, 16, 5, 0, 0, 0),
				        'America/Los_Angeles': new Date(2011, 2, 13, 8, 0, 0, 0),
				        'America/Santa_Isabel': new Date(2011, 3, 5, 8, 0, 0, 0),
				        'America/Havana': new Date(2012, 2, 10, 2, 0, 0, 0),
				        'America/New_York': new Date(2012, 2, 10, 7, 0, 0, 0),
				        'Asia/Beirut': new Date(2011, 2, 27, 1, 0, 0, 0),
				        'Europe/Helsinki': new Date(2011, 2, 27, 4, 0, 0, 0),
				        'Europe/Istanbul': new Date(2011, 2, 28, 5, 0, 0, 0),
				        'Asia/Damascus': new Date(2011, 3, 1, 2, 0, 0, 0),
				        'Asia/Jerusalem': new Date(2011, 3, 1, 6, 0, 0, 0),
				        'Asia/Gaza': new Date(2009, 2, 28, 0, 30, 0, 0),
				        'Africa/Cairo': new Date(2009, 3, 25, 0, 30, 0, 0),
				        'Pacific/Auckland': new Date(2011, 8, 26, 7, 0, 0, 0),
				        'Pacific/Fiji': new Date(2010, 11, 29, 23, 0, 0, 0),
				        'America/Halifax': new Date(2011, 2, 13, 6, 0, 0, 0),
				        'America/Goose_Bay': new Date(2011, 2, 13, 2, 1, 0, 0),
				        'America/Miquelon': new Date(2011, 2, 13, 5, 0, 0, 0),
				        'America/Godthab': new Date(2011, 2, 27, 1, 0, 0, 0),
				        'Europe/Moscow': ru_pre_dst_change,
				        'Asia/Yekaterinburg': ru_pre_dst_change,
				        'Asia/Omsk': ru_pre_dst_change,
				        'Asia/Krasnoyarsk': ru_pre_dst_change,
				        'Asia/Irkutsk': ru_pre_dst_change,
				        'Asia/Yakutsk': ru_pre_dst_change,
				        'Asia/Vladivostok': ru_pre_dst_change,
				        'Asia/Kamchatka': ru_pre_dst_change,
				        'Europe/Minsk': ru_pre_dst_change,
				        'Australia/Perth': new Date(2008, 10, 1, 1, 0, 0, 0)
			        };

			    return dst_starts[tz_name];
		    };

		return {
			determine: determine,
			date_is_dst: date_is_dst,
			dst_start_for: dst_start_for
		};
	}());

	/**
	 * Simple object to perform ambiguity check and to return name of time zone.
	 */
	jstz.TimeZone = function(tz_name) {
		'use strict';
		/**
				 * The keys in this object are timezones that we know may be ambiguous after
				 * a preliminary scan through the olson_tz object.
				 *
				 * The array of timezones to compare must be in the order that daylight savings
				 * starts for the regions.
				 * 
				 * @@TODO: Once 2013 is upon us, remove Asia/Gaza from the Beirut ambiguity list,
				 * by then it should suffice that it lives in the Africa/Johannesburg check.
				 */
		var AMBIGUITIES = {
			'America/Denver': ['America/Denver', 'America/Mazatlan'],
			'America/Chicago': ['America/Chicago', 'America/Mexico_City'],
			'America/Santiago': ['America/Santiago', 'America/Asuncion', 'America/Campo_Grande'],
			'America/Montevideo': ['America/Montevideo', 'America/Sao_Paulo'],
			'Asia/Beirut': ['Asia/Beirut', 'Europe/Helsinki', 'Europe/Istanbul', 'Asia/Damascus', 'Asia/Jerusalem', 'Asia/Gaza'],
			'Pacific/Auckland': ['Pacific/Auckland', 'Pacific/Fiji'],
			'America/Los_Angeles': ['America/Los_Angeles', 'America/Santa_Isabel'],
			'America/New_York': ['America/Havana', 'America/New_York'],
			'America/Halifax': ['America/Goose_Bay', 'America/Halifax'],
			'America/Godthab': ['America/Miquelon', 'America/Godthab'],
			'Asia/Dubai': ['Europe/Moscow'],
			'Asia/Dhaka': ['Asia/Yekaterinburg'],
			'Asia/Jakarta': ['Asia/Omsk'],
			'Asia/Shanghai': ['Asia/Krasnoyarsk', 'Australia/Perth'],
			'Asia/Tokyo': ['Asia/Irkutsk'],
			'Australia/Brisbane': ['Asia/Yakutsk'],
			'Pacific/Noumea': ['Asia/Vladivostok'],
			'Pacific/Tarawa': ['Asia/Kamchatka'],
			'Africa/Johannesburg': ['Asia/Gaza', 'Africa/Cairo'],
			'Asia/Baghdad': ['Europe/Minsk']
		},
		    timezone_name = tz_name,
		    /**
					 * Checks if a timezone has possible ambiguities. I.e timezones that are similar.
					 *
					 * For example, if the preliminary scan determines that we're in America/Denver.
					 * We double check here that we're really there and not in America/Mazatlan.
					 *
					 * This is done by checking known dates for when daylight savings start for different
					 * timezones during 2010 and 2011.
					 */
		    ambiguity_check = function() {
			    var ambiguity_list = AMBIGUITIES[timezone_name],
			        length = ambiguity_list.length,
			        i = 0,
			        tz = ambiguity_list[0];

			    for (; i < length; i += 1) {
				    tz = ambiguity_list[i];

				    if (jstz.date_is_dst(jstz.dst_start_for(tz))) {
					    timezone_name = tz;
					    return;
				    }
			    }
		    },
		    /**
					 * Checks if it is possible that the timezone is ambiguous.
					 */
		    is_ambiguous = function() {
			    return typeof(AMBIGUITIES[timezone_name]) !== 'undefined';
		    };

		if (is_ambiguous()) {
			ambiguity_check();
		}

		return {
			name: function() {
				return timezone_name;
			}
		};
	};

	jstz.olson = {};

	/*
	 * The keys in this dictionary are comma separated as such:
	 *
	 * First the offset compared to UTC time in minutes.
	 *
	 * Then a flag which is 0 if the timezone does not take daylight savings into account and 1 if it
	 * does.
	 *
	 * Thirdly an optional 's' signifies that the timezone is in the southern hemisphere,
	 * only interesting for timezones with DST.
	 *
	 * The mapped arrays is used for constructing the jstz.TimeZone object from within
	 * jstz.determine_timezone();
	 */
	jstz.olson.timezones = {
		'-720,0': 'Etc/GMT+12',
		'-660,0': 'Pacific/Pago_Pago',
		'-600,1': 'America/Adak',
		'-600,0': 'Pacific/Honolulu',
		'-570,0': 'Pacific/Marquesas',
		'-540,0': 'Pacific/Gambier',
		'-540,1': 'America/Anchorage',
		'-480,1': 'America/Los_Angeles',
		'-480,0': 'Pacific/Pitcairn',
		'-420,0': 'America/Phoenix',
		'-420,1': 'America/Denver',
		'-360,0': 'America/Guatemala',
		'-360,1': 'America/Chicago',
		'-360,1,s': 'Pacific/Easter',
		'-300,0': 'America/Bogota',
		'-300,1': 'America/New_York',
		'-270,0': 'America/Caracas',
		'-240,1': 'America/Halifax',
		'-240,0': 'America/Santo_Domingo',
		'-240,1,s': 'America/Santiago',
		'-210,1': 'America/St_Johns',
		'-180,1': 'America/Godthab',
		'-180,0': 'America/Argentina/Buenos_Aires',
		'-180,1,s': 'America/Montevideo',
		'-120,0': 'Etc/GMT+2',
		'-120,1': 'Etc/GMT+2',
		'-60,1': 'Atlantic/Azores',
		'-60,0': 'Atlantic/Cape_Verde',
		'0,0': 'Etc/UTC',
		'0,1': 'Europe/London',
		'60,1': 'Europe/Berlin',
		'60,0': 'Africa/Lagos',
		'60,1,s': 'Africa/Windhoek',
		'120,1': 'Asia/Beirut',
		'120,0': 'Africa/Johannesburg',
		'180,0': 'Asia/Baghdad',
		'180,1': 'Europe/Moscow',
		'210,1': 'Asia/Tehran',
		'240,0': 'Asia/Dubai',
		'240,1': 'Asia/Baku',
		'270,0': 'Asia/Kabul',
		'300,1': 'Asia/Yekaterinburg',
		'300,0': 'Asia/Karachi',
		'330,0': 'Asia/Kolkata',
		'345,0': 'Asia/Kathmandu',
		'360,0': 'Asia/Dhaka',
		'360,1': 'Asia/Omsk',
		'390,0': 'Asia/Rangoon',
		'420,1': 'Asia/Krasnoyarsk',
		'420,0': 'Asia/Jakarta',
		'480,0': 'Asia/Shanghai',
		'480,1': 'Asia/Irkutsk',
		'525,0': 'Australia/Eucla',
		'525,1,s': 'Australia/Eucla',
		'540,1': 'Asia/Yakutsk',
		'540,0': 'Asia/Tokyo',
		'570,0': 'Australia/Darwin',
		'570,1,s': 'Australia/Adelaide',
		'600,0': 'Australia/Brisbane',
		'600,1': 'Asia/Vladivostok',
		'600,1,s': 'Australia/Sydney',
		'630,1,s': 'Australia/Lord_Howe',
		'660,1': 'Asia/Kamchatka',
		'660,0': 'Pacific/Noumea',
		'690,0': 'Pacific/Norfolk',
		'720,1,s': 'Pacific/Auckland',
		'720,0': 'Pacific/Tarawa',
		'765,1,s': 'Pacific/Chatham',
		'780,0': 'Pacific/Tongatapu',
		'780,1,s': 'Pacific/Apia',
		'840,0': 'Pacific/Kiritimati'
	};

	if (typeof exports !== 'undefined') {
		exports.jstz = jstz;
	} else {
		root.jstz = jstz;
	}
})(this);");

			var expires = $"{DateTime.UtcNow.AddYears(1):r}";

			content.AppendLine();
			content.AppendLine("jQuery(document).ready(function () {");
			content.AppendLine($"\tdocument.cookie = \"@(ISI.Libraries.Web.Mvc.Extensions.TimeZoneHelpers.TimeOffsetCookieName)=\" + (-(new Date()).getTimezoneOffset() / 60) + \"; expires={expires}; path=/\";");
			content.AppendLine($"\tdocument.cookie = \"@(ISI.Libraries.Web.Mvc.Extensions.TimeZoneHelpers.TimeZoneCookieName)=\" + jstz.determine().name() + \"; expires={expires}; path=/\";");
			content.AppendLine("});");

			return Content(content.ToString(), ISI.Extensions.WebClient.Rest.ContentTypeJavascriptHeaderValue);
		}
	}
}