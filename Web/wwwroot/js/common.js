// Common.js - Các functions JavaScript chung

const Common = {
    // Khởi tạo
    init: function() {
        this.bindEvents();
        this.initDataTables();
        this.initCurrencyFormatting();
        this.initColorPickers();
        this.initImagePreviews();
    },

    // Bind events
    bindEvents: function() {
        // Modal close buttons
        $(document).on('click', '[data-dismiss="modal"], .close, .modal-close-btn', function () {
            var modal = $(this).closest('.modal');
            modal.modal('hide');
        });
        
        // Clear modal content when hidden
        $('.modal').on('hidden.bs.modal', function () {
            $('#modalContainer').empty();
        });

        // Toastr configuration
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
    },

    // Khởi tạo DataTables
    initDataTables: function() {
        $('.data-table').DataTable({
            "language": {
                "url": "/js/dataTables.vietnamese.json"
            },
            "responsive": true,
            "autoWidth": false,
            "pageLength": 25,
            "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Tất cả"]]
        });
    },

    // Khởi tạo currency formatting
    initCurrencyFormatting: function() {
        $(document).on('input', '.currency-input', function() {
            var value = $(this).val().replace(/[^\d]/g, '');
            if (value) {
                $(this).val(parseInt(value).toLocaleString('vi-VN'));
            }
        });
    },

    // Khởi tạo color pickers
    initColorPickers: function() {
        $(document).on('change', '.color-picker', function() {
            var color = $(this).val();
            var preview = $(this).siblings('.color-preview');
            if (preview.length) {
                preview.css('background-color', color);
            }
        });
    },

    // Khởi tạo image previews
    initImagePreviews: function() {
        $(document).on('change', '.image-input', function() {
            var file = this.files[0];
            var preview = $(this).siblings('.image-preview');
            
            if (file && preview.length) {
                var reader = new FileReader();
                reader.onload = function(e) {
                    preview.attr('src', e.target.result);
                    preview.show();
                };
                reader.readAsDataURL(file);
            }
        });
    },

    // Format currency
    formatCurrency: function(amount) {
        return parseInt(amount).toLocaleString('vi-VN');
    },

    // Parse currency
    parseCurrency: function(value) {
        return value.replace(/[^\d]/g, '');
    },

    // Show loading
    showLoading: function() {
        $('#loadingOverlay').show();
    },

    // Hide loading
    hideLoading: function() {
        $('#loadingOverlay').hide();
    },

    // Show success message
    showSuccess: function(message) {
        toastr.success(message);
    },

    // Show error message
    showError: function(message) {
        toastr.error(message);
    },

    // Show warning message
    showWarning: function(message) {
        toastr.warning(message);
    },

    // Show info message
    showInfo: function(message) {
        toastr.info(message);
    },

    // Confirm dialog
    confirm: function(message, callback) {
        if (confirm(message)) {
            if (typeof callback === 'function') {
                callback();
            }
        }
    },

    // AJAX helper
    ajax: function(options) {
        var defaults = {
            type: 'GET',
            dataType: 'json',
            error: function(xhr, status, error) {
                console.error('AJAX Error:', xhr.responseText);
                Common.showError('Có lỗi xảy ra!');
            }
        };

        return $.ajax($.extend(defaults, options));
    },

    // Form helper
    serializeForm: function(form) {
        var formData = form.serialize();
        
        // Handle currency inputs
        form.find('.currency-input').each(function() {
            var name = $(this).attr('name');
            var value = Common.parseCurrency($(this).val());
            formData = formData.replace(new RegExp(name + '=\\d*'), name + '=' + value);
        });

        return formData;
    }
};

// Global functions for backward compatibility
function formatCurrency(amount) {
    return Common.formatCurrency(amount);
}

function parseCurrency(value) {
    return Common.parseCurrency(value);
}

function showSuccess(message) {
    Common.showSuccess(message);
}

function showError(message) {
    Common.showError(message);
}

function showWarning(message) {
    Common.showWarning(message);
}

function showInfo(message) {
    Common.showInfo(message);
}

// Initialize when document is ready
$(document).ready(function() {
    Common.init();
}); 