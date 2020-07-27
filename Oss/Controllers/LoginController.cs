using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Oss.Controllers
{
    public class LoginController : Controller
    {
        Models.OSMSEntities1 db = new Models.OSMSEntities1();
        // GET: Login
        public ActionResult Index()
        {


            //ViewData["No"] = db.Staff.Distinct().ToList();
            return View();
        }
        /// <summary>
        /// 登录后拿到他们对应的职位,角色,机构
        /// </summary>
        /// <param name="u_Name"></param>
        /// <param name="u_Pwd"></param>
        /// <returns></returns>
        public ActionResult Login(string u_Name, string u_Pwd)
        {
            /*密码格式转换*/
            u_Pwd = UI.EncryptHelper.Encode(u_Pwd);

            var list = db.Staff.Where(s => s.No == u_Name && s.Password == u_Pwd).ToList();
            /*数据库不完善，账号为8898999,797979799,7987697979的用户不能登录*/
            if (u_Name == "")
            {
                return Json(false);
            }
            else
            {
                if (list.Count > 0)
                {
                    //姓名
                    Models.Staff s = list.FirstOrDefault();//将泛型集合转成实体对象              
                                                           //var ss1 = db.Staff.Join(db.Job, st => st.JobId, job => job.Id, (st, job) => st).ToList();             


                    var JName = (from job in db.Job
                                 join st in db.Staff on job.Id equals st.JobId
                                 where job.Id == s.JobId
                                 select job).Distinct().ToList();

                    //职位
                    Models.Job Name = JName.FirstOrDefault();


                    Session["Name"] = Name;
                    Session["U_Name"] = s;
                    /*通过机构表和员工表连接，获取到所属机构*/
                    #region lambad 获取机构
                    //db.OrganizationStructure.Join
                    //    (db.Staff, or => or.Id, stf => stf.OrgID, (or, stf) => new
                    //    {
                    //        OrgName = or.Name,
                    //        //登录名
                    //        No = stf.No

                    //    }).Where(stf => stf.No == u_Name).Select(or => or).ToList()
                    #endregion

                    /*拿到Organizations单个职位*/
                    var Organizations = (from org in db.OrganizationStructure
                                         join stf in db.Staff on org.Id equals stf.OrgID
                                         where stf.No == u_Name
                                         select org
                                        ).Distinct().ToList();

                    //session，存储模型类方便于前台取值
                    Models.OrganizationStructure Organization = Organizations.FirstOrDefault();

                    Session["Organization"] = Organization;

                    #region /*lambad表达式三个表拿到角色*/
                    //var Role = db.Staff.Join(db.StaffRole.Join(db.Role, stf => stf.RoleId, ro => ro.Id, (stf, ro) => new
                    //{
                    //    Sid = stf.StaffId,
                    //    RoName = ro.Name
                    //}), sst => sst.Id, srole => srole.Sid, (sst, srole) => new
                    //{
                    //    sstNo = sst.No,
                    //    srole,
                    //    RName = srole.RoName
                    //}).Where(sst => sst.sstNo == u_Name).Select(sst => sst.RName).ToList();
                    #endregion
                    var Role = (from stRo in db.StaffRole
                                join Stf in db.Staff on stRo.StaffId equals Stf.Id
                                join Ro in db.Role on stRo.RoleId equals Ro.Id
                                where Stf.No == u_Name
                                select Ro).ToList();

                    /*如果包含元素为空
                    * 就把没有角色的全部，变为超级用户
                    */
                    if (Role.Count == 0)
                    {
                   
                        Role = (from stRo in db.StaffRole
                                join Stf in db.Staff on stRo.StaffId equals Stf.Id
                                join Ro in db.Role on stRo.RoleId equals Ro.Id
                                where Stf.No == "7900797"
                                select Ro).ToList();
                        /*拿到角色*/
                        Models.Role RoleName = Role.FirstOrDefault();

                        Session["RoleName"] = RoleName;
                    }
                    else
                    {
                        /*拿到角色*/
                        Models.Role RoleName = Role.FirstOrDefault();

                        Session["RoleName"] = RoleName;
                    }

                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
        }



        public ActionResult Main()
        {
            return View();
        }
        public ActionResult Others()
        {
            return View();
        }
        //查询
        public ActionResult OthersAll()
        {

            //真分页
            int page = Convert.ToInt32(Request["page"]);
            int limit = Convert.ToInt32(Request["limit"]);
            page = Convert.ToInt32(Request["page"]);
            var list = (from s in db.Staff
                        select new
                        {
                            U_ID = s.OrgID,//重新声明字段
                            U_Name = s.Name,
                            U_Sex = s.Sex,
                            U_Adress = s.Address,
                            U_Tel = s.Tel,

                        }).ToList();


            return Json(new { code = 0, msg = "", count = list.Count(), data = list.OrderBy(s => s.U_ID).Skip((page - 1) * limit).Take(limit).ToList() }, JsonRequestBehavior.AllowGet);//将集合转换成json格式
        }

        [HttpPost]
        //添加
        public ActionResult AddAll(string U_Name, string U_Pwd, bool U_Sex, Guid U_ID, string U_Tel, string U_Address)
        {
            Models.Staff s = new Models.Staff();
            s.No = "";
            s.BirthDay = new DateTime();
            s.NativePlace = "湖北武汉";
            s.Email = "2333";
            s.Status = "";
            s.CreateTime = new DateTime();
            s.UpdateTime = new DateTime();

            var guid = new Guid();//string转Guid
            s.JobId = guid;
            s.OrgID = guid;

            s.Name = U_Name;
            s.Address = U_Address;
            s.Password = U_Pwd;
            s.Id = U_ID;
            s.Sex = U_Sex;
            s.Tel = U_Tel;
            s.IsHSEGroup = null;

            db.Staff.Add(s);
            int i = db.SaveChanges();
            return Json(i > 0, JsonRequestBehavior.AllowGet);
        }
    }
}