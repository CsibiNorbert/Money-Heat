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
            int x=0;
            int pp = 0;
            foreach (var date in userDatesDictionary)
            {

                    foreach (var temper in userTemperaturesDictionary.ToArray())
                    {
                       
                        pp = temper.Key + x;
                    if ((1 == date.Value.Month) && (date.Key == (temper.Key+x)))
                        {
                            if (userTemperaturesDictionary.ContainsKey(pp))
                            {
                                double d = userTemperaturesDictionary[pp];
                                userTemperatureListJanuary.Add(d);
                            }
                            
                        }
                        if ( (2 == date.Value.Month) && (date.Key == temper.Key + x))
                        {

                            if (userTemperaturesDictionary.ContainsKey(pp))
                            {
                                double d = userTemperaturesDictionary[pp];
                                userTemperatureListFeb.Add(d);
                            }
                        }
                        if ((3 == date.Value.Month) && (date.Key == temper.Key + x))
                        {

                            if (userTemperaturesDictionary.ContainsKey(pp))
                            {
                                double d = userTemperaturesDictionary[pp];
                                userTemperatureListMarch.Add(d);
                            }
                        }
                        if ((4 == date.Value.Month) && (date.Key == temper.Key + x))
                        {

                            if (userTemperaturesDictionary.ContainsKey(pp))
                            {
                                double d = userTemperaturesDictionary[pp];
                                userTemperatureListApr.Add(d);
                            }
                        }
                        if ((5 == date.Value.Month) && (date.Key == temper.Key + x))
                        {

                            if (userTemperaturesDictionary.ContainsKey(pp))
                            {
                                double d = userTemperaturesDictionary[pp];
                                userTemperatureListMay.Add(d);
                            }
                        }
                        if ((6 == date.Value.Month) && (date.Key == temper.Key + x))
                        {

                            if (userTemperaturesDictionary.ContainsKey(pp))
                            {
                                double d = userTemperaturesDictionary[pp];
                                userTemperatureListJun.Add(d);
                            }
                        }
                        if ((7 == date.Value.Month) && (date.Key == temper.Key + x))
                        {

                            if (userTemperaturesDictionary.ContainsKey(pp))
                            {
                                double d = userTemperaturesDictionary[pp];
                                userTemperatureListJul.Add(d);
                            }
                        }
                        if ((8 == date.Value.Month) && (date.Key == temper.Key + x))
                        {

                            if (userTemperaturesDictionary.ContainsKey(pp))
                            {
                                double d = userTemperaturesDictionary[pp];
                                userTemperatureListAug.Add(d);
                            }
                        }
                        if ((9 == date.Value.Month) && (date.Key == temper.Key + x))
                        {

                            if (userTemperaturesDictionary.ContainsKey(pp))
                            {
                                double d = userTemperaturesDictionary[pp];
                                userTemperatureListSept.Add(d);
                            }
                        }
                        if ((10 == date.Value.Month) && (date.Key == temper.Key + x))
                        {

                            if (userTemperaturesDictionary.ContainsKey(pp))
                            {
                                double d = userTemperaturesDictionary[pp];
                                userTemperatureListOct.Add(d);
                            }
                        }
                        if ((11 == date.Value.Month) && (date.Key == temper.Key + x))
                        {

                            if (userTemperaturesDictionary.ContainsKey(pp))
                            {
                                double d = userTemperaturesDictionary[pp];
                                userTemperatureListNov.Add(d);
                            }
                        }
                        if ((12== date.Value.Month) && (date.Key == temper.Key + x))
                        {

                            if (userTemperaturesDictionary.ContainsKey(pp))
                            {
                                double d = userTemperaturesDictionary[pp];
                                userTemperatureListDec.Add(d);
                            }
                        }
                        x++;

                    break;
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