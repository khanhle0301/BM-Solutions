(function (app) {
    'use strict';

    app.controller('appRoleAddController', appRoleAddController);

    appRoleAddController.$inject = ['$scope', 'apiService', 'notificationService', '$location', 'commonService'];

    function appRoleAddController($scope, apiService, notificationService, $location, commonService) {
        $scope.role = {
            Id: 0
        }

        $scope.addAppRole = addAppRole;

        function addAppRole() {
            apiService.post('/api/appRole/add', $scope.role, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.role.Name + ' đã được thêm mới.');

            $location.url('app_roles');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
        }
    }
})(angular.module('bm-solutions.app_users'));