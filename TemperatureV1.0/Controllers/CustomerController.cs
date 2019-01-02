using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Web.Helpers;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using TemperatureV1._0.Models;

namespace TemperatureV1._0.Controllers
{
    public class CustomerController : Controller
    {
        private MySqlDataAdapter adp = new MySqlDataAdapter();

        private MySqlCommand cmd = new MySqlCommand();

        //MySqlConnection connection = new MySqlConnection();
        //private string strConnString = ConfigurationManager.ConnectionStrings["MySqlTemperature"].ConnectionString;
        private readonly MySqlConnection connection = new MySqlConnection(
            "server=localhost;user id=root;database=customer;password=Clujnapoca2019;Convert Zero Datetime=true;");

        private readonly List<DateTime> dateList = new List<DateTime>();
        private readonly DataSet ds = new DataSet();

        private readonly List<string> intList = new List<string>();
        private string portAvailable;
        //private readonly string[] ports = SerialPort.GetPortNames();


        private bool x8;

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(Customer account)
        {
            var registerUser = "INSERT INTO user (FName,LName,Email,Username,City,Password) VALUES ('" + account.FName +
                               "','" + account.LName + "','" + account.Email + "','" + account.Username + "','" +
                               account.City + "','" + account.Password + "')";
            //MySqlConnection connection1 = new MySqlConnection();
            connection.Open();
            cmd = new MySqlCommand(registerUser, connection);
            adp = new MySqlDataAdapter(cmd);

            adp.Fill(ds);

            connection.Close();
            cmd.Dispose();


            /* if (ModelState.IsValid)
             {


                 using (DbMyContext db = new DbMyContext())
                 {

                     db.customer.Add(account);
                     db.SaveChanges();
                 }
                 //clear content of all input controls
                 ModelState.Clear();
                 
             }*/
            ViewBag.message = account.FName + " " + account.LName + " successfully registered";
            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Customer userLoggin)
        {
            connection.Open();

            using (var db = new DbMyContext())
            {
                var retrieveUserForLoggin = userLoggin.Username;

                //var usr = db.customer.Where(u=> retrieveUserForLoggin == userLoggin.Username).FirstOrDefault();
                //var usrPSW = db.customer.Where(u=> u.Password == userLoggin.Password).FirstOrDefault();

                var retrieveUser = "SELECT * FROM user WHERE Username='" + userLoggin.Username + "';";
                cmd = new MySqlCommand(retrieveUser, connection);
                var mdr = cmd.ExecuteReader();
                if (mdr.Read())
                {
                    Session["UserID"] = mdr.GetString("idUser");
                    //usr.Id.ToString();
                    Session["Username"] = mdr.GetString("Username");
                    //usr.Username.ToString();
                    mdr.Close();
                    return RedirectToAction("Loggedin");
                }

                ModelState.AddModelError("", "Check your credentials");
                return View();
            }
        }

