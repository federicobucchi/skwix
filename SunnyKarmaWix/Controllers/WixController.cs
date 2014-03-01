using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
//using Facebook;
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
        public async Task<JsonResult> OwnerSignIn(string Email, string Password, string WixInstanceId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://sunnykarma.com/ajax/auth/signin.ashx", new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("Email", Email),
                        new KeyValuePair<string, string>("Password", Password),
                    }));

                var data = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);

                if (data["IsOK"].Value<bool>() == false)
                {
                    return new JsonResult { Data = new { Status = "error" } };
                }
                var username = data["Username"].Value<string>();
                Session["Username"] = username;
                DataManager.AddWixOwner(new WixOwner {Username = username, WixInstanceId = WixInstanceId});
                return new JsonResult { Data = new { Status = "ok", Username =  username} };
            }
        }

        [HttpPost]
        public async Task<JsonResult> OwnerSignUp(string Email, string Password, string Username, string Zipcode, string WixInstanceId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://sunnykarma.com/ajax/auth/signup.ashx", new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("Email", Email),
                        new KeyValuePair<string, string>("Password", Password),
                        new KeyValuePair<string, string>("Username", Username),
                        new KeyValuePair<string, string>("Zipcode", Zipcode), 
                    }));

                var data = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);

                if (data["IsOK"].Value<bool>() == false)
                {
                    return new JsonResult { Data = new { Status = "error" } };
                }
                var username = data["Username"].Value<string>();
                Session["Username"] = username;
                DataManager.AddWixOwner(new WixOwner { Username = username, WixInstanceId = WixInstanceId });
                return new JsonResult { Data = new { Status = "ok", Username = username } };
            }
        }

        [HttpPost]
        public async Task<JsonResult> SignIn(string Email, string Password)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://sunnykarma.com/ajax/auth/signin.ashx", new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("Email", Email),
                        new KeyValuePair<string, string>("Password", Password),
                    }));

                var data = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                
                if (data["IsOK"].Value<bool>() == false)
                {
                    return new JsonResult {Data = new {Status = "error"}};
                }
                var username = data["Username"].Value<string>();
                Session["Username"] = username;
                return new JsonResult { Data = new { Status = "ok", Username = username } };
            }
        }

        [HttpPost]
        public async Task<JsonResult> SignUp(string Email, string Password, string Username, string Zipcode)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://sunnykarma.com/ajax/auth/signup.ashx", new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("Email", Email),
                        new KeyValuePair<string, string>("Password", Password),
                        new KeyValuePair<string, string>("Username", Username),
                        new KeyValuePair<string, string>("Zipcode", Zipcode), 
                    }));

                var data = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);

                if (data["IsOK"].Value<bool>() == false)
                {
                    return new JsonResult { Data = new { Status = "error" } };
                }
                var username = data["Username"].Value<string>();
                Session["Username"] = username;
                return new JsonResult { Data = new { Status = "ok", Username = username } };
            }
        }

        [HttpPost]
        public JsonResult Donate(string Username, string CCNumber, string CCSecurity, string CCExpDate, int Amount,
                                 string WixInstanceId)
        {
            try
            {
                string causeId = "LLS";

                // TODO: CHARGE CARD
                DataManager.DonateToCause(Username, causeId, Amount);
                DataManager.AwardKarmaPointsToUser(Username, causeId, Amount / 3);
                DataManager.AwardKarmaPointsToOwner(Username, causeId, Amount / 5);

                return new JsonResult {Data = new {Status = "ok"}};
            }
            catch (Exception)
            {
                return new JsonResult { Data = new { Status = "error" } };
            }
        }

        [HttpPost]
        public async Task<ActionResult> FacebookLogin(string token)
        {
            /*var fbClient = new FacebookClient(token);
            dynamic me = fbClient.Get("/me");

            var Email = (string)me.Email;
            var firstName = (string)me.first_name;
            var lastName = (string)me.last_name;
            var Username = (string)me.Username;
            var Zipcode = "94103";*/

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
    }
}
