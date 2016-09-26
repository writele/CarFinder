(function(){

    var carData = function ($http) {

        getCarData = function () {
            return $http.get('/api/Car/getCar?year=2014&make=Kia&Model=Soul&Trim=4dr%20Wagon%20(1.6L%204cyl%206A)')
            .then(function (response) {
                return response.data;
            });
        };

        return {
            getCarData: getCarData
        };
    }

    var app = angular.module('appCar', ['ui.router']);
    app.factory("carData", carData);

}());