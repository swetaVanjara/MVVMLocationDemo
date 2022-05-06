using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using MVVMPlaceDemo.iOS.Services;
using MVVMPlaceDemo.Models;
using UIKit;
using Xamarin.Forms;

namespace MVVMPlaceDemo.iOS
{
    
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        iOsLocationService locationService;
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            locationService = new iOsLocationService();
            SetServiceMethods();

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            //UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);



            return base.FinishedLaunching(app, options);
        }

        private async void SetServiceMethods()
        {
            await locationService.Start();
            MessagingCenter.Subscribe<StartServiceMessage>(this, "ServiceStarted", async message => {
                if (!locationService.isStarted)
                    await locationService.Start();
            });

            MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped", message => {
                if (locationService.isStarted)
                    locationService.Stop();
            });
        }
    }
}
