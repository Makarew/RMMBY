using System;
using System.Globalization;
using System.Net;
using System.Net.Http;

namespace RMMBYGB
{
    public class Client
    {
        public static HttpClient HttpClient { get; private set; }

        public static HttpClient Get()
        {
            if (!Singleton.HasInstance<HttpClient>())
            {
                Client.HttpClient = new HttpClient();
                Client.HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (compatible; RMMBY)");
                Singleton.SetInstance<HttpClient>(Client.HttpClient);
            }
            return Singleton.GetInstance<HttpClient>();
        }
    }
}
