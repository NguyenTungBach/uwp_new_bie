using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_SQLite.Entity;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_SQLite.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DataTransaction : Page
    {
        public List<PersonalTransaction> listTransaction;
        public PersonalTransaction personal;
        private string checkedStartDate;
        private string checkedEndDate;
        public DataTransaction()
        {
            this.InitializeComponent();
            this.Loaded += DataTransaction_Loaded;
        }

        private void DataTransaction_Loaded(object sender, RoutedEventArgs e)
        {
            listTransaction = Server.DataInitialization.ListTransaction();
            Debug.WriteLine(listTransaction);
            ListDataGridTransaction.ItemsSource = listTransaction;
            btnTotalMoney.Text = Server.DataInitialization.totalMoney.ToString();
        }

        private void CreateTransactionButton(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pages.CreatedTransaction));
        }

        private void ListDataGridTransaction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            personal = ListDataGridTransaction.SelectedItem as PersonalTransaction;
            if(personal != null)
            {
                btnName.Text = personal.Name.ToString();
                btnDescription.Text = personal.Description.ToString();
                btnDetail.Text = personal.Detail.ToString();
                btnMoney.Text = Convert.ToDouble(personal.Money).ToString("#,###", cul.NumberFormat) + " Đồng";
                btnCreatedDate.Text = personal.CreatedDate.ToString("dd-MM-yyyy");
                btnCategory.Text = personal.Category.ToString();
                
            }
            else
            {
                btnName.Text = "Waiting";
                btnDescription.Text = "Waiting";
                btnDetail.Text = "Waiting";
                btnMoney.Text = "Waiting";
                btnCreatedDate.Text = "Waiting";
                btnCategory.Text = "Waiting";
            }
        }

        private void FindByCategory(object sender, RoutedEventArgs e)
        {
            personal = null;
            btnName.Text = "Waiting";
            btnDescription.Text = "Waiting";
            btnDetail.Text = "Waiting";
            btnMoney.Text = "Waiting";
            btnCreatedDate.Text = "Waiting";
            btnCategory.Text = "Waiting";
            List<PersonalTransaction> listTransactionByCategory = Server.DataInitialization.ListTransactionByCategory(Convert.ToInt32(searchCategory.Text));
            ListDataGridTransaction.ItemsSource = listTransactionByCategory;
            btnTotalMoney.Text = Server.DataInitialization.totalMoney.ToString();
        }

        private void ResetList(object sender, RoutedEventArgs e)
        {
            btnName.Text = "Waiting";
            btnDescription.Text = "Waiting";
            btnDetail.Text = "Waiting";
            btnMoney.Text = "Waiting";
            btnCreatedDate.Text = "Waiting";
            btnCategory.Text = "Waiting";
            ListDataGridTransaction.ItemsSource = listTransaction;
            btnTotalMoney.Text = Server.DataInitialization.totalMoney.ToString();
        }

        private void FindByStartDateAndEndDate(object sender, RoutedEventArgs e)
        {
            
            personal = null;
            btnName.Text = "Waiting";
            btnDescription.Text = "Waiting";
            btnDetail.Text = "Waiting";
            btnMoney.Text = "Waiting";
            btnCreatedDate.Text = "Waiting";
            btnCategory.Text = "Waiting";
            checkedStartDate = startDate.Date.ToString("yyyy-dd-MM");
            checkedEndDate = endDate.Date.ToString("yyyy-dd-MM");
            ListDataGridTransaction.ItemsSource = Server.DataInitialization.ListTransactionByStartDateAndEndDate(checkedStartDate,checkedEndDate);
            btnTotalMoney.Text = Server.DataInitialization.totalMoney.ToString();
        }

        private void startDate_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            
        }

        private void endDate_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            
        }
    }
}
