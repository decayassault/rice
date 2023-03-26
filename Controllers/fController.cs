using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Net;
namespace App.Controllers
{
    public sealed class fController : Controller
    {
        public readonly IFriendlyFire Force;
        public fController(IFriendlyFire force)
        {
            Force = force;
        }

        [HttpGet]
        public ContentResult t() // threads
        {
            Response.Headers.Add("X-Frame-Options", "deny");

            return Content(Force.ForumLogic_GetMainPageLocked(), "text/html");
        }

        [HttpGet]
        public ContentResult f(int? i, int? p) // thread (flow) (id, page)
        {
            Response.Headers.Add("X-Frame-Options", "deny");

            return Content(Force.ThreadData_GetThreadPage(i, p), "text/html");
        }

        [HttpGet]
        public ContentResult c() // maincontent
        {
            Response.Headers.Add("X-Frame-Options", "deny");

            return Content(Force.ForumLogic_GetMainContentLocked(), "text/html");
        }

        [HttpGet]
        public ContentResult s(int? i, int? p) // section (id, page)
        {
            Response.Headers.Add("X-Frame-Options", "deny");

            return Content(Force.SectionLogic_GetSectionPage(i, p), "text/html");
        }

        [HttpGet]
        public ContentResult e(int? i) // endpoint (id)
        {
            Response.Headers.Add("X-Frame-Options", "deny");

            return Content(Force.EndPointLogic_GetEndPointPage(i), "text/html");
        }

        [HttpGet]
        public string a
            (string c, string l, string p) // authenticate (captcha, login, password)
        {
            if (Force.CheckAndIncrementIpHashesCounter(Request.HttpContext.Connection.RemoteIpAddress, Constants.five))
            {
                Response.Headers.Add("X-Frame-Options", "deny");

                return Force.LoginData_CheckAndAuth(c, l, p);
            }

            return Constants.SE;
        }

        [HttpGet]
        public ContentResult r() // registrationpage
        {
            Response.Headers.Add("X-Frame-Options", "deny");

            return Content(Force.GetRegistrationDataPageToReturn(), "text/html");
        }

        [HttpGet]
        public ContentResult l() // login
        {
            Response.Headers.Add("X-Frame-Options", "deny");
            StringValues at;
            if (HttpContext.Request.Headers.ContainsKey("a")
                && HttpContext.Request.Headers.TryGetValue("a", out at)
                && Force.AuthenticationLogic_AccessGranted(at[Constants.Zero]))
                return Content(Constants.ReplyPage, "text/html");
            else
                return Content(Constants.LoginRequirement, "text/html");
        }

        [HttpGet]
        public void y(int? i, string t) // reply (id, t)
        {
            if (Force.CheckAndIncrementIpHashesCounter(Request.HttpContext.Connection.RemoteIpAddress, Constants.One))
            {
                Response.Headers.Add("X-Frame-Options", "deny");
                StringValues at;

                if (HttpContext.Request.Headers.ContainsKey("a")
                    && HttpContext.Request.Headers.TryGetValue("a", out at)
                    && Force.AuthenticationLogic_AccessGranted(at[Constants.Zero]))
                    Force.ReplyData_Start(i, Force.AuthenticationLogic_GetPair(at[Constants.Zero]), t);
            }
        }
        [HttpGet]
        public ContentResult o() // loginpage
        {
            Response.Headers.Add("X-Frame-Options", "deny");

            return Content(Force.LoginData_GetPageToReturn(), "text/html");
        }
        [HttpPost]
        public void g(string c, string l, string p,
            string e, string n) // register (captcha, login, password, email, nick)
        {
            if (Force.CheckAndIncrementIpHashesCounter(Request.HttpContext.Connection.RemoteIpAddress, Constants.five))
            {
                Response.Headers.Add("X-Frame-Options", "deny");
                Force.RegistrationData_PreRegistration(c, l, p, e, n);
            }
        }

        [HttpGet]
        public ContentResult n() // newtopic
        {
            Response.Headers.Add("X-Frame-Options", "deny");
            StringValues at;
            if (HttpContext.Request.Headers.ContainsKey("a")
                && HttpContext.Request.Headers.TryGetValue("a", out at)
                && Force.AuthenticationLogic_AccessGranted(at[Constants.Zero]))
                return Content(Constants.PageToReturnTopic, "text/html");
            else
                return Content(Constants.LoginRequirement, "text/html");
        }

