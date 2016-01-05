using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Diagnostics.Contracts;
using System.Net.Mail;
using System.Net;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.IO;

namespace Netricity.Common
{
   /// <summary>
   /// Contains emailing functions.
   /// </summary>
   public class Mailer
   {
      /// <summary>
      /// Sends an email on a new thread.
      /// </summary>
      /// <param name="mailSettings">The mailSettings.</param>
      /// <param name="toAddress">To address.</param>
      /// <param name="toName">To name. Optional. Ignored if null or whitespace.</param>
      /// <param name="ccAddressesCsv">Optional CSV of CC addresses.</param>
      /// <param name="bccAddressesCsv">Optional CSV of BCC addresses.</param>
      /// <param name="subject">The subject.</param>
      /// <param name="body">The body.</param>
      /// <param name="isBodyHtml">If set to <c>true</c> the body is HTML.</param>
      /// <param name="attachmentDosPaths">Optional full DOS paths to file attachments (semi-colon-separated).</param>
      /// <param name="priority">Priority of the email.</param>
      /// <param name="replacements">Optional dictionary of key-value pairs of replacememts for the email body.</param>
      public static void SendMail(MailSettings mailSettings, string toAddress, string toName,
         string ccAddressesCsv, string bccAddressesCsv, string subject, string body, bool isBodyHtml, string attachmentDosPaths = null, MailPriority priority = MailPriority.Normal, IDictionary replacements = null)
      {
         var args = new object[] {
            mailSettings,
            toAddress,
            toName,
            ccAddressesCsv,
            bccAddressesCsv,
            subject,
            body,
            isBodyHtml,
            attachmentDosPaths,
            priority,
            replacements
         };

         Thread newThread = new Thread(SendMail);
         newThread.Start(args);
      }

      /// <summary>
      /// Sends the template mail.
      /// </summary>
      /// <param name="mailSettings">STMP email settings</param>
      /// <param name="fromAddress">From address.</param>
      /// <param name="fromDisplayName">From display name.</param>
      /// <param name="toAddress">To address.</param>
      /// <param name="toName">To display name.</param>
      /// <param name="ccAddressesCSV">Optional CC addresses (CSV).</param>
      /// <param name="bccAddressesCSV">Optional BCC addresses CSV.</param>
      /// <param name="subject">The subject.</param>
      /// <param name="templateFilePath">The template file path.</param>
      /// <param name="replacements">The replacements.</param>
      /// <param name="appRootFolder">'/' if site is running in the base folder, or '/foldername/' if site is running in a sub-folder.</param>
      /// <param name="templateVirtualFolder">The template virtual folder.</param>
      /// <param name="wrapperTemplateFilePath">Optional wrapper template file path.</param>
      /// <param name="isBodyHtml">Set to true if the body of the email is HTML; false otherwise.</param>
      public static void SendTemplateMail(MailSettings mailSettings, string toAddress, string toName,
         string ccAddressesCSV, string bccAddressesCSV, string subject, string templateFilePath,
         ListDictionary replacements, string templateVirtualFolder, string appRootFolder, string wrapperTemplateFilePath = null, bool isBodyHtml = true, MailPriority priority = MailPriority.Normal,
         string attachmentDosPaths = null)
      {
         if (File.Exists(templateFilePath))
         {
            StringBuilder messageText = null;
            string templateText = null;

            if (File.Exists(templateFilePath))
            {
               templateText = File.ReadAllText(templateFilePath);
            }
            else
            {
               throw new Exception("Cannot find template " + templateFilePath);
            }

            if (!string.IsNullOrEmpty(wrapperTemplateFilePath) && File.Exists(wrapperTemplateFilePath))
            {
               string str = File.ReadAllText(wrapperTemplateFilePath).Replace("@@WrapperContents", templateText);
               messageText = new StringBuilder(str);
            }
            else
            {
               messageText = new StringBuilder(templateText);
            }

            // Add some mandatory fields to the replacements
            if (replacements == null)
            {
               // Ensure not null.
               replacements = new ListDictionary();
            }

            replacements.Add("@@HttpRoot", string.Format("http://{0}/", mailSettings.Host));
            replacements.Add("@@TemplateFolder", string.Format("http://{0}{1}{2}", mailSettings.Host, appRootFolder, templateVirtualFolder.Trim("~/".ToCharArray())));

            SendMail(
               mailSettings,
               toAddress,
               toName,
               ccAddressesCSV,
               bccAddressesCSV,
               subject,
               messageText.ToString(),
               true,
               attachmentDosPaths,
               priority,
               replacements);
         }
         else
         {
            //Every1.Core.Web.Utils.TraceWarn("SendTemplateMail", "templateFilePath " + templateFilePath + " does not exist");
         }
      }

