using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Facebook;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SunnyKarmaWix.Models;

namespace SunnyKarmaWix.Controllers
{
    public class WixController : Controller
    {
        public ActionResult LaunchPad()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(string email, string password)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://sunnykarma.com/ajax/auth/signin.ashx", new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("email", email),
                        new KeyValuePair<string, string>("password", password),
                    }));

                var data = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                
                if (data["IsOK"].Value<bool>() == false)
                {
                    TempData["message"] = "Signing in failed";
                }
                else
                {
                    TempData["message"] = "Signed in! Username: " + data["Username"].Value<string>();
                }

                return RedirectToAction("LaunchPad");
            }
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(string email, string password, string username, string zipcode)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://sunnykarma.com/ajax/auth/signup.ashx", new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("email", email),
                        new KeyValuePair<string, string>("password", password),
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("zipcode", zipcode), 
                    }));

                var data = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);

                if (data["IsOK"].Value<bool>() == false)
                {
                    TempData["message"] = "Signing up failed: " + data["Status"].Value<string>();
                }
                else
                {
                    TempData["message"] = "Signed up! Username: " + data["Username"].Value<string>();
                }

                return RedirectToAction("LaunchPad");
            }
        }

        [HttpPost]
        public async Task<ActionResult> FacebookLogin(string token)
        {
            /*var fbClient = new FacebookClient(token);
            dynamic me = fbClient.Get("/me");

            var email = (string)me.email;
            var firstName = (string)me.first_name;
            var lastName = (string)me.last_name;
            var username = (string)me.username;
            var zipcode = "94103";*/

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://sunnykarma.com/ajax/auth/signinfacebook.ashx", new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("token", token)
                    }));

                var data = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);

                if (data["IsOK"].Value<bool>() == false)
                {
                    TempData["message"] = "Signing in failed: " + data["Error"].Value<string>();
                }
                else
                {
                    TempData["message"] = "Signed in! Username: " + data["Username"].Value<string>();
                }
                return RedirectToAction("LaunchPad");
            }
        }

        public void Test()
        {
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(
                    "");
            var tableClient = storageAccount.CreateCloudTableClient();
            var causeTable = tableClient.GetTableReference("Cause");
            var causes = causeTable.ExecuteQuery(causeTable.CreateQuery<Cause>()).ToArray();
        }
    }
}
