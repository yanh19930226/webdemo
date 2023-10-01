using Newtonsoft.Json;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace webdemo.Infrastructure.Utils
{
    public static class CarterUtil
    {

        /// <summary>
        /// 显示特性上的说明文字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ExtName(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            var va = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return va == null ? value.ToString() : va.Description;
        }

        /// <summary>
        /// 数据转换为DateTime类型
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static DateTime ObjectToDate(this object thisValue)
        {
            DateTime result = DateTime.MinValue;
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out result))
                result = Convert.ToDateTime(thisValue);
            return result;
        }

        public static string ToJson<T>(this T obj, JsonSerializerSettings settings = null)
        {
            if (settings == null)
            {
                return JsonConvert.SerializeObject(obj);
            }
            else
            {
                return JsonConvert.SerializeObject(obj, settings);
            }
        }

        /// <summary>
        /// 判断非null，非0长度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty<T>(this T value)
          where T : class
        {
            //IsNullOrEmpty取反
            return !value.IsNullOrEmpty();
        }
        /// <summary>
        /// 判断null，null或0长度都返回true
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value">要判断的对象</param>
        /// <returns>判断结果,null或0长度返回true,否则返回false</returns>
        public static bool IsNullOrEmpty<T>(this T value)
          where T : class
        {
            #region 1.对象级别

            //引用为null
            bool isObjectNull = value == null;
            if (isObjectNull == true) return true;

            //判断是否为集合
            IEnumerator tempEnumerator = (value as IEnumerable)?.GetEnumerator();
            if (tempEnumerator == null) return false;//这里出去代表是对象 且 引用不为null.所以为false

            #endregion 1.对象级别

            #region 2.集合级别

            //到这里就代表是集合且引用不为空，判断长度
            //MoveNext方法返回tue代表集合中至少有一个数据,返回false就代表0长度
            bool isZeroLenth = tempEnumerator.MoveNext() == false;
            if (isZeroLenth == true) return true;

            return isZeroLenth;

            #endregion 2.集合级别
        }
    }
}
