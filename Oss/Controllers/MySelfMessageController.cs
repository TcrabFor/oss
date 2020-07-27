using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oss.Controllers
{

    /// <summary>
    /// 个人基础信息展示
    /// </summary>
    public class MySelfMessageController : Controller
    {
        Models.OSMSEntities1 db = new Models.OSMSEntities1();
        // GET: MySelfMessage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetMyself() {
            Models.Staff st = (Models.Staff)Session["U_Name"];

            Models.Job jo = (Models.Job)Session["Name"];

            Models.OrganizationStructure org = (Models.OrganizationStructure)Session["Organization"];

            Models.Role RoName = (Models.Role)Session["RoleName"];

            List<string> list = new List<string>
            {
                st.No,
                st.Name,
                jo.Name,
                org.Name,
                RoName.Name
            };
            return Json(new { msg = "", code = 0, data = list }, JsonRequestBehavior.AllowGet);
        }
    }
}