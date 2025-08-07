// NhaCungCap.js - Quản lý nhà cung cấp

const NhaCungCap = {
    // Khởi tạo
    init: function() {
        this.initDataTable();
        this.bindEvents();
    },

    // Khởi tạo DataTable
    initDataTable: function() {
        Common.initDataTable('#nhaCungCapTable');
    },

    // Bind events
    bindEvents: function() {
        // Load Create Modal
        $('[data-target="#createModal"]').on('click', function () {
            NhaCungCap.showModal('/NhaCungCap/Create', 'createModal');
        });

        // Load Edit Modal
        $(document).on('click', '.edit-btn', function () {
            var id = $(this).data('id');
            NhaCungCap.showModal('/NhaCungCap/Edit/' + id, 'editModal');
        });

        // Load Delete Modal
        $(document).on('click', '.delete-btn', function () {
            var id = $(this).data('id');
            NhaCungCap.showModal('/NhaCungCap/Delete/' + id, 'deleteModal');
        });

        // Handle form submissions
        $(document).on('submit', '#createForm', function(e) {
            e.preventDefault();
            NhaCungCap.handleFormSubmission($(this), 'Thêm nhà cung cấp thành công!', 'Có lỗi xảy ra khi thêm nhà cung cấp!');
        });

        $(document).on('submit', '#editForm', function(e) {
            e.preventDefault();
            NhaCungCap.handleFormSubmission($(this), 'Cập nhật nhà cung cấp thành công!', 'Có lỗi xảy ra khi cập nhật nhà cung cấp!');
        });

        $(document).on('submit', '#deleteForm', function(e) {
            e.preventDefault();
            NhaCungCap.handleFormSubmission($(this), 'Xóa nhà cung cấp thành công!', 'Có lỗi xảy ra khi xóa nhà cung cấp!');
        });

        // Handle phone number formatting
        $(document).on('input', '#SoDienThoaiInput', function() {
            NhaCungCap.formatPhoneNumber($(this));
        });

        // Handle tax number formatting
        $(document).on('input', '#MaSoThueInput', function() {
            NhaCungCap.formatTaxNumber($(this));
        });

        // Handle email validation
        $(document).on('blur', '#EmailInput', function() {
            NhaCungCap.validateEmail($(this));
        });
    },

    // Show modal
    showModal: function(url, modalId) {
        $.get(url, function (data) {
            $('#modalContainer').html(data);
            $('#' + modalId).modal('show');
        });
    },

    // Handle form submission
    handleFormSubmission: function(form, successMessage, errorMessage) {
        // Convert formatted values back to numbers
        form.find('#SoDienThoaiInput').each(function() {
            var value = $(this).val().replace(/[^\d]/g, '');
            $(this).val(value);
        });

        form.find('#MaSoThueInput').each(function() {
            var value = $(this).val().replace(/[^\d]/g, '');
            $(this).val(value);
        });

        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: form.serialize(),
            success: function (result) {
                if (result.success) {
                    $('.modal').modal('hide');
                    toastr.success(successMessage);
                    setTimeout(function () {
                        location.reload();
                    }, 1000);
                } else {
                    $('#modalContainer').html(result);
                }
            },
            error: function () {
                toastr.error(errorMessage);
            }
        });
    },

    // Format phone number
    formatPhoneNumber: function(input) {
        var value = input.val().replace(/[^\d]/g, '');
        if (value.length > 0) {
            if (value.startsWith('0')) {
                value = '+84' + value.substring(1);
            } else if (!value.startsWith('+84')) {
                value = '+84' + value;
            }
            
            // Format: +84 xxx xxx xxxx
            if (value.length > 4) {
                value = value.substring(0, 4) + ' ' + value.substring(4);
            }
            if (value.length > 8) {
                value = value.substring(0, 8) + ' ' + value.substring(8);
            }
            if (value.length > 12) {
                value = value.substring(0, 12) + ' ' + value.substring(12);
            }
        }
        input.val(value);
    },

    // Format tax number
    formatTaxNumber: function(input) {
        var value = input.val().replace(/[^\d]/g, '');
        if (value.length > 0) {
            // Format: XXX XXX XXX
            if (value.length > 3) {
                value = value.substring(0, 3) + ' ' + value.substring(3);
            }
            if (value.length > 7) {
                value = value.substring(0, 7) + ' ' + value.substring(7);
            }
            if (value.length > 11) {
                value = value.substring(0, 11) + ' ' + value.substring(11);
            }
        }
        input.val(value);
    },

    // Validate email
    validateEmail: function(input) {
        var email = input.val();
        var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        
        if (email && !emailRegex.test(email)) {
            input.addClass('is-invalid');
            if (!input.next('.invalid-feedback').length) {
                input.after('<div class="invalid-feedback">Email không hợp lệ</div>');
            }
        } else {
            input.removeClass('is-invalid');
            input.next('.invalid-feedback').remove();
        }
    }
};

// Initialize when document is ready
$(document).ready(function() {
    NhaCungCap.init();
}); 