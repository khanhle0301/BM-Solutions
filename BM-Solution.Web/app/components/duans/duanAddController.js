(function (app) {
    app.controller('duanAddController', duanAddController);

    duanAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$location', 'commonService', '$http', '$filter'];

    function duanAddController(apiService, $scope, notificationService, $state, $location, commonService, $http, $filter) {
        // khởi tạo scope
        $scope.init = function () {
            // ngày tạo
            $scope.ngayTao = new Date();
            // thời gian dự tính
            $scope.thoiGian = new Date();
            // set opened popup
            $scope.popup1 = {
                opened: false
            };
            // set opened popup
            $scope.popup2 = {
                opened: false
            };
            // cấu hình tinymce
            $scope.tinymceOptions = {
                language: 'vi_VN'
            };
            // khởi tạo model dự án
            $scope.duan = {
                IsDelete: false,
                AppUsers: [],
                TienVonBanDauViewModel: []
            }
        };
        $scope.init();

        // mở popup date
        $scope.open1 = function () {
            $scope.popup1.opened = true;
        };

        // mở popup date
        $scope.open2 = function () {
            $scope.popup2.opened = true;
        };

        // load danh sách người dùng
        function loadUser() {
            apiService.get('api/appUser/getListString', null, function (result) {
                $scope.listUser = result.data;
            }, function () {
                console.log('Cannot get list user');
            });
        }

        // sự kiện chọn user
        $scope.loadUsers = function ($query) {
            var users = $scope.listUser;
            return users.filter(function (user) {
                return user.toLowerCase().indexOf($query.toLowerCase()) != -1;
            });
        };

        // thêm mới dự án
        $scope.addDuAn = addDuAn;
        // thêm mới dự án
        function addDuAn() {
            // gán lại ngày tạo
            $scope.duan.NgayTao = $scope.ngayTao;
            // gán lại thời gian dự tính
            $scope.duan.ThoiGianDuTinh = $scope.thoiGian;
            apiService.post('api/duan/add', $scope.duan, addSuccessed, addFailed);
        }

        // thêm thành công
        function addSuccessed() {
            notificationService.displaySuccess($scope.duan.Id + ' đã được thêm mới.');
            $location.url('/');
        }

        // thêm thất bại
        function addFailed(response) {
            if (response.status == "403") {
                notificationService.displayError("Bạn không có quền thêm mới dự án");
                $location.url('/');
            } else {
                notificationService.displayError(response.data.Message);
            }
        }

        $scope.add = function () {
            $scope.TienVonBanDauViewModel = {
                IsDelete: false,
                UserId: $scope.User,
                DuAnId: $scope.duan.Id,
                TongTien: $scope.TienVonBanDau
            };
            $scope.duan.TienVonBanDauViewModel.push($scope.TienVonBanDauViewModel);
            $scope.TienVonBanDauViewModel = {};
        }

        $scope.remove = function (item) {
            var index = $scope.bdays.indexOf(item);
            $scope.bdays.splice(index, 1);
        }

        // load user
        loadUser();
    }
})(angular.module('bm-solutions.duans'));