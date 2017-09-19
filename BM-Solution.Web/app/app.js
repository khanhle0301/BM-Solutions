﻿/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('bm-solutions',
            ['bm-solutions.duans',
                'bm-solutions.app_users',
                'bm-solutions.lichsus',
                'bm-solutions.app_roles',
                'bm-solutions.common'])
        .config(config)
        .config(configAuthentication);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('base',
                {
                    url: '',
                    templateUrl: '/app/shared/views/baseView.html',
                    abstract: true
                })
            .state('login',
                {
                    url: "/login",
                    templateUrl: "/app/components/login/loginView.html",
                    controller: "loginController"
                })
            .state('home',
                {
                    url: "/",
                    parent: 'base',
                    templateUrl: "/app/components/home/homeView.html",
                    controller: "homeController"
                });
        $urlRouterProvider.otherwise('/login');
    }

    function configAuthentication($httpProvider) {
        $httpProvider.interceptors.push(function ($q, $location) {
            return {
                request: function (config) {
                    return config;
                },
                requestError: function (rejection) {
                    return $q.reject(rejection);
                },
                response: function (response) {
                    if (response.status == "401") {
                        $location.path('/login');
                    }
                    return response;
                },
                responseError: function (rejection) {
                    if (rejection.status == "401") {
                        $location.path('/login');
                    }
                    return $q.reject(rejection);
                }
            };
        });
    }
})();