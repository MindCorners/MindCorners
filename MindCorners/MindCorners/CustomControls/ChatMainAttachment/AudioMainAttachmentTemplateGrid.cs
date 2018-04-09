using System;

using Xamarin.Forms;

namespace MindCorners
{
	public class AudioMainAttachmentTemplateGrid : ContentPage
	{
		public AudioMainAttachmentTemplateGrid()
		{
			Content = new StackLayout
			{
				Children = {
					new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}

