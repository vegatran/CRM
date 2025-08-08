// KhachHang.js - Quản lý khách hàng
$(document).ready(function() {
    // Khởi tạo DataTable
    if ($.fn.DataTable) {
        $('#khachHangTable').DataTable({
            "language": {
                "sProcessing": "Đang xử lý...",
                "sLengthMenu": "Xem _MENU_ mục",
                "sZeroRecords": "Không tìm thấy dữ liệu",
                "sInfo": "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
                "sInfoEmpty": "Đang xem 0 đến 0 trong tổng số 0 mục",
                "sInfoFiltered": "(được lọc từ _MAX_ mục)",
                "sSearch": "Tìm kiếm:",
                "sEmptyTable": "Không có dữ liệu trong bảng",
                "sLoadingRecords": "Đang tải...",
                "oPaginate": {
                    "sFirst": "Đầu",
                    "sPrevious": "Trước",
                    "sNext": "Tiếp",
                    "sLast": "Cuối"
                }
            },
            "responsive": true,
            "autoWidth": false,
            "pageLength": 25,
            "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Tất cả"]]
        });
    }
});

// Mở modal tạo mới
function openCreateModal() {
    $.get('/KhachHang/Create', function(data) {
        $('#khachHangModalContent').html(data);
        $('#khachHangModal').modal('show');
    });
}

// Mở modal chi tiết
function openDetailsModal(id) {
    $.get('/KhachHang/Details/' + id, function(data) {
        $('#detailsModalContent').html(data);
        $('#detailsModal').modal('show');
    });
}

// Mở modal chỉnh sửa
function openEditModal(id) {
    $.get('/KhachHang/Edit/' + id, function(data) {
        $('#khachHangModalContent').html(data);
        $('#khachHangModal').modal('show');
    });
}

// Mở modal xóa
function openDeleteModal(id) {
    $.get('/KhachHang/Delete/' + id, function(data) {
        $('#deleteModalContent').html(data);
        $('#deleteModal').modal('show');
    });
}

// Lưu khách hàng
function saveKhachHang() {
    var form = $('#khachHangForm');
    if (!form[0].checkValidity()) {
        form[0].reportValidity();
        return;
    }

    var formData = new FormData(form[0]);
    
    $.ajax({
        url: form.attr('action'),
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function(response) {
            if (response.success) {
                toastr.success('Lưu khách hàng thành công!');
                $('#khachHangModal').modal('hide');
                setTimeout(function() {
                    location.reload();
                }, 1000);
            } else {
                toastr.error(response.message || 'Có lỗi xảy ra!');
            }
        },
        error: function() {
            toastr.error('Có lỗi xảy ra khi lưu khách hàng!');
        }
    });
}

// Xóa khách hàng
function deleteKhachHang(id) {
    $.post('/KhachHang/DeleteConfirmed/' + id, function(response) {
        if (response.success) {
            toastr.success('Xóa khách hàng thành công!');
            $('#deleteModal').modal('hide');
            setTimeout(function() {
                location.reload();
            }, 1000);
        } else {
            toastr.error(response.message || 'Có lỗi xảy ra!');
        }
    }).fail(function() {
        toastr.error('Có lỗi xảy ra khi xóa khách hàng!');
    });
}

// Tạo mã khách hàng tự động
function generateMaKhachHang() {
    var tenKhachHang = $('#TenKhachHang').val();
    if (tenKhachHang) {
        var maKhachHang = 'KH' + tenKhachHang.substring(0, 3).toUpperCase() + Date.now().toString().substring(8);
        $('#MaKhachHang').val(maKhachHang);
    }
}

// Xử lý sự kiện thay đổi tên khách hàng
$(document).on('input', '#TenKhachHang', function() {
    if (!$('#MaKhachHang').val()) {
        generateMaKhachHang();
    }
});

// Validate email
function validateEmail(email) {
    var re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

// Validate số điện thoại
function validatePhone(phone) {
    var re = /^[0-9]{10,11}$/;
    return re.test(phone.replace(/\s/g, ''));
}

// Xử lý validate form
$(document).on('submit', '#khachHangForm', function(e) {
    var email = $('#Email').val();
    var phone = $('#SoDienThoai').val();
    
    if (email && !validateEmail(email)) {
        toastr.error('Email không hợp lệ!');
        e.preventDefault();
        return false;
    }
    
    if (phone && !validatePhone(phone)) {
        toastr.error('Số điện thoại không hợp lệ!');
        e.preventDefault();
        return false;
    }
    
    return true;
});
