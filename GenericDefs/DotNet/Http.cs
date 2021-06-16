using System.IO;
using System.Net;

namespace GenericDefs.DotNet
{
    public class Http
    {
        public class Request
        {
            public static string Get(string url)
            {
                WebClient client = new WebClient();

                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                string retVal = string.Empty;
                using (Stream data = client.OpenRead(url))
                {
                    using (StreamReader reader = new StreamReader(data)) { retVal = reader.ReadToEnd(); }
                }

                return retVal;
            }

            public static string GetHtmlResponse(string url)
            {
                string html = string.Empty;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }

                return html;
            }
        }
    }
}
