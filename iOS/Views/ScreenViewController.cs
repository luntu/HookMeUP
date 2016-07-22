using System;
using System.Drawing;
using Foundation;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class ScreenViewController : UIViewController
	{
		
		public static RegisterViewController registerViewController = new RegisterViewController();
		public static LoginViewController loginViewController = new LoginViewController();
		public static ForgotPasswordViewController forgotPasswordViewController = new ForgotPasswordViewController();
		public static OrderViewController orderViewController = new OrderViewController();

		public void AlertPopUp(string title, string message, params string[] buttonText)
		{

			if (!title.Equals("") && !message.Equals("") && buttonText != null) { 

				UIAlertView alert = new UIAlertView();
				alert.Title = title;
				alert.Message = message;

				foreach (string elements in buttonText)
				{
					alert.AddButton(elements);
				}

				alert.Show();
			}



		}

		public void BorderButton(params UIButton[] button)
		{
			if(button != null){ 

				foreach (UIButton element in button)
				{
					element.Layer.BorderWidth = 1;
					element.Layer.CornerRadius = 4;
					element.Layer.BorderColor = UIColor.Cyan.CGColor;

				}
			
			}


		}

		public void NavigationScreenController(UIViewController screen)
		{
			NavigationController.PushViewController(screen, true);

		}

		public void ClearFields(params UITextField[] texts) {

			if (texts != null)
			{
				foreach (UITextField parameters in texts)
				{
					parameters.Text = string.Empty;
				}
			}
		}

		//==============================================================================================================

		#region Keyboard adjust

		/// <summary>
		/// Set this field to any view inside the scroll view to center this view instead of the current responder
		/// </summary>
		protected UIView ViewToCenterOnKeyboardShown;
		public bool HandlesKeyboardNotifications()
		{
			return true;
		}

		protected void RegisterForKeyboardNotifications()
		{
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
		}

		/// <summary>
		/// Gets the UIView that represents the "active" user input control (e.g. textfield, or button under a text field)
		/// </summary>
		/// <returns>
		/// A <see cref="UIView"/>
		/// </returns>
		protected virtual UIView KeyboardGetActiveView()
		{
			return View.FindFirstResponder();
		}

		private void OnKeyboardNotification(NSNotification notification)
		{
			if (!IsViewLoaded) return;

			//Check if the keyboard is becoming visible
			var visible = notification.Name == UIKeyboard.WillShowNotification;

			//Start an animation, using values from the keyboard
			UIView.BeginAnimations("AnimateForKeyboard");
			UIView.SetAnimationBeginsFromCurrentState(true);
			UIView.SetAnimationDuration(UIKeyboard.AnimationDurationFromNotification(notification));
			UIView.SetAnimationCurve((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification(notification));

			//Pass the notification, calculating keyboard height, etc.
			bool landscape = InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || InterfaceOrientation == UIInterfaceOrientation.LandscapeRight;
			var keyboardFrame = visible
									? UIKeyboard.FrameEndFromNotification(notification)
									: UIKeyboard.FrameBeginFromNotification(notification);

			OnKeyboardChanged(visible, (float)(landscape ? keyboardFrame.Width : keyboardFrame.Height));

			//Commit the animation
			UIView.CommitAnimations();
		}

		/// <summary>
		/// Override this method to apply custom logic when the keyboard is shown/hidden
		/// </summary>
		/// <param name='visible'>
		/// If the keyboard is visible
		/// </param>
		/// <param name='keyboardHeight'>
		/// Calculated height of the keyboard (width not generally needed here)
		/// </param>
		protected virtual void OnKeyboardChanged(bool visible, float keyboardHeight)
		{
			var activeView = ViewToCenterOnKeyboardShown ?? KeyboardGetActiveView();
			if (activeView == null)
				return;

			var scrollView = activeView.FindSuperviewOfType(View, typeof(UIScrollView)) as UIScrollView;
			if (scrollView == null)
				return;

			if (!visible)
				RestoreScrollPosition(scrollView);
			else
				CenterViewInScroll(activeView, scrollView, keyboardHeight);
		}

		protected virtual void CenterViewInScroll(UIView viewToCenter, UIScrollView scrollView, float keyboardHeight)
		{
			var contentInsets = new UIEdgeInsets(0.0f, 0.0f, keyboardHeight, 0.0f);
			scrollView.ContentInset = contentInsets;
			scrollView.ScrollIndicatorInsets = contentInsets;

			// Position of the active field relative isnside the scroll view
			RectangleF relativeFrame = (RectangleF)viewToCenter.Superview.ConvertRectToView(viewToCenter.Frame, scrollView);

			bool landscape = InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || InterfaceOrientation == UIInterfaceOrientation.LandscapeRight;
			var spaceAboveKeyboard = (landscape ? scrollView.Frame.Width : scrollView.Frame.Height) - keyboardHeight;

			// Move the active field to the center of the available space
			var offset = relativeFrame.Y - (spaceAboveKeyboard - viewToCenter.Frame.Height) / 2;
			scrollView.ContentOffset = new PointF(0, (float)offset);
		}

		protected virtual void RestoreScrollPosition(UIScrollView scrollView)
		{
			scrollView.ContentInset = UIEdgeInsets.Zero;
			scrollView.ScrollIndicatorInsets = UIEdgeInsets.Zero;
		}

		/// <summary>
		/// Call it to force dismiss keyboard when background is tapped
		/// </summary>
		protected void DismissKeyboardOnBackgroundTap()
		{
			// Add gesture recognizer to hide keyboard
			var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
			tap.AddTarget(() => View.EndEditing(true));
			View.AddGestureRecognizer(tap);
		}

		#endregion
	}

	public static class ViewExtensions
	{
		/// <summary>
		/// Find the first responder in the <paramref name="view"/>'s subview hierarchy
		/// </summary>
		/// <param name="view">
		/// A <see cref="UIView"/>
		/// </param>
		/// <returns>
		/// A <see cref="UIView"/> that is the first responder or null if there is no first responder
		/// </returns>
		public static UIView FindFirstResponder(this UIView view)
		{
			if (view.IsFirstResponder)
			{
				return view;
			}
			foreach (UIView subView in view.Subviews)
			{
				var firstResponder = subView.FindFirstResponder();
				if (firstResponder != null)
					return firstResponder;
			}
			return null;
		}

		/// <summary>
		/// Find the first Superview of the specified type (or descendant of)
		/// </summary>
		/// <param name="view">
		/// A <see cref="UIView"/>
		/// </param>
		/// <param name="stopAt">
		/// A <see cref="UIView"/> that indicates where to stop looking up the superview hierarchy
		/// </param>
		/// <param name="type">
		/// A <see cref="Type"/> to look for, this should be a UIView or descendant type
		/// </param>
		/// <returns>
		/// A <see cref="UIView"/> if it is found, otherwise null
		/// </returns>
		public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			if (view.Superview != null)
			{
				if (type.IsAssignableFrom(view.Superview.GetType()))
				{
					return view.Superview;
				}

				if (view.Superview != stopAt)
					return view.Superview.FindSuperviewOfType(stopAt, type);
			}

			return null;
		}
	}

}

