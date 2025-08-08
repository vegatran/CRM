// SanPham.js - Quản lý sản phẩm

const SanPham = {
    // Khởi tạo
    init: function() {
        this.initDataTable();
        this.bindEvents();
        this.initLoaiSanPhamHandler();
    },

    // Khởi tạo DataTable
    initDataTable: function() {
        var $table = $('#sanPhamTable');
        if ($table.length) {
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
        }
    },

    // Bind events
    bindEvents: function() {
        // Create modal
        $(document).on('click', '[data-target="#createModal"]', function() {
            SanPham.loadCreateModal();
        });

        // Edit modal
        $(document).on('click', '.edit-btn', function() {
            var id = $(this).data('id');
            SanPham.loadEditModal(id);
        });

        // Delete modal
        $(document).on('click', '.delete-btn', function() {
            var id = $(this).data('id');
            SanPham.loadDeleteModal(id);
        });

        // Form submissions
        $(document).on('submit', '#createForm', function(e) {
            e.preventDefault();
            SanPham.handleFormSubmission($(this), 'Thêm sản phẩm thành công!', 'Lỗi khi thêm sản phẩm!');
        });

        $(document).on('submit', '#editForm', function(e) {
            e.preventDefault();
            SanPham.handleFormSubmission($(this), 'Cập nhật sản phẩm thành công!', 'Lỗi khi cập nhật sản phẩm!');
        });

        $(document).on('submit', '#deleteForm', function(e) {
            e.preventDefault();
            SanPham.handleFormSubmission($(this), 'Xóa sản phẩm thành công!', 'Lỗi khi xóa sản phẩm!');
        });

        // Modal events
        $(document).on('shown.bs.modal', '.modal', function() {
            SanPham.initializeModal($(this));
        });

        // Close dropdowns when modal is hidden
        $(document).on('hidden.bs.modal', '.modal', function() {
            $(this).find('.dropdown-menu').removeClass('show');
        });

        // Color picker
        $(document).on('change', '#ColorPicker', function() {
            var modal = $(this).closest('.modal');
            var colorInput = modal.find('#MauSacInput');
            if (colorInput.length) {
                colorInput.val($(this).val());
            }
        });

        // Currency formatting
        $(document).on('input', '.currency-input', function() {
            SanPham.formatCurrency($(this));
        });

        $(document).on('blur', '.currency-input', function() {
            SanPham.parseCurrency($(this));
        });

        // Tính toán giá bán đề xuất khi thay đổi giá trị
        $(document).on('input', '#GiaNhapInput, #ChiPhiNhanCongInput', function() {
            SanPham.calculateGiaBanDeXuat();
        });
    },

    // Xử lý thay đổi loại sản phẩm
    initLoaiSanPhamHandler: function() {
        $(document).on('change', '#LoaiSanPhamSelect', function() {
            SanPham.handleLoaiSanPhamChange($(this));
        });
    },

    // Xử lý khi thay đổi loại sản phẩm
    handleLoaiSanPhamChange: function(select) {
        var loaiSanPham = parseInt(select.val());
        var giaNhapRow = $('#giaNhapRow');
        var giaNhapInput = $('#GiaNhapInput');
        var chiPhiNhanCongInput = $('#ChiPhiNhanCongInput');
        var description = $('#loaiSanPhamDescription');

        if (loaiSanPham === 1) { // Tự sản xuất
            giaNhapRow.hide();
            giaNhapInput.prop('required', false).val('0');
            chiPhiNhanCongInput.prop('required', false).val('0');
            description.html('<span><strong>Tự sản xuất:</strong> Sản phẩm do công ty tự sản xuất, giá vốn được tính tự động từ định mức nguyên liệu và quy trình sản xuất</span>');
        } else { // Mua ngoài
            giaNhapRow.show();
            giaNhapInput.prop('required', true);
            chiPhiNhanCongInput.prop('required', false).val('0');
            description.html('<span><strong>Mua ngoài:</strong> Sản phẩm mua từ nhà cung cấp, giá vốn = giá nhập</span>');
        }
        
        // Tính toán lại giá bán đề xuất
        this.calculateGiaBanDeXuat();
    },

    // Tính toán giá bán đề xuất
    calculateGiaBanDeXuat: function() {
        var loaiSanPham = parseInt($('#LoaiSanPhamSelect').val());
        var giaVon = 0;
        const heSoLoiNhuan = 1.3; // 30% lợi nhuận

        if (loaiSanPham === 1) { // Tự sản xuất
            // Đối với sản phẩm tự sản xuất, giá vốn sẽ được tính tự động từ định mức nguyên liệu và quy trình sản xuất
            // Tạm thời sử dụng giá mặc định hoặc giá đã có
            var giaBanHienTai = parseFloat($('#GiaBanInput').val().replace(/[^\d]/g, '') || 0);
            if (giaBanHienTai > 0) {
                giaVon = giaBanHienTai / heSoLoiNhuan; // Ước tính ngược từ giá bán
            } else {
                giaVon = 0; // Sẽ được tính khi có đủ thông tin
            }
        } else { // Mua ngoài
            var giaNhap = parseFloat($('#GiaNhapInput').val().replace(/[^\d]/g, '') || 0);
            giaVon = giaNhap;
        }

        var giaBanDeXuat = giaVon * heSoLoiNhuan;
        $('#giaBanDeXuat').text(giaBanDeXuat.toLocaleString('vi-VN'));
        
        // Tự động điền giá bán đề xuất nếu chưa có giá
        var giaBanHienTai = $('#GiaBanInput').val().replace(/[^\d]/g, '');
        if (!giaBanHienTai || giaBanHienTai === '0') {
            $('#GiaBanInput').val(giaBanDeXuat.toLocaleString('vi-VN'));
        }
    },

    // Load create modal
    loadCreateModal: function() {
        $.get('/SanPham/Create', function(data) {
            $('#modalContainer').html(data);
            $('#createModal').modal('show');
        });
    },

    // Load edit modal
    loadEditModal: function(id) {
        $.get('/SanPham/Edit/' + id, function(data) {
            $('#modalContainer').html(data);
            $('#editModal').modal('show');
        });
    },

    // Load delete modal
    loadDeleteModal: function(id) {
        $.get('/SanPham/Delete/' + id, function(data) {
            $('#modalContainer').html(data);
            $('#deleteModal').modal('show');
        });
    },

    // Handle form submission
    handleFormSubmission: function(form, successMessage, errorMessage) {
        var formData = new FormData(form[0]);
        
        // Xử lý hình ảnh cho form edit
        if (form.attr('id') === 'editForm') {
            var hinhAnhInput = form.find('#HinhAnhInput');
            var currentHinhAnh = form.find('#currentHinhAnh');
            
            // Nếu không có file được chọn và có hình ảnh hiện tại, thêm hình ảnh hiện tại vào formData
            if (hinhAnhInput[0].files.length === 0 && currentHinhAnh.val()) {
                // Tạo một blob từ URL hình ảnh hiện tại
                var currentImageUrl = currentHinhAnh.val();
                if (currentImageUrl) {
                    // Thêm thông tin hình ảnh hiện tại vào formData
                    formData.append('CurrentHinhAnh', currentImageUrl);
                }
            }
        }
        
        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function(response) {
                if (response.success) {
                    toastr.success(successMessage);
                    $('.modal').modal('hide');
                    setTimeout(function() {
                        location.reload();
                    }, 1000);
                } else {
                    toastr.error(response.message || errorMessage);
                }
            },
            error: function() {
                toastr.error(errorMessage);
            }
        });
    },

    // Format currency input
    formatCurrency: function(input) {
        var value = input.val().replace(/[^\d]/g, '');
        if (value) {
            value = parseInt(value).toLocaleString('vi-VN');
            input.val(value);
        }
    },

    // Parse currency input
    parseCurrency: function(input) {
        var value = input.val().replace(/[^\d]/g, '');
        if (value) {
            input.val(value);
        }
    },

    // Initialize modal
    initializeModal: function(modal) {
        // Initialize currency inputs
        modal.find('.currency-input').each(function() {
            SanPham.formatCurrency($(this));
        });

        // Initialize color picker and pattern dropdown
        var colorInput = modal.find('#MauSacInput');
        var colorPicker = modal.find('#ColorPicker');
        
        if (colorInput.length && colorPicker.length) {
            // Set default color if no value
            if (!colorInput.val()) {
                colorPicker.val('#000000');
            } else {
                // If color input has a value, try to set it to color picker
                var colorValue = colorInput.val();
                if (colorValue && colorValue.match(/^#[0-9A-F]{6}$/i)) {
                    colorPicker.val(colorValue);
                } else {
                    colorPicker.val('#000000');
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
            if (pattern && colorInput.length) {
                colorInput.val(pattern);
            }

            // Close dropdown
            modal.find('.dropdown-menu').removeClass('show');
        });

        // Initialize image preview
        var hinhAnhInput = modal.find('#HinhAnhInput');
        var previewImg = modal.find('#previewImg');
        var currentHinhAnh = modal.find('#currentHinhAnh');
        
        if (hinhAnhInput.length) {
            // Hiển thị hình ảnh hiện tại nếu có (cho form edit)
            if (currentHinhAnh.length && currentHinhAnh.val()) {
                previewImg.attr('src', currentHinhAnh.val());
                previewImg.show();
                
                // Hiển thị tên file hiện tại trong file label
                var currentImagePath = currentHinhAnh.val();
                if (currentImagePath) {
                    var fileName = currentImagePath.split('/').pop(); // Lấy tên file từ đường dẫn
                    hinhAnhInput.next('.custom-file-label').html(fileName || 'Hình ảnh hiện tại');
                }
            }
            
            hinhAnhInput.off('change').on('change', function() {
                var file = this.files[0];
                if (file) {
                    // Update file label
                    $(this).next('.custom-file-label').html(file.name);
                    
                    // Show preview
                    var reader = new FileReader();
                    reader.onload = function(e) {
                        previewImg.attr('src', e.target.result);
                        previewImg.show();
                    };
                    reader.readAsDataURL(file);
                } else {
                    // Nếu không có file được chọn, hiển thị lại hình ảnh hiện tại (nếu có)
                    if (currentHinhAnh.length && currentHinhAnh.val()) {
                        previewImg.attr('src', currentHinhAnh.val());
                        previewImg.show();
                        
                        // Hiển thị lại tên file hiện tại
                        var currentImagePath = currentHinhAnh.val();
                        if (currentImagePath) {
                            var fileName = currentImagePath.split('/').pop();
                            hinhAnhInput.next('.custom-file-label').html(fileName || 'Hình ảnh hiện tại');
                        }
                    } else {
                        previewImg.hide();
                        hinhAnhInput.next('.custom-file-label').html('Chọn file...');
                    }
                }
            });
        }

        // Initialize loại sản phẩm handler
        var loaiSanPhamSelect = modal.find('#LoaiSanPhamSelect');
        if (loaiSanPhamSelect.length) {
            // Xử lý hiển thị/ẩn trường dựa trên loại sản phẩm hiện tại
            SanPham.handleLoaiSanPhamChange(loaiSanPhamSelect);
        }
    }
};

// Initialize when document is ready
$(document).ready(function() {
    SanPham.init();
}); 