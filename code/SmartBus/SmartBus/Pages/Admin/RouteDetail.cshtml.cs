using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace SmartBus.Pages.Admin
{
    public class RouteDetailModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Route ID is required.")]
        public int RouteId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Origin is required.")]
        public string Origin { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Destination is required.")]
        public string Destination { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Bus ID is required.")]
        public int BusId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public double Price { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public List<RouteInfo> listRoute = new List<RouteInfo>();
        public void OnGet()
        {

            try
            {
                String connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=SmartBus;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    String sqlQuery = "SELECT * FROM RouteDetails";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                RouteInfo routeInfo = new RouteInfo();
                                routeInfo.RouteId = "" + reader.GetInt32(0);
                                routeInfo.Origin = "" + reader.GetString(1);
                                routeInfo.Destination = reader.GetString(2);
                                routeInfo.BusId = "" + reader.GetInt32(3);
                                routeInfo.Destination = reader.GetString(4);
                                listRoute.Add(routeInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
            }
        }
        public class RouteInfo
        {
            public string RouteId { get; set; }
            public string Origin { get; set; }
            public string Destination { get; set; }
            public string BusId { get; set; }
            public string price { get; set; }

        }



        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=SmartBus;Integrated Security=True"))
                {
                    connection.Open();

                    // Check if route already exists in the RouteDetails table
                    using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM RouteDetails WHERE RouteId = @RouteId", connection))
                    {
                        command.Parameters.AddWithValue("@RouteId", RouteId);
                        int count = (int)command.ExecuteScalar();
                        if (count > 0)
                        {
                            ErrorMessage = "A route with this ID already exists.";
                            return Page();
                        }
                    }

                    // Insert new route record into the RouteDetails table
                    using (SqlCommand command = new SqlCommand("INSERT INTO RouteDetails (RouteId, Origin, Destination, BusId, Price) VALUES (@RouteId, @Origin, @Destination, @BusId, @Price)", connection))
                    {
                        command.Parameters.AddWithValue("@RouteId", RouteId);
                        command.Parameters.AddWithValue("@Origin", Origin);
                        command.Parameters.AddWithValue("@Destination", Destination);
                        command.Parameters.AddWithValue("@BusId", BusId);
                        command.Parameters.AddWithValue("@Price", Price);
                        command.ExecuteNonQuery();
                    }

                    SuccessMessage = "Route added successfully!";
                }

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while processing your request.";
                return Page();
            }
        }
        public IActionResult OnPostDelete()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=SmartBus;Integrated Security=True"))
                {
                    connection.Open();

                    // Check if route exists in the RouteDetails table
                    using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM RouteDetails WHERE RouteId = @RouteId", connection))
                    {
                        command.Parameters.AddWithValue("@RouteId", RouteId);
                        int count = (int)command.ExecuteScalar();
                        if (count == 0)
                        {
                            ErrorMessage = "A route with this ID does not exist.";
                            return Page();
                        }
                    }

                    // Delete route record from the RouteDetails table
                    using (SqlCommand command = new SqlCommand("DELETE FROM RouteDetails WHERE RouteId = @RouteId", connection))
                    {
                        command.Parameters.AddWithValue("@RouteId", RouteId);
                        command.ExecuteNonQuery();
                    }

                    SuccessMessage = "Route deleted successfully!";
                }

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while processing your request.";
                return Page();
            }
        }
        public IActionResult OnPostSearch()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=SmartBus;Integrated Security=True"))
                {
                    connection.Open();

                    // Search for route record in the RouteDetails table
                    using (SqlCommand command = new SqlCommand("SELECT * FROM RouteDetails WHERE RouteId = @RouteId", connection))
                    {
                        command.Parameters.AddWithValue("@RouteId", RouteId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // If a record is found, populate the properties with the data
                            if (reader.Read())
                            {
                                RouteId = reader.GetInt32(0);
                                Origin = reader.GetString(1);
                                Destination = reader.GetString(2);
                                BusId = reader.GetInt32(3);
                                Price = reader.GetDouble(4);

                                SuccessMessage = "Route found!";
                                return Page();
                            }
                            else
                            {
                                ErrorMessage = "A route with this ID does not exist.";
                                return Page();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while processing your request.";
                return Page();
            }
        }


    }
}