        [HttpGet]
        public void h(int? i, string t, string m) // starttopic (id, t, m)
        {
            if (Force.CheckAndIncrementIpHashesCounter(Request.HttpContext.Connection.RemoteIpAddress, Constants.One))
            {
                Response.Headers.Add("X-Frame-Options", "deny");
                StringValues at;

                if (HttpContext.Request.Headers.ContainsKey("a")
                    && HttpContext.Request.Headers.TryGetValue("a", out at)
                    && Force.AuthenticationLogic_AccessGranted(at[Constants.Zero]))
                    Force.NewTopicData_Start(t, i, Force.AuthenticationLogic_GetPair(at[Constants.Zero]), m);
            }
        }

        [HttpGet]
        public ContentResult d(int? i) // dialog (id)
        {
            var test = Force.GetDialogPagesLengthFast(); // удалить

            Response.Headers.Add("X-Frame-Options", "deny");
            StringValues at;

            if (HttpContext.Request.Headers.ContainsKey("a")//access token
                && HttpContext.Request.Headers.TryGetValue("a", out at)
                && Force.AuthenticationLogic_AccessGranted(at[Constants.Zero]))
            {
                return Content(Force.PrivateDialogLogic_GetDialogPage(i,
                    Force.AuthenticationLogic_GetPair(at[Constants.Zero])), "text/html");
            }
            else
                return Content(Constants.LoginRequirement, "text/html");
        }

        [HttpGet]
        public ContentResult p(int? i, int? p) // personal (id, page)
        {
            Response.Headers.Add("X-Frame-Options", "deny");
            StringValues at;

            if (HttpContext.Request.Headers.ContainsKey("a")
                && HttpContext.Request.Headers.TryGetValue("a", out at)
                && Force.AuthenticationLogic_AccessGranted(at[Constants.Zero]))
                return Content(Force.PrivateMessageLogic_GetPersonalPage(i, p,
                Force.AuthenticationLogic_GetPair(at[Constants.Zero])), "text/html");
            else
                return Content(Constants.LoginRequirement, "text/html");
        }

        [HttpGet]
        public ContentResult m() // replypersonal
        {
            Response.Headers.Add("X-Frame-Options", "deny");
            StringValues at;
            if (HttpContext.Request.Headers.ContainsKey("a")
                && HttpContext.Request.Headers.TryGetValue("a", out at)
                && Force.AuthenticationLogic_AccessGranted(at[Constants.Zero]))
                return Content(Constants.PrivateReplyPage, "text/html");
            else
                return Content(Constants.LoginRequirement, "text/html");
        }

        [HttpGet]
        public void b(int? i, string t) // sendpersonal (id, t)
        {
            if (Force.CheckAndIncrementIpHashesCounter(Request.HttpContext.Connection.RemoteIpAddress, Constants.One))
            {
                Response.Headers.Add("X-Frame-Options", "deny");
                StringValues at;

                if (HttpContext.Request.Headers.ContainsKey("a")
                    && HttpContext.Request.Headers.TryGetValue("a", out at)
                    && Force.AuthenticationLogic_AccessGranted(at[Constants.Zero]))
                    Force.NewPrivateMessageLogic_Start(i,
                     Force.AuthenticationLogic_GetPair(at[Constants.Zero]), t);
            }
        }

        [HttpGet]
        public ContentResult i() // newdialog
        {
            Response.Headers.Add("X-Frame-Options", "deny");
            StringValues at;

            if (HttpContext.Request.Headers.ContainsKey("a")
                && HttpContext.Request.Headers.TryGetValue("a", out at)
                && Force.AuthenticationLogic_AccessGranted(at[Constants.Zero]))
                return Content(Constants.PageToReturn, "text/html");
            else
                return Content(Constants.LoginRequirement, "text/html");
        }

        [HttpGet]
        public void j(string n, string m) // startdialog (nick, msg)
        {
            if (Force.CheckAndIncrementIpHashesCounter(Request.HttpContext.Connection.RemoteIpAddress, Constants.One))
            {
                Response.Headers.Add("X-Frame-Options", "deny");
                StringValues at;

                if (HttpContext.Request.Headers.ContainsKey("a")
                    && HttpContext.Request.Headers.TryGetValue("a", out at)
                    && Force.AuthenticationLogic_AccessGranted(at[Constants.Zero]))
                    Force.NewPrivateDialogLogic_Start(n,
                    Force.AuthenticationLogic_GetPair(at[Constants.Zero]), m);
            }
        }
    }
}
