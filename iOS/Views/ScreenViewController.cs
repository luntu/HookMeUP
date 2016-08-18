using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using Parse;
using UIKit;



namespace HookMeUP.iOS
{
	public partial class ScreenViewController : UIViewController
	{

		public static AdminViewController adminViewController = new AdminViewController();
		public static QueueViewController queueViewController = new QueueViewController();
		public static RegisterViewController registerViewController = new RegisterViewController();
		public static LoginViewController loginViewController = new LoginViewController();
		public static ForgotPasswordViewController forgotPasswordViewController = new ForgotPasswordViewController();
		public static OrderViewController orderViewController = new OrderViewController();
		public ParseObject tableNameUserInfo = new ParseObject("UserInformation");
		public ParseObject tableNameOrders = new ParseObject("Orders");
		public LoadingOverlay loadingOverlay;
		public CGRect bounds = UIScreen.MainScreen.Bounds;



		public void AlertPopUp(string title, string message, params string[] buttonText)
		{

			if (!title.Equals("") && !message.Equals("") && buttonText != null)
			{

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
			if (button != null)
			{

				foreach (UIButton element in button)
				{
					element.Layer.BorderWidth = 1;
					element.Layer.CornerRadius = 3;
					element.Layer.BorderColor = UIColor.Cyan.CGColor;
				}

			}


		}

		public void NavigationScreenController(UIViewController screen)
		{
			NavigationController.PushViewController(screen, true);

		}

		public void ClearFields(params UITextField[] texts)
		{

			if (texts != null)
			{
				foreach (UITextField parameters in texts)
				{
					parameters.Text = string.Empty;
				}
			}
		}

		public void TextFieldKeyboardIteration(params UITextField[] fields) {

			//set Tags
			for (int i = 0; i < fields.Length; i++) {
				fields[i].Tag = i;
			}

			for (int i = 0; i < fields.Length - 1; i++)
			{

				fields[i].ShouldReturn += (textField) =>
				{

					int nextTag = (int)textField.Tag + 1;
					fields[nextTag].BecomeFirstResponder();

					return true;
				};

			}

		}

		//==============================================================================================================

		#region Keyboard adjust


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

	
		protected virtual UIView KeyboardGetActiveView()
		{
			return View.FindFirstResponder();
		}

		private void OnKeyboardNotification(NSNotification notification)
		{
			if (!IsViewLoaded) return;

			//Check if the keyboard is becoming visible
			bool visible = notification.Name == UIKeyboard.WillShowNotification; // is this the keyboard that pops up boolean type

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
			var spaceAboveKeyboard = (landscape ? scrollView.Frame.Width : scrollView.Frame.Height) - 75;

			// Move the active field to the center of the available space
			var offset = relativeFrame.Y - (spaceAboveKeyboard - viewToCenter.Frame.Height) / 2;
			scrollView.ContentOffset = new PointF(0, (float)offset);
		}

		protected virtual void RestoreScrollPosition(UIScrollView scrollView)
		{
			scrollView.ContentInset = UIEdgeInsets.Zero;
			scrollView.ScrollIndicatorInsets = UIEdgeInsets.Zero;
		}

		protected void DismissKeyboardOnBackgroundTap()
		{
			// Add gesture recognizer to hide keyboard
			var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
			tap.AddTarget(() => View.EndEditing(true));
			View.AddGestureRecognizer(tap);
		}

		public bool ShouldReturn(params UITextField[] textField)
		{

			for (int i = 0; i < textField.Length; i++)
			{
				if (textField[i].Tag == 1)
					textField[i + 1].BecomeFirstResponder();
				else
					textField[i].ResignFirstResponder();
			}
			return true;
		}
		#endregion
	}

	//==================================================================================================================
	//keyboard notification => active view detection
	public static class ViewExtensions
	{

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


	//==================================================================================================================
	//loading status bar
	public class LoadingOverlay : UIView
	{
		// control declarations
		UIActivityIndicatorView activitySpinner;

		public LoadingOverlay(CGRect frame) : base(frame)
		{
			// configurable bits

			Alpha = 0.75f;
			BackgroundColor = UIColor.White;

			// derive the center x and y
			nfloat centerX = Frame.Width / 2;
			nfloat centerY = Frame.Height / 2;

			// create the activity spinner, center it horizontall and put it 5 points above center x
			activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
			activitySpinner.Frame = new CGRect(
				centerX - (activitySpinner.Frame.Width / 2),
				centerY - activitySpinner.Frame.Height - 20,
				activitySpinner.Frame.Width,
				activitySpinner.Frame.Height);
			AddSubview(activitySpinner);
			activitySpinner.StartAnimating();
			activitySpinner.Color = UIColor.Black;


		}

		/// <summary>
		/// Fades out the control and then removes it from the super view
		/// </summary>
		public void Hide()
		{
			UIView.Animate(
				0.5, // duration
				() => { Alpha = 0; },
				() => { RemoveFromSuperview(); }
			);
		}
	}

}