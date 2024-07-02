using Avalonia.Controls;
using Avalonia.Interactivity;
using VisualSharp.Editor.ViewModels;

namespace VisualSharp.Editor.Views;

public partial class MainWindow : Window
{

    public MainWindow()
    {
        InitializeComponent();
    }

    public void ShowCreateProject(object source,RoutedEventArgs args)
    {

        var createProject = new CreateProjectWindow();
        createProject.Show(this);
    }
}
