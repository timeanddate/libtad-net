Time And Date .NET API
======================================
[![Build Status](https://app.travis-ci.com/timeanddate/libtad-net.svg?branch=master)](https://app.travis-ci.com/timeanddate/libtad-net)

Time and Date APIs support looking up several different locations and IDs. As of API version 2 the following variations are available:

* Numeric ID (e.g. 187)
* Textual ID (e.g. "usa/las-vegas")
* Coordinates (e.g. "+59.743+10.204")
* Airports (e.g. "a-ENZV")

The class [LocationId](http://services.timeanddate.com/api/doc/v2/type-locationid.html) is used to set the appropriate location ID. Airports categorize the same way as textual ID.

An access key and a secret key is required to use the API. If you are not already a Time and Date API user, please see our [API offers](https://services.timeanddate.com/free-trial) to get a free 3 month trial. For more information, see our [API Services page](https://dev.timeanddate.com/).

Developed with Mono C# Compiler 4.0.1.0 

Astronomy Service
--------------------------------------
  
Get astronomy information for a place on a date by textual ID:
         
         var place = new LocationId("usa/anchorage");
         var date = new TADDateTime(2015, 1, 1);
         var service = new AstronomyService('accessKey', 'secretKey');
         var astroInfo = service.GetAstronomicalInfo(AstronomyObjectType.Sun, place, date);
         
Get astronomy information for a place between two dates by numeric ID:
 
         var place = new LocationId(187);
         var startDate = new TADDateTime(2015, 1, 1);
         var endDate = new TADDateTime(2015, 1, 30);
         var service = new AstronomyService('accessKey', 'secretKey');
         var astroInfo = service.GetAstronomicalInfo(AstronomyObjectType.Moon, place, startDate, endDate);

Retrieve specific astronomy events by coordinates:

        var coordinates = new Coordinates(59.743m, 10.204m);
        var place = new LocationId(coordinates);
        var date = new TADDateTime(2015, 1, 1);
        var service = new AstronomyService('accessKey', 'secretKey');

        service.Types = AstronomyEventClass.Meridian | AstronomyEventClass.NauticalTwilight;

        var astroInfo = service.GetAstronomicalInfo(AstronomyObjectType.Moon, place, startDate, endDate);

Other options:

        // Adds the TADDateTime-object ISOTime to every astronomical day
        service.IncludeISOTime = true;

        // Adds the TADDateTime-object UTCTime to every astronomical day
        service.IncludeUTCTime = true;

        // Adds a search radius if GetAstronomicalInfo is used with coordinates
        service.Radius = 50; // km

Astrodata Service
--------------------------------------
  
Get astronomical position for a place on a date by textual ID:
         
         var place = new LocationId("usa/anchorage");
         var date = new TADDateTime(2015, 1, 1);
         var service = new AstroDataService('accessKey', 'secretKey');
         var astroInfo = service.GetAstroData(AstronomyObjectType.Sun, place, date);
         
Get astronomical position for a place for two date intervals:
 
         var place = new LocationId(187);
	 var list = new List<TADDateTime> ();
         list.Add (new TADDateTime(2015, 1, 1));
         list.Add (new TADDateTime(2015, 1, 30));
         var service = new AstrodataService('accessKey', 'secretKey');
         var astroData = service.GetAstroData(AstronomyObjectType.Moon, place, list);

Get astronomical position for multiple astronomical objects:

	var place = new Coordinates(57.743m, 10.204m);
	AstronomyObjectType object = AstronomyObjectType.Sun | AstronomyObjectType.Moon;
	var service = new AstrodataService('accessKey', 'secretKey');
	var astroData = service.GetAstrodata(object, place, new TADDateTime(2021, 5, 3));

Other options:
	
	// Consider the specified intervals to be local time (UTC if false)
	service.LocalTime = true;

        // Adds the TADDateTime-object ISOTime to every astronomical day
        service.IncludeISOTime = true;

        // Adds the TADDateTime-object UTCTime to every astronomical day
        service.IncludeUTCTime = true;

        // Adds a search radius if GetAstrodata is used with coordinates
        service.Radius = 50; // km


Convert Time Service
--------------------------------------

Convert time from a location:

        var place = new LocationId("norway/oslo");
        var date = new TADDateTime(DateTime.Now);
        var service = new ConvertTimeService('accessKey', 'secretKey');
        var convertedTime = service.ConvertTime(place, date);

Convert time from a location using an [ISO 8601](https://dev.timeanddate.com/docs/external-references#ISO8601)-string:

        ...
        var convertedTime = service.ConvertTime(place, "2015-04-21T16:45:00");

Convert time from one location to multiple locations:

        var listOfLocations = new List<LocationId>();
        listOfLocations.Add(new LocationId("usa/las-vegas"));
        listOfLocations.Add(new LocationId(179);
        
        var place = new LocationId("oslo/norway");
        var service = new ConvertTimeService('accessKey', 'secretKey');
        var result = service.ConvertTime(place, new TADDateTime(DateTime.Now), listOfLocations);

Other options:

        // Add TimeChanges for each location
        service.IncludeTimeChanges = true;

        // Add Timezone information for each location
        service.IncludeTimezoneInformation = true;

        // Search for a place by a specified radius
        service.Radius = 50; // km


Daylight Saving Time Service
--------------------------------------

Get all daylight saving times:

        var service = new DSTService('accessKey', 'secretKey');
        var allDST = service.GetDaylightSavingTime();

Get daylight saving time for a specified year:

        var service = new DSTService('accessKey', 'secretKey');
        var result = service.GetDaylightSavingTime(2014);

Get daylight saving time for a specified [ISO3166-1 (Alpha2)](https://dev.timeanddate.com/docs/external-references#ISO3166) country code:

        var service = new DSTService('accessKey', 'secretKey');
        var result = service.GetDaylightSavingTime("no");

Get daylight saving time for a specified [ISO3166-1 (Alpha2)](https://dev.timeanddate.com/docs/external-references#ISO3166) country code and year:

        var service = new DSTService('accessKey', 'secretKey');
        var result = service.GetDaylightSavingTime("no", 2014);

   
Other options:
       
       // Add TimeChanges to each location
       service.IncludeTimeChanges = true;

       // Return only countries which have DST
       service.IncludeOnlyDstCountries = true;

       // Add locations for every country
       service.IncludePlacesForEveryCountry = true;

        

Dial Code Service
--------------------------------------

Get dial code for a location:

        var osloId = new LocationId("norway/oslo");
        var service = new DialCodeService('accessKey', 'secretKey');
        var result = service.GetDialCode(osloId);

Get dial code to a location, from a location:

        var osloId = new LocationId("norway/oslo")
        var newYorkId = new LocationId("usa/new-york");
        var service = new DialCodeService('accessKey', 'secretKey');
        var result = service.GetDialCode(osloId, newYorkId);

Get dial code results with a local number:

        var osloId = new LocationId("norway/oslo")
        var newYorkId = new LocationId("usa/new-york");
        var number = 51515151;
        var service = new DialCodeService('accessKey', 'secretKey');
        var result = service.GetDialCode(osloId, newYorkId, number);

Other options:

        // Do not include locations in return value
        service.IncludeLocations = false;

        // Do not include current time in return value
        service.IncludeCurrentTime = false;

        // Do not include coordinates to locations in return value
        service.IncludeCoordinates = false;

        // Do not include Timezone Information in return value
        service.IncludeTimezoneInformation = false;

Holidays Service
--------------------------------------

Get all holidays for a country by [ISO3166-1 (Alpha2)](https://dev.timeanddate.com/docs/external-references#ISO3166) country code:

        var country = "no";
        var service = new HolidaysService('accessKey', 'secretKey');
        var result = service.GetHolidaysForCountry(country);

Get all holidays for a country by year and [ISO3166-1 (Alpha2)](https://dev.timeanddate.com/docs/external-references#ISO3166) country code:

        var country = "no";
        var year = 2014;
        var service = new HolidaysService('accessKey', 'secretKey');
        var result = service.GetHolidaysForCountry(country, 2014);

Get specific holidays for a country:

        var country = "no";
        var service = new HolidaysService('accessKey', 'secretKey');
        service.Types = HolidayType.Federal | HolidayType.Weekdays;
        var result = service.GetHolidaysForCountry(country);

Places Service
--------------------------------------

Get all places in Time and Date (these can be used to look up data in other services):

        var service = new  PlacesService('accessKey', 'secretKey')
        var result = service.GetPlaces();

Other options:

        // Do not include coordinates in return value
        service.IncludeCoordinates = false;

Time Service
--------------------------------------

Get current time for a place:

        var place = new LocationId(179);
        var service = new TimeService('accessKey', 'secretKey');
        var result = service.GetCurrentTimeForPlace(place);

Other options:

        // Limit the number of responses
        service.Limit = 5;

        // Limit the search radius when using coordinates
        service.Radius = 50; // km

        // Do not add coordinates to location in return value
        service.IncludeCoordinates = false;

        // Do not add sunrise and sunset for location in return value
        service.IncludeSunriseSunset = false;

        // Do not add list of time changes in return value
        service.IncludeListOfTimeChanges = false;

        // Do not add timezone information to return value
        service.IncldueTimezoneInformation = false;


On This Day Service
--------------------------------------

Notice: This API is not released yet. Constructing this service will fail until it's available.

Get events, births and deaths for a month and day:
	
	var month = 5;
	var day = 24;
	var service = new OnThisDayService('accessKey', 'secretKey');
	var result = service.EventsOnThisDay(month, day);

Get events, births and deaths for todays month and day:

	var result = service.EventsOnThisDay();

Get a specific type of event:

	service.Types = EventType.Deaths;
	var result = service.EventsOnThisDay();

Get a combination of events:

	service.Types = EventType.Events | EventType.Births;
	var result = service.EventsOnThisDay();
