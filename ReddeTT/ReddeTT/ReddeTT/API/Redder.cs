using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Settings;
using ReddeTT.Models;

namespace ReddeTT.API
{
    public class ReddeTT
    {
        // https://www.reddit.com/api/v1/authorize.compact?client_id=i0phHFtWDX9m1g&response_type=code&state=testje&redirect_uri=ReddeTT://callback&duration=permanent&scope=identity
        private string _authenticateUri = "https://www.reddit.com/api/v1/authorize.compact";
        private string _redirectUri = "ReddeTT://callback";
        private string _endpointUri = "https://oauth.reddit.com/api/v1/";
        private string _tokenUri = "https://www.reddit.com/api/v1/access_token";
        private string _clientId = "i0phHFtWDX9m1g";
        private string[] _scope = {"identity", "mysubreddits", "history"};

        public Uri CodeAuthenticationUri => new Uri(_authenticateUri + "?client_id=" + _clientId +
                        "&response_type=code&state=" + RandomString(10) + "&redirect_uri=" + _redirectUri +
                        "&duration=permanent&scope=" + string.Join("%20", _scope));
        public Uri TokenAuthenticationUri => new Uri(_tokenUri + "?grant_type=authorization_code&code=" + Code + "&redirect_uri=" + _redirectUri);

        public bool IsAuthenticated => !string.IsNullOrEmpty(Code) && !string.IsNullOrEmpty(AccessToken) && !string.IsNullOrEmpty(RefreshToken);

        public User CurrentUser { get; set; }

        public string Code
        {
            get
            {
                var code = CrossSettings.Current.GetValueOrDefault<string>("code");
                return string.IsNullOrWhiteSpace(code) ? null : code;
            }
        }
        public string AccessToken
        {
            get
            {
                var accessToken = CrossSettings.Current.GetValueOrDefault<string>("access_token");
                return string.IsNullOrWhiteSpace(accessToken) ? null : accessToken;
            }
        }
        public string RefreshToken
        {
            get
            {
                var refreshToken = CrossSettings.Current.GetValueOrDefault<string>("refresh_token");
                return string.IsNullOrWhiteSpace(refreshToken) ? null : refreshToken;
            }
        }

        public bool IsExpired
        {
            get
            {
                var expiresIn = CrossSettings.Current.GetValueOrDefault<DateTime>("token_expired");
                return expiresIn < DateTime.Now;
            }
        }

        public ReddeTT()
        {
            
        }

        private static Random _random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public async Task GetToken(string code)
        {
            using (var client = new HttpClient())
            {
                var byteArray = Encoding.UTF8.GetBytes(_clientId + ":" + string.Empty);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                var response = await client.PostAsync(_tokenUri + "?grant_type=authorization_code&code=" + code + "&redirect_uri=" + _redirectUri, new StringContent(""));
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<dynamic>(json);
                    CrossSettings.Current.AddOrUpdateValue("access_token", obj["access_token"].Value);
                    CrossSettings.Current.AddOrUpdateValue("refresh_token", obj["refresh_token"].Value);

                    DateTime expires = DateTime.Now.AddSeconds(obj["expires_in"].Value);
                    CrossSettings.Current.AddOrUpdateValue("token_expired", expires);
                }
                // TODO: catch error
            }
        }

        public async Task RefreshAccessToken()
        {
            using (var client = new HttpClient())
            {
                var byteArray = Encoding.UTF8.GetBytes(_clientId + ":" + string.Empty);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", RefreshToken)
                });

                var response = await client.PostAsync(_tokenUri, content);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<dynamic>(json);
                    CrossSettings.Current.AddOrUpdateValue("access_token", obj["access_token"].Value);

                    DateTime expires = DateTime.Now.AddSeconds(obj["expires_in"].Value);
                    CrossSettings.Current.AddOrUpdateValue("token_expired", expires);
                }
                // TODO: catch error
            }
        }
        public async Task GetCurrentUser()
        {
            using (var client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _endpointUri + "me");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                request.Headers.Add("User-Agent", Uri.EscapeDataString("android:com.arnvanhoutte.ReddeTT:v1.0.0 (by /u/nerdiator)"));
                
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    
                    CurrentUser = JsonConvert.DeserializeObject<User>(json);
                }
            }
        }
    }

    public class Settings
    {
        public string LookingFor
        {
            get
            {
                var lookingfor = CrossSettings.Current.GetValueOrDefault<string>("looking_for");
                return string.IsNullOrWhiteSpace(lookingfor) ? null : lookingfor;
            }
        }

    }
}