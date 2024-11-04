using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace VisualNovelEditor;

public class BaseComponent
{
    public string Name { get; set; } = "";

    public virtual void getInfo()
    {
        Console.WriteLine($"Component: {Name}");
    }
}

public class SceneComponent : BaseComponent
{
    public List<SceneComponent> components;
    public Canvas canvas;
    //public Commands commands;


    public SceneComponent()
    {
        components = new List<SceneComponent>();
        canvas = new Canvas();
        canvas.Height = Double.NaN;
        canvas.Width = Double.NaN;
        canvas.Background = Brushes.Transparent;
    }

    public void setCanvasSize(int width, int height)
    {
        this.canvas.Width = width;
        this.canvas.Height = height;
    }

    public virtual void addComponent(SceneComponent component)
    {
        component.canvas = this.canvas;
        components.Add(component);
    }

    public virtual void removeComponent(int index)
    {
        components.RemoveAt(index);
    }

    public void createCanvas()
    {
    }
}

public class Character : SceneComponent
{
    public string Caption;
    public List<string> ImagesPath;
    public List<Dialog> Dialogs;
    public DialogBox DialogBox;

    public WrapPanel wrapPanel;
    public ListBox lbDialogs;

    public int Height; // видалити
    public int Width; // видалити
    public int X; // видалити
    public int Y; // видалити

    public int Position; // редагування зробити, дописати save

    public int currentImageIndex; 
    public int currentDialogIndex; 

    //public int itterator;

    public Character()
    {
        ImagesPath = new List<string>();
        Dialogs = new List<Dialog>();
        DialogBox = new DialogBox();
        Caption = "";
        Height = 100;
        Width = 100;
        X = 0;
        Y = 0;
        currentImageIndex = -1;
        currentDialogIndex = -1;
        Position = -1;
        //itterator = 0;

        wrapPanel = new WrapPanel()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Orientation = Orientation.Horizontal,
            Name = "wrpnlProperLists"
        };

        lbDialogs = new ListBox()
        {
            Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#2B2B2B"),
            BorderThickness = new Thickness(0),
            Name = "lbDialogs",
            Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF"),
            Width = 200,
            Height = 213,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(5)
            //ItemsSource = Dialogs//????
        };

        lbDialogs.MouseDoubleClick += lbDialogs_OnMouseDoubleClick; ////////////////////////
    }

    private void lbDialogs_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListBox listBox && listBox.SelectedIndex >= 0)
        {
            SupportViewPort.getInstance().ClearCurrentDialog(SupportViewPort.getInstance().lbScenes.SelectedIndex);
            currentDialogIndex = listBox.SelectedIndex;
            SupportViewPort.getInstance().Refresh();
        }
    }

    public void NewWrapBtn(string imagePath)
    {
        Button btnWrapImage = new Button()
        {
            Width = 200,
            Height = 250
        };
        //btnWrapImage.Tag = itterator;
        Image image = new Image()
        {
            Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute)),
            Width = Double.NaN,
            Height = Double.NaN,
            Stretch = Stretch.Uniform,
            VerticalAlignment = VerticalAlignment.Bottom
        };

        void btn_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button btn && wrapPanel != null)
            {
                ImagesPath.RemoveAt(wrapPanel.Children.IndexOf(btn));
                wrapPanel.Children.Remove(btn);
            }

            currentImageIndex = -1;
        }

        void btn_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button btn && wrapPanel != null)
            {
                int index = wrapPanel.Children.IndexOf(btn);

                windSelectCharacterImagePosition windSelectCharacterImagePosition =
                    new windSelectCharacterImagePosition();

                windSelectCharacterImagePosition.ShowDialog();

                if (windSelectCharacterImagePosition.DialogResult == true)
                {
                    SupportViewPort.getInstance().ClearCurrentImage(SupportViewPort.getInstance().lbScenes.SelectedIndex, windSelectCharacterImagePosition.position);
                    currentImageIndex = wrapPanel.Children.IndexOf(btn);
                    Position = windSelectCharacterImagePosition.position;
                    SupportViewPort.getInstance().Refresh();
                }

                windSelectCharacterImagePosition.Close();
            }
        }

        // void btn_OnClick(object sender, RoutedEventArgs e)
        // {
        //     if (sender is Button btn && wrapPanel != null)
        //     {
        //         currentImageIndex = wrapPanel.Children.IndexOf(btn);
        //     }
        // }

        btnWrapImage.PreviewMouseDoubleClick += btn_OnPreviewMouseDoubleClick;
        //btnWrapImage.Click += btn_OnClick;
        btnWrapImage.MouseRightButtonDown += btn_OnPreviewMouseRightButtonDown;

        btnWrapImage.Content = image;
        //itterator++;
        wrapPanel.Children.Add(btnWrapImage);
    }

    public void refreshListBox()
    {
        lbDialogs.Items.Clear();

        foreach (Dialog dialog in Dialogs)
        {
            lbDialogs.Items.Add(dialog.Caption);
        }
    }

    public void addNewDialog()
    {
        Dialog newDialog = new Dialog();

        Dialogs.Add(newDialog);
        lbDialogs.Items.Add(newDialog.Caption);
    }

    public void deleteSelectedDialog(int DialogSelectedIndex)
    {
        Dialogs.RemoveAt(DialogSelectedIndex);
        refreshListBox();
    }

    public virtual void addImage(string imagepath)
    {
        ImagesPath.Add(imagepath);
    }

    public virtual void removeImage(string imagepath)
    {
        ImagesPath.Remove(imagepath);
    }

    public void move(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}

public class Dialog
{
    public string Caption;
    public string Text;
    public Color FontColor; //не зберігається
    public float dialogRenderSpeed; //не зберігається
    public int fontSize; //не зберігається

    public bool ViewPortVisible;

    public Dialog()
    {
        Caption = "Dialog";
        Text = "";
    }

    public void setFontSize(int size)
    {
        this.fontSize = size;
    }

    public void setFontColor(Color newColor)
    {
        this.FontColor = newColor;
    }

    public void renderText()
    {
    }
}

public class DialogBox : SceneComponent
{
    public Color BackgroundColor;
    public string ImagePath;
    public int Height;
    public bool Visible;
    public float Opacity;

    public DialogBox()
    {
        BackgroundColor = System.Drawing.Color.Black;
        ImagePath = "";
        Height = 300;
        Visible = true;
        Opacity = 0.5F;
    }

    public void changeBackgroundColor(Color newColor)
    {
        this.BackgroundColor = newColor;
    }

    public void changeImage(string imagePath)
    {
        this.ImagePath = imagePath;
    }

    public void changeHeight(int h)
    {
        this.Height = h;
    }

    public void VisibleTrue()
    {
        this.Visible = true;
    }

    public void VisibleFalse()
    {
        this.Visible = false;
    }
}

public class Background : SceneComponent
{
    public string ImagePath;

    public bool currentBackground;

    public Background()
    {
        ImagePath = "";
        currentBackground = false;
    }

    public void changeImage(string imagePath)
    {
        this.ImagePath = imagePath;
    }
}

// public class Music : SceneComponent
// {
//     public string audioFilePath;
//     public float volume;
//
//     public void play()
//     {
//         
//     }
//
//     public void pause()
//     {
//         
//     }
//
//     public void stop()
//     {
//         
//     }
//
//     public void setVolume(float volume)
//     {
//         this.volume = volume;
//     }
// }