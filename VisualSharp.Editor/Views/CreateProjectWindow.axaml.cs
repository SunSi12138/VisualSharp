using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Threading.Tasks;
using VisualSharp.Editor.ViewModels;

namespace VisualSharp.Editor.Views;

public partial class CreateProjectWindow : ReactiveWindow<CreateProjectViewModel>
{
    public CreateProjectWindow()
    {
        DataContext = new CreateProjectViewModel();
        InitializeComponent();
        this.WhenActivated(action =>
        {
            Console.WriteLine(ViewModel is null);
            ViewModel?.SelectFolderCommand.RegisterHandler(SelectFolderAsync);
        });
    }

    private async Task SelectFolderAsync(InteractionContext<string,string> interaction)
    {
        var path = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = interaction.Input, AllowMultiple = false
        });

        interaction.SetOutput(path[0].Path.AbsolutePath);
    }

}

