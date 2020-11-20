using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixXamarinApp.Models
{
    public class Register
    {

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }
        
    }
}
