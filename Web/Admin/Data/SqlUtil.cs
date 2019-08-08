using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Reflection;

namespace IFrameWork.Data
{
    public class SqlUtil
    {
        /// <summary>
        /// ����Ƿ���SqlΣ���ַ�
        /// </summary>
        /// <param name="str">Ҫ�ж��ַ���</param>
        /// <returns>�жϽ��</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// �����ַ�����Uncode�ַ��ĺ���
        /// ��ֱ��ƴ��SQL����е�=�����ʹ�á�
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        /// <remarks>
        /// Ŀ���Ƿ�sqlע��Ĺ��������磺
        /// string sql = "select * from Customers where CustomerID = '" + temp + "'";
        /// �������temp��ֵΪ 5' or 1=1 --  
        /// ��ô��ƴ�����������Ϊ select * from Customers where CustomerID = '5' or 1=1 -- '
        /// ����滻�˵����žͱ��
        /// select * from Customers where CustomerID = '5'' or 1=1 -- '
        /// �Ͳ�����������
        /// 
        /// ע�⣺
        ///    ����ʲô���͵Ĳ��������������߶�������ϵ����ţ����磺
        /// string sql = "select * from Customers where CustomerID = " + temp;
        /// temp����û�е����ţ� �����temp��ֵΪ 5 or 1=1 --  
        /// ��ô��ƴ�����������Ϊ select * from Customers where CustomerID = 5 or 1=1 --
        /// û�е����ţ���������������
        /// </remarks>
        public static string SCCEqual(string strValue)
        {
            strValue = strValue.Replace("'", "''");
            return strValue;
        }

        /// <summary>
        /// �����ַ�����Uncode�ַ��ĺ���
        /// ��ֱ��ƴ��SQL����е�Like�����ʹ�á�
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string SCCLike(string strValue)
        {
            strValue = strValue.Replace("'", "''");
            strValue = strValue.Replace("[", "[[]");
            strValue = strValue.Replace("_", "[_]");
            strValue = strValue.Replace("%", "[%]");
            
            return strValue;
        }

        public static object Nullable(object o)
        {
            object obj = (o == null ? System.DBNull.Value : o);
            return obj;
        }

        /// <summary>
        /// ��DbDataReader��һ��ת���һ������
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static Object ReaderToObject(DbDataReader reader, Type entityType)
        {
            Object entity = Activator.CreateInstance(entityType, true);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                PropertyInfo propertyInfo = entityType.GetProperty(reader.GetName(i));
                if (propertyInfo != null)
                {
                    if (reader.GetValue(i) != DBNull.Value)
                    {
                        if (propertyInfo.PropertyType.IsEnum)
                        {
                            propertyInfo.SetValue(entity, Enum.ToObject(propertyInfo.PropertyType, reader.GetValue(i)), null);
                        }
                        else
                        {
                            propertyInfo.SetValue(entity, reader.GetValue(i), null);
                        }
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// ��DataTable��һ��ת���һ������
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static Object DataRowToObject(DataRow row, Type entityType)
        {
            Object entity = Activator.CreateInstance(entityType, true);
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = entityType.GetProperty(row.Table.Columns[i].ColumnName);
                if (propertyInfo != null)
                {
                    if (row[i] != DBNull.Value)
                    {
                        if (propertyInfo.PropertyType.IsEnum)
                        {
                            propertyInfo.SetValue(entity, Enum.ToObject(propertyInfo.PropertyType, row[i]), null);
                        }
                        else
                        {
                            propertyInfo.SetValue(entity, row[i], null);
                        }
                    }
                }
            }
            return entity;
        }

        #region ת�����

        public static int ToInt(object strValue)
        {
            return ToInt(strValue, 0);
        }
        public static int ToInt(object strValue, int defaultValue)
        {
            if ((strValue == null) || (strValue.ToString() == string.Empty) || (strValue.ToString().Length > 10))
            {
                return defaultValue;
            }
            int intValue = defaultValue;

            bool IsInt = new Regex(@"^([-]|[0-9])[0-9]*$").IsMatch(strValue.ToString());
            if (IsInt)
            {
                intValue = Convert.ToInt32(strValue);
            }
            
            return intValue;
        }

        public static float ToFloat(object strValue)
        {
            return ToFloat(strValue, 0);
        }
        public static float ToFloat(object strValue, float defValue)
        {
            if ((strValue == null) || (strValue.ToString().Length > 10))
            {
                return defValue;
            }

            float intValue = defValue;
            
            bool IsFloat = new Regex(@"^([-]|[0-9])[0-9]*(\.\w*)?$").IsMatch(strValue.ToString());
            if (IsFloat)
            {
                intValue = Convert.ToSingle(strValue);
            }
         
            return intValue;
        }
        public static int TimeToInt(DateTime time)
        {
            TimeSpan timeSpan = (TimeSpan)(time - new DateTime(1970, 1, 1));
            return ((int)timeSpan.TotalSeconds);
        }
        #endregion
    }
}
