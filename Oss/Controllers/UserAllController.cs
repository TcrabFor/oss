using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oss.Controllers
{
    public class UserAllController : Controller
    {
        Models.OSMSEntities1 db = new Models.OSMSEntities1();
        // GET: UserAll
        public ActionResult Index()
        {
            ViewData["ID"] = db.Staff.ToList();
            return View();
        }
        //public ActionResult Indexs(string name = "", string sex = "全部", int D_ID = 0)
        //{
        //    真分页
        //    int page = Convert.ToInt32(Request["page"]);
        //    int limit = Convert.ToInt32(Request["limit"]);
        //    page = Convert.ToInt32(Request["page"]);
        //    var list = (from u in db.Users
        //                join d in db.Departments on u.D_ID equals d.D_ID
        //                where ((name == "") || (u.U_Name.Contains(name))) && ((sex == "全部") || (u.U_Sex == sex)) && ((D_ID == 0) || (d.D_ID == D_ID))  //where ((输入的值==默认值)|| （表格查询字段==输入值字段）)
        //                select new
        //                {
        //                    U_ID = u.U_ID,//重新声明字段
        //                    U_Name = u.U_Name,
        //                    U_Sex = u.U_Sex,
        //                    U_Age = u.U_Age,
        //                    U_Tel = u.U_Tel,
        //                    D_Name = d.D_Name
        //                }).ToList();

        //    return Json(new { code = 0, msg = "", count = list.Count(), data = list.OrderBy(u => u.U_ID).Skip((page - 1) * limit).Take(limit).ToList() }, JsonRequestBehavior.AllowGet);//将集合转换成json格式
        //}
        //[HttpPost]
        //public ActionResult Add(string U_Name, string U_Pwd, string U_Sex, int U_ID, string U_Tel, int U_Age)
        //{
        //    Models.User u = new Models.User();
        //    u.U_Name = U_Name;
        //    u.U_Pwd = U_Pwd;
        //    u.U_ID = U_ID;
        //    u.U_Sex = U_Sex;
        //    u.U_Tel = U_Tel;
        //    u.U_Age = U_Age;
        //    db.Users.Add(u);
        //    int i = db.SaveChanges();
        //    return Json(i > 0, JsonRequestBehavior.AllowGet);
        //}
    }
}