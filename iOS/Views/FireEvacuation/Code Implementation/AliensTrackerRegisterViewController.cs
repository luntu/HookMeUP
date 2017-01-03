using UIKit;
using Parse;

namespace HookMeUP.iOS
{
	public partial class AliensTrackerRegisterViewController : UIViewController
	{
		public AliensTrackerRegisterViewController() : base("AliensTrackerViewController", null)
		{
		}

		public override void ViewDidLoad()
		{
			//string fullName = nameFireText.Text;
			//string contactNo = contactNoFireText.Text;

			//ParseObject pObj = new ParseObject("Guests");
			//pObj["Name"] = fullName;
			//pObj["ContactNo"] = contactNo;


		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


