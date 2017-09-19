(function (app) {
    'use strict';

    app.controller('lichSuGiaoDichListController', lichSuGiaoDichListController);

    lichSuGiaoDichListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter', '$location'];

    function lichSuGiaoDichListController($scope, apiService, notificationService, $ngBootbox, $filter, $location) {
        angular.element(document).ready(function () {
            $('input[name="reservation"]').daterangepicker(
                {
                    locale: {
                        format: 'DD/MM/YYYY'
                    }
                });
        });
    }
})(angular.module('bm-solutions.lichsus'));