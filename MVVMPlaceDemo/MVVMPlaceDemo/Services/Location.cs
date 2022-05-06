using System;
using System.Threading;
using System.Threading.Tasks;
using MVVMPlaceDemo.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MVVMPlaceDemo.Services
{
    public class Location
    {
        readonly bool stopping = false;

        public Location()
        {
        }

		public  Task Run(CancellationToken token)
		{
			return Task.Run(async () => {
				while (!stopping)
				{
					token.ThrowIfCancellationRequested();
					try
					{
						await Task.Delay(1000);

						Device.BeginInvokeOnMainThread(async () => 
						{
							var request = new GeolocationRequest(GeolocationAccuracy.High);
							var location = await Geolocation.GetLocationAsync(request);
							if (location != null)
							{
								var message = new LocationMessage
								{
									Latitude = location.Latitude,
									Longitude = location.Longitude
								};

								Device.BeginInvokeOnMainThread(() =>
								{

									MessagingCenter.Send<LocationMessage>(message, "Location");
								});
							}
						});

						
					}
					catch (Exception ex)
					{
						Device.BeginInvokeOnMainThread(() =>
						{
							var errormessage = new LocationErrorMessage();
							MessagingCenter.Send<LocationErrorMessage>(errormessage, "LocationError");
						});
					}
				}
				return;
			}, token);
		}
	}
}
