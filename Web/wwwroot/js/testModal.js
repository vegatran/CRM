// TestModal.js - Quản lý test modal

const TestModal = {
    // Khởi tạo
    init: function() {
        this.bindEvents();
    },

    // Bind events
    bindEvents: function() {
        // Handle modal close buttons
        $(document).on('click', '[data-dismiss="modal"], .close, .modal-close-btn', function () {
            var modal = $(this).closest('.modal');
            modal.modal('hide');
        });
        
        // Clear modal content when hidden
        $('.modal').on('hidden.bs.modal', function () {
            $('#modalContainer').empty();
        });
    },

    // Test create modal
    testCreateModal: function() {
        $.get('/SanPham/Create', function (data) {
            $('#modalContainer').html(data);
            $('#createModal').modal('show');
        });
    },
    
    // Test edit modal
    testEditModal: function() {
        // Giả sử có sản phẩm với ID = 1
        $.get('/SanPham/Edit/1', function (data) {
            $('#modalContainer').html(data);
            $('#editModal').modal('show');
        });
    },
    
    // Test delete modal
    testDeleteModal: function() {
        // Giả sử có sản phẩm với ID = 1
        $.get('/SanPham/Delete/1', function (data) {
            $('#modalContainer').html(data);
            $('#deleteModal').modal('show');
        });
    }
};

// Global functions for backward compatibility
function testCreateModal() {
    TestModal.testCreateModal();
}

function testEditModal() {
    TestModal.testEditModal();
}

function testDeleteModal() {
    TestModal.testDeleteModal();
}

// Initialize when document is ready
$(document).ready(function() {
    TestModal.init();
}); 