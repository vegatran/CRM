// ChiTietSanXuat.js - Quản lý chi tiết sản xuất

const ChiTietSanXuat = {
    // Khởi tạo
    init: function() {
        console.log('ChiTietSanXuat.init() called');
        this.loadDinhMucNguyenLieu();
        this.loadQuyTrinhSanXuat();
    },

    // Load định mức nguyên liệu
    loadDinhMucNguyenLieu: function() {
        var sanPhamId = $('#dinhMucList').data('san-pham-id');
        console.log('Loading DinhMucNguyenLieu for sanPhamId:', sanPhamId);
        if (sanPhamId) {
            var url = '/DinhMucNguyenLieu/BySanPham/' + sanPhamId;
            console.log('Calling URL:', url);
            $.get(url, function(data) {
                console.log('DinhMucNguyenLieu loaded successfully');
                $('#dinhMucList').html(data);
            }).fail(function(xhr, status, error) {
                console.error('Error loading DinhMucNguyenLieu:', error);
                console.error('Status:', status);
                console.error('Response:', xhr.responseText);
                $('#dinhMucList').html('<div class="alert alert-danger">Lỗi khi tải định mức nguyên liệu</div>');
            });
        } else {
            console.error('No sanPhamId found for DinhMucNguyenLieu');
        }
    },

    // Load quy trình sản xuất
    loadQuyTrinhSanXuat: function() {
        var sanPhamId = $('#quyTrinhList').data('san-pham-id');
        console.log('Loading QuyTrinhSanXuat for sanPhamId:', sanPhamId);
        if (sanPhamId) {
            var url = '/QuyTrinhSanXuat/BySanPham/' + sanPhamId;
            console.log('Calling URL:', url);
            $.get(url, function(data) {
                console.log('QuyTrinhSanXuat loaded successfully');
                $('#quyTrinhList').html(data);
            }).fail(function(xhr, status, error) {
                console.error('Error loading QuyTrinhSanXuat:', error);
                console.error('Status:', status);
                console.error('Response:', xhr.responseText);
                $('#quyTrinhList').html('<div class="alert alert-danger">Lỗi khi tải quy trình sản xuất</div>');
            });
        } else {
            console.error('No sanPhamId found for QuyTrinhSanXuat');
        }
    }
};

// Initialize when document is ready
$(document).ready(function() {
    console.log('Document ready, initializing ChiTietSanXuat');
    ChiTietSanXuat.init();
}); 