      /// <summary>
      /// Sends an email. To be called by new thread.
      /// </summary>
      /// <param name="args">The arguments.</param>
      private static void SendMail(object args)
      {
         Contract.Requires(args is object[]);

         var argArray = (object[])args;

         Contract.Requires(argArray != null);
         Contract.Requires(new[] { 8, 9, 10, 11 }.Contains(argArray.Length));

         MailSettings mailSettings = null;
         string toAddress = null;
         string toName = null;
         string ccAddressCsv = null;
         string bccAddressCsv = null;
         string subject = null;
         string body = null;
         bool isBodyHtml = false;
         string attachmentDosPaths = null;
         MailPriority priority = MailPriority.Normal;
         IDictionary replacements = null;

         // Check mandatory params
         Contract.Requires(argArray[0] is MailSettings);
         mailSettings = (MailSettings)argArray[0];

         Contract.Requires(argArray[1] is string);
         toAddress = (string)argArray[1];

         Contract.Requires(argArray[2] is string);
         toName = (string)argArray[2];

         Contract.Requires(argArray[3] is string);
         ccAddressCsv = (string)argArray[3];

         Contract.Requires(argArray[4] is string);
         bccAddressCsv = (string)argArray[4];

         Contract.Requires(argArray[5] is string);
         subject = (string)argArray[5];

         Contract.Requires(argArray[6] is string);
         body = (string)argArray[6];

         Contract.Requires(argArray[7] is bool);
         isBodyHtml = (bool)argArray[7];

         if (argArray.Length > 8)
         {
            // Check attachmentDosPaths
            Contract.Requires(argArray[8] is string);
            attachmentDosPaths = (string)argArray[8];
         }

         if (argArray.Length > 9)
         {
            // Check priority
            Contract.Requires(argArray[9] is MailPriority);
            priority = (MailPriority)argArray[9];
         }

         if (argArray.Length > 10)
         {
            // Check dictionary of replacements
            Contract.Requires(argArray[10] is IDictionary);
            replacements = (IDictionary)argArray[10];

            if (replacements != null)
            {
               foreach (var key in replacements.Keys)
               {
                  Contract.Requires(key is string, "Replacement keys must be of type string");
                  Contract.Requires(replacements[key] is string, "Replacement values must be of type string");
               }
            }
         }

         SendMailInternal(mailSettings, toAddress, toName, ccAddressCsv, bccAddressCsv, subject, body, isBodyHtml, attachmentDosPaths, priority, replacements);
      }

