(function (app) {
    'use strict';
    app.service('loginService', ['$http', '$q', 'authenticationService', 'authData',
    function ($http, $q, authenticationService, authData) {
        var userInfo;
        var deferred;

        this.login = function (userName, password) {
            deferred = $q.defer();
            var data = "grant_type=password&username=" + userName + "&password=" + password;
            $http.post('/api/oauth/token', data, {
                headers:
                   { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (response) {
                userInfo = {
                    accessToken: response.access_token,
                    userName: userName,
                    fullName: response.fullName,
                    roles: response.roles
                };
                authenticationService.setTokenInfo(userInfo);
                authData.authenticationData.IsAuthenticated = true;
                authData.authenticationData.userName = userName;
                authData.authenticationData.fullName = response.fullName;
                authData.authenticationData.roles = JSON.parse(response.roles);
                deferred.resolve(null);
            })
            .error(function (err) {
                authData.authenticationData.IsAuthenticated = false;
                authData.authenticationData.userName = "";
                authData.authenticationData.fullName = "";
                authData.authenticationData.roles = [];
                deferred.resolve(err);
            });
            return deferred.promise;
        }

        this.logOut = function () {
            authenticationService.removeToken();
            authData.authenticationData.IsAuthenticated = false;
            authData.authenticationData.userName = "";
            authData.authenticationData.fullName = "";
            authData.authenticationData.roles = [];
        }
    }]);
})(angular.module('bm-solutions.common'));