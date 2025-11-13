# 📋 NGHIỆP VỤ HỆ THỐNG - SUPERMARKET MANAGEMENT SYSTEM

## 🎯 MỤC ĐÍCH HỆ THỐNG

Hệ thống quản lý siêu thị đa chi nhánh, hỗ trợ:
- Quản lý sản phẩm, tồn kho
- Quản lý nhiều kho/cửa hàng
- Phân quyền theo vai trò (Admin, Manager, Staff)
- Quản lý bán hàng và doanh thu
- Chuyển hàng giữa các kho

---

## 👥 CÁC VAI TRÒ (ROLES)

### 1. **ADMIN (Quản trị viên)**
**Quyền hạn:**
- ✅ Quản lý TẤT CẢ sản phẩm (CRUD Products)
- ✅ Quản lý TẤT CẢ kho/cửa hàng (CRUD Warehouses)
- ✅ Quản lý TẤT CẢ tài khoản (CRUD Accounts)
- ✅ Gán Manager cho các Store
- ✅ Xem tất cả dữ liệu hệ thống
- ✅ Không thuộc Store nào (WarehouseId = NULL)

**Nghiệp vụ:**
- Tạo/sửa/xóa sản phẩm trong hệ thống
- Tạo/sửa/xóa kho trung tâm và các cửa hàng chi nhánh
- Tạo tài khoản Manager và Staff
- Gán Manager quản lý từng Store
- Gán Staff vào các Store

---

### 2. **MANAGER (Quản lý cửa hàng)**
**Quyền hạn:**
- ✅ Quản lý Store được gán (chỉ 1 Store)
- ✅ Quản lý Staff trong Store của mình (CRUD Staff)
- ✅ Xem tồn kho của Store mình quản lý
- ✅ Xem doanh thu của Store mình
- ✅ Chuyển hàng từ Kho Trung Tâm → Store của mình
- ❌ KHÔNG thể quản lý sản phẩm (chỉ Admin)
- ❌ KHÔNG thể quản lý Store khác

**Nghiệp vụ:**
- Xem và quản lý tồn kho của Store
- Tạo/sửa/xóa tài khoản Staff trong Store
- Yêu cầu chuyển hàng từ Kho Trung Tâm về Store
- Xem báo cáo doanh thu Store

---

### 3. **STAFF (Nhân viên bán hàng)**
**Quyền hạn:**
- ✅ Bán hàng tại Store được gán
- ✅ Xem tồn kho của Store mình làm việc
- ✅ Tạo hóa đơn bán hàng
- ❌ KHÔNG thể quản lý sản phẩm
- ❌ KHÔNG thể quản lý kho
- ❌ KHÔNG thể quản lý tài khoản

**Nghiệp vụ:**
- Bán hàng cho khách
- Tạo hóa đơn (Invoice)
- Xem tồn kho để tư vấn khách hàng

---

## 🏢 CẤU TRÚC KHO HÀNG

### **Kho Trung Tâm (Central Warehouse)**
- Là kho chính, chứa tất cả sản phẩm
- Admin quản lý trực tiếp
- Không có Manager (ManagerId = NULL)
- Chuyển hàng từ đây → các Store

### **Cửa Hàng Chi Nhánh (Store)**
- Các cửa hàng bán lẻ
- Mỗi Store có 1 Manager quản lý
- Có nhiều Staff làm việc
- Nhận hàng từ Kho Trung Tâm

**Ví dụ:**
```
Kho Trung Tâm (Central)
    ↓ (chuyển hàng)
Cửa hàng Chi nhánh 1 (Store) - Manager: manager1
    ├── Staff: staff1
    └── Staff: staff2

Cửa hàng Chi nhánh 2 (Store) - Manager: manager2
    └── Staff: staff3
```

---

## 📦 QUẢN LÝ SẢN PHẨM & TỒN KHO

