using System;
using System.Xml;
using System.Configuration;
using System.Collections.Specialized;

namespace Web.Admin.Data
{
    /// <summary>
    /// ʵ�ֶ�web.config�Զ������ýڵĶ�ȡ
    /// </summary>
    public class AppConfig : IConfigurationSectionHandler
    {

        public Object Create(Object parent, object configContext, XmlNode section)
        {

            NameValueCollection settings;

            NameValueSectionHandler baseHandler = new NameValueSectionHandler();

            settings = (NameValueCollection)baseHandler.Create(parent, configContext, section);

            return settings;
        }
        public static NameValueCollection Settings
        {
            get
            {
                NameValueCollection collection1 = (NameValueCollection)ConfigurationManager.GetSection("appSettings");
                return collection1;
            }
        }

    }

}
