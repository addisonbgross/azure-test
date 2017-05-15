var azureApp = angular.module('AzureApp', []);

azureApp.controller('PokemonController', ['$scope', function($scope) {
    $scope.currentType = "ghost";

    $scope.show = function(name, spriteUrl) {
        $scope.currentName = name.charAt(0).toUpperCase() + name.slice(1);
        $scope.currentSprite = spriteUrl;
    }
}]);