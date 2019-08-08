using Common;
using Me.Common.Cache;
using Me.Common.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using WeChat;

namespace Web.API
{
    /// <summary>
    /// WeChatInteractive 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class WeChatInteractive : System.Web.Services.WebService
    {

        VideoEntities ve = DBContextFactory.GetDbContext();
        [WebMethod]
        public void IndexInteractive() 
        {
            //获取微信请求参数
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];
            string echostr = HttpContext.Current.Request.QueryString["echostr"];
            string token = "InteractiveWeChat";
            //加密sha1对比signature字符串
            if(WeChatInteractiveHelper.CheckSignature(token,signature,timestamp,nonce))
            {
                //返回echostr参数内容
                Write(echostr);
            }
        }

        //公众号菜单栏
        [WebMethod]
        public void UpdateMenu() 
        {
            string str = "{\"button\":[{\"type\":\"view\",\"name\":\"名师课堂\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf9ade153117809bf&redirect_uri=http://ftv.icoxtech.com&response_type=code&scope=snsapi_base&state=1#wechat_redirect\"},"
                + "{\"name\":\"亲子互动\",\"sub_button\":[{\"type\":\"click\",\"name\":\"一键解锁\",\"key\":\"V1006_GOOD\"},{\"type\":\"click\",\"name\":\"一键锁屏\",\"key\":\"V1005_GOOD\"},{\"type\":\"click\",\"name\":\"远程定位\",\"key\":\"V1002_GOOD\"},{\"type\":\"view\",\"name\":\"习惯养成\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf9ade153117809bf&redirect_uri=http://v.icoxtech.com/AlarmClock.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect\"},{\"type\":\"view\",\"name\":\"使用轨迹\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf9ade153117809bf&redirect_uri=http://v.icoxtech.com/StudyStation.aspx&response_type=code&scope=snsapi_base&state=1#wechat_redirect\"}]},"
                + "{\"name\":\"远程管理\",\"sub_button\":[{\"type\":\"click\",\"name\":\"一键抓拍\",\"key\":\"V1003_GOOD\"},{\"type\":\"click\",\"name\":\"视频抓拍\",\"key\":\"V1001_GOOD\"}," +
                "{\"type\":\"view\",\"name\":\"设备管理\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf9ade153117809bf&redirect_uri=http://v.icoxtech.com/Child/ChildDeviceList.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect\"}," +
                "{\"type\":\"view\",\"name\":\"成员管理\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf9ade153117809bf&redirect_uri=http://v.icoxtech.com/Child/ChildUsers.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect\"}]}]}";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}" , GetToken());
            Write(Submit.HttpPost(url,str));
        }

        [WebMethod]
        public void GetQrCodeImg(string token,string deviceId)
        {
            //TempBind temp = new TempBind();
            //var ChildBind = (from c in ve.ChildBind orderby c.ID descending select new { sceneId = c.sceneId, deviceId = c.deviceId }).FirstOrDefault().sceneId;
            ////int sceneId = Convert.ToInt32(ChildBind.sceneId);
            //int scendIdOne = Convert.ToInt32(ve.ChildBind.OrderByDescending(cb=>cb.sceneId).FirstOrDefault().sceneId);

            //string sqlStr = string.Format("select top 1 sceneId from ChildBind order by ID desc");
            //int SceneId = Convert.ToInt32(SqlHelper.ExecuteScalar(sqlStr));

            int num = Convert.ToInt32(ve.ChildBind.OrderByDescending(cb => cb.sceneId).First().sceneId)+1;
            ChildBind child = new ChildBind();
            child.sceneId = num;
            child.token = token;
            child.deviceId = deviceId;
            child.BindDate = DateTime.Now;
            ve.ChildBind.Add(child);
            ve.SaveChanges();

            WriteImg(QrCodeManager.GenerateTemp(GetToken(), num));

            //WriteImg(QrCodeManager.GenerateTemp(GetToken(), 11));
        }

        private string GetToken() 
        {
            var key = "gh_a2e48791ecc1";
            var token = CacheCore.Get<string>(key);
            if(string.IsNullOrEmpty(token))
            {
                token = WeChatInteractiveHelper.GetToken("wxf9ade153117809bf", "c876b01424956a2f7f7d1bee9dbb0acd");
                CacheCore.Set<string>(key,token,60);
            }
            return token;
        }

        private void Write(string str) 
        {
            Context.Response.Clear();
            Context.Response.ContentEncoding = Encoding.UTF8;
            Context.Response.ContentType = "text/plain";
            Context.Response.Write(str);
            Context.Response.End();
        }

        private void WriteImg(Byte[] str)
        {
            Context.Response.Clear();
            Context.Response.ContentType = "image/jpg";
            Context.Response.BinaryWrite(str);
            Context.Response.End();
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}
