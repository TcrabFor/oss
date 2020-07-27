using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oss.Controllers
{ 
    public class LeaveOfficeController : Controller
    {
        Models.OSMSEntities1 db = new Models.OSMSEntities1();
        // GET: LeaveOffice
        public ActionResult Index()
        {

            ViewData["Name"] = db.Staff.ToList();
            ViewData["JobId"] = db.Job.ToList();

            Models.Staff staff = (Models.Staff)Session["U_Name"];
            ViewData["StaffName"] = staff.Name;
            Models.Job job = (Models.Job)Session["Name"];
            ViewData["JobName"] = job.Name;

            return View();
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        #region 加载
        public ActionResult SelectLeaveOffice()
        {
            //session实体对象取值
            Models.Staff st = (Models.Staff)Session["U_Name"];
            Models.Job jo = (Models.Job)Session["Name"];
            var list = (from s in db.Staff
                        join job in db.Job on s.JobId equals job.Id
                        join lo in db.LeaveOffice on s.Name equals lo.StaffName
                        where ((s.No == st.No) && (job.Name == jo.Name))
                        select
                        new
                        {
                            LeaveID = lo.Id,
                            No = lo.No,
                            StaffName = lo.StaffName,
                            CreateTime = lo.CreateTime,
                            JobName = jo.Name,
                            LeaveType = lo.LeaveType,
                            IsDel = lo.IsDel,
                            Reason = lo.Reason,
                            ApplyPersonId = lo.ApplyPersonId,
                            ExplanationHandover = lo.ExplanationHandover,
                            HandoverSatffId = db.Staff.Where(s => s.Id == lo.HandoverSatffId).Select(s => s.Name)
                        }).ToList();
            if (list.Count > 0) { }
            return Json(new { code = 0, msg = "", data = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region //添加申请表单=>添加整个流程
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StaffName"></param>
        /// <param name="JId"></param>
        /// <param name="LeaveType"></param>
        /// <param name="ApplyDate"></param>
        /// <param name="Reason"></param>
        /// <param name="ExplanationHandover"></param>
        /// <param name="HandoverSatffId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LeaveOfficeAdd(string StaffName, string JName, string LeaveType, DateTime ApplyDate, string Reason, string ExplanationHandover, Guid HandoverSatffId)
        {
            //静态方法调用生成随机字符串
            //string rndstr = Oss.Unity.CharacterUtility.GenerateRandomString().ToString();
            var time = DateTime.Now.ToString("yyyyMMdd");
            Models.Staff st = (Models.Staff)Session["U_Name"];
            var JoID = db.Job.SingleOrDefault(js=>js.Id==st.JobId&&js.Name== JName);
            try
            {
                #region 申请单添加
                Models.LeaveOffice lo = new Models.LeaveOffice
                {
                    No = $"RZSQ{time}000001",
                    Id = Guid.NewGuid(),
                    StaffName = StaffName,
                    JobId = JoID.Id,
                    LeaveType = LeaveType,
                    ApplyDate = ApplyDate,
                    Reason = Reason,
                    ExplanationHandover = ExplanationHandover,
                    HandoverSatffId = HandoverSatffId,
                    ApplyPersonId =db.Staff.Where(ss=>ss.No==st.No).Select(sel=>sel.Id).SingleOrDefault() ,
                    CreateTime = DateTime.Now.ToLocalTime(),
                    UpdateTime = DateTime.Now.ToLocalTime(),
                    IsDel = true
                };
                db.LeaveOffice.Add(lo);
                #endregion

                Models.OrganizationStructure structure = (Models.OrganizationStructure)Session["Organization"];
                Models.Job job = (Models.Job)Session["Name"];
                /*武汉大区流程,荆州大区流程
                **判断所在机构是否是武汉大区流程
                * true就走武汉大区流程，false就走荆州大区流程
                */
                if (structure.Name.Contains("新洲"))
                {
                    #region 全武汉大区流程添加，通过顺序循环添加整个流程
                    int ran = 0;
                    for (ran = 0; ran < 5; ran++)
                    {
                        //每次发起或者通过都要添加一个流程信息记录
                        Models.ProcessStepRecord stepRecord = new Models.ProcessStepRecord
                        {
                            Id = Guid.NewGuid(),
                            Type = "LeaveOffice",
                            RecordRemarks = "新洲区发起",
                            StepOrder = ran,
                            CreateTime = DateTime.Now.ToLocalTime(),
                            UpdateTime = DateTime.Now.ToLocalTime(),
                            WhetherToExecute = false,
                            No = lo.No,
                            RefOrderId = lo.Id,
                            //默认未通过
                            Result = false,
                            ExecuteMethod = "离职",
                            Discrible = "未处理"
                        };
                        //通过顺序，连续添加流程
                        switch (stepRecord.StepOrder)
                        {
                            case 0:
                                stepRecord.WaitForExecutionStaffId = new Guid("8ED28AF7-9029-45F9-AB61-F01177A82915");
                                break;
                            case 1:
                                stepRecord.WaitForExecutionStaffId = new Guid("41C63B02-CB0A-4ECF-AE3E-04C2EE7F4C38");
                                break;
                            case 2:
                                stepRecord.WaitForExecutionStaffId = new Guid("3A4A9F17-0B21-4FE0-A302-701BB348B1FC");
                                break;
                            case 3:
                                stepRecord.WaitForExecutionStaffId = new Guid("D1A27A43-AA6B-4C5D-9EB0-173576772F89");
                                break;
                            case 4:
                                stepRecord.WaitForExecutionStaffId = new Guid("1145145B-FA04-479D-9786-9E4A9DEDE8D6");
                                break;
                        }

                        db.ProcessStepRecord.Add(stepRecord);
                    }
                    #endregion
                }
                else
                {
                    #region 全荆州大区流程添加，通过顺序循环添加整个流程
                    int ran = 0;
                    for (ran = 0; ran < 5; ran++)
                    {
                        //每次发起或者通过都要添加一个流程信息记录
                        Models.ProcessStepRecord stepRecord = new Models.ProcessStepRecord
                        {
                            Id = Guid.NewGuid(),
                            Type = "LeaveOffice",
                            RecordRemarks = "江陵发起",
                            StepOrder = ran,
                            CreateTime = DateTime.Now.ToLocalTime(),
                            UpdateTime = DateTime.Now.ToLocalTime(),
                            WhetherToExecute = false,
                            No = lo.No,
                            RefOrderId = lo.Id,
                            //默认未通过
                            Result = false,
                            ExecuteMethod = "离职",
                            Discrible = "[" + structure.Name + "]" + job.Name + "已发起"
                        };
                        //通过顺序，连续添加流程
                        switch (stepRecord.StepOrder)
                        {
                            case 0:
                                stepRecord.WaitForExecutionStaffId = new Guid("46D97D3B-2B01-4CEB-A817-D7737B1EC4E4");
                                break;
                            case 1:
                                stepRecord.WaitForExecutionStaffId = new Guid("761AA2DB-7549-48F5-A1C1-46FB21FC936F");
                                break;
                            case 2:
                                stepRecord.WaitForExecutionStaffId = new Guid("3A0FED20-128D-49A7-AC44-7718B7489FD8");
                                break;
                            case 3:
                                stepRecord.WaitForExecutionStaffId = new Guid("BC6C6077-283F-413B-878F-2D64C0145C81");
                                break;
                            case 4:
                                stepRecord.WaitForExecutionStaffId = new Guid("5760B5BA-7E89-4BD2-BC57-D8AF22260FEB");
                                break;
                        }

                        db.ProcessStepRecord.Add(stepRecord);
                    }
                    #endregion
                }
                int i = db.SaveChanges();
                return Json(i > 0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region //删除当前申请单
        [HttpPost]
        public ActionResult DelLeaveOffice(Guid LeaveID)
        {
            //已提交的不能删除
            var apply = (from lo in db.LeaveOffice
                         where lo.Id == LeaveID
                         select lo.IsDel).ToList();
            bool? a = apply[0];
            if (a == false)
            {
                return Json(false);
            }
            else
            {
                var del = db.LeaveOffice.SingleOrDefault(l => l.Id == LeaveID);
                var delPsr = db.ProcessStepRecord.SingleOrDefault(psr=>psr.RefOrderId==LeaveID);
                if (del.Id == null)
                {
                    return Json(false);
                }
                else
                {
                    db.LeaveOffice.Remove(del);
                    db.ProcessStepRecord.Remove(delPsr);
                    int i = db.SaveChanges();
                    return Json(i > 0, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion

        #region  //流程视图
        public ActionResult SelectProcessStepRecord(Guid LeaveID,string No, string StaffName)
        {
            var list = (from psr in db.ProcessStepRecord
                        join s in db.Staff on psr.WaitForExecutionStaffId equals s.Id
                        join lo in db.LeaveOffice on psr.RefOrderId equals lo.Id
                        where lo.No == No && lo.StaffName == StaffName && lo.Id== LeaveID
                        orderby psr.StepOrder descending
                        select new
                        {
                            OrgName = db.OrganizationStructure.Where(os => os.Id == s.OrgID).Select(ost => ost.Name).FirstOrDefault(),
                            TakeName = s.Name,
                            Job = db.Job.Where(job => job.Id == s.JobId).Select(st => st.Name).FirstOrDefault(),
                            Result = psr.Result
                        }).ToList();
            return Json(new { code = 0, msg = "", data = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region //发起申请=>修改IsDel
        [HttpPost]
        public ActionResult LeaveOfficeUp(Guid LeaveID, Guid ApplyPersonId, string No)
        {
            var update = db.LeaveOffice.SingleOrDefault(le => le.Id == LeaveID);
            /*
             * 发起修改流程视图里的发起流程         
             */
            var upPsr = db.ProcessStepRecord.Where(psr => psr.No==No && psr.StepOrder==0&&psr.RefOrderId== LeaveID).SingleOrDefault();
            if (update.Id == null)
            {

                return Json(false);
            }
            else
            {
                update.IsDel = false;
                upPsr.Result = true;
                Models.OrganizationStructure organization = (Models.OrganizationStructure)Session["Organization"];
                Models.Job job = (Models.Job)Session["Name"];
                upPsr.Discrible = $"[{organization.Name}][{job.Name}]已发起";
                int i = db.SaveChanges();
                return Json(i > 0, JsonRequestBehavior.AllowGet);

            }
        }

        #endregion


        /// <summary>
        ///  //修改
        /// </summary>
        /// <param name="No"></param>
        /// <param name="Name"></param>
        /// <param name="JName"></param>
        /// <param name="LeaveType"></param>
        /// <param name="date"></param>
        /// <param name="Reason"></param>
        /// <param name="ExplanationHandover"></param>
        /// <param name="HandoverSatffId"></param>
        /// <returns></returns>
        #region 修改信息
        [HttpPost]
        public ActionResult LeaveUpdata(Guid LeaveID, string No, string Name, string JName, string LeaveType, DateTime date, string Reason, string ExplanationHandover, string HandoverSatffId)
        {
            //判断是否已发起，true:不能修改,false:执行下面        
            var apply = (from lo in db.LeaveOffice
                         where lo.Id == LeaveID
                         select lo.IsDel).ToList();
            bool? a = apply[0];
            if (a == false)
            {

                return Json(false);
            }
            else
            {
                var list = db.LeaveOffice.SingleOrDefault(le => le.Id == LeaveID);
                if (list.Id == null)
                {
                    return Json(false);

                }
                else
                {
                    list.No = No;
                    list.StaffName = Name;
                    var jname = (from j in db.Job where j.Name == JName select j.Id).ToList();
                    list.JobId = jname[0];
                    list.LeaveType = LeaveType;
                    list.ApplyDate = date;
                    list.Reason = Reason;
                    list.ExplanationHandover = ExplanationHandover;
                    var Sname = (from s in db.Staff where s.Name == HandoverSatffId select s.Id).ToList();
                    list.HandoverSatffId = Sname[0];
                    int i = db.SaveChanges();
                    return Json(new { code = 0, msg = "", data = list }, JsonRequestBehavior.AllowGet);
                }
            }


        }
        #endregion
    }


}
