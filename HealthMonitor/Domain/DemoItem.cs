using System;
using System.Windows;

namespace HealthMonitor.Domain
{
    public class DemoItem : ViewModelBase
    {
        private readonly Type _contentType;
        private readonly object _dataContext;

        private object _content;
        public DemoItem(string icon,string shortcout,string name, Type contentType, object dataContext = null)
        {
            Icon = icon;
            ShortCut = shortcout;
            Name = name;
            _contentType = contentType;
            _dataContext = dataContext; 
        }

        public string Name { get; }
        public string ShortCut { get; }
        public string Icon { get; }


        public object Content => _content == null ? CreateContent() : null;


        private object CreateContent()
        {
            var content = Activator.CreateInstance(_contentType);
            if (_dataContext != null && content is FrameworkElement element)
            {
                element.DataContext = _dataContext;
            }

            return content;
        }
    }
}
