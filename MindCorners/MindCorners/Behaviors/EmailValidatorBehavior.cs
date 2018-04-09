using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.Behaviors
{
    public class EmailValidatorBehavior : Behavior<Entry>
    {
        public static BindableProperty IsValidProperty = BindableProperty.Create<EmailValidatorBehavior, bool>(tc => tc.IsValid, false, BindingMode.TwoWay);

        public bool IsValid
        {
            get
            {
                return (bool)GetValue(IsValidProperty);
            }
            set
            {
                SetValue(IsValidProperty, value);
            }
        }


        const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +

            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";



        protected override void OnAttachedTo(Entry bindable)

        {

            bindable.TextChanged += HandleTextChanged;

            base.OnAttachedTo(bindable);

        }


        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
           
            IsValid = (Regex.IsMatch(e.NewTextValue, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));

            ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;

        }


        protected override void OnDetachingFrom(Entry bindable)

        {

            bindable.TextChanged -= HandleTextChanged;

            base.OnDetachingFrom(bindable);

        }

    }

}
