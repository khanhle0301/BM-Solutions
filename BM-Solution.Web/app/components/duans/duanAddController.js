(function (app) {
    app.controller('duanAddController', duanAddController);

    duanAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$location', 'commonService', '$http', '$filter'];

    function duanAddController(apiService, $scope, notificationService, $state, $location, commonService, $http, $filter) {
        $scope.today = function () {
            $scope.ngayTao = new Date();
            $scope.thoiGian = new Date();
        };
        $scope.today();

        $scope.open1 = function () {
            $scope.popup1.opened = true;
        };

        $scope.popup1 = {
            opened: false
        };

        $scope.open2 = function () {
            $scope.popup2.opened = true;
        };

        $scope.popup2 = {
            opened: false
        };

        $scope.tinymceOptions = {
            language: 'vi_VN'
        };

        $scope.duan = {
            IsDelete: false
        }

        $scope.addDuAn = addDuAn;

        function addSuccessed() {
            notificationService.displaySuccess($scope.duan.Ten + ' đã được thêm mới.');
            $location.url('/');
        }

        function addFailed(response) {
            if (response.status == "403") {
                notificationService.displayError(response.statusText);
                $location.url('/');
            } else {
                notificationService.displayError(response.data.Message);
            }
        }

        function loadUser() {
            apiService.get('api/appUser/getListString', null, function (result) {
                $scope.listUser = result.data;
            }, function () {
                console.log('Cannot get list user');
            });
        }

        $scope.loadUsers = function ($query) {
            var users = $scope.listUser;
            return users.filter(function (user) {
                return user.toLowerCase().indexOf($query.toLowerCase()) != -1;
            });
        };

        function addDuAn() {
            $scope.duan.NgayTao = $scope.ngayTao;
            $scope.duan.ThoiGianDuTinh = $scope.thoiGian;
            apiService.post('api/duan/add', $scope.duan, addSuccessed, addFailed);
        }

        loadUser();
    }
})(angular.module('bm-solutions.duans'));