### **1. Quản lý Sản phẩm (Products)**
- **Admin** tạo/sửa/xóa sản phẩm
- Mỗi sản phẩm có:
  - Mã sản phẩm (ProductCode) - duy nhất
  - Tên sản phẩm
  - Danh mục (Category)
  - Giá bán
  - Nhà cung cấp
  - Bảo hành
  - Mô tả

### **2. Quản lý Tồn kho (Inventory)**
- Tồn kho được quản lý **theo từng kho**
- Mỗi sản phẩm có thể có tồn kho ở nhiều kho khác nhau
- **Ví dụ:**
  - P001 (Áo thun) có 500 cái ở Kho Trung Tâm
  - P001 có 50 cái ở Store 1
  - P001 có 40 cái ở Store 2

### **3. Điều chỉnh Tồn kho**
- **Admin**: Điều chỉnh tồn kho ở TẤT CẢ các kho
- **Manager**: Điều chỉnh tồn kho ở Store của mình
- **Staff**: Chỉ xem, không điều chỉnh

**Các thao tác:**
- ➕ Tăng số lượng
- ➖ Giảm số lượng
- 📝 Điều chỉnh số lượng cố định

---

## 🚚 CHUYỂN KHO (Stock Transfer)

### **Quy trình:**
1. **Admin/Manager** yêu cầu chuyển hàng
2. Chọn kho nguồn (From) và kho đích (To)
3. Chọn sản phẩm và số lượng
4. Hệ thống kiểm tra:
   - Kho nguồn có đủ hàng không?
   - Nếu đủ → Trừ kho nguồn, Cộng kho đích
   - Nếu không đủ → Báo lỗi

### **Ví dụ:**
```
Kho Trung Tâm (500 áo thun)
    ↓ Chuyển 50 cái
Store 1 (50 áo thun)
```

**Kết quả:**
- Kho Trung Tâm: 500 - 50 = 450
- Store 1: 50 + 50 = 100

---

## 💰 BÁN HÀNG (Sales)

### **Quy trình bán hàng:**
1. **Staff** đăng nhập vào hệ thống
2. Chọn sản phẩm từ Store mình làm việc
3. Nhập số lượng bán
4. Hệ thống:
   - Lấy giá hiện tại của sản phẩm
   - Tính tổng tiền = Số lượng × Giá
   - Tạo bản ghi Sales
   - **Tự động trừ tồn kho** (Trigger)

### **Thông tin lưu trong Sales:**
- Staff nào bán (AccountId)
- Bán từ Store nào (WarehouseId)
- Sản phẩm gì (ProductCode)
- Số lượng (QuantitySold)
- Giá tại thời điểm bán (UnitPrice) - **Quan trọng!**
- Tổng tiền (TotalAmount)
- Ngày bán (SaleDate)

### **Ví dụ:**
```
Staff1 bán tại Store 1:
- 2 áo thun × 150,000 = 300,000 VNĐ
- 1 quần jean × 350,000 = 350,000 VNĐ
Tổng: 650,000 VNĐ
```

**Lưu ý:** Giá bán được lưu lại để báo cáo chính xác, dù giá sản phẩm có thay đổi sau này.

---

## 🔐 PHÂN QUYỀN & BẢO MẬT

### **Quy tắc phân quyền:**

1. **Admin:**
   - WarehouseId = NULL (không thuộc Store nào)
   - Xem và quản lý TẤT CẢ

2. **Manager:**
   - WarehouseId = Store mà họ quản lý
   - Chỉ xem/quản lý Store của mình
   - Quản lý Staff trong Store

3. **Staff:**
   - WarehouseId = Store mà họ làm việc
   - Chỉ xem Store của mình
   - Chỉ bán hàng, không quản lý

### **Ví dụ:**
```
Manager1 (WarehouseId = 2) → Store 1
    ├── Chỉ thấy tồn kho Store 1
    ├── Chỉ quản lý Staff trong Store 1
    └── KHÔNG thấy Store 2

Staff1 (WarehouseId = 2) → Store 1
    ├── Chỉ thấy tồn kho Store 1
    ├── Chỉ bán hàng tại Store 1
    └── KHÔNG thấy Store 2
```

