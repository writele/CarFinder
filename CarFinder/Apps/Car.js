var app = angular.module('appCar', ['ui.router']);

  app.config(function($routeProvider){
    $routeProvider
      .when("/", {
        templateUrl: "ViewCar.html",
        controller: "CarController"
      });
      //.otherwise({redirectTo:"/"});
  });