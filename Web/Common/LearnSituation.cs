using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WeChat;
using Me.Common.Data;
using System.Data;

namespace Web.Common {
    /// <summary>
    /// 类型数据库类
    /// </summary>
  
    public class LearnSituation {
        /// <summary>
        /// 私有的构造函数
        /// </summary>
        private LearnSituation() {
        }
        public static LearnSituation ls;//类型为ProductLogic的全局字段
        /// <summary>
        /// 用于实例化全局静态字段的，并返回全局静态字段方法
        /// </summary>
        /// <returns></returns>
        public static LearnSituation Instance() {
            if (ls == null) {
                ls = new LearnSituation();
            }
            return ls;
        }

        /// <summary>
        /// 获取幼儿学习内容和时间的方法
        /// </summary>
        /// <returns>List<Category>集合</returns>
        public List<LearnSit> GetAllLearnSit(string deviceID, int yeshu)
        {
            string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
            yeshu = 15 * yeshu;
            string sql = string.Format("select top({0}) * from [dbo].LearnSituation where convert(varchar(11),createtime,120) ='{1}' and DeviceId='{2}'and id not in (select top({3}) id from [dbo].LearnSituation where convert(varchar(11),createtime,120) ='{1}' and DeviceId='{2}' order by createtime desc)  order by createtime desc", 15, date, deviceID, yeshu);//查询当天时间
            return SqlHelper.ExecuteReader(sql).ToList<LearnSit>();
        }
        /// <summary>
        /// 查询昨天的学习情况
        /// </summary>
        /// <returns></returns>
        public List<LearnSit> GetYesdLearn(string deviceID,int yeshu) {
            string Yesdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            yeshu = 15 * yeshu;
            string sql = string.Format("select top({0}) * from [dbo].LearnSituation where convert(varchar(11),createtime,120) ='{1}' and DeviceId='{2}'and id not in (select top({3}) id from [dbo].LearnSituation where convert(varchar(11),createtime,120) ='{1}' and DeviceId='{2}' order by createtime desc)  order by createtime desc", 15, Yesdate, deviceID, yeshu);
            //string sql = string.Format("select * from [dbo].[LearnSituation] where deviceID='{0}' and convert(varchar(11),createtime,120)='{1}' order by createtime desc", deviceID, Yesdate);//查询昨天时间
          return  SqlHelper.ExecuteReader(sql).ToList<LearnSit>();
        }

        /// <summary>
        /// 查询一周的学习情况
        /// </summary>
        /// <returns></returns>
        public List<LearnSit> GetWeekLearn(string deviceID,int yeshu) {
            yeshu = 15 * yeshu;
            string WeekDate = DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek)))).ToShortDateString();//获取一周前的日期
            string sql = string.Format("select top({0}) * from [dbo].LearnSituation where createtime >='{1}' and DeviceId='{2}'and id not in (select top({3}) id from [dbo].LearnSituation where createtime >='{1}' and DeviceId='{2}' order by createtime desc)  order by createtime desc", 15, WeekDate, deviceID, yeshu);
            //string sql = string.Format("select * from [dbo].[LearnSituation] where deviceID='{0}' and createtime >='{1}' order by createtime desc", deviceID, WeekDate);//查询一周的时间
            return SqlHelper.ExecuteReader(sql).ToList<LearnSit>();

        }

        /// <summary>
        /// 查询一个月的学习情况
        /// </summary>
        /// <returns></returns>
        public List<LearnSit> GetMothLearn(string deviceID, int yeshu) {
            yeshu = 15 * yeshu;
            DateTime now = DateTime.Now;
            string d1ate = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd");
            string sql = string.Format("select top({0}) * from [dbo].LearnSituation where createtime >='{1}' and DeviceId='{2}'and id not in (select top({3}) id from [dbo].LearnSituation where createtime >='{1}' and DeviceId='{2}' order by createtime desc)  order by createtime desc", 15, d1ate, deviceID, yeshu);
            //string sql = string.Format("select * from [dbo].[LearnSituation] where deviceID='{0}' and createtime > ='{1}' order by createtime desc", deviceID, d1ate);//查询一个月的时间
            return SqlHelper.ExecuteReader(sql).ToList<LearnSit>();

        }

        /// <summary>
        /// 查询更久之前的学习情况
        /// </summary>
        /// <returns></returns>
        public List<LearnSit> GetForTimeLearn(string deviceID, int yeshu) {
            yeshu = 15 * yeshu;
            DateTime now = DateTime.Now;
            string lastd1ate = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd");
            string sql = string.Format("select top({0}) * from [dbo].LearnSituation where createtime <='{1}' and DeviceId='{2}'and id not in (select top({3}) id from [dbo].LearnSituation where createtime <='{1}' and DeviceId='{2}' order by createtime desc)  order by createtime desc", 15, lastd1ate, deviceID, yeshu);
            // string sql = string.Format("select * from [dbo].[LearnSituation] where deviceID='{0}' and createtime <='{1}' order by createtime desc", deviceID, lastd1ate);//查询更久之前时间
            return SqlHelper.ExecuteReader(sql).ToList<LearnSit>();

        }
        public int setcont1(string deviceID) {  //获取今天的总条数
            string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
            string sql = string.Format("select count(*) from [dbo].[LearnSituation] where deviceID='{0}' and convert(varchar(11),createtime,120)='{1}' ", deviceID, date);
           return SqlHelper.ExecuteScalar(sql).ToInt();
        }
        public int setcont2(string deviceID) { //获取昨天的总条数
            string Yesdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string sql = string.Format("select count(*) from [dbo].[LearnSituation] where deviceID='{0}' and convert(varchar(11),createtime,120)='{1}' ", deviceID, Yesdate);
            return SqlHelper.ExecuteScalar(sql).ToInt();
        }
        public int setcont3(string deviceID) { //获取一周的总条数
            string WeekDate = DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek)))).ToShortDateString();//获取一周前的日期
            string sql = string.Format("select count(*) from [dbo].[LearnSituation] where deviceID='{0}' and createtime >='{1}' ", deviceID, WeekDate);
            return SqlHelper.ExecuteScalar(sql).ToInt();
        }
        public int setcont4(string deviceID) { //获取一个月的总条数
            DateTime now = DateTime.Now;
            string d1ate = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd");
            string sql = string.Format("select count(*) from [dbo].[LearnSituation] where deviceID='{0}' and createtime >='{1}' ", deviceID, d1ate);
            return SqlHelper.ExecuteScalar(sql).ToInt();
        }
        public int setcont5(string deviceID) { //获取更久的总条数
            DateTime now = DateTime.Now;
            string lastd1ate = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd");
            string sql = string.Format("select count(*) from [dbo].[LearnSituation] where deviceID='{0}' and createtime <='{1}' ", deviceID, lastd1ate);
            return SqlHelper.ExecuteScalar(sql).ToInt();
        }
    }
}