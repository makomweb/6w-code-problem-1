using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TweetSearchCSharp
{
    public class Searcher
    {
        private const string Key = ""; // TODO enter your consumer key here!
        private const string Secret = ""; // TODO enter your consumer secret here!
        private readonly string _bearerToken;
        
        private static string BasicAuth
        {
            get { return Convert.ToBase64String(Encoding.UTF8.GetBytes(Key + ":" + Secret)); }
        }

        [DataContract]
        public class Credentials
        {
            [DataMember(Name = "access_token")]
            public string AccessToken { get; set; }
        }

        public static async Task<string> BearerTokenAsync()
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", BasicAuth);
                var responseMessage =
                    await
                        http.PostAsync("https://api.twitter.com/oauth2/token",
                            new StringContent("grant_type=client_credentials", Encoding.UTF8,
                                "application/x-www-form-urlencoded"));

                return (await Deserialize<Credentials>(responseMessage)).AccessToken;
            }
        }

        public Searcher(string bearerToken)
        {
            _bearerToken = bearerToken;
        }

        [DataContract]
        public class Tweet
        {
            [DataMember(Name = "text")]
            public string Text { get; set; }
        }

        [DataContract]
        public class SearchResponse
        {
            [DataMember(Name = "statuses")]
            public Tweet[] Tweets { get; set; }
        }

        public async Task<Tweet[]> SearchAsync(string term, int count = 100)
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
                var responseMessage =
                    await
                        http.GetAsync(string.Format("https://api.twitter.com/1.1/search/tweets.json?q={0}&count={1}",
                            Uri.EscapeDataString(term), count));

                return (await Deserialize<SearchResponse>(responseMessage)).Tweets;
            }
        }

        private static async Task<T> Deserialize<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new Exception(string.Format("HTTP request failed with {0}", response.StatusCode));

            try
            {
                var stream = await response.Content.ReadAsStreamAsync();
                using (var reader = new JsonTextReader(new StreamReader(stream, Encoding.UTF8)))
                {
                    return new JsonSerializer().Deserialize<T>(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Response for {0} could not be deserialized!", response.RequestMessage.RequestUri), ex);
            }
        }
    }
}
