using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace SmartBus.Pages.Admin
{
    public class ManageBusModel : PageModel
    {
        public List<BusInfo> busInfosList = new List<BusInfo>();
        public void OnGet()
        {
            String connectionString = "Data Source=Raphaela\\SQLEXPRESS;Initial Catalog=SmartBusDB;Integrated Security=True;TrustServerCertificate = True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                String sqlQuery = "SELECT busid, busname, totalseats FROM BusMaster";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BusInfo busInfo = new BusInfo();
                            busInfo.BusId = reader.GetInt32(0);
                            busInfo.BusName = reader.GetString(1);
                            busInfo.TotalSeats = "" + reader.GetInt32(2);

                            busInfosList.Add(busInfo);
                        }
                    }
                }
            }
        }

        public class BusInfo
        {
            public int BusId;
            public string BusName;
            public string TotalSeats;

        }
    }
}
