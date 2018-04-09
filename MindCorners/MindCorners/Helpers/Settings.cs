// Helpers/Settings.cs

using System;
using MindCorners.CustomControls;
using MindCorners.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

namespace MindCorners.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
    private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

    #region Setting Constants

    private const string SettingsKey = "settings_key";
    private static readonly string SettingsDefault = string.Empty;

    #endregion


    public static string GeneralSettings
    {
      get
      {
        return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
      }
      set
      {
        AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
      }
    }


        public static User CurrnetUser
        {
            get
            {
                return new User()
                {
                    Email = CurrentUserEmail,
                    FirstName = CurrentUserFirstName,
                    LastName = CurrentUserLastName,
                    FullName = CurrentUserFirstName + " " + CurrentUserLastName,
                    Id = CurrentUserId,
                    Password = CurrentUserPassword,
                    ProfileImageString = CurrentUserProfileImageString,
                };
            }
            set
            {
                if (value != null)
                {
                    CurrentUserEmail = value.Email;
                    CurrentUserFirstName = value.FirstName;
                    CurrentUserLastName = value.LastName;
                    CurrentUserFullName = value.FullName;
                    CurrentUserId = value.Id;
                    CurrentUserPassword = value.Password;
                    CurrentUserProfileImageString = value.ProfileImageString;
                }
                else
                {
                    CurrentUserEmail = null;
                    CurrentUserFirstName = null;
                    CurrentUserLastName = null;
                    CurrentUserFullName = null;
                    CurrentUserId = Guid.Empty;
                    CurrentUserPassword = null;
                    CurrentUserProfileImageString = null;
                }
            }
        }

      public static CustomSlider CurrentAudioPlayer;

      #region UserData

        //public static string GeneralSettings
        //{
        //    get { return AppSettings.GetValueOrDefault(nameof(GeneralSettings), string.Empty); }

        //    set { AppSettings.AddOrUpdateValue(nameof(GeneralSettings), value); }
        //}

        public static string CurrentUserFirstName
        {
            get { return AppSettings.GetValueOrDefault<string>("CurrentUserFirstName", null); }
            set { AppSettings.AddOrUpdateValue<string>("CurrentUserFirstName", value);}
        }
        public static string CurrentUserLastName
        {
            get { return AppSettings.GetValueOrDefault<string>("CurrentUserLastName", null); }
            set { AppSettings.AddOrUpdateValue<string>("CurrentUserLastName", value); }
        }

        public static string CurrentUserEmail
        {
            get { return AppSettings.GetValueOrDefault<string>("CurrentUserEmail", null); }
            set { AppSettings.AddOrUpdateValue<string>("CurrentUserEmail", value); }
        }
        public static string CurrentUserFullName
        {
            get { return AppSettings.GetValueOrDefault<string>("CurrentUserFullName", null); }
            set { AppSettings.AddOrUpdateValue<string>("CurrentUserFullName", value); }
        }
        public static string CurrentUserPassword
        {
            get { return AppSettings.GetValueOrDefault<string>("CurrentUserPassword", null); }
            set { AppSettings.AddOrUpdateValue<string>("CurrentUserPassword", value); }
        }
        public static Guid CurrentUserId
        {
            get { return AppSettings.GetValueOrDefault<Guid>("CurrentUserId", Guid.Empty); }
            set { AppSettings.AddOrUpdateValue<Guid>("CurrentUserId", value); }
        }

        public static string CurrentUserProfileImageString
        {
            get { return AppSettings.GetValueOrDefault<string>("CurrentUserProfileImageString", null); }
            set { AppSettings.AddOrUpdateValue<string>("CurrentUserProfileImageString", value); }
        }
        #endregion


        public static bool IsActivationCodeOk
        {
            get { return AppSettings.GetValueOrDefault<bool>("IsActivationCodeOk", false); }
            set { AppSettings.AddOrUpdateValue<bool>("IsActivationCodeOk", value); }
        }

        public static Guid InvitationId
        {
            get { return AppSettings.GetValueOrDefault<Guid>("InvitationId", Guid.Empty); }
            set { AppSettings.AddOrUpdateValue<Guid>("InvitationId", value); }
        }

        public static string ProfilePictureFilePath
        {
            get { return AppSettings.GetValueOrDefault<string>("ProfilePictureFilePath", null); }
            set { AppSettings.AddOrUpdateValue<string>("ProfilePictureFilePath", value); }
        }



        public static byte[] ProfilePictureStream
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("ProfilePictureStream"))
                {
                    return (byte[])Application.Current.Properties["ProfilePictureStream"];
                    // do something with id
                }
                return null;
            }
            set
            {
                Application.Current.Properties["ProfilePictureStream"] = value;
            }
        }









    }
}