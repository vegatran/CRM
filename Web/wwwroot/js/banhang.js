const BanHang = (function() {
    'use strict';

    // Khởi tạo DataTable
    function initDataTable() {
        Common.initDataTable('#banHangTable');
    }

    // Xác nhận bán hàng
    function xacNhanBanHang(id) {
        if (confirm('Bạn có chắc chắn muốn xác nhận phiếu bán hàng này?')) {
            $.post('/BanHang/XacNhan', { id: id })
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
                    toastr.error('Có lỗi xảy ra khi xác nhận phiếu bán hàng!');
                });
        }
    }

    // Mở modal tạo mới
    function openCreateModal() {
        $('#banHangModalLabel').text('Tạo phiếu bán hàng');
        $('#banHangModalContent').load('/BanHang/CreateContent', function() {
            $('#banHangModal').modal('show');
            initCreateForm();
        });
    }

    // Mở modal chỉnh sửa
    function openEditModal(id) {
        $('#banHangModalLabel').text('Chỉnh sửa phiếu bán hàng');
        $('#banHangModalContent').load('/BanHang/EditContent/' + id, function() {
            $('#banHangModal').modal('show');
            initEditForm();
        });
    }

    // Mở modal chi tiết
    function openDetailsModal(id) {
        $('#detailsModalContent').load('/BanHang/DetailsContent/' + id, function() {
            $('#detailsModal').modal('show');
        });
    }

    // Mở modal xóa
    function openDeleteModal(id) {
        $('#deleteModalContent').load('/BanHang/DeleteContent/' + id, function() {
            $('#deleteModal').modal('show');
        });
    }

    // Xóa bán hàng
    function deleteBanHang(id) {
        $.post('/BanHang/Delete/' + id)
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
                toastr.error('Có lỗi xảy ra khi xóa phiếu bán hàng!');
            });
    }

    // Khởi tạo form tạo phiếu bán hàng
    function initCreateForm() {
        // Bind events cho form
        $(document).on('input', '.so-luong, .don-gia', function() {
            calculateThanhTien($(this).closest('.chi-tiet-item'));
        });

        $(document).on('change', '.san-pham', function() {
            var selectedOption = $(this).find('option:selected');
            var donGia = selectedOption.data('don-gia') || 0;
            $(this).closest('.chi-tiet-item').find('.don-gia').val(donGia.toLocaleString('vi-VN'));
            calculateThanhTien($(this).closest('.chi-tiet-item'));
        });

        // Format số tiền khi nhập
        $(document).on('blur', '.don-gia, #GiamGia', function() {
            var value = $(this).val().replace(/[^\d]/g, '');
            if (value) {
                $(this).val(parseInt(value).toLocaleString('vi-VN'));
            }
        });

        $(document).on('click', '[data-action="remove-chi-tiet"]', function() {
            removeChiTiet($(this));
        });

        $(document).on('click', '[data-action="add-chi-tiet"]', function() {
            addChiTiet();
        });

        // Bind form submit
        $('#banHangForm').on('submit', function(e) {
            e.preventDefault();
            handleCreateFormSubmit();
        });
    }

    // Khởi tạo form chỉnh sửa
    function initEditForm() {
        // Bind events cho form
        $(document).on('input', '.so-luong, .don-gia', function() {
            calculateThanhTien($(this).closest('.chi-tiet-item'));
        });

        $(document).on('change', '.san-pham', function() {
            var selectedOption = $(this).find('option:selected');
            var donGia = selectedOption.data('don-gia') || 0;
            $(this).closest('.chi-tiet-item').find('.don-gia').val(donGia.toLocaleString('vi-VN'));
            calculateThanhTien($(this).closest('.chi-tiet-item'));
        });

        // Format số tiền khi nhập
        $(document).on('blur', '.don-gia, #GiamGia', function() {
            var value = $(this).val().replace(/[^\d]/g, '');
            if (value) {
                $(this).val(parseInt(value).toLocaleString('vi-VN'));
            }
        });

        $(document).on('click', '[data-action="remove-chi-tiet"]', function() {
            removeChiTiet($(this));
        });

        $(document).on('click', '[data-action="add-chi-tiet"]', function() {
            addChiTiet();
        });

        // Bind form submit
        $('#banHangEditForm').on('submit', function(e) {
            e.preventDefault();
            handleEditFormSubmit();
        });
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
        
        var giamGia = parseFloat($('#GiamGia').val().replace(/[^\d]/g, '')) || 0;
        var thanhToan = tongTien - giamGia;
        
        $('#tongTien').val(tongTien.toLocaleString('vi-VN'));
        $('#thanhToan').val(thanhToan.toLocaleString('vi-VN'));
    }

    // Thêm chi tiết
    function addChiTiet() {
        var template = $('.chi-tiet-item').first().clone();
        template.find('input, select').val('');
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
        var chiTietData = [];
        $('.chi-tiet-item').each(function() {
            var item = $(this);
            var sanPhamId = item.find('.san-pham').val();
            var soLuong = item.find('.so-luong').val();
            var donGia = item.find('.don-gia').val().replace(/[^\d]/g, '');
            
            if (sanPhamId && soLuong && donGia) {
                var chiTiet = {
                    sanPhamId: sanPhamId,
                    soLuong: soLuong,
                    donGia: donGia
                };
                chiTietData.push(chiTiet);
            }
        });
        
        if (chiTietData.length === 0) {
            toastr.error('Vui lòng thêm ít nhất một chi tiết bán hàng!');
            return;
        }
        
        var formData = {
            phieuBanHang: {
                khachHangId: $('#KhachHangId').val(),
                ngayBan: $('#NgayBan').val(),
                ghiChu: $('#GhiChu').val(),
                giamGia: $('#GiamGia').val().replace(/[^\d]/g, '') || 0
            },
            chiTietBanHangs: chiTietData
        };
        
        $.post('/BanHang/Create', formData)
            .done(function(result) {
                if (result.success) {
                    toastr.success(result.message);
                    $('#banHangModal').modal('hide');
                    setTimeout(function() {
                        location.reload();
                    }, 1000);
                } else {
                    toastr.error(result.message);
                }
            })
            .fail(function() {
                toastr.error('Có lỗi xảy ra khi tạo phiếu bán hàng!');
            });
    }

    // Xử lý submit form chỉnh sửa
    function handleEditFormSubmit() {
        var chiTietData = [];
        $('.chi-tiet-item').each(function() {
            var item = $(this);
            var chiTietId = item.find('.chi-tiet-id').val();
            var sanPhamId = item.find('.san-pham').val();
            var soLuong = item.find('.so-luong').val();
            var donGia = item.find('.don-gia').val().replace(/[^\d]/g, '');
            
            if (sanPhamId && soLuong && donGia) {
                var chiTiet = {
                    id: chiTietId,
                    sanPhamId: sanPhamId,
                    soLuong: soLuong,
                    donGia: donGia
                };
                chiTietData.push(chiTiet);
            }
        });
        
        if (chiTietData.length === 0) {
            toastr.error('Vui lòng thêm ít nhất một chi tiết bán hàng!');
            return;
        }
        
        var formData = {
            phieuBanHang: {
                id: $('#Id').val(),
                khachHangId: $('#KhachHangId').val(),
                ngayBan: $('#NgayBan').val(),
                ghiChu: $('#GhiChu').val(),
                giamGia: $('#GiamGia').val().replace(/[^\d]/g, '') || 0
            },
            chiTietBanHangs: chiTietData
        };
        
        $.post('/BanHang/Edit', formData)
            .done(function(result) {
                if (result.success) {
                    toastr.success(result.message);
                    $('#banHangModal').modal('hide');
                    setTimeout(function() {
                        location.reload();
                    }, 1000);
                } else {
                    toastr.error(result.message);
                }
            })
            .fail(function() {
                toastr.error('Có lỗi xảy ra khi cập nhật phiếu bán hàng!');
            });
    }

    // Khởi tạo module
    function init() {
        // Khởi tạo DataTable nếu có
        if ($('#banHangTable').length > 0) {
            initDataTable();
        }
        
        // Bind events
        $(document).on('click', '[data-action="xac-nhan"]', function() {
            const id = $(this).data('id');
            xacNhanBanHang(id);
        });
    }

    // Public API
    return {
        init: init,
        xacNhanBanHang: xacNhanBanHang,
        openCreateModal: openCreateModal,
        openEditModal: openEditModal,
        openDetailsModal: openDetailsModal,
        openDeleteModal: openDeleteModal,
        deleteBanHang: deleteBanHang,
        addChiTiet: addChiTiet,
        removeChiTiet: removeChiTiet,
        calculateThanhTien: calculateThanhTien,
        calculateTongTien: calculateTongTien
    };
})();

// Khởi tạo khi document ready
$(document).ready(function() {
    BanHang.init();
});

// Global functions for onclick handlers
function openCreateModal() {
    BanHang.openCreateModal();
}

function openEditModal(id) {
    BanHang.openEditModal(id);
}

function openDetailsModal(id) {
    BanHang.openDetailsModal(id);
}

function openDeleteModal(id) {
    BanHang.openDeleteModal(id);
}

function deleteBanHang(id) {
    BanHang.deleteBanHang(id);
}

function xacNhanBanHang(id) {
    BanHang.xacNhanBanHang(id);
}
