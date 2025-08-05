// ChiTietSanXuat.js - Quản lý chi tiết sản xuất

const ChiTietSanXuat = {
    init: function() {
        console.log('ChiTietSanXuat.init() called');
        this.loadQuyTrinhSanXuat();
        this.bindDinhMucEvents();
    },

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
    },

    bindDinhMucEvents: function() {
        // Thêm định mức mới
        $(document).on('click', '#addDinhMucBtn', function() {
            var sanPhamId = $('#dinhMucList').data('san-pham-id');
            ChiTietSanXuat.loadCreateDinhMucModal(sanPhamId);
        });

        // Sửa định mức
        $(document).on('click', '.edit-dinhmuc-btn', function() {
            var id = $(this).data('id');
            ChiTietSanXuat.loadEditDinhMucModal(id);
        });

        // Xóa định mức
        $(document).on('click', '.delete-dinhmuc-btn', function() {
            var id = $(this).data('id');
            ChiTietSanXuat.loadDeleteDinhMucModal(id);
        });
    },

    loadCreateDinhMucModal: function(sanPhamId) {
        $.get('/DinhMucNguyenLieu/Create', { sanPhamId: sanPhamId }, function(data) {
            $('#modalContainer').html(data);
            $('#createDinhMucModal').modal('show');
        });
    },

    loadEditDinhMucModal: function(id) {
        $.get('/DinhMucNguyenLieu/Edit/' + id, function(data) {
            $('#modalContainer').html(data);
            $('#editDinhMucModal').modal('show');
        });
    },

    loadDeleteDinhMucModal: function(id) {
        $.get('/DinhMucNguyenLieu/Delete/' + id, function(data) {
            $('#modalContainer').html(data);
            $('#deleteDinhMucModal').modal('show');
        });
    },

    reloadDinhMucList: function() {
        var sanPhamId = $('#dinhMucList').data('san-pham-id');
        if (sanPhamId) {
            var url = '/DinhMucNguyenLieu/BySanPham/' + sanPhamId;
            $.get(url, function(data) {
                $('#dinhMucList').html(data);
            });
        }
    }
};

$(document).ready(function() {
    console.log('Document ready, initializing ChiTietSanXuat');
    ChiTietSanXuat.init();
}); 