(function (app) {
    app.controller('duanAddController', duanAddController);

    duanAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$location', 'commonService', '$http', '$filter'];

    function duanAddController(apiService, $scope, notificationService, $state, $location, commonService, $http, $filter) {

        $('input[name="single_cal1"]').datetimepicker({
            format: 'DD/MM/YYYY'
        });

        $scope.tinymceOptions = {
            language: 'vi_VN'
        };

        $scope.tags = [];

        var s = [
            { id: 4, name: 'User 4' },
            { id: 5, name: 'User 5' },
            { id: 6, name: 'User 6' }
        ];

        function loadUser() {
            apiService.get('api/appUser/getlistpaging', null, function (result) {
                $scope.user = result.data;
            }, function () {
                console.log('Cannot get list user');
            });
        }

        $scope.loadTags = function (query) {
            return $scope.user;
        };

        $scope.duan = {
            IsDelete: false,
            NgayTao: $filter('date')(Date.now(), 'dd/MM/yyyy'),
            ThoiGianDuTinh: $filter('date')(Date.now(), 'dd/MM/yyyy')
        }

        $scope.addDuAn = addDuAn;

        function addDuAn() {
            apiService.post('api/duan/add', $scope.duan, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.duan.Ten + ' đã được thêm mới.');
            $location.url('/');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        loadUser();

    }
})(angular.module('bm-solutions.duans'));