using System;
using System.Net.Mail;

namespace Netricity.Common
{
   public class MailSettings
   {
      public MailSettings()
      {
      }

      public MailSettings(SmtpDeliveryMethod deliveryMethod, string fromAddress, string fromName, string host, int port, bool enableSsl, string replyToAddress)
         : this(deliveryMethod, fromAddress, fromName, host, port, "anthony@netricity.co.uk", "rm250k5!", enableSsl, replyToAddress)
      {

      }

      public MailSettings(SmtpDeliveryMethod deliveryMethod, string fromAddress, string fromName, string host, int port, string userName, string password, bool enableSsl, string replyToAddress)
      {
         this.DeliveryMethod = deliveryMethod;
         this.FromAddress = fromAddress;
         this.FromName = fromName;
         this.Host = host;
         this.Port = port;
         //Me.UseDefaultCredentials = useDefaultCredentials
         this.UserName = userName;
         this.Password = password;
         this.EnableSsl = enableSsl;
         this.ReplyToAddress = replyToAddress;

      }

      SmtpDeliveryMethod _deliveryMethod = SmtpDeliveryMethod.Network;

      public SmtpDeliveryMethod DeliveryMethod
      {
         get { return this._deliveryMethod; }
         set { this._deliveryMethod = value; }
      }

      public string FromAddress { get; set; }

      public string FromName { get; set; }

      public string Host { get; set; }

      public int Port { get; set; }

      //public bool UseDefaultCredentials
      //{
      //	get;
      //	set;
      //}

      public string UserName { get; set; }

      public string Password { get; set; }

      public bool EnableSsl { get; set; }

      public string ReplyToAddress { get; set; }
   }
}
