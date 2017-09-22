(function (app) {
    app.controller('duanEditController', duanEditController);

    duanEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$location',
        'commonService', '$stateParams', 'authData', '$ngBootbox'];

    function duanEditController(apiService, $scope, notificationService, $state, $location,
        commonService, $stateParams, authData, $ngBootbox) {

        $scope.openModel = function () {
            $('#myModal').modal('show');
        }

        var roles = authData.authenticationData.roles;
        $scope.isSys = roles.includes('Admin');

        $scope.isEdit = true;

        $scope.duan = {};

        $scope.edit = edit;
        $scope.cancel = cancel;

        $scope.tinymceOptions = {
            language: 'vi_VN'
        };

        function loadDetail() {
            apiService.get('/api/duan/detail/' + $stateParams.id, null,
                function (result) {
                    $scope.duan = result.data;
                    $scope.ngayTao = new Date(result.data.NgayTao);
                    $scope.thoiGian = new Date(result.data.ThoiGianDuTinh);
                },
                function (result) {
                    notificationService.displayError(result.data);
                });
        }

        $scope.data = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.search = search;
        $scope.filter = '';

        function search(page) {
            page = page || 0;
            var obj = JSON.parse(angular.toJson($scope.date));
            var config = {
                params: {
                    duAnId: $stateParams.id,
                    page: page,
                    pageSize: 10,
                    startDate: obj.startDate.substring(0, 10),
                    endDate: obj.endDate.substring(0, 10)
                }
            }

            apiService.get('api/chitietthuchi/getbyduanid', config, dataLoadCompleted, dataLoadFailed);
        }

        function dataLoadCompleted(result) {
            if (result.data.TotalCount == 0) {
                notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
            }
            $scope.data = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loading = false;
            var tongthu = 0, tongchi = 0;
            for (var i = 0; i < $scope.data.length; i++) {
                tongthu += ($scope.data[i].TienThu);
                tongchi += ($scope.data[i].TienChi);
            }
            $scope.TongThu = tongthu;
            $scope.TongChi = tongchi;
        }
        function dataLoadFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        $scope.style = 'hidden';

        function edit() {
            $scope.isEdit = false;
        }

        function cancel() {
            $scope.isEdit = true;
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

        $scope.editDuAn = editDuAn;

        function editDuAn() {
            apiService.put('api/duan/update', $scope.duan, editSuccessed, editFailed);
        }

        function editSuccessed() {
            notificationService.displaySuccess($scope.duan.Ten + ' đã được cập nhật.');
            $location.url('/');
        }

        function editFailed(response) {
            notificationService.displayError(response.data.Message);
        }


        $scope.today = function () {
            $scope.ngaytaoct = new Date();
        };
        $scope.today();

        $scope.openchitiet = function () {
            $scope.popupchitiet.opened = true;
        };

        $scope.popupchitiet = {
            opened: false
        };

        $scope.chitiet = {
            IsDelete: false,
            DuAnId: $stateParams.id
        };

        $scope.addChiTiet = addChiTiet;

        function addChiTiet() {
            $scope.chitiet.NgayTao = $scope.ngaytaoct;
            apiService.post('api/chitietthuchi/add', $scope.chitiet, addChiTietSuccessed, addChiTietFailed);
        }

        function addChiTietSuccessed() {
            notificationService.displaySuccess('Thêm giao dịch thành công');
            loadUser();
            loadDetail();
            $scope.search();
            $('#myModal').modal('hide');
        }

        function addChiTietFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        //
        $scope.date = {
            startDate: moment().subtract(1, "days"),
            endDate: moment()
        };

        $scope.ketthuc = ketthuc;

        function ketthuc() {
            $ngBootbox.confirm('Bạn có chắc muốn kết thúc dự án?')
                .then(function () {
                    var config = {
                        params: {
                            id: $stateParams.id
                        }
                    }
                    apiService.del('/api/duan/ketthuc', config, function () {
                            notificationService.displaySuccess('Dự án đã kết thúc.');
                            loadDetail();
                        },
                        function () {
                            notificationService.displayError('Kết thúc thất bại.');
                        });
                });
        }


        loadUser();
        loadDetail();
        $scope.search();
    }
})(angular.module('bm-solutions.duans'));