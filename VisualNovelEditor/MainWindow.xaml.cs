using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace VisualNovelEditor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public Logger logger;
    public MainWindow()
    {
        InitializeComponent();
        logger = Logger.getInstance();
        logger.projectLogger.GetProjectFilepaths();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Scene scene = new Scene();
        scene.Show();
        this.Close();
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        CenterWindowOnScreen();

        foreach (string filepath in logger.projectLogger.ProjectFilepaths)
        {
            Border border = new Border
        {
            Margin = new Thickness(12, 12, 12, 0),
            CornerRadius = new CornerRadius(6),
            Height = 80,
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A1A"))
        };


        Button button = new Button
        {
            Background = Brushes.Transparent,
            BorderThickness = new Thickness(0),
            Width = Double.NaN, // Auto
            Height = Double.NaN  // Auto
        };


        StackPanel stack = new StackPanel
        {
            Width = 564,
            Orientation = Orientation.Vertical
        };


        TextBlock title = new TextBlock
        {
            Text = Path.GetFileNameWithoutExtension(filepath),
            FontSize = 20,
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CE7D63")),
            Margin = new Thickness(10, 0, 0, 5),
            FontWeight = FontWeights.Medium,
            FontFamily = new FontFamily("pack://application:,,,/fonts/windNewProject/#Roboto Mono")
        };


        TextBlock subtitle = new TextBlock
        {
            Text = "Last edited: " + File.GetLastWriteTime(filepath),
            FontSize = 10,
            Foreground = Brushes.White,
            Margin = new Thickness(10, 0, 0, 0),
            FontWeight = FontWeights.Medium,
            FontFamily = new FontFamily("pack://application:,,,/fonts/windNewProject/#Roboto Mono")
        };


        TextBlock date = new TextBlock
        {
            Text = filepath,
            FontSize = 10,
            Foreground = Brushes.DimGray,
            Margin = new Thickness(10, 0, 0, 0),
            FontWeight = FontWeights.Medium,
            FontFamily = new FontFamily("pack://application:,,,/fonts/windNewProject/#Roboto Mono")
        };


        stack.Children.Add(title);
        stack.Children.Add(subtitle);
        stack.Children.Add(date);
        button.Content = stack;
        border.Child = button;


        StckPnl_ProjectsList.Children.Add(border);
        
        button.Click += ButtonProject_OnClick;
        }
    }

    private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        try //перетягування вікна
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        catch (Exception ex)
        {
            
        }
    }
    
    private void CenterWindowOnScreen()
    {
        var screenWidth = SystemParameters.PrimaryScreenWidth;
        var screenHeight = SystemParameters.PrimaryScreenHeight;
        
        this.Left = (screenWidth - this.Width) / 2;
        this.Top = (screenHeight - this.Height) / 2;
    }

    private void BtnClose_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void BtnOpen_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (*.txt)|*.txt";
            if (ofd.ShowDialog() == true)
            {
                Scene scene = new Scene();
                scene.OpenAs(ofd.FileName);
                logger.projectLogger.AddProjectPath(ofd.FileName);
                scene.Show();
                this.Close();
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Введіть правильний шлях до проекту!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
    
    private void ButtonProject_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Content is StackPanel stackPanel)
        {
            foreach (var child in stackPanel.Children)
            {
                if (child is TextBlock tb && tb.Foreground == Brushes.DimGray)
                {
                    string filepath = tb.Text;

                    if (File.Exists(filepath))
                    {
                        Scene scene = new Scene();
                        scene.Show();
                        scene.OpenAs(filepath);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Файл не знайдений чи видалений зі списку:\n{filepath}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        logger.projectLogger.RemovePath(filepath);
                    }

                    break;
                }
            }
        }
    }
}