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

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method

			// Code to start the Xamarin Test Cloud Agent
			const string APPLICATION_ID = "G7S25vITx0tfeOhODauYKwtauCvzityLwJFGYHPw";
			const string DOT_NET_ID = "ypPxS2V2rTGl1lNbvEVKUEACKF8PRhWxkWQsbkFe";

			ParseClient.Initialize(APPLICATION_ID, DOT_NET_ID);

			////Register for remote notifications.
			//if (Convert.ToInt16(UIDevice.CurrentDevice.SystemVersion.Split('.')[0]) < 8)
			//{
			//	UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
			//	UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
			//}
			//else 
			//{
				UIUserNotificationType notificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
				var settings = UIUserNotificationSettings.GetSettingsForTypes(notificationTypes, null);
				UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications();
			//}

			//Handle Parse Push notifications.

			ParsePush.ParsePushNotificationReceived += (sender, e) =>
			{
				Debug.WriteLine("You have received a notification");



			};

			Window = new UIWindow(UIScreen.MainScreen.Bounds);
			UIViewController loginController = new LoginViewController();
			UINavigationController navigationController = new UINavigationController(loginController);
			Window.RootViewController = navigationController;
			Window.MakeKeyAndVisible();

			return true;
		}

		//public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
		//{
			
		//	//application.RegisterForRemoteNotifications();
		//}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			
			ParseInstallation installation = ParseInstallation.CurrentInstallation;
			installation.SetDeviceTokenFromData(deviceToken);
			installation.Channels = new string[] { "Global" }; 
			installation.SaveAsync();
		}

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			ParsePush.HandlePush(userInfo);
		}



		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}
	}
}


