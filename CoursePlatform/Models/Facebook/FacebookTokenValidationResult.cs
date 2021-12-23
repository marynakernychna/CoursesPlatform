namespace CoursesPlatform.Models.Facebook
{
    public class FacebookTokenValidationResult
    {
        public FacebookTokenValidationData Data { get; set; }
    }

    public class FacebookTokenValidationData
    {
        public string app_id { get; set; }
        public string type { get; set; }
        public string application { get; set; }
        public long data_access_expires_at { get; set; }
        public long expires_at { get; set; }
        public bool is_valid { get; set; }
        public string[] scopes { get; set; }
        public string user_id { get; set; }
    }
}
