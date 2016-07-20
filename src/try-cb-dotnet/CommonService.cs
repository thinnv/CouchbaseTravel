using Couchbase.N1QL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using try_cb_dotnet.Helper;
using try_cb_dotnet.Models;
using try_cb_dotnet.Storage.Couchbase;

namespace try_cb_dotnet
{
    public class CommonService
    {


        public static void SendMail()
        {
            var mail = new MailHelper();
            string subject = "";
            string body = "";
            string mailTemp = "/templates/Email.htm";
            AccountEmail cauhinh = GetAccountEmail("easywebhub.com");
            //  string emailSend = ConfigurationManager.AppSettings["Email"];
            if (mail.ReadEmailTemplate(mailTemp, ref subject, ref body))
            {
                body = body.Replace("[GioiTinh]", "Anh");
                body = body.Replace("[HoTen]", "Thìn nguyễn");
                body = body.Replace("[DienThoai]", "0123456789");
                body = body.Replace("[NoiDung]", "Test Send");
                mail.SendEmail("quocbao.tn@gmail.com",
                    "nvthintt@gmail.com",
                    "",
                    subject, body, cauhinh);
            }

        }
        private static AccountEmail GetAccountEmail(string domain)
        {
            var query =
                 new QueryRequest(
                     "SELECT  c  FROM `" + CouchbaseConfigHelper.Instance.Bucket + "` as c  " +
                     "WHERE domain = $domain")
                 .AddNamedParameter("domain", domain);
            var rs = CouchbaseStorageHelper.Instance.ExecuteQuery(query);
            if (rs.Rows.Count > 0)
            {
                var account = new AccountEmail()
                {
                    Pass = rs.Rows[0]["c"]["Pass"],
                    EmailSend = rs.Rows[0]["c"]["EmailSend"],
                    Account = rs.Rows[0]["c"]["Account"],
                    Domain = rs.Rows[0]["c"]["Domain"],

                };


                return account;
            }
            return new AccountEmail();
        }

    }
}