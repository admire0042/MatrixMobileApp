using MimeKit;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MatrixXamarinApp.Models
{
    public class GetMessages
    {
        [JsonProperty("MessageId")]
        public int MessageId { get; set; }
        [JsonProperty("Subject")]
        public string Subject { get; set; }
        [JsonProperty("CreatedTime")]
        public DateTime CreatedTime { get; set; }
        [JsonProperty("Receipient")]
        public string Receipient { get; set; }

        [JsonProperty("Receipients")]
        public string Receipients { get; set; }

        [JsonProperty("From")]
        public string From { get; set; }

        [JsonProperty("OrigFrom")]
        public string OrigFrom { get; set; }

        [JsonProperty("Owner")]
        public string Owner { get; set; }
        
        [JsonProperty("IO")]
        public string IO { get; set; }
    }

    public class GetMessages2
    {
        [JsonProperty("MessageId")]
        public int MessageId { get; set; }
        [JsonProperty("Subject")]
        public string Subject { get; set; }
        [JsonProperty("CreatedTime")]
        public DateTime CreatedTime { get; set; }
        [JsonProperty("Receipient")]
        public string Receipient { get; set; }

        [JsonProperty("Receipients")]
        public string Receipients { get; set; }

        [JsonProperty("From")]
        public string From { get; set; }

        [JsonProperty("OrigFrom")]
        public string OrigFrom { get; set; }

        [JsonProperty("Owner")]
        public string Owner { get; set; }

        [JsonProperty("IO")]
        public string IO { get; set; }
    }

    public class GetFullMessages
    {
            [PrimaryKey]
            public int MessageId { get; set; }
            public string EML { get; set; }
        //public ImapClient Imap { get; set; }
    }

    public class GetFullMessages2
    {
        [PrimaryKey]
        public int MessageId { get; set; }
        public string EML { get; set; }
        //public ImapClient Imap { get; set; }
    }

    public class GetFulledMessages
    {
        public string To { get; set; }
        public string Reply_To { get; set; }
        public string Date { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; } 
        public string BodyText { get; set; }
    }

    public class GetFulledMessages2
    {
        public string To { get; set; }
        public string Reply_To { get; set; }
        public string Date { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string BodyText { get; set; }
    }

}