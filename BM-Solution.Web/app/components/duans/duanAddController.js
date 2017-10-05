(function (app) {
    app.controller('duanAddController', duanAddController);

    duanAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$location', 'commonService', '$http', '$filter'];

    function duanAddController(apiService, $scope, notificationService, $state, $location, commonService, $http, $filter) {
        // khởi tạo scope
        $scope.addUser = {
            type: '0',
            PhanTramHoaHong: '0',
            TienVonBanDau: '0',
            TienVonBanDauVND: '0',
            User: 'admin'
        };

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
                DuAnUserViewModels: [],
                TienVonBanDau: 0,
                TienChiDuTinh: 0,
                LoiNhuanDuTinh: 0,
                TienThuDuTinh: 0
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
            apiService.get('api/appUser/getlistall', null, function (result) {
                $scope.listUser = result.data;
            }, function () {
                console.log('Cannot get list user');
            });
        }

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

            $scope.DuAnUserViewModel = {
                IsDelete: false,
                UserId: $scope.addUser.User,
                DuAnId: $scope.duan.Id,
                PhanTramHoaHong: $scope.addUser.PhanTramHoaHong,
                NgayTao: new Date()
            };

            var phantramVon = 0;
            var vondautu = 0;
            if ($scope.addUser.type == '0') {
                phantramVon = $scope.addUser.TienVonBanDau;
                vondautu = parseInt(($scope.addUser.TienVonBanDau) * ($scope.duan.TienVonBanDau));
            }
            else {
                phantramVon = ($scope.addUser.TienVonBanDauVND / $scope.duan.TienVonBanDau);
                vondautu = $scope.addUser.TienVonBanDauVND;
            }
            $scope.DuAnUserViewModel.TienVonBanDau = vondautu;
            $scope.DuAnUserViewModel.PhanTramVon = phantramVon;

            var obj = $scope.duan.DuAnUserViewModels.find(function (item) {
                return item.UserId === $scope.DuAnUserViewModel.UserId;
            });

            if (typeof obj != "undefined") {
                notificationService.displayError("Thành viên đã tồn tại");
            }

            else {
                $scope.duan.DuAnUserViewModels.push($scope.DuAnUserViewModel);
            }
            $scope.DuAnUserViewModel = {};
        }

        $scope.remove = function (item) {
            var index = $scope.duan.DuAnUserViewModels.indexOf(item);
            $scope.duan.DuAnUserViewModels.splice(index, 1);
        }

        // open model
        //$scope.popupEdit = function (item) {
        //    $scope.capnhat = {
        //        thanhvien: item.UserId,
        //        vondautu: item.TienVonBanDau,
        //        phantram: item.PhanTramHoaHong
        //    };
        //    $('#myModal_Edit').modal('show');
        //}

        //$scope.editThanhVien = function () {
        //    var obj = $scope.duan.DuAnUserViewModels.find(function (item) {
        //        return item.UserId === $scope.capnhat.thanhvien;
        //    });

        //    if (typeof obj != "undefined") {
        //        obj.TienVonBanDau = $scope.capnhat.vondautu;
        //        obj.PhanTramHoaHong = $scope.capnhat.phantram;
        //    }

        //    $('#myModal_Edit').modal('hide');
        //}

        // load user
        loadUser();
    }
})(angular.module('bm-solutions.duans'));