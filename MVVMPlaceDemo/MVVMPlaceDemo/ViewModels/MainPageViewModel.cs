using System;
using System.ComponentModel;
using System.Diagnostics;
using MVVMPlaceDemo.Helpers;
using MVVMPlaceDemo.Interfaces;
using MVVMPlaceDemo.Models;
using Xamarin.Forms;

namespace MVVMPlaceDemo.ViewModels
{
    public class MainPageViewModel: BaseViewModel
    {
        #region vars
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string UserMessage { get; set; }
        public bool StartEnabled { get; set; }
        public bool StopEnabled { get; set; }
        #endregion vars

        private ILocationStore _locationStore;


        public MainPageViewModel()
        {
            StartCommand = new Command(() => OnStartClick());
            EndCommand = new Command(() => OnStopClick());
            HandleReceivedMessages();
            StartEnabled = true;
            StopEnabled = false;
            _locationStore = new SQLiteLocationStore(DependencyService.Get<ISQLiteDb>());
        }

        public void OnStartClick()
        {
            Start();
        }

        void Start()
        {
            var message = new StartServiceMessage();
            MessagingCenter.Send(message, "ServiceStarted");
            UserMessage = "Location Service has been started!";
            //SecureStorage.SetAsync(Constants.SERVICE_STATUS_KEY, "1");
            StartEnabled = false;
            StopEnabled = true;
        }



        public void OnStopClick()
        {
            var message = new StopServiceMessage();
            MessagingCenter.Send(message, "ServiceStopped");
            UserMessage = "Location Service has been stopped!";
           
            StartEnabled = true;
            StopEnabled = false;
        }

        void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<LocationMessage>(this, "Location", message => {
                Device.BeginInvokeOnMainThread(() => {
                    Latitude = message.Latitude;
                    Longitude = message.Longitude;
                    UserMessage = "Location Updated";

                    addDataIntoDb(Latitude, Longitude);

                    Debug.WriteLine($"Latitude: {Latitude}, Longitude: {Longitude}, UserMessage: {UserMessage}");
                });
            });
            MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped", message => {
                Device.BeginInvokeOnMainThread(() => {
                    UserMessage = "Location Service has been stopped!";
                });
            });
            MessagingCenter.Subscribe<LocationErrorMessage>(this, "LocationError", message => {
                Device.BeginInvokeOnMainThread(() => {
                    UserMessage = "There was an error updating location!";
                });
            });
        }

        private void addDataIntoDb(double lattitude,double longitude)
        {
            LocationEntry location = new LocationEntry();
            location.lat = lattitude;
            location.lng = longitude;
             _locationStore.AddLocation(location);

        }


        #region commands
        public Command StartCommand { get; }
        public Command EndCommand { get; }
        #endregion commands


        
    }
}
