(function (app) {
    'use strict';

    app.controller('appUserDetailController', appUserDetailController);

    appUserDetailController.$inject = ['$scope', 'apiService', 'notificationService', '$location',
        '$filter', '$stateParams'];

    function appUserDetailController($scope, apiService, notificationService,
        $location, $filter, $stateParams) {
        $scope.user = {}
        $scope.editUser = editUser;
        function editUser() {
            apiService.put('api/appUser/updateProfile', $scope.user, editSuccessed, editFailed);
        }
        function editSuccessed() {
            notificationService.displaySuccess('Cập nhật thành công');
            loadDetail();
        }
        function editFailed(response) {
            if (response.status == "403") {
                notificationService.displayError("Bạn không có quyền");
                $location.url('/');
            } else {
                notificationService.displayError(response.data.Message);
            }
        }

        function loadDetail() {
            apiService.get('/api/appUser/profile', null,
                function (result) {
                    $scope.user = result.data;
                },
                function (result) {
                    notificationService.displayError(result.data);
                });
        }
        loadDetail();
    }
})(angular.module('bm-solutions.app_users'));