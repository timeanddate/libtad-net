Time And Date .NET API
======================================

### Note that this is currently a Release Candidate no commercial support will be provided by Time and Date at this time!

Time and Date APIs support looking up several different locations and IDs. As of API version 2 the following variations is availalble

* Numeric ID (e.g. 187)
* Textual ID (e.g. "usa/las-vegas")
* Coordinates (e.g. "+59.743+10.204")
* Airports (e.g. "a-ENZV")

The class LocationId is used to set the appropriate location ID. Airports categorize the same way as textual ID.

 
Astronomy Service
--------------------------------------
  
Get astronomy information from place on date by textual ID:
         
         var place = new LocationId("usa/anchorage");
         var date = new DateTime(2015, 1, 1);
         var service = new AstronomyService('accessKey', 'secretKey');
         var astroInfo = service.GetAstronomicalInfo(AstronomyObjectType.Sun, place, date);
         
Get astronomy information from place between dates by numeric ID:
 
         var place = new LocationId(187);
         var startDate = new DateTime(2015, 1, 1);
         var endDate = new DateTime(2015, 1, 30);
         var service = new AstronomyService('accessKey', 'secretKey');
         var astroInfo = service.GetAstronomicalInfo(AstronomyObjectType.Moon, place, startDate, endDate);

Retrieve specific astronomy events by coordinates:

        var coordinates = new Coordinates(59.743m, 10.204m);
        var place = new LocationId(coordinates);
        var date = new DateTime(2015, 1, 1);
        var service = new AstronomyService('accessKey', 'secretKey');

        service.Types = AstronomyEventClass.Meridian | AstronomyEventClass.NauticalTwilight;

        var astroInfo = service.GetAstronomicalInfo(AstronomyObjectType.Moon, place, startDate, endDate);

Other options:

        // Adds the DateTime-object ISOTime to every astronomical day
        service.IncludeISOTime = true;

        // Adds the DateTime-object UTCTime to every astronomical day
        service.IncludeUTCTime = true;

        // Adds a search radius if GetAstronomicalInfo is used with coordinates
        service.Radius = 50; // km


Convert Time Service
--------------------------------------

Convert time from a location:

        var place = new LocationId("norway/oslo");
        var date = DateTime.Now;
        var service = new ConvertTimeService('accessKey', 'secretKey');
        var convertedTime = service.ConvertTime(place, date);

Convert time from a location using an ISO 8601-string:

        ...
        var convertedTime = service.ConvertTime(place, "2015-04-21T16:45:00");

Convert time from one location to multiple locations:

        var listOfLocations = new List<LocationId>();
        listOfLocations.Add(new LocationId("usa/las-vegas"));
        listOfLocations.Add(new LocationId(179);
        
        var place = new LocationId("oslo/norway");
        var service = new ConvertTimeService('accessKey', 'secretKey');
        var result = service.ConvertTime(place, DateTime.Now, listOfLocations);

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

Get daylight saving time for specified year:

        var service = new DSTService('accessKey', 'secretKey');
        var result = service.GetDaylightSavingTime(2014);

Get daylight saving time for specified ISO 639 country code:

        var service = new DSTService('accessKey', 'secretKey');
        var result = service.GetDaylightSavingTime(2014);

Get daylight saving time for specified ISO 639 country code and year:

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

Get dial code to a location:

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

Get all holidays for a country by ISO 630 country code:

        var country = "no";
        var service = new HolidaysService('accessKey', 'secretKey');
        var result = service.GetHolidaysForCountry(country);

Get all holidays for a country by year and ISO 630 country code:

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

Get all places in Time and Date (these can be used to lookup data in other services):

        var service = new  PlacesService('accessKey', 'secretKey')
        var result = service.GetPlaces();

Other options:

        // Do not include coordinates in return value
        service.IncludeCoordinates = false;

Time Service
--------------------------------------

Get current time for place:

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


Location data type:
--------------------------------------

Get UTC offset from local time (only applicable if service.IncludeListOfTimeChanges has been set to true):

		DateTimeOffset localTime = new DateTimeOffset(2015, 6, 7, 0);
		Location sampleLoc = result.FirstOrDefault();
		TimeSpan offset = sampleLoc.GetUTCOffsetFromLocalTime(localTime);



