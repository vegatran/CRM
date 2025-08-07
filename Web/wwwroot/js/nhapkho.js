const NhapKho = (function() {
    'use strict';

    // Khởi tạo DataTable
    function initDataTable() {
        Common.initDataTable('#nhapKhoTable');
    }

    // Xác nhận nhập kho
    function xacNhanNhapKho(id) {
        if (confirm('Bạn có chắc chắn muốn xác nhận phiếu nhập kho này?')) {
            $.post('/NhapKho/XacNhan', { id: id })
                .done(function(result) {
                    if (result.success) {
                        toastr.success(result.message);
                        setTimeout(function() {
                            location.reload();
                        }, 1000);
                    } else {
                        toastr.error(result.message);
                    }
                })
                .fail(function() {
                    toastr.error('Có lỗi xảy ra khi xác nhận phiếu nhập kho!');
                });
        }
    }

    // Mở modal tạo mới
    function openCreateModal() {
        $('#nhapKhoModalLabel').text('Tạo phiếu nhập kho');
        $('#nhapKhoModalContent').load('/NhapKho/CreateContent', function() {
            $('#nhapKhoModal').modal('show');
            initCreateForm();
        });
    }

    // Mở modal chỉnh sửa
    function openEditModal(id) {
        $('#nhapKhoModalLabel').text('Chỉnh sửa phiếu nhập kho');
        $('#nhapKhoModalContent').load('/NhapKho/EditContent/' + id, function() {
            $('#nhapKhoModal').modal('show');
            initEditForm();
        });
    }

    // Mở modal chi tiết
    function openDetailsModal(id) {
        $('#detailsModalContent').load('/NhapKho/DetailsContent/' + id, function() {
            $('#detailsModal').modal('show');
        });
    }

    // Mở modal xóa
    function openDeleteModal(id) {
        $('#deleteModalContent').load('/NhapKho/DeleteContent/' + id, function() {
            $('#deleteModal').modal('show');
        });
    }

    // Xóa nhập kho
    function deleteNhapKho(id) {
        $.post('/NhapKho/Delete/' + id)
            .done(function(result) {
                if (result.success) {
                    toastr.success(result.message);
                    $('#deleteModal').modal('hide');
                    setTimeout(function() {
                        location.reload();
                    }, 1000);
                } else {
                    toastr.error(result.message);
                }
            })
            .fail(function() {
                toastr.error('Có lỗi xảy ra khi xóa phiếu nhập kho!');
            });
    }

    // Khởi tạo form tạo phiếu nhập kho
    function initCreateForm() {
        // Bind events cho form
        $(document).on('change', '.loai-hang', function() {
            onLoaiHangChange($(this));
        });

        $(document).on('input', '.so-luong, .don-gia', function() {
            calculateThanhTien($(this).closest('.chi-tiet-item'));
        });

        $(document).on('click', '[data-action="remove-chi-tiet"]', function() {
            removeChiTiet($(this));
        });

        $(document).on('click', '[data-action="add-chi-tiet"]', function() {
            addChiTiet();
        });

        // Bind form submit
        $('#nhapKhoForm').on('submit', function(e) {
            e.preventDefault();
            handleCreateFormSubmit();
        });
    }

    // Khởi tạo form chỉnh sửa
    function initEditForm() {
        // Bind events cho form
        $(document).on('change', '.loai-hang', function() {
            onLoaiHangChange($(this));
        });

        $(document).on('input', '.so-luong, .don-gia', function() {
            calculateThanhTien($(this).closest('.chi-tiet-item'));
        });

        $(document).on('click', '[data-action="remove-chi-tiet"]', function() {
            removeChiTiet($(this));
        });

        $(document).on('click', '[data-action="add-chi-tiet"]', function() {
            addChiTiet();
        });

        // Bind form submit
        $('#nhapKhoEditForm').on('submit', function(e) {
            e.preventDefault();
            handleEditFormSubmit();
        });
    }

    // Xử lý thay đổi loại hàng
    function onLoaiHangChange(select) {
        var item = $(select).closest('.chi-tiet-item');
        var hangHoaSelect = item.find('.hang-hoa');
        var loaiHang = $(select).val();
        
        hangHoaSelect.empty().append('<option value="">-- Chọn hàng hóa --</option>');
        
        if (loaiHang === 'nguyenlieu') {
            // Load nguyên liệu
            $.get('/NguyenLieu/GetAllForSelect')
                .done(function(data) {
                    data.forEach(function(item) {
                        hangHoaSelect.append(`<option value="${item.id}" data-gia="${item.giaNhap}">${item.tenNguyenLieu}</option>`);
                    });
                });
        } else if (loaiHang === 'sanpham') {
            // Load sản phẩm
            $.get('/SanPham/GetAllForSelect')
                .done(function(data) {
                    data.forEach(function(item) {
                        hangHoaSelect.append(`<option value="${item.id}" data-gia="${item.giaNhap}">${item.tenSanPham}</option>`);
                    });
                });
        }
        
        hangHoaSelect.prop('disabled', false);
    }

    // Tính thành tiền
    function calculateThanhTien(item) {
        var soLuong = parseInt(item.find('.so-luong').val()) || 0;
        var donGia = parseFloat(item.find('.don-gia').val().replace(/[^\d]/g, '')) || 0;
        var thanhTien = soLuong * donGia;
        
        item.find('.thanh-tien').val(thanhTien.toLocaleString('vi-VN'));
        calculateTongTien();
    }

    // Tính tổng tiền
    function calculateTongTien() {
        var tongTien = 0;
        $('.chi-tiet-item').each(function() {
            var soLuong = parseInt($(this).find('.so-luong').val()) || 0;
            var donGia = parseFloat($(this).find('.don-gia').val().replace(/[^\d]/g, '')) || 0;
            tongTien += soLuong * donGia;
        });
        
        $('#tongTien').val(tongTien.toLocaleString('vi-VN'));
    }

    // Thêm chi tiết
    function addChiTiet() {
        var template = $('.chi-tiet-item').first().clone();
        template.find('input, select').val('');
        template.find('.hang-hoa').prop('disabled', true);
        $('#chiTietContainer').append(template);
    }

    // Xóa chi tiết
    function removeChiTiet(button) {
        if ($('.chi-tiet-item').length > 1) {
            button.closest('.chi-tiet-item').remove();
            calculateTongTien();
        }
    }

    // Xử lý submit form tạo mới
    function handleCreateFormSubmit() {
        // Validation
        if (!$('#NhaCungCapId').val()) {
            toastr.error('Vui lòng chọn nhà cung cấp!');
            return;
        }

        if (!$('#NgayNhap').val()) {
            toastr.error('Vui lòng chọn ngày nhập!');
            return;
        }

        // Kiểm tra ngày nhập không được trong tương lai
        var ngayNhap = new Date($('#NgayNhap').val());
        var today = new Date();
        today.setHours(0, 0, 0, 0);
        
        if (ngayNhap > today) {
            toastr.error('Ngày nhập không được trong tương lai!');
            return;
        }

        var chiTietData = [];
        var hasValidChiTiet = false;
        var hasError = false;
        
        $('.chi-tiet-item').each(function() {
            if (hasError) return false;
            
            var item = $(this);
            var loaiHang = item.find('.loai-hang').val();
            var hangHoaId = item.find('.hang-hoa').val();
            var soLuong = item.find('.so-luong').val();
            var donGia = item.find('.don-gia').val().replace(/[^\d]/g, '');
            
            if (loaiHang && hangHoaId && soLuong && donGia) {
                // Kiểm tra số lượng > 0
                if (parseInt(soLuong) <= 0) {
                    toastr.error('Số lượng phải lớn hơn 0!');
                    hasError = true;
                    return false;
                }
                
                // Kiểm tra đơn giá > 0
                if (parseFloat(donGia) <= 0) {
                    toastr.error('Đơn giá phải lớn hơn 0!');
                    hasError = true;
                    return false;
                }
                
                hasValidChiTiet = true;
                var chiTiet = {
                    loaiHang: loaiHang,
                    hangHoaId: hangHoaId,
                    soLuong: parseInt(soLuong),
                    donGia: parseFloat(donGia)
                };
                
                if (loaiHang === 'nguyenlieu') {
                    chiTiet.nguyenLieuId = parseInt(hangHoaId);
                } else {
                    chiTiet.sanPhamId = parseInt(hangHoaId);
                }
                
                chiTietData.push(chiTiet);
            }
        });
        
        if (hasError) {
            return;
        }
        
        if (!hasValidChiTiet) {
            toastr.error('Vui lòng thêm ít nhất một chi tiết nhập kho hợp lệ!');
            return;
        }
        
        var formData = {
            phieuNhapKho: {
                nhaCungCapId: parseInt($('#NhaCungCapId').val()),
                ngayNhap: $('#NgayNhap').val(),
                ghiChu: $('#GhiChu').val() || ""
            },
            chiTietNhapKhos: chiTietData
        };
        
        console.log('Sending data:', formData);
        
        $.ajax({
            url: '/NhapKho/Create',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function(result) {
                if (result.success) {
                    toastr.success(result.message);
                    $('#nhapKhoModal').modal('hide');
                    setTimeout(function() {
                        location.reload();
                    }, 1000);
                } else {
                    toastr.error(result.message || 'Có lỗi xảy ra khi tạo phiếu nhập kho!');
                }
            },
            error: function(xhr, status, error) {
                console.error('Error:', xhr.responseText);
                toastr.error('Có lỗi xảy ra khi tạo phiếu nhập kho!');
            }
        });
    }

    // Xử lý submit form chỉnh sửa
    function handleEditFormSubmit() {
        var chiTietData = [];
        $('.chi-tiet-item').each(function() {
            var item = $(this);
            var chiTietId = item.find('.chi-tiet-id').val();
            var loaiHang = item.find('.loai-hang').val();
            var hangHoaId = item.find('.hang-hoa').val();
            var soLuong = item.find('.so-luong').val();
            var donGia = item.find('.don-gia').val().replace(/[^\d]/g, '');
            
            if (loaiHang && hangHoaId && soLuong && donGia) {
                var chiTiet = {
                    id: chiTietId,
                    loaiHang: loaiHang,
                    hangHoaId: hangHoaId,
                    soLuong: soLuong,
                    donGia: donGia
                };
                
                if (loaiHang === 'nguyenlieu') {
                    chiTiet.nguyenLieuId = hangHoaId;
                } else {
                    chiTiet.sanPhamId = hangHoaId;
                }
                
                chiTietData.push(chiTiet);
            }
        });
        
        if (chiTietData.length === 0) {
            toastr.error('Vui lòng thêm ít nhất một chi tiết nhập kho!');
            return;
        }
        
        var formData = {
            phieuNhapKho: {
                id: $('#Id').val(),
                soPhieu: $('#SoPhieu').val(),
                trangThai: $('#TrangThai').val(),
                nhaCungCapId: $('#NhaCungCapId').val(),
                ngayNhap: $('#NgayNhap').val(),
                ghiChu: $('#GhiChu').val()
            },
            chiTietNhapKhos: chiTietData
        };
        
        $.post('/NhapKho/Edit', formData)
            .done(function(result) {
                if (result.success) {
                    toastr.success(result.message);
                    $('#nhapKhoModal').modal('hide');
                    setTimeout(function() {
                        location.reload();
                    }, 1000);
                } else {
                    toastr.error(result.message);
                }
            })
            .fail(function() {
                toastr.error('Có lỗi xảy ra khi cập nhật phiếu nhập kho!');
            });
    }

    // Khởi tạo module
    function init() {
        // Khởi tạo DataTable nếu có
        if ($('#nhapKhoTable').length > 0) {
            initDataTable();
        }
        
        // Bind events
        $(document).on('click', '[data-action="xac-nhan"]', function() {
            const id = $(this).data('id');
            xacNhanNhapKho(id);
        });
    }

    // Public API
    return {
        init: init,
        xacNhanNhapKho: xacNhanNhapKho,
        openCreateModal: openCreateModal,
        openEditModal: openEditModal,
        openDetailsModal: openDetailsModal,
        openDeleteModal: openDeleteModal,
        deleteNhapKho: deleteNhapKho,
        addChiTiet: addChiTiet,
        removeChiTiet: removeChiTiet,
        calculateThanhTien: calculateThanhTien,
        calculateTongTien: calculateTongTien
    };
})();

// Khởi tạo khi document ready
$(document).ready(function() {
    NhapKho.init();
});

// Global functions for onclick handlers
function openCreateModal() {
    NhapKho.openCreateModal();
}

function openEditModal(id) {
    NhapKho.openEditModal(id);
}

function openDetailsModal(id) {
    NhapKho.openDetailsModal(id);
}

function openDeleteModal(id) {
    NhapKho.openDeleteModal(id);
}

function deleteNhapKho(id) {
    NhapKho.deleteNhapKho(id);
}
