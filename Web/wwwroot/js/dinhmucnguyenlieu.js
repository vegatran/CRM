// DinhMucNguyenLieu.js - Quản lý định mức nguyên liệu

const DinhMucNguyenLieu = {
    // Khởi tạo
    init: function() {
        this.initDataTable();
        this.bindEvents();
    },

    // Khởi tạo DataTable
    initDataTable: function() {
        $('#dinhMucTable').DataTable({
            "language": {
                "url": "/js/dataTables.vietnamese.json"
            },
            "responsive": true,
            "autoWidth": false,
            "order": [[0, "asc"]],
            "columnDefs": [
                { "orderable": false, "targets": [9] } // Disable sorting for action column
            ]
        });
    },

    // Bind events
    bindEvents: function() {
        // Create button click
        $('[data-target="#createModal"]').click(function() {
            DinhMucNguyenLieu.loadCreateModal();
        });

        // Edit button click
        $(document).on('click', '.edit-btn', function() {
            var id = $(this).data('id');
            DinhMucNguyenLieu.loadEditModal(id);
        });

        // Delete button click
        $(document).on('click', '.delete-btn', function() {
            var id = $(this).data('id');
            DinhMucNguyenLieu.loadDeleteModal(id);
        });

        // Create form submit
        $(document).on('submit', '#createForm', function(e) {
            e.preventDefault();
            DinhMucNguyenLieu.submitCreateForm();
        });

        // Edit form submit
        $(document).on('submit', '#editForm', function(e) {
            e.preventDefault();
            DinhMucNguyenLieu.submitEditForm();
        });

        // Delete form submit
        $(document).on('submit', 'form[asp-action="Delete"]', function(e) {
            e.preventDefault();
            DinhMucNguyenLieu.submitDeleteForm($(this));
        });
    },

    // Load create modal
    loadCreateModal: function() {
        $.get('/DinhMucNguyenLieu/Create', function(data) {
            $('#modalContainer').html(data);
            $('#createModal').modal('show');
        });
    },

    // Load edit modal
    loadEditModal: function(id) {
        $.get('/DinhMucNguyenLieu/Edit/' + id, function(data) {
            $('#modalContainer').html(data);
            $('#editModal').modal('show');
        });
    },

    // Load delete modal
    loadDeleteModal: function(id) {
        $.get('/DinhMucNguyenLieu/Delete/' + id, function(data) {
            $('#modalContainer').html(data);
            $('#deleteModal').modal('show');
        });
    },

    // Submit create form
    submitCreateForm: function() {
        var form = $('#createForm');
        var formData = new FormData(form[0]);

        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function(response) {
                if (response.success) {
                    $('#createModal').modal('hide');
                    toastr.success(response.message);
                    setTimeout(function() {
                        location.reload();
                    }, 1000);
                } else {
                    toastr.error('Có lỗi xảy ra khi thêm định mức nguyên liệu!');
                }
            },
            error: function() {
                toastr.error('Có lỗi xảy ra khi thêm định mức nguyên liệu!');
            }
        });
    },

    // Submit edit form
    submitEditForm: function() {
        var form = $('#editForm');
        var formData = new FormData(form[0]);

        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function(response) {
                if (response.success) {
                    $('#editModal').modal('hide');
                    toastr.success(response.message);
                    setTimeout(function() {
                        location.reload();
                    }, 1000);
                } else {
                    toastr.error('Có lỗi xảy ra khi cập nhật định mức nguyên liệu!');
                }
            },
            error: function() {
                toastr.error('Có lỗi xảy ra khi cập nhật định mức nguyên liệu!');
            }
        });
    },

    // Submit delete form
    submitDeleteForm: function(form) {
        var formData = new FormData(form[0]);

        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function(response) {
                if (response.success) {
                    $('#deleteModal').modal('hide');
                    toastr.success(response.message);
                    setTimeout(function() {
                        location.reload();
                    }, 1000);
                } else {
                    toastr.error('Có lỗi xảy ra khi xóa định mức nguyên liệu!');
                }
            },
            error: function() {
                toastr.error('Có lỗi xảy ra khi xóa định mức nguyên liệu!');
            }
        });
    }
};

// Initialize when document is ready
$(document).ready(function() {
    DinhMucNguyenLieu.init();
}); 