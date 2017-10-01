/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('bm-solutions.app_users', ['bm-solutions.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('app_users', {
            url: "/app_users",
            templateUrl: "/app/components/app_users/appUserListView.html",
            parent: 'base',
            controller: "appUserListController"
        })
            .state('add_app_user', {
                url: "/add_app_user",
                parent: 'base',
                templateUrl: "/app/components/app_users/appUserAddView.html",
                controller: "appUserAddController"
            })
            .state('edit_app_user', {
                url: "/edit_app_user/:id",
                templateUrl: "/app/components/app_users/appUserEditView.html",
                controller: "appUserEditController",
                parent: 'base'
            })
            .state('profile', {
                url: "/profile",
                parent: 'base',
                templateUrl: "/app/components/app_users/appUserDetailView.html",
                controller: "appUserDetailController"
            })
            .state('change_pass', {
                url: "/change_pass",
                parent: 'base',
                templateUrl: "/app/components/app_users/changePassView.html",
                controller: "changePassController"
            })
             .state('reset_pass', {
                 url: "/reset_pass/:id",
                 templateUrl: "/app/components/app_users/resetPassView.html",
                 controller: "resetPassController",
                 parent: 'base'
             });
    }
})();