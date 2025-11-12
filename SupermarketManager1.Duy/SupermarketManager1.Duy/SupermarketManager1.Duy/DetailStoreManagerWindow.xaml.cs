using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SupperMarket.BLL.Service;
using SupperMarket.DAL.Models;

namespace SupermarketManager1.Duy
{
    /// <summary>
    /// Interaction logic for DetailStoreManagerWindow.xaml
    /// </summary>
    public partial class DetailStoreManagerWindow : Window
    {
        public DetailStoreManagerWindow()
        {
            InitializeComponent();
        }

        public Warehouse EditedOne { get; set; }
        private WarehouseService _ws = new();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Warehouse? warehouse = new() { };

            warehouse.WarehouseId = int.Parse(IdWarehouseTextBox.Text);
            warehouse.WarehouseName = NameWarehouseTextBox.Text;
            warehouse.Type = TypeWarehouseTextBox.Text;
            warehouse.Address = AddressWarehouseTextBox.Text;

            if (EditedOne == null)
            {
                _ws.CreateStore(warehouse);
            }
            else
            {
                _ws.UpdateStore(warehouse);
            }
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(EditedOne != null)
            {
                DetailLabel.Content = "Update the Warehouse";
                IdWarehouseTextBox.Text = EditedOne.WarehouseId.ToString();
                NameWarehouseTextBox.Text = EditedOne.WarehouseName;
                TypeWarehouseTextBox.Text = EditedOne.Type;
                AddressWarehouseTextBox.Text = EditedOne.Address;
                IdWarehouseTextBox.IsEnabled = false;
            }
            else
            {
                DetailLabel.Content = "Create the Warehouse";
            }
        }
    }
}
