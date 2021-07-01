using System;

namespace iDAS.Core
{
    public class WebClientHelper
    {
        public static bool Request(string url)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            bool success = Convert.ToBoolean(client.DownloadString(url));
            return success;
        }
    }
}
