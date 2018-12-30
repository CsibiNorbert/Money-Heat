using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using TemperatureV1._0.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO.Ports;
using System.Threading;
using System.Web.Services.Description;


namespace TemperatureV1._0.Controllers
{
    public class CustomerController : Controller
    {
        //MySqlConnection connection = new MySqlConnection();
        //private string strConnString = ConfigurationManager.ConnectionStrings["MySqlTemperature"].ConnectionString;
        MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;database=customer;password=Clujnapoca2019;Convert Zero Datetime=true;");
        private MySqlCommand cmd = new MySqlCommand();
        MySqlDataAdapter adp = new MySqlDataAdapter();
        private DataSet ds = new DataSet();
        private string[] ports = SerialPort.GetPortNames();
        private string portAvailable;



        private Boolean x8;
        public  ActionResult Register()
        {
            return View();
        }

        
       

        [HttpPost]
        public ActionResult Register(Customer account)
        {

            
            string registerUser = "INSERT INTO user (FName,LName,Email,Username,City,Password) VALUES ('" + account.FName + "','" + account.LName + "','" + account.Email + "','" + account.Username + "','" + account.City + "','" + account.Password + "')";
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
            
            using (DbMyContext db = new DbMyContext())
            {

                string retrieveUserForLoggin = userLoggin.Username.ToString();

                //var usr = db.customer.Where(u=> retrieveUserForLoggin == userLoggin.Username).FirstOrDefault();
                //var usrPSW = db.customer.Where(u=> u.Password == userLoggin.Password).FirstOrDefault();

                string retrieveUser = "SELECT * FROM user WHERE Username='"+ userLoggin.Username.ToString() +"';";
                cmd = new MySqlCommand(retrieveUser, connection);
                MySqlDataReader mdr = cmd.ExecuteReader();
                if (mdr.Read() !=false)
                {
                    Session["UserID"] = mdr.GetString("idUser");
                        //usr.Id.ToString();
                    Session["Username"] = mdr.GetString("Username");
                    //usr.Username.ToString();
                    mdr.Close();
                    return RedirectToAction("Loggedin");
                }
                else
                {
                    ModelState.AddModelError("", "Check your credentials");
                }
                return View();
            }
                
        }

        private int i = 0;
        List<string> intList = new List<string>();
        List<DateTime> dateList = new List<DateTime>();
        private Boolean serialPortFlag = false;
        
        public ActionResult Loggedin()
        {
           

            if (Session["UserID"] != null )
            {
               

                // database
                string retrieveUsername = Session["Username"].ToString();
                string retrieveUserId = Session["UserID"].ToString();
                for (int j = 0; j < ports.Length; j++)
                {
                    if (SerialPort.GetPortNames().Any(x => x == ports[j]))
                    {
                        portAvailable = ports[j];
                    }
                    break;
                }
                SerialPort port = new SerialPort(portAvailable, 9600);
                x8 = SerialPort.GetPortNames().Any(x => x == portAvailable);
                //THIS SHOULD BE CHANGED AND TESTED 
                if (x8 != false)
                {
                    port.Open();
                    DateTime nowDateTime = DateTime.Now;
                    string dateSubmitting = nowDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    port.WriteLine("connected"); //send to arduino
                    string txt = port.ReadLine();
                    if (dateSubmitting.Length>0)
                    {
                        dateSubmitting = dateSubmitting.Remove(dateSubmitting.Length - 13);
                    }
                    if (txt.Length > 5)
                    {
                        txt = txt.Remove(txt.Length - 1);
                    }

                    double x = double.Parse(txt);

                    ViewBag.DailyTemp = x;


                    insertToDbTemperature(x, dateSubmitting);
                    port.Close();
                }
                else
                {
                    ViewBag.DeviceOff = false;
                    ViewBag.DailyTempOff = "DEVICE OFF";
                }

                

                connection.Open();
                string retrieveUsernamev = Session["Username"].ToString();
                //string retrieveUsername = userLoggin.Username.ToString();
                string retrieveUser = "SELECT * FROM temperature WHERE Username='" + retrieveUsernamev.ToString() + "';";
                string x1 = null;
                string tempDates;
                DateTime dates;
                try
                {

                    cmd = new MySqlCommand(retrieveUser, connection);
                   
                    //MySqlDataReader mdr = cmd.ExecuteReader();
                    DataTable ds = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(retrieveUser, connection);
                   
                   
                   
                    adapter.Fill(ds);
                   
                    foreach (DataRow row in ds.Rows)
                    {
                         x1 = row["Temperature"].ToString();
                       
                        DateTime.TryParse(row["dateTemp"].ToString(), out dates);
                        
                        intList.Add(x1);
                        tempDates = dates.ToString("Y").ToString();
                        dateList.Add(Convert.ToDateTime(tempDates));
                      
                    }
                    adapter.Dispose();



                   
                    ViewBag.Temperatures = intList;







                  

                    port.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
                
                
                return View();
                
            }
            else
            {
                return RedirectToAction("Login");
            }
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
            {//test it
                if (x8 != false)
                {
                    SerialPort port = new SerialPort(portAvailable, 9600);
                    port.Open();
                    port.WriteLine("connected"); //send to arduino
                    string txt = port.ReadLine();

                    if (txt.Length > 5)
                    {
                        txt = txt.Remove(txt.Length - 1);
                    }

                    double x = double.Parse(txt);
                    ViewBag.DailyTemp = x;
                }


                //ViewBag.DailyTemp = "DEVICE OFF";


            }
            return View();
        }
        public ActionResult myChart()
        {
           
           

            SerialPort port = new SerialPort(portAvailable, 9600);
            port.Open();
            port.WriteLine("connected"); //send to arduino
            string txt = port.ReadLine();
           
            if (txt.Length > 5)
            {
                txt = txt.Remove(txt.Length - 1);
            }

            double x = double.Parse(txt);
            
            port.Dispose();

            new System.Web.Helpers.Chart(width: 800, height: 200, theme: ChartTheme.Green)
                .AddTitle("Temperature")
                .AddSeries(
                    name:"Month",
                    chartType: "column",
                    xValue: new[] { "Jan", "Feb", "Mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" },
                    yValues: new[] {x}).Write("png");
            return View();
        }
       
       public ActionResult dailyTemp()
        {
            

            SerialPort port = new SerialPort(portAvailable, 9600);
            port.Open();
            port.WriteLine("connected"); //send to arduino
            string txt = port.ReadLine();

            if (txt.Length > 5)
            {
                txt = txt.Remove(txt.Length - 1);
            }
            

            System.Web.Helpers.Chart mych = new System.Web.Helpers.Chart(width: 800, height: 200, theme: ChartTheme.Green);
            double x = double.Parse(txt);
            port.Dispose();

            mych.AddTitle("Temperature")
                .AddSeries(
                    name: "Daily",
                    chartType: "column",
                    xValue: new[] { "Today"},
                    yValues: new[] { x }).Write("png");
            ViewData["gauge"] = txt;
            //kayChart serialData = new kayChart(mych,60);
            return View();
        }
        
        public void insertToDbTemperature(double x,string date)
        {
            connection.Open();
            string retrieveUsername = Session["Username"].ToString();
            string retrieveUserId = Session["UserID"].ToString();
            string registerTemp = "INSERT INTO temperature (idTemperature,temperature,Username,dateTemp) VALUES ('" +
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