using System.Windows;

namespace VisualNovelEditor;

public partial class windDialogEdit : Window
{
    public windDialogEdit()
    {
        InitializeComponent();
    }

    private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void BtnSave_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}