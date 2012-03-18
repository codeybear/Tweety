using System;
using System.Net;
using System.IO;

namespace Tweety.Core
{
    static class WebHelper
    {
        public const string HTTPGET = "GET";
        public const string HTTPPOST = "POST";

        public static byte[] GetBytesFromURL(string url) {
            return CopyStreamToByteArray(GetWebResponse(url, HTTPGET));
        }

        public static Stream GetWebResponse(String url, String sMethod) {
            return GetWebResponse(url, sMethod, "", "");
        }

        public static Stream GetWebResponse(String url, String sMethod, string sUserName, string sPassword) {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = sMethod;

            if (!string.IsNullOrEmpty(sUserName) && !string.IsNullOrEmpty(sPassword)) 
                request.Credentials = new NetworkCredential(sUserName, sPassword);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
        }

        /// <summary> For copying a non-seekable stream to a byte array </summary>
        private static byte[] CopyStreamToByteArray(Stream stream) {
            byte[] buffer = new byte[1024];

            using (MemoryStream outStream = new MemoryStream()) {
                int bytesRead;

                do {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    outStream.Write(buffer, 0, bytesRead);
                } while (bytesRead > 0);

                return outStream.ToArray();
            }
        }

        /// <summary> UrlDecodes a string without requiring System.Web </summary>
        /// <remarks> This is to avoid including system.web which means it can use the client only version
        /// of the framework, giving a potentially smaller framework download</remarks>
        public static string UrlDecode(string text) {
            // pre-process for + - signs space formatting since System.Uri doesn't handle it
            // TODO use regular expressions here
            text = text.Replace("+", "");
            text = text.Replace("-", "");
            text = text.Replace("%", "");
            text = text.Replace("&lt", "<");
            text = text.Replace("&gt", ">");
            return text;
        }

    }
}
