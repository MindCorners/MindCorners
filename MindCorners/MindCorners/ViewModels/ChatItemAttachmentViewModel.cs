using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.Code;
using MindCorners.DAL;
using MindCorners.Helpers;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.Pages;
using MindCorners.Pages.UserControls;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace MindCorners.ViewModels
{
	public class ChatItemAttachmentViewModel : BaseViewModel
	{

		private bool canSend;
		public bool CanSend
		{
			get { return canSend; }
			set
			{
				canSend = value;
				OnPropertyChanged();
			}
		}

		private int recordTimeSeconds;
		public int RecordTimeSeconds
		{
			get { return recordTimeSeconds; }
			set
			{
				recordTimeSeconds = value;
				OnPropertyChanged();
				TimeSpan time = TimeSpan.FromSeconds(RecordTimeSeconds);
				RecordTimeSecondsString = time.ToString(@"mm\:ss");
				EditingItem.FileDuration = RecordTimeSeconds;
			}
		}

		private string recordTimeSecondsString;
		public string RecordTimeSecondsString
		{
			get
			{
				//TimeSpan time = TimeSpan.FromSeconds(RecordTimeSeconds);
				//return time.ToString(@"mm\:ss\:fff");

				return recordTimeSecondsString;
			}
			set
			{
				recordTimeSecondsString = value;
				OnPropertyChanged();
			}
		}

		public ICommand SendPostCommand { get; set; }

		private Post parenPost;
		public Post ParentPost
		{
			get { return parenPost; }
			set
			{
				parenPost = value;
				OnPropertyChanged();
				//LoadChatInfo
			}
		}

		private bool canReply;
		public bool CanReply
		{
			get { return canReply; }
			set
			{
				canReply = value;
				OnPropertyChanged();
			}
		}

		private bool isFile;

		public bool IsFile
		{
			get { return isFile; }
			set { isFile = value; }
		}

		private string fileName;
		public string FileName
		{
			get { return fileName; }
			set
			{
				fileName = value;
				OnPropertyChanged();
			}
		}
		private string fileFullPath;
		public string FileFullPath
		{
			get { return fileFullPath; }
			set
			{
				fileFullPath = value;
				OnPropertyChanged();
			}
		}


		private ObservableCollection<Post> replies;
		public ObservableCollection<Post> Replies
		{
			get { return replies; }
			set
			{
				replies = value;
				OnPropertyChanged();
			}
		}

		private PostAttachment editigItem;
		public PostAttachment EditingItem
		{
			get { return editigItem; }
			set
			{
				editigItem = value;
				OnPropertyChanged();
				//LoadChatInfo
			}
		}

		private bool showTextTempate;
		public bool ShowTextTempate
		{
			get { return showTextTempate; }
			set
			{
				showTextTempate = value;
				OnPropertyChanged();
			}
		}

		private byte[] fileItemSourceArray;
		public byte[] FileItemSourceArray
		{
			get { return fileItemSourceArray; }
			set
			{
				fileItemSourceArray = value;
				OnPropertyChanged();
			}
		}

		private byte[] fileThumbnailItemSourceArray;
		public byte[] FileThumbnailItemSourceArray
		{
			get { return fileThumbnailItemSourceArray; }
			set
			{
				fileThumbnailItemSourceArray = value;
				OnPropertyChanged();
			}
		}

		public ChatItemAttachmentViewModel()
		{
			SendPostCommand = new Command(UploadPostAttachment);
		}

		private async void UploadPostAttachment()
		{
			CanSend = false;
			IsBusy = true;
			try
			{
				PostRepository postRepository = new PostRepository();
				ParentPost.CreatorFullName = Settings.CurrentUserFullName;
				ParentPost.UserProfileImageName = Settings.CurrentUserProfileImageString;
				//ParentPost.CircleName = circleName;
				ParentPost.MainAttachment = new PostAttachment() {Type = -1};
				ParentPost.CanShowFormattedString = false;

				bool isSaveOk = false;
				string filePath = null;
				string fileName = null;
				string fileThumbnailUrl = null;
				int fileDuration = 0;
				AttachmentFile file = null;

				EditingItem.IsMainAttachment = true;
				EditingItem.FileDuration = EditingItem.FileDuration.HasValue
					? TimeSpan.FromSeconds(EditingItem.FileDuration.Value).TotalMilliseconds
					: (double?) null;


				if (IsFile)
				{
					file = new AttachmentFile()
					{
						FileName = FileName,
						FileData = FileItemSourceArray,
						AttachmentType = EditingItem.Type ?? 0,
						ThumbnailFileData = FileThumbnailItemSourceArray,
					};
				}
				var dataToSend = new PostAndPostAttachment(){
					Post=ParentPost, 
					PostAttachment = EditingItem,
					IsFile = IsFile,
					AttachmentFile = file
				};

				var fileResult =  await postRepository.SubmitPostAndAttachment(dataToSend);

				if(fileResult == null)
				{
					await Navigation.PushPopupAsync(new CustomAlertDialog("Error", "Error", "Ok"));
				}
				else{
					if (fileResult.IsOk)
					{
						ParentPost.Id = fileResult.PostId.Value;
						EditingItem.FileUrl = fileResult.FileUrl;
						isSaveOk = true;
						filePath = fileResult.FileUrl;
						fileName = fileResult.FileName;
						fileThumbnailUrl = fileResult.ThumbnailUrl;

						EditingItem.PostId = fileResult.PostId.Value;
						EditingItem.FileUrl = filePath;
						EditingItem.FilePath = fileName;
						EditingItem.FileThumbnailUrl = fileThumbnailUrl;

						CanReply = true;

						ParentPost.MainAttachment = EditingItem;
						ParentPost.LastAttachmentType = EditingItem.Type;
						ParentPost.LastAttachmentId = fileResult.Id;
						ParentPost.LastUpdatedDate = DateTime.Now;
						ParentPost.LastUpdatedUserFullName = Settings.CurrentUserFullName;

						var pages = App.NavigationPage.Navigation.NavigationStack;
						var page =
							pages.FirstOrDefault(
								p =>
								p.ClassId == "MindCorners.Pages.PromptItem" ||
								p.ClassId == "MindCorners.Pages.ReplyItem");
						if (page != null)
						{
							App.NavigationPage.Navigation.RemovePage(page);
						}

						var chatItemPage = pages.FirstOrDefault(p => p.ClassId == "MindCorners.Pages.ChatItem");

						if (chatItemPage != null)
						{
							var context = (ChatItemViewModel) chatItemPage.BindingContext;
							context.CanReply = true;
							context.IsNew = false;
							context.ShowTextTempate = false;
							await context.LoadChatPosts();
						}

						var cameraPage =
							pages.FirstOrDefault(p => p.ClassId == "MindCorners.CustomControls.CustomCamera");
						if (cameraPage != null)
						{
							App.NavigationPage.Navigation.RemovePage(cameraPage);
						}
						var cameraVideoPage =
							pages.FirstOrDefault(p => p.ClassId == "MindCorners.CustomControls.CustomVideoCamera");
						if (cameraVideoPage != null)
						{
							App.NavigationPage.Navigation.RemovePage(cameraVideoPage);
						}
						await App.NavigationPage.PopAsync(true);
					}
					else
					{
						await Navigation.PushPopupAsync(new CustomAlertDialog("Error", fileResult.ErrorMessage, "Ok"));
					}
					
				}
					//var fileResult = await postAttachmentRepository.UploadFile();




			}
			catch (Exception e)
			{
				await Navigation.PushPopupAsync(new CustomAlertDialog("Error", e.InnerException?.ToString(), "Ok"));
			}
			finally
			{
				CanSend = true;
				IsBusy = false;
			}
		}

		protected string GetFileNameFromFilePath(string filePath)
		{
			var filePaths = filePath.Split('/');
			return filePaths.Last();
		}
	}
}
