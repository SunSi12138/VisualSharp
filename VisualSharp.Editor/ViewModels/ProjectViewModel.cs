using ReactiveUI;
using System;

namespace VisualSharp.Editor.ViewModels;

public class ProjectViewModel:ViewModelBase
{
    private string _name;
    private string _path;
    private Version _savedVersion;
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public string Path
    {
        get => _path;
        set => this.RaiseAndSetIfChanged(ref _path, value);
    }

    public Version SavedVersion
    {
        get => _savedVersion;
        set => this.RaiseAndSetIfChanged(ref _savedVersion, value);
    }
}
