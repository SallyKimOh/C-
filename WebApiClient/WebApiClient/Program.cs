using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace WebApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            String userName = "a1@a.com";
            String password = "T123456&t";
            var registerResult = Register(userName, password);

            Console.WriteLine("Registration Status Code: {0}", registerResult);

//            String token = GetToken(userName, password);
            Dictionary<string, string> token = GetTokenDictionary(userName, password);
            Console.WriteLine("");
            Console.WriteLine("Access Token:");
            Console.WriteLine(token);

            foreach (var kvp in token)
            {
                Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
            }
            Console.WriteLine("");

            Console.WriteLine("Getting User Info:");
            Console.WriteLine(GetUserInfo(token["access_token"]));

            Dictionary<string, string> userInfo = GetUserInfoDictionary(token["access_token"]);
            Dictionary<string, string> userInfo1 = GetUserInfoDictionary("adfasd");

            Console .WriteLine("userEmail:");
            Console.WriteLine(userInfo1.ContainsKey("Email"));
            Console.WriteLine(userInfo1);



            //           Console.Read();
        }


        static String Register(String email, String password)
        {
            var registerModel = new
            {
                Email = email,
                Password = password,
                ConfirmPassword = password
            };
            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync(
                    "http://localhost:2944/api/Account/Register",
                    registerModel).Result;
                return response.StatusCode.ToString();

            }
        }


        static String GetToken(String userName, String password)
        {
            var pairs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<string, string>("grant_type","password"),
                new KeyValuePair<string, string>("username",userName),
                new KeyValuePair<string, string>("Password",password)

            };

            var content = new FormUrlEncodedContent(pairs);
            using (var client = new HttpClient())
            {
                var response = client.PostAsync("http://localhost:2944/Token", content).Result;
                return response.Content.ReadAsStringAsync().Result;
            }

        }


        static Dictionary<string, string> GetTokenDictionary(String userName, String password)
        {
            var pairs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<string, string>("grant_type","password"),
                new KeyValuePair<string, string>("username",userName),
                new KeyValuePair<string, string>("Password",password)

            };

            var content = new FormUrlEncodedContent(pairs);
            using (var client = new HttpClient())
            {
                
                var response = client.PostAsync("http://localhost:2944/Token", content).Result;
//                var response = client.PostAsync("http://192.168.127.1:2944/Token", content).Result;

                var result = response.Content.ReadAsStringAsync().Result;

                Dictionary<string, string> tokenDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                return tokenDictionary;

            }

        }

        static Dictionary<string, string> GetUserInfoDictionary(string token)
        {

            using (var client = CreateClient(token))
            {
                var response = client.GetAsync("http://localhost:2944/api/Account/UserInfo").Result;
                var result = response.Content.ReadAsStringAsync().Result;
                Dictionary<string, string> userDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                return userDictionary;

            }

        }




        /*
                static String GetUserInfo(string token)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        var response = client.GetAsync("http://localhost:2944/api/Account/UserInfo").Result;
                        return response.Content.ReadAsStringAsync().Result;

                    }
                }
        */
        static String GetUserInfo(string token)
        {
            using (var client = CreateClient(token))
            {
                var response = client.GetAsync("http://localhost:2944/api/Account/UserInfo").Result;
                return response.Content.ReadAsStringAsync().Result;

            }
        }

 

        static HttpClient CreateClient(string token)
        {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            }
            return client;
        }
    }
}
