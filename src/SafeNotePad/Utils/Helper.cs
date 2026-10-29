using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace SafeNotePad.Utils
{
    internal class Helper
    {
        public static bool IsTests { get; set; }

        public static byte[] TestKey
        {
            get
            {
                return Convert.FromBase64String(TestKeyStr);
            }
        }

        public static string TestKeyStr
        {
            get
            {
                return "XVuuQG/DV3I8ZoZUDLgzhQmPmYsQT7MWZlIEFQrCw+w=";
            }
        }

        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }
            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }
            var property = body.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }
            return property.Name;
        }

        public static string GetResourcePath(string relativePath)
        {
            return string.Format("pack://application:,,,/{0};component/{1}", Settings.ApplicationAssemblyName,
                relativePath);
        }

        public static BitmapImage GetBitmapImage(string relativePath)
        {
            return new BitmapImage(new Uri(GetResourcePath(relativePath)));
        }
    }
}