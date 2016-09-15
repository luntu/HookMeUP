using System;
using Parse;


//namespace HookMeUP.iOS
//{
//	public partial class ForgotPasswordViewController : ScreenViewController
//	{

//		//MFMailComposeViewController mailcontroller;
//		public override void ViewDidLoad()
//		{
//			base.ViewDidLoad();
//			// Perform any additional setup after loading the view, typically from a nib.

//			//usernameTextForgot.BecomeFirstResponder();
//			//DismissKeyboardOnBackgroundTap();
//			//RegisterForKeyboardNotifications();
//			//ShouldReturn(usernameTextForgot, emailTextForgot);
//			//TextFieldKeyboardIteration(usernameTextForgot, emailTextForgot);

//			//int i = 0;

//			//getPasswordButton.TouchUpInside += (obj, evt) => 
//			//{
				
//				//i++;

//				//switch (!usernameTextForgot.Text.Equals("") && !emailTextForgot.Text.Equals("")) {

//				//	case true:

//						//try
//						//{
//							//loadingOverlay = new LoadingOverlay(bounds);
//							//View.Add(loadingOverlay);

//							//ParseQuery<ParseObject> query = from userInfo in ParseObject.GetQuery("UserInformation")
//							//								where userInfo.Get<string>("Username") == TrimInput(usernameTextForgot.Text)
//							//								&& userInfo.Get<string>("Email") == TrimInput(emailTextForgot.Text)
//							//								select userInfo;

//							//ParseObject objQ = await query.FirstAsync();

//							//string name = objQ.Get<string>("Name");
//							//string email = objQ.Get<string>("Email");
//							//string password = objQ.Get<string>("Password");

//							//string emailSender = "luntu@cowboyaliens.com";
//							//string passwordSender = "********************";

//							//MailMessage mail = new MailMessage();
//							//SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
//							//mail.From = new MailAddress(emailSender);
//							//mail.To.Add(email);
//							//mail.Subject = "noreply";
//							//mail.Body = "Hi\b " + name + "\n\nWe found your lost password\nPassword: " + password + "\n\nRegards\n\nHookMeUP Team";
//							//smtpServer.Port = 587;
//							//smtpServer.Credentials = new NetworkCredential(emailSender, passwordSender);
//							//smtpServer.EnableSsl = true;
//							//ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
//							//{
//							//	return true;
//							//};
//							//smtpServer.Send(mail);
//							//loadingOverlay.Hide();
//							//ToastIOS.Toast.MakeText("Mail sent!").Show();
//							//NetworkCredential netC = new NetworkCredential(usernameSender,passwordSender);
//							//MailMessage mailM = new MailMessage();
//							//mailM.To.Add(email);
//							//mailM.Subject = "noreply";
//							//mailM.From = new MailAddress(usernameSender);

//							//loadingOverlay.Hide();

//							//if (MFMailComposeViewController.CanSendMail)
//							//{
//							//	mailcontroller = new MFMailComposeViewController();
//							//	//mailcontroller.SetToRecipients(new string[] { email });
//							//	//mailcontroller.SetSubject("no reply");
//							//	//mailcontroller.SetMessageBody("Hi\b " + name + "\n\nWe found your lost password\nPassword: " + password + "\n\nRegards\n\nHookMeUP Team", false);

//							//	//mailcontroller.Finished += (sender, e) =>
//							//	//{
//							//	//	Console.WriteLine(e.Result);
//							//	//	e.Controller.DismissViewController(true, null);
//							//	//};
//							//	PresentViewController(mailcontroller, true, null);
//							//	//AlertPopUp("Done!", "We,ve sent an email to " + email + "\nIt will take a while", "OK");

//							//}
//							//else System.Diagnostics.Debug.WriteLine("Mail can't be sent");

//			//			}
//			//			catch (ParseException)
//			//			{
//			//				loadingOverlay.Hide();
//			//				AlertPopUp("Error", "Username and email do not match", "OK");
//			//				if (i == 3)
//			//				{
//			//					AlertPopUp("Error", "You failed to retrive password 3 times \n we suggest you register as a new user ", "OK");
//			//					ClearFields(emailTextForgot, usernameTextForgot);
//			//					NavigationController.PopViewController(true);
//			//				}
//			//			}
//			//			catch (Exception e) 
//			//			{
//			//				Console.WriteLine(e.StackTrace);
//			//			}
//			//			break;

//			//		case false:
						
//			//			AlertPopUp("Error","Please fill in details","Ok");

//			//			break;
						
//				//}
			
//			//};

//			backButtonForgot.TouchUpInside += (o, e) =>
//			{
//				NavigationController.PopViewController(true);
//				ClearFields(usernameTextForgot,emailTextForgot);
//			};

//		}


//		public override void DidReceiveMemoryWarning()
//		{
//			base.DidReceiveMemoryWarning();
//			// Release any cached data, images, etc that aren't in use.
//		}
//	}
//}


