// XuatKho.js - Quản lý xuất kho
$(document).ready(function() {
    // Khởi tạo DataTable
    if ($.fn.DataTable) {
        $('#xuatKhoTable').DataTable({
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

    // Xử lý nút xác nhận
    $(document).on('click', '[data-action="xac-nhan"]', function() {
        var id = $(this).data('id');
        if (confirm('Bạn có chắc chắn muốn xác nhận phiếu xuất kho này?')) {
            xacNhanPhieuXuatKho(id);
        }
    });
});

// Mở modal tạo mới
function openCreateModal() {
    $.get('/XuatKho/Create', function(data) {
        $('#xuatKhoModalContent').html(data);
        $('#xuatKhoModal').modal('show');
    });
}

// Mở modal chi tiết
function openDetailsModal(id) {
    $.get('/XuatKho/Details/' + id, function(data) {
        $('#detailsModalContent').html(data);
        $('#detailsModal').modal('show');
    });
}

// Mở modal chỉnh sửa
function openEditModal(id) {
    $.get('/XuatKho/Edit/' + id, function(data) {
        $('#xuatKhoModalContent').html(data);
        $('#xuatKhoModal').modal('show');
    });
}

// Mở modal xóa
function openDeleteModal(id) {
    $.get('/XuatKho/Delete/' + id, function(data) {
        $('#deleteModalContent').html(data);
        $('#deleteModal').modal('show');
    });
}

// Xác nhận phiếu xuất kho
function xacNhanPhieuXuatKho(id) {
    $.post('/XuatKho/XacNhan/' + id, function(response) {
        if (response.success) {
            toastr.success('Xác nhận phiếu xuất kho thành công!');
            setTimeout(function() {
                location.reload();
            }, 1000);
        } else {
            toastr.error(response.message || 'Có lỗi xảy ra!');
        }
    }).fail(function() {
        toastr.error('Có lỗi xảy ra khi xác nhận phiếu xuất kho!');
    });
}

// Lưu phiếu xuất kho
function savePhieuXuatKho() {
    var form = $('#xuatKhoForm');
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
                toastr.success('Lưu phiếu xuất kho thành công!');
                $('#xuatKhoModal').modal('hide');
                setTimeout(function() {
                    location.reload();
                }, 1000);
            } else {
                toastr.error(response.message || 'Có lỗi xảy ra!');
            }
        },
        error: function() {
            toastr.error('Có lỗi xảy ra khi lưu phiếu xuất kho!');
        }
    });
}

// Xóa phiếu xuất kho
function deletePhieuXuatKho(id) {
    $.post('/XuatKho/DeleteConfirmed/' + id, function(response) {
        if (response.success) {
            toastr.success('Xóa phiếu xuất kho thành công!');
            $('#deleteModal').modal('hide');
            setTimeout(function() {
                location.reload();
            }, 1000);
        } else {
            toastr.error(response.message || 'Có lỗi xảy ra!');
        }
    }).fail(function() {
        toastr.error('Có lỗi xảy ra khi xóa phiếu xuất kho!');
    });
}

// Thêm dòng chi tiết
function addChiTietRow() {
    var rowCount = $('.chi-tiet-row').length;
    var newRow = `
        <tr class="chi-tiet-row">
            <td>
                <select name="ChiTietXuatKhos[${rowCount}].SanPhamId" class="form-control san-pham-select" required>
                    <option value="">Chọn sản phẩm</option>
                    @foreach (var sanPham in ViewBag.SanPhams ?? new List<SanPham>())
                    {
                        <option value="@sanPham.Id" data-don-gia="@sanPham.GiaBan">@sanPham.TenSanPham</option>
                    }
                </select>
            </td>
            <td>
                <input type="number" name="ChiTietXuatKhos[${rowCount}].SoLuong" class="form-control so-luong" min="1" required>
            </td>
            <td>
                <input type="number" name="ChiTietXuatKhos[${rowCount}].DonGia" class="form-control don-gia" min="0" step="1000" required>
            </td>
            <td>
                <input type="number" name="ChiTietXuatKhos[${rowCount}].ThanhTien" class="form-control thanh-tien" readonly>
            </td>
            <td>
                <button type="button" class="btn btn-danger btn-sm" onclick="removeChiTietRow(this)">
                    <i class="fas fa-trash"></i>
                </button>
            </td>
        </tr>
    `;
    $('#chiTietTable tbody').append(newRow);
    updateTongTien();
}

// Xóa dòng chi tiết
function removeChiTietRow(button) {
    $(button).closest('tr').remove();
    updateTongTien();
}

// Cập nhật tổng tiền
function updateTongTien() {
    var tongTien = 0;
    $('.thanh-tien').each(function() {
        var thanhTien = parseFloat($(this).val()) || 0;
        tongTien += thanhTien;
    });
    $('#TongTien').val(tongTien.toLocaleString('vi-VN'));
}

// Xử lý sự kiện thay đổi sản phẩm
$(document).on('change', '.san-pham-select', function() {
    var row = $(this).closest('tr');
    var selectedOption = $(this).find('option:selected');
    var donGia = selectedOption.data('don-gia') || 0;
    row.find('.don-gia').val(donGia);
    calculateThanhTien(row);
});

// Xử lý sự kiện thay đổi số lượng hoặc đơn giá
$(document).on('input', '.so-luong, .don-gia', function() {
    var row = $(this).closest('tr');
    calculateThanhTien(row);
});

// Tính thành tiền
function calculateThanhTien(row) {
    var soLuong = parseFloat(row.find('.so-luong').val()) || 0;
    var donGia = parseFloat(row.find('.don-gia').val()) || 0;
    var thanhTien = soLuong * donGia;
    row.find('.thanh-tien').val(thanhTien);
    updateTongTien();
}
