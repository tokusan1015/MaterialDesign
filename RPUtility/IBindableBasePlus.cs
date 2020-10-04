using Prism.Regions;

namespace RPUtility
{
    public interface IBindableBasePlus
    {
        object GetNavigateParam(NavigationContext navigationContext);
        bool HasErrors();
        void InvertEnables(ControlInfoBase ci);
        void InvertEnables(int groupNo);
        void InvertVisible(ControlInfoBase ci);
        void InvertVisible(int groupNo);
        bool IsNavigationTarget(NavigationContext navigationContext);
        void Navigate(string regionName, string target);
        void Navigate(string regionName, string target, NavigationParameters parameters);
        void OnNavigatedFrom(NavigationContext navigationContext);
        void OnNavigatedTo(NavigationContext navigationContext);
        void SetEnables(bool flag, int groupNo = -1);
        void SetEnables(bool flag, ControlInfoBase ci);
        void SetNavigateParam(NavigationContext navigationContext, object value);
        void SetVisibles(bool flag, int groupNo = -1);
        void SetVisibles(bool flag, ControlInfoBase ci);
    }
}