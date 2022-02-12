using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_SQLite.Entity;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class DetailTransaction : Page
    {
        public PersonalTransaction personalDetail;
        public DetailTransaction()
        {
            this.InitializeComponent();
            this.Loaded += DetailTransaction_Loaded;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            personalDetail = DataTransaction.personal;
            //ApplicationData.Current.LocalSettings.Values["CurrentText"] = JsonConvert.SerializeObject(personalDetail);
            if (personalDetail == null)
            {
                ApplicationData.Current.LocalSettings.Values["btnNameText"] = "Waiting";
                ApplicationData.Current.LocalSettings.Values["btnDescriptionText"] = "Waiting";
                ApplicationData.Current.LocalSettings.Values["btnDetailText"] = "Waiting";
                ApplicationData.Current.LocalSettings.Values["btnMoneyText"] = "Waiting";
                ApplicationData.Current.LocalSettings.Values["btnCreatedText"] = "Waiting";
                ApplicationData.Current.LocalSettings.Values["btnCategoryText"] = "Waiting";
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values["btnNameText"] = personalDetail.Name.ToString();
                ApplicationData.Current.LocalSettings.Values["btnDescriptionText"] = personalDetail.Description.ToString();
                ApplicationData.Current.LocalSettings.Values["btnDetailText"] = personalDetail.Detail.ToString();
                ApplicationData.Current.LocalSettings.Values["btnMoneyText"] = personalDetail.Money.ToString();
                ApplicationData.Current.LocalSettings.Values["btnCreatedText"] = personalDetail.CreatedDate.ToString();
                ApplicationData.Current.LocalSettings.Values["btnCategoryText"] = personalDetail.Category.ToString();
            }
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("btnNameText"))
            {
                CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                //personalDetail = JsonConvert.DeserializeObject<PersonalTransaction>((string)ApplicationData.Current.RoamingSettings.Values["CurrentText"]);
                var getDetailAffterNavigatedbtnName = (string)ApplicationData.Current.LocalSettings.Values["btnNameText"];
                var getDetailAffterNavigatedbtnDescription = (string)ApplicationData.Current.LocalSettings.Values["btnDescriptionText"];
                var getDetailAffterNavigatedbtnDetail = (string)ApplicationData.Current.LocalSettings.Values["btnDetailText"];
                var getDetailAffterNavigatedbtnMoney = (string)ApplicationData.Current.LocalSettings.Values["btnMoneyText"];
                var getDetailAffterNavigatedbtnCreated = (string)ApplicationData.Current.LocalSettings.Values["btnCreatedText"];
                var getDetailAffterNavigatedbtnCategory = (string)ApplicationData.Current.LocalSettings.Values["btnCategoryText"];
                if (getDetailAffterNavigatedbtnName == null)
                {
                    btnName.Text = "Waiting";
                    btnDescription.Text = "Waiting";
                    btnDetail.Text = "Waiting";
                    btnMoney.Text = "Waiting";
                    btnCreatedDate.Text = "Waiting";
                    btnCategory.Text = "Waiting";
                }
                else
                {
                    btnName.Text = getDetailAffterNavigatedbtnName;
                    btnDescription.Text = getDetailAffterNavigatedbtnDescription;
                    btnDetail.Text = getDetailAffterNavigatedbtnDetail;
                    btnMoney.Text = getDetailAffterNavigatedbtnMoney + " Đồng";
                    btnCreatedDate.Text = getDetailAffterNavigatedbtnCreated;
                    btnCategory.Text = getDetailAffterNavigatedbtnCategory;
                }
            }

            base.OnNavigatedTo(e);
        }

        private void DetailTransaction_Loaded(object sender, RoutedEventArgs e)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            if(DataTransaction.personal != null)
            {
                personalDetail = DataTransaction.personal;
                btnName.Text = personalDetail.Name.ToString();
                btnDescription.Text = personalDetail.Description.ToString();
                btnDetail.Text = personalDetail.Detail.ToString();
                btnMoney.Text = Convert.ToDouble(personalDetail.Money).ToString("#,###", cul.NumberFormat) + " Đồng";
                btnCreatedDate.Text = personalDetail.CreatedDate.ToString("dd-MM-yyyy");
                btnCategory.Text = personalDetail.Category.ToString();
            }
            
        }

        private void ReturnListTransaction_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pages.DataTransaction));
        }


    }
}
