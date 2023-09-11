using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Text;

namespace KdyWeb.CloudParse.SelfHost.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Debug()
        {
            var stringBuild = new StringBuilder();

            var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            foreach (var cert in store.Certificates)
            {
                stringBuild.AppendLine($"Subject:{cert.Subject},Issuer: {cert.Issuer},Thumbprint: {cert.Thumbprint}");
            }

            store.Close();
            return Content(stringBuild.ToString().Replace("\n","<br/>"),"text/html");
        }

        public IActionResult Debug1()
        {
            var stringBuild = new StringBuilder();

            var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            foreach (var cert in store.Certificates)
            {
                stringBuild.AppendLine($"Subject:{cert.Subject},Issuer: {cert.Issuer},Thumbprint: {cert.Thumbprint}");
            }

            store.Close();
            return Content(stringBuild.ToString().Replace("\n", "<br/>"), "text/html");
        }
    }
}
