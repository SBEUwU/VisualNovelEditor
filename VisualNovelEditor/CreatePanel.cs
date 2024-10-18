using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VisualNovelEditor;

public class CreatePanel
{
    private string fontPath = "pack://application:,,,/fonts/windNewProject/#Roboto Mono";
    public void create(string title, DateTime datatime, StackPanel MainStackPanel)
    { 
        //FontFamily robotoMonoFontFamily = new FontFamily(fontPath);

            Border border = new Border
            {
                Margin = new Thickness(12, 12, 12, 0),
                CornerRadius = new CornerRadius(6),
                Width = double.NaN,
                Height = 80,
                Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#1A1A1A")
            };
        
            Button button = new Button
            {
                Width = double.NaN,
                Height = 80,
                Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent"),
                BorderThickness = new Thickness(0)
            };
            
            StackPanel stackPanel = new StackPanel
            {
                Width = 564,
                Height = double.NaN,
                Orientation = Orientation.Vertical
            };
            
            TextBlock titleTextBlock = new TextBlock
            {
                Text = title,
                FontSize = 20,
                FontWeight = FontWeights.Medium,
                Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#CE7D63"),
                Margin = new Thickness(10, 0, 0, 5),
                FontFamily = (FontFamily)Application.Current.Resources["RobotoMono"]
            };
            
            TextBlock subtitleTextBlock = new TextBlock
            {
                Text = "Last opened",
                FontSize = 10,
                FontWeight = FontWeights.Medium,
                Foreground = Brushes.White,
                Margin = new Thickness(10, 0, 0, 0),
                FontFamily = (FontFamily)Application.Current.Resources["RobotoMono"]
            };
            
            TextBlock dateTextBlock = new TextBlock
            {
                Text = datatime.ToString(),
                FontSize = 10,
                FontWeight = FontWeights.Medium,
                Foreground = Brushes.White,
                Margin = new Thickness(10, 0, 0, 0),
                FontFamily = (FontFamily)Application.Current.Resources["RobotoMono"]
            };
            
            stackPanel.Children.Add(titleTextBlock);
            stackPanel.Children.Add(subtitleTextBlock);
            stackPanel.Children.Add(dateTextBlock);
            
            button.Content = stackPanel;
            
            border.Child = button;
            
            MainStackPanel.Children.Add(border);
    }
}