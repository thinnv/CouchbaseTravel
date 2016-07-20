using Couchbase.Linq.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace try_cb_dotnet.Models
{

    [EntityTypeFilter("accountEmail")]
    public class AccountEmail
    {
        [JsonProperty("domain")]
        public string Domain { get; set; }
        [JsonProperty("account")]
        public string Account { get; set; }
        [JsonProperty("pass")]
        public string Pass { get; set; }
        [JsonProperty("emailSend")]
        public string EmailSend { get; set; }
    }
}