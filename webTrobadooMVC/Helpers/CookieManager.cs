using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace trobadoo.com.web.Helpers
{
    public class CookieManager
    {
        public static void WriteLanguageCookie(string cookieValue, HttpResponseBase response)
        {
            WriteCookie("languageCode", cookieValue, 24 * 365, response);
        }

        public static string ReadLanguageCookie(HttpRequestBase request)
        {
            return ReadCookie("languageCode", request);
        }

        private static void WriteCookie(string cookieName, string cookieValue, int hoursOffset, HttpResponseBase response)
        {
            HttpCookie myCookie = new HttpCookie(cookieName);
            DateTime now = DateTime.Now;

            // Set the cookie value.
            myCookie.Value = cookieValue;
            // Set the cookie expiration date.
            myCookie.Expires = now.AddHours(hoursOffset);

            // Add the cookie.
            response.Cookies.Add(myCookie);
        }

        private static string ReadCookie(string cookieName, HttpRequestBase request)
        {
            HttpCookie myCookie = new HttpCookie(cookieName);
            myCookie = request.Cookies[cookieName];

            // Read the cookie information and display it.
            if (myCookie != null)
                return myCookie.Value;

            return string.Empty;
        }
    }
}