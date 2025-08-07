// Dashboard.js - Quản lý dashboard

const Dashboard = {
    // Khởi tạo
    init: function() {
        this.initSalesChart();
        this.initRevenueChart();
    },

    // Khởi tạo biểu đồ doanh số
    initSalesChart: function() {
        var salesChartCanvas = document.getElementById('salesChart');
        if (salesChartCanvas) {
            var ctx = salesChartCanvas.getContext('2d');
            var salesChartData = {
                labels: ['T1', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7', 'T8', 'T9', 'T10', 'T11', 'T12'],
                datasets: [
                    {
                        label: 'Doanh số',
                        backgroundColor: 'rgba(60,141,188,0.9)',
                        borderColor: 'rgba(60,141,188,0.8)',
                        pointRadius: 3,
                        pointColor: '#3b8bba',
                        pointStrokeColor: 'rgba(60,141,188,1)',
                        pointHighlightFill: '#fff',
                        pointHighlightStroke: 'rgba(60,141,188,1)',
                        data: [28, 48, 40, 19, 86, 27, 90, 45, 67, 89, 34, 56]
                    }
                ]
            };

            var salesChartOptions = {
                maintainAspectRatio: false,
                responsive: true,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    x: {
                        grid: {
                            display: false
                        }
                    },
                    y: {
                        grid: {
                            display: true
                        }
                    }
                }
            };

            new Chart(ctx, {
                type: 'line',
                data: salesChartData,
                options: salesChartOptions
            });
        }
    },

    // Khởi tạo biểu đồ doanh thu
    initRevenueChart: function() {
        var revenueChartCanvas = document.getElementById('revenueChart');
        if (revenueChartCanvas) {
            var ctx = revenueChartCanvas.getContext('2d');
            var revenueChartData = {
                labels: ['T1', 'T2', 'T3', 'T4', 'T5', 'T6'],
                datasets: [
                    {
                        label: 'Doanh thu',
                        backgroundColor: 'rgba(210, 214, 222, 1)',
                        borderColor: 'rgba(210, 214, 222, 1)',
                        pointRadius: 3,
                        pointColor: 'rgba(210, 214, 222, 1)',
                        pointStrokeColor: '#c1c7d1',
                        pointHighlightFill: '#fff',
                        pointHighlightStroke: 'rgba(220,220,220,1)',
                        data: [65, 59, 80, 81, 56, 55]
                    }
                ]
            };

            var revenueChartOptions = {
                maintainAspectRatio: false,
                responsive: true,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    x: {
                        grid: {
                            display: false
                        }
                    },
                    y: {
                        grid: {
                            display: true
                        }
                    }
                }
            };

            new Chart(ctx, {
                type: 'bar',
                data: revenueChartData,
                options: revenueChartOptions
            });
        }
    }
};

// Initialize when document is ready
$(document).ready(function() {
    Dashboard.init();
}); 