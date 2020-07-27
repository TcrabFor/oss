using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oss.Controllers
{
    public class LeaveApproverController : Controller
    {
        Models.OSMSEntities1 db = new Models.OSMSEntities1();
        // GET: LeaveApprover
        public ActionResult Index()
        {
            ViewData["Name"] = db.Staff.ToList();
            ViewData["JobId"] = db.Job.ToList();
            return View();
        }

        #region
        #endregion

        #region //加载数据
        public ActionResult SelectLeaveApprover()
        {
            Models.Staff st = (Models.Staff)Session["U_Name"];
            var list = (from psr in db.ProcessStepRecord
                        join s in db.Staff on psr.WaitForExecutionStaffId equals s.Id
                        join lo in db.LeaveOffice on psr.RefOrderId equals lo.Id
                        where psr.WaitForExecutionStaffId == st.Id && lo.IsDel == false
                        orderby psr.StepOrder descending
                        select new
                        {
                            //流程记录Id
                            PsrId = psr.Id,
                            //关联单据ID
                            RefOrderId = psr.RefOrderId,
                            //待执行人id
                            WaitForExecutionStaffId = psr.WaitForExecutionStaffId,
                            //申请人id
                            ApplyPersonId = lo.ApplyPersonId,
                            //交接人ID
                            HandoverSatffId = lo.HandoverSatffId,
                            //申请id
                            LeaveID = lo.Id,
                            No = lo.No,
                            JobName = db.Job.Where(jo => jo.Id == lo.JobId).Select(jo => jo.Name).FirstOrDefault(),
                            //是否通过
                            Result = psr.Result,
                            StaffName = lo.StaffName,
                            CreateTime = lo.CreateTime,
                            LeaveType = lo.LeaveType,
                            ExplanationHandover = lo.ExplanationHandover,
                        }).ToList();
            if (list.Count > 0) { }
            return Json(new { code = 0, msg = "", data = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region //审批=>流程视图       
        public ActionResult ApproverView(Guid LeaveID)
        {
            Models.Staff st = (Models.Staff)Session["U_Name"];
            var list = (from psr in db.ProcessStepRecord
                        join s in db.Staff on psr.WaitForExecutionStaffId equals s.Id
                        join lo in db.LeaveOffice on psr.RefOrderId equals lo.Id
                        where lo.IsDel == false && lo.Id == LeaveID
                        orderby psr.StepOrder descending
                        select new
                        {
                            OrgName = db.OrganizationStructure.Where(os => os.Id == s.OrgID).Select(ost => ost.Name).FirstOrDefault(),
                            TakeName = s.Name,
                            Job = db.Job.Where(job => job.Id == s.JobId).Select(ss => ss.Name).FirstOrDefault(),
                            Result = psr.Result
                        }).ToList();
            return Json(new { code = 0, msg = "", data = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region //未通过=>修改Result
        [HttpPost]
        public ActionResult LeaveUpdate(Guid PsrId, bool Result, Guid RefOrderId)
        {
            /*
             *Result?ture:false true为通过，false未通过   
             * 通过顺序来判断前面是否已经通过
             */
            if (Result == true)
            {
                return Json(false);
            }
            #region  //判断前面是否没有通过:通过就给修改,
            var update = db.ProcessStepRecord.SingleOrDefault(psr => psr.Id == PsrId);
            int i = 0;
            while (i < update.StepOrder)
            {
                //循环通过条件判断是否前面有未处理的流程
                var selResult = db.ProcessStepRecord.SingleOrDefault(proc => proc.StepOrder == i && proc.Result == true && proc.RefOrderId == RefOrderId);
                if (selResult.Result == false)
                {
                    return Json(false);
                };
                i++;
            };
            Models.OrganizationStructure organization = (Models.OrganizationStructure)Session["Organization"];
            Models.Job job = (Models.Job)Session["Name"];
            update.Discrible = $"[{organization.Name}][{job.Name}]已通过";
            update.Result = true;
            int m = db.SaveChanges();
            return Json(m > 0, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region  //驳回=>删除流程和申请单
        [HttpPost]
        public ActionResult LeaveApproverUp(Guid RefOrderId, bool Result, Guid LeaveID)
        {
            if (Result == true)
            {
                return Json(false);
            }

            var delProcess = db.ProcessStepRecord.Where(sel => sel.RefOrderId == RefOrderId).Select(sel => sel.Id).ToList();
            var delLeaveOffice = db.LeaveOffice.SingleOrDefault(leave => leave.Id == LeaveID);
            if (delProcess == null)
            {

                return Json(false);
            }
            else
            {
                int i = 0;
                while (i<delProcess.Count) {
                    Guid guid = delProcess[i];
                    var del = db.ProcessStepRecord.SingleOrDefault(psr=>psr.Id==guid&&psr.RefOrderId==RefOrderId);
                    db.ProcessStepRecord.Remove(del);
                    i++;
                }              
                db.LeaveOffice.Remove(delLeaveOffice);
                int m = db.SaveChanges();
                return Json(m> 0, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion

    }
}