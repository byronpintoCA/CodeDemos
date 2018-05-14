var myApp = angular.module("Vehicle", []);

myApp.factory('VehicleDataFactory', function ($http) {

    var factory = {};

    factory.vm = {};

    factory.Version = "v1";

    factory.GetList = function () {

        var theUrl = '/' + factory.Version + "/Vehicles";
        $http({
            method: "GET",
            url: theUrl

        }).success(function (data) {
            factory.vm.ListDataRecieved(data);

        }).error(function () {
            factory.vm.RaiseInformation("Error Retrieving Data Vehicle List : GET");
        });
    }

    factory.GetPage = function (page,limit) {
        var theUrl = '/' + factory.Version + "/Vehicles?page=" + page +'&limit=' + limit;
        $http({
            method: "GET",
            url: theUrl

        }).success(function (data) {
            factory.vm.ListDataRecieved(data);

        }).error(function () {
            factory.vm.RaiseInformation("Error Retrieving Data Vehicle List : GET");
        });
    }


    factory.Get = function (id) {
        var theUrl = '/' + factory.Version + "/Vehicles/" + id;
        $http({
            method: "GET",
            url: theUrl

        }).success(function (data) {
            factory.vm.SelectRow(data);
            factory.vm.RaiseInformation('Retrieved record');

        }).error(function () {
            factory.vm.RaiseError("Error Retrieving Data Vehicle List : GET");
        });
    }

    factory.Add = function (vm) {

        var vehicle = new Object();
        vehicle.Id = 0;
        vehicle.Manufacturer = vm.Manufacturer;
        vehicle.Model = vm.Model;
        vehicle.Year = vm.Year;

        var theUrl = '/' + factory.Version + "/Vehicles";
        $http({
            url: theUrl,
            method: "POST",
            dataType: 'json',
            data: vehicle,

        }).success(function (data) {
            factory.vm.SelectRow(data);
            factory.vm.RaiseInformation('Added record');

        }).error(function () {
            factory.vm.RaiseError("Error Updating Data Vehicle List : PUT");
        });

    }
    factory.Update = function (vm) {
        var vehicle = new Object();
        vehicle.Id = vm.Id;
        vehicle.Manufacturer = vm.Manufacturer;
        vehicle.Model = vm.Model;
        vehicle.Year = vm.Year;

        var theUrl = '/' + factory.Version + "/Vehicles";
        $http({
            url: theUrl,
            method: "PUT",
            dataType: 'json',
            data: vehicle,

        }).success(function (data) {
            factory.vm.SelectRow(data);
            factory.vm.RaiseInformation('Updated record');

        }).error(function () {
            factory.vm.RaiseError("Error Updating Data Vehicle List : PUT");
        });
    }

    factory.Delete = function (id) {

        var theUrl = '/' + factory.Version + "/Vehicles/" + id ;
        $http({
            url: theUrl,
            method: "DELETE",

        }).success(function (data) {
            var vehicle = new Object();
            vehicle.Id = '';
            vehicle.Manufacturer ='';
            vehicle.Model = '';
            vehicle.Year ='';
            factory.vm.SelectRow(vehicle);
            factory.vm.RaiseInformation('Deleted record');

        }).error(function () {
            factory.vm.RaiseError("Error Deleting Data Vehicle List : DELETE");
        });
    }

    return factory;

});


myApp.controller("VehicleController", function (VehicleDataFactory) {

    var vm = this;
    vm.Version = "";
    vm.Id = "";
    vm.Manufacturer = "";
    vm.Model = "";
    vm.Year = "";
    vm.Info = "";
    vm.Error = "";
    vm.List = {};
    vm.ListDataRecievedCallback = function () { };
    vm.RefreshUI = function() {};

    vm.SetVersion = function (vers) {
        VehicleDataFactory.Version = vers;
    }

    vm.GetList = function () {
        VehicleDataFactory.GetList();
    }
    vm.GetPage = function (page,limit) {
        VehicleDataFactory.GetPage(page,limit);
    }

    vm.ListDataRecieved = function (data) {
        vm.List = data;
        vm.ListDataRecievedCallback(data);
    }

    vm.SelectRow = function (rec) {
        vm.Id = rec.Id;
        vm.Manufacturer = rec.Manufacturer;
        vm.Model = rec.Model;
        vm.Year = rec.Year;
        vm.Error = '';
        vm.Info = '';
        vm.RefreshUI();
    }

    vm.Clear = function () {
        vm.Id = '';
        vm.Manufacturer = '';
        vm.Model = '';
        vm.Year = '';
        vm.Error = '';
        vm.Info = '';
        vm.RefreshUI();
    }

    vm.RaiseInformation = function (info) {
        vm.Info = info;
        vm.Error = '';
        vm.RefreshUI();
    }

    vm.RaiseError = function (error) {
        vm.Info = '';
        vm.Error = error;
        vm.RefreshUI();
    }

    vm.Get = function () {
        VehicleDataFactory.Get(vm.Id);
    }

    vm.Add = function () {
        VehicleDataFactory.Add(vm);
    }
    vm.Update = function () {
        VehicleDataFactory.Update(vm);
    }

    vm.Delete = function (id) {
        VehicleDataFactory.Delete(vm.Id);
    }

    VehicleDataFactory.vm = vm;

});