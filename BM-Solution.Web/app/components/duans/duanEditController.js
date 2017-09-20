(function (app) {
    app.controller('duanEditController', duanEditController);

    duanEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$location',
        'commonService', '$stateParams', 'authData'];

    function duanEditController(apiService, $scope, notificationService, $state, $location,
        commonService, $stateParams, authData) {
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

            $scope.loading = true;
            var config = {
                params: {
                    duAnId: $stateParams.id,
                    page: page,
                    pageSize: 10,
                    filter: $scope.filter
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

        loadUser();
        loadDetail();
        $scope.search();
    }
})(angular.module('bm-solutions.duans'));