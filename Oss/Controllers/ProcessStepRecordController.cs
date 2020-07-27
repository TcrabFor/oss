using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oss.Controllers
{
    public class ProcessStepRecordController : Controller
    {
        Models.OSMSEntities1 db = new Models.OSMSEntities1();
        // GET: ProcessStepRecord
        public ActionResult Index()
        {
            return View();
        }

        //查询
        public ActionResult SelectProcessStepRecord()
        {
            int page = Convert.ToInt32(Request["page"]);
            int limit = Convert.ToInt32(Request["limit"]);
            page = Convert.ToInt32(Request["page"]);
            var list = (from psr in db.ProcessStepRecord
                        select new
                        {
                            //重新声明字段
                            ID=psr.Id,
                            Type = psr.Type,
                            RecordRemarks=psr.RecordRemarks,
                            StepOrder=psr.StepOrder,
                            WaitForExecutionStaffId=psr.WaitForExecutionStaffId,
                            CreateTime=psr.CreateTime,
                            UpdateTime=psr.UpdateTime,
                            WhetherToExecute=psr.WhetherToExecute,
                            No=psr.No,
                            RefOrderId=psr.RefOrderId,
                            Result=psr.Result,
                            ExecuteMethod=psr.ExecuteMethod,
                            Discrible=psr.Discrible
                        }).ToList();


            return Json(new { code = 0, msg = "", count = list.Count(), data = list.OrderBy(psr => psr.ID).Skip((page - 1) * limit).Take(limit).ToList() }, JsonRequestBehavior.AllowGet);//将集合转换成json格式
        }

    }
}