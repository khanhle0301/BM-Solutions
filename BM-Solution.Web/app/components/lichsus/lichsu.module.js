/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('bm-solutions.lichsus', ['bm-solutions.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('syslog',
                {
                    url: "/syslog",
                    templateUrl: "/app/components/lichsus/lichSuHeThongListView.html",
                    parent: 'base',
                    controller: "lichSuHeThongListController"
                })
            .state('transactionlog',
                {
                    url: "/transactionlog",
                    templateUrl: "/app/components/lichsus/lichSuGiaoDichListView.html",
                    parent: 'base',
                    controller: "lichSuGiaoDichListController"
                });
    }
})();