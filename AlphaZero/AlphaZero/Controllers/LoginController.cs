using AlphaZero.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.Remoting;
using System.Web;
using System.Web.Mvc;
using System.Net.PeerToPeer;

namespace AlphaZero.Controllers
{
    public class LoginController : Controller
    {

        db_roomrentalEntities db = new db_roomrentalEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        //forgot password
        public ActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(user objchk)
        {
            if (ModelState.IsValid)
            {

                var obj = db.users.Where(a => a.user_name.Equals(objchk.user_name) && a.user_password.Equals(objchk.user_password)).FirstOrDefault();

                if (obj != null)
                {
                    Session["UserID"] = obj.user_id;
                    Session["UserName"] = obj.user_name.ToString();
                    Session["UserType"] = obj.user_type.ToString();

                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "The Username or Password Incorrect");
                }

            }

            return View();
        }


        [HttpPost]
        public ActionResult ForgotPassword(string user_email)
        {
            //Verify Email ID
            //Generate Reset Password Link
            //Send email

            string message = "";


            var account = db.users.Where(u => u.user_email == user_email).FirstOrDefault();

            if (account != null)
            {
                //send email to reset password
                //generate unique identification number
                string resetCode = Guid.NewGuid().ToString();
                SendEmail(account.user_email, resetCode, "ResetPassword");//method
                account.u_forgotpwdCode = resetCode;

                //avid confirm password not match issue
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                message = "Reset password link has been sent to your email address";
            }
            else
            {
                message = "Account not found";
            }

            ViewBag.Message = message;
            return View();
        }


        [NonAction]
        public void SendEmail(string emailID, string activationCode, string emailFor = "ResetPassword")
        {
            var verifyUrl = "/Login/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("akmalrentalsalphazero@gmail.com", "Akmal Rentals");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "otubezrpgcnrvxnl"; // Replace with actual password

            string subject = "";
            string body = "";

            string logoImageUrl = "https://i.imgur.com/j3xnJzi.png";
            string backgroundImageUrl = "https://i.imgur.com/4Q9bX9h.png";

            if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = $@"
                    
            <html lang='en' xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:v='urn:schemas-microsoft-com:vml'>
                        <head>
                            <title></title>
                            <meta charset='utf-8'/>
                            <meta content='width=device-width, initial-scale=1.0' name='viewport'/>
                            <!--[if mso]><xml><o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch><o:AllowPNG/></o:OfficeDocumentSettings></xml><![endif]-->
                            <style>
                                * {{
                                    box-sizing: border-box;
                                }}

                                body {{
                                    margin: 0;
                                    padding: 0;
                                }}

                                a[x-apple-data-detectors] {{
                                    color: inherit !important;
                                    text-decoration: inherit !important;
                                }}

                                #MessageViewBody a {{
                                    color: inherit;
                                    text-decoration: none;
                                }}

                                p {{
                                    line-height: inherit
                                }}

                                @media (max-width:690px) {{
                                    .icons-inner {{
                                        text-align: center;
                                    }}

                                    .icons-inner td {{
                                        margin: 0 auto;
                                    }}

                                    .row-content {{
                                        width: 100% !important;
                                    }}

                                    .stack .column {{
                                        width: 100%;
                                        display: block;
                                    }}
                                }}
                            </style>
                        </head>
                        <body style='background: url({backgroundImageUrl}) center / cover; margin: 0; padding: 0; -webkit-text-size-adjust: none; text-size-adjust: none;'>
                            <table border='0' cellpadding='0' cellspacing='0' class='nl-container' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt; background: url({backgroundImageUrl}) center / cover;' width='100%'>
                                <tbody>
                                    <tr>
                                        <td>
                                            <table align='center' border='0' cellpadding='0' cellspacing='0' class='row row-1' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt;' width='100%'>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <table align='center' border='0' cellpadding='0' cellspacing='0' class='row-content stack' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #ffffff; color: #000000; width: 670px;' width='670'>
                                                                <tbody>
                                                                    <tr>
                                                                        <td class='column' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px none #000000; border-bottom: 0px none #000000; background-color: transparent;' valign='top'>
                                                                            <table align='center' border='0' cellpadding='0' cellspacing='0' class='icons-inner' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt;' width='100%'>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td align='middle' style='padding-bottom: 10px;' valign='middle'>
                                                                                            <img alt='Logo' class='icon' height='32' src='{logoImageUrl}' style='outline: none; text-decoration: none; -ms-interpolation-mode: bicubic; height: auto; border: none; display: block;' title='Logo' width='null'/>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table align='center' border='0' cellpadding='0' cellspacing='0' class='row row-2' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt;' width='100%'>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <table align='center' border='0' cellpadding='0' cellspacing='0' class='row-content stack' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #ffffff; color: #000000; width: 670px;' width='670'>
                                                                <tbody>
                                                                    <tr>
                                                                        <td class='column' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px none #000000; border-bottom: 0px none #000000; background-color: transparent;' valign='top'>
                                                                            <table align='center' border='0' cellpadding='0' cellspacing='0' class='icons-inner' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt; margin: 0 auto;' width='100%'>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style='padding: 20px; text-align: center;'>
                                                                                            <h3 style='text-align: center;'>Hello, We received a request to change your password</h3>
                                                                                            <p style='text-align: center;'>Click on the link below, to continue to reset your password</p>
                                                                                            
                                                                                            <p style='text-align: center;'>Link: {link}</p>
                                                                                            <br>
                                                                                            <p style='text-align: center;'>Follow us: https://www.facebook.com/AkmalBilikSewa</p>

                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>

                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table align='center' border='0' cellpadding='0' cellspacing='0' class='row row-3' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt;' width='100%'>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <table align='center' border='0' cellpadding='0' cellspacing='0' class='row-content stack' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #ffffff; color: #000000; width: 670px;' width='670'>
                                                                <tbody>
                                                                    <tr>
                                                                        <td class='column' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px none #000000; background-color: transparent;' valign='top'>
                                                                            <table align='center' border='0' cellpadding='0' cellspacing='0' class='icons-inner' role='presentation' style='mso-table-lspace: 0pt; mso-table-rspace: 0pt;' width='100%'>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style='padding: 20px;'>
                                                                                            <p style='margin-bottom: 0;'>Regards,</p>
                                                                                            <p style='margin-top: 0;'>Akmal Rentals</p>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </body>
                    </html>";
            }


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }



        public ActionResult ResetPassword(string id)
        {
            //verify the reset password link
            //find account associated with this link
            //redirect user to reset password page


            var user = db.users.Where(u => u.u_forgotpwdCode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {

                var user = db.users.Where(u => u.u_forgotpwdCode == model.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    //hash
                    user.user_password = model.NewPassword;
                    //user can update one time with each user password code
                    user.u_forgotpwdCode = "";
                    //avoid confirm password not match issue
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    TempData["success"] = "New password updated successfully";
                }
                else
                {
                    return HttpNotFound();
                }

            }
            else
            {
                TempData["fail"] = "Something invalid";
            }
            return View(model);
        }



        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }

    }


}