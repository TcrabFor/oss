using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oss.Controllers
{
    public class StaffManagersController : Controller
    {
        Models.OSMSEntities1 db = new Models.OSMSEntities1();
        // GET: StaffManagers
        public ActionResult Index()
        {
            ViewData["S_ID"] = db.Staff.ToList();
            return View();
        }
        //查询
        public ActionResult SelectStaff()
        {
            int page = Convert.ToInt32(Request["page"]);
            int limit = Convert.ToInt32(Request["limit"]);
            var list = (from s in db.Staff
                        select new
                        {
                            S_ID = s.Id,//重新声明字段
                            S_Name = s.Name,
                            S_Sex = s.Sex
                        }).ToList();
            return Json(new
            {
                code = 0,
                msg = "",
                count = list.Count(),
                data = list.OrderBy(s => s.S_ID).Skip((page - 1) * limit).Take(limit).ToList()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectRole()
        {
            int page = Convert.ToInt32(Request["page"]);
            int limit = Convert.ToInt32(Request["limit"]);
            var list = (from r in db.Role
                        select new
                        {
                            R_ID = r.Id,
                            //重新声明字段
                            R_Name = r.Name,
                            R_Code = r.Code

                        }).ToList();


            return Json(new { code = 0, msg = "", count = list.Count(), data = list.OrderBy(r => r.R_ID).Skip((page - 1) * limit).Take(limit).ToList() }, JsonRequestBehavior.AllowGet);//将集合转换成json格式
        }
    }
}