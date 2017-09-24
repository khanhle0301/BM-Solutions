(function (app) {
    'use strict';

    app.controller('lichSuGiaoDichListController', lichSuGiaoDichListController);

    lichSuGiaoDichListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox',
        'authData', '$filter', '$location'];

    function lichSuGiaoDichListController($scope, apiService, notificationService, $ngBootbox
        , authData, $filter, $location) {
        $scope.loading = true;
        $scope.data = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.search = search;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;
        $scope.filter = '';
        $scope.date = {
            startDate: moment().subtract(1, "days"),
            endDate: moment()
        };

        var roles = authData.authenticationData.roles;
        $scope.isSys = roles.includes('Admin');

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
                    }, function (error) {
                        notificationService.displayError('Xóa không thành công');
                    });
                });
        }

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

        $scope.$watch("data", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function search(page) {
            page = page || 0;
            var obj = JSON.parse(angular.toJson($scope.date));
            $scope.loading = true;
            var config = {
                params: {
                    page: page,
                    pageSize: 10,
                    startDate: obj.startDate.substring(0, 10),
                    endDate: obj.endDate.substring(0, 10)
                }
            }

            apiService.get('/api/chitietthuchi/nhatkygiaodich', config, dataLoadCompleted, dataLoadFailed);
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

        $scope.search();
    }
})(angular.module('bm-solutions.lichsus'));