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
       

        private readonly MySqlConnection connection = new MySqlConnection(
            "server=localhost;user id=root;database=customer;password=Clujnapoca2019;Convert Zero Datetime=true;");
        
       
        private readonly DataTable dt = new DataTable();
      
        private DateTime userDate;
       

       
        private  List<double> userTemperatureListJanuary = new List<double>();
        private  List<double> userTemperatureListFeb = new List<double>();
        private  List<double> userTemperatureListMarch = new List<double>();
        private  List<double> userTemperatureListApr = new List<double>();
        private  List<double> userTemperatureListMay = new List<double>();
        private  List<double> userTemperatureListJun = new List<double>();
        private  List<double> userTemperatureListJul = new List<double>();
        private  List<double> userTemperatureListAug = new List<double>();
        private  List<double> userTemperatureListSept = new List<double>();
        private  List<double> userTemperatureListOct = new List<double>();
        private  List<double> userTemperatureListNov = new List<double>();
        private  List<double> userTemperatureListDec = new List<double>();
        DateTime currentUserDate = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {
            var retrieveUsername = Session["Username"].ToString();

            var retrieveUserTemp = "SELECT temperature,dateTemp FROM temperature WHERE Username='" + retrieveUsername + "';";
            connection.Open();
           
            adp = new MySqlDataAdapter(retrieveUserTemp, connection);
            adp.Fill(dt);
            double temp;
            Dictionary<int,DateTime> userDatesDictionary = new Dictionary<int, DateTime>();
            Dictionary<int,double> userTemperaturesDictionary = new Dictionary<int, double>();
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                userDate = Convert.ToDateTime(row["dateTemp"]);
                userDatesDictionary.Add(i,userDate);
               temp = Convert.ToDouble(row["temperature"]);
                userTemperaturesDictionary.Add(i,temp);
                i++;
              

            }


            var mych = new System.Web.Helpers.Chart(800, 200, ChartTheme.Green);
            adp.Dispose();
            dt.Dispose();
            foreach (var date in userDatesDictionary)
            {
                if (currentUserDate.Month == date.Value.Month)
                {
                    foreach (var temper in userTemperaturesDictionary)
                    {
                        if ((currentUserDate.Month == 1) && (currentUserDate.Year == date.Value.Year) && (date.Key == temper.Key))
                        {
                            userTemperatureListJanuary.Add(temper.Value);
                        }
                        if ((currentUserDate.Month == 2) && (currentUserDate.Year == date.Value.Year) && (date.Key == temper.Key))
                        {
                            userTemperatureListFeb.Add(temper.Value);
                        }
                        if ((currentUserDate.Month == 3) && (currentUserDate.Year == date.Value.Year) && (date.Key == temper.Key))
                        {
                            userTemperatureListMarch.Add(temper.Value);
                        }
                        if ((currentUserDate.Month == 4) && (currentUserDate.Year == date.Value.Year) && (date.Key == temper.Key))
                        {
                            userTemperatureListApr.Add(temper.Value);
                        }
                        if ((currentUserDate.Month == 5) && (currentUserDate.Year == date.Value.Year) && (date.Key == temper.Key))
                        {
                            userTemperatureListMay.Add(temper.Value);
                        }
                        if ((currentUserDate.Month == 6) && (currentUserDate.Year == date.Value.Year) && (date.Key == temper.Key))
                        {
                            userTemperatureListJun.Add(temper.Value);
                        }
                        if ((currentUserDate.Month == 7) && (currentUserDate.Year == date.Value.Year) && (date.Key == temper.Key))
                        {
                            userTemperatureListJul.Add(temper.Value);
                        }
                        if ((currentUserDate.Month == 8) && (currentUserDate.Year == date.Value.Year) && (date.Key == temper.Key))
                        {
                            userTemperatureListAug.Add(temper.Value);
                        }
                        if ((currentUserDate.Month == 9) && (currentUserDate.Year == date.Value.Year) && (date.Key == temper.Key))
                        {
                            userTemperatureListSept.Add(temper.Value);
                        }
                        if ((currentUserDate.Month == 10) && (currentUserDate.Year == date.Value.Year) && (date.Key == temper.Key))
                        {
                            userTemperatureListOct.Add(temper.Value);
                        }
                        if ((currentUserDate.Month == 11) && (currentUserDate.Year == date.Value.Year) && (date.Key == temper.Key))
                        {
                            userTemperatureListNov.Add(temper.Value);
                        }
                        if ((currentUserDate.Month == 12) && (currentUserDate.Year == date.Value.Year) && (date.Key == temper.Key))
                        {
                            userTemperatureListDec.Add(temper.Value);
                        }
                    }
                }
               
            
            }
           
            adp.Dispose();
            dt.Dispose();
            var avgUserTemperatureJan = 0.0;
            var avgUserTemperatureFeb = 0.0;
            var avgUserTemperatureMarch = 0.0;
            var avgUserTemperatureApr = 0.0;
            var avgUserTemperatureMay = 0.0;
            var avgUserTemperatureJun = 0.0;
            var avgUserTemperatureJul = 0.0;
            var avgUserTemperatureAug = 0.0;
            var avgUserTemperatureSep = 0.0;
            var avgUserTemperatureOct = 0.0;
            var avgUserTemperatureNov = 0.0;
            var avgUserTemperatureJDec = 0.0;
            if (userTemperatureListJanuary.Count == 0)
            {
                //do nothing
            }
            else
            {
                avgUserTemperatureJan = userTemperatureListJanuary.Average();
            }
            
            if (userTemperatureListFeb.Count == 0)
            {
                //do nothing
            }
            else
            {
                avgUserTemperatureFeb = userTemperatureListFeb.Average();
            }
            if (userTemperatureListMarch.Count == 0)
            {
                //do nothing
            }
            else
            {
                avgUserTemperatureMarch = userTemperatureListMarch.Average();
            }

            if (userTemperatureListApr.Count == 0)
            {
                //do nothing
            }
            else
            {
                avgUserTemperatureApr = userTemperatureListApr.Average();
            }
            if (userTemperatureListMay.Count == 0)
            {
                //do nothing
            }
            else
            {
                avgUserTemperatureMay = userTemperatureListMay.Average();
            }
            if (userTemperatureListJun.Count == 0)
            {
                //do nothing
            }
            else
            {
                avgUserTemperatureJun = userTemperatureListJun.Average();
            }
            if (userTemperatureListJul.Count == 0)
            {
                //do nothing
            }
            else
            {
                avgUserTemperatureJul = userTemperatureListJul.Average();
            }
            if (userTemperatureListAug.Count == 0)
            {
                //do nothing
            }
            else
            {
                avgUserTemperatureAug = userTemperatureListAug.Average();
            }
            if (userTemperatureListSept.Count == 0)
            {
                //do nothing
            }
            else
            {
                avgUserTemperatureSep = userTemperatureListSept.Average();
            }
            if (userTemperatureListOct.Count == 0)
            {
                //do nothing
            }
            else
            {
                avgUserTemperatureOct = userTemperatureListOct.Average();
            }
            if (userTemperatureListNov.Count == 0)
            {
                //do nothing
            }
            else
            {
                avgUserTemperatureNov = userTemperatureListNov.Average();
            }
            if (userTemperatureListDec.Count == 0)
            {
                //do nothing
            }
            else
            {
                avgUserTemperatureJDec = userTemperatureListDec.Average();
            }
            


            mych.AddTitle("Monthly Average Temperature")
                .AddSeries(
                    "Month",
                    "column",
                    xValue: new[] { "Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec" },
                    yValues: new[] { avgUserTemperatureJan, avgUserTemperatureFeb, avgUserTemperatureMarch, avgUserTemperatureApr, avgUserTemperatureMay, avgUserTemperatureJun, avgUserTemperatureJul, avgUserTemperatureAug, avgUserTemperatureSep, avgUserTemperatureOct, avgUserTemperatureNov, avgUserTemperatureJDec }).Write("png");
           
        }
    }
}