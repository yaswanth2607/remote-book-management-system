using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Privacy_Folder.Models;
using System.Data.SqlClient;

namespace Privacy_Folder.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View(new LoginDetails());
        }
        public IActionResult SignUPPage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUPPage(LoginDetails model)
        {
            if (!ModelState.IsValid)
            {
                // Validation failed - return with validation messages
                return View(model);
            }

            string connectionString = _configuration.GetConnectionString("Data");

            using (SqlConnection sql = new SqlConnection(connectionString))
            {
                sql.Open();

                // Check if email already exists
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM LoginDetails WHERE Email = @Email", sql);
                checkCmd.Parameters.AddWithValue("@Email", model.Email);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    ViewBag.Error = "User already registered with this email. You Can Go and Login...";
                    return View(model);
                }

                SqlCommand insertCmd = new SqlCommand("INSERT INTO LoginDetails (Name, Email, Pass) VALUES (@Name, @Email, @Pass)", sql);
                insertCmd.Parameters.AddWithValue("@Name", model.Name);
                insertCmd.Parameters.AddWithValue("@Email", model.Email);
                insertCmd.Parameters.AddWithValue("@Pass", model.Pass); 
                insertCmd.ExecuteNonQuery();
            }

            ViewBag.Success = "Account Created You Can Login Your Acccount";
            return View("LoginPage");// Clear form on success
        }


        [HttpGet]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginPage(string Email, string Pass)
        {
            string connectionString = _configuration.GetConnectionString("Data");
            using (SqlConnection sql = new SqlConnection(connectionString))
            {
                sql.Open();
                SqlCommand cm = new SqlCommand("select count(*) from LoginDetails where Email=@email", sql);
                cm.Parameters.AddWithValue("@email", Email);
                int validemail = (int)cm.ExecuteScalar();
                if (validemail == 0)
                {
                    ViewBag.Invalid = "No Accout Found With this Email Pls Check Email";
                    ViewBag.Create = "If You Didn't Have An Account Go and Create An Account";
                    return View();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from LoginDetails where Email=@email AND Pass=@pass", sql);
                    cmd.Parameters.AddWithValue("@email", Email);
                    cmd.Parameters.AddWithValue("@pass", Pass);
                    int validpass = (int)cmd.ExecuteScalar();
                    if (validpass == 0)
                    {
                        ViewBag.Invalidpass = "Incorrect Password ! Please Try Again Later.";
                        return View();
                    }
                    else
                    {
                        sql.Close();
                        return View("~/Views/Book/BuyorSell.cshtml");
                    }
                }
            }
        }
        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PasswordChange(string Email, string Pass)
        {
            string connectionString = _configuration.GetConnectionString("Data");
            using (SqlConnection sql = new SqlConnection(connectionString))
            {
                sql.Open();
                SqlCommand checkEmailCmd = new SqlCommand("SELECT COUNT(*) FROM LoginDetails WHERE Email = @Email", sql);
                checkEmailCmd.Parameters.AddWithValue("@Email", Email);
                int exists = (int)checkEmailCmd.ExecuteScalar();

                if (exists == 1)
                {
                    SqlCommand updateCmd = new SqlCommand("UPDATE LoginDetails SET Pass = @NewPass WHERE Email = @Email", sql);
                    updateCmd.Parameters.AddWithValue("@NewPass", Pass);
                    updateCmd.Parameters.AddWithValue("@Email", Email);
                    updateCmd.ExecuteNonQuery();

                    ViewBag.success = "Password successfully changed!";
                }
                else
                {
                    ViewBag.error = "No account found with that email.";
                }

                sql.Close();
            }

            return View();
        }
    }
}

