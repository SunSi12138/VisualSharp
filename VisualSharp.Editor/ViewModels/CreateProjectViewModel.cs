using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows.Input;

namespace VisualSharp.Editor.ViewModels;

public class CreateProjectViewModel:ViewModelBase
{
    private string _name = "NewProject";
    private string _path;
    private string _namespace = "GlobalNameSpace";
    private BinaryType _type = BinaryType.Executable;

    public string Name
    {
        get => _name;
        set
        {
            Console.WriteLine(value);
            this.RaiseAndSetIfChanged(ref _name, value);
        }
    }

    public string Path
    {
        get => _path;
        set => this.RaiseAndSetIfChanged(ref _path, value);
    }

    public string NameSpace
    {
        get => _namespace;
        set => this.RaiseAndSetIfChanged(ref _namespace, value);
    }

    public BinaryType Type
    {
        get => _type;
        set => this.RaiseAndSetIfChanged(ref _type, value);
    }

    public ICommand SelectFolder { get; }
    public Interaction<string,string> SelectFolderCommand { get; }

    public CreateProjectViewModel()
    {
        SelectFolderCommand = new();
        SelectFolder = ReactiveCommand.CreateFromTask(async () =>
        {
            Path = await SelectFolderCommand.Handle("选择项目存储位置");
        });
    }

    public bool IsExecutable => Type == BinaryType.Executable;
    public void ToggleBinaryType()
    {
        Type = Type == BinaryType.SharedLibrary ? BinaryType.Executable : BinaryType.SharedLibrary;
    }
}
