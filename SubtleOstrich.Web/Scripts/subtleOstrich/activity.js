angular.module('activity', ['ngResource']).
    config(function($routeProvider) {
        $routeProvider.
            when('/', { controller: ActivityControl, templateUrl: 'list' }).
            when('/MonthDashboard', { controller: MonthDashboardController }).
            when('/YearDashboard', {controller: YearDashboardController}).
            otherwise({ redirectTo: '/' });
    }).factory('Activity', function($resource) {
        var Activity = $resource('ActivityList', { name: '@Name', date: '@Date', id: '@Id' });

        return Activity;
    }).factory('Dashboard', function($resource) {
        var Dashboard = $resource('MonthDashboard');

        return Dashboard;
    }).factory('YearDashboard', function($resource) {
        return $resource('YearDashboard');
    });

function ActivityControl($scope, $location, Activity) {
    $scope.date = new Date();
    $scope.activity = Activity.query();

    $scope.save = function () {
        var act = new Activity({ Name: $scope.name, Date: $scope.date });
        act.$save(function (a, putResponseHeaders) {
            $scope.activity.push({ Name: a.Name, Id: a.Id });
            $scope.$emit('listUpdated');
        });
        $scope.name = '';
    };

    $scope.delete = function (id) {

        var act = new Activity({ Id: id });
        act.$remove(function (a) {
            for (var i = 0; i < $scope.activity.length; i++) {
                if($scope.activity[i].Id == id) {
                    $scope.activity.splice(i, 1);
                    $scope.$emit('listUpdated');
                }
            }
        });
    };
}

function MonthDashboardController($scope, $location, Dashboard) {
    $scope.dashboard = Dashboard.get();
}

function YearDashboardController($scope, $location, YearDashboard) {
    $scope.dashboard = YearDashboard.get();
}