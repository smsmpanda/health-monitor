namespace HealthMonitor.Domain
{
    public class DemoItem : ViewModelBase
    {
        public DemoItem(string viewName, string icon, string shortcout, string name)
        {
            Icon = icon;
            ShortCut = shortcout;
            Name = name;
            ViewName = viewName;
        }

        public string Name { get; }
        public string ShortCut { get; }
        public string Icon { get; }
        public string ViewName { get; }
    }
}
