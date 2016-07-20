using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using try_cb_dotnet.Helper;
using try_cb_dotnet.Models;
using try_cb_dotnet.Storage.Couchbase;

namespace try_cb_dotnet.Controllers
{
    public class OrderController : ApiController
    {
        [HttpPost]
        public HttpContentResultModel<bool> InsertObjectJson(JObject model)
        {
            string bucket = CouchbaseConfigHelper.Instance.Bucket;
            var result = new HttpContentResultModel<bool>();
            try
            {
                if (String.IsNullOrEmpty(model["orderId"].ToString()))
                {
                    model["orderId"] = Guid.NewGuid().ToString();
                }
                var rs = CouchbaseStorageHelper.Instance.Upsert(model["orderId"].ToString(), model, bucket);

                if (!rs.Success || rs.Exception != null)
                {
                    throw new Exception("could not save user to Couchbase");
                }

                result.Data = true;
                result.StatusCode = Globals.StatusCode.Success.Code;
                result.Message = Globals.StatusCode.Success.Message;
                result.Result = true;
                result.ItemsCount = 1;

            }
            catch (Exception ex)
            {
                result.StatusCode = Globals.StatusCode.Error.Code;
                result.Message = ex.Message;
                result.Result = false;
                result.ItemsCount = 0;


            }



            return result;
        }
        [HttpGet]
        public HttpContentResultModel<bool> InsertAccountEmail()
        {
            string bucket = CouchbaseConfigHelper.Instance.Bucket;
            var accountEmail = new AccountEmail()
            {
                EmailSend = "admin@easywebhub.com",
                Account = "SMTP_Injection",
                Pass = "64afdd680ed97147de160afe53cb682798c38bc2",
                Domain = "easywebhub.com"

            };
            var result = new HttpContentResultModel<bool>();
            try
            {

                var rs = CouchbaseStorageHelper.Instance.Upsert(accountEmail.Domain, accountEmail, bucket);

                if (!rs.Success || rs.Exception != null)
                {
                    throw new Exception("could not save user to Couchbase");
                }

                result.Data = true;
                result.StatusCode = Globals.StatusCode.Success.Code;
                result.Message = Globals.StatusCode.Success.Message;
                result.Result = true;
                result.ItemsCount = 1;

            }
            catch (Exception ex)
            {
                result.StatusCode = Globals.StatusCode.Error.Code;
                result.Message = ex.Message;
                result.Result = false;
                result.ItemsCount = 0;


            }



            return result;
        }
    }

}
