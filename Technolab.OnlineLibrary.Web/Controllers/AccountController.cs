using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Technolab.OnlineLibrary.Web.Models;

namespace Technolab.OnlineLibrary.Web.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.Users)]
    public class AccountController : Controller
    {
        public ActionResult Index() 
        {
            return View();
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        private static string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "users.json");
        [HttpPost]
        [Route("/Account/ChangePassword")]
        public IActionResult ChangePassword(string username, string oldPassword, string newPassword, string newPasswordRepeat)
        {
            if (newPassword == newPasswordRepeat && newPassword.Length >= 8 && newPassword != oldPassword)
            {
                List<User> users = LoadUsersFromJson();

                foreach (var user in users)
                {
                    if (user.Username == username)
                    {
                        user.Password = newPassword;
                        break;
                    }
                }
                return View();
                SaveUsersToJson(users);
            }
            else
            {
                return View();
            }
            
        }
        private static List<User> LoadUsersFromJson()
        {
            if (System.IO.File.Exists(jsonFilePath))
            {
                string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
                return JsonConvert.DeserializeObject<List<User>>(jsonContent);
            }
            else
            {
                return new List<User>();
            }
        }

        private static void SaveUsersToJson(List<User> users)
        {
            string jsonContent = JsonConvert.SerializeObject(users, Formatting.Indented);

            System.IO.File.WriteAllText(jsonFilePath, jsonContent);
        }
        public class User
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
