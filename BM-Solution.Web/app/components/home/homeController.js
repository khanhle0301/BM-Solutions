(function (app) {
    app.controller('homeController', homeController);

    homeController.$inject = ['$state', 'authData', '$scope', 'authenticationService', 'apiService'
        , 'notificationService', '$stateParams', '$ngBootbox', '$filter', '$location'];

    function homeController($state, authData, $scope, authenticationService, apiService,
        notificationService, $stateParams, $ngBootbox, $filter, $location) {
        // định nghĩa các scope
        $scope.loading = true;
        $scope.data = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.search = search;
        $scope.deleteItem = deleteItem;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;
        $scope.filter = '';

        $scope.listDuan = [
            { id: 0, text: 'Đang hoạt động' },
            { id: 1, text: 'Đã có tiền' },
            { id: 2, text: 'Đã có lợi nhuận' },
            { id: 3, text: 'Có thể kết thúc' },
            { id: 4, text: 'Đã kết thúc' }
        ];

        $scope.Duans = [0];

        $scope.imChanged = function () {
            // load
            $scope.search();
        }

        // cờ admin
        var roles = authData.authenticationData.roles;
        $scope.isSys = roles.includes('Admin');

        // xóa multiple
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
                    apiService.del('/api/duan/deletemulti', config, function (result) {
                        notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                        search();
                    }, function (error) {
                        notificationService.displayError('Xóa không thành công');
                    });
                });
        }

        // event checkbox
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

        // event checkbox
        $scope.$watch("data", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        // xóa 1 item
        function deleteItem(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?')
                .then(function () {
                    var config = {
                        params: {
                            id: id
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

        // tìm kiếm
        function search(page) {
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    page: page,
                    pageSize: 10,
                    filter: $scope.filter,
                    status: JSON.stringify($scope.Duans)
                }
            }
            apiService.get('/api/duan/getlistpaging', config, dataLoadCompleted, dataLoadFailed);
        }

        // load dữ liệu thành công
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

        // load dữ liệu thất bại
        function dataLoadFailed(response) {
            if (response.status == 401) {
                notificationService.displayError('Bạn hết phiên đăng nhập. Mời đăng nhập lại.');
            }
            else if (response.status == 403) {
                notificationService.displayError('Bạn bị chặn truy cập.');
            }
            else {
                notificationService.displayError(response.data.Message);
            }
        }

        // load
        $scope.search();
    }
})(angular.module('bm-solutions'));