using System;
using Foundation;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class AdminViewController : UIViewController
	{



		public AdminViewController() : base("AdminViewController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
	


		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}


	}

	class TableSourceAdmin : UITableViewSource
	{
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			throw new NotImplementedException();
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			throw new NotImplementedException();
		}
	}


}


