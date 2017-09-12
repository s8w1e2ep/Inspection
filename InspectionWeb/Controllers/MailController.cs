using System.Net.Mail;
using System.Web.Mvc;

namespace InspectionWeb.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }

        // POST: /User/AddUser
        [HttpPost]
        public ActionResult SendEmail(EmailJson data)
        {
            // set smtp server
            SmtpClient smtpClient = new SmtpClient("mail.MyWebsiteDomainName.com", 25);
            smtpClient.Credentials = new System.Net.NetworkCredential("info@MyWebsiteDomainName.com", "myIDPassword");
            smtpClient.UseDefaultCredentials = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            MailMessage mail = new MailMessage(
                data.from,
                data.to,
                data.subject,
                data.msg
            );
            mail.IsBodyHtml = true;

            smtpClient.Send(mail);

            return Json(new { result = true });
        }

        public class EmailJson
        {
            public string from { get; set; }
            public string to { get; set; }
            public string subject { get; set; }
            public string msg { get; set; }
        }
    }
}