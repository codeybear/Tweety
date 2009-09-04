﻿using System;
using System.Net;
using System.IO;
using System.Xml;

namespace Core
{
    static class WebHelper
    {
        public const string HTTPGET = "GET";
        public const string HTTPPOST = "POST";

        public static byte[] GetBytesFromURL(string sURL) {
            return CopyStreamToByteArray(GetWebResponse(sURL, HTTPGET));
        }

        public static Stream GetWebResponse(String sURL, String sMethod) {
            return GetWebResponse(sURL, sMethod, "", "");
        }

        public static Stream GetWebResponse(String sURL, String sMethod, string sUserName, string sPassword) {
            HttpWebRequest request = WebRequest.Create(sURL) as HttpWebRequest;
            request.Method = sMethod;

            if (!string.IsNullOrEmpty(sUserName) && !string.IsNullOrEmpty(sPassword)) 
                request.Credentials = new NetworkCredential(sUserName, sPassword);

            HttpWebResponse Response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine("{0} - {1}", Response.StatusCode, Response.StatusDescription);
            return Response.GetResponseStream();
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
