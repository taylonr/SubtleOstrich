angular.module('activity', ['ngResource']).
  config(function ($routeProvider) {
      $routeProvider.
        when('/', { controller: ActivityControl, templateUrl: 'list' }).
        otherwise({ redirectTo: '/' });
  }).factory('Activity', function ($resource) {    
    var Activity = $resource('/Home/ActivityList', {name: '@Name'});

    return Activity;
});

function ActivityControl($scope, Activity) {
    $scope.activity = Activity.query();
    $scope.save = function() {
        $scope.activity.push({ Name: $scope.name });
        var act = new Activity({ Name: $scope.name });
        act.$save();
        $scope.name = '';
    };
}
