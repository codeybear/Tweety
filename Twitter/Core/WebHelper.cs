using System;
using System.Net;
using System.IO;
using System.Xml;

namespace Core
{
    class WebHelper
    {
        public const string HTTPGET = "GET";
        public const string HTTPPOST = "POST";

        public static byte[] GetBytesFromURL(string sURL) {
            return CopyStreamToByteArray(GetWebResponse(sURL, HTTPGET));
        }

        public static System.Drawing.Bitmap GetBitmapFromURL(string sURL) {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(sURL);
            WebResponse Response = Request.GetResponse();
            Stream ResponseStream = Response.GetResponseStream();
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ResponseStream);

            return bmp;
        }

        public static Stream GetWebResponse(String sURL, String sMethod) {
            HttpWebRequest request = WebRequest.Create(sURL) as HttpWebRequest;
            request.Method = sMethod;
            return request.GetResponse().GetResponseStream();
        }

        public static Stream GetWebResponse(String sURL, String sMethod, string sUserName, string sPassword) {
            HttpWebRequest request = WebRequest.Create(sURL) as HttpWebRequest;
            request.Method = sMethod;
            request.Credentials = new NetworkCredential(sUserName, sPassword);
            return request.GetResponse().GetResponseStream();
        }

        /// <summary> For copying a non-seekable stream to a byte array </summary>
        private static byte[] CopyStreamToByteArray(Stream Stream) {
            MemoryStream OutStream = new MemoryStream();
            byte[] Buffer = new byte[1024];
            int iBytesRead;

            do {
                iBytesRead = Stream.Read(Buffer, 0, Buffer.Length);
                OutStream.Write(Buffer, 0, iBytesRead);
            } while (iBytesRead > 0);

            return OutStream.ToArray();
        }
    }
}
