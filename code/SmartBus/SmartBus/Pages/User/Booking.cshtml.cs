using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SmartBus.Pages.Admin;
using System.Text.RegularExpressions;
using static SmartBus.Pages.Admin.ManageBusModel;

namespace SmartBus.Pages.User
{
    public class BookingModel : PageModel
    {
        public BookTrip Trip = new BookTrip();
        public String errorMessage = "";
        public String successMessage = "";
        public List<RouteDetails> RouteInfoList = new List<RouteDetails>();
        public void OnGet()
        {
            String connectionString = "Data Source=Raphaela\\SQLEXPRESS;Initial Catalog=SmartBusDB;Integrated Security=True;TrustServerCertificate = True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                String sqlQuery = "SELECT RouteId, Origin, Destination, BusId, Price FROM RouteDetails";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RouteDetails route = new RouteDetails();

                            route.routeId = reader.GetInt32(0);
                            route.origin = reader.GetString(1);
                            route.destination = reader.GetString(2);
                            route.busId = reader.GetInt32(3);
                            route.price = reader.GetInt32(4);

                            RouteInfoList.Add(route);
                        }
                    }
                }
            }
        }
        public void OnPost()
        {
            Trip.RouteId = int.Parse(Request.Form["routeId"]);
            Trip.regId = Request.Form["regId"];
            Trip.departdate = Request.Form["departdate"];
            Trip.departtime = Request.Form["departtime"];
            Trip.SeatNo = int.Parse(Request.Form["seatno"]);

            try
            {
                String connectionString = "Data Source=Raphaela\\SQLEXPRESS;Initial Catalog=SmartBusDB;Integrated Security=True;TrustServerCertificate = True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    String sqlQuery = "INSERT INTO BookingMaster(routeId, regId, seatno, departdate, departtime) VALUES(@routeId,@regId,@seatno,@departdate,@departtime)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@routeId", Trip.RouteId);
                        cmd.Parameters.AddWithValue("@regId", Trip.regId);
                        cmd.Parameters.AddWithValue("@seatno", Trip.SeatNo);
                        cmd.Parameters.AddWithValue("@departdate", Trip.departdate);
                        cmd.Parameters.AddWithValue("@departtime", Trip.departtime);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            //bus = new Bus();
            successMessage = "Ticket Successfully Booked";

        }
    }

    public class RouteDetails
    {
        public int routeId;
        public string origin;
        public string destination;
        public int busId;
        public decimal price;
    }
    public class BookTrip
    {
        public int RouteId;
        public int SeatNo;
        public string regId;
        public string departtime;
        public string departdate;

    }
}
