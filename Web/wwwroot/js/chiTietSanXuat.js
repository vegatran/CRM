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
        $.get('/DinhMucNguyenLieu/Edit/' + id, function(data) {
            $('#dinhMucModalContent').html(data);
            $('#dinhMucModal').modal('show');
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
        $.get('/QuyTrinhSanXuat/Create', { sanPhamId: sanPhamId }, function(data) {
            $('#quyTrinhModalContent').html(data);
            $('#quyTrinhModal').modal('show');
        });
    },

    loadEditQuyTrinhModal: function(id) {
        $.get('/QuyTrinhSanXuat/Edit/' + id, function(data) {
            $('#quyTrinhModalContent').html(data);
            $('#quyTrinhModal').modal('show');
        });
    },

    loadDeleteQuyTrinhModal: function(id, sanPhamId) {
        if (confirm('Bạn có chắc chắn muốn xóa quy trình này?')) {
            $.post('/QuyTrinhSanXuat/Delete/' + id, function(response) {
                if (response.success) {
                    toastr.success(response.message);
                    // Reload the entire page to update cost analysis
                    setTimeout(function() {
                        location.reload();
                    }, 1000);
                } else {
                    toastr.error(response.message);
                }
            }).fail(function() {
                toastr.error('Có lỗi xảy ra khi xóa quy trình!');
            });
        }
    }
};

$(document).ready(function() {
    console.log('Document ready, initializing ChiTietSanXuat');
    ChiTietSanXuat.init();
}); 