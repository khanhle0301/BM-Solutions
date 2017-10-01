(function (app) {
    app.controller('resetPassController', resetPassController);

    resetPassController.$inject = ['$state', '$scope', 'apiService', 'notificationService', '$location',
        '$filter', '$stateParams'];

    function resetPassController($state, $scope, apiService, notificationService,
        $location, $filter, $stateParams) {
        $scope.model = {
            Id: $stateParams.id
        };
        $scope.resetPass = resetPass;
        function resetPass() {
            apiService.put('api/appUser/resetpass', $scope.model, editSuccessed, editFailed);
        }
        function editSuccessed() {
            notificationService.displaySuccess('Khôi phục mật khẩu thành công');
            $location.url('app_users');
        }
        function editFailed(response) {
            notificationService.displayError(response.data.Message);
        }
    }
})(angular.module('bm-solutions.app_users'));