        public ActionResult Loggedin()
        {
            if (Session["UserID"] != null)
            {
                // database
                var retrieveUsername = Session["Username"].ToString();
                var retrieveUserId = Session["UserID"].ToString();
                string[] ports = SerialPort.GetPortNames();
                for (var j = 0; j < ports.Length; j++)
                {
                    if (SerialPort.GetPortNames().Any(x => x == ports[j])) portAvailable = ports[j];
                    break;
                }

                if (portAvailable == null)
                {
                    ViewBag.DeviceOff = false;
                    ViewBag.DailyTempOff = "DEVICE OFF";
                }
                else
                {
                    var port = new SerialPort(portAvailable, 9600);
                    x8 = SerialPort.GetPortNames().Any(x => x == portAvailable);
                    //THIS SHOULD BE CHANGED AND TESTED 
                    if (x8)
                    {
                        port.Open();
                        var nowDateTime = DateTime.Now;
                        var dateSubmitting = nowDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        port.WriteLine("connected"); //send to arduino
                        var txt = port.ReadLine();
                        if (dateSubmitting.Length > 0) dateSubmitting = dateSubmitting.Remove(dateSubmitting.Length - 13);
                        if (txt.Length > 5) txt = txt.Remove(txt.Length - 1);

                        var x = double.Parse(txt);

                        ViewBag.DailyTemp = x;


                        insertToDbTemperature(x, dateSubmitting);
                        port.Dispose();
                        port.Close();
                    }
                    else
                    {
                        ViewBag.DeviceOff = false;
                        ViewBag.DailyTempOff = "DEVICE OFF";
                    }
                }
                


                connection.Open();
                var retrieveUsernamev = Session["Username"].ToString();
                //string retrieveUsername = userLoggin.Username.ToString();
                var retrieveUser = "SELECT * FROM temperature WHERE Username='" + retrieveUsernamev + "';";
                string x1 = null;
                string tempDates;
                DateTime dates;
                try
                {
                    cmd = new MySqlCommand(retrieveUser, connection);

                    //MySqlDataReader mdr = cmd.ExecuteReader();
                    var ds = new DataTable();
                    var adapter = new MySqlDataAdapter(retrieveUser, connection);


                    adapter.Fill(ds);

                    foreach (DataRow row in ds.Rows)
                    {
                        x1 = row["Temperature"].ToString();

                        DateTime.TryParse(row["dateTemp"].ToString(), out dates);

                        intList.Add(x1);
                        tempDates = dates.ToString("Y");
                        dateList.Add(Convert.ToDateTime(tempDates));
                    }

                    adapter.Dispose();


                    ViewBag.Temperatures = intList;


                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


                return View();
            }

            return RedirectToAction("Login");
        }

        public ActionResult Logoff()
        {
            if (Session["UserID"] != null)
            {
                Session.Abandon();
                return RedirectToAction("Login");
            }

            return View();
        }

        public ActionResult tableDanger()
        {
            if (Session["UserID"] != null)
                if (x8)
                {
                    var port = new SerialPort(portAvailable, 9600);
                    port.Open();
                    port.WriteLine("connected"); //send to arduino
                    var txt = port.ReadLine();

                    if (txt.Length > 5) txt = txt.Remove(txt.Length - 1);

                    var x = double.Parse(txt);
                    ViewBag.DailyTemp = x;
                }

            return View();
        }

        public ActionResult myChart()
        {
            var port = new SerialPort(portAvailable, 9600);
            port.Open();
            port.WriteLine("connected"); //send to arduino
            var txt = port.ReadLine();

            if (txt.Length > 5) txt = txt.Remove(txt.Length - 1);

            var x = double.Parse(txt);

            port.Dispose();

            new System.Web.Helpers.Chart(800, 200, ChartTheme.Green)
                .AddTitle("Temperature")
                .AddSeries(
                    "Month",
                    "column",
                    xValue: new[] {"Jan", "Feb", "Mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec"},
                    yValues: new[] {x}).Write("png");
            return View();
        }

        public ActionResult dailyTemp()
        {
            var port = new SerialPort(portAvailable, 9600);
            port.Open();
            port.WriteLine("connected"); //send to arduino
            var txt = port.ReadLine();

            if (txt.Length > 5) txt = txt.Remove(txt.Length - 1);


            var mych = new System.Web.Helpers.Chart(800, 200, ChartTheme.Green);
            var x = double.Parse(txt);
            port.Dispose();

            mych.AddTitle("Temperature")
                .AddSeries(
                    "Daily",
                    "column",
                    xValue: new[] {"Today"},
                    yValues: new[] {x}).Write("png");
            ViewData["gauge"] = txt;
            //kayChart serialData = new kayChart(mych,60);
            return View();
        }

        public void insertToDbTemperature(double x, string date)
        {
            connection.Open();
            var retrieveUsername = Session["Username"].ToString();
            var retrieveUserId = Session["UserID"].ToString();
            var registerTemp = "INSERT INTO temperature (idTemperature,temperature,Username,dateTemp) VALUES ('" +
                               retrieveUserId + "','" + x + "','" + retrieveUsername + "','" + date + "')";

            cmd = new MySqlCommand(registerTemp, connection);
            adp = new MySqlDataAdapter(cmd);

            adp.Fill(ds);
            connection.Close();
            cmd.Dispose();
            Thread.Sleep(10000);
        }
    }
}