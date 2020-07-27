using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oss.Controllers
{
    public class OrganizationStructureController : Controller
    {
        Models.OSMSEntities1 db = new Models.OSMSEntities1();
        // GET: OrganizationStructure
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SearchOrganizationStructure()
        {
            var list = (from org in db.OrganizationStructure
                        select new
                        {
                            Id=org.Id,
                            Code =org.Code,
                            Name=org.Name,
                            Leve=org.Leve,
                            Pid=org.ParentId,
                            CreateTime=org.CreateTime,
                            UpdateTime=org.UpdateTime,
                            IsDel=org.IsDel
                        }).ToList();

            return Json(new { msg = "", code = 0, data = list }, JsonRequestBehavior.AllowGet);//将集合转换成json格式
        }
    }
}