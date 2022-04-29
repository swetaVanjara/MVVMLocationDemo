using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MVVMPlaceDemo.ViewModels;
using Plugin.LocalNotifications;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MVVMPlaceDemo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        Location oldLocation = null;
        CancellationTokenSource cts;

        MainPageViewModel mainPageViewModel;
        public MainPage()
        {
            mainPageViewModel = new MainPageViewModel();
            InitializeComponent();
            BindingContext = mainPageViewModel;
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
              //  GetCurrentLocation();
                return true;
            });
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await GetCurrentLocation();
        }

        async Task GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Debug.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    if (oldLocation == null)
                    {
                        oldLocation = location;
                        
                    }
                    if (location.Latitude != oldLocation.Latitude || location.Longitude != oldLocation.Longitude)
                    {
                        oldLocation = location;
                        var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                        var placemark = placemarks?.FirstOrDefault();
                        if (placemark != null)
                        {
                            var geocodeAddress =
                                $"AdminArea:       {placemark.AdminArea}\n" +
                                $"CountryCode:     {placemark.CountryCode}\n" +
                                $"CountryName:     {placemark.CountryName}\n" +
                                $"FeatureName:     {placemark.FeatureName}\n" +
                                $"Locality:        {placemark.Locality}\n" +
                                $"PostalCode:      {placemark.PostalCode}\n" +
                                $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                                $"SubLocality:     {placemark.SubLocality}\n" +
                                $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                                $"Location :       {placemark.Location}\n" +
                                $"Thoroughfare:    {placemark.Thoroughfare}\n";

                            Debug.WriteLine(geocodeAddress);
                        }

                        CrossLocalNotifications.Current.Show("Location Updated", "You checked in to " + placemark.FeatureName + " " + placemark.Locality + " " + placemark.SubLocality, 101, DateTime.Now.AddSeconds(5));
                    }
                }
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException)
            {
                // Handle permission exception
            }
            catch (Exception)
            {
                // Unable to get location
            }
        
    }
    }
}
