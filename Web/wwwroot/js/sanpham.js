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
        $('#sanPhamTable').DataTable({
            "language": { "url": "/js/dataTables.vietnamese.json" },
            "responsive": true,
            "autoWidth": false,
            "pageLength": 25,
            "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Tất cả"]]
        });
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

        // Color picker
        $(document).on('change', '#ColorPicker', function() {
            $('#MauSacInput').val($(this).val());
        });

        // Color pattern dropdown
        $(document).on('click', '.color-pattern', function(e) {
            e.preventDefault();
            var color = $(this).data('color');
            var pattern = $(this).data('pattern');
            $('#ColorPicker').val(color);
            $('#MauSacInput').val(pattern);
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
            giaNhapInput.prop('required', false);
            chiPhiNhanCongInput.prop('required', true);
            description.html('<span><strong>Tự sản xuất:</strong> Sản phẩm do công ty tự sản xuất, giá vốn = chi phí sản xuất</span>');
        } else { // Mua ngoài
            giaNhapRow.show();
            giaNhapInput.prop('required', true);
            chiPhiNhanCongInput.prop('required', false);
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
            var chiPhiNhanCong = parseFloat($('#ChiPhiNhanCongInput').val().replace(/[^\d]/g, '') || 0);
            // Giá vốn = chi phí nhân công (tạm thời, sẽ cập nhật khi có định mức nguyên liệu)
            giaVon = chiPhiNhanCong;
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

        // Initialize color picker
        var colorInput = modal.find('#MauSacInput');
        var colorPicker = modal.find('#ColorPicker');
        if (colorInput.length && colorPicker.length) {
            colorPicker.val('#000000');
        }

        // Initialize loại sản phẩm handler
        var loaiSanPhamSelect = modal.find('#LoaiSanPhamSelect');
        if (loaiSanPhamSelect.length) {
            SanPham.handleLoaiSanPhamChange(loaiSanPhamSelect);
        }
    }
};

// Initialize when document is ready
$(document).ready(function() {
    SanPham.init();
}); 