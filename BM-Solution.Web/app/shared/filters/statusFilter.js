(function (app) {
    app.filter('statusFilter', function () {
        return function (input) {
            if (input == 0)
                return 'Đang hoạt động';
            else if (input == 1)
                return 'Đã có tiền';
            else if (input == 2)
                return 'Đã có lợi nhuận';
            else if (input == 3)
                return 'Có thế kết thúc';
            else
                return 'Đã kết thúc';
        }
    });
})(angular.module('bm-solutions.common'));