using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixXamarinApp.Models
{
   public class UserDetailCredentials
    {
        [JsonProperty("userName")]
        public string userName { get; set; }
        [JsonProperty("webGuid")]
        public string webGuid { get; set; }
    }
}
