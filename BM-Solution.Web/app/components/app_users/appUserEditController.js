(function (app) {
    'use strict';

    app.controller('appUserEditController', appUserEditController);

    appUserEditController.$inject = ['$scope', 'apiService', 'notificationService', '$location',
        '$filter', '$stateParams'];

    function appUserEditController($scope, apiService, notificationService,
        $location, $filter, $stateParams) {
        $scope.user = {}
        $scope.editUser = editUser;
        function editUser() {
            apiService.put('api/appUser/update', $scope.user, editSuccessed, editFailed);
        }
        function editSuccessed() {
            notificationService.displaySuccess($scope.user.FullName + ' đã được cập nhật.');
            $location.url('app_users');
        }
        function editFailed(response) {
            if (response.status == "403") {
                notificationService.displayError("Bạn không có quyền");
                $location.url('/');
            } else {
                notificationService.displayError(response.data.Message);
            }
        }

        function loadRole() {
            apiService.get('api/appRole/getliststring', null, function (result) {
                $scope.listRole = result.data;
            }, function () {
                console.log('Cannot get list');
            });
        }
        $scope.loadRoles = function ($query) {
            var roles = $scope.listRole;
            return roles.filter(function (role) {
                return role.toLowerCase().indexOf($query.toLowerCase()) != -1;
            });
        };
        function loadDetail() {
            apiService.get('/api/appUser/detail/' + $stateParams.id, null,
                function (result) {
                    $scope.user = result.data;
                },
                function (result) {
                    notificationService.displayError(result.data);
                });
        }

        loadDetail();
        loadRole();
    }
})(angular.module('bm-solutions.app_users'));