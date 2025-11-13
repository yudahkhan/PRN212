using SupperMarket.BLL.Service;
using SupperMarket.DAL.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace SupermarketManager1.Duy
{
    public partial class AccountDetailWindow : Window
    {
        private AccountService _accountService = new();
        private RoleService _roleService = new();
        private WarehouseService _warehouseService = new();

        public Account? EditedAccount { get; set; }
        public bool IsEditMode => EditedAccount != null;
        public int? DefaultRoleId { get; set; } // Để set mặc định Role khi tạo mới
        public int? DefaultWarehouseId { get; set; } // Để set mặc định Warehouse khi tạo mới

        public AccountDetailWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load Roles
            RoleComboBox.ItemsSource = _roleService.GetAllRoles();

            // Load Warehouses (chỉ Store, không có Central)
            var stores = _warehouseService.GetStores();
            WarehouseComboBox.ItemsSource = stores;

            if (IsEditMode)
            {
                TitleLabel.Content = "Sửa thông tin tài khoản";
                LoadAccountData();
            }
            else
            {
                TitleLabel.Content = "Tạo tài khoản mới";
                // Set mặc định nếu có
                if (DefaultRoleId.HasValue)
                {
                    RoleComboBox.SelectedValue = DefaultRoleId.Value;
                }
                if (DefaultWarehouseId.HasValue)
                {
                    WarehouseComboBox.SelectedValue = DefaultWarehouseId.Value;
                }
            }
        }

        private void LoadAccountData()
        {
            if (EditedAccount == null) return;

            UsernameTextBox.Text = EditedAccount.Username;
            UsernameTextBox.IsEnabled = false; // Không cho sửa username
            FullNameTextBox.Text = EditedAccount.FullName;
            EmailTextBox.Text = EditedAccount.Email ?? "";
            PhoneNumberTextBox.Text = EditedAccount.PhoneNumber ?? "";
            
            if (EditedAccount.DateOfBirth.HasValue)
            {
                DateOfBirthDatePicker.SelectedDate = new DateTime(
                    EditedAccount.DateOfBirth.Value.Year,
                    EditedAccount.DateOfBirth.Value.Month,
                    EditedAccount.DateOfBirth.Value.Day);
            }

            RoleComboBox.SelectedValue = EditedAccount.RoleId;
            
            if (EditedAccount.WarehouseId.HasValue)
            {
                WarehouseComboBox.SelectedValue = EditedAccount.WarehouseId.Value;
            }

            // Set Status
            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Content.ToString() == EditedAccount.Status)
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Hiển thị Warehouse dropdown nếu chọn Manager hoặc Staff
            if (RoleComboBox.SelectedItem is Role selectedRole)
            {
                if (selectedRole.RoleName == "Manager" || selectedRole.RoleName == "Staff")
                {
                    WarehouseLabel.Visibility = Visibility.Visible;
                    WarehouseComboBox.Visibility = Visibility.Visible;
                }
                else // Admin
                {
                    WarehouseLabel.Visibility = Visibility.Collapsed;
                    WarehouseComboBox.Visibility = Visibility.Collapsed;
                    WarehouseComboBox.SelectedValue = null;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                Account account;

                if (IsEditMode)
                {
                    account = EditedAccount!;
                }
                else
                {
                    account = new Account();
                }

                // Cập nhật thông tin
                if (!IsEditMode)
                {
                    account.Username = UsernameTextBox.Text.Trim();
                    account.Password = PasswordTextBox.Password;
                }
                else if (!string.IsNullOrWhiteSpace(PasswordTextBox.Password))
                {
                    // Cho phép đổi password khi edit
                    account.Password = PasswordTextBox.Password;
                }

                account.FullName = FullNameTextBox.Text.Trim();
                account.Email = string.IsNullOrWhiteSpace(EmailTextBox.Text) ? null : EmailTextBox.Text.Trim();
                account.PhoneNumber = string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text) ? null : PhoneNumberTextBox.Text.Trim();

                if (DateOfBirthDatePicker.SelectedDate.HasValue)
                {
                    account.DateOfBirth = DateOnly.FromDateTime(DateOfBirthDatePicker.SelectedDate.Value);
                }
                else
                {
                    account.DateOfBirth = null;
                }

                account.RoleId = (int)RoleComboBox.SelectedValue!;

                // WarehouseId: NULL cho Admin, có giá trị cho Manager/Staff
                if (RoleComboBox.SelectedItem is Role role && (role.RoleName == "Manager" || role.RoleName == "Staff"))
                {
                    account.WarehouseId = WarehouseComboBox.SelectedValue as int?;
                }
                else
                {
                    account.WarehouseId = null;
                }

                // Status
                if (StatusComboBox.SelectedItem is ComboBoxItem statusItem)
                {
                    account.Status = statusItem.Content.ToString();
                }

                if (IsEditMode)
                {
                    _accountService.UpdateAccount(account);
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _accountService.CreateAccount(account);
                    MessageBox.Show("Tạo tài khoản thành công!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            // Username
            if (!IsEditMode && string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                MessageBox.Show("Vui lòng nhập Username!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                UsernameTextBox.Focus();
                return false;
            }

            if (!IsEditMode)
            {
                // Kiểm tra username đã tồn tại chưa
                var existing = _accountService.GetByUsername(UsernameTextBox.Text.Trim());
                if (existing != null)
                {
                    MessageBox.Show("Username đã tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    UsernameTextBox.Focus();
                    return false;
                }
            }

            // Password
            if (!IsEditMode && string.IsNullOrWhiteSpace(PasswordTextBox.Password))
            {
                MessageBox.Show("Vui lòng nhập Password!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                PasswordTextBox.Focus();
                return false;
            }

            if (PasswordTextBox.Password.Length < 6)
            {
                MessageBox.Show("Password phải có ít nhất 6 ký tự!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                PasswordTextBox.Focus();
                return false;
            }

            // Full Name
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
            {
                MessageBox.Show("Vui lòng nhập Họ tên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                FullNameTextBox.Focus();
                return false;
            }

            // Email validation
            if (!string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                if (!Regex.IsMatch(EmailTextBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Email không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    EmailTextBox.Focus();
                    return false;
                }
            }

            // Role
            if (RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Vai trò!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Warehouse (nếu là Manager hoặc Staff)
            if (RoleComboBox.SelectedItem is Role role && (role.RoleName == "Manager" || role.RoleName == "Staff"))
            {
                if (WarehouseComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn Store!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}

