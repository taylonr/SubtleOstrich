var activityModule = angular.module('activity', ['ngResource']).
    config(function($routeProvider) {
        $routeProvider.
            when('/:date', { controller: ActivityControl, templateUrl: 'list' }).
            when('/MonthDashboard', { controller: MonthDashboardController }).
            when('/YearDashboard', { controller: YearDashboardController }).
            otherwise({ redirectTo: '/' });
    }).factory('Activity', function($resource) {
        var Activity = $resource('ActivityList', { name: '@Name', date: '@Date', id: '@Id' });

        return Activity;
    }).factory('Dashboard', function($resource) {
        var Dashboard = $resource('MonthDashboard');

        return Dashboard;
    }).factory('YearDashboard', function($resource) {
        return $resource('YearDashboard');
    }).factory('updateService', function($rootScope) {
        var updateService = { };

        updateService.broadcastItem = function () {
            $rootScope.$broadcast('handleBroadcast');
        };

        return updateService;
    });

function ActivityControl($scope, $location, Activity, updateService) {
    $scope.date = new Date();
    $scope.showNextArrow = false;

    function getItems() {
        $scope.activity = Activity.query({ date: $scope.date.toUTCString() });
    }

    getItems();

    $scope.save = function () {
        var act = new Activity({ Name: $scope.name, Date: $scope.date });
        act.$save(function (a, putResponseHeaders) {
            $scope.activity.push({ Name: a.Name, Id: a.Id });
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

ActivityControl.$inject = ['$scope', '$location', 'Activity', 'updateService'];
MonthDashboardController.$inject = ['$scope', 'Dashboard'];
YearDashboardController.$inject = ['$scope', 'YearDashboard'];