// QuyTrinhSanXuat.js - Quản lý quy trình sản xuất

const QuyTrinhSanXuat = {
    // Khởi tạo
    init: function() {
        this.bindEvents();
    },

    // Bind events
    bindEvents: function() {
        // Currency formatting for ChiPhiNhanCong input
        $(document).on('input', '.currency-input', function() {
            var value = $(this).val().replace(/[^\d]/g, '');
            if (value) {
                // Format as currency with commas
                var formattedValue = parseInt(value).toLocaleString('vi-VN');
                $(this).val(formattedValue);
            }
        });

        // Create form submission
        $(document).on('submit', '#createQuyTrinhForm', function(e) {
            e.preventDefault();
            QuyTrinhSanXuat.handleCreateForm($(this));
        });

        // Edit form submission
        $(document).on('submit', '#editQuyTrinhForm', function(e) {
            e.preventDefault();
            QuyTrinhSanXuat.handleEditForm($(this));
        });

        // Delete form submission
        $(document).on('submit', '#deleteQuyTrinhForm', function(e) {
            e.preventDefault();
            QuyTrinhSanXuat.handleDeleteForm($(this));
        });
    },

    // Mở modal tạo mới
    openCreateModal: function(sanPhamId) {
        $.get('/QuyTrinhSanXuat/Create', { sanPhamId: sanPhamId }, function(data) {
            $('#quyTrinhModalContent').html(data);
            $('#quyTrinhModal').modal('show');
            
            // Format currency input sau khi modal được load (nếu có giá trị mặc định)
            setTimeout(function() {
                var currencyInput = $('#quyTrinhModal .currency-input');
                if (currencyInput.length > 0) {
                    var value = currencyInput.val();
                    if (value && !isNaN(value) && value !== '0') {
                        var formattedValue = parseInt(value).toLocaleString('vi-VN');
                        currencyInput.val(formattedValue);
                    }
                }
            }, 100);
        }).fail(function() {
            toastr.error('Không thể tải form tạo mới!');
        });
    },

    // Mở modal chỉnh sửa
    openEditModal: function(id) {
        $.get('/QuyTrinhSanXuat/Edit/' + id, function(data) {
            $('#quyTrinhModalContent').html(data);
            $('#quyTrinhModal').modal('show');
            
            // Format currency input sau khi modal được load
            setTimeout(function() {
                var currencyInput = $('#quyTrinhModal .currency-input');
                if (currencyInput.length > 0) {
                    var value = currencyInput.val();
                    if (value && !isNaN(value)) {
                        var formattedValue = parseInt(value).toLocaleString('vi-VN');
                        currencyInput.val(formattedValue);
                    }
                }
            }, 100);
        }).fail(function() {
            toastr.error('Không thể tải form chỉnh sửa!');
        });
    },

    // Xóa quy trình
    deleteQuyTrinh: function(id, sanPhamId) {
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
    },

    // Load danh sách quy trình
    loadQuyTrinhList: function(sanPhamId) {
        $.get('/QuyTrinhSanXuat/BySanPham/' + sanPhamId, function(data) {
            $('#quyTrinhListContainer').html(data);
        });
    },

    // Xử lý form tạo mới
    handleCreateForm: function(form) {
        var chiPhiInput = form.find('.currency-input');
        var chiPhiValue = chiPhiInput.val().replace(/[^\d]/g, '');
        
        // Tạo object data từ form
        var formData = {};
        form.serializeArray().forEach(function(item) {
            if (item.name === 'ChiPhiNhanCong') {
                formData[item.name] = chiPhiValue;
            } else {
                formData[item.name] = item.value;
            }
        });

        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: formData,
            success: function(response) {
                if (response.success) {
                    toastr.success(response.message);
                    $('#quyTrinhModal').modal('hide');
                    // Reload the entire page to update cost analysis
                    setTimeout(function() {
                        location.reload();
                    }, 1000);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function(xhr, status, error) {
                console.error('Error:', xhr.responseText);
                toastr.error('Có lỗi xảy ra khi thêm quy trình!');
            }
        });
    },

    // Xử lý form chỉnh sửa
    handleEditForm: function(form) {
        var chiPhiInput = form.find('.currency-input');
        var chiPhiValue = chiPhiInput.val().replace(/[^\d]/g, '');
        
        // Tạo object data từ form
        var formData = {};
        form.serializeArray().forEach(function(item) {
            if (item.name === 'ChiPhiNhanCong') {
                formData[item.name] = chiPhiValue;
            } else {
                formData[item.name] = item.value;
            }
        });

        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: formData,
            success: function(response) {
                if (response.success) {
                    toastr.success(response.message);
                    $('#quyTrinhModal').modal('hide');
                    // Reload the entire page to update cost analysis
                    setTimeout(function() {
                        location.reload();
                    }, 1000);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function(xhr, status, error) {
                console.error('Error:', xhr.responseText);
                toastr.error('Có lỗi xảy ra khi cập nhật quy trình!');
            }
        });
    },

    // Xử lý form xóa
    handleDeleteForm: function(form) {
        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: form.serialize(),
            success: function(response) {
                if (response.success) {
                    toastr.success(response.message);
                    $('#quyTrinhModal').modal('hide');
                    // Reload the entire page to update cost analysis
                    setTimeout(function() {
                        location.reload();
                    }, 1000);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function(xhr, status, error) {
                console.error('Error:', xhr.responseText);
                toastr.error('Có lỗi xảy ra khi xóa quy trình!');
            }
        });
    }
};

// Global functions for backward compatibility
function openCreateQuyTrinhModal(sanPhamId) {
    QuyTrinhSanXuat.openCreateModal(sanPhamId);
}

function openEditQuyTrinhModal(id) {
    QuyTrinhSanXuat.openEditModal(id);
}

function deleteQuyTrinh(id, sanPhamId) {
    QuyTrinhSanXuat.deleteQuyTrinh(id, sanPhamId);
}

function loadQuyTrinhList(sanPhamId) {
    QuyTrinhSanXuat.loadQuyTrinhList(sanPhamId);
}

// Initialize when document is ready
$(document).ready(function() {
    QuyTrinhSanXuat.init();
}); 