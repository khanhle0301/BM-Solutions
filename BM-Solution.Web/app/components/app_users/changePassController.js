(function (app) {
    app.controller('changePassController', changePassController);

    changePassController.$inject = ['$state', '$scope', 'apiService', 'notificationService', '$location',
        '$filter', '$stateParams', 'loginService'];

    function changePassController($state, $scope, apiService, notificationService,
        $location, $filter, $stateParams, loginService) {
        $scope.model = {};
        $scope.changePass = changePass;
        function changePass() {
            apiService.put('api/appUser/changepass', $scope.model, editSuccessed, editFailed);
        }
        function editSuccessed() {
            notificationService.displaySuccess('Đổi mật khẩu thành công, xin đăng nhập lại.');
            loginService.logOut();
            $state.go('login');
        }
        function editFailed(response) {
            notificationService.displayError(response.data.Message);
        }
    }
})(angular.module('bm-solutions.app_users'));