﻿(function (app) {
    'use strict';

    app.controller('appUserAddController', appUserAddController);

    appUserAddController.$inject = ['$scope', 'apiService', 'notificationService', '$location', 'commonService'];

    function appUserAddController($scope, apiService, notificationService, $location, commonService) {

        $scope.tags = [
            { id: 1, name: 'Dự án 1' },
            { id: 2, name: 'Dự án 2' },
            { id: 3, name: 'Dự án 3' }
        ];

        var s = [
            { id: 4, name: 'Dự án 4' },
            { id: 5, name: 'Dự án 5' },
            { id: 6, name: 'Dự án 6' }
        ];

        $scope.loadTags = function (query) {
            return s;
        };
    }
})(angular.module('bm-solutions.app_users'));