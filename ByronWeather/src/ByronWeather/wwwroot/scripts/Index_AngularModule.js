var myApp = angular.module("WeatherLanding", []);

myApp.factory('LocationDataFactory', function ($http) {

    var factory = {};

    factory.vm = {};

    factory.PositionSelected = function (latitude, longitude) {

        var theUrl = "https://nominatim.openstreetmap.org/reverse?format=json&lat=" + latitude + "&lon=" + longitude + "&zoom=18&addressdetails=1";
        $http({
            method: "GET",
            url: theUrl

        }).success(function (data) {

            factory.vm.PositionFoundCallback(latitude, longitude, data);
            //scope.$apply();


        }).error(function () {
            EndAnimation();
            alert("error getting Location Data");
        });
    }

    factory.GetWeatherData = function (latitude, longitude) {

        var theUrl = "/Home/WeatherFromCoordinates?&latitude=" + latitude + "&longitude=" + longitude;
        $http({
            method: "GET",
            url: theUrl

        }).success(function (data) {
            factory.vm.WeatherDataCallback(data);
            //scope.$apply();
        }).error(function () {
            EndAnimation();
            alert("error getting Location Data");
        });

    }

    factory.GetGeoLocationData = function () {
        BeginAnimationAuto('DrawingArea');
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(PositionFound, PositionError);
        }
        else {
            EndAnimation();
            alert('no geo location');

        }

        function PositionError(error) {
            EndAnimation();
            factory.vm.GeoLocationError(error);
            
            //alert(error.message);
        }

        function PositionFound(position) {
            factory.PositionSelected(position.coords.latitude, position.coords.longitude)
        }


    };

    return factory;

});


myApp.controller("WeatherLocationController", function (LocationDataFactory) {

    var vm = this;
    vm.latitude = "Detecting";
    vm.longitude = "Detecting";
    vm.city = "Not Selected",
    vm.county = "Not Selected",
    vm.state = "Not Selected",
    vm.country = "Not Selected",
    vm.country_code = "Not Selected"
    vm.elevation = "Detecting";
    vm.status = "";
    vm.Forecast = {};
    vm.iconLocation = "None";
    LocationDataFactory.vm = vm;


    vm.locationSearchCallback = function (data) {

        LocationDataFactory.PositionSelected(data.latitude, data.longitude);
        //alert(data);
    }


    vm.GeoLocationError = function (error) {

        //alert(locationData);
        this.latitude = error.message;
        this.longitude = error.message;
        this.elevation = error.message;
        //EndAnimation();
    }

    vm.PositionFoundCallback = function PositionFound(latitude , longitude ,locationData) {

        //alert(locationData);
        this.latitude = latitude;
        this.longitude = longitude;
        this.city = locationData.address.city;
        this.county = locationData.address.county;
        this.state = locationData.address.state;
        this.country = locationData.address.country;
        this.country_code = locationData.address.country_code;
        this.status = "Retrieving Data..";
        
        LocationDataFactory.GetWeatherData(latitude, longitude);
        //EndAnimation();
    }

    vm.WeatherDataCallback = function (weatherData) {

        //alert(weatherData);
        this.Forecast = weatherData.Forecast.period;
        this.elevation = weatherData.Forecast['@elevation'];
        this.iconLocation = weatherData.Forecast['icon-location'];
        this.status = "";
        //this.city = locationData.address.city;
        EndAnimation();
    }


    vm.StartAnimation = function () {
        BeginAnimationAuto('DrawingArea');
    };

    vm.TerminateAnimation = function () {
        EndAnimation();
    }

    LocationDataFactory.GetGeoLocationData();
  
});