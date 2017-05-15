var azureApp = angular.module('AzureApp', []);

azureApp.controller('PokemonController', ['$scope', '$http', function($scope, $http) {
    $scope.currentType = "ghost";

    $scope.show = function(name, spriteUrl) {
        $scope.currentName = name.charAt(0).toUpperCase() + name.slice(1);
        $scope.currentSprite = spriteUrl;
    };

    $scope.updateList = function() {
        console.log($scope.selectedType);

        var url = 'home/index/' + (parseInt($scope.selectedType) + 1).toString();

        $http({
            method: 'GET',
            url: url,
        }).then(
            function success(response) {
                console.log("WIN");
            },
            function error(response) {
                console.log("ERR");
            }
        )
    };
}]);