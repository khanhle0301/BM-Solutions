/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('bm-solutions.app_roles', ['bm-solutions.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('app_roles', {
            url: "/app_roles",
            templateUrl: "/app/components/app_roles/appRoleListView.html",
            parent: 'base',
            controller: "appRoleListController"
        })
            .state('add_app_role', {
                url: "/add_app_role",
                parent: 'base',
                templateUrl: "/app/components/app_roles/appRoleAddView.html",
                controller: "appRoleAddController"
            })
            .state('edit_app_role', {
                url: "/edit_app_role/:id",
                templateUrl: "/app/components/app_roles/appRoleEditView.html",
                controller: "appRoleEditController",
                parent: 'base'
            });
    }
})();