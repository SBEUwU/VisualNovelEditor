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

namespace VisualNovelEditor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public Logger logger;
    public CreatePanel createPanel;
    public MainWindow()
    {
        InitializeComponent();
        logger = Logger.getInstance();
        createPanel = new CreatePanel();
    }

    // private void Button1_OnClick(object sender, RoutedEventArgs e)
    // {
    //     switch(((Button)sender).Name)
    //     {
    //         case "button1":
    //             logger.addLog(Commands.ButtonOpen.ToString());
    //             break;
    //         case "button2":
    //             logger.addLog(Commands.ButtonSave.ToString());
    //             break;
    //         case "button3":
    //             logger.addLog(Commands.ButtonExit.ToString());
    //             break;
    //     }
    // }
    // private void BtnSave_OnClick(object sender, RoutedEventArgs e)
    // {
    //     logger.saveLog();
    // }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        //NewProject newProject = new NewProject();
        //newProject.Show();
        //createPanel.create("PROJECT NAME", DateTime.Now,StckPnl_ProjectsList);
        Scene scene = new Scene();
        scene.Show();
        this.Close();
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        CenterWindowOnScreen();
        
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
        Border border = new Border
        {
            Margin = new Thickness(12, 12, 12, 0),
            CornerRadius = new CornerRadius(6),
            Height = 80,
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A1A"))
        };

// Кнопка
        Button button = new Button
        {
            Background = Brushes.Transparent,
            BorderThickness = new Thickness(0),
            Width = Double.NaN, // Auto
            Height = Double.NaN  // Auto
        };

// StackPanel внутри кнопки
        StackPanel stack = new StackPanel
        {
            Width = 564,
            Orientation = Orientation.Vertical
        };

// Первый TextBlock – Название проекта
        TextBlock title = new TextBlock
        {
            Text = "PROJECT 1",
            FontSize = 20,
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CE7D63")),
            Margin = new Thickness(10, 0, 0, 5),
            FontWeight = FontWeights.Medium,
            FontFamily = new FontFamily("pack://application:,,,/fonts/windNewProject/#Roboto Mono")
        };

// Второй TextBlock – подпись
        TextBlock subtitle = new TextBlock
        {
            Text = "Last opened",
            FontSize = 10,
            Foreground = Brushes.White,
            Margin = new Thickness(10, 0, 0, 0),
            FontWeight = FontWeights.Medium,
            FontFamily = new FontFamily("pack://application:,,,/fonts/windNewProject/#Roboto Mono")
        };

// Третий TextBlock – дата
        TextBlock date = new TextBlock
        {
            Text = DateTime.Now.ToLongDateString(),
            FontSize = 10,
            Foreground = Brushes.White,
            Margin = new Thickness(10, 0, 0, 0),
            FontWeight = FontWeights.Medium,
            FontFamily = new FontFamily("pack://application:,,,/fonts/windNewProject/#Roboto Mono")
        };

// Сборка
        stack.Children.Add(title);
        stack.Children.Add(subtitle);
        stack.Children.Add(date);
        button.Content = stack;
        border.Child = button;

// Пример добавления в StackPanel (где ты хочешь показать список проектов)
        StckPnl_ProjectsList.Children.Add(border);
    }
}