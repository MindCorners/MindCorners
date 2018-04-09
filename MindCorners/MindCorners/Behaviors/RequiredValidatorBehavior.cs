using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.Behaviors
{
    public class RequiredValidatorBehavior : Behavior<Entry>
    {
        public static BindableProperty IsValidProperty = BindableProperty.Create<RequiredValidatorBehavior, bool>(tc => tc.IsValid, false, BindingMode.TwoWay);
        
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

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }
        
        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = e.NewTextValue.Length >0;
            ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }

    public class RequiredEditorValidatorBehavior : Behavior<Editor>
    {
        public static BindableProperty IsValidProperty = BindableProperty.Create<RequiredValidatorBehavior, bool>(tc => tc.IsValid, false, BindingMode.TwoWay);

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

        protected override void OnAttachedTo(Editor bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = e.NewTextValue.Length > 0;
            ((Editor)sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Editor bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}
