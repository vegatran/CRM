# JavaScript Files Structure

## Tổng quan
Tất cả JavaScript functions đã được tách ra khỏi các view và tổ chức thành các file riêng biệt để dễ quản lý và bảo trì.

## Cấu trúc file

### 1. `common.js`
**Mục đích:** Chứa các functions chung được sử dụng trong toàn bộ ứng dụng

**Các tính năng:**
- Currency formatting (định dạng tiền tệ)
- DataTables initialization
- Color picker handling
- Image preview handling
- Toastr configuration
- Modal utilities
- AJAX helpers
- Form helpers

**Các functions chính:**
- `Common.init()` - Khởi tạo tất cả tính năng chung
- `Common.formatCurrency()` - Định dạng tiền tệ
- `Common.parseCurrency()` - Parse tiền tệ
- `Common.showSuccess()`, `Common.showError()` - Hiển thị thông báo
- `Common.ajax()` - Helper cho AJAX requests
- `Common.serializeForm()` - Serialize form với xử lý currency

### 2. `dashboard.js`
**Mục đích:** Quản lý dashboard và biểu đồ

**Các tính năng:**
- Sales chart initialization
- Revenue chart initialization
- Chart configuration

**Các functions chính:**
- `Dashboard.init()` - Khởi tạo dashboard
- `Dashboard.initSalesChart()` - Khởi tạo biểu đồ doanh số
- `Dashboard.initRevenueChart()` - Khởi tạo biểu đồ doanh thu

### 3. `quyTrinhSanXuat.js`
**Mục đích:** Quản lý quy trình sản xuất

**Các tính năng:**
- Modal management cho Create/Edit
- Form handling với currency formatting
- AJAX operations
- List reloading

**Các functions chính:**
- `QuyTrinhSanXuat.openCreateModal()` - Mở modal tạo mới
- `QuyTrinhSanXuat.openEditModal()` - Mở modal chỉnh sửa
- `QuyTrinhSanXuat.deleteQuyTrinh()` - Xóa quy trình
- `QuyTrinhSanXuat.loadQuyTrinhList()` - Load danh sách
- `QuyTrinhSanXuat.handleCreateForm()` - Xử lý form tạo mới
- `QuyTrinhSanXuat.handleEditForm()` - Xử lý form chỉnh sửa

### 4. `chiTietSanXuat.js`
**Mục đích:** Quản lý chi tiết sản xuất

**Các tính năng:**
- Load định mức nguyên liệu
- Load quy trình sản xuất

**Các functions chính:**
- `ChiTietSanXuat.init()` - Khởi tạo
- `ChiTietSanXuat.loadDinhMucNguyenLieu()` - Load định mức
- `ChiTietSanXuat.loadQuyTrinhSanXuat()` - Load quy trình

### 5. `testModal.js`
**Mục đích:** Test modal functionality

**Các tính năng:**
- Test create modal
- Test edit modal
- Test delete modal
- Modal event handling

**Các functions chính:**
- `TestModal.testCreateModal()` - Test modal tạo mới
- `TestModal.testEditModal()` - Test modal chỉnh sửa
- `TestModal.testDeleteModal()` - Test modal xóa

## Cách sử dụng

### 1. Include trong Layout
File `common.js` đã được include trong `_Layout.cshtml`:
```html
<script src="~/js/common.js"></script>
```

### 2. Include trong View cụ thể
```html
@section Scripts {
    <script src="~/js/quyTrinhSanXuat.js"></script>
}
```

### 3. Sử dụng trong HTML
```html
<button onclick="openCreateQuyTrinhModal(1)">Thêm quy trình</button>
```

## Global Functions
Để đảm bảo backward compatibility, các global functions vẫn được giữ lại:
- `openCreateQuyTrinhModal()`
- `openEditQuyTrinhModal()`
- `deleteQuyTrinh()`
- `loadQuyTrinhList()`
- `testCreateModal()`
- `testEditModal()`
- `testDeleteModal()`

## Best Practices

### 1. Module Pattern
Sử dụng module pattern với `const ModuleName = {}` để tổ chức code.

### 2. Event Delegation
Sử dụng `$(document).on()` để handle events cho dynamic content.

### 3. Error Handling
Tất cả AJAX calls đều có error handling với toastr notifications.

### 4. Currency Formatting
Tự động format currency inputs với Vietnamese locale.

### 5. Modal Management
Centralized modal management với proper cleanup.

## Maintenance

### Thêm function mới:
1. Tạo function trong module tương ứng
2. Thêm global function nếu cần backward compatibility
3. Update documentation

### Sửa lỗi:
1. Kiểm tra console errors
2. Verify AJAX responses
3. Check event bindings
4. Test modal functionality

### Performance:
- Sử dụng event delegation
- Minimize DOM queries
- Cache jQuery objects
- Use proper selectors 