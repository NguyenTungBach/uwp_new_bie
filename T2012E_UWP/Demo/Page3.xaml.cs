using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using T2012E_UWP.Entity;
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

namespace T2012E_UWP.Demo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page3 : Page
    {
        private static readonly HttpClient client = new HttpClient();
        public Page3()
        {
            this.InitializeComponent();

            var account = new Account()
            {
                firstName = "Nguyễn Tùng",
                lastName = "Bách",
                password = "123",
                address = "Ha Noi",
                phone = "0912345678",
                avatar = "https://trumboss.com/wp-content/uploads/2018/10/xa-hoi-hoa-meo-con-696x530.jpg",
                gender = 1,
                email = "bach@gmail.com",
                birthday = "1998-06-02",
            };

            //string API = "https://music-i-like.herokuapp.com/api/v1/accounts";
            //string myParameters = "param1=value1&param2=value2&param3=value3";

            string responseString = "";
            using (WebClient client = new WebClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "thing1", "hello" },
                    { "thing2", "world" }
                };


                //responseString = Encoding.Default.GetString(response);
            }

            ContentDialog contentDialog = new ContentDialog();
            contentDialog.Title = "Đã chọn";
            contentDialog.Content = responseString;
            contentDialog.CloseButtonText = "OK";
            //contentDialog.ShowAsync();
            return;
        }
    }
}
