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

    private void Button1_OnClick(object sender, RoutedEventArgs e)
    {
        switch(((Button)sender).Name)
        {
            case "button1":
                logger.addLog(Commands.ButtonOpen.ToString());
                break;
            case "button2":
                logger.addLog(Commands.ButtonSave.ToString());
                break;
            case "button3":
                logger.addLog(Commands.ButtonExit.ToString());
                break;
        }
    }
    private void BtnSave_OnClick(object sender, RoutedEventArgs e)
    {
        logger.saveLog();
    }

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
        try
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
        // Получаем размеры экрана
        var screenWidth = SystemParameters.PrimaryScreenWidth;
        var screenHeight = SystemParameters.PrimaryScreenHeight;

        // Вычисляем координаты для центрирования
        this.Left = (screenWidth - this.Width) / 2;
        this.Top = (screenHeight - this.Height) / 2;
    }

    private void BtnClose_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}