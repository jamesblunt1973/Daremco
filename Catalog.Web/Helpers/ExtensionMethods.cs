using Catalog.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace Catalog.Web.Helpers
{
    public static class ExtensionMethods
    {
        public static string GetQueryString(this FilterData data, Dictionary<string, object> newValues = null)
        {
            var properties = new List<string>();
            foreach (var p in data.GetType().GetProperties())
            {
                object value;
                if (newValues != null && newValues.ContainsKey(p.Name))
                    value = newValues[p.Name];
                else
                    value = p.GetValue(data, null);

                if (value != null)
                    properties.Add(p.Name + "=" + HttpUtility.UrlEncode(value.ToString()));
            }

            var qs = string.Join("&", properties.ToArray());
            if (qs != "")
                qs = "?" + qs;
            return qs;
        }
        public static Dictionary<string, object> GetDictionary(this FilterData data)
        {
            var result = new Dictionary<string, object>();
            foreach (var p in data.GetType().GetProperties())
            {
                var attr = p.GetCustomAttribute<QueryParamAttribute>();
                var value = p.GetValue(data, null);
                if (value != null && attr != null)
                    result.Add(p.Name, value);
            }
            return result;
        }
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
        public static string GetExceptionMessage(this Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return ex.Message;
        }
        public static string ToW3CDate(this DateTime dt)
        {
            return $"{dt.ToUniversalTime().ToString("s")}Z";
        }
    }
}
