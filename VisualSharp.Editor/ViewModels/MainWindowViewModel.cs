using System;
using System.Collections.ObjectModel;

namespace VisualSharp.Editor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<ProjectViewModel> HistoryProject { get; } = [];

    public MainWindowViewModel()
    {
        HistoryProject.Add(new ProjectViewModel(){Name = "测试项目1",Path = AppContext.BaseDirectory,SavedVersion = new Version(1,2,3,12)});
        HistoryProject.Add(new ProjectViewModel(){Name = "测试项目2",Path = AppContext.BaseDirectory,SavedVersion = new Version(1,2,3,12)});
        HistoryProject.Add(new ProjectViewModel(){Name = "测试项目3",Path = AppContext.BaseDirectory,SavedVersion = new Version(1,2,3,12)});
        HistoryProject.Add(new ProjectViewModel(){Name = "测试项目4",Path = AppContext.BaseDirectory,SavedVersion = new Version(1,2,3,12)});
        HistoryProject.Add(new ProjectViewModel(){Name = "测试项目5",Path = AppContext.BaseDirectory,SavedVersion = new Version(1,2,3,12)});
    }

}
