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

        /// <summary>
        /// Clicked event of <see cref="BtnRegister"/>
        /// </summary>
        private async void BtnRegister_Clicked(object sender, EventArgs e)
        {
            // Activity feedback on
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

            // Activity feedback off
            Activity.IsRunning = false;
            Activity.IsVisible = false;

            // Popup message saying if registration was succesful
            await DisplayAlert("Registration", $"{InputName.Text} has {(res ? "" : "NOT ")}been registered.", "OK");
            if (res)
                InputName.Text = "";
        }

        /// <summary>
        /// Register this device with <paramref name="name"/> as username, via the webapi
        /// </summary>
        /// <param name="name">The username as chosen by user</param>
        /// <returns><see langword="true"/> if registration is succesful, otherwise <see langword="false"/></returns>
        private async Task<bool> RegisterUser(string name)
        {
            // Android ID on android
            string did = DependencyService.Get<IDevice>().GetIdentifier();

            var content = new { DID = did, UserName = name };

            // WebApi client
            RestClient client = new RestClient("https://bluenetweb.azurewebsites.net/api/registrar");
            // Post request with DID and UserName in a json body
            IRestRequest req = new RestRequest(Method.POST).AddJsonBody(content);
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
            {
                // Make request and return true if succesful
                IRestResponse res = await client.ExecuteTaskAsync(req, cancellationTokenSource.Token);
                return res.StatusCode == System.Net.HttpStatusCode.OK;
            }
        }
    }
}
