using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AlphaZero.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace AlphaZero.Controllers
{
    public class WhatsAppController : Controller
    {
        private db_roomrentalEntities db = new db_roomrentalEntities();

        // GET: WhatsApp
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SendWhatsAppMessage(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            reminds reminds = db.reminds.Find(id);

            if (reminds == null)
            {
                return HttpNotFound();
            }

            var accountSid = System.Configuration.ConfigurationManager.AppSettings["TwilioAccountSid"];
            var authToken = System.Configuration.ConfigurationManager.AppSettings["TwilioAuthToken"];

            TwilioClient.Init(accountSid, authToken);

            using (db = new db_roomrentalEntities())
            {
                // Retrieve the bank account number from sysConfig table
                var bankAccountConfig = await db.sysConfigs.FirstOrDefaultAsync(c => c.category == "bankAcc");
                var bankAccountConfig1 = await db.sysConfigs.FirstOrDefaultAsync(c => c.category == "bankName");
                var tenantName = reminds.tenant.tenant_name;
                var tenantRoom = reminds.tenant.room.room_number;
                var tenantFloor = reminds.tenant.room.floor.floor_id;
                var link = "https://www.facebook.com/AkmalBilikSewa";


                if (bankAccountConfig != null || bankAccountConfig1 != null)
                {
                    var bankAccNo = bankAccountConfig.value;
                    var bankName = bankAccountConfig1.value;
                    
                    // Emoji representation: U+1F44B (Wave)
                    var emoji = "\U0001F44B";


                    var message = await MessageResource.CreateAsync(
                        from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                        body: $"Hello there {tenantName} ! {emoji} \nIt is time to pay your rent.\n\nPlease do so via online transfer to the following bank account details.\nBank account: {bankAccNo}\nBank name: {bankName}\n\n" +
                        $"How was your stay at {tenantFloor}?\nBe sure to leave us a good review in Facebook : {link}\n\nFor any inquiries, contact us at : 0177122977",
                        to: new Twilio.Types.PhoneNumber("whatsapp:+601131350164")
                    );

                    // Process the response or handle any errors
                }
            }

            /*var message = await MessageResource.CreateAsync(
                from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                body: "Hello, this is a WhatsApp message.",
                to: new Twilio.Types.PhoneNumber("whatsapp:+601131350164")
            );*/

            // Process the response or handle any errors

            return RedirectToAction("Index", "Notification");
        }


    }
}
