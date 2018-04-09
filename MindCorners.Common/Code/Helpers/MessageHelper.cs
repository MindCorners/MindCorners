using MindCorners.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Common.Code.Enums;

namespace MindCorners.Common.Code.Helpers
{
    public static class MessageHelper
    {
        public static void SendMessage(MessageTemplateTypes messageTemplateType, Dictionary<string, string> values, Guid? currentUserOrganizationId, string email, List<System.Net.Mail.Attachment> attachments = null, string bccEmail = null)
        {
            var messageTemplate = GetMessageTemplateWithReplacedBody(messageTemplateType, values);
            if (messageTemplate != null)
            {
                SendEmail(currentUserOrganizationId, email, messageTemplate.Name, messageTemplate.BodyReplaced, attachments, bccEmail);
            }
        }


        public static MessageTemplate GetMessageTemplateWithReplacedBody(MessageTemplateTypes messageTemplateType, Dictionary<string, string> values)
        {
            using (MindCornersEntities context = new MindCornersEntities())
            using (MessageTemplateRepository messageTemplateRepository = new MessageTemplateRepository(context, Guid.Empty, null))
            {
                var messageTemplate = messageTemplateRepository.GetMessageTemplateByType(messageTemplateType);
                if (messageTemplate != null)
                {
                    StringBuilder sb = new StringBuilder(messageTemplate.Body);
                    foreach (var value in values)
                    {
                        sb.Replace(value.Key, value.Value);
                    }
                    messageTemplate.BodyReplaced = sb.ToString();
                    return messageTemplate;
                }
            }

            return null;
        }

        public static void SendEmail(Guid? currentUserOrganizationId,string email, string subject, string body, List<System.Net.Mail.Attachment> attachments = null, string bccEmail = null)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                try
                {
                    var smtp = new SmtpClient
                    {
                        Host = ConfigRepository.SmtpServer(currentUserOrganizationId),
                        Port = ConfigRepository.SmtpServerPort(currentUserOrganizationId),
                        EnableSsl = true,  // am mniSvnelobas aigebs Web.config-idan
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                    };
                    if (!string.IsNullOrWhiteSpace(ConfigRepository.SmtpServerUsername(currentUserOrganizationId)) &&
                        !string.IsNullOrWhiteSpace(ConfigRepository.SmtpServerPassword(currentUserOrganizationId)))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(ConfigRepository.SmtpServerUsername(currentUserOrganizationId),
                                                                 ConfigRepository.SmtpServerPassword(currentUserOrganizationId));
                    }
                    else
                    {
                        smtp.UseDefaultCredentials = true;
                    }
                    var messageAddress = new MailAddress(ConfigRepository.SmtpServerFrom(currentUserOrganizationId), "MindCorners");

                    using (var message = new MailMessage(messageAddress, new MailAddress(email))
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    })
                    {
                        if (attachments != null)
                        {
                            foreach (var attachment in attachments)
                            {
                                message.Attachments.Add(attachment);
                            }
                        }
                        if (!string.IsNullOrEmpty(bccEmail))
                        {
                            message.Bcc.Add(new MailAddress(bccEmail));
                        }
                        smtp.Send(message);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                }
            }
        }
    }
}
