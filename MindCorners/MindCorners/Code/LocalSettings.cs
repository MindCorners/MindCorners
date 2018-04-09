using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

namespace MindCorners.Code
{
    public static class LocalSettings
    {

        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }














        //public static Stream ProfilePictureStream
        //{
        //    get { return AppSettings.GetValueOrDefault<Stream>("ProfilePictureStream", null); }
        //    set { AppSettings.AddOrUpdateValue<Stream>("ProfilePictureStream", value); }
        //}
    }

    //public static class LocalSettings
    //{

    //    public static User CurrnetUser
    //    {
    //        get
    //        {
    //            if (Application.Current.Properties.ContainsKey("CurrnetUser"))
    //            {
    //                return Application.Current.Properties["CurrnetUser"] as User;
    //                // do something with id
    //            }
    //            return null;
    //        }
    //        set
    //        {
    //            Application.Current.Properties["CurrnetUser"] = value;
    //        }
    //    }

    //    public static bool IsActivationCodeOk
    //    {
    //        get
    //        {
    //            if (Application.Current.Properties.ContainsKey("IsActivationCodeOk"))
    //            {
    //                return (bool)Application.Current.Properties["IsActivationCodeOk"];
    //                // do something with id
    //            }
    //            return false;
    //        }
    //        set
    //        {
    //            Application.Current.Properties["IsActivationCodeOk"] = value;
    //        }
    //    }

    //    public static Guid? InvitationId
    //    {
    //        get
    //        {
    //            if (Application.Current.Properties.ContainsKey("InvitationId"))
    //            {
    //                return (Guid)Application.Current.Properties["InvitationId"];
    //                // do something with id
    //            }
    //            return null;
    //        }
    //        set
    //        {
    //            Application.Current.Properties["InvitationId"] = value;
    //        }
    //    }

    //    public static string ProfilePictureFilePath
    //    {
    //        get
    //        {
    //            if (Application.Current.Properties.ContainsKey("ProfilePictureFilePath"))
    //            {
    //                return (string)Application.Current.Properties["ProfilePictureFilePath"];
    //                // do something with id
    //            }
    //            return null;
    //        }
    //        set
    //        {
    //            Application.Current.Properties["ProfilePictureFilePath"] = value;
    //        }
    //    }

    //    public static Stream ProfilePictureStream
    //    {
    //        get
    //        {
    //            if (Application.Current.Properties.ContainsKey("ProfilePictureStream"))
    //            {
    //                return (Stream)Application.Current.Properties["ProfilePictureStream"];
    //                // do something with id
    //            }
    //            return null;
    //        }
    //        set
    //        {
    //            Application.Current.Properties["ProfilePictureStream"] = value;
    //        }
    //    }
    //}
}
