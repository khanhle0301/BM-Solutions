(function (app) {
    app.controller('duanEditController', duanEditController);

    duanEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$location',
        'commonService', '$stateParams', 'authData', '$ngBootbox', '$filter', 'Lightbox'];

    function duanEditController(apiService, $scope, notificationService, $state, $location,
        commonService, $stateParams, authData, $ngBootbox, $filter, Lightbox) {

        Lightbox.fullScreenMode = false;

        // khởi tạo scope chi tiết dự án
        $scope.init = function () {
            // role admin
            var roles = authData.authenticationData.roles;
            $scope.isSys = roles.includes('Admin');
            // flag isedit
            $scope.isEdit = true;
            // model dự án
            $scope.duan = {};
            // cấu hình tinymce
            $scope.tinymceOptions = {
                language: 'vi_VN'
            };
            // style
            $scope.style = 'hidden';
        };
        $scope.init();

        // edit
        $scope.edit = edit;
        function edit() {
            $scope.isEdit = false;
        }
        // cancel
        $scope.cancel = cancel;
        function cancel() {
            $scope.isEdit = true;
        }
        // open model
        $scope.openModel = function () {
            $scope.chitiet = {
                IsDelete: false,
                DuAnId: $stateParams.id
            };
            $scope.moreImages = [];
            $('#myModal').modal('show');
        }

        // load detail dự án
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

        // load dánh sách user
        function loadUser() {
            apiService.get('api/appUser/getListString', null, function (result) {
                $scope.listUser = result.data;
            }, function () {
                console.log('Cannot get list user');
            });
        }
        // load user
        $scope.loadUsers = function ($query) {
            var users = $scope.listUser;
            return users.filter(function (user) {
                return user.toLowerCase().indexOf($query.toLowerCase()) != -1;
            });
        };

        // kết thúc dự án
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

        // xóa dự án
        $scope.xoa = xoa;
        function xoa() {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?')
                .then(function () {
                    var config = {
                        params: {
                            id: $stateParams.id
                        }
                    }
                    apiService.del('/api/duan/delete', config, function () {
                        notificationService.displaySuccess('Đã xóa thành công.');
                        $location.url('/');
                    },
                        function () {
                            notificationService.displayError('Xóa không thành công.');
                        });
                });
        }

        // cập nhật dự án
        $scope.editDuAn = editDuAn;
        function editDuAn() {
            apiService.put('api/duan/update', $scope.duan, editSuccessed, editFailed);
        }
        // cập nhật dự án thành công
        function editSuccessed() {
            notificationService.displaySuccess($scope.duan.Id + ' đã được cập nhật.');
            $location.url('/');
        }
        // câp nhật dự án thất bại
        function editFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        // khởi tạo scope nhật ký thu chi
        $scope.nhatky = function () {
            // ngày tạo chi tiết
            $scope.ngaytaoct = new Date();
            // data nhật ký thu chi
            $scope.data = [];
            // page
            $scope.page = 0;
            // pageCount
            $scope.pagesCount = 0;
            // popupchitiet
            $scope.popupchitiet = {
                opened: false
            };
            // chitiet
            $scope.chitiet = {
                IsDelete: false,
                DuAnId: $stateParams.id
            };
        };
        $scope.nhatky();

        // date
        $scope.date = {};

        // openchitiet
        $scope.openchitiet = function () {
            $scope.popupchitiet.opened = true;
        };

        // getrange
        getRange = function () {
            var config = {
                params: {
                    duAnId: $stateParams.id,
                }
            }
            apiService.get('/api/chitietthuchi/getrange', config,
                 function (result) {
                     var _date = {
                         startDate: moment(result.data.MinDate),
                         endDate: moment(result.data.MaxDate)
                     };

                     $scope.date = _date;
                     // load search
                     $scope.search();
                 },
                 function () {
                     console.log('Can not get range');
                 });

        };

        // search nhật ký thu chi
        $scope.search = search;
        function search(page) {
            page = page || 0;
            //var obj = JSON.parse(angular.toJson($scope.date));
            //var startDate = (obj.startDate == null) ? moment().subtract(1, "days").toJSON().slice(0, 10) : obj.startDate;
            //var endDate = (obj.endDate == null) ? moment().toJSON().slice(0, 10) : obj.endDate;
            var config = {
                params: {
                    duAnId: $stateParams.id,
                    page: page,
                    pageSize: 10,
                    startDate: moment($scope.date.startDate).format('DD-MM-YYYY'),
                    endDate: moment($scope.date.endDate).format('DD-MM-YYYY')
                }
            }
            apiService.get('api/chitietthuchi/getbyduanid', config, dataLoadCompleted, dataLoadFailed);
        }

        // load thành công
        function dataLoadCompleted(result) {
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
        // load thất bại
        function dataLoadFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        // thêm chi tiết
        $scope.addChiTiet = addChiTiet;
        function addChiTiet() {
            $scope.chitiet.MoreImages = JSON.stringify($scope.moreImages);
            $scope.chitiet.NgayTao = $scope.ngaytaoct;
            apiService.post('api/chitietthuchi/add', $scope.chitiet, addChiTietSuccessed, addChiTietFailed);
        }
        // thêm thành công
        function addChiTietSuccessed() {
            notificationService.displaySuccess('Thêm giao dịch thành công');
            loadUser();
            loadDetail();
            $scope.search();
            $('#myModal').modal('hide');
        }
        // thêm thất bại
        function addChiTietFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        // selectAll
        $scope.selectAll = selectAll;
        // checkbox
        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.data, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.data, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("data", function (n) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        // xóa multiple nhật ký giao dịch
        $scope.deleteMultiple = deleteMultiple;
        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.Id);
            });
            $ngBootbox.confirm('Bạn có chắc muốn xóa?')
                .then(function () {
                    var config = {
                        params: {
                            checkedList: JSON.stringify(listId)
                        }
                    }
                    apiService.del('/api/chitietthuchi/deletemulti', config, function (result) {
                        notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                        search();
                    }, function () {
                        notificationService.displayError('Xóa không thành công');
                    });
                });
        }

        // upload image
        $scope.moreImages = [];

        $scope.ChooseMoreImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.moreImages.push(fileUrl);
                })

            }
            finder.popup();
        }

        $scope.deleteItem = function (index) {
            $scope.moreImages.splice(index, 1);
        }

        //show image
        $scope.images = [];
        $scope.openLightboxModal = function (index, id) {
            var listImage = $scope.data.find(function (item) {
                return item.Id === id;
            });
            $scope.images = listImage.MoreImages;
            Lightbox.openModal($scope.images, index);
        };

        //get range
        getRange();
        // load chi tiết dự án
        loadDetail();
        // load user
        loadUser();
    }
})(angular.module('bm-solutions.duans'));