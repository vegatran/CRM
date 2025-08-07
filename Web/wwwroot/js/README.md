# JavaScript Files Documentation

## DataTable Configuration

### Hàm khởi tạo DataTable dùng chung

Để thống nhất cấu hình DataTable và dễ dàng thay đổi ngôn ngữ cho toàn bộ hệ thống, chúng ta sử dụng hàm `Common.initDataTable()`.

#### Cách sử dụng:

```javascript
// 1. Cơ bản - sử dụng cấu hình mặc định
Common.initDataTable('#myTable');

// 2. Với tùy chọn bổ sung
Common.initDataTable('#myTable', {
    "order": [[0, "asc"]],
    "pageLength": 10
});

// 3. Với columnDefs (vô hiệu hóa sắp xếp cho cột cụ thể)
Common.initDataTable('#myTable', {
    "columnDefs": [
        { "orderable": false, "targets": [5] } // Vô hiệu hóa sắp xếp cho cột thứ 6
    ]
});
```

#### Thay đổi ngôn ngữ:

Để thay đổi ngôn ngữ cho toàn bộ hệ thống, chỉ cần cập nhật hàm `getDataTableLanguage()` trong file `common.js`:

```javascript
// Trong file: Web/wwwroot/js/common.js
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
}
```

#### Cấu hình mặc định:

- **Responsive**: true
- **AutoWidth**: false
- **PageLength**: 25
- **LengthMenu**: [10, 25, 50, 100, -1] (Tất cả)
- **Language**: Tiếng Việt

## Files Structure

- `common.js` - Các functions JavaScript chung, bao gồm hàm DataTable dùng chung
- `sanpham.js` - Quản lý sản phẩm
- `nguyenlieu.js` - Quản lý nguyên liệu
- `nhacungcap.js` - Quản lý nhà cung cấp
- `dinhmucnguyenlieu.js` - Quản lý định mức nguyên liệu
- `quyTrinhSanXuat.js` - Quản lý quy trình sản xuất
- `chiTietSanXuat.js` - Quản lý chi tiết sản xuất

## Usage Examples

### 1. Khởi tạo DataTable cơ bản

```javascript
// Trong file JavaScript của bạn
const MyModule = {
    init: function() {
        this.initDataTable();
    },
    
    initDataTable: function() {
        Common.initDataTable('#myTable');
    }
};
```

### 2. Khởi tạo DataTable với tùy chọn

```javascript
initDataTable: function() {
    Common.initDataTable('#myTable', {
        "order": [[0, "asc"]],
        "columnDefs": [
            { "orderable": false, "targets": [5] }
        ],
        "pageLength": 10
    });
}
```

### 3. Thay đổi ngôn ngữ toàn hệ thống

```javascript
// Trong common.js, thay đổi hàm getDataTableLanguage()
getDataTableLanguage: function() {
    return {
        "sProcessing": "Processing...",
        "sLengthMenu": "Show _MENU_ entries",
        "sZeroRecords": "No data found",
        "sInfo": "Showing _START_ to _END_ of _TOTAL_ entries",
        "sInfoEmpty": "Showing 0 to 0 of 0 entries",
        "sInfoFiltered": "(filtered from _MAX_ total entries)",
        "sSearch": "Search:",
        "sEmptyTable": "No data available in table",
        "sLoadingRecords": "Loading...",
        "oPaginate": {
            "sFirst": "First",
            "sPrevious": "Previous",
            "sNext": "Next",
            "sLast": "Last"
        }
    };
}
```

## Benefits

1. **Thống nhất**: Tất cả DataTable sử dụng cùng cấu hình cơ bản
2. **Dễ bảo trì**: Chỉ cần thay đổi ở một nơi để áp dụng cho toàn bộ hệ thống
3. **Linh hoạt**: Vẫn có thể tùy chỉnh cho từng table cụ thể
4. **Hiệu suất**: Giảm code trùng lặp
5. **Dễ mở rộng**: Dễ dàng thêm tính năng mới cho tất cả DataTable 