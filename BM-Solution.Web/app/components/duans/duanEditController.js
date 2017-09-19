(function (app) {
    app.controller('duanEditController', duanEditController);

    duanEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$location', 'commonService', '$stateParams'];

    function duanEditController(apiService, $scope, notificationService, $state, $location, commonService, $stateParams) {
        $scope.duan = {}

        $('input[name="single_cal1"]').datetimepicker({
            format: 'DD/MM/YYYY'
        });

        $scope.tinymceOptions = {
            language: 'vi_VN'
        };

        $scope.tags = [
            { id: 1, name: 'User 1' },
            { id: 2, name: 'User 2' },
            { id: 3, name: 'User 3' }
        ];

        var s = [
            { id: 4, name: 'User 4' },
            { id: 5, name: 'User 5' },
            { id: 6, name: 'User 6' }
        ];

        $scope.loadTags = function (query) {
            return s;
        };

        function loadDetail() {
            apiService.get('/api/duan/detail/' + $stateParams.id, null,
                function (result) {
                    $scope.duan = result.data;
                },
                function (result) {
                    notificationService.displayError(result.data);
                });
        }

        loadDetail();

    }
})(angular.module('bm-solutions.duans'));