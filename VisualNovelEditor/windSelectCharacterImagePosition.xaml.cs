using System.Windows;

namespace VisualNovelEditor;

public partial class windSelectCharacterImagePosition : Window
{
    public int position;
    
    public windSelectCharacterImagePosition()
    {
        InitializeComponent();
        position = -1;
    }

    private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void BtnPos2_OnClick(object sender, RoutedEventArgs e)
    {
        position = 1;
        DialogResult = true;
    }

    private void BtnPos1_OnClick(object sender, RoutedEventArgs e)
    {
        position = 0;
        DialogResult = true;
    }

    private void BtnClear_OnClick(object sender, RoutedEventArgs e)
    {
        position = -1;
        DialogResult = true;
    }
}