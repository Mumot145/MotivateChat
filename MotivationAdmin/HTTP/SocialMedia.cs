using MotivationAdmin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MotivationAdmin.HTTP
{
    public partial class SocialMedia
    {
        HttpClient client = new HttpClient();
        static SocialMedia defaultInstance = new SocialMedia();
        private SocialMedia()
        {
           
        }
        public static SocialMedia DefaultService
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }
        public async Task<FacebookUser> GetFacebookId(string AccessToken)
        {

            var requestUrl = "https://graph.facebook.com/v2.8/me/"
                             + "?fields=name,picture,cover,age_range,devices,email,gender,is_verified"
                             + "&access_token=" + AccessToken;
            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            return JsonConvert.DeserializeObject<FacebookUser>(userJson);
        }
    }
}
