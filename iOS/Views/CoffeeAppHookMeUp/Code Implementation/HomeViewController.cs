using System;

using UIKit;

namespace HookMeUP.iOS
{
	public partial class HomeViewController : UIViewController
	{
		public HomeViewController() : base("HomeViewController", null)
		{
		}

		public override void ViewDidLoad()
		{
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


