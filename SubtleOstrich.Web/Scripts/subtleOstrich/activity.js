var activityModule = angular.module('activity', ['ngResource', '$strap.directives']).
    config(function($routeProvider) {
        $routeProvider.
            when('/Activity/:date', { controller: ActivityControl, templateUrl: 'list' }).
            when('/Activity/MonthDashboard', { controller: MonthDashboardController }).
            when('/Activity/YearDashboard', { controller: YearDashboardController }).
            otherwise({ redirectTo: '/Activity/' });
    }).factory('Activity', function($resource) {
        var Activity = $resource('Activity/ActivityList', { name: '@Name', date: '@Date', id: '@Id' });

        return Activity;
    }).factory('Typeahead', function ($resource) {
        var Typeahead = $resource('Activity/AutoComplete');
        return Typeahead;
    }).factory('Dashboard', function ($resource) {
        var Dashboard = $resource('Activity/MonthDashboard');

        return Dashboard;
    }).factory('YearDashboard', function($resource) {
        return $resource('Activity/YearDashboard');
    }).factory('updateService', function($rootScope) {
        var updateService = { };

        updateService.broadcastItem = function () {
            $rootScope.$broadcast('handleBroadcast');
        };

        return updateService;
    });

function ActivityControl($scope, $http, $location, Activity, updateService) {
    $scope.date = new Date();
    $scope.showNextArrow = false;

    function getItems() {
        $scope.activity = Activity.query({ date: $scope.date.toUTCString() }, function () {
            for (var i = 0; i < $scope.activity.length; i++) {
                if($scope.typeAhead.indexOf($scope.activity[i].Name == -1))
                {
                    $scope.typeAhead.push($scope.activity[i].Name);
                } 
            }
        });
    }

    getItems();

    $scope.save = function () {
        var act = new Activity({ Name: $scope.name, Date: $scope.date });
        act.$save(function (a, putResponseHeaders) {
            $scope.activity.push({ Name: a.Name, Id: a.Id, Note: a.Note });
            if ($scope.typeAhead.indexOf(a.Name) == -1) {
                $scope.typeAhead.push(a.Name);
            }
            updateService.broadcastItem();
        });
        $scope.name = '';
    };

    $scope.decreaseDate = function() {
        $scope.date.setDate($scope.date.getDate() - 1);
        $scope.showNextArrow = true;
        getItems();
    };

    $scope.increaseDate = function() {
        if($scope.date.toDateString() !== new Date().toDateString()) {
            $scope.date.setDate($scope.date.getDate() + 1);
            if($scope.date.toDateString() === new Date().toDateString()) {
                $scope.showNextArrow = false;
            }
            getItems();
        }else {
            $scope.showNextArrow = false;
        }
    };

    $scope.delete = function (id) {

        var act = new Activity({ Id: id });
        act.$remove(function (a) {
            for (var i = 0; i < $scope.activity.length; i++) {
                if($scope.activity[i].Id == id) {
                    $scope.activity.splice(i, 1);
                    updateService.broadcastItem();
                }
            }
        });
    };

    $scope.typeaheadFn = function(query, callback) {
        $http.get('/activity/autocomplete?term=' + query).success(function(stations) {
            callback(stations); // This will automatically open the popup with retrieved results
        });
    };
}

function MonthDashboardController($scope, Dashboard) {
    $scope.dashboard = Dashboard.get();
    
    $scope.$on('handleBroadcast', function (event, args) {
        $scope.dashboard = Dashboard.get();
    });
}

function YearDashboardController($scope, YearDashboard) {
    $scope.dashboard = YearDashboard.get();
   
    $scope.$on('handleBroadcast', function (event, args) {
        $scope.dashboard = YearDashboard.get();
    });
}

ActivityControl.$inject = ['$scope', '$http', '$location', 'Activity', 'updateService'];
MonthDashboardController.$inject = ['$scope', 'Dashboard'];
YearDashboardController.$inject = ['$scope', 'YearDashboard'];