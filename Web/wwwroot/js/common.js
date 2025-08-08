// Common.js - Các functions JavaScript chung

const Common = {
    // Khởi tạo
    init: function() {
        this.bindEvents();
        this.initDataTables();
        this.initCurrencyFormatting();
        this.initColorPickers();
        this.initImagePreviews();
        this.highlightActiveMenu();
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

        // Mobile sidebar overlay click to close
        $(document).on('click', '.sidebar-overlay', function() {
            $('body').removeClass('sidebar-open');
        });

        // Close sidebar when clicking menu items on mobile
        $(document).on('click', '.nav-sidebar .nav-link', function() {
            if ($(window).width() <= 768) {
                $('body').removeClass('sidebar-open');
            }
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

    // Highlight active menu based on current URL
    highlightActiveMenu: function() {
        var currentPath = window.location.pathname;
        var currentUrl = window.location.href;
        
        console.log('Current path:', currentPath); // Debug log
        
        // Remove all active classes first
        $('.nav-sidebar .nav-link').removeClass('active current').removeAttr('aria-current');
        
        // Find and highlight the active menu item
        $('.nav-sidebar .nav-link').each(function() {
            var $link = $(this);
            var href = $link.attr('href');
            var dataMenu = $link.attr('data-menu');
            var text = $link.text().trim();
            
            console.log('Checking link:', href, 'Data-menu:', dataMenu, 'Text:', text); // Debug log
            
            if (href) {
                // Handle relative and absolute URLs
                if (href.startsWith('/')) {
                    // Absolute path - exact match
                    if (currentPath === href) {
                        console.log('Exact match found:', href); // Debug log
                        $link.addClass('active current').attr('aria-current', 'page');
                        return false; // Break the loop
                    }
                    // Absolute path - starts with
                    if (currentPath.startsWith(href + '/')) {
                        console.log('Starts with match found:', href); // Debug log
                        $link.addClass('active current').attr('aria-current', 'page');
                        return false; // Break the loop
                    }
                } else if (href.startsWith('http')) {
                    // Full URL
                    if (currentUrl === href) {
                        console.log('Full URL match found:', href); // Debug log
                        $link.addClass('active current').attr('aria-current', 'page');
                        return false; // Break the loop
                    }
                } else {
                    // Relative path
                    if (currentPath.endsWith(href) || currentPath.includes(href)) {
                        console.log('Relative path match found:', href); // Debug log
                        $link.addClass('active current').attr('aria-current', 'page');
                        return false; // Break the loop
                    }
                }
            }
            
            // Check by data-menu attribute
            if (dataMenu && currentPath.toLowerCase().includes(dataMenu.toLowerCase())) {
                console.log('Data-menu match found:', dataMenu); // Debug log
                $link.addClass('active current').attr('aria-current', 'page');
                return false; // Break the loop
            }
        });
        
        // If no exact match found, try partial matching
        if (!$('.nav-sidebar .nav-link.active').length) {
            console.log('No exact match found, trying partial matching...'); // Debug log
            $('.nav-sidebar .nav-link').each(function() {
                var $link = $(this);
                var href = $link.attr('href');
                var text = $link.text().trim().toLowerCase();
                
                // Check if current path contains the href path
                if (href && href.startsWith('/')) {
                    var hrefPath = href.replace('/', '');
                    if (currentPath.includes(hrefPath)) {
                        console.log('Partial href match found:', href); // Debug log
                        $link.addClass('active current').attr('aria-current', 'page');
                        return false;
                    }
                }
                
                // Check if current path contains menu text (remove spaces and special chars)
                if (text) {
                    var cleanText = text.replace(/[^\w]/g, '').toLowerCase();
                    var cleanPath = currentPath.replace(/[^\w]/g, '').toLowerCase();
                    if (cleanPath.includes(cleanText)) {
                        console.log('Text match found:', text); // Debug log
                        $link.addClass('active current').attr('aria-current', 'page');
                        return false;
                    }
                }
            });
        }
        
        // Add click event to menu items
        $('.nav-sidebar .nav-link').off('click.menuHighlight').on('click.menuHighlight', function() {
            $('.nav-sidebar .nav-link').removeClass('active current').removeAttr('aria-current');
            $(this).addClass('active current').attr('aria-current', 'page');
        });
        
        // Force active state periodically to prevent AdminLTE override
        setInterval(function() {
            var currentPath = window.location.pathname;
            $('.nav-sidebar .nav-link').each(function() {
                var $link = $(this);
                var href = $link.attr('href');
                var dataMenu = $link.attr('data-menu');
                
                // Check by href
                if (href === currentPath) {
                    $link.addClass('active current').attr('aria-current', 'page');
                    return;
                }
                
                // Check by data-menu attribute
                if (dataMenu && currentPath.toLowerCase().includes(dataMenu)) {
                    $link.addClass('active current').attr('aria-current', 'page');
                    return;
                }
            });
        }, 1000);
        
        // Log final result
        var activeLink = $('.nav-sidebar .nav-link.active');
        if (activeLink.length) {
            console.log('Active menu found:', activeLink.attr('href'), activeLink.text().trim());
            console.log('Active menu classes:', activeLink.attr('class'));
        } else {
            console.log('No active menu found');
        }
        
        // Force refresh active state for debugging
        setTimeout(function() {
            var currentPath = window.location.pathname;
            console.log('Final check - Current path:', currentPath);
            $('.nav-sidebar .nav-link').each(function() {
                var $link = $(this);
                var href = $link.attr('href');
                if (href === currentPath) {
                    console.log('Found matching link:', href);
                    $link.addClass('active current').attr('aria-current', 'page');
                }
            });
        }, 100);
        
        // Log final result
        var activeLink = $('.nav-sidebar .nav-link.active');
        if (activeLink.length) {
            console.log('Active menu found:', activeLink.attr('href'), activeLink.text().trim());
            console.log('Active menu classes:', activeLink.attr('class'));
        } else {
            console.log('No active menu found');
        }
    },

    // Cấu hình ngôn ngữ DataTable (có thể thay đổi ở đây để áp dụng cho toàn bộ hệ thống)
    getDataTableLanguage: function() {
        return {
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
        };
    },

    // Hàm khởi tạo DataTable dùng chung
    // @param {string} selector - CSS selector của table (ví dụ: '#myTable', '.data-table')
    // @param {object} options - Tùy chọn bổ sung cho DataTable (tùy chọn)
    // 
    // Ví dụ sử dụng:
    // - Cơ bản: Common.initDataTable('#myTable');
    // - Với tùy chọn: Common.initDataTable('#myTable', { "order": [[0, "asc"]], "pageLength": 10 });
    // - Với columnDefs: Common.initDataTable('#myTable', { "columnDefs": [{ "orderable": false, "targets": [5] }] });
    initDataTable: function(selector, options) {
        var defaultOptions = {
            "language": this.getDataTableLanguage(),
            "responsive": true,
            "autoWidth": false,
            "pageLength": 25,
            "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Tất cả"]]
        };

        // Merge options
        var finalOptions = $.extend(true, {}, defaultOptions, options);
        
        return $(selector).DataTable(finalOptions);
    },

    // Khởi tạo DataTables
    initDataTables: function() {
        // Khởi tạo cho các bảng có class data-table (backward compatibility)
        $('.data-table').each(function() {
            var $table = $(this);
            var $tbody = $table.find('tbody');
            var $rows = $tbody.find('tr');
            
            // Kiểm tra xem bảng có dữ liệu thực tế không
            var hasRealData = false;
            $rows.each(function() {
                var $row = $(this);
                // Bỏ qua các dòng có colspan (thường là "Chưa có dữ liệu")
                if ($row.find('td[colspan]').length === 0) {
                    hasRealData = true;
                    return false; // break the loop
                }
            });
            
            // Chỉ khởi tạo DataTables nếu có dữ liệu thực tế
            if (hasRealData) {
                Common.initDataTable($table);
            }
        });
        
        // Không khởi tạo DataTables cho các bảng có ID cụ thể ở đây
        // Các bảng có file JS riêng sẽ được khởi tạo trong file JS đó
        // Các bảng không có file JS riêng sẽ được khởi tạo trong file JS của trang tương ứng
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