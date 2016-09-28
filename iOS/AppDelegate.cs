using System;
using System.Collections.Generic;
using System.Diagnostics;
using Foundation;
using Parse;
using UIKit;

namespace HookMeUP.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations

		public override UIWindow Window
		{
			get;
			set;
		}
		ParseInstallation CurrentInstallation
		{
			get;
			set;
		}

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method

			// Code to start the Xamarin Test Cloud Agent
			const string APPLICATION_ID = "G7S25vITx0tfeOhODauYKwtauCvzityLwJFGYHPw";
			const string DOT_NET_ID = "ypPxS2V2rTGl1lNbvEVKUEACKF8PRhWxkWQsbkFe";

			ParseClient.Initialize(APPLICATION_ID, DOT_NET_ID);

			////Register for remote notifications.
			if (Convert.ToInt16(UIDevice.CurrentDevice.SystemVersion.Split('.')[0]) < 8)
			{
				UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
				UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
			}
			else
			{
				UIUserNotificationType notificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
				var settings = UIUserNotificationSettings.GetSettingsForTypes(notificationTypes, null);
				UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications();
			}

			//Handle Parse Push notifications.

			ParsePush.ParsePushNotificationReceived += (sender, e) =>
			{
				IDictionary<string, object> payload = e.Payload;
				object aps;

				if (payload.TryGetValue("aps", out aps))
				{
					NSDictionary dictionary = aps as NSDictionary;

					foreach (var elements in dictionary) 
					{
						string value = elements.Value.ToString();

						if (value.Contains("New"))
						{
							//App Admin side handles notification
							string[] splitArr = value.Split(' ');
							string channelName = splitArr[3] + splitArr[4];
							((Window.RootViewController as UINavigationController).TopViewController as AdminViewController).AddOrders();
							//var adminOrders = new AdminViewController(channelName);
							//adminOrders.AddNewOrders();
							break;
						}
						if (value.Contains("Ready!!!")) 
						{
							Debug.WriteLine("Order ready for user");
							//App user side handles notification 
						}
						
					}

				}

			};

			Window = new UIWindow(UIScreen.MainScreen.Bounds);
			UIViewController loginController = new LoginViewController();
			UINavigationController navigationController = new UINavigationController(loginController);
			Window.RootViewController = navigationController;
			Window.MakeKeyAndVisible();

			return true;
		}

		public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
		{
			application.RegisterForRemoteNotifications();
		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{

			ParseInstallation installation = ParseInstallation.CurrentInstallation;
			CurrentInstallation = installation;
			CurrentInstallation.SetDeviceTokenFromData(deviceToken);
			if (CurrentInstallation.Badge != 0)
				CurrentInstallation.Badge = 0;

			CurrentInstallation.SaveAsync();
		}

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			ParsePush.HandlePush(userInfo);
		}

		public override void OnActivated(UIApplication application)
		{

			if (CurrentInstallation != null)
			{
				if (CurrentInstallation.Badge != 0)
				{
					CurrentInstallation.Badge = 0;
					CurrentInstallation.SaveAsync();
				}
			}

		}

		public override void DidEnterBackground(UIApplication application)
		{
			if (CurrentInstallation.Badge != 0)
			{
				CurrentInstallation.Badge = 0;
				CurrentInstallation.SaveAsync();
			}
		}

		public override void WillTerminate(UIApplication application)
		{
			try
			{
				ParseUser.LogOut();
			}
			catch (ParseException e)
			{
				Debug.WriteLine(e.StackTrace);
			}

		}

	}
}


