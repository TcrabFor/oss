using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oss.Controllers
{
    public class RoleManagersController : Controller
    {
        Models.OSMSEntities1 db = new Models.OSMSEntities1();
        // GET: RoleManagers
        public ActionResult Index()
        {
            //绑定id
            ViewData["R_ID"] = db.Role.ToList();
            ViewData["R_Name"] = db.Role.ToList();
            return View();
        }

        //查询
        public ActionResult SelectRole()
        {
            int page = Convert.ToInt32(Request["page"]);
            int limit = Convert.ToInt32(Request["limit"]);
            page = Convert.ToInt32(Request["page"]);
            var list = (from r in db.Role
                        select new
                        {
                            R_ID = r.Id,//重新声明字段
                            R_Name = r.Name,
                            R_Code = r.Code

                        }).ToList();


            return Json(new { code = 0, msg = "", count = list.Count(), data = list.OrderBy(r => r.R_ID).Skip((page - 1) * limit).Take(limit).ToList() }, JsonRequestBehavior.AllowGet);//将集合转换成json格式
        }
        //查询Tree
        public ActionResult SelectSystem()
        {

            var list = (from s in db.SystemResourceModule
                        select new
                        {
                            Id = s.Id/*== null ? new Guid ("{e10ce31c-124c-4398-b118-1d5bf6dd39f3}") :s.Id*/,
                            Name = s.Name,
                            Code = s.Code,
                            Url = s.Url,
                            Type = s.Type,
                            Pid = s.ParentId/* == null ? new Guid("{e10ce31c-124c-4398-b118-1d5bf6dd39f3}") : s.ParentId*/,

                        }).ToList();


            return Json(new { msg = "", code = 0, data = list }, JsonRequestBehavior.AllowGet);//将集合转换成json格式
        }
        //修改
        [HttpPost]
        public ActionResult RoleUp(Guid R_ID,string R_Name,string R_Code)
        {

            Models.Role r = new Models.Role();
            var update = db.Role.SingleOrDefault(ro=>ro.Id== R_ID);
                     
            //Guid RID = new Guid("{00000000-0000-0000-0000-000000000000}");
            if (update.Id==null)
            {
                return Json(false);
                
            }
            else
            {
              
                update.Name = R_Name;
                update.Code = R_Code;
                //db.Role.Add(r);
                int i = db.SaveChanges();
                return Json(i > 0, JsonRequestBehavior.AllowGet);
               
            }          
        }
        [HttpPost]
        //删除
        public ActionResult RoleDel(string RID)
        {
            Guid id = new Guid(RID);
            var del = db.Role.SingleOrDefault(r => r.Id == id);//ling删除语句
            if (del.Id == null)
            {
                return Json(false);
            }
            else
            {
                db.Role.Remove(del);
                int i=db.SaveChanges();
                return Json(i > 0, JsonRequestBehavior.AllowGet);
            }
          
        }
         
        [HttpPost]
        //添加
        public ActionResult AddAll(string RName, string RCode)
        {
            Models.Role r = new Models.Role();
            Guid rid = Guid.NewGuid();
            r.Id = rid;
            r.Name = RName;
            r.Code = RCode;
            db.Role.Add(r);
            int i = db.SaveChanges();
            return Json(i > 0, JsonRequestBehavior.AllowGet);
        }
    }
}