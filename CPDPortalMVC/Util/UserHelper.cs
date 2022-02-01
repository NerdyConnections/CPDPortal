using CPDPortalMVC.DAL;
using CPDPortalMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;
using System.Globalization;
using CPDPortalMVC.ViewModels;
using System.Text;

namespace CPDPortalMVC.Util
{
    public static class UserHelper
    {



        #region Andrew'sCode
        private static void LoadDataIntoSession()
        {
            // PhysicianController usrControler = new PhysicianController();
            UserRepository UserRepo = new UserRepository();

            Models.UserModel user = UserRepo.GetUserDetails(System.Web.HttpContext.Current.User.Identity.Name);

            if (user != null)
            {
                HttpContext.Current.Session[Constants.USER] = user;
            }

        }
        public static UserModel GetLoggedInUser()
        {
            if (HttpContext.Current.Session[Constants.USER] == null)

                LoadDataIntoSession();

            return HttpContext.Current.Session[Constants.USER] as UserModel;

        }
        public static string GetRoleByUserID(int UserID)
        {
            UserRepository UserRepo = new UserRepository();
            return UserRepo.GetRoleByUserID(UserID);

        }
        public static string GetCPDLogo(HttpRequestBase objHttpRequest)
        {
            string currentURL = objHttpRequest.Url.Host;

            string[] ParsedHost;
            if (!String.IsNullOrEmpty(currentURL))
            {
                ParsedHost = currentURL.Split('.');
                string imageFile;
                if (ParsedHost.Length < 3)
                {
                    //by default use amgen logo, say in a dev environ where host is localhost and no subdomain avaialble
                    return Util.Constants.LogoImagePath + "/amgen.png";
                }
                else
                {
                    imageFile = ParsedHost[0].ToLower();
                    return Util.Constants.LogoImagePath + "/" + imageFile + ".png";

                }
            }
            else
            {
                return Util.Constants.LogoImagePath + "/amgen.png";

            }

        }

        //pass in a comma delimited string of roles and determine if current user is in any one of them
        public static bool IsInRole(string roles)
        {

            String[] ArRoles = roles.Split(',');
            var user = HttpContext.Current.User;
            foreach (string role in ArRoles)
            {
                if (user.IsInRole(role))
                    return true;
            }

            return false;

        }
        public static void SetLoggedInUser(UserModel user, System.Web.SessionState.HttpSessionState session)
        {

            session[Constants.USER] = user;

        }

        public static bool SendEmailToAdmin(string emailTo, string emailBody, string emailSubject)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(emailTo));
                //testing  mailMessage.To.Add(new System.Net.Mail.MailAddress("amanullaha@chrc.net"));
                mailMessage.Subject = emailSubject;

                mailMessage.IsBodyHtml = true;
                //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(GetRegistrationEmailBody(string.Empty, string.Empty, string.Empty, string.Empty), null, "text/html");
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(emailBody, null, "text/html");


                //LinkedResource imagelink = new LinkedResource(Server.MapPath("~/images/regEmailImage.jpg"), "image/jpg");

                //imagelink.ContentId = "imageId";

                //imagelink.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

                //htmlView.LinkedResources.Add(imagelink);

                mailMessage.AlternateViews.Add(htmlView);
                //  mailMessage.Attachments.Add(new Attachment(Server.MapPath("~/pdf/CHOLESTABETES Needs Assessment.pdf")));

