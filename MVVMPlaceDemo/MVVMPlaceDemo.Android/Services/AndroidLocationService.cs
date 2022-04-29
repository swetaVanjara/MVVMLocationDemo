﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using MVVMPlaceDemo.Droid.Helpers;
using MVVMPlaceDemo.Models;
using MVVMPlaceDemo.Services;
using Xamarin.Forms;
using OperationCanceledException = Android.OS.OperationCanceledException;

namespace MVVMPlaceDemo.Droid.Services
{
    [Service]
    public class AndroidLocationService : Service
    {
        CancellationTokenSource _cts;
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;

        //public AndroidLocationService()
        //{
        //}

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			_cts = new CancellationTokenSource();

			Notification notif = DependencyService.Get<INotification>().ReturnNotif();
			StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notif);

			Task.Run(() => {
				try
				{
					var locShared = new Location();
					locShared.Run(_cts.Token).Wait();
				}
				catch (OperationCanceledException)
				{
				}
				finally
				{
					if (_cts.IsCancellationRequested)
					{
						var message = new StopServiceMessage();
						Device.BeginInvokeOnMainThread(
							() => MessagingCenter.Send(message, "ServiceStopped")
						);
					}
				}
			}, _cts.Token);

			return StartCommandResult.Sticky;
		}
	}
}
