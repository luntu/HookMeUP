using System.Diagnostics;
using Parse;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class UnpaidViewController : ScreenViewController
	{

		string Tester 
		{
			get;
			set;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			backButton.TouchUpInside += (sender, e) => 
			{
				NavigationController.PopViewController(true);
			};
		}
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			ShowUnpaid();
		}

		async void ShowUnpaid()
		{
			LoadingOverlay lo = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			View.Add(lo);
			try
			{
				var query = ParseObject.GetQuery("Unpaid");
				var results = await query.FindAsync();

				var coll = results;

			} 
			catch (ParseException ex)
			{
				Debug.WriteLine(ex.Message);
			}
			lo.Hide();

		}
}
}

