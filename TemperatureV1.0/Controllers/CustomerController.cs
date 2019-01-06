using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Windows.Forms;
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

            ViewBag.message = account.FName + " " + account.LName;
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

        private int[] temperaturesOfTheDay;
        double maxTemp;
       
        static double x;
        double minTemp = x;
        public ActionResult Loggedin()
        {
            var nowDateTime = DateTime.Now;
            var dateSubmitting = nowDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
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
                        
                        port.WriteLine("connected"); //send to arduino
                        var txt = port.ReadLine();
                        if (dateSubmitting.Length > 0) dateSubmitting = dateSubmitting.Remove(dateSubmitting.Length - 13);
                        if (txt.Length > 5) txt = txt.Remove(txt.Length - 1);

                        x = double.Parse(txt);
                        
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
                        x1 = row["temperature"].ToString();

                        DateTime.TryParse(row["dateTemp"].ToString(), out dates);

                        intList.Add(x1);
                        tempDates = dates.ToString("Y");
                        dateList.Add(Convert.ToDateTime(tempDates));
                    }

                    adapter.Dispose();
                    cmd.Dispose();
                    ds.Dispose();

                    
                    var retrieveMaxTemp = "SELECT MAX(temperature) from temperature WHERE Username='" + retrieveUsernamev +
                                          "' AND dateTemp='" + dateSubmitting + "';";
                    var retrieveMinTemp = "SELECT MIN(temperature) from temperature WHERE Username='" + retrieveUsernamev +
                                          "' AND dateTemp='" + dateSubmitting + "';";

                    var dsO = new DataTable();
                    var adapterO = new MySqlDataAdapter(retrieveMaxTemp, connection);
                    adapterO.Fill(dsO);
                    foreach (DataRow row in dsO.Rows)
                    {
                        maxTemp = Convert.ToDouble(row["MAX(temperature)"]);


                    }
                    dsO.Dispose();
                    adapterO.Dispose();
                    var dsM = new DataTable();
                    var adapterM = new MySqlDataAdapter(retrieveMinTemp, connection);
                    adapterM.Fill(dsM);
                    foreach (DataRow row in dsM.Rows)
                    {
                        minTemp = Convert.ToDouble(row["MIN(temperature)"]);


                    }
                    ViewBag.Temperatures = intList;

                    ViewBag.maxTemperature = maxTemp;
                    ViewBag.minTemperature = minTemp;


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
            ViewBag.TestChartJS = 1;
            return View();
        }

        public ActionResult ourProject()
        {
           
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