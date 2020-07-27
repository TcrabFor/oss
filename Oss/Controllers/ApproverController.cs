using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Oss.Controllers
{
    public class ApproverController : Controller
    {
        Models.OSMSEntities1 db = new Models.OSMSEntities1();
        // GET: Approver
        public ActionResult Index()
        {
            ViewData["Type"] = db.ProcessItem.ToList();
            ViewData["AreaLeve"] = db.Approver.ToList();

            return View();
        }

        //查询
        public ActionResult SelectApprover()
        {
            int page = Convert.ToInt32(Request["page"]);
            int limit = Convert.ToInt32(Request["limit"]);
            var list = (from a in db.Approver
                        join p in db.ProcessItem
                        on a.ProcessItemId equals p.Id
                        where p.Discrible== "油料申请审批流程"
                        orderby a.OrderApps descending
                        select new
                        {
                            //重新声明字段
                            ID = a.Id,
                            JobCode = a.JobCode,
                            AreaLeve = a.AreaLeve,
                            A_Discrible = a.Discrible,
                            OrderApps = a.OrderApps,
                            ProcessItemId = a.ProcessItemId,
                            ExecuteMethod = a.ExecuteMethod,

                            P_Discrible = p.Discrible,
                            Code = p.Code
                        }).ToList();


            return Json(new { code = 0, msg = "", count = list.Count(), data = list.OrderBy(a => a.ID).Skip((page - 1) * limit).Take(limit).ToList() }, JsonRequestBehavior.AllowGet);//将集合转换成json格式
        }

        public ActionResult Selected(string Discrible)
        {
            #region   lambad表达式
            //var approver = (db.Approver.Join(db.ProcessItem, app => app.ProcessItemId,
            //    pro => pro.Id, (app, pro) => new
            //    {
            //        Discrible = pro.Discrible,
            //    }).Select(pro => pro.Discrible).Distinct()).ToList();
            #endregion
            var approver = (from app in db.Approver
                            join pro in db.ProcessItem on app.ProcessItemId equals pro.Id
                            where pro.Discrible== Discrible
                            group new { pro.Discrible } by pro.Discrible into g
                            select new { g.Key }).ToList();

            return Json(new { msg = "", code = 0, data = 0 }, JsonRequestBehavior.AllowGet);
        }
    }
}