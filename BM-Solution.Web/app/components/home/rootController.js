(function (app) {
    app.controller('rootController', rootController);

    rootController.$inject = ['$state', 'authData', 'loginService', '$scope', 'authenticationService','$sce'];

    function rootController($state, authData, loginService, $scope, authenticationService, $sce) {
        $scope.logOut = function () {
            loginService.logOut();
            $state.go('login');
        }
        $scope.authentication = authData.authenticationData;

        var roles = authData.authenticationData.roles;
        $scope.admin = roles.includes('Admin');
    }
})(angular.module('bm-solutions'));