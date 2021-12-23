using CoursesPlatform.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoursesPlatform.Models.Facebook
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private const string TokenValidationUrl = "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";
        private const string UserInfoUrl = "https://graph.facebook.com/me?fields=first_name,last_name,email&access_token={0}";

        private readonly FacebookAuthSetting FacebookAuthSettings;
        private readonly IHttpClientFactory HttpClientFactory;

        public FacebookAuthService(FacebookAuthSetting FacebookAuthSetting, IHttpClientFactory HttpClientFactory)
        {
            this.FacebookAuthSettings = FacebookAuthSetting;
            this.HttpClientFactory = HttpClientFactory;
        }

        public async Task<FacebookTokenValidationResult> ValidateAccessTokenAsync(string accessToken)
        {
            var formattedUrl = string.Format(TokenValidationUrl, accessToken, FacebookAuthSettings.AppId, FacebookAuthSettings.AppSecret);
            var result = await HttpClientFactory.CreateClient().GetAsync(formattedUrl);
            result.EnsureSuccessStatusCode();

            var resopnceAsString = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FacebookTokenValidationResult>(resopnceAsString);
        }

        public async Task<FacebookUserInfoResult> GetUserInfoAsync(string accessToken)
        {
            var formattedUrl = string.Format(UserInfoUrl, accessToken);
            var result = await HttpClientFactory.CreateClient().GetAsync(formattedUrl);
            result.EnsureSuccessStatusCode();

            var resopnceAsString = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FacebookUserInfoResult>(resopnceAsString);
        }
    }
}
