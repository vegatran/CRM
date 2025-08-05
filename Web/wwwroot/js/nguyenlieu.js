// NguyenLieu.js - Quản lý nguyên liệu

const NguyenLieu = {
    // Khởi tạo
    init: function() {
        this.initDataTable();
        this.bindEvents();
    },

    // Khởi tạo DataTable
    initDataTable: function() {
        $('#nguyenLieuTable').DataTable({
            "language": {
                "url": "/js/dataTables.vietnamese.json"
            },
            "responsive": true,
            "autoWidth": false,
            "pageLength": 25,
            "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Tất cả"]]
        });
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
        // Convert formatted currency values back to numbers
        form.find('#GiaNhapInput').each(function() {
            var value = $(this).val().replace(/[^\d]/g, '');
            $(this).val(value);
        });

        // Check if form has file input
        var hasFileInput = form.find('input[type="file"]').length > 0;
        
        if (hasFileInput) {
            // Use FormData for file upload
            var formData = new FormData(form[0]);
            
            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
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
        } else {
            // Use regular form submission for non-file forms
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
        var previewImg = input.closest('.modal').find('#previewImg');
        
        if (file) {
            // Update file label
            input.next('.custom-file-label').html(file.name);
            
            // Show preview
            var reader = new FileReader();
            reader.onload = function(e) {
                previewImg.attr('src', e.target.result);
                previewImg.show();
            };
            reader.readAsDataURL(file);
        } else {
            // Hide preview if no file selected
            previewImg.hide();
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
            colorPicker.on('change', function() {
                mauSacInput.val($(this).val());
            });
            
            // Handle input change (manual entry)
            mauSacInput.on('input', function() {
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
        }
    }
};

// Initialize when document is ready
$(document).ready(function() {
    NguyenLieu.init();
}); 