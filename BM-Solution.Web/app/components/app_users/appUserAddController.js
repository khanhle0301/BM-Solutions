﻿(function (app) {
    'use strict';

    app.controller('appUserAddController', appUserAddController);

    appUserAddController.$inject = ['$scope', 'apiService', 'notificationService', '$location', 'commonService', '$filter'];

    function appUserAddController($scope, apiService, notificationService, $location, commonService, $filter) {
        $scope.user = {
            Roles: [],
            Status: true
        }

        $scope.addUser = addUser;

        function addUser() {
            apiService.post('api/appUser/add', $scope.user, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.user.FullName + ' đã được thêm mới.');
            $location.url('app_users');
        }

        function addFailed(response) {
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

        loadRole();
    }
})(angular.module('bm-solutions.app_users'));