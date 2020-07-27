using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oss.Controllers
{
    public class SystemResourceModuleController : Controller
    {
        Models.OSMSEntities1 db = new Models.OSMSEntities1();
        // GET: SystemResourceModule
        public ActionResult Index()
        {
            return View();
        }
        //查询
        public ActionResult SelectSystem()
        {
           
            var list = (from s in db.SystemResourceModule
                        select new
                        {
                            Id = s.Id/*== null ? new Guid ("{e10ce31c-124c-4398-b118-1d5bf6dd39f3}") :s.Id*/,
                            Name=s.Name,
                            Code = s.Code,
                            Url = s.Url,
                            Type = s.Type,
                            Pid = s.ParentId/* == null ? new Guid("{e10ce31c-124c-4398-b118-1d5bf6dd39f3}") : s.ParentId*/,

                        }).ToList();


            return Json(new {msg="",code=0, data = list } ,JsonRequestBehavior.AllowGet);//将集合转换成json格式
        }
    }
}