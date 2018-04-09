using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.ViewModels
{
    public class ExternalLoginViewModel : BaseViewModel
    {
        private string externalLoginUrl;

        public string ExternalLoginUrl
        {
            get { return externalLoginUrl; }
            set
            {
                externalLoginUrl = value;
                OnPropertyChanged();
            }
        }
    }
}
