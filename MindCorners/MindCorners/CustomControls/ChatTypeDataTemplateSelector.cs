using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.CustomControls.ChatItemTemplates;
using MindCorners.CustomControls.ChatMainAttachment;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.ViewModels;
using Xamarin.Forms;
using TextTemplate = MindCorners.CustomControls.ChatItemTemplates.TextTemplate;

namespace MindCorners.CustomControls
{
    public class ChatTypeDataTemplateSelector : Xamarin.Forms.DataTemplateSelector
    {
        public ChatTypeDataTemplateSelector()
        {
            // Retain instances!
            textTemplate = new DataTemplate(typeof(TextTemplate));
            imageTemplate = new DataTemplate(typeof(ImageTemplate));
            audioTemplate = new DataTemplate(typeof(AudioTemplate));
            videoTemplate = new DataTemplate(typeof(VideoTemplate));
            tellMeMoreTemplate = new DataTemplate(typeof(TellMeMoreTemplate));
            textMainAttachmentTemplate = new DataTemplate(typeof(TextMainAttachmentTemplate));
            imageMainAttachmentTemplate = new DataTemplate(typeof(ImageMainAttachmentTemplate));
            audioMainAttachmentTemplate = new DataTemplate(typeof(AudioMainAttachmentTemplate));
            videoMainAttachmentTemplate = new DataTemplate(typeof(VideoMainAttachmentTemplate));

        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var vm = item as Post;
            if (vm == null || vm.MainAttachment == null || !vm.MainAttachment.Type.HasValue)
                return null;

            if (vm.Type == (int)PostTypes.Prompt)
            {
                var mainPostType = int.Parse(vm.MainAttachment.Type.ToString());
                switch (mainPostType)
                {
                    case (int)ChatType.Text:
                        return textMainAttachmentTemplate;
                    case (int)ChatType.Image:
                        return imageMainAttachmentTemplate;
                    case (int)ChatType.Audio:
                        return audioMainAttachmentTemplate;
                    case (int)ChatType.Video:
                        return videoMainAttachmentTemplate;
                }
            }

            if (vm.Type == (int)PostTypes.TellMeMore)
            {
                return tellMeMoreTemplate;
            }
            var chatType = int.Parse(vm.MainAttachment.Type.ToString());
            switch (chatType)
            {
                case (int)ChatType.Text:
                    return textTemplate;
                case (int)ChatType.Image:
                    return imageTemplate;
                case (int)ChatType.Audio:
                    return audioTemplate;
                case (int)ChatType.Video:
                    return videoTemplate;
            }
            //return messageVm.IsIncoming ? this.incomingDataTemplate : this.outgoingDataTemplate;
            return null;
        }

        private readonly DataTemplate textTemplate;
        private readonly DataTemplate imageTemplate;
        private readonly DataTemplate audioTemplate;
        private readonly DataTemplate videoTemplate;
        private readonly DataTemplate tellMeMoreTemplate;

        private readonly DataTemplate textMainAttachmentTemplate;
        private readonly DataTemplate imageMainAttachmentTemplate;
        private readonly DataTemplate audioMainAttachmentTemplate;
        private readonly DataTemplate videoMainAttachmentTemplate;
        //private readonly DataTemplate outgoingDataTemplate;
    }
}
