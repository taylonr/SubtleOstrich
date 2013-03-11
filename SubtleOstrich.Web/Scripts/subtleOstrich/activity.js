angular.module('activity', ['ngResource']).
  config(function ($routeProvider) {
      $routeProvider.
        when('/', { controller: ActivityControl, templateUrl: 'list' }).
        otherwise({ redirectTo: '/' });
  }).factory('Activity', function ($resource) {    
    var Activity = $resource('/Home/ActivityList', {name: '@Name', date: '@Date', id: '@Id'});

    return Activity;
});

function ActivityControl($scope, Activity) {
    $scope.date = new Date();
    $scope.activity = Activity.query();
    $scope.save = function() {
        $scope.activity.push({ Name: $scope.name });
        var act = new Activity({ Name: $scope.name, Date: $scope.date });
        act.$save();
        $scope.name = '';
    };

    $scope.delete = function (id) {
        for (var i = 0; i < $scope.activity.length; i++) {
            if($scope.activity[i].Id == id) {
                $scope.activity.splice(i, 1);
            }
        }
        var act = new Activity({ Id: id });
        act.$remove();
    };
}
