using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oss.Controllers
{
    public class OilMaterialOrderController : Controller
    {
        Models.OSMSEntities1 db = new Models.OSMSEntities1();
        // GET: OilMaterialOrder
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SelectOilM() {
            //session实体对象取值
            Models.Staff st = (Models.Staff)Session["U_Name"];
            var Name = st.Name;
            var list = (from oi in db.OilMaterialOrder
                        join s in db.Staff on oi.ApplyPersonId equals s.Id
                        where s.Name==Name
                        select 
                        new {
                            No=oi.No,
                            ApplyName=s.Name,
                            ApplyDate=oi.ApplyDate,
                            Remark=oi.Remark,
                            IsDel=oi.IsDel,
                            CreateTime=oi.CreateTime,
                            UpdateTime=oi.UpdateTime
                        }).ToList();

            return Json(new {code=0,msg="",data=list },JsonRequestBehavior.AllowGet);
        }
    }
}