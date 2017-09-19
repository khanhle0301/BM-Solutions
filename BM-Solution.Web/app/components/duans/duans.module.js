/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('bm-solutions.duans', ['bm-solutions.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
            .state('duan_add', {
                url: "/duan_add",
                parent: 'base',
                templateUrl: "/app/components/duans/duanAddView.html",
                controller: "duanAddController"
            })
            .state('duan_edit', {
                url: "/duan_edit/:id",
                parent: 'base',
                templateUrl: "/app/components/duans/duanEditView.html",
                controller: "duanEditController"
            });
    }
})();