      /// <summary>
      /// Sends an mail. To be called from a new thread.
      /// </summary>
      /// <param name="mailSettings">The mail settings.</param>
      /// <param name="toAddress">To address.</param>
      /// <param name="toName">To name. Optional. Ignored if null or whitespace.</param>
      /// <param name="ccAddressCsv">The cc address CSV.</param>
      /// <param name="bccAddressCsv">The BCC address CSV.</param>
      /// <param name="subject">The subject.</param>
      /// <param name="body">The body.</param>
      /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
      /// <param name="attachmentDosPaths">The attachment dos paths.</param>
      /// <param name="priority">The priority.</param>
      /// <param name="replacements">The replacements.</param>
      /// <returns></returns>
      private static bool SendMailInternal(MailSettings mailSettings, string toAddress, string toName, string ccAddressCsv, string bccAddressCsv, string subject, string body, bool isBodyHtml, string attachmentDosPaths = null, MailPriority priority = MailPriority.Normal, IDictionary replacements = null)
      {
         // Send the email
         try
         {
            // Build From address
            MailAddress fromMailAdd = null;

            if (!string.IsNullOrWhiteSpace(mailSettings.FromName))
            {
               fromMailAdd = new MailAddress(mailSettings.FromAddress, mailSettings.FromName);
            }
            else
            {
               fromMailAdd = new MailAddress(mailSettings.FromAddress);
            }

            // Build To address
            MailAddress toMailAddress = null;

            if (!string.IsNullOrWhiteSpace(toName))
            {
               toMailAddress = new MailAddress(toAddress, toName);
            }
            else
            {
               toMailAddress = new MailAddress(toAddress);
            }

            var mailDef = new MailDefinition();
            mailDef.IsBodyHtml = isBodyHtml;
            mailDef.Priority = priority;
            mailDef.Subject = subject;
            mailDef.From = fromMailAdd.Address;
            // Need to set From here, otherwise CreateMailMessage will blow up if no smtp from setting in app.config or web.config. We'll update it further down.

            //Using mailMsg As New MailMessage()

            if (replacements == null)
            {
               // Replacements mustn't be empty
               replacements = new ListDictionary();
            }

            using (var mailMsg = mailDef.CreateMailMessage(toAddress, replacements, body, new System.Web.UI.LiteralControl()))
            {
               {
                  mailMsg.From = fromMailAdd;
                  // Update From with MailAddress object so we also apply the sender's name
                  //.To.Add(toMailAddress)
                  mailMsg.To.Clear();
                  mailMsg.To.Add(toMailAddress);
                  // Update To with MailAddress object so we also apply the recipient's name
                  mailMsg.Subject = subject;
                  //.Body = body
                  mailMsg.IsBodyHtml = isBodyHtml;
                  mailMsg.Priority = priority;

                  if (!string.IsNullOrWhiteSpace(mailSettings.ReplyToAddress))
                  {
                     mailMsg.ReplyToList.Add(mailSettings.ReplyToAddress);
                  }

                  if (!string.IsNullOrWhiteSpace(ccAddressCsv))
                  {
                     mailMsg.CC.Add(ccAddressCsv);
                  }

                  if (!string.IsNullOrWhiteSpace(bccAddressCsv))
                  {
                     mailMsg.Bcc.Add(bccAddressCsv);
                  }

                  if (!string.IsNullOrEmpty(attachmentDosPaths))
                  {
                     string[] strAttach = attachmentDosPaths.Split(";".ToCharArray());

                     foreach (var strFile in strAttach)
                     {
                        Attachment attachment = new Attachment(strFile);
                        mailMsg.Attachments.Add(attachment);
                     }
                  }
               }

               using (SmtpClient smtp = new SmtpClient())
               {
                  // If provided, override default host and port in web.config with values passed in.
                  if (mailSettings.Port > 0 && !string.IsNullOrWhiteSpace(mailSettings.Host))
                  {
                     smtp.Host = mailSettings.Host;
                     smtp.Port = mailSettings.Port;
                  }

                  if (!string.IsNullOrWhiteSpace(mailSettings.UserName) && !string.IsNullOrWhiteSpace(mailSettings.Password))
                  {
                     smtp.Credentials = new NetworkCredential(mailSettings.UserName, mailSettings.Password);
                  }
                  else
                  {
                     smtp.UseDefaultCredentials = true;
                  }

                  //if (!(mailSettings.DeliveryMethod == null))
                  if ((int)mailSettings.DeliveryMethod > 0)
                  {
                     smtp.DeliveryMethod = mailSettings.DeliveryMethod;
                  }

                  smtp.EnableSsl = mailSettings.EnableSsl;
                  smtp.Send(mailMsg);
               }
            }

         }
         catch (Exception ex)
         {
            var methodDesc = typeof(Mailer).FullName + ".SendMailInternal()";
            //Every1.Core.Web.Utils.TraceWarn(methodDesc, ex.Message + ex.InnerException != null ? ex.InnerException.Message : string.Empty);
            //Every1.Core.ErrorLogger.LogError(methodDesc, ex);
            return false;
         }

         return true;

      }
   }
}
