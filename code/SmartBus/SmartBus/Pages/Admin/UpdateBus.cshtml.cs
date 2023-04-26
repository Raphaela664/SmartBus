using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static SmartBus.Pages.Admin.ManageBusModel;

namespace SmartBus.Pages.Admin
{
    public class UpdateBusModel : PageModel
    {
        public BusToUpdate bus = new BusToUpdate();
        public List<BusToUpdate> busList = new List<BusToUpdate>();

        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=Raphaela\\SQLEXPRESS;Initial Catalog=SmartBusDB;Integrated Security=True;TrustServerCertificate = True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    String sqlQuery = "SELECT BusId, BusName, TotalSeats FROM BusMaster WHERE BusId=@BusId";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@BusId", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                BusToUpdate busInfo = new BusToUpdate();

                                ViewData["BusId"] = reader.GetInt32(0);
                                busInfo.BusId = reader.GetInt32(0);
                                ViewData["name"] = reader.GetString(1);
                                busInfo.BusName = reader.GetString(1);
                                ViewData["totalseats"] = reader.GetInt32(2);
                                busInfo.TotalSeats = "" + reader.GetInt32(2);

                                busList.Add(busInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

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

                    String sqlQuery = "UPDATE BusMaster SET BusName=@BusName, TotalSeats=@TotalSeats WHERE BusId=@BusId";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@BusId", bus.BusId);
                        cmd.Parameters.AddWithValue("@BusName", bus.BusName);
                        cmd.Parameters.AddWithValue("@TotalSeats", bus.TotalSeats);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected >= 1) {
                            Response.Redirect("/Admin/ManageBus/");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            successMessage = "New Bus updated";

        }
    }
    public class BusToUpdate
    {
        public int BusId;
        public string BusName;
        public string TotalSeats;

    }
}
