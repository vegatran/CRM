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
            console.log('Edit định mức clicked, id:', id);
            ChiTietSanXuat.loadEditDinhMucModal(id);
        });

        // Xóa định mức
        $(document).on('click', '.delete-dinhmuc-btn', function() {
            var id = $(this).data('id');
            ChiTietSanXuat.loadDeleteDinhMucModal(id);
        });

        // Thêm quy trình mới
        $(document).on('click', '#addQuyTrinhBtn', function() {
            var sanPhamId = $(this).data('san-pham-id');
            ChiTietSanXuat.loadCreateQuyTrinhModal(sanPhamId);
        });

        // Sửa quy trình
        $(document).on('click', '.edit-quytrinh-btn', function() {
            var id = $(this).data('id');
            console.log('Edit quy trình clicked, id:', id);
            ChiTietSanXuat.loadEditQuyTrinhModal(id);
        });

        // Xóa quy trình
        $(document).on('click', '.delete-quytrinh-btn', function() {
            var id = $(this).data('id');
            var sanPhamId = $(this).data('san-pham-id');
            ChiTietSanXuat.loadDeleteQuyTrinhModal(id, sanPhamId);
        });
    },

    loadCreateDinhMucModal: function(sanPhamId) {
        $.get('/DinhMucNguyenLieu/Create', { sanPhamId: sanPhamId }, function(data) {
            $('#dinhMucModalContent').html(data);
            $('#dinhMucModal').modal('show');
        });
    },

    loadEditDinhMucModal: function(id) {
        console.log('Loading edit định mức modal for id:', id);
        $.get('/DinhMucNguyenLieu/Edit/' + id, function(data) {
            console.log('Edit định mức modal data loaded, showing modal');
            $('#dinhMucModalContent').html(data);
            $('#dinhMucModal').modal('show');
        }).fail(function(xhr, status, error) {
            console.error('Error loading edit định mức modal:', error);
            console.error('Status:', status);
            console.error('Response:', xhr.responseText);
        });
    },

    loadDeleteDinhMucModal: function(id) {
        $.get('/DinhMucNguyenLieu/Delete/' + id, function(data) {
            $('#dinhMucModalContent').html(data);
            $('#dinhMucModal').modal('show');
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
    },

    loadCreateQuyTrinhModal: function(sanPhamId) {
        QuyTrinhSanXuat.openCreateModal(sanPhamId);
    },

    loadEditQuyTrinhModal: function(id) {
        console.log('Loading edit quy trình modal for id:', id);
        QuyTrinhSanXuat.openEditModal(id);
    },

    loadDeleteQuyTrinhModal: function(id, sanPhamId) {
        $.get('/QuyTrinhSanXuat/Delete/' + id, function(data) {
            $('#quyTrinhModalContent').html(data);
            $('#quyTrinhModal').modal('show');
        });
    }
};

$(document).ready(function() {
    console.log('Document ready, initializing ChiTietSanXuat');
    ChiTietSanXuat.init();
}); 