                SendMail(mailMessage);
                return true;


            }

            catch (Exception e)
            {
                // Response.Write("fail in sendEmailNotification+++++" + e.Message.ToString());

                return false;
            }
        }
        public static HttpCookie GetAuthorizationCookie(string userName, string userData)
        {
            HttpCookie httpCookie = FormsAuthentication.GetAuthCookie(userName, true);
            FormsAuthenticationTicket currentTicket = FormsAuthentication.Decrypt(httpCookie.Value);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(currentTicket.Version, currentTicket.Name, currentTicket.IssueDate, currentTicket.Expiration, currentTicket.IsPersistent, userData);
            httpCookie.Value = FormsAuthentication.Encrypt(ticket);
            return httpCookie;
        }

        public static string GetProvinceFullName(string ProvinceCode)
        {
            string ProvinceFullName = string.Empty;
            switch (ProvinceCode.ToUpper())
            {
                case "AB":
                    ProvinceFullName = "Alberta";
                    break;
                case "BC":
                    ProvinceFullName = "British Columbia";
                    break;
                case "MB":
                    ProvinceFullName = "Manitoba";
                    break;
                case "NB":
                    ProvinceFullName = "New Brunswick";
                    break;
                case "NL":
                    ProvinceFullName = "Newfoundland";
                    break;
                case "NS":
                    ProvinceFullName = "Nova Scotia";
                    break;
                case "NT":
                    ProvinceFullName = "Northwest Territories";
                    break;
                case "NU":
                    ProvinceFullName = "Nunavut";
                    break;
                case "ON":
                    ProvinceFullName = "Ontario";
                    break;
                case "PEI":
                    ProvinceFullName = "Prince Edward Island";
                    break;
                case "QC":
                    ProvinceFullName = "Quebec";
                    break;
                case "SK":
                    ProvinceFullName = "Saskatchewan";
                    break;
                case "YT":
                    ProvinceFullName = "Yukon";
                    break;
            }
            return ProvinceFullName;
        }


        public static void SendMail(MailMessage Message)
        {
            SmtpClient client = new SmtpClient();
            try
            {

                client.Host = ConfigurationManager.AppSettings["smtpServer"];

                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                // NetworkCred.UserName = "webmaster@questionaf.ca";
                //NetworkCred.Password = "xkc232v";
                NetworkCred.UserName = ConfigurationManager.AppSettings["smtpUser"];
                NetworkCred.Password = ConfigurationManager.AppSettings["smtpPassword"];
                client.UseDefaultCredentials = false;
                client.Credentials = NetworkCred;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl= Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]); 
                // client.Port = 25;
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
                //client.EnableSsl = true;//DON"T set to true doesn't work in webserver01, it doesn't support ssl, no email will be sent.
                client.Timeout = 20000;
                client.Send(Message);

            }
            catch (Exception e)
            {
                client = null;
                String Error = e.Message.ToString();
                //Utility.WriteToLog("SendMail Error: " + Error);
            }

        }


        public static IEnumerable<SelectListItem> GetProvinces()
        {
            List<SelectListItem> provinces = new List<SelectListItem>
            {

                      new SelectListItem {Text = "AB", Value = "AB"},
                      new SelectListItem {Text = "BC", Value = "BC"},
                      new SelectListItem {Text = "MB", Value = "MB"},
                      new SelectListItem {Text = "NS", Value = "NS"},
                      new SelectListItem {Text = "NB", Value = "NB"},
                      new SelectListItem {Text = "NL", Value = "NL"},
                      new SelectListItem {Text = "ON", Value = "ON"},
                      new SelectListItem {Text = "PE", Value = "PE"},
                      new SelectListItem {Text = "QC", Value = "QC"},
                      new SelectListItem {Text = "SK", Value = "SK"},


            };
            return provinces;

        }
        public static void EmailFromSaleRepToAdmin_Cancellation(String FromEmailAddress, ProgramRequestCancellationVM pcvm)
        {
            try
            {


                string html = string.Empty;
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                //	mailMessage.From = new System.Net.Mail.MailAddress(FromEmailAddress);

                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["AdminEmail"]));
                mailMessage.Subject = "Session Cancellation Request";
                mailMessage.IsBodyHtml = true;
                html = @"<table width = '900' border = '0' cellspacing = '0' cellpadding = '10'>
				<tr>                
				<td align = 'left'><span style = 'font-size: 30px; font-family: Arial, Helvetica, sans-serif;'> Session Cancellation Request </span></td> 
									   
				 </tr>  
				  
				<tr>
				<td align='left'>
						Session ID: {Session ID}<br/>
						Session Contact:  {Session Contact}<br/>
						Session Date: {Session Date}<br/>
						Session Location: {Session Location}<br/>
						Session Speaker: {Session Speaker}<br/>
						Session Speaker 2 (if applicable) : {Session Speaker2}<br/>
						Session Moderator (if applicable) : {Session Moderator}<br/>
						Reason: {Reason}<br/>
				</td>

				</tr>  
					   
				

			</table> ";
                html = html.Replace("{Session ID}", pcvm.ProgramRequestID.ToString());

                html = html.Replace("{Session Contact}", pcvm.ContactName);
                html = html.Replace("{Session Date}", pcvm.ConfirmedSessionDate);
                html = html.Replace("{Session Location}", pcvm.LocationName);
                html = html.Replace("{Session Speaker}", pcvm.SpeakerName);
                html = html.Replace("{Session Moderator}", pcvm.ModeratorName);
                html = html.Replace("{Reason}", pcvm.CancellationReason);

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
        public static void EmailFromSaleRepToAdmin_Modify(String FromEmailAddress, ProgramRequestModifyVM pcvm)
        {
            try
            {


                string html = string.Empty;
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                //	mailMessage.From = new System.Net.Mail.MailAddress(FromEmailAddress);

                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["AdminEmail"]));
                mailMessage.Subject = "Session Modify Request";
                mailMessage.IsBodyHtml = true;
                html = @"<table width = '900' border = '0' cellspacing = '0' cellpadding = '10'>
				<tr>                
				<td align = 'left'><span style = 'font-size: 30px; font-family: Arial, Helvetica, sans-serif;'> Session Modify Request </span></td> 
									   
				 </tr>  
				  
				<tr>
				<td align='left'>
						Session ID: {Session ID}<br/>
						Session Contact:  {Session Contact}<br/>
						Session Date: {Session Date}<br/>
						Session Location: {Session Location}<br/>
						Session Speaker: {Session Speaker}<br/>
						Session Speaker 2 (if applicable) : {Session Speaker2}<br/>
						Session Moderator (if applicable) : {Session Moderator}<br/>
						Reason: {Reason}<br/>
				</td>

				</tr>  
					   
				

			</table> ";
                html = html.Replace("{Session ID}", pcvm.ProgramRequestID.ToString());

                html = html.Replace("{Session Contact}", pcvm.ContactName);
                html = html.Replace("{Session Date}", pcvm.ConfirmedSessionDate);
                html = html.Replace("{Session Location}", pcvm.LocationName);
                html = html.Replace("{Session Speaker}", pcvm.SpeakerName);
                html = html.Replace("{Session Moderator}", pcvm.ModeratorName);
                html = html.Replace("{Reason}", pcvm.ModifyReason);

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
        public static void EmailSaleRep_SpeakerApproved(UserModel UMSalesRep, UserModel UMSpeaker)
        {
            try
            {
                string html = string.Empty;

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(UMSalesRep.EmailAddress));
                mailMessage.Subject = "Speaker Request Approved";
                mailMessage.IsBodyHtml = true;


                html = @"<table width = '900' border = '0' cellspacing = '0' cellpadding = '10'>
				<tr>                
				<td align = 'left'><span style = 'font-size: 30px; font-family: Arial, Helvetica, sans-serif;'> Dear {SRFirstName} </span></td> 
									   
				 </tr>  
				<tr>                
				<td align = 'left'><span style ='font-color:red; font-size: 30px; font-family: Arial, Helvetica, sans-serif;'> Your speaker request has been approved and Dr. {SpeakerFirstName} {SpeakerLastName} has been forwarded an invitation to register through the speaker resource centre. Should the speaker decline the invitation, the “Status” in the column will be updated accordingly.<br/><br/>  </td> 
									   
				 </tr>   
				<tr>
				<td align='left'>
						Please do not hesitate to contact us should you have any questions or require any assistance.<br/><br/>
				</td>

				</tr>  
				<tr>
				<td align='left'>
						The CCPDHM Web Portal Team <br/><br/>
				</td>

				</tr>          
				<tr>
				<td align='left'>
					   E: <a mailto:'amgen@ccpdhm.com'>amgen@ccpdhm.com</a> <br/><br/>
				</td>

				</tr>   

			</table> ";
                html = html.Replace("{SRFirstName}", UMSalesRep.FirstName.ToString());

                html = html.Replace("{SpeakerFirstName}", UMSpeaker.FirstName);
                html = html.Replace("{SpeakerLastName}", UMSpeaker.LastName);

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }

        public static void EmailSpeaker_SpeakerApproved(UserModel UMSpeaker)
        {


            try
            {
                string html = string.Empty;

                string SpeakerActivation = System.Configuration.ConfigurationManager.AppSettings["SpeakerActivation"];
                string SpeakerModeratorOptout = System.Configuration.ConfigurationManager.AppSettings["SpeakerModeratorOptOut"];

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(UMSpeaker.EmailAddress));
                mailMessage.Subject = "Invitation to Participate as a Speaker: Activate Your Speaker Resource Account";
                mailMessage.IsBodyHtml = true;


                html = @"<table width = '100%' border = '0' cellspacing = '0' cellpadding = '10'>
												 <tr>                
												 <td align = 'left'><span style = 'font-size: 22px; font-family: Arial, Helvetica, sans-serif;'> Dear {SpeakerLastName} </span></td> 
																												 
												  </tr>  
												 <tr>                
												 <td align='left'><span style ='font-color:red; font-size: 14px; font-family: Arial, Helvetica, sans-serif;'> 
				 On behalf of the Canadian Heart Research Centre and the Canadian Centre for Professional Development in Health and Medicine, we are pleased to invite you to activate your personal speaker resource portal. <br/>
				 This portal has been developed to assist you in facilitating upcoming certified activities that have been developed by our physician organization with unrestricted educational grant support from Amgen Canada.  
				<br/><br/>  </td> 
																												 
												  </tr>   
												 <tr>
												 <td align='left'>
															  <b>Next Steps: If you ARE INTERESTED in being invited as a speaker and/or moderator (the program date(s) would be scheduled at a time and location that is convenient for you). </b><br/><br/>
									  • Please activate your account by following this link <a href='{SpeakerActivation}'> https://speaker.ccpdhm.com/activate </a> and entering your username: {Username} <br/>
									  • Once your account is activated, you will be able to log-in and complete the COI and Payee Forms as well as access the portal’s functionalities and program materials. <br/> <br/> <br/>
					 <b>Next Steps: If you are <span style='color:red;'>NOT INTERESTED </span> in being invited as a speaker and/or moderator </b>. <br/><br/>
									 • Please follow this link to opt-out <a href='{SpeakerModeratorOptOut}{userid}'> https://speaker.ccpdhm.com/optout </a> and entering your username: {Username} <br/> <br/> <br/>
					 Please do not hesitate to contact us should you have any questions or require any assistance. 

				</td>       
												 

												 </tr>  
												 <tr>
												 <td align='left'>
																		 The CCPDHM Web Portal Team <br/><br/>
												 </td>

												 </tr>          
												 <tr>
												 <td align='left'>
																E: <a mailto:'info@ccpdhm.com'>info@ccpdhm.com </a> <br/><br/>
												 </td>

												 </tr>   

									 </table> ";


                html = html.Replace("{SpeakerFirstName}", UMSpeaker.FirstName);
                html = html.Replace("{SpeakerLastName}", UMSpeaker.LastName);
                html = html.Replace("{Username}", UMSpeaker.EmailAddress);
                html = html.Replace("{SpeakerActivation}", SpeakerActivation);
                html = html.Replace("{SpeakerModeratorOptOut}", SpeakerModeratorOptout);
                html = html.Replace("{userid}", UMSpeaker.ID.ToString());




                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
        public static void EmailSpeaker_SpeakerApprovedfr(UserModel UMSpeaker)
        {


            try
            {
                string html = string.Empty;

                string SpeakerActivation = System.Configuration.ConfigurationManager.AppSettings["SpeakerActivation"] + "?lang=fr";
                string SpeakerModeratorOptout = System.Configuration.ConfigurationManager.AppSettings["SpeakerModeratorOptOut"] + "?userid={userid}" + "&lang=fr";

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(UMSpeaker.EmailAddress));
                mailMessage.Subject = "Invitation à participer comme Conférencier: Activer votre compte de Ressources du Conférencier";
                mailMessage.IsBodyHtml = true;


                html = @"<table width = '100%' border = '0' cellspacing = '0' cellpadding = '10'>
												 <tr>                
												 <td align = 'left'><span style = 'font-size: 22px; font-family: Arial, Helvetica, sans-serif;'> Cher Dr.  {SpeakerLastName} </span></td> 
																												 
												  </tr>  
												 <tr>                
												 <td align='left'><span style ='font-color:red; font-size: 14px; font-family: Arial, Helvetica, sans-serif;'> 
				Au nom du Centre canadien de recherche en cardiologie (CCRC) et du Centre International de Développement Professionnel en Santé et Médecine (CIDPSM), nous sommes heureux de vous inviter à activer votre site virtuel de Ressources du Conférencier personnel.<br/>

Ce site a été développé dans le but de vous aider dans la préparation de vos futures présentations d’activités accréditées qui ont été développées par notre  organisme de médecins qui est un groupe de professionnels de la santé à but non lucratif. Le présent programme a été rendu possible grâce à une subvention à visée éducative sans restriction provenant d’Amgen Canada.
  
				<br/><br/>  </td> 
																												 
												  </tr>   
												 <tr>
												 <td align='left'>
															  <b>Étapes suivantes: Si vous ÊTES INTÉRESSÉ de recevoir une invitation comme Conférencier et/ou Modérateur (les dates du programme pourraient être réservées à une heure et d’un endroit qui vous conviendrait): </b><br/><br/>
									  • Veuillez activer votre compte en suivant ce lien  <a href='{SpeakerActivation}'> https://speaker.ccpdhm.com/activate?lang=fr </a> et en entrant votre nom d’utilisateur: {Username} <br/>
									  • Lorsque votre compte sera activé, vous serez capable de vous joindre et de compléter les Formulaires de Conflit d’Intérêt et de Paiement Bancaire, en plus d’accéder aux fonctionnalités du site Internet et au contenu du matériel scientifique disponible. <br/> <br/> <br/>
					 <b>Étapes suivantes: Si vous <span style='color:red;'>N’ÊTES PAS INTÉRESSÉ</span> de recevoir une invitation comme Conférencier et/ou Modérateur</b>. <br/><br/>
									 • Veuillez suivre ce lien pour décliner  <a href='{SpeakerModeratorOptOut}'> https://speaker.ccpdhm.com/optout?lang=fr </a> et veuillez entrer votre nom d’utilisateur:  {Username} <br/> <br/> <br/>
					N’hésitez pas à nous contacter pour toute question ou si avez besoin d’aide.

				</td>       
												 

												 </tr>  
												 <tr>
												 <td align='left'>
																		L’Équipe du Site Internet CIDPSM <br/><br/>
												 </td>

												 </tr>          
												 <tr>
												 <td align='left'>
																E: <a mailto:'info@ccpdhm.com'>info@ccpdhm.com </a> <br/><br/>
												 </td>

												 </tr>   

									 </table> ";


                html = html.Replace("{SpeakerFirstName}", UMSpeaker.FirstName);
                html = html.Replace("{SpeakerLastName}", UMSpeaker.LastName);
                html = html.Replace("{Username}", UMSpeaker.EmailAddress);
                html = html.Replace("{SpeakerActivation}", SpeakerActivation);
                html = html.Replace("{SpeakerModeratorOptOut}", SpeakerModeratorOptout);
                html = html.Replace("{userid}", UMSpeaker.ID.ToString());




                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }

        public static ProgramRequestStatusCount GetProgramRequestStatusCounts(int ProgramID)
        {
            ProgramRequestStatusCount prsc = new ProgramRequestStatusCount();


            ProgramRepository pr = new ProgramRepository();
            prsc = pr.GetProgramRequestStatusCounts(ProgramID);

            return prsc;
        }
        public static void WriteToLog(string errorMessage)
        {


            using (StreamWriter w = File.AppendText(HttpContext.Current.Server.MapPath("~/log/log.txt")))
            {
                Log(errorMessage, w);

            }


        }
        private static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }

        #endregion

        #region Ali's Code =================================================================================================================================
        public static void SendEmailForgotPassword(string email, string password)
        {

            try
            {
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();

                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(email));
                mailMessage.Subject = Resources.Resource.FORGOT_PWD_HEADER;
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(GetEmailBodyPassword(email, password), null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {

                string msg = e.Message;


            }


        }
        private static string GetEmailBodyPassword(string userName, string password)
        {
            string html = string.Empty;


            html = Resources.Resource.FORGOTPASSWORD_TEXT;

            html = html.Replace("{userName}", userName);
            html = html.Replace("{password}", password);



            return html;

        }

        public static void SendRequestActivationEmail(UserViewModel model, Controller controller)
        {

            try
            {
                MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(model.EmailAddress));
                mailMessage.Subject = "Welcome to your personal CPD Portal: Activate Your Account";
                mailMessage.IsBodyHtml = true;
                string body = "Try to see why this email is not working";
                //string body = "<p>Dear {FirstName},</p>"
                //    + "<p>On behalf the Canadian Heart Research Centre (CHRC) and the Canadian Centre for Professional Development in Health and Medicine (CCPDHM), we would like to take this opportunity to welcome you to your personal Amgen CPD Portal. This portal has been developed to assist you in seamlessly planning and executing your regional CPD events in compliance with the 2018 National Standard.</p>"
                //    + "<p><b>As the first step, please activate your account by following this link</b> <a href='{ActivateLink}'>ACTIVATE MY ACCOUNT</a> and enter your username: {UserName}</p>"
                //    + "<p>Once your account is activated, you will be able to log-in and access the portal’s functionalities and initiate the planning process of your regional CPD events.</p>"
                //    + "<p>Please do not hesitate to contact us should you have any questions or require any assistance.</p>"
                //    + "<p>Lianne on behalf of the CHRC/CCPDHM Web Portal Team<br/>"
                //    + "E: <a href='mailto: amgen @ccpdhm.com'>amgen@ccpdhm.com</a></p>";

                Uri uri = controller.Request.Url;
                string activateLink = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port + "/Activate";
                body = body.Replace("{FirstName}", model.FirstName).Replace("{UserName}", model.EmailAddress).Replace("{ActivateLink}", activateLink);

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);


                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                UserHelper.WriteToLog("Error in SendRequestActivationEmail:" + e.Message);
                string msg = e.Message;
            }
        }


        public static void SendAdminProgramRequest(ProgramRequestIIModel pr, string speakername, string ModeratorName, string SessionCredit, string ProgramName, Controller controller)
        {

            try
            {

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["AdminEmail"]));
                mailMessage.Subject = ProgramName + "  Certification Request - ProgramDate " + pr.ProgramDate1;
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(GetEmailBodyAdmin(pr, speakername, ModeratorName, SessionCredit), null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);
                if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
                {
                    string FileName = GetFileNameForEmailAttachment(pr.SessionAgendaFileName, pr.ProgramRequestID);
                    mailMessage.Attachments.Add(new Attachment(controller.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserFileUploadPath"] + FileName)));
                }

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }

        public static void SendRegisteredEmail(ProgramRequest pr, UserModel um, string ProgramName, string ModeratorName, string SessionCredit, Controller controller)
        {
            try
            {
                string date1 = ConvertTimetoProperFormat(pr.ProgramDate1);

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(um.EmailAddress));
                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]));
                mailMessage.Subject = ProgramName + " - Invitation to Present - Program Date: " + date1 + " - PLEASE REVIEW AND RESPOND";
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(RegisteredEmailBody(pr, um, ProgramName, ModeratorName, SessionCredit), null, "text/html");


                mailMessage.AlternateViews.Add(htmlView);
                if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
                {
                    string FileName = GetFileNameForEmailAttachment(pr.SessionAgendaFileName, pr.ProgramRequestID);
                    mailMessage.Attachments.Add(new Attachment(controller.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserFileUploadPath"] + FileName)));
                }
                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
        public static void SendRegisteredEmailfr(ProgramRequest pr, UserModel um, string ProgramName, string ModeratorName,string SessionCreditfr,Controller controller)
        {
            try
            {
                string date1 = UserHelper.ConvertToFrenchDate(ConvertTimetoProperFormat(pr.ProgramDate1));

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(um.EmailAddress));
                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]));
                mailMessage.Subject = ProgramName + " - Invitation pour être Conférencier – Date du programme:   " + date1 + " - VEUILLEZ SVP EN PRENDRE CONNAISSANCE ET Y RÉPONDRE";
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(RegisteredEmailBodyfr(pr, um, ProgramName, ModeratorName, SessionCreditfr), null, "text/html");


                mailMessage.AlternateViews.Add(htmlView);
                if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
                {
                    string FileName = GetFileNameForEmailAttachment(pr.SessionAgendaFileName, pr.ProgramRequestID);
                    mailMessage.Attachments.Add(new Attachment(controller.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserFileUploadPath"] + FileName)));
                }
                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
        public static void SendRegistrationNotComplete(ProgramRequest pr, UserModel um, string ProgramName, string ModeratorName, string SessionCredit, Controller controller)
        {

            try
            {
                string date1 = ConvertTimetoProperFormat(pr.ProgramDate1);

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(um.EmailAddress));
                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]));
                mailMessage.Subject = ProgramName + " - Invitation to Present - Program Date: " + date1 + " - PLEASE REVIEW AND RESPOND";
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(RegistrationNotCompleteBody(pr, um, ProgramName, ModeratorName, SessionCredit), null, "text/html");


                mailMessage.AlternateViews.Add(htmlView);
                if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
                {
                    string FileName = GetFileNameForEmailAttachment(pr.SessionAgendaFileName, pr.ProgramRequestID);
                    mailMessage.Attachments.Add(new Attachment(controller.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserFileUploadPath"] + FileName)));
                }
                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
        public static void SendRegistrationNotCompletefr(ProgramRequest pr, UserModel um, string ProgramName, string ModeratorName, string SessionCredit, Controller controller)
        {

            try
            {
                string date1 = UserHelper.ConvertToFrenchDate(ConvertTimetoProperFormat(pr.ProgramDate1));

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(um.EmailAddress));
                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]));
                mailMessage.Subject = ProgramName + " - Invitation pour être Conférencier – Date du programme: " + date1 + " - VEUILLEZ SVP EN PRENDRE CONNAISSANCE ET Y RÉPONDRE";
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(RegistrationNotCompleteBodyfr(pr, um, ProgramName, ModeratorName, SessionCredit), null, "text/html");


                mailMessage.AlternateViews.Add(htmlView);
                if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
                {
                    string FileName = GetFileNameForEmailAttachment(pr.SessionAgendaFileName, pr.ProgramRequestID);
                    mailMessage.Attachments.Add(new Attachment(controller.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserFileUploadPath"] + FileName)));
                }
                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
        public static void SendNotRegistered(ProgramRequest pr, UserModel um, string ProgramName, string ModeratorName, string SessionCredit, Controller controller)
        {
            try
            {
                string date1 = ConvertTimetoProperFormat(pr.ProgramDate1);

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(um.EmailAddress));
                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]));
                mailMessage.Subject = ProgramName + " - Invitation to Present - Program Date: " + date1 + " - PLEASE REVIEW AND RESPOND";
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(NotRegisteredBody(pr, um, ProgramName, ModeratorName, SessionCredit), null, "text/html");

                mailMessage.AlternateViews.Add(htmlView);
                if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
                {
                    string FileName = GetFileNameForEmailAttachment(pr.SessionAgendaFileName, pr.ProgramRequestID);
                    mailMessage.Attachments.Add(new Attachment(controller.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserFileUploadPath"] + FileName)));
                }

                UserHelper.SendMail(mailMessage);
            }
            catch (Exception e)
            {
                string msg = e.Message;

            }
        }
        public static void SendNotRegisteredfr(ProgramRequest pr, UserModel um, string ProgramName, string ModeratorName, string SessionCredit, Controller controller)
        {
            try
            {
                string date1 = UserHelper.ConvertToFrenchDate(ConvertTimetoProperFormat(pr.ProgramDate1));

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(um.EmailAddress));
                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]));
                mailMessage.Subject = ProgramName + " - Invitation pour être Conférencier – Date du programme: " + date1 + " - VEUILLEZ SVP EN PRENDRE CONNAISSANCE ET Y RÉPONDRE";
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(NotRegisteredBodyfr(pr, um, ProgramName, ModeratorName, SessionCredit), null, "text/html");

                mailMessage.AlternateViews.Add(htmlView);
                if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
                {
                    string FileName = GetFileNameForEmailAttachment(pr.SessionAgendaFileName, pr.ProgramRequestID);
                    mailMessage.Attachments.Add(new Attachment(controller.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserFileUploadPath"] + FileName)));
                }

                UserHelper.SendMail(mailMessage);
            }
            catch (Exception e)
            {
                string msg = e.Message;

            }
        }
        private static string GetEmailBodyAdmin(ProgramRequestIIModel pr, string speakername, string ModeratorName, string SessionCredit)
        {
            string html = string.Empty;

            string Materials = "";

            if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
            {
                Materials = Path.GetFileName(pr.SessionAgendaFileName);
            }



            html = Resources.Resource.AdminEmailText;

            html = html.Replace("{RecordID}", pr.ProgramRequestID.ToString());
            html = html.Replace("{DateSubmitted}", DateTime.Now.ToString());
            html = html.Replace("{SessionContact}", pr.ContactName);
            html = html.Replace("{ContactPhoneNumber}", pr.ContactPhone);
            html = html.Replace("{ContactEmailAddress}", pr.ContactEmail);
            html = html.Replace("{SessionDate1}", ConvertTimetoProperFormat(pr.ProgramDate1));
            html = html.Replace("{SessionDate2}", ConvertTimetoProperFormat(pr.ProgramDate2));
            html = html.Replace("{SessionDate3}", ConvertTimetoProperFormat(pr.ProgramDate3));
            html = html.Replace("{MealStartTime}", pr.MealStartTime);
            html = html.Replace("{SessionStartTime}", pr.ProgramStartTime);
            html = html.Replace("{SessionEndTime}", pr.ProgramEndTime);
            html = html.Replace("{SessionCredits}", SessionCredit);
            html = html.Replace("{MultiSessionEvent}", pr.MultiSession.ToString());
            html = html.Replace("{CopyofAgenda}", Materials);
            html = html.Replace("{CostsbyParticipants}", pr.CostPerparticipants.ToString());
            html = html.Replace("{SessionSpeaker}", speakername);
            html = html.Replace("{SessionSpeakerStatus}", "");
            html = html.Replace("{SessionModerator}", ModeratorName);

            html = html.Replace("{SessionModeratorStatus}", "");
            html = html.Replace("{VenueContacted}", pr.VenueContacted);
            html = html.Replace("{LocationType}", pr.LocationType);


            html = html.Replace("{LocationName}", pr.LocationName);

            html = html.Replace("{Address}", pr.LocationAddress);

            html = html.Replace("{City}", pr.LocationCity);

            html = html.Replace("{Province}", pr.LocationProvince);

            html = html.Replace("{PhoneNumber}", pr.LocationPhoneNumber);

            html = html.Replace("{Website}", pr.LocationWebsite);

            html = html.Replace("{MealType}", pr.MealType);

            html = html.Replace("{CostPerPerson}", pr.CostPerPerson.ToString());

            html = html.Replace("{AudioVisual}", pr.AVEquipment);

            html = html.Replace("{AdditionalInfo}", pr.Comments);

            html = html.Replace("{ProgramCostsSplit}", pr.ProgramCostsSplit);

            html = html.Replace("{AmgenRepName}", pr.AmgenRepName);








            return html;

        }
        private static string RegisteredEmailBody(ProgramRequest pr, UserModel um, string ProgramName, string ModeratorName, string SessionCredits)
        {
            string html = string.Empty;


            string SpeakerConfirmUrl = System.Configuration.ConfigurationManager.AppSettings["SpeakerConfirmUrl"];

            string Materials = "";

            if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
            {
                Materials = Path.GetFileName(pr.SessionAgendaFileName);


            }

            string para1 = @"<table width = '900' border = '0' cellspacing = '0' cellpadding = '10'>
				
				 <tr>                         
				<td align = 'left'> 										
				<br /> 
		
				<p>Dear Dr. {LastName}, </p> 
				<p>You are invited to present the accredited CPD program entitled {ProgramName}. 
				Please review the details and confirm your availability by clicking the Confirm or Not Available button.</p> 
			
					 <table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							<a style='display: block;color: #ffffff;font-size: 10px;text-decoration: none;' 
							href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate1}'>
								Confirm
							</a>						
						</td>
													
						<td>														
								&nbsp;&nbsp;&nbsp;&nbsp;Program Date: {SessionDate1}<br /> 
								&nbsp;&nbsp;&nbsp;&nbsp;Meal Start Time: {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Session Start Time: {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Session End Time: {SessionEndTime}
						</td>
					</tr>
				</table>
				
											
				<table>
					<tr>
						<td style='background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							<a style='display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
									Not Available
								</a>
							</td>
 
							<td>
									&nbsp; &nbsp; &nbsp; &nbsp; Not Available
						</td>
					</tr>
				</table>";

            //if there is program date 2
            string para2 = @"<table>
					<tr>
						<td style = 'background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							 <a style = 'display: block;color: #ffffff;font-size: 10px;text-decoration: none;'
							href = '{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate2}' >
								Confirm
							</a>
						</td>
						<td>
								&nbsp; &nbsp; &nbsp; &nbsp; Program Date(2nd Option): {SessionDate2} <br />
									 &nbsp; &nbsp; &nbsp; &nbsp; Meal Start Time: {MealStartTime}
			&nbsp; &nbsp; &nbsp; &nbsp; Session Start Time: {SessionStartTime}
			&nbsp; &nbsp; &nbsp; &nbsp; Session End Time: {SessionEndTime}
						</td>
					</tr>
				</table>
				<table>
					<tr>
						<td style = 'background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							 <a style = 'display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href = '{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
										Not Available
									</a>   
								</td>
								<td>
										&nbsp; &nbsp; &nbsp; &nbsp; Not Available
						</td>
					</tr>
				</table>";

            //if there is program date 3
            string para3 = @"<table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
						<a style='display: block;color: #ffffff;font-size: 10px;text-decoration: none;'
							href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate3}'>
								Confirm
							</a>
														
						</td>
													
						<td>
														
								&nbsp;&nbsp;&nbsp;&nbsp;Program Date(3rd Option) : {SessionDate3} <br /> 
								&nbsp;&nbsp;&nbsp;&nbsp;Meal Start Time : {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Session Start Time : {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Session End Time : {SessionEndTime}
						</td>
														
					</tr>
				</table>
											
				<table>
					<tr>
						<td style='background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							<a style='display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
									Not Available
								</a>
 
							</td>
 
							<td>
									&nbsp; &nbsp; &nbsp; &nbsp; Not Available
						</td>
					</tr>
				</table>";

            string paraLast = @"<hr>
			Program Location: <span><strong> {LocationName} </strong> &nbsp; </span > &nbsp;<br />
			Your Honorarium: <span><strong> {Honorarium} </strong> &nbsp; </span> &nbsp;<br />
			Additional Presenter: if applicable  <span><strong> {ModeratorName} </strong> &nbsp; </span> &nbsp;<br />
			Materials to be presented: <span><strong> {SessionCredits} </strong> &nbsp; </span> &nbsp;<br />
			Session Contact: if applicable <span><strong> {SessionContact} </strong> &nbsp; </span> &nbsp;<br />
			Session Contact Number: <span><strong>{ContactPhoneNumber} </strong> &nbsp; </span> &nbsp;<br />
			Session Contact Email: <span><strong>{ContactEmailAddress} </strong> &nbsp; </span > &nbsp;<br />                                                                 

			<p>Please visit <a href='https://speaker.ccpdhm.com/'>speaker.ccpdhm.com</a> to gain access to the program materials.  
			
			
		   <p>Should you have any questions, please do not hesitate to contact your CHRC/CCPDHM program coordinator: {ProgramCoordinator}, email address: {CoordinatorEmail}</ p >
		  </td>
	</ tr >
</table> ";

            if (string.IsNullOrEmpty(pr.ProgramDate2) && (string.IsNullOrEmpty(pr.ProgramDate3)))
            {
                //Email1 option4
                html = para1 + paraLast;
            }


            if (!string.IsNullOrEmpty(pr.ProgramDate2) && (string.IsNullOrEmpty(pr.ProgramDate3)))
            {
                //Email1 option2
                html = para1 + para2 + paraLast;
            }

            if (!string.IsNullOrEmpty(pr.ProgramDate3) && (string.IsNullOrEmpty(pr.ProgramDate2)))
            {


                //Email1 option3
                html = para1 + para3 + paraLast;

            }

            if (!string.IsNullOrEmpty(pr.ProgramDate3) && (!string.IsNullOrEmpty(pr.ProgramDate2)))
            {
                //EmailText1
                html = para1 + para2 + para3 + paraLast;
            }



            html = html.Replace("{ProgramRequestID}", pr.ProgramRequestID.ToString());
            html = html.Replace("{LastName}", um.LastName);
            html = html.Replace("{ProgramName}", ProgramName);

            html = html.Replace("{querydate1}", pr.ProgramDate1);
            html = html.Replace("{querydate2}", pr.ProgramDate2);
            html = html.Replace("{querydate3}", pr.ProgramDate3);

            html = html.Replace("{SessionDate1}", ConvertTimetoProperFormat(pr.ProgramDate1));
            if (!string.IsNullOrEmpty(pr.ProgramDate2))
            {
                html = html.Replace("{SessionDate2}", ConvertTimetoProperFormat(pr.ProgramDate2));
            }
            if (!string.IsNullOrEmpty(pr.ProgramDate3))
            {
                html = html.Replace("{SessionDate3}", ConvertTimetoProperFormat(pr.ProgramDate3));
            }



            html = html.Replace("{MealStartTime}", pr.MealStartTime);
            html = html.Replace("{SessionStartTime}", pr.ProgramStartTime);
            html = html.Replace("{SessionEndTime}", pr.ProgramEndTime);


            html = html.Replace("{LocationName}", pr.LocationName);
            html = html.Replace("{Honorarium}", um.SpeakerHonariumRange);
            html = html.Replace("{ModeratorName}", ModeratorName);
            html = html.Replace("{Materials}", Materials);
            html = html.Replace("{SessionContact}", pr.ContactName);
            html = html.Replace("{ContactPhoneNumber}", pr.ContactPhone);
            html = html.Replace("{ContactEmailAddress}", pr.ContactEmail);
            html = html.Replace("{SpeakerConfirmUrl}", SpeakerConfirmUrl);

            html = html.Replace("{SessionCredits}", SessionCredits);

            html = html.Replace("{ProgramCoordinator}", ConfigurationManager.AppSettings["ProgramCoordinator"]);
            html = html.Replace("{CoordinatorEmail}", ConfigurationManager.AppSettings["CoordinatorEmail"]);
            html = html.Replace("{CoordinatorPhone}", ConfigurationManager.AppSettings["CoordinatorPhone"]);


            return html;

        }
        private static string RegisteredEmailBodyfr(ProgramRequest pr, UserModel um, string ProgramName, string ModeratorName, string SessionCredits)
        {
            string html = string.Empty;


            string SpeakerConfirmUrl = System.Configuration.ConfigurationManager.AppSettings["SpeakerConfirmUrl"];

            string Materials = "";

            if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
            {
                Materials = Path.GetFileName(pr.SessionAgendaFileName);


            }

            string para1 = @"<table width = '900' border = '0' cellspacing = '0' cellpadding = '10'>
				
				 <tr>                         
				<td align = 'left'> 										
				<br /> 
		
				<p>Cher Dr.  {LastName}, </p> 
				<p>Vous êtes invité à présenter le programme de Formation Professionnelle Continue intitulé  {ProgramName}. 
				Veuillez svp prendre connaissance des informations ci-jointes et nous confirmer votre disponibilité en cliquant le bouton Confirmer ou Non Disponible.</p> 
			
					 <table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							<a style='display: block;color: #ffffff;font-size: 10px;text-decoration: none;' 
							href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate1}'>
								Confirmer
							</a>						
						</td>
													
						<td>														
								&nbsp;&nbsp;&nbsp;&nbsp;Date du Programme: {SessionDate1}<br /> 
								&nbsp;&nbsp;&nbsp;&nbsp;Début du repas: {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Début de session: {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Fin de session: {SessionEndTime}
						</td>
					</tr>
				</table>
				
											
				<table>
					<tr>
						<td style='background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							<a style='display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
									Non Disponible 	
								</a>
							</td>
 
							<td>
									&nbsp; &nbsp; &nbsp; &nbsp; Non Disponible 	
						</td>
					</tr>
				</table>";

            //if there is program date 2
            string para2 = @"<table>
					<tr>
						<td style = 'background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							 <a style = 'display: block;color: #ffffff;font-size: 10px;text-decoration: none;'
							href = '{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate2}' >
								Confirmer
							</a>
						</td>
						<td>
								&nbsp; &nbsp; &nbsp; &nbsp; Date du Programme(2nd Option): {SessionDate2} <br />
									 &nbsp; &nbsp; &nbsp; &nbsp; Début du repas: {MealStartTime}
			&nbsp; &nbsp; &nbsp; &nbsp; Début de session: {SessionStartTime}
			&nbsp; &nbsp; &nbsp; &nbsp; Fin de session: {SessionEndTime}
						</td>
					</tr>
				</table>
				<table>
					<tr>
						<td style = 'background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							 <a style = 'display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href = '{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
										Non Disponible 
									</a>   
								</td>
								<td>
										&nbsp; &nbsp; &nbsp; &nbsp; Non Disponible 
						</td>
					</tr>
				</table>";

            //if there is program date 3
            string para3 = @"<table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
						<a style='display: block;color: #ffffff;font-size: 10px;text-decoration: none;'
							href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate3}'>
								Confirmer
							</a>
														
						</td>
													
						<td>
														
								&nbsp;&nbsp;&nbsp;&nbsp;Date du Programme(3rd Option) : {SessionDate3} <br /> 
								&nbsp;&nbsp;&nbsp;&nbsp;Début du repas : {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Début de session : {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Fin de session : {SessionEndTime}
						</td>
														
					</tr>
				</table>
											
				<table>
					<tr>
						<td style='background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							<a style='display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
									Non Disponible 
								</a>
 
							</td>
 
							<td>
									&nbsp; &nbsp; &nbsp; &nbsp;Non Disponible 
						</td>
					</tr>
				</table>";

            string paraLast = @"<hr>
			Lieu du Programme: <span><strong> {LocationName} </strong> &nbsp; </span > &nbsp;<br />
			Vos Honoraires: <span><strong> {Honorarium} </strong> &nbsp; </span> &nbsp;<br />
			Conférencier additionnel: si applicable    <span><strong> {ModeratorName} </strong> &nbsp; </span> &nbsp;<br />
			Contenu / Matériel scientifique à présenter: <span><strong> {SessionCredits} </strong> &nbsp; </span> &nbsp;<br />
			Personne-contact pour la session: si applicable <span><strong> {SessionContact} </strong> &nbsp; </span> &nbsp;<br />
			Numéro de la session: <span><strong>{ContactPhoneNumber} </strong> &nbsp; </span> &nbsp;<br />
			Adresse courriel pour la session: <span><strong>{ContactEmailAddress} </strong> &nbsp; </span > &nbsp;<br />                                                                 

			<p>Veuillez consulter le site  <a href='https://speaker.ccpdhm.com/'>speaker.ccpdhm.com</a> pour avoir accès au contenu / matériel scientifique du Programme.  
			
			
		   <p>Pour toute question, n’hésitez pas à contacter votre coordinatrice de programme CCRC/CIDPSM: Gail Karaim à l’adresse courriel suivante: {ProgramCoordinator}, email address: {CoordinatorEmail}</ p >
		  </td>
	</ tr >
</table> ";

            if (string.IsNullOrEmpty(pr.ProgramDate2) && (string.IsNullOrEmpty(pr.ProgramDate3)))
            {
                //Email1 option4
                html = para1 + paraLast;
            }


            if (!string.IsNullOrEmpty(pr.ProgramDate2) && (string.IsNullOrEmpty(pr.ProgramDate3)))
            {
                //Email1 option2
                html = para1 + para2 + paraLast;
            }

            if (!string.IsNullOrEmpty(pr.ProgramDate3) && (string.IsNullOrEmpty(pr.ProgramDate2)))
            {


                //Email1 option3
                html = para1 + para3 + paraLast;

            }

            if (!string.IsNullOrEmpty(pr.ProgramDate3) && (!string.IsNullOrEmpty(pr.ProgramDate2)))
            {
                //EmailText1
                html = para1 + para2 + para3 + paraLast;
            }



            html = html.Replace("{ProgramRequestID}", pr.ProgramRequestID.ToString());
            html = html.Replace("{LastName}", um.LastName);
            html = html.Replace("{ProgramName}", ProgramName);

            html = html.Replace("{querydate1}", pr.ProgramDate1);
            html = html.Replace("{querydate2}", pr.ProgramDate2);
            html = html.Replace("{querydate3}", pr.ProgramDate3);

            html = html.Replace("{SessionDate1}", UserHelper.ConvertToFrenchDate(ConvertTimetoProperFormat(pr.ProgramDate1)));
            if (!string.IsNullOrEmpty(pr.ProgramDate2))
            {
                html = html.Replace("{SessionDate2}", UserHelper.ConvertToFrenchDate(ConvertTimetoProperFormat(pr.ProgramDate2)));
            }
            if (!string.IsNullOrEmpty(pr.ProgramDate3))
            {
                html = html.Replace("{SessionDate3}", UserHelper.ConvertToFrenchDate(ConvertTimetoProperFormat(pr.ProgramDate3)));
            }



            html = html.Replace("{MealStartTime}", pr.MealStartTime);
            html = html.Replace("{SessionStartTime}", pr.ProgramStartTime);
            html = html.Replace("{SessionEndTime}", pr.ProgramEndTime);


            html = html.Replace("{LocationName}", pr.LocationName);
            html = html.Replace("{Honorarium}", um.SpeakerHonariumRange);
            html = html.Replace("{ModeratorName}", ModeratorName);
            html = html.Replace("{Materials}", Materials);
            html = html.Replace("{SessionContact}", pr.ContactName);
            html = html.Replace("{ContactPhoneNumber}", pr.ContactPhone);
            html = html.Replace("{ContactEmailAddress}", pr.ContactEmail);
            html = html.Replace("{SpeakerConfirmUrl}", SpeakerConfirmUrl);

            html = html.Replace("{SessionCredits}", SessionCredits);

            html = html.Replace("{ProgramCoordinator}", ConfigurationManager.AppSettings["ProgramCoordinator"]);
            html = html.Replace("{CoordinatorEmail}", ConfigurationManager.AppSettings["CoordinatorEmail"]);
            html = html.Replace("{CoordinatorPhone}", ConfigurationManager.AppSettings["CoordinatorPhone"]);


            return html;

        }
        private static string RegistrationNotCompleteBody(ProgramRequest pr, UserModel um, string ProgramName, string ModeratorName, string SessionCredits)
        {
            string html = string.Empty;

            string Materials = "";

            string SpeakerConfirmUrl = System.Configuration.ConfigurationManager.AppSettings["SpeakerConfirmUrl"];

            if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
            {
                Materials = Path.GetFileName(pr.SessionAgendaFileName);
            }


            string para1 = @"<table width = '900' border = '0' cellspacing = '0' cellpadding = '10'>
				
				 <tr>                         
				<td align = 'left'> 										
				<br /> 
		
				<p>Dear Dr. {LastName}, </p> 
				<p>You are invited to present the accredited CPD program entitled {ProgramName}. 
				Please review the details and confirm your availability by clicking the Confirm or Not Available button.</p> 
			
					 <table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							<a style='display: block;color: #ffffff;font-size: 10px;text-decoration: none;' 
							href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate1}'>
								Confirm
							</a>						
						</td>
													
						<td>														
								&nbsp;&nbsp;&nbsp;&nbsp;Program Date: {SessionDate1}<br /> 
								&nbsp;&nbsp;&nbsp;&nbsp;Meal Start Time: {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Session Start Time: {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Session End Time: {SessionEndTime}
						</td>
					</tr>
				</table>
				
											
				<table>
					<tr>
						<td style='background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							<a style='display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
									Not Available
								</a>
							</td>
 
							<td>
									&nbsp; &nbsp; &nbsp; &nbsp; Not Available
						</td>
					</tr>
				</table>";

            //if there is program date 2
            string para2 = @"<table>
					<tr>
						<td style = 'background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							 <a style = 'display: block;color: #ffffff;font-size: 10px;text-decoration: none;'
							href = '{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate2}' >
								Confirm
							</a>
						</td>
						<td>
								&nbsp; &nbsp; &nbsp; &nbsp; Program Date(2nd Option): {SessionDate2} <br />
									 &nbsp; &nbsp; &nbsp; &nbsp; Meal Start Time: {MealStartTime}
			&nbsp; &nbsp; &nbsp; &nbsp; Session Start Time: {SessionStartTime}
			&nbsp; &nbsp; &nbsp; &nbsp; Session End Time: {SessionEndTime}
						</td>
					</tr>
				</table>
				<table>
					<tr>
						<td style = 'background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							 <a style = 'display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href = '{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
										Not Available
									</a>   
								</td>
								<td>
										&nbsp; &nbsp; &nbsp; &nbsp; Not Available
						</td>
					</tr>
				</table>";

            //if there is program date 3
            string para3 = @"<table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
						<a style='display: block;color: #ffffff;font-size: 10px;text-decoration: none;'
							href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate3}'>
								Confirm
							</a>
														
						</td>
													
						<td>
														
								&nbsp;&nbsp;&nbsp;&nbsp;Program Date(3rd Option) : {SessionDate3} <br /> 
								&nbsp;&nbsp;&nbsp;&nbsp;Meal Start Time : {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Session Start Time : {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Session End Time : {SessionEndTime}
						</td>
														
					</tr>
				</table>
											
				<table>
					<tr>
						<td style='background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							<a style='display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
									Not Available
								</a>
 
							</td>
 
							<td>
									&nbsp; &nbsp; &nbsp; &nbsp; Not Available
						</td>
					</tr>
				</table>";

            string paraLast = @"<hr>
			Program Location: <span><strong> {LocationName} </strong> &nbsp; </span > &nbsp;<br />
			Your Honorarium: <span><strong> {Honorarium} </strong> &nbsp; </span> &nbsp;<br />
			Additional Presenter: if applicable  <span><strong> {ModeratorName} </strong> &nbsp; </span> &nbsp;<br />
			Materials to be presented: <span><strong> {SessionCredits} </strong> &nbsp; </span> &nbsp;<br />
			Session Contact: if applicable <span><strong> {SessionContact} </strong> &nbsp; </span> &nbsp;<br />
			Session Contact Number: <span><strong>{ContactPhoneNumber} </strong> &nbsp; </span> &nbsp;<br />
			Session Contact Email: <span><strong>{ContactEmailAddress} </strong> &nbsp; </span > &nbsp;<br />                                                                 

			<p>Next Steps: Please visit <a href='https://speaker.ccpdhm.com/'>speaker.ccpdhm.com</a> and complete the required forms (payee and COI forms) at your earliest convenience.  
			The Speaker Resource Centre includes all the pertinent program materials for your reference and perusal. 
			

		   <p>Should you have any questions, please do not hesitate to contact your CHRC/CCPDHM program coordinator: {ProgramCoordinator}, email address: {CoordinatorEmail}</ p >
		  </td>
	</ tr >
</table> ";




            if (string.IsNullOrEmpty(pr.ProgramDate2) && (string.IsNullOrEmpty(pr.ProgramDate3)))
            {
                //Email2Option4
                html = para1 + paraLast;
            }


            if (!string.IsNullOrEmpty(pr.ProgramDate2) && (string.IsNullOrEmpty(pr.ProgramDate3)))
            {


                //Email2Option2
                html = para1 + para2 + paraLast;
            }

            if (!string.IsNullOrEmpty(pr.ProgramDate3) && (string.IsNullOrEmpty(pr.ProgramDate2)))
            {

                //Email2Option 3
                html = para1 + para3 + paraLast;
            }

            if (!string.IsNullOrEmpty(pr.ProgramDate3) && (!string.IsNullOrEmpty(pr.ProgramDate2)))
            {
                //EmailText2
                html = para1 + para2 + para3 + paraLast;

            }

            html = html.Replace("{ProgramRequestID}", pr.ProgramRequestID.ToString());
            html = html.Replace("{LastName}", um.LastName);
            html = html.Replace("{ProgramName}", ProgramName);

            html = html.Replace("{querydate1}", pr.ProgramDate1);
            html = html.Replace("{querydate2}", pr.ProgramDate2);
            html = html.Replace("{querydate3}", pr.ProgramDate3);

            html = html.Replace("{SessionDate1}", ConvertTimetoProperFormat(pr.ProgramDate1));
            if (!string.IsNullOrEmpty(pr.ProgramDate2))
            {
                html = html.Replace("{SessionDate2}", ConvertTimetoProperFormat(pr.ProgramDate2));
            }
            if (!string.IsNullOrEmpty(pr.ProgramDate3))
            {
                html = html.Replace("{SessionDate3}", ConvertTimetoProperFormat(pr.ProgramDate3));
            }

            html = html.Replace("{MealStartTime}", pr.MealStartTime);
            html = html.Replace("{SessionStartTime}", pr.ProgramStartTime);
            html = html.Replace("{SessionEndTime}", pr.ProgramEndTime);


            html = html.Replace("{LocationName}", pr.LocationName);
            html = html.Replace("{Honorarium}", um.SpeakerHonariumRange);
            html = html.Replace("{ModeratorName}", ModeratorName);
            html = html.Replace("{Materials}", Materials);
            html = html.Replace("{SessionContact}", pr.ContactName);
            html = html.Replace("{ContactPhoneNumber}", pr.ContactPhone);
            html = html.Replace("{ContactEmailAddress}", pr.ContactEmail);
            html = html.Replace("{SpeakerConfirmUrl}", SpeakerConfirmUrl);
            html = html.Replace("{SessionCredits}", SessionCredits);

            html = html.Replace("{ProgramCoordinator}", ConfigurationManager.AppSettings["ProgramCoordinator"]);
            html = html.Replace("{CoordinatorEmail}", ConfigurationManager.AppSettings["CoordinatorEmail"]);
            html = html.Replace("{CoordinatorPhone}", ConfigurationManager.AppSettings["CoordinatorPhone"]);

            return html;

        }
        private static string RegistrationNotCompleteBodyfr(ProgramRequest pr, UserModel um, string ProgramName, string ModeratorName, string SessionCredits)
        {
            string html = string.Empty;

            string Materials = "";

            string SpeakerConfirmUrl = System.Configuration.ConfigurationManager.AppSettings["SpeakerConfirmUrl"];

            if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
            {
                Materials = Path.GetFileName(pr.SessionAgendaFileName);
            }


            string para1 = @"<table width = '900' border = '0' cellspacing = '0' cellpadding = '10'>
				
				 <tr>                         
				<td align = 'left'> 										
				<br /> 
		
				<p>Cher Dr.  {LastName}, </p> 
				<p>Vous êtes invité à présenter le programme de Formation Professionnelle Continue intitulé  {ProgramName}. 
				Veuillez svp prendre connaissance des informations ci-jointes et nous confirmer votre disponibilité en cliquant le bouton Confirmer ou Non Disponible.</p> 
			
					 <table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							<a style='display: block;color: #ffffff;font-size: 10px;text-decoration: none;' 
							href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate1}'>
								Confirmer
							</a>						
						</td>
													
						<td>														
								&nbsp;&nbsp;&nbsp;&nbsp;Date du Programme: {SessionDate1}<br /> 
								&nbsp;&nbsp;&nbsp;&nbsp;Début du repas: {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Début de session: {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Fin de session: {SessionEndTime}
						</td>
					</tr>
				</table>
				
											
				<table>
					<tr>
						<td style='background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							<a style='display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
									Non Disponible 
								</a>
							</td>
 
							<td>
									&nbsp; &nbsp; &nbsp; &nbsp; Non Disponible 
						</td>
					</tr>
				</table>";

            //if there is program date 2
            string para2 = @"<table>
					<tr>
						<td style = 'background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							 <a style = 'display: block;color: #ffffff;font-size: 10px;text-decoration: none;'
							href = '{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate2}' >
								Confirmer
							</a>
						</td>
						<td>
								&nbsp; &nbsp; &nbsp; &nbsp; Date du Programme(2nd Option): {SessionDate2} <br />
									 &nbsp; &nbsp; &nbsp; &nbsp; Début du repas: {MealStartTime}
			&nbsp; &nbsp; &nbsp; &nbsp; Début de session: {SessionStartTime}
			&nbsp; &nbsp; &nbsp; &nbsp; Fin de session: {SessionEndTime}
						</td>
					</tr>
				</table>
				<table>
					<tr>
						<td style = 'background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							 <a style = 'display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href = '{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
										Fin de session
									</a>   
								</td>
								<td>
										&nbsp; &nbsp; &nbsp; &nbsp; Fin de session
						</td>
					</tr>
				</table>";

            //if there is program date 3
            string para3 = @"<table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
						<a style='display: block;color: #ffffff;font-size: 10px;text-decoration: none;'
							href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate3}'>
								Confirmer
							</a>
														
						</td>
													
						<td>
														
								&nbsp;&nbsp;&nbsp;&nbsp;Date du Programme(3rd Option) : {SessionDate3} <br /> 
								&nbsp;&nbsp;&nbsp;&nbsp;Début du repas : {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Début de session : {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Fin de session: {SessionEndTime}
						</td>
														
					</tr>
				</table>
											
				<table>
					<tr>
						<td style='background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							<a style='display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
									Non Disponible
								</a>
 
							</td>
 
							<td>
									&nbsp; &nbsp; &nbsp; &nbsp;Non Disponible
						</td>
					</tr>
				</table>";

            string paraLast = @"<hr>
			Lieu du Programme: <span><strong> {LocationName} </strong> &nbsp; </span > &nbsp;<br />
			Vos Honoraires: <span><strong> {Honorarium} </strong> &nbsp; </span> &nbsp;<br />
			Conférencier additionnel: si applicable    <span><strong> {ModeratorName} </strong> &nbsp; </span> &nbsp;<br />
			Contenu / Matériel scientifique à présenter: <span><strong> {SessionCredits} </strong> &nbsp; </span> &nbsp;<br />
			Personne-contact pour la session: si applicable  <span><strong> {SessionContact} </strong> &nbsp; </span> &nbsp;<br />
			Numéro de la session: <span><strong>{ContactPhoneNumber} </strong> &nbsp; </span> &nbsp;<br />
			Adresse courriel pour la session <span><strong>{ContactEmailAddress} </strong> &nbsp; </span > &nbsp;<br />                                                                 

			<p>Étapes suivantes: Veuillez joindre votre centre de données personnelles <a href='https://speaker.ccpdhm.com/'>speaker.ccpdhm.com</a>  COI forms) et remplir les formulaires requis (Informations de paiement et Conflits d’Intérêt) dès qu’il vous conviendra.
		Visitez le Centre des Ressources pour Conférencier pour avoir accès à tous les documents pertinents au programme et disponibles mis à votre disposition pour consultation
			

		   <p>Pour toute question, n’hésitez pas à contacter votre coordinatrice de programme CCRC/CIDPSM: {ProgramCoordinator}, à l’adresse courriel suivante: {CoordinatorEmail}</ p >
		  </td>
	</ tr >
</table> ";




            if (string.IsNullOrEmpty(pr.ProgramDate2) && (string.IsNullOrEmpty(pr.ProgramDate3)))
            {
                //Email2Option4
                html = para1 + paraLast;
            }


            if (!string.IsNullOrEmpty(pr.ProgramDate2) && (string.IsNullOrEmpty(pr.ProgramDate3)))
            {


                //Email2Option2
                html = para1 + para2 + paraLast;
            }

            if (!string.IsNullOrEmpty(pr.ProgramDate3) && (string.IsNullOrEmpty(pr.ProgramDate2)))
            {

                //Email2Option 3
                html = para1 + para3 + paraLast;
            }

            if (!string.IsNullOrEmpty(pr.ProgramDate3) && (!string.IsNullOrEmpty(pr.ProgramDate2)))
            {
                //EmailText2
                html = para1 + para2 + para3 + paraLast;

            }

            html = html.Replace("{ProgramRequestID}", pr.ProgramRequestID.ToString());
            html = html.Replace("{LastName}", um.LastName);
            html = html.Replace("{ProgramName}", ProgramName);

            html = html.Replace("{querydate1}", pr.ProgramDate1);
            html = html.Replace("{querydate2}", pr.ProgramDate2);
            html = html.Replace("{querydate3}", pr.ProgramDate3);

            html = html.Replace("{SessionDate1}", UserHelper.ConvertToFrenchDate(ConvertTimetoProperFormat(pr.ProgramDate1)));
            if (!string.IsNullOrEmpty(pr.ProgramDate2))
            {
                html = html.Replace("{SessionDate2}", UserHelper.ConvertToFrenchDate(ConvertTimetoProperFormat(pr.ProgramDate2)));
            }
            if (!string.IsNullOrEmpty(pr.ProgramDate3))
            {
                html = html.Replace("{SessionDate3}", UserHelper.ConvertToFrenchDate(ConvertTimetoProperFormat(pr.ProgramDate3)));
            }

            html = html.Replace("{MealStartTime}", pr.MealStartTime);
            html = html.Replace("{SessionStartTime}", pr.ProgramStartTime);
            html = html.Replace("{SessionEndTime}", pr.ProgramEndTime);


            html = html.Replace("{LocationName}", pr.LocationName);
            html = html.Replace("{Honorarium}", um.SpeakerHonariumRange);
            html = html.Replace("{ModeratorName}", ModeratorName);
            html = html.Replace("{Materials}", Materials);
            html = html.Replace("{SessionContact}", pr.ContactName);
            html = html.Replace("{ContactPhoneNumber}", pr.ContactPhone);
            html = html.Replace("{ContactEmailAddress}", pr.ContactEmail);
            html = html.Replace("{SpeakerConfirmUrl}", SpeakerConfirmUrl);
            html = html.Replace("{SessionCredits}", SessionCredits);

            html = html.Replace("{ProgramCoordinator}", ConfigurationManager.AppSettings["ProgramCoordinator"]);
            html = html.Replace("{CoordinatorEmail}", ConfigurationManager.AppSettings["CoordinatorEmail"]);
            html = html.Replace("{CoordinatorPhone}", ConfigurationManager.AppSettings["CoordinatorPhone"]);

            return html;

        }
        private static string NotRegisteredBody(ProgramRequest pr, UserModel um, string ProgramName, string ModeratorName, string SessionCredits)
        {
            string html = string.Empty;

            string Materials = "";
            string SpeakerConfirmUrl = System.Configuration.ConfigurationManager.AppSettings["SpeakerConfirmUrl"];

            if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
            {
                Materials = Path.GetFileName(pr.SessionAgendaFileName);


            }

            string para1 = @"<table width = '900' border = '0' cellspacing = '0' cellpadding = '10'>
				
				 <tr>                         
				<td align = 'left'> 										
				<br /> 
		
				<p>Dear Dr. {LastName}, </p> 
				<p>You are invited to present the accredited CPD program entitled {ProgramName}. 
				Please review the details and confirm your availability by clicking the Confirm or Not Available button.</p>
			
					 <table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							<a style='display: block;color: #ffffff;font-size: 10px;text-decoration: none;' 
							href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate1}'>
								Confirm
							</a>						
						</td>
													
						<td>														
								&nbsp;&nbsp;&nbsp;&nbsp;Program Date: {SessionDate1}<br /> 
								&nbsp;&nbsp;&nbsp;&nbsp;Meal Start Time: {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Session Start Time: {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Session End Time: {SessionEndTime}
						</td>
					</tr>
				</table>
				
											
				<table>
					<tr>
						<td style='background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							<a style='display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
									Not Available
								</a>
							</td>
 
							<td>
									&nbsp; &nbsp; &nbsp; &nbsp; Not Available
						</td>
					</tr>
				</table>";

            //if there is program date 2
            string para2 = @"<table>
					<tr>
						<td style = 'background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							 <a style = 'display: block;color: #ffffff;font-size: 10px;text-decoration: none;'
							href = '{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate2}' >
								Confirm
							</a>
						</td>
						<td>
								&nbsp; &nbsp; &nbsp; &nbsp; Program Date(2nd Option): {SessionDate2} <br />
									 &nbsp; &nbsp; &nbsp; &nbsp; Meal Start Time: {MealStartTime}
			&nbsp; &nbsp; &nbsp; &nbsp; Session Start Time: {SessionStartTime}
			&nbsp; &nbsp; &nbsp; &nbsp; Session End Time: {SessionEndTime}
						</td>
					</tr>
				</table>
				<table>
					<tr>
						<td style = 'background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							 <a style = 'display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href = '{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
										Not Available
									</a>   
								</td>
								<td>
										&nbsp; &nbsp; &nbsp; &nbsp; Not Available
						</td>
					</tr>
				</table>";

            //if there is program date 3
            string para3 = @"<table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
						<a style='display: block;color: #ffffff;font-size: 10px;text-decoration: none;'
							href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate3}'>
								Confirm
							</a>
														
						</td>
													
						<td>
														
								&nbsp;&nbsp;&nbsp;&nbsp;Program Date(3rd Option) : {SessionDate3} <br /> 
								&nbsp;&nbsp;&nbsp;&nbsp;Meal Start Time : {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Session Start Time : {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Session End Time : {SessionEndTime}
						</td>
														
					</tr>
				</table>
											
				<table>
					<tr>
						<td style='background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							<a style='display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
									Not Available
								</a>
 
							</td>
 
							<td>
									&nbsp; &nbsp; &nbsp; &nbsp; Not Available
						</td>
					</tr>
				</table>";

            string paraLast = @"<hr>
			Program Location: <span><strong> {LocationName} </strong> &nbsp; </span > &nbsp;<br />
			Your Honorarium: <span><strong> {Honorarium} </strong> &nbsp; </span> &nbsp;<br />
			Additional Presenter: if applicable  <span><strong> {ModeratorName} </strong> &nbsp; </span> &nbsp;<br />
			Materials to be presented: <span><strong> {SessionCredits} </strong> &nbsp; </span> &nbsp;<br />
			Session Contact: if applicable <span><strong> {SessionContact} </strong> &nbsp; </span> &nbsp;<br />
			Session Contact Number: <span><strong>{ContactPhoneNumber} </strong> &nbsp; </span> &nbsp;<br />
			Session Contact Email: <span><strong>{ContactEmailAddress} </strong> &nbsp; </span > &nbsp;<br />                                                                 

			<p>Next Steps: </p> 

			1. Please visit <a href='https://speaker.ccpdhm.com/activate'> speaker.ccpdhm.com/activate </a> and register - your username is: {SpeakerEmail} <br />
			2. Once your registration is complete, please login to your personal resource centre <a href='https://speaker.ccpdhm.com'> speaker.ccpdhm.com</a> and complete the required forms (payee and COI forms) at your earliest convenience.<br />			
			3. Access the Speaker Resource Centre to gain access to all the pertinent program materials for your reference and perusal. <br />
			

			<p>• If you are <span style='color:red'>NOT INTERESTED</span> in participating in this program as a speaker, please go to <a href='https://speaker.ccpdhm.com/optout'> speaker.ccpdhm.com/optout</a>  and enter your username: {SpeakerEmail} </p>

		   <p>Should you have any questions, please do not hesitate to contact your CHRC/CCPDHM program coordinator: {ProgramCoordinator}, email address: {CoordinatorEmail}</ p >
		  </td>
	</ tr >
</table> ";



            if (string.IsNullOrEmpty(pr.ProgramDate2) && (string.IsNullOrEmpty(pr.ProgramDate3)))
            {
                //Email3 option 4
                html = para1 + paraLast;
            }


            if (!string.IsNullOrEmpty(pr.ProgramDate2) && (string.IsNullOrEmpty(pr.ProgramDate3)))
            {

                //Email3 option2
                html = para1 + para2 + paraLast;
            }

            if (!string.IsNullOrEmpty(pr.ProgramDate3) && (string.IsNullOrEmpty(pr.ProgramDate2)))
            {
                //Email3 option3
                html = para1 + para3 + paraLast;
            }

            if (!string.IsNullOrEmpty(pr.ProgramDate3) && (!string.IsNullOrEmpty(pr.ProgramDate2)))
            {
                //EmailText3
                html = para1 + para2 + para3 + paraLast;


            }

            html = html.Replace("{ProgramRequestID}", pr.ProgramRequestID.ToString());
            html = html.Replace("{LastName}", um.LastName);
            html = html.Replace("{ProgramName}", ProgramName);

            html = html.Replace("{querydate1}", pr.ProgramDate1);
            html = html.Replace("{querydate2}", pr.ProgramDate2);
            html = html.Replace("{querydate3}", pr.ProgramDate3);

            html = html.Replace("{SessionDate1}", ConvertTimetoProperFormat(pr.ProgramDate1));
            if (!string.IsNullOrEmpty(pr.ProgramDate2))
            {
                html = html.Replace("{SessionDate2}", ConvertTimetoProperFormat(pr.ProgramDate2));
            }
            if (!string.IsNullOrEmpty(pr.ProgramDate3))
            {
                html = html.Replace("{SessionDate3}", ConvertTimetoProperFormat(pr.ProgramDate3));
            }

            html = html.Replace("{MealStartTime}", pr.MealStartTime);
            html = html.Replace("{SessionStartTime}", pr.ProgramStartTime);
            html = html.Replace("{SessionEndTime}", pr.ProgramEndTime);

            html = html.Replace("{LocationName}", pr.LocationName);
            html = html.Replace("{Honorarium}", um.SpeakerHonariumRange);
            html = html.Replace("{ModeratorName}", ModeratorName);
            html = html.Replace("{Materials}", Materials);
            html = html.Replace("{SessionContact}", pr.ContactName);
            html = html.Replace("{ContactPhoneNumber}", pr.ContactPhone);
            html = html.Replace("{ContactEmailAddress}", pr.ContactEmail);

            html = html.Replace("{SpeakerEmail}", um.EmailAddress);
            html = html.Replace("{SpeakerConfirmUrl}", SpeakerConfirmUrl);
            html = html.Replace("{SessionCredits}", SessionCredits);

            html = html.Replace("{ProgramCoordinator}", ConfigurationManager.AppSettings["ProgramCoordinator"]);
            html = html.Replace("{CoordinatorEmail}", ConfigurationManager.AppSettings["CoordinatorEmail"]);
            html = html.Replace("{CoordinatorPhone}", ConfigurationManager.AppSettings["CoordinatorPhone"]);


            return html;

        }
        private static string NotRegisteredBodyfr(ProgramRequest pr, UserModel um, string ProgramName, string ModeratorName, string SessionCredits)
        {
            string html = string.Empty;

            string Materials = "";
            string SpeakerConfirmUrl = System.Configuration.ConfigurationManager.AppSettings["SpeakerConfirmUrl"];

            if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
            {
                Materials = Path.GetFileName(pr.SessionAgendaFileName);


            }

            string para1 = @"<table width = '900' border = '0' cellspacing = '0' cellpadding = '10'>
				
				 <tr>                         
				<td align = 'left'> 										
				<br /> 
		
				<p>Cher Dr.  {LastName}, </p> 
				<p>Vous êtes invité à présenter le programme de Formation Professionnelle Continue intitulé  {ProgramName}. 
  Veuillez svp prendre connaissance des informations ci-jointes et nous confirmer votre disponibilité en cliquant le bouton Confirmer ou Non Disponible.</p>
			
					 <table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							<a style='display: block;color: #ffffff;font-size: 10px;text-decoration: none;' 
							href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate1}'>
								Confirmer
							</a>						
						</td>
													
						<td>														
								&nbsp;&nbsp;&nbsp;&nbsp;Date du Programme: {SessionDate1}<br /> 
								&nbsp;&nbsp;&nbsp;&nbsp;Début du repas: {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Début de session: {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Fin de session: {SessionEndTime}
						</td>
					</tr>
				</table>
				
											
				<table>
					<tr>
						<td style='background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							<a style='display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
									Non Disponible
								</a>
							</td>
 
							<td>
									&nbsp; &nbsp; &nbsp; &nbsp; Non Disponible
						</td>
					</tr>
				</table>";

            //if there is program date 2
            string para2 = @"<table>
					<tr>
						<td style = 'background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							 <a style = 'display: block;color: #ffffff;font-size: 10px;text-decoration: none;'
							href = '{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate2}' >
								Confirmer
							</a>
						</td>
						<td>
								&nbsp; &nbsp; &nbsp; &nbsp; Date du Programme(2nd Option): {SessionDate2} <br />
									 &nbsp; &nbsp; &nbsp; &nbsp; Début du repas: {MealStartTime}
			&nbsp; &nbsp; &nbsp; &nbsp; Début de session: {SessionStartTime}
			&nbsp; &nbsp; &nbsp; &nbsp; Fin de session: {SessionEndTime}
						</td>
					</tr>
				</table>
				<table>
					<tr>
						<td style = 'background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							 <a style = 'display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href = '{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
										Non Disponible
									</a>   
								</td>
								<td>
										&nbsp; &nbsp; &nbsp; &nbsp; Non Disponible
						</td>
					</tr>
				</table>";

            //if there is program date 3
            string para3 = @"<table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
						<a style='display: block;color: #ffffff;font-size: 10px;text-decoration: none;'
							href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate={querydate3}'>
								Confirmer
							</a>
														
						</td>
													
						<td>
														
								&nbsp;&nbsp;&nbsp;&nbsp;Date du Programme(3rd Option) : {SessionDate3} <br /> 
								&nbsp;&nbsp;&nbsp;&nbsp;Début du repas : {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Début de session : {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Fin de session : {SessionEndTime}
						</td>
														
					</tr>
				</table>
											
				<table>
					<tr>
						<td style='background-color: #D96C6B; padding:0px 2px; text-align: center;'>
							<a style='display:block; color: #ffffff; font-size: 10px; text-decoration: none;' href='{SpeakerConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
									Non Disponible
								</a>
 
							</td>
 
							<td>
									&nbsp; &nbsp; &nbsp; &nbsp; Non Disponible
						</td>
					</tr>
				</table>";

            string paraLast = @"<hr>
			Lieu du Programme: <span><strong> {LocationName} </strong> &nbsp; </span > &nbsp;<br />
			Vos Honoraires: <span><strong> {Honorarium} </strong> &nbsp; </span> &nbsp;<br />
			Conférencier additionnel: si applicable    <span><strong> {ModeratorName} </strong> &nbsp; </span> &nbsp;<br />
			Contenu / Matériel scientifique à présenter: <span><strong> {SessionCredits} </strong> &nbsp; </span> &nbsp;<br />
			Personne-contact pour la session: if applicable <span><strong> {SessionContact} </strong> &nbsp; </span> &nbsp;<br />
			Numéro de la session: <span><strong>{ContactPhoneNumber} </strong> &nbsp; </span> &nbsp;<br />
			Adresse courriel pour la session: <span><strong>{ContactEmailAddress} </strong> &nbsp; </span > &nbsp;<br />                                                                 

			<p>Étapes suivantes: </p> 

			1. Veuillez joindre le lien  <a href='https://speaker.ccpdhm.com/activate?lang=fr'>speaker.ccpdhm.com/activate?lang=fr</a> et vous enregistrer – votre nom d’utilisateur est: {SpeakerEmail} <br />
			2. Lorsque votre enregistrement est fait, veuillez joindre votre centre de données personnelles  <a href='https://speaker.ccpdhm.com'> speaker.ccpdhm.com</a> et remplir les formulaires requis (Informations de paiement et Conflits d’Intérêt) dès qu’il vous conviendra.<br />			
			3. Visitez le Centre des Ressources pour Conférencier pour avoir accès à tous les documents pertinents au programme et disponibles mis à votre disposition pour consultation <br />
			

			<p>Si vous n’êtes  <span style='color:red'>PAS INTÉRESSÉ </span>à participer à ce programme en tant que Conférencier, veuillez svp aller au site <a href='https://speaker.ccpdhm.com/optout?lang=fr'> speaker.ccpdhm.com/optout?lang=fr</a>  et entrez votre nom d’utilisateur: {SpeakerEmail} </p>

		   <p>Pour toute question, n’hésitez pas à contacter votre coordinatrice de programme CCRC/CIDPSM: {ProgramCoordinator}, à l’adresse courriel suivante: {CoordinatorEmail}</ p >
		  </td>
	</ tr >
</table> ";



            if (string.IsNullOrEmpty(pr.ProgramDate2) && (string.IsNullOrEmpty(pr.ProgramDate3)))
            {
                //Email3 option 4
                html = para1 + paraLast;
            }


            if (!string.IsNullOrEmpty(pr.ProgramDate2) && (string.IsNullOrEmpty(pr.ProgramDate3)))
            {

                //Email3 option2
                html = para1 + para2 + paraLast;
            }

            if (!string.IsNullOrEmpty(pr.ProgramDate3) && (string.IsNullOrEmpty(pr.ProgramDate2)))
            {
                //Email3 option3
                html = para1 + para3 + paraLast;
            }

            if (!string.IsNullOrEmpty(pr.ProgramDate3) && (!string.IsNullOrEmpty(pr.ProgramDate2)))
            {
                //EmailText3
                html = para1 + para2 + para3 + paraLast;


            }

            html = html.Replace("{ProgramRequestID}", pr.ProgramRequestID.ToString());
            html = html.Replace("{LastName}", um.LastName);
            html = html.Replace("{ProgramName}", ProgramName);

            html = html.Replace("{querydate1}", pr.ProgramDate1);
            html = html.Replace("{querydate2}", pr.ProgramDate2);
            html = html.Replace("{querydate3}", pr.ProgramDate3);

            html = html.Replace("{SessionDate1}", UserHelper.ConvertToFrenchDate(ConvertTimetoProperFormat(pr.ProgramDate1)));
            if (!string.IsNullOrEmpty(pr.ProgramDate2))
            {
                html = html.Replace("{SessionDate2}", UserHelper.ConvertToFrenchDate(ConvertTimetoProperFormat(pr.ProgramDate2)));
            }
            if (!string.IsNullOrEmpty(pr.ProgramDate3))
            {
                html = html.Replace("{SessionDate3}", UserHelper.ConvertToFrenchDate(ConvertTimetoProperFormat(pr.ProgramDate3)));
            }

            html = html.Replace("{MealStartTime}", pr.MealStartTime);
            html = html.Replace("{SessionStartTime}", pr.ProgramStartTime);
            html = html.Replace("{SessionEndTime}", pr.ProgramEndTime);

            html = html.Replace("{LocationName}", pr.LocationName);
            html = html.Replace("{Honorarium}", um.SpeakerHonariumRange);
            html = html.Replace("{ModeratorName}", ModeratorName);
            html = html.Replace("{Materials}", Materials);
            html = html.Replace("{SessionContact}", pr.ContactName);
            html = html.Replace("{ContactPhoneNumber}", pr.ContactPhone);
            html = html.Replace("{ContactEmailAddress}", pr.ContactEmail);

            html = html.Replace("{SpeakerEmail}", um.EmailAddress);
            html = html.Replace("{SpeakerConfirmUrl}", SpeakerConfirmUrl);
            html = html.Replace("{SessionCredits}", SessionCredits);

            html = html.Replace("{ProgramCoordinator}", ConfigurationManager.AppSettings["ProgramCoordinator"]);
            html = html.Replace("{CoordinatorEmail}", ConfigurationManager.AppSettings["CoordinatorEmail"]);
            html = html.Replace("{CoordinatorPhone}", ConfigurationManager.AppSettings["CoordinatorPhone"]);


            return html;

        }
        public static string GetFileNameForEmailAttachment(string path, int ProgramRequestID)
        {


            string retVal = "";




            if (!(string.IsNullOrEmpty(path)))
            {
                var FileName = Path.GetFileName(path);
                retVal = ProgramRequestID + "/Agenda/" + FileName;
            }
            return retVal;

        }
        public static void SelectInvitationOption(ProgramRequest pr, string ModeratorName, string SessionCredit,string SessionCreditfr, Controller controller)
        {
            ProgramRepository repo = new ProgramRepository();
            UserRepository userRepo = new UserRepository();


            string ProgramName = repo.GetProgramRequestName(pr.ProgramID);
            string ProgramNamefr = repo.GetProgramRequestNamefr(pr.ProgramID);
            UserModel um = new UserModel();
            

            if (pr != null)
            {
                um = userRepo.GetUserForConfirmEmail(pr.ProgramSpeakerID);
                //get the id from Userinfo
                string SendEmail = userRepo.CheckUserStatusForEmail(um.UserID);

                if (SendEmail.Equals("RegisteredCompleted"))
                {
                    //when a new speaker is created, rep must specifiy language preference
                    //of the speaker, if the speaker speaks french send him french invitation
                    //this is new function so old speaker Language is null, new speaker is either en or fr
                    if (String.IsNullOrEmpty(um.Language) || um.Language.Equals("en"))
                        SendRegisteredEmail(pr, um, ProgramName, ModeratorName, SessionCredit, controller);
                    else
                    {//if the speaker has Language=fr, send french invitations.
                        
                        SendRegisteredEmailfr(pr, um, ProgramNamefr, ModeratorName, SessionCreditfr, controller);
                    }
                }
                if (SendEmail.Equals("RegistrationNotComplete"))
                {
                    if (String.IsNullOrEmpty(um.Language) || um.Language.Equals("en"))
                        SendRegistrationNotComplete(pr, um, ProgramName, ModeratorName, SessionCredit, controller);
                    else {
                        SendRegistrationNotCompletefr(pr, um, ProgramName, ModeratorName, SessionCredit, controller);
                    }
                }

                if (SendEmail.Equals("NotRegistered"))
                {
                    if (String.IsNullOrEmpty(um.Language) || um.Language.Equals("en"))
                        SendNotRegistered(pr, um, ProgramName, ModeratorName, SessionCredit, controller);
                    else
                    {
                        SendNotRegisteredfr(pr, um, ProgramNamefr, ModeratorName, SessionCreditfr, controller);
                    }
                }
            }

        }

        public static void SendEmailToSpeaker(ProgramRequestIIModel pr, string moderatorName, string sessionCredit, Controller controller) //moderatorName for content displayed
        {
            if (pr == null)
            {
                return;
            }

            ProgramRepository repo = new ProgramRepository();
            string programName = repo.GetProgramRequestName(pr.ProgramID);

            UserRepository userRepo = new UserRepository();
            UserModel um = userRepo.GetUserForConfirmEmail(pr.ProgramSpeakerID);

            string emailBody = "";
            string status = userRepo.CheckUserStatusForEmail(um.UserID);

            //from SessionCreditLookUp table
            int maxNumModules = 0;
            float sessionHr = 0;
            if (pr.SessionCredit16)
            {
                maxNumModules = 3;
                sessionHr = 1;
            }
            else if (pr.SessionCredit17)
            {
                maxNumModules = 5;
                sessionHr = 1.5f;
            }
            else if (pr.SessionCredit18)
            {
                maxNumModules = 7;
                sessionHr = 2f;
            }
            else if (pr.SessionCredit19)
            {
                maxNumModules = 10;
                sessionHr = 2.75f;
            }



            //block of confirm buttons; requirement 1
            string confirmBlock = @"<p style='color:#CC6600;text-decoration: underline;'>Requirement 1: Confirm Availability</p>
									<p>&nbsp;&nbsp;&nbsp;&nbsp;&#8226;&nbsp;&nbsp;&nbsp;&nbsp;Please review the details and confirm your availability by clicking the ""Confirm"" or ""Not Available"" button.</p>";

            //confirm button 1           
            string confirmBn = @"<table>
							<tr>
								<td>
									<a style='display: block;color: #ffffff;text-decoration: none;background-color: green;border-radius: 5px;color:white; width:100px; text-align: center;line-height: 35px;' 
							href='{ConfirmDateURL}'>Confirm
									</a>
								</td>
								<td>
									&nbsp;&nbsp;&nbsp;&nbsp;Program Date: {SessionDate}<br />
									&nbsp;&nbsp;&nbsp;&nbsp;Meal Start Time: {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Session Start Time: {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Session End Time: {SessionEndTime}
								</td>
							</tr>
						</table><br />";


            string confirmDateURL = System.Configuration.ConfigurationManager.AppSettings["SpeakerConfirmUrl"] + pr.ProgramRequestID.ToString() + "&ProgramDate=" + pr.ProgramDate1;
            confirmBlock += confirmBn.Replace("{ConfirmDateURL}", confirmDateURL).Replace("{SessionDate}", pr.ProgramDate1);

            //confirm button 2
            if (!string.IsNullOrEmpty(pr.ProgramDate2))
            {
                confirmDateURL = System.Configuration.ConfigurationManager.AppSettings["SpeakerConfirmUrl"] + pr.ProgramRequestID.ToString() + "&ProgramDate=" + pr.ProgramDate2;
                confirmBlock += confirmBn.Replace("{ConfirmDateURL}", confirmDateURL).Replace("{SessionDate}", pr.ProgramDate2);
            }
            //confirm button 3
            if (!string.IsNullOrEmpty(pr.ProgramDate3))
            {
                confirmDateURL = System.Configuration.ConfigurationManager.AppSettings["SpeakerConfirmUrl"] + pr.ProgramRequestID.ToString() + "&ProgramDate=" + pr.ProgramDate3;
                confirmBlock += confirmBn.Replace("{ConfirmDateURL}", confirmDateURL).Replace("{SessionDate}", pr.ProgramDate3);
            }


            //not available button
            confirmDateURL = System.Configuration.ConfigurationManager.AppSettings["SpeakerConfirmUrl"] + pr.ProgramRequestID.ToString() + "&ProgramDate=NotAvailable";
            confirmBlock += @"<div style='display:inline-flex;padding-left: 3px;'>
							<a style='display: block;color: #ffffff;text-decoration: none;background-color:  #D96C6B;border-radius: 5px;color:white; width:100px; text-align: center;line-height: 30px;' href='" + confirmDateURL + @"'>
								Not Available
							</a>                            
						</div>
						<div style='display:inline-flex;'>&nbsp;&nbsp;&nbsp;&nbsp;Not Available</div>";

            //requirement 2
            string selectModules = "<p style='color:#CC6600;text-decoration: underline;'>Requirement 2: Once you have confirmed your availability – Select Modules</p>"
                        + "<p>&nbsp;&nbsp;&nbsp;&nbsp;&#8226;&nbsp;&nbsp;&nbsp;&nbsp;The program duration is set for " + sessionHr + " hour(s).  Please select "
                        + maxNumModules
                        + " topics from the clinical question list by clicking <a href='" + ConfigurationManager.AppSettings["BaseURL"] + "/admin/managemodules?programrequestid=" + pr.ProgramRequestID + "' style='color:#CC6600'>SELECT MODULES</a> You will present these modules during your session.</p>"
                        + "<p>PROGRAM DETAILS:</p><hr/>"
                        + "Program Location: " + pr.LocationName + "<br/>"
                        + "Your Honorarium: " + um.SpeakerHonariumRange + "<br/>"
                        + "Additional Presenter: " + moderatorName + "<br/>"
                        + "Session Contact: " + pr.ContactName + "<br/>"
                        + "Session Contact Number: " + pr.ContactPhone + "<br/>"
                        + "Session Contact Email: " + pr.ContactEmail + "<br/><br />";

            if (status.Equals("RegisteredCompleted"))
            {
                emailBody += @"<p>Dear Dr. {LastName},</p>
						<p>You are invited to present the accredited CPD program entitled ""{ProgramName}"". We ask that you complete the following two requirements:</p>
						&nbsp;&nbsp;&nbsp;&nbsp;1. Confirm your availability (once confirmed, please close the pop-up and return to this email)<br />
						&nbsp;&nbsp;&nbsp;&nbsp;2. Click on the Select Modules link to select the modules you wish to present based on the program duration<br /><br />"
                        + confirmBlock
                        + selectModules
                        + "Please visit <a href='" + ConfigurationManager.AppSettings["SpeakerBaseURL"] + "'>" + ConfigurationManager.AppSettings["SpeakerBaseURL"] + "</a> to gain access to the program materials.<br/>";
            }
            else if (status.Equals("RegistrationNotComplete"))
            {
                emailBody += @"<p>Dear Dr. {LastName},</p>
						<p>You are invited to present the accredited CPD program entitled ""{ProgramName}"". We ask that you complete the following three requirements:</p>
						&nbsp;&nbsp;&nbsp;&nbsp;1. Confirm your availability (once confirmed, please close the pop-up and return to this email)<br />
						&nbsp;&nbsp;&nbsp;&nbsp;2. Click on the Select Modules link to select the modules you wish to present based on the program duration<br />
						&nbsp;&nbsp;&nbsp;&nbsp;3. Complete the updated CFPC COI form which is available in your speaker resource centre.<br /><br />"
                        + confirmBlock
                        + selectModules
                        + "<p style='color:#CC6600;text-decoration: underline;'>Requirement 3: Complete and upload the updated COI form and access program materials</p>"
                        + "Please visit <a href='" + ConfigurationManager.AppSettings["SpeakerBaseURL"] + "'>" + ConfigurationManager.AppSettings["SpeakerBaseURL"] + "</a> to gain access to the program materials. <span style='color:blue;'>Please complete the updated COI form which is available in the speaker resource centre at your earliest convenience.</span> The Speaker Resource Centre includes all the pertinent program materials for your reference and perusal.<br/>";
            }
            else if (status.Equals("NotRegistered"))
            {
                emailBody += @"<p>Dear Dr. {LastName},</p>
						<p>You are invited to present the accredited CPD program entitled ""{ProgramName}"". We ask that you complete the following three requirements:</p>
						&nbsp;&nbsp;&nbsp;&nbsp;1. Confirm your availability (once confirmed, please close the pop-up and return to this email)<br />
						&nbsp;&nbsp;&nbsp;&nbsp;2. Click on the Select Modules link to select the modules you wish to present based on the program duration<br />
						&nbsp;&nbsp;&nbsp;&nbsp;3. Register to access your speaker portal and complete the required forms (details provided below) <br /><br />"
                        + confirmBlock
                        + selectModules
                        + "<p style='color:#CC6600;text-decoration: underline;'>Requirement 3: Register to access your speaker portal</p>"
                        + "&nbsp;&nbsp;&nbsp;&nbsp;1. Please visit <a href='" + ConfigurationManager.AppSettings["SpeakerActivation"] + "'>" + ConfigurationManager.AppSettings["SpeakerActivation"] + "</a> to register - your username is: " + um.EmailAddress + ".<br />"
                        + "&nbsp;&nbsp;&nbsp;&nbsp;2. Once your registration is complete, please login to your personal resource centre <a href='" + ConfigurationManager.AppSettings["SpeakerBaseURL"] + "'>" + ConfigurationManager.AppSettings["SpeakerBaseURL"] + "</a> and complete the required forms (payee and COI forms) at your earliest convenience.<br />"
                        + "&nbsp;&nbsp;&nbsp;&nbsp;3. Access the Speaker Resource Centre to gain access to all the pertinent program materials for your reference and perusal.<br /><br />"
                        + "&nbsp;&nbsp;&nbsp;&nbsp;&#8226;&nbsp;&nbsp;&nbsp;&nbsp;If you are <span style='color:red'>NOT INTERESTED</span> in participating in this program as a speaker, please visit <a href='" + ConfigurationManager.AppSettings["SpeakerModeratorOptOut"] + "'>" + ConfigurationManager.AppSettings["SpeakerModeratorOptOut"] + "</a> and enter your username: " + um.EmailAddress + ".<br />";
            }

            emailBody += "<br />Should you have any questions, please do not hesitate to contact your CHRC/CCPDHM program coordinator: <br/>"
                        + ConfigurationManager.AppSettings["ProgramCoordinator"] + ". Email address: <a href='mailto:" + ConfigurationManager.AppSettings["CoordinatorEmail"] + "'>" + ConfigurationManager.AppSettings["CoordinatorEmail"]
                        + ".</a>" +"<br/>";



            emailBody = emailBody.Replace("{ProgramRequestID}", pr.ProgramRequestID.ToString());
            emailBody = emailBody.Replace("{LastName}", um.LastName);
            emailBody = emailBody.Replace("{ProgramName}", repo.GetProgramRequestName(pr.ProgramID));



            emailBody = emailBody.Replace("{MealStartTime}", pr.MealStartTime);
            emailBody = emailBody.Replace("{SessionStartTime}", pr.ProgramStartTime);
            emailBody = emailBody.Replace("{SessionEndTime}", pr.ProgramEndTime);


            emailBody = emailBody.Replace("{LocationName}", pr.LocationName);
            emailBody = emailBody.Replace("{Honorarium}", um.SpeakerHonariumRange);
            emailBody = emailBody.Replace("{ModeratorName}", moderatorName);

            try
            {
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(um.EmailAddress));
                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]));
                mailMessage.Subject = programName + " - Invitation to Present - Program Date: " + pr.ProgramDate1 + " - PLEASE REVIEW AND RESPOND";
                mailMessage.IsBodyHtml = true;

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(emailBody, null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
                {
                    string FileName = GetFileNameForEmailAttachment(pr.SessionAgendaFileName, pr.ProgramRequestID);
                    mailMessage.Attachments.Add(new Attachment(controller.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserFileUploadPath"] + FileName)));
                }
                UserHelper.SendMail(mailMessage);
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }
        }


        public static string ConvertTimetoProperFormat(string date)
        {
            string retDate = "";

            if (!string.IsNullOrEmpty(date))
            {
                DateTime dt = DateTime.ParseExact(date, "yyyy/MM/dd", null);
                retDate = dt.ToLongDateString();
            }

            return retDate;
        }

        public static bool IsEmailValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static void SendActivationEmail(string FirstName, string email, string password)
        {

            try
            {
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();

                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(email));
                mailMessage.Subject = "Welcome to your personal CPD Portal: Your Account has been Successfully Activated";

                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(GetActivationEmailBody(FirstName, email, password), null, "text/html");

                mailMessage.AlternateViews.Add(htmlView);
                mailMessage.Attachments.Add(new Attachment(HttpContext.Current.Server.MapPath("~/PDF/Cookies.pdf")));
                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {

                string msg = e.Message;


            }


        }
        private static string GetActivationEmailBody(string FirstName, string userName, string password)
        {
            string html = string.Empty;

            html = Resources.Resource.ActivationEmail;

            html = html.Replace("{FirstName}", FirstName);
            html = html.Replace("{userName}", userName);
            html = html.Replace("{password}", password);



            return html;

        }
        public static void FromSpeakerToModerator(ProgramRequest pr, UserModel um, string ProgramName, string ChosenDate, string SessionCredits)
        {
            try
            {
                //string date1 = ConvertTimetoProperFormat(pr.ProgramDate1);

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(um.EmailAddress));
                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]));
                mailMessage.Subject = "From Speaker to Moderator";
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(FromSpeakerToModeratorBody(pr, um, ProgramName, ChosenDate, SessionCredits), null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);
                if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
                {
                    string FileName = GetFileNameForEmailAttachment(pr.SessionAgendaFileName, pr.ProgramRequestID);
                    mailMessage.Attachments.Add(new Attachment(HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserFileUploadPath"] + FileName)));
                }
                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
        public static void FromSpeakerToModeratorfr(ProgramRequest pr, UserModel um, string ProgramName, string ChosenDate, string SessionCredits)
        {
            try
            {
                //string date1 = ConvertTimetoProperFormat(pr.ProgramDate1);

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(um.EmailAddress));
                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]));
                mailMessage.Subject = "De Conférencier à Modérateur";
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(FromSpeakerToModeratorBodyfr(pr, um, ProgramName, ChosenDate, SessionCredits), null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);
                if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
                {
                    string FileName = GetFileNameForEmailAttachment(pr.SessionAgendaFileName, pr.ProgramRequestID);
                    mailMessage.Attachments.Add(new Attachment(HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserFileUploadPath"] + FileName)));
                }
                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }

        private static string FromSpeakerToModeratorBody(ProgramRequest pr, UserModel um, string ProgramName, string ChosenDate, string SessionCredits)
        {
            string Materials = "";
            string ModeratorConfirmUrl = System.Configuration.ConfigurationManager.AppSettings["ModeratorConfirmUrl"];

            if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
            {
                Materials = Path.GetFileName(pr.SessionAgendaFileName);


            }
            string html = string.Empty;
            html = @"<table width = '900' border = '0' cellspacing = '0' cellpadding = '10'>
				<tr>                
				<td align = 'center'><strong style = 'font-size: 30px; font-family: Arial, Helvetica, sans-serif;'> Speaker to Moderator </strong></td>                         
				 </tr>                       
		  
				 <tr>                         
				<td align = 'left'> 					
					
				<br /> 
		
				<p>Dear {LastName}, </p> 
				<p>You are invited to moderate the accredited CPD program entitled {ProgramName}. 
				Please review the details and confirm your availability by clicking the Confirm or Not Available button </p> <br />
			
					<table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							<a style='display: block;color: #ffffff;font-size: 10px;text-decoration: none;' 
							href='{ModeratorConfirmUrl}{ProgramRequestID}&ProgramDate={ProgramDate}'>
								Confirm
							</a>
														
						</td>
													
						<td>
														
								&nbsp;&nbsp;&nbsp;&nbsp;Program Date : {ProgramDate} <br /> 
								&nbsp;&nbsp;&nbsp;&nbsp;Meal Start Time : {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Session Start Time : {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Session End Time : {SessionEndTime}
						</td>
					</tr>
				</table>
			
											
				<table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							<a style='display:block; color: #ffffff; font-size: 10px;' href='{ModeratorConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
									Not Available
								</a>
 
							</td>
							<td>
									&nbsp; &nbsp; &nbsp; &nbsp; Not Available

						</td>

					</tr>

				</table>


				<hr>


			Program Location:</strong> <span><strong> {LocationName} </strong> &nbsp; </span > &nbsp;<br />       
			Materials to be presented: </strong> <span><strong> {SessionCredits} </strong> &nbsp; </span> &nbsp;<br />
			Your Honorarium:</strong> <span><strong> {Honorarium} </strong> &nbsp; </span> &nbsp;<br />
			Session Contact: if applicable </strong> <span><strong> {SessionContact} </strong> &nbsp; </span> &nbsp;<br />
			Session Contact Number:</strong> <span><strong>{ContactPhoneNumber} </strong> &nbsp; </span> &nbsp;<br />
			Session Contact Address:</strong> <span><strong>{ContactEmailAddress} </strong> &nbsp; </span > &nbsp;<br />                                                                     

				
			
			Should you have any questions, please do not hesitate to contact your CHRC/CCPDHM program coordinator: {SessionContact} – email address: {ContactEmailAddress}
			phone number: {ContactPhoneNumber}

		</p>


		</td>

	</tr>


		</table> ";

            html = html.Replace("{ProgramRequestID}", pr.ProgramRequestID.ToString());
            html = html.Replace("{ProgramDate}", ChosenDate);
            html = html.Replace("{LastName}", um.LastName);
            html = html.Replace("{ProgramName}", ProgramName);
            html = html.Replace("{MealStartTime}", pr.MealStartTime);
            html = html.Replace("{SessionStartTime}", pr.ProgramStartTime);
            html = html.Replace("{SessionEndTime}", pr.ProgramEndTime);

            html = html.Replace("{LocationName}", pr.LocationName);
            html = html.Replace("{AdditionalPresenter}", "AdditionalPresenter");
            html = html.Replace("{Materials}", Materials);
            html = html.Replace("{SessionContact}", pr.ContactName);
            html = html.Replace("{ContactPhoneNumber}", pr.ContactPhone);
            html = html.Replace("{ContactEmailAddress}", pr.ContactEmail);
            html = html.Replace("{ModeratorConfirmUrl}", ModeratorConfirmUrl);
            html = html.Replace("{Honorarium}", um.ModeratorHonariumRange);
            html = html.Replace("{SessionCredits}", SessionCredits);


            return html;

        }
        private static string FromSpeakerToModeratorBodyfr(ProgramRequest pr, UserModel um, string ProgramName, string ChosenDate, string SessionCredits)
        {
            string Materials = "";
            string ModeratorConfirmUrl = System.Configuration.ConfigurationManager.AppSettings["ModeratorConfirmUrl"] + pr.ProgramRequestID;

            if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
            {
                Materials = Path.GetFileName(pr.SessionAgendaFileName);


            }
            string html = string.Empty;
            html = @"<table width = '900' border = '0' cellspacing = '0' cellpadding = '10'>
				<tr>                
				<td align = 'center'><strong style = 'font-size: 30px; font-family: Arial, Helvetica, sans-serif;'> De Conférencier à Modérateur </strong></td>                         
				 </tr>                       
		  
				 <tr>                         
				<td align = 'left'> 					
					
				<br /> 
		
				<p>Cher Dr. {LastName}, </p> 
				<p>Vous êtes invité comme Modérateur pour le programme de Formation Professionnelle Continue intitulé  {ProgramName}. 
				Veuillez svp prendre connaissance des informations ci-jointes et nous confirmer votre disponibilité en cliquant le bouton Confirmer ou Non Disponible. </p> <br />
			
					<table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							<a style='display: block;color: #ffffff;font-size: 10px;text-decoration: none;' 
							href='{ModeratorConfirmUrl}{ProgramRequestID}&ProgramDate={ProgramDate}'>
								Confirmer
							</a>
														
						</td>
													
						<td>
														
								&nbsp;&nbsp;&nbsp;&nbsp;Date du Programme : {ProgramDate} <br /> 
								&nbsp;&nbsp;&nbsp;&nbsp;Début du repas : {MealStartTime} &nbsp;&nbsp;&nbsp;&nbsp;Début de session : {SessionStartTime} &nbsp;&nbsp;&nbsp;&nbsp; Fin de session : {SessionEndTime}
						</td>
					</tr>
				</table>
			
											
				<table>
					<tr>
						<td style='background-color: #4ecdc4; padding:0px 2px; text-align: center;'>
							<a style='display:block; color: #ffffff; font-size: 10px;' href='{ModeratorConfirmUrl}{ProgramRequestID}&ProgramDate=NotAvailable' >
									Non Disponible
								</a>
 
							</td>
							<td>
									&nbsp; &nbsp; &nbsp; &nbsp; Non Disponible

						</td>

					</tr>

				</table>


				<hr>


			Lieu du Programme:</strong> <span><strong> {LocationName} </strong> &nbsp; </span > &nbsp;<br />       
			Contenu / Matériel scientifique à présenter: </strong> <span><strong> {SessionCredits} </strong> &nbsp; </span> &nbsp;<br />
			Vos Honoraires:</strong> <span><strong> {Honorarium} </strong> &nbsp; </span> &nbsp;<br />
			Personne-contact pour la session: si applicable   </strong> <span><strong> {SessionContact} </strong> &nbsp; </span> &nbsp;<br />
			Numéro de la session:</strong> <span><strong>{ContactPhoneNumber} </strong> &nbsp; </span> &nbsp;<br />
			Adresse courriel pour la session:</strong> <span><strong>{ContactEmailAddress} </strong> &nbsp; </span > &nbsp;<br />                                                                     

		    Veuillez consulter le site <a href=""https://speaker.ccpdhm.com"">speaker.ccpdhm.com</a> pour avoir accès au contenu / matériel scientifique du Programme
			
			Pour toute question, n’hésitez pas à contacter votre coordinatrice de programme CCRC/CIDPSM: {SessionContact} – à l’adresse courriel suivante: {ContactEmailAddress}
			

		</p>


		</td>

	</tr>


		</table> ";

            html = html.Replace("{ProgramRequestID}", pr.ProgramRequestID.ToString());
            html = html.Replace("{ProgramDate}", ChosenDate);
            html = html.Replace("{LastName}", um.LastName);
            html = html.Replace("{ProgramName}", ProgramName);
            html = html.Replace("{MealStartTime}", pr.MealStartTime);
            html = html.Replace("{SessionStartTime}", pr.ProgramStartTime);
            html = html.Replace("{SessionEndTime}", pr.ProgramEndTime);

            html = html.Replace("{LocationName}", pr.LocationName);
            html = html.Replace("{AdditionalPresenter}", "AdditionalPresenter");
            html = html.Replace("{Materials}", Materials);
            html = html.Replace("{SessionContact}", System.Configuration.ConfigurationManager.AppSettings["ProgramCoordinator"]);
            html = html.Replace("{ContactPhoneNumber}", pr.ContactPhone);
            html = html.Replace("{ContactEmailAddress}", System.Configuration.ConfigurationManager.AppSettings["CoordinatorEmail"]);
            html = html.Replace("{ModeratorConfirmUrl}", ModeratorConfirmUrl);
            html = html.Replace("{Honorarium}", um.ModeratorHonariumRange);
            html = html.Replace("{SessionCredits}", SessionCredits);


            return html;

        }
        public static void FromModeratorToAdmin(ProgramRequest pr, UserModel um, string ProgramName, string ChosenDate)
        {
            try
            {


                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["AdminEmail"]));
                mailMessage.Subject = "From Moderator to Admin";
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(FromModeratorToAdminBody(pr, um, ProgramName, ChosenDate), null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
        private static string FromModeratorToAdminBody(ProgramRequest pr, UserModel um, string ProgramName, string ChosenDate)
        {
            string html = string.Empty;

            if (ChosenDate.Equals("NotAvailable"))
            {

                html = @" <style>
					   
						.emailBodyWrapper
						{
							padding: 5px;
							font-family: Candara;
							 font-size:14px;
						}
					 li
							{
							  padding-top:28px;   
							}
					</style>
					<div class='emailBodyWrapper'>
						<p>
							Dear Admin,
						</p>
						<p>
						   Please note that the selected moderator, Dr. {FirstName} {LastName}, is not available for the session date you had requested. 
						</p>
						
					   
					   
					</div>";



            }
            else
            {


                html = @" <style>
					   
						.emailBodyWrapper
						{
							padding: 5px;
							font-family: Candara;
							 font-size:14px;
						}
					 li
							{
							  padding-top:28px;   
							}
					</style>
					<div class='emailBodyWrapper'>
						<p>
							  Dear Admin,
						</p>
						<p>
						   Please note that the selected moderator, Dr. {FirstName} {LastName}, is  available for {ChosenDate} date.
						</p>
												
					   
					</div>";

            }



            html = html.Replace("{ProgramRequestID}", pr.ProgramRequestID.ToString());

            html = html.Replace("{ChosenDate}", ChosenDate);
            html = html.Replace("{LastName}", um.LastName);
            html = html.Replace("{FirstName}", um.FirstName);
            html = html.Replace("{ProgramName}", ProgramName);
            html = html.Replace("{ContactFirstName}", pr.ContactFirstName);



            return html;

        }
        public static void FromModeratorToSalesRep(ProgramRequest pr, UserModel um, string ProgramName, string ChosenDate)
        {
            try
            {


                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(pr.ContactEmail));
                if (ChosenDate.Equals("NotAvailable"))
                {
                    mailMessage.Subject = "Moderator Changes Required " + ProgramName + "  – Session Request: " + ChosenDate;
                }
                else
                {
                    mailMessage.Subject = "Moderator Selected " + ProgramName + "  – Session Date: " + ChosenDate;

                }
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(FromModeratorToSalesRepBody(pr, um, ProgramName, ChosenDate), null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
        private static string FromModeratorToSalesRepBody(ProgramRequest pr, UserModel um, string ProgramName, string ChosenDate)
        {
            string html = string.Empty;

            if (ChosenDate.Equals("NotAvailable"))
            {

                html = @" <style>
					   
						.emailBodyWrapper
						{
							padding: 5px;
							font-family: Candara;
							 font-size:14px;
						}
					 li
							{
							  padding-top:28px;   
							}
					</style>
					<div class='emailBodyWrapper'>
						<p>
							Dear {ContactFirstName},
						</p>
						<p>
						   Please note that the selected moderator, Dr. {FirstName} {LastName}, is not available for the session date you had requested. 
						</p>
						
						<p>
						   Next Steps:
						   
						<p>
							1.	Please log in to your portal: https://amgen.ccpdhm.com  <br />
							2.	Visit your dashboard for the Program Name <br />
							3.	Click on the <strong>pencil</strong> icon under the <strong>Full Session Details</strong> column  <br />
							4.	Select a different moderator from the drop-down menu  <br /> 
						</p>
						
						<p>
							Please do not hesitate to contact us should you have any questions or require any assistance. <br />
							E: info@ccpdhm.com
						</p>
					   
					</div>";




            }
            else
            {


                html = @" <style>
					   
						.emailBodyWrapper
						{
							padding: 5px;
							font-family: Candara;
							 font-size:14px;
						}
					 li
							{
							  padding-top:28px;   
							}
					</style>
					<div class='emailBodyWrapper'>
						<p>
							 Dear {ContactFirstName},
						</p>
						<p>
						   Please note that the selected moderator, Dr. {FirstName} {LastName}, is  available for {ChosenDate} date.
						</p>
												
					   
					</div>";

            }



            html = html.Replace("{ProgramRequestID}", pr.ProgramRequestID.ToString());

            html = html.Replace("{ChosenDate}", ChosenDate);
            html = html.Replace("{LastName}", um.LastName);
            html = html.Replace("{FirstName}", um.FirstName);
            html = html.Replace("{ProgramName}", ProgramName);
            html = html.Replace("{ContactFirstName}", pr.ContactFirstName);



            return html;



        }
        public static void FromSpeakerToSalesRep(ProgramRequest pr, UserModel um, string ProgramName, string ChosenDate)
        {
            try
            {


                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                //	mailMessage.From = new System.Net.Mail.MailAddress(um.EmailAddress);

                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(pr.ContactEmail));

                if (ChosenDate.Equals("NotAvailable"))
                {
                    mailMessage.Subject = "Speaker Changes Required " + ProgramName + "  – Session Request: " + ChosenDate;
                }
                else
                {
                    mailMessage.Subject = "Speaker Selected " + ProgramName + "  – Session Date: " + ChosenDate;

                }

                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(FromSpeakerToSalesRepBody(pr, um, ProgramName, ChosenDate), null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
        private static string FromSpeakerToSalesRepBody(ProgramRequest pr, UserModel um, string ProgramName, string ChosenDate)
        {
            string html = string.Empty;

            if (ChosenDate.Equals("NotAvailable"))
            {

                html = @" <style>
					   
						.emailBodyWrapper
						{
							padding: 5px;
							font-family: Candara;
							 font-size:14px;
						}
					 li
							{
							  padding-top:28px;   
							}
					</style>
					<div class='emailBodyWrapper'>
						<p>
							Dear {ContactFirstName},
						</p>
						<p>
						   Please note that the selected speaker, Dr. {FirstName} {LastName}, is not available for the session date you had requested. 
						</p>
						
						<p>
						   Next Steps:
						   
						<p>
							1.	Please log in to your portal: https://amgen.ccpdhm.com  <br />
							2.	Visit your dashboard for the Program Name <br />
							3.	Click on the <strong>pencil</strong> icon under the <strong>Full Session Details</strong> column  <br />
							4.	Select a different session date or a different speaker from the drop-down menu   <br /> 
						</p>
						
						<p>
							Please do not hesitate to contact us should you have any questions or require any assistance. <br />
							E: info@ccpdhm.com
						</p>
					   
					</div>";




            }
            else
            {


                html = @" <style>
					   
						.emailBodyWrapper
						{
							padding: 5px;
							font-family: Candara;
							 font-size:14px;
						}
					 li
							{
							  padding-top:28px;   
							}
					</style>
					<div class='emailBodyWrapper'>
						<p>
							 Dear {ContactFirstName},
						</p>
						<p>
						   Please note that the selected speaker, Dr. {FirstName} {LastName}, is  available for {ChosenDate} date.
						</p>
												
					   
					</div>";

            }



            html = html.Replace("{ProgramRequestID}", pr.ProgramRequestID.ToString());

            html = html.Replace("{ChosenDate}", ChosenDate);
            html = html.Replace("{LastName}", um.LastName);
            html = html.Replace("{FirstName}", um.FirstName);
            html = html.Replace("{ProgramName}", ProgramName);
            html = html.Replace("{ContactFirstName}", pr.ContactFirstName);



            return html;


        }
        public static void FromSpeakerToAdmin(ProgramRequest pr, UserModel um, string ProgramName, string ChosenDate)
        {
            try
            {



                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                //mailMessage.From = new System.Net.Mail.MailAddress(um.EmailAddress);

                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["AdminEmail"]));
                mailMessage.Subject = "From Speaker to Admin";
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(FromSpeakerToAdminBody(pr, um, ProgramName, ChosenDate), null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
        private static string FromSpeakerToAdminBody(ProgramRequest pr, UserModel um, string ProgramName, string ChosenDate)
        {
            string html = string.Empty;

            if (ChosenDate.Equals("NotAvailable"))
            {

                html = @" <style>
					   
						.emailBodyWrapper
						{
							padding: 5px;
							font-family: Candara;
							 font-size:14px;
						}
					 li
							{
							  padding-top:28px;   
							}
					</style>
					<div class='emailBodyWrapper'>
						<p>
							Dear Admin,
						</p>
						<p>
						   Please note that the selected speaker, Dr. {FirstName} {LastName}, is not available for the session date you had requested. 
						</p>
						
					   
					   
					</div>";



            }
            else
            {


                html = @" <style>
					   
						.emailBodyWrapper
						{
							padding: 5px;
							font-family: Candara;
							 font-size:14px;
						}
					 li
							{
							  padding-top:28px;   
							}
					</style>
					<div class='emailBodyWrapper'>
						<p>
							  Dear Admin,
						</p>
						<p>
						   Please note that the selected speaker, Dr. {FirstName} {LastName}, is  available for {ChosenDate} date.
						</p>
												
					   
					</div>";

            }



            html = html.Replace("{ProgramRequestID}", pr.ProgramRequestID.ToString());

            html = html.Replace("{ChosenDate}", ChosenDate);
            html = html.Replace("{LastName}", um.LastName);
            html = html.Replace("{FirstName}", um.FirstName);
            html = html.Replace("{ProgramName}", ProgramName);
            html = html.Replace("{ContactFirstName}", pr.ContactFirstName);



            return html;
        }

        public static void AdminToSalesRep(string LocationName, UserModel um, string ProgramName, string ChosenDate)
        {
            try
            {


                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(um.EmailAddress));
                mailMessage.Subject = "Changes Required" + ProgramName + " – Session Request:  " + ChosenDate;
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(AdminToSalesRepBody(LocationName, um, ProgramName, ChosenDate), null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
        private static string AdminToSalesRepBody(String LocationName, UserModel um, string ProgramName, string ChosenDate)
        {
            string html = string.Empty;

            html = @" <style>
					   
						.emailBodyWrapper
						{
							padding: 5px;
							font-family: Candara;
							 font-size:14px;
						}
					 li
							{
							  padding-top:28px;   
							}
					</style>
					<div class='emailBodyWrapper'>
						<p>
							Dear  {LastName},
						</p>
						<p>
							Please note that the selected venue, {VenueName}, is not available for the session date you had requested. 
						</p>
						
						<p>
						   Next Steps:
						   
						<p>
							1.	Please log in to your portal: https://amgen.ccpdhm.com  <br />
							2.	Visit your dashboard for the Program Name <br />
							3.	Click on the <strong>pencil</strong> icon under the <strong>Full Session Details</strong> column  <br />
							4.	Provide alternative venue details for this session <br /> 
						</p>
						
						<p>
							Please do not hesitate to contact us should you have any questions or require any assistance. <br />
							E: info@ccpdhm.com
						</p>
					   
					</div>";






            html = html.Replace("{ChosenDate}", ChosenDate);
            html = html.Replace("{LastName}", um.LastName);
            html = html.Replace("{VenueName}", LocationName);


            return html;

        }
        public static void EmailSalesRepWhenAdminMakeChanges(ProgramRequest pr, UserModel SalesRep, string ModeratorName, string SessionCredit, string ProgramName)
        {

            try
            {

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(SalesRep.EmailAddress));
                mailMessage.Subject = ProgramName + "  Certification Request - ProgramDate " + pr.ProgramDate1;
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(EmailSalesRepWhenAdminMakeChangesBody(pr, SalesRep.LastName, ModeratorName, SessionCredit), null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);
                if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
                {
                    string FileName = GetFileNameForEmailAttachment(pr.SessionAgendaFileName, pr.ProgramRequestID);
                    mailMessage.Attachments.Add(new Attachment(HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserFileUploadPath"] + FileName)));
                }

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }


        private static string EmailSalesRepWhenAdminMakeChangesBody(ProgramRequest pr, string speakername, string ModeratorName, string SessionCredit)
        {
            string html = string.Empty;

            string Materials = "";

            if (!string.IsNullOrEmpty(pr.SessionAgendaFileName))
            {
                Materials = Path.GetFileName(pr.SessionAgendaFileName);
            }



            html = Resources.Resource.SaleRepEmailText;

            html = html.Replace("{RecordID}", pr.ProgramRequestID.ToString());
            html = html.Replace("{DateSubmitted}", DateTime.Now.ToString());
            html = html.Replace("{SessionContact}", pr.ContactName);
            html = html.Replace("{ContactPhoneNumber}", pr.ContactPhone);
            html = html.Replace("{ContactEmailAddress}", pr.ContactEmail);
            html = html.Replace("{SessionDate1}", ConvertTimetoProperFormat(pr.ProgramDate1));
            html = html.Replace("{SessionDate2}", ConvertTimetoProperFormat(pr.ProgramDate2));
            html = html.Replace("{SessionDate3}", ConvertTimetoProperFormat(pr.ProgramDate3));
            html = html.Replace("{MealStartTime}", pr.MealStartTime);
            html = html.Replace("{SessionStartTime}", pr.ProgramStartTime);
            html = html.Replace("{SessionEndTime}", pr.ProgramEndTime);
            html = html.Replace("{SessionCredits}", SessionCredit);
            html = html.Replace("{MultiSessionEvent}", pr.MultiSession.ToString());
            html = html.Replace("{CopyofAgenda}", Materials);
            html = html.Replace("{CostsbyParticipants}", pr.CostPerparticipants.ToString());
            html = html.Replace("{SessionSpeaker}", speakername);
            html = html.Replace("{SessionSpeakerStatus}", "");
            html = html.Replace("{SessionModerator}", ModeratorName);

            html = html.Replace("{SessionModeratorStatus}", "");
            html = html.Replace("{VenueContacted}", pr.VenueContacted);
            html = html.Replace("{LocationType}", pr.LocationType);


            html = html.Replace("{LocationName}", pr.LocationName);

            html = html.Replace("{Address}", pr.LocationAddress);

            html = html.Replace("{City}", pr.LocationCity);

            html = html.Replace("{Province}", pr.LocationProvince);

            html = html.Replace("{PhoneNumber}", pr.LocationPhoneNumber);

            html = html.Replace("{Website}", pr.LocationWebsite);

            html = html.Replace("{MealType}", pr.MealType);

            html = html.Replace("{CostPerPerson}", pr.CostPerPerson.ToString());

            html = html.Replace("{AudioVisual}", pr.AVEquipment);

            html = html.Replace("{AdditionalInfo}", pr.Comments);





            return html;

        }

        public static void AdminChangeRequestStatusID2(StatusChangeEmailViewModel sc)
        {
            try
            {


                string html = string.Empty;
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(sc.Email));
                mailMessage.Subject = sc.ProgramName + "  – Session Request: " + sc.ProgramDate + " – Your Event Information has been submitted for Regional Ethics Review";
                mailMessage.IsBodyHtml = true;
                html = @"<table width = '900' border = '0' cellspacing = '0' cellpadding = '10'>
				<tr>                
				<td>Dear {FirstName}, </td>
									   
				 </tr>  
			
				 <tr>
				<td>Your <b>{ProgramName}</b> event which will be hosted on <b>{Date}</b> has been submitted to the CFPC chapter for regional ethics review </td>
				 <tr>

				<td> 
					<tr> You may now initiate recruitment utilizing the National Invitation Template.   </tr>
				</td>



				<tr>

				<b>Next Steps: </b> <br/> <br/>

				<td align='left'>
						1.	Please log in to your portal: https://amgen.ccpdhm.com<br/>
						2.	Access the Program Materials tab and download the modifiable National Invitation Template pdf<br/>
						3.	Follow the instructions to complete the pertinent fields and print your invitations<br/>
						
				</td>

				</tr>  
					   
				

			</table> ";
                html = html.Replace("{FirstName}", sc.FirstName);
                html = html.Replace("{ProgramName}", sc.ProgramName);
                html = html.Replace("{Date}", sc.ProgramDate);






                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }

        public static void AdminChangeRequestStatusID3(StatusChangeEmailViewModel sc)
        {
            try
            {


                string html = string.Empty;
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(sc.Email));
                mailMessage.Subject = sc.ProgramName + "  – Session Request: " + sc.ProgramDate + " – Your event has received regional ethics approval ";
                mailMessage.IsBodyHtml = true;
                html = @"<table width = '900' border = '0' cellspacing = '0' cellpadding = '10'>
				<tr>                
				<td>Dear {FirstName}, </td>
									   
				 </tr>  
			
				 <tr>
				<td>Your <b>{ProgramName}</b> event which will be hosted on <b>{Date}</b> has received regional ethics approval.  Please note your event ID: <b> {EventId} </b> – the event ID is to be included in the Certificate of Attendance.  </td>
				 <tr>

				<td> 
					<tr> You may continue recruitment utilizing the Regional Invitation Template and download the remainder of the program materials   </tr>
				</td>



				<tr>

				<b>Next Steps: </b> <br/> <br/>

				<td align='left'>
						1.	Please log in to your portal: https://amgen.ccpdhm.com<br/>
						2.	Access the Program Materials tab and download the modifiable Regional Invitation Template pdf., Certificate of Attendance, Evaluation Forms and the Sign-In Sheet. <br/>
						3.	Follow the instructions to complete the pertinent fields and print your materials<br/>
						
				</td>

				</tr>  
					   
				

			</table> ";
                html = html.Replace("{FirstName}", sc.FirstName);
                html = html.Replace("{ProgramName}", sc.ProgramName);
                html = html.Replace("{EventId}", sc.EventID);
                html = html.Replace("{Date}", sc.ProgramDate);






                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }
        public static string ConvertToFrenchDate(string EnglishDate)

        {
            StringBuilder sb = new StringBuilder(EnglishDate);
            sb.Replace("January", "Janvier").Replace("February", "Février").Replace("March", "Mars").Replace("April", "Avril").Replace("May", "Mai").Replace("June", "Juin").Replace("July", "Juillet").Replace("August", "Aaoût").Replace("September", "Septembre").Replace("October", "Octobre").Replace("November", "Novembre").Replace("December", "Décembre")
                .Replace("Monday","Lundi").Replace("Tuesday", "Mardi ").Replace("Wednesday", "Mercredi ").Replace("Thursday", "Jeudi ").Replace("Friday", "Vendredi ").Replace("Saturday", "Samedi").Replace("Sunday", "Dimanche");
         
            return sb.ToString();
        }
        public static void AdminChangeRequestStatusID5(StatusChangeEmailViewModel sc)
        {
            try
            {


                string html = string.Empty;
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailGeneral"]);
                mailMessage.To.Add(new System.Net.Mail.MailAddress(sc.Email));
                mailMessage.Subject = sc.ProgramName + " – " + sc.ProgramDate + " – Please upload the post-program materials";
                mailMessage.IsBodyHtml = true;
                html = @"
						<style>
					   
						.moveright
						{
							margin-left:25px;
						}
					 
					</style>

				<table width = '900' border = '0' cellspacing = '0' cellpadding = '10'>
				<tr>                
				<td>Dear {FirstName}, </td>
									   
				 </tr>  
			
				 <tr>
				<td>We hope that your <b>{ProgramName}</b> on <b>{ProgramDate}</b> was a success and of educational value to the physicians in attendance. </td>
				 <tr>

				<td> 
					<tr>Please upload the required program materials within 5 business days of today’s date.  </tr>
				</td>

				<tr>

				<b>Next Steps: </b> <br/> <br/>

				<td align='left'>
						1.	Please log in to your portal: https://amgen.ccpdhm.com.<br/>
						2.	Visit your dashboard for the {ProgramName} . <br/>
						3.	Click the “upload” icon under the Post-Event Amgen column header.<br/>
						4.	Please submit the completed Evaluation Forms, Sign-In Sheet and any other pertinent documents. <b>Alternatively you may submit the materials as following: </b> <br/><br />
								
							<p class='moveright'> <b> a. Email </b>  email the completed forms to amgen@ccpdhm.com – please include your event date in the subject line </p> 
							<p class='moveright'> <b> b. Fax: </b> fax the completed forms to 416-977-8020 or 1-800-238-5335 – attention Amgen CPD Programs </p>  
							<p class='moveright'> <b> c. Mail:</b> CHRC c/o Amgen CPD 110 Sheppard Avenue East, Suite 309, North York, ON, M2N 6Y8</p>
				</td>

				</tr>  
					   
				

			</table> ";
                html = html.Replace("{FirstName}", sc.FirstName);
                html = html.Replace("{ProgramName}", sc.ProgramName);
                html = html.Replace("{ProgramDate}", sc.ProgramDate);



                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                UserHelper.SendMail(mailMessage);
            }

            catch (Exception e)
            {
                string msg = e.Message;
            }
        }


        #endregion

    }
}