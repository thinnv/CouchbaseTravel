using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;
using try_cb_dotnet.Models;

namespace try_cb_dotnet.Helper
{
    public class MailHelper
    {
        private class EmailInfo
        {
            private string _email;

            public string Email
            {
                get { return _email; }
                set { _email = value; }
            }
        }

        private delegate void SendEmailDelegate(MailMessage m);

        private static void SendEmailResponse(IAsyncResult ar)
        {
            try
            {
                SendEmailDelegate sd = (SendEmailDelegate)(ar.AsyncState);

                sd.EndInvoke(ar);
            }
            catch (Exception ex)
            {
                Exception Error = new Exception("Send Email Delegate" + ex.Message);

            }

        }

        public bool SendEmail(string to, string bcc, string cc, string subject, string body, AccountEmail account)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.sparkpostmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                string emailSend = account.EmailSend;
                string pass = account.Pass;
                client.Credentials = new System.Net.NetworkCredential(account.Account, pass);
                //SmtpSection smtpSec = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                // Initialize the mailmessage class
                MailMessage message = new MailMessage();
                message.From = new MailAddress(emailSend,
                                             emailSend);
                // Set the recepient address of the mail message
                if ((to != null) && (to != string.Empty))
                {
                    to = to.Replace(";", ",");
                    if (to.Contains(","))
                    {
                        string[] sto = to.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s in sto)
                        {
                            if (Globals.ValidateEmail(s))
                                message.To.Add(new MailAddress(s));
                        }
                    }
                    else
                    {
                        if (Globals.ValidateEmail(to))
                            message.To.Add(new MailAddress(to));
                    }
                }

                // Check if the bcc value is null or an empty string
                if ((bcc != null) && (bcc != string.Empty))
                {
                    bcc = bcc.Replace(";", ",");
                    if (bcc.Contains(","))
                    {
                        string[] sbcc = bcc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s in sbcc)
                        {
                            if (Globals.ValidateEmail(s))
                                message.Bcc.Add(new MailAddress(s));
                        }
                    }
                    else
                    {
                        if (Globals.ValidateEmail(bcc))
                            message.Bcc.Add(new MailAddress(bcc));
                    }
                }
                // Check if the cc value is null or an empty value
                if ((cc != null) && (cc != string.Empty))
                {
                    cc = cc.Replace(";", ",");
                    if (cc.Contains(","))
                    {
                        string[] scc = cc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s in scc)
                        {
                            if (Globals.ValidateEmail(s))
                                message.CC.Add(new MailAddress(s));
                        }
                    }
                    else
                    {
                        if (Globals.ValidateEmail(cc))
                            message.CC.Add(new MailAddress(cc));
                    }
                }

                message.Subject = subject;
                // Set the body of the mail message
                message.Body = body;

                message.IsBodyHtml = true;

                // Set the priority of the mail message to normal
                message.Priority = MailPriority.Normal;
                UTF8Encoding Encode = new UTF8Encoding();
                message.BodyEncoding = Encode;

                /* Create SMTP Client and add credentials */
                //  SmtpClient smtpClient = new SmtpClient(smtpSec.Network.Host, smtpSec.Network.Port);
                // smtpClient.UseDefaultCredentials = false;
                /* Email with Authentication */
                //   smtpClient.Credentials = new NetworkCredential(smtpSec.Network.UserName, smtpSec.Network.Password);
                //smtpClient.EnableSsl = true;
                /* Send the message */
                client.Send(message);



                SendEmailDelegate sd = new SendEmailDelegate(client.Send);
                AsyncCallback cb = new AsyncCallback(SendEmailResponse);
                sd.BeginInvoke(message, cb, sd);

                //System.Threading.Thread th = new Thread(() => SendMail(smtpClient, message));
                //th.IsBackground = true;
                //th.Start();
                // ts.
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ReadEmailTemplate(string path, ref string subject, ref string body)
        {
            string result = string.Empty;
            string[] lines;
            string file_path = path;
            try
            {
                file_path = HttpContext.Current.Server.MapPath(path);
            }
            catch { }

            int i;

            if (File.Exists(file_path))
            {
                lines = File.ReadAllLines(file_path);
                if (lines.Length > 0)
                {
                    subject = lines[0];
                    for (i = 1; i < lines.Length; i++)
                    {
                        body += lines[i];
                    }
                    return true;
                }
            }
            return false;
        }
    }
}