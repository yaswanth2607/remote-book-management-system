using Microsoft.AspNetCore.Mvc;
using SecondHandBookEkartApp.Models;
using System.Data.SqlClient;
using System.Reflection;

namespace SecondHandBookEkartApp.Controllers
{
    public class BookController : Controller
    {
        private readonly IConfiguration _configuration;
        public BookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Sell()
        {
            ViewBag.msg = TempData["msg"];
            return View(new Book());
        }

        [HttpPost]
        public IActionResult Sell(Book book)
        {
            string connectionString = _configuration.GetConnectionString("Data");
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Books
                (Title, Author, Price, Category, Location, SellerName, StockAvailable,MobileNumber,Deletekey)
                VALUES (@title, @author, @price, @category, 
                        @location, @sellername, @stockavailable,@MobileNumber,@deletekey)", con);

                    cmd.Parameters.AddWithValue("@title", book.Title);
                    cmd.Parameters.AddWithValue("@Author", book.Author);
                    cmd.Parameters.AddWithValue("@Price", book.Price);
                    cmd.Parameters.AddWithValue("@Category", book.Category);
                    cmd.Parameters.AddWithValue("@Location", book.Location);
                    cmd.Parameters.AddWithValue("@SellerName", book.SellerName);
                    cmd.Parameters.AddWithValue("@StockAvailable", book.StockAvailable);
                    cmd.Parameters.AddWithValue("@MobileNumber",book.MobileNumber);
                    cmd.Parameters.AddWithValue("@deletekey", book.Deletekey);
                    cmd.ExecuteNonQuery();
                }
                TempData["msg"] = "Book Added Successfully";
                return RedirectToAction("Sell");
            }
            return View("Sell", new Book());
        }


        public IActionResult Buy()
        {
            return View("Buy");
        }
        [HttpGet]
        public IActionResult ViewAll()
        {
            return View(new List<Book>());
        }

       [HttpPost]
        public IActionResult ViewAll(string location)
        {
        List<Book> list = new List<Book>();
        string connectionString = _configuration.GetConnectionString("Data");

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
                     string query = "SELECT * FROM Books WHERE StockAvailable > 0 AND Location= @location";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                    cmd.Parameters.AddWithValue("@location",location);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                    while (reader.Read())
                    {
                            list.Add(new Book
                            {
                                Title = (string)reader["Title"],
                                Author = (string)reader["Author"],
                                Price = Convert.ToDecimal(reader["Price"]),
                                Category = (string)reader["Category"],
                                Location = (string)reader["Location"],
                                SellerName = (string)reader["SellerName"],
                                StockAvailable = Convert.ToInt32(reader["StockAvailable"]),
                                MobileNumber = (string)reader["MobileNumber"]
                            });
                    }
                    }
                }
            }
        return View("DisplayBooks", list);
}
        [HttpGet]
        public IActionResult DeleteBook()
        {
            ViewBag.msg = TempData["msg"];
            return View();
        }

        [HttpPost]
        public IActionResult DeleteBook(string SellerName, string MobileNumber, string Deletekey)
        {
            string connectionString = _configuration.GetConnectionString("Data");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string checkQuery = "SELECT COUNT(*) FROM Books WHERE SellerName = @SellerName AND MobileNumber = @MobileNumber AND Deletekey = @Deletekey";
                using (SqlCommand cmd = new SqlCommand(checkQuery, con))
                {
                    cmd.Parameters.AddWithValue("@SellerName", SellerName);
                    cmd.Parameters.AddWithValue("@MobileNumber", MobileNumber);
                    cmd.Parameters.AddWithValue("@Deletekey", Deletekey);

                    int count = (int)cmd.ExecuteScalar();

                    if (count == 0)
                    {
                        ViewBag.error = "No seller found with the specified credentials.";
                        return View();
                    }
                    else
                    {
                        string deleteQuery = "DELETE FROM Books WHERE SellerName = @SellerName AND MobileNumber = @MobileNumber AND Deletekey = @Deletekey";
                        using (SqlCommand cmdDelete = new SqlCommand(deleteQuery, con))
                        {
                            cmdDelete.Parameters.AddWithValue("@SellerName", SellerName);
                            cmdDelete.Parameters.AddWithValue("@MobileNumber", MobileNumber);
                            cmdDelete.Parameters.AddWithValue("@Deletekey", Deletekey);

                            cmdDelete.ExecuteNonQuery();
                        }

                        TempData["msg"] = "Book deleted successfully.";
                        return RedirectToAction("DeleteBook");
                    }
                }
            }
        }


        [HttpGet]
        public IActionResult Logout()
        {
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult Note()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BuyorSell()
        {
            return View();
        }
    }
}
