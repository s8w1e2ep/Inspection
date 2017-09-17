using System.Net.Mail;
using System.Web.Mvc;
using InspectionWeb.Models;

namespace InspectionWeb.Controllers
{
    [AuthorizeUser(Super = true, Manager = true, Dispatch = true)]
    public class MailController : Controller
    {
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }
        /*
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }
        */
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
            //ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
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