(function (app) {
    'use strict';

    app.controller('appUserListController', appUserListController);

    appUserListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter', '$location'];

    function appUserListController($scope, apiService, notificationService, $ngBootbox, $filter, $location) {
    }
})(angular.module('bm-solutions.app_users'));