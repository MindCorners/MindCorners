using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Common.Code.Enums
{
    public enum MessageTemplateTypes
    {
        RegistrationActivationCode,
        InvitationToCircle,

        NotificationAboutInvitationToContactsRecieved,
        NotificationAboutInvitationToContactsConfirmed,
        NotificationAboutInvitationToCircleRecieved,
        NotificationAboutInvitationToCircleConfirmed,
        NotificationAboutNewPromptInCircle,
        NotificationAboutReply,
        NotificationAboutReplyWasRead,
        NotificationAboutTellMeMore
    }
}
