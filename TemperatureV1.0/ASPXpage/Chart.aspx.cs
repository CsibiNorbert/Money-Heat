using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Helpers;
using System.Web.UI;
using MySql.Data.MySqlClient;

namespace TemperatureV1._0.ASPXpage
{
    public partial class Chart : Page
    {
        private MySqlDataAdapter adp;
        private MySqlCommand cmd = new MySqlCommand();

        private readonly MySqlConnection connection = new MySqlConnection(
            "server=localhost;user id=root;database=customer;password=Clujnapoca2019;Convert Zero Datetime=true;");

        private DataSet ds = new DataSet();
        private readonly DataTable dt = new DataTable();
        private DateTime usableDateTime;
        private DateTime userDate;
        private List<DateTime> userDateList = new List<DateTime>();

        private double userTemperature;
        private readonly List<double> userTemperatureList = new List<double>();

        protected void Page_Load(object sender, EventArgs e)
        {
            var retrieveUsername = Session["Username"].ToString();
            var retrieveUserTemp = "SELECT * FROM temperature WHERE Username='" + retrieveUsername + "';";

            connection.Open();
            cmd = new MySqlCommand(retrieveUsername, connection);
            adp = new MySqlDataAdapter(retrieveUserTemp, connection);
            adp.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                userDate = Convert.ToDateTime(row["dateTemp"]);


                //usableDateTime = DateTime.Parse(userDate);

                userTemperature = Convert.ToDouble(row["temperature"]);
                userTemperatureList.Add(userTemperature);
            }

            var adevargeUserTemp = userTemperatureList.Average();

            new System.Web.Helpers.Chart(800, 200, ChartTheme.Green)
                .AddTitle("Temperature")
                .AddSeries(
                    "Month",
                    "column",
                    xValue: new[] {"Jan"},
                    yValues: new[] {2}).Write("png");
        }
    }
}