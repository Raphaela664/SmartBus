using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace SmartBus.Pages.Admin
{
    public class RegisterBusModel : PageModel
    {
        public Bus bus = new Bus();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            bus.BusId = int.Parse(Request.Form["BusId"]);
            bus.BusName = Request.Form["name"];
            bus.TotalSeats = Request.Form["totalseats"];

            try
            {
                String connectionString = "Data Source=Raphaela\\SQLEXPRESS;Initial Catalog=SmartBusDB;Integrated Security=True;TrustServerCertificate = True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    String sqlQuery = "INSERT INTO BusMaster(BusId, BusName, TotalSeats) VALUES(@BusId,@BusName,@TotalSeats)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@BusId", bus.BusId);
                        cmd.Parameters.AddWithValue("@BusName", bus.BusName);
                        cmd.Parameters.AddWithValue("@TotalSeats", bus.TotalSeats);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            bus = new Bus();
            successMessage = "New Bus registered";

        }
    }
    public class Bus
    {
        public int BusId { get; set; }
        public string BusName { get; set; }
        public string TotalSeats { get; set; }

    }
}
