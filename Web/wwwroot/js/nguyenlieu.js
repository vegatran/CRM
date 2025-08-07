// NguyenLieu.js - Quản lý nguyên liệu

const NguyenLieu = {
    // Khởi tạo
    init: function() {
        this.initDataTable();
        this.bindEvents();
    },

    // Khởi tạo DataTable
    initDataTable: function() {
        Common.initDataTable('#nguyenLieuTable');
    },

    // Bind events
    bindEvents: function() {
        // Load Create Modal
        $('[data-target="#createModal"]').on('click', function () {
            NguyenLieu.showModal('/NguyenLieu/Create', 'createModal');
        });

        // Load Edit Modal
        $(document).on('click', '.edit-btn', function () {
            var id = $(this).data('id');
            NguyenLieu.showModal('/NguyenLieu/Edit/' + id, 'editModal');
        });

        // Load Delete Modal
        $(document).on('click', '.delete-btn', function () {
            var id = $(this).data('id');
            NguyenLieu.showModal('/NguyenLieu/Delete/' + id, 'deleteModal');
        });

        // Handle form submissions
        $(document).on('submit', '#createForm', function(e) {
            e.preventDefault();
            NguyenLieu.handleFormSubmission($(this), 'Thêm nguyên liệu thành công!', 'Có lỗi xảy ra khi thêm nguyên liệu!');
        });

        $(document).on('submit', '#editForm', function(e) {
            e.preventDefault();
            console.log('Edit form submitted');
            NguyenLieu.handleFormSubmission($(this), 'Cập nhật nguyên liệu thành công!', 'Có lỗi xảy ra khi cập nhật nguyên liệu!');
        });

        $(document).on('submit', '#deleteForm', function(e) {
            e.preventDefault();
            NguyenLieu.handleFormSubmission($(this), 'Xóa nguyên liệu thành công!', 'Có lỗi xảy ra khi xóa nguyên liệu!');
        });

        // Handle currency input formatting
        $(document).on('input', '#GiaNhapInput', function() {
            var value = $(this).val().replace(/[^\d]/g, '');
            if (value) {
                $(this).val(parseInt(value).toLocaleString('vi-VN'));
            }
        });

        // Handle auto code generation
        $(document).on('input', '#TenNguyenLieuInput', function() {
            NguyenLieu.generateCode($(this));
        });

        // Handle material type change
        $(document).on('change', '#LoaiNguyenLieuIdInput', function() {
            NguyenLieu.handleMaterialTypeChange($(this));
        });

        // Handle color picker
        $(document).on('change', '#ColorPicker', function() {
            var color = $(this).val();
            var mauSacInput = $(this).siblings('#MauSacInput');
            if (mauSacInput.length) {
                mauSacInput.val(color);
            }
        });

        // Handle image preview
        $(document).on('change', '#HinhAnhInput', function() {
            NguyenLieu.handleImagePreview($(this));
        });

        // Format existing values when modal loads
        $(document).on('shown.bs.modal', '.modal', function() {
            NguyenLieu.initializeModal($(this));
        });
        
        // Handle modal hidden event
        $(document).on('hidden.bs.modal', '.modal', function() {
            $('#modalContainer').empty();
            // Close dropdowns when modal is hidden
            $(this).find('.dropdown-menu').removeClass('show');
        });
    },

    // Show modal
    showModal: function(url, modalId) {
        console.log('Loading modal from:', url);
        $.get(url, function (data) {
            $('#modalContainer').html(data);
            $('#' + modalId).modal('show');
        }).fail(function(xhr, status, error) {
            console.error('Error loading modal:', error);
            toastr.error('Có lỗi xảy ra khi tải form!');
        });
    },

    // Handle form submission
    handleFormSubmission: function(form, successMessage, errorMessage) {
        var hasFileInput = form.find('input[type="file"]').length > 0;
        var hasFileSelected = false;
        
        if (hasFileInput) {
            var fileInput = form.find('input[type="file"]')[0];
            hasFileSelected = fileInput.files.length > 0;
        }
        
        if (hasFileInput && hasFileSelected) {
            // Use FormData for file uploads
            var formData = new FormData(form[0]);
            
            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (result) {
                    console.log('Success result:', result);
                    if (result.success) {
                        $('.modal').modal('hide');
                        toastr.success(result.message || successMessage);
                        setTimeout(function () {
                            location.reload();
                        }, 1000);
                    } else {
                        // Hiển thị lỗi từ JSON response
                        toastr.error(result.message || 'Có lỗi xảy ra!');
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error details:', {
                        status: xhr.status,
                        statusText: xhr.statusText,
                        responseText: xhr.responseText,
                        error: error
                    });
                    
                    // Thử parse JSON response nếu có
                    try {
                        var response = JSON.parse(xhr.responseText);
                        if (response.message) {
                            toastr.error(response.message);
                        } else {
                            toastr.error(errorMessage + ' (Status: ' + xhr.status + ')');
                        }
                    } catch (e) {
                        toastr.error(errorMessage + ' (Status: ' + xhr.status + ')');
                    }
                }
            });
        } else {
            // Use regular form submission for non-file forms
            var formData = form.serialize();
            
            // Xử lý hình ảnh cho form edit
            if (form.attr('id') === 'editForm') {
                var hinhAnhInput = form.find('#HinhAnhInput');
                var currentHinhAnh = form.find('#currentHinhAnh');
                
                // Nếu không có file được chọn và có hình ảnh hiện tại, thêm hình ảnh hiện tại vào formData
                if (hinhAnhInput.length > 0 && hinhAnhInput[0].files.length === 0 && currentHinhAnh.val()) {
                    // Tạo một blob từ URL hình ảnh hiện tại
                    var currentImageUrl = currentHinhAnh.val();
                    if (currentImageUrl) {
                        // Thêm thông tin hình ảnh hiện tại vào formData
                        formData += '&CurrentHinhAnh=' + encodeURIComponent(currentImageUrl);
                    }
                }
            }
            
            console.log('Form data:', formData);
            
            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: formData,
                success: function (result) {
                    console.log('Success result:', result);
                    if (result.success) {
                        $('.modal').modal('hide');
                        toastr.success(result.message || successMessage);
                        setTimeout(function () {
                            location.reload();
                        }, 1000);
                    } else {
                        // Hiển thị lỗi từ JSON response
                        toastr.error(result.message || 'Có lỗi xảy ra!');
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error details:', {
                        status: xhr.status,
                        statusText: xhr.statusText,
                        responseText: xhr.responseText,
                        error: error
                    });
                    
                    // Thử parse JSON response nếu có
                    try {
                        var response = JSON.parse(xhr.responseText);
                        if (response.message) {
                            toastr.error(response.message);
                        } else {
                            toastr.error(errorMessage + ' (Status: ' + xhr.status + ')');
                        }
                    } catch (e) {
                        toastr.error(errorMessage + ' (Status: ' + xhr.status + ')');
                    }
                }
            });
        }
    },

    // Generate code from name
    generateCode: function(input) {
        var name = input.val();
        var codeInput = input.closest('.modal').find('#MaNguyenLieuInput');
        
        if (name && codeInput.length) {
            // Generate code: VL + 3 digits
            var code = 'VL' + Math.floor(Math.random() * 900 + 100);
            codeInput.val(code);
        }
    },

    // Handle material type change
    handleMaterialTypeChange: function(select) {
        var modal = select.closest('.modal');
        var donViInput = modal.find('#DonViInput');
        var ghiChuInput = modal.find('#GhiChuInput');
        
        // Set default unit based on material type
        var selectedText = select.find('option:selected').text().toLowerCase();
        if (selectedText.includes('vải')) {
            donViInput.val('mét');
            ghiChuInput.val('Vải cotton cao cấp');
        } else if (selectedText.includes('chỉ')) {
            donViInput.val('cuộn');
            ghiChuInput.val('Chỉ may chất lượng cao');
        } else if (selectedText.includes('cúc')) {
            donViInput.val('chiếc');
            ghiChuInput.val('Cúc áo bền đẹp');
        } else {
            donViInput.val('');
            ghiChuInput.val('');
        }
    },

    // Handle image preview
    handleImagePreview: function(input) {
        var file = input[0].files[0];
        var modal = input.closest('.modal');
        var previewImg = modal.find('#previewImg');
        var imagePreview = modal.find('#imagePreview');
        
        if (file) {
            // Update file label
            input.next('.custom-file-label').html(file.name);
            
            // Show preview
            var reader = new FileReader();
            reader.onload = function(e) {
                previewImg.attr('src', e.target.result);
                imagePreview.show();
            };
            reader.readAsDataURL(file);
        } else {
            // Hide preview if no file selected
            imagePreview.hide();
        }
    },

    // Initialize modal
    initializeModal: function(modal) {
        var giaNhapInput = modal.find('#GiaNhapInput');
        var mauSacInput = modal.find('#MauSacInput');
        var colorPicker = modal.find('#ColorPicker');
        var hinhAnhInput = modal.find('#HinhAnhInput');
        
        // Only format if the value is not already formatted (doesn't contain comma)
        if (giaNhapInput.length && giaNhapInput.val() && !giaNhapInput.val().includes(',')) {
            var value = giaNhapInput.val().replace(/[^\d]/g, '');
            if (value) {
                giaNhapInput.val(parseInt(value).toLocaleString('vi-VN'));
            }
        }
        
        // Initialize color picker
        if (mauSacInput.length && colorPicker.length) {
            // Set color picker value from input
            if (mauSacInput.val()) {
                colorPicker.val(mauSacInput.val());
            }
            
            // Handle color picker change
            colorPicker.off('change').on('change', function() {
                mauSacInput.val($(this).val());
            });
            
            // Handle input change (manual entry)
            mauSacInput.off('input').on('input', function() {
                var value = $(this).val();
                if (value && value.match(/^#[0-9A-F]{6}$/i)) {
                    colorPicker.val(value);
                }
            });
        }

        // Initialize image input label
        if (hinhAnhInput.length) {
            var fileName = hinhAnhInput.val().split('\\').pop();
            if (fileName) {
                hinhAnhInput.next('.custom-file-label').html(fileName);
            }
            
            // Hiển thị hình ảnh hiện tại nếu có (cho form edit)
            var currentHinhAnh = modal.find('#currentHinhAnh');
            var previewImg = modal.find('#previewImg');
            var imagePreview = modal.find('#imagePreview');
            
            if (currentHinhAnh.length && currentHinhAnh.val()) {
                previewImg.attr('src', currentHinhAnh.val());
                imagePreview.show();
                
                // Hiển thị tên file hiện tại trong file label
                var currentImagePath = currentHinhAnh.val();
                if (currentImagePath) {
                    var fileName = currentImagePath.split('/').pop(); // Lấy tên file từ đường dẫn
                    hinhAnhInput.next('.custom-file-label').html(fileName || 'Hình ảnh hiện tại');
                }
            }
        }
        
        // Initialize Bootstrap dropdown for color patterns
        modal.find('.dropdown-toggle').off('click.colorPattern').on('click.colorPattern', function(e) {
            e.preventDefault();
            e.stopPropagation();
            var dropdown = $(this).siblings('.dropdown-menu');
            var isOpen = dropdown.hasClass('show');
            
            // Close all other dropdowns first
            modal.find('.dropdown-menu').removeClass('show');
            
            // Toggle current dropdown
            if (!isOpen) {
                dropdown.addClass('show');
            }
        });

        // Close dropdown when clicking outside
        $(document).off('click.colorPatternDropdown').on('click.colorPatternDropdown', function(e) {
            if (!$(e.target).closest('.dropdown').length) {
                modal.find('.dropdown-menu').removeClass('show');
            }
        });
        
        // Initialize color pattern dropdown items
        modal.find('.color-pattern').off('click.colorPattern').on('click.colorPattern', function(e) {
            e.preventDefault();
            e.stopPropagation();
            var color = $(this).data('color');
            var pattern = $(this).data('pattern');
            
            // Update color picker if color is provided
            if (color && colorPicker.length) {
                colorPicker.val(color);
            }
            
            // Update color input with pattern
            if (pattern && mauSacInput.length) {
                mauSacInput.val(pattern);
            }

            // Close dropdown
            modal.find('.dropdown-menu').removeClass('show');
        });
    }
};

// Initialize when document is ready
$(document).ready(function() {
    NguyenLieu.init();
}); 