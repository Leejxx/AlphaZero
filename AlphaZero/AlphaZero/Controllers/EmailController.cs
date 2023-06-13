using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using System.Web.Mail;

namespace AlphaZero.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Index()
        {
            return View();
        }

        // POST: Email/SendCredentials
        [HttpPost]
        public ActionResult SendCredentials(string userName, string password, string userEmail, string investorName)
        {
            // Sender's email credentials
            string senderEmail = "akmalrentalsalphazero@gmail.com"; // Replace with your Gmail address
            string senderPassword = "otubezrpgcnrvxnl"; // Replace with your Gmail password

            // Recipient email address
            string recipientEmail = userEmail; // Replace with the recipient's email address

            // Construct the email message
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(senderEmail, recipientEmail);

            string logoImageUrl = "https://i.imgur.com/j3xnJzi.png";
            string backgroundImageUrl = "https://i.imgur.com/4Q9bX9h.png";

            mail.Subject = "Welcome to Akmal Rental System";
            mail.Body = $@"
                    
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
                                                                                            <h3 style='text-align: center;'>Hello, {investorName}!</h3>
                                                                                            <p style='text-align: center;'>Welcome to Akmal Rentals Management System</p>
                                                                                            <p style='text-align: center;'>Your Credentials to Login into the System are as follows:</p>
                                                                                            <p style='text-align: center;'>Username: {userName}</p>
                                                                                            <p style='text-align: center;'>Password: {password}</p>
                                                                                            <p style='text-align: center;'>Link: https://www.facebook.com/AkmalBilikSewa</p>

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


            mail.IsBodyHtml = true;

            //$"Your username: {userName}\nYour password: {password}" +
            //$"";


            // Configure the SMTP client
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            try
            {
                smtpClient.Send(mail); // Send the email
                ViewBag.Message = "Email sent successfully";
            }
            catch (SmtpException ex)
            {
                // Handle any errors that occur during email sending
                ViewBag.Message = $"Error sending email: {ex.Message}";
            }

            return RedirectToAction("Index", "Investor"); // Redirect to the desired action after sending the email
        }
    }
}