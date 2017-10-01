(function (app) {
    'use strict';

    app.controller('lichSuHeThongListController', lichSuHeThongListController);

    lichSuHeThongListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter', '$location'];

    function lichSuHeThongListController($scope, apiService, notificationService, $ngBootbox, $filter, $location) {
        $scope.loading = true;
        $scope.data = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.search = search;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;
        $scope.date = {};

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
                    apiService.del('/api/systemlog/deletemulti', config, function (result) {
                        notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                        search();
                    }, function (error) {
                        notificationService.displayError('Xóa không thành công');
                    });
                });
        }

        // getrange
        function getRange() {
            apiService.get('/api/systemlog/getRange', null,
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
            $scope.loading = true;
            var config = {
                params: {
                    page: page,
                    pageSize: 10,
                    startDate: new Date($scope.date.startDate),
                    endDate: new Date($scope.date.endDate)
                }
            }

            apiService.get('/api/systemlog/getlistpaging', config, dataLoadCompleted, dataLoadFailed);
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
            if (response.status == "403") {
                notificationService.displayError("Bạn không có quyền truy cập");
                $location.url('/');
            } else {
                notificationService.displayError(response.data.Message);
            }
        }

        getRange();
    }
})(angular.module('bm-solutions.lichsus'));