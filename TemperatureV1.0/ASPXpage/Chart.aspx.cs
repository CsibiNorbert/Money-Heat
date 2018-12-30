using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.Web.Helpers;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;

namespace TemperatureV1._0.ASPXpage
{
    public partial class Chart : System.Web.UI.Page
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=customer;password=Clujnapoca2019;Convert Zero Datetime=true;");
        private MySqlCommand cmd = new MySqlCommand();
        MySqlDataAdapter adp ;
        private DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        private double userTemperature;
        private DateTime userDate;
        private DateTime usableDateTime;
        List<double> userTemperatureList = new List<double>();
        List<DateTime> userDateList = new List<DateTime>();
        protected void Page_Load(object sender, EventArgs e)
        {

            string retrieveUsername = Session["Username"].ToString();
            string retrieveUserTemp = "SELECT * FROM temperature WHERE Username='" + retrieveUsername.ToString() + "';";

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

            double adevargeUserTemp = userTemperatureList.Average();

            new System.Web.Helpers.Chart(width: 800, height: 200, theme: ChartTheme.Green)
                .AddTitle("Temperature")
                .AddSeries(
                    name: "Month",
                    chartType: "column",
                    xValue: new[] { "Jan" },
                    yValues: new[] { 2 }).Write("png");

        }
    }
}