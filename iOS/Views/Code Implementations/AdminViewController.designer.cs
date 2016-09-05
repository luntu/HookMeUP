// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace HookMeUP.iOS
{
	[Register ("AdminViewController")]
	partial class AdminViewController
	{
		[Outlet]
		UIKit.UITableView AminOrdersTable { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AminOrdersTable != null) {
				AminOrdersTable.Dispose ();
				AminOrdersTable = null;
			}
		}
	}
}
