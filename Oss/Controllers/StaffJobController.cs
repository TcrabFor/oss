using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oss.Controllers
{
    public class StaffJobController : Controller
    {
        Models.OSMSEntities1 db = new Models.OSMSEntities1();
        // GET: StaffJob
        public ActionResult Index()
        {
            ViewData["JobId"] = db.Job.ToList();
            ViewData["OrgID"] = db.OrganizationStructure.ToList();
            return View();
        }
        /// <summary>
        /// 查询所有员工基础信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectStaffJob()
        {
            //定义分页参数
            int page = Convert.ToInt32(Request["page"]);
            int limit = Convert.ToInt32(Request["limit"]);
            #region
            var list = (
                        from s in db.Staff
                        join j in db.Job on s.JobId equals j.Id
                        join os in db.OrganizationStructure on s.OrgID equals os.Id
                        select new
                        {
                            ID = s.Id,
                            No = s.No,
                            Name = s.Name,
                            Sex = s.Sex,
                            BirthDay = s.BirthDay,
                            NativePlace = s.NativePlace,
                            Address = s.Address,
                            Tel = s.Tel,
                            Email = s.Email,
                            J_Name = j.Name,
                            OrgName = os.Name,//所属机构
                            Status = s.Status

                        }).ToList();
            #endregion
            return Json(new
            {
                msg = "",
                code = 0,
                count = list.Count(),
                data = list.OrderBy(s => s.ID).Skip((page - 1) * limit).Take(limit).ToList()
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //添加
        public ActionResult AddStaffJob(string No, string Name, string Sex, string NativePlace, string Address, string Email, string Tel, string JobId, string OrgID, string Status)
        {
            Models.Staff s = new Models.Staff();
            s.Id = Guid.NewGuid();
            s.No = No;
            s.Name = Name;
            bool she;
            if (Sex == "0")
            {
                she = true;
            }
            else
            {
                she = false;
            }
            s.Sex = she;
            s.BirthDay = Convert.ToDateTime("1999-01-01");
            s.NativePlace = NativePlace;
            s.Address = Address;
            s.Email = Email;
            s.Tel = Tel;
            s.JobId = Guid.Parse(JobId);
            s.OrgID = Guid.Parse(OrgID);
            s.Status = Status;
            s.CreateTime = DateTime.Now.ToLocalTime();
            s.UpdateTime = DateTime.Now.ToLocalTime();
            s.Password = "123456";
            s.IsHSEGroup = true;
            db.Staff.Add(s);
            int i = db.SaveChanges();
            return Json(i > 0, JsonRequestBehavior.AllowGet);

        }

    }
}