---

## 📊 LUỒNG HOẠT ĐỘNG CHÍNH

### **1. Luồng Admin:**
```
Login → Admin Dashboard
    ├── Quản lý Sản phẩm (Products)
    │   ├── Create
    │   ├── Update
    │   └── Delete
    ├── Quản lý Kho (Warehouses)
    │   ├── Create Store
    │   ├── Gán Manager cho Store
    │   └── Delete
    └── Quản lý Tài khoản (Accounts)
        ├── Create Manager/Staff
        ├── Gán Staff vào Store
        └── Update/Delete
```

### **2. Luồng Manager:**
```
Login → Manager Dashboard
    ├── Xem tồn kho Store của mình
    ├── Điều chỉnh tồn kho
    ├── Quản lý Staff trong Store
    │   ├── Create Staff
    │   ├── Update Staff
    │   └── Delete Staff
    └── Xem báo cáo doanh thu Store
```

### **3. Luồng Staff:**
```
Login → Staff Dashboard
    ├── Xem tồn kho Store
    └── Bán hàng
        ├── Chọn sản phẩm
        ├── Nhập số lượng
        └── Tạo hóa đơn (Invoice)
```

---

## 🔄 CÁC QUY TRÌNH NGHIỆP VỤ

### **1. Quy trình Nhập hàng mới:**
```
Admin tạo sản phẩm mới
    ↓
Admin nhập hàng vào Kho Trung Tâm
    ↓
Admin chuyển hàng từ Kho Trung Tâm → Các Store
```

### **2. Quy trình Bán hàng:**
```
Staff chọn sản phẩm
    ↓
Nhập số lượng
    ↓
Hệ thống kiểm tra tồn kho
    ↓
Nếu đủ → Tạo Sales + Trừ tồn kho
Nếu không đủ → Báo lỗi
```

### **3. Quy trình Chuyển kho:**
```
Admin/Manager yêu cầu chuyển kho
    ↓
Chọn kho nguồn, kho đích, sản phẩm, số lượng
    ↓
Hệ thống kiểm tra tồn kho nguồn
    ↓
Nếu đủ → Trừ kho nguồn + Cộng kho đích
Nếu không đủ → Báo lỗi
```

---

## 📈 BÁO CÁO & THỐNG KÊ

### **Các báo cáo có thể tạo:**
1. **Doanh thu theo Store:**
   - Tổng doanh thu mỗi Store
   - Doanh thu theo ngày/tuần/tháng

2. **Sản phẩm bán chạy:**
   - Top sản phẩm bán nhiều nhất
   - Sản phẩm bán chạy theo Store

3. **Tồn kho:**
   - Tồn kho hiện tại theo kho
   - Sản phẩm sắp hết hàng

---

## ⚠️ CÁC RÀNG BUỘC NGHIỆP VỤ

1. **Không thể bán hàng nếu không đủ tồn kho**
2. **Không thể chuyển kho nếu kho nguồn không đủ hàng**
3. **Manager chỉ quản lý 1 Store**
4. **Staff chỉ làm việc tại 1 Store**
5. **Admin không thuộc Store nào**
6. **Kho Trung Tâm không có Manager**

---

## 🎯 TÓM TẮT

**Hệ thống này giúp:**
- ✅ Quản lý đa chi nhánh hiệu quả
- ✅ Phân quyền rõ ràng theo vai trò
- ✅ Theo dõi tồn kho chính xác
- ✅ Quản lý bán hàng và doanh thu
- ✅ Tự động hóa các quy trình (trừ tồn kho, chuyển kho)

**Điểm mạnh:**
- Cấu trúc rõ ràng, dễ mở rộng
- Phân quyền linh hoạt
- Tự động hóa nhiều thao tác
- Hỗ trợ đa chi nhánh

