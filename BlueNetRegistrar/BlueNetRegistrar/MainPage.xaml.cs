using RestSharp;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BlueNetRegistrar
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void BtnRegister_Clicked(object sender, EventArgs e)
        {
            Activity.IsRunning = true;
            Activity.IsVisible = true;

            OnPropertyChanged();

            // Has to input a name
            if (InputName.Text == null || InputName.Text.Length <= 0)
            {
                InputName.Text = "";
                InputName.Placeholder = "Input a name";
                return;
            }

            bool res = await RegisterUser(InputName.Text).ConfigureAwait(true);

            Activity.IsRunning = false;
            Activity.IsVisible = false;

            await DisplayAlert("Registration", $"{InputName.Text} has {(res ? "" : "NOT ")}been registered.", "OK");
            if (res)
                InputName.Text = "";
        }

        private async Task<bool> RegisterUser(string name)
        {
            string did = DependencyService.Get<IDevice>().GetIdentifier();

            RestClient client = new RestClient("https://bluenetweb.azurewebsites.net/api/registrar");
            IRestRequest req = new RestRequest(Method.POST).AddJsonBody(new { DID = did, UserName = name });
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
            {
                IRestResponse res = await client.ExecuteTaskAsync(req, cancellationTokenSource.Token);

                return res.StatusCode == System.Net.HttpStatusCode.OK;
            }
        }
    }
}
