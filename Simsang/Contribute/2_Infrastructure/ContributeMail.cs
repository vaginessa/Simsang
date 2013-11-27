using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;



namespace Simsang.Contribute.Infrastructure
{
  class ContributeMail : IContribute
  {

    #region MEMBERS

    private static IContribute cInstance;

    #endregion


    #region PUBLIC

    private ContributeMail()
    {
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static IContribute getInstance()
    {
      if (cInstance == null)
        cInstance = new ContributeMail();

      return (cInstance);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pData"></param>
    /// <returns></returns>
    public bool sendContribution(String pData)
    {
      bool lRetVal = false;
      return (true);


      if (!String.IsNullOrEmpty(pData))
      {
        try
        {
          MailMessage lSMTPMsg = new MailMessage();
          lSMTPMsg.From = new System.Net.Mail.MailAddress(Config.ContributionSenderEmail);
          lSMTPMsg.Subject = Config.ContributionSubject;

          // The important part -- configuring the SMTP client
          SmtpClient lSMTPClient = new SmtpClient();
          lSMTPClient.Timeout = 5000;
          lSMTPClient.Port = Config.SMTPPort;
          lSMTPClient.EnableSsl = false; // true;
          lSMTPClient.DeliveryMethod = SmtpDeliveryMethod.Network; // [2] Added this
          lSMTPClient.UseDefaultCredentials = false; // [3] Changed this
          lSMTPClient.Host = Config.SMTPServer;

          //recipient address
          lSMTPMsg.To.Add(new MailAddress(Config.ContributionRecipientEmail));

          //Formatted mail body
          lSMTPMsg.IsBodyHtml = false; // true;

          lSMTPMsg.Body = pData;
          lSMTPClient.Send(lSMTPMsg);
          lRetVal = true;
        }
        catch (Exception lEx)
        {
          LogConsole.Main.LogConsole.pushMsg(lEx.ToString());
        }
      } // if (...

      return (lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>

    public String getMethod()
    {
      return ("Email");
    }

    #endregion

  }
}
