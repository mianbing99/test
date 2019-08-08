using Me.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Web.API
{
    /// <summary>
    /// StudentService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class StudentService : System.Web.Services.WebService
    {
        [WebMethod]
        public void getinfo()
        {
            string sql = null;
            sql = "select deviceId from StudentDeviceBind where deviceId like 'd%'";
            string deviceId = SqlHelper.ExecuteScalar(sql).ToString();
            sql = "select token from StudentDeviceBind where deviceId ='"+deviceId+"'";
            string token = SqlHelper.ExecuteScalar(sql).ToString();
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}
