using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oss.Controllers
{
    public class JobController : Controller
    {
        Models.OSMSEntities1 db = new Models.OSMSEntities1();
        // GET: Job
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SelectJob() {

            var list = (from j in db.Job
                        select new {
                            JName=j.Name,
                            JCode=j.Code

                        }).ToList();
            return Json(new {msg="",code=0,data=list },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //添加
        public ActionResult AddAll(string Name, string Code)
        {
            Models.Job J = new Models.Job();
            Guid rid = Guid.NewGuid();
            J.Id = rid;
            J.Name = Name;
            J.Code = Code;
            string  UpdateTime=DateTime.Now.ToLocalTime().ToString();
            UpdateTime =Convert.ToString(J.UpdateTime);
            string CreateTime = DateTime.Now.ToLocalTime().ToString();
            CreateTime= Convert.ToString(J.CreateTime);
            string IsDel = "0";
            IsDel = Convert.ToString(J.IsDel);
            db.Job.Add(J);
            int i = db.SaveChanges();
            return Json(i > 0, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //删除
        public ActionResult JobDel(string JName)
        {           
            var del = db.Job.SingleOrDefault(j => j.Name == JName);//ling删除语句
            if (del.Name == null)
            {
                return Json(false);
            }
            else
            {
                db.Job.Remove(del);
                int i = db.SaveChanges();
                return Json(i > 0, JsonRequestBehavior.AllowGet);
            }

        }
        //修改
        [HttpPost]
        public ActionResult JobUp( string JName, string JCode)
        {

            Models.Job j = new Models.Job();
            var update = db.Job.SingleOrDefault(job => job.Name == JName);

            if (update.Name == null)
            {
                return Json(false);

            }
            else
            {
                update.Name = JName;
                update.Code = JCode;
                update.CreateTime= DateTime.Now.ToLocalTime();
                update.UpdateTime = DateTime.Now.ToLocalTime();
                update.IsDel = true;

                int i = db.SaveChanges();
                return Json(i > 0, JsonRequestBehavior.AllowGet);

            }
        }
    }
}