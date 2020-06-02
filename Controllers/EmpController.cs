using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net;                       // add this for authenticating
using WebApplication3.com.attendanceondemand.aodws;
using WebApplication3.Models;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace WebApplication3.Controllers

{
    public class EmpController : Controller
    {
        // GET: Emp
        public ActionResult Index()
        {
     
            return View();
        }
        public ActionResult Index3()
        {
            IAeXMLBridgeservice webServices = new IAeXMLBridgeservice();
            NetworkCredential creds = new NetworkCredential()
            {
                UserName = "NTFoodsWS737",                    // replace with your username/access account
                Password = "P@ssw0rd1"                        // replace with your password
            };
            //TLS
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            webServices.Credentials = creds;
            webServices.PreAuthenticate = true;
            webServices.Timeout = -1;                      // -1 = no timeout, else 1 sec = 1000ms

            webServices.Url = "https://ntfoods.attendanceondemand.com:8192/cc1exec.aew/soap/IAeXMLBridge";
            string[] hyperQueries = webServices.getHyperQueriesSimple();
            DateTime datesch = new DateTime();
            TAeEmployeeBasic[] activeEmps = webServices.getActiveEmployeesList();
            List<Employeesch> schview = new List<Employeesch>();
            
            foreach (TAeEmployeeBasic activeEmp in activeEmps)
            {
             
                schview.Add(new Employeesch(activeEmp.EmpID,activeEmp.EmpName));
            }
                TAeSchedule[] allSchedules = webServices.extractRangedSchedulesUsingHyperQuery(hyperQueries[32], TDateRangeEnum.drCurrWeek, "", "");
            List<string> sche = new List<string>();
            List<string> temp = new List<string>();
            int xc = 0;
            int i = 0;
            int ii = 0;
            foreach (TAeSchedule allSchedule in allSchedules)
            {
                ii = 0;
                for(i= 0; i < schview.Count; i++)
                {
                    if (schview[i].EmpID == allSchedule.EmpID)
                    {
                        ii = 1;
                        break;
                    }
                }
                if (ii == 1)
                {
                    datesch = DateTime.Parse(allSchedule.SchDate);
                    int schweekday = (int)datesch.DayOfWeek;
                    switch (schweekday)
                    {
                        case 1:
                            schview[i].monstart = allSchedule.SchStartTime;
                            schview[i].monend = allSchedule.SchEndTime;
                            break;
                        case 2:
                            schview[i].tuestart = allSchedule.SchStartTime;
                            schview[i].tueend = allSchedule.SchEndTime;
                            break;
                        case 3:
                            schview[i].wedstart = allSchedule.SchStartTime;
                            schview[i].wedend = allSchedule.SchEndTime;
                            break;
                        case 4:
                            schview[i].thustart = allSchedule.SchStartTime;
                            schview[i].thuend = allSchedule.SchEndTime;
                            break;
                        case 5:
                            schview[i].fristart = allSchedule.SchStartTime;
                            schview[i].friend = allSchedule.SchEndTime;
                            break;
                        case 6:
                            schview[i].satstart = allSchedule.SchStartTime;
                            schview[i].satend = allSchedule.SchEndTime;
                            break;

                        default:

                            break;
                    }
                }
                sche.Add(allSchedule.EmpName + "/" + allSchedule.SchDate + "/" + allSchedule.SchStartTime + "/" + allSchedule.SchEndTime);

                xc++;
            }
            ViewBag.sche = sche;
            ViewBag.schview = schview;
            return View();
        }
        public ActionResult Index2()
        {
            string connStr = "server=localhost;user=root;database=larahrm;port=3306;SslMode=none";
           
            using (MySqlConnection connection = new MySqlConnection(connStr))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM employee", connection);
                MySqlDataReader reader = command.ExecuteReader();

                List<string> users = new List<string>();
                                
                while (reader.Read())
                {
                    users.Add(reader["EmpID"] + "\t" + reader["EmpName"]);
                }

                ViewBag.users = users;
                reader.Close();
            }

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateE(EmpViewModels model)
        {
            IAeXMLBridgeservice webServices = new IAeXMLBridgeservice();
            NetworkCredential creds = new NetworkCredential()
            {
                UserName = "NTFoodsWS737",                    // replace with your username/access account
                Password = "P@ssw0rd1"                        // replace with your password
            };
            //TLS
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            webServices.Credentials = creds;
            webServices.PreAuthenticate = true;
            webServices.Timeout = -1;                      // -1 = no timeout, else 1 sec = 1000ms

            webServices.Url = "https://ntfoods.attendanceondemand.com:8192/cc1exec.aew/soap/IAeXMLBridge";

            string empID = model.EmpID;
            // we are using employee with ID "1234" and are going to add

            TAeEmployeeDetail2 empDetail2 = new TAeEmployeeDetail2();
            empDetail2.FirstName = model.EmpFName;
            empDetail2.LastName = model.EmpLName;
            empDetail2.ESSPIN = empID;
            empDetail2.EmpID = empID;
            empDetail2.ActiveStatus = 0;
            empDetail2.ActiveStatusConditionID = 1;
            // note these are not all of the fields in the structure. you can 
            // update any or all fields, as well as those included in the base structures


            TAddEmpMode addEmpMode = TAddEmpMode.aemAuto;
            // setting this to auto will force the method to see if an employee
            // with the EmpID already exists                            

            TBadgeManagement badgeManagement = TBadgeManagement.bmAuto;

            try
            {
                TMaintainEmployeeResult res = webServices.maintainEmployeeDetail2(empDetail2, addEmpMode, badgeManagement);
            }
            catch (Exception ex)
            {
               
            }

            return View("index");
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateS(SchViewModels model)
        {
            IAeXMLBridgeservice webServices = new IAeXMLBridgeservice();
            NetworkCredential creds = new NetworkCredential()
            {
                UserName = "NTFoodsWS737",                    // replace with your username/access account
                Password = "P@ssw0rd1"                        // replace with your password
            };
            //TLS
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            webServices.Credentials = creds;
            webServices.PreAuthenticate = true;
            webServices.Timeout = -1;                      // -1 = no timeout, else 1 sec = 1000ms

            webServices.Url = "https://ntfoods.attendanceondemand.com:8192/cc1exec.aew/soap/IAeXMLBridge";

            string empID = model.EmpID;
            // we will append to employee# 1234

            TAeSchedule2 sched2 = new TAeSchedule2();

            sched2.BenefitID = 0;
            // since we are appending a Pay Designation schedule we set BenefitID to 0

            TAeBasicDataItem[] payDes = webServices.getPayDesignationsSimple();
            // retrieve a list of Pay Designations and codes

            sched2.PayDesID = payDes[0].Num;
            sched2.SchDate = model.SchDate;
            // add the schedule 7 days from now

            sched2.SchEndTime = model.SchEnd;
           
            // total time of hours in minutes
            sched2.SchPattID = 0;
            //sched2.SchRate = 12.50;
            // set employee rate for schedule

            sched2.SchStartTime = model.SchStart;
            sched2.SchStyle = 0;
            sched2.SchType = TSchTypeEnum.steNormal;
            // type of schedule followed

            TAeEmployeeBasic emp = webServices.getEmployeeBasicByIDNum(empID);
            // retrieve workgroup information

            sched2.SchWG1 = emp.WG1;
            sched2.SchWG2 = emp.WG2;
            sched2.SchWG3 = emp.WG3;

            try
            {
                webServices.appendEmployeeSchedule2ByIDNum(empID, sched2);
            }
            catch (Exception ex)
            {
                
            }

            return View("index2");
        }
        /*       [HttpPost]
               [AllowAnonymous]
               [ValidateAntiForgeryToken]
               public async Task<ActionResult> CreateE(EmpViewModels model, string returnUrl)
               {

               }
          */
        // GET: Emp/Details/5
        public ActionResult Details(int id)
        {
            IAeXMLBridgeservice webServices = new IAeXMLBridgeservice();
            NetworkCredential creds = new NetworkCredential()
            {
                UserName = "NTFoodsWS737",                    // replace with your username/access account
                Password = "P@ssw0rd1"                        // replace with your password
            };
            //TLS
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            webServices.Credentials = creds;
            webServices.PreAuthenticate = true;
            webServices.Timeout = -1;                      // -1 = no timeout, else 1 sec = 1000ms

            webServices.Url = "https://ntfoods.attendanceondemand.com:8192/cc1exec.aew/soap/IAeXMLBridge";

            string empID = "900";
            // we will append to employee# 1234

            TAeSchedule2 sched2 = new TAeSchedule2();

            sched2.BenefitID = 0;
            // since we are appending a Pay Designation schedule we set BenefitID to 0

            TAeBasicDataItem[] payDes = webServices.getPayDesignationsSimple();
            // retrieve a list of Pay Designations and codes

            sched2.PayDesID = payDes[0].Num;
            sched2.SchDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            // add the schedule 7 days from now

            sched2.SchEndTime = "18:00";

            // total time of hours in minutes

            sched2.SchHoursHund = 3.5;
            // convert SchHours to numerical representation

            sched2.SchPattID = 0;
            sched2.SchRate = 12.50;
            // set employee rate for schedule

            sched2.SchStartTime = "08:30";
            sched2.SchStyle = 0;
            sched2.SchType = TSchTypeEnum.steNormal;
            // type of schedule followed

            TAeEmployeeBasic emp = webServices.getEmployeeBasicByIDNum(empID);
            // retrieve workgroup information

            sched2.SchWG1 = emp.WG1;
            sched2.SchWG2 = emp.WG2;
            sched2.SchWG3 = emp.WG3;

            try
            {
                webServices.appendEmployeeSchedule2ByIDNum(empID, sched2);
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        // GET: Emp/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Emp/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Emp/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Emp/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Emp/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Emp/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
