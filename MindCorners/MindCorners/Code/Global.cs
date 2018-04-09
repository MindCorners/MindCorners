using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.DAL;
using MindCorners.Helpers;
using MindCorners.Models;

namespace MindCorners.Code
{
	public static class Global
	{
		public static int NewNotificationsCount { get; set;}
		public static async Task<int> LoadNewNotifications()
		{
			NotificationRepository notificationRepository = new NotificationRepository();
			return NewNotificationsCount = await notificationRepository.GetUnreadCount();
		}

		private static List<Contact> contacts;
		public static List<Contact> Contacts
		{
			get { return contacts; }
			set
			{
				contacts = value;
			}
		}

		private static List<Circle> circles;
		public static List<Circle> Circles
		{
			get { return circles; }
			set
			{
				circles = value;
			}
		}

		private static List<Post> latestPosts;
		public static List<Post> LatestPosts
		{
			get { return latestPosts; }
			set
			{
				latestPosts = value;
			}
		}

		public static async Task<List<Contact>> LoadContacts()
		{
			ContactRepository contactRepository = new ContactRepository();
			var contacts = await contactRepository.GetAll(Settings.CurrentUserId);
			if (contacts != null)
			{
				Contacts = new List<Contact>(contacts.Select(p => new Contact
					{
						Email = p.Email,
						Id = p.Id,
						FirstName = p.FirstName,
						LastName = p.LastName,
						IsActivated = p.IsActivated,
						ProfileImageString = p.ProfileImageString
					}));
				return new List<Contact> (Contacts);
			}
			return new List<Contact>();
		}

		public static async Task<List<Circle>> LoadCircles()
		{
			CircleRepository circleRepository = new CircleRepository();
			var circles = await circleRepository.GetAll(Settings.CurrentUserId);
			if (circles != null)
			{
				Circles = new List<Circle>(circles.Select(p => new Circle
					{
						Id = p.Id,
						Name = p.Name,
						IsCreatedByUser = p.IsCreatedByUser
					}));
				return new List<Circle> (Circles);
			}
			return new List<Circle>();
		}

	}
}
