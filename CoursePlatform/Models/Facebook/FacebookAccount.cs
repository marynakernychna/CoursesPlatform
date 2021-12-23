﻿using Newtonsoft.Json;

namespace CoursesPlatform.Models.Facebook
{
    public class FacebookAccount
    {
        public string Id { get; set; }

        public string Email { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }
    }
}
