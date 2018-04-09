using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
    public class InfiniteListView : ListView
    {
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create(nameof(LoadMoreCommand), typeof(ICommand), typeof(InfiniteListView), default(ICommand));
        //BindableProperty.Create<InfiniteListView, ICommand>(bp => bp.LoadMoreCommand, default(ICommand));

        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        public static readonly BindableProperty CanLoadMoreProperty = BindableProperty.Create(nameof(LoadMoreCommand), typeof(bool), typeof(InfiniteListView), true);
        //BindableProperty.Create<InfiniteListView, ICommand>(bp => bp.LoadMoreCommand, default(ICommand));

        public bool CanLoadMore
        {
            get { return (bool)GetValue(CanLoadMoreProperty); }
            set { SetValue(CanLoadMoreProperty, value); }
        }


        public InfiniteListView()
        {
            if (CanLoadMore)
            {
                ItemAppearing += InfiniteListView_ItemAppearing;
            }
        }

        void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = ItemsSource as IList;

            if (items != null && items.Count >= 5 && e.Item == items[items.Count - 5])
            {
                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                    LoadMoreCommand.Execute(null);
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == CanLoadMoreProperty.PropertyName)
            {
                if (!CanLoadMore)
                {
                    ItemAppearing -= InfiniteListView_ItemAppearing;
                }
            }
            base.OnPropertyChanged(propertyName);
        }
    }
}
