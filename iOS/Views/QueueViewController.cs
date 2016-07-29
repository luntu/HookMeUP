using System;

using UIKit;

namespace HookMeUP.iOS
{
	public partial class QueueViewController : ScreenViewController
	{


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			backOrdersButton.TouchUpInside += (o, e) =>
			{
				NavigationController.PopViewController(true);
			};
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


