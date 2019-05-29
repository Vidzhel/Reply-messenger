using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UI.UIPresenter.Attachment_Properties
{
    public class MessageListControl : BaseAttachedProperty<MessageListControl, double>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = (d as ScrollViewer);

            if (scrollViewer == null)
                return;

            scrollViewer.Loaded -= Instance_Loaded;

            if((double)e.NewValue == 1)
            scrollViewer.Loaded += Instance_Loaded;
        }

        private void Instance_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as ScrollViewer).ScrollToEnd();
        }
    }
}
