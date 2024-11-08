using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.VisualBasic.CompilerServices;

namespace VisualNovelEditor;

public partial class windEditCommandsTimeLine : Window
{
    TimeLine timeLine;
    public int SceneIndex;
    public int CompIndex;
    public int SelectIndex;
    
    List<BaseComponent> filteredComponents = new List<BaseComponent>();
    public windEditCommandsTimeLine()
    {
        InitializeComponent();
        CompIndex = -1;
        SelectIndex = -1;
        //RefreshLbReadyCommands();
        timeLine = TimeLine.getInstance();
    }

    private void BtnClose_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
    
    public void RefreshLbReadyCommands()
    {
        lbReadyCommands.Items.Clear();
        
        foreach (TimeLineCommand cmd in ((SceneComponent)TimeLine.scenesContainer.getScene(SceneIndex)).cmds)
        {
            lbReadyCommands.Items.Add(cmd.NameCommand);
        }
    }
    
    private void ShowComponents(int index)
    {
        brdrSelect.Child = null;
        SelectIndex = -1;
        CompIndex = -1;
        
        if (index == 0)
        {
            filteredComponents = ((SceneComponent)TimeLine.scenesContainer.getScene(SceneIndex)).components
                .OfType<Character>()
                .Cast<BaseComponent>()
                .ToList();
        }
        else if (index == 1)
        {
            filteredComponents = ((SceneComponent)TimeLine.scenesContainer.getScene(SceneIndex)).components
                .OfType<Background>()
                .Cast<BaseComponent>()
                .ToList();
        }
        
        
        lbComponents.ItemsSource = null;
        lbComponents.ItemsSource = filteredComponents;
        lbComponents.DisplayMemberPath = "Name";
    }


    private void LbCommands_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        lbComponents.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#2B2B2B");
        switch (lbCommands.SelectedIndex)
        {
            case 0:
                ShowComponents(1);
                break;
            case 1:
                ShowComponents(0);
                break;
            case 2:
                ShowComponents(0);
                break;
            case 3:
                ShowComponents(0);
                break;
            case 4:
                brdrSelect.Child = null;
                SelectIndex = -1;
                CompIndex = -1;
                lbComponents.ItemsSource = null;
                lbComponents.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#191919");
                SelectWaitClick();
                break;
        }
    }

    private void LbComponents_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectIndex = -1;
        if (lbComponents.SelectedIndex != -1)
        {
            CompIndex = getCompIndex();

            switch (lbCommands.SelectedIndex)
            {
                case 0:
                    SelectBackground((Background)((SceneComponent)TimeLine.scenesContainer.getScene(SceneIndex))
                        .components[CompIndex]);
                    break;
                case 1:
                    SelectImageBorder((Character)((SceneComponent)TimeLine.scenesContainer.getScene(SceneIndex))
                        .components[CompIndex]);
                    break;
                case 2:
                    SelectPositionBorder((Character)((SceneComponent)TimeLine.scenesContainer.getScene(SceneIndex))
                        .components[CompIndex]);
                    break;
                case 3:
                    SelectDialog((Character)((SceneComponent)TimeLine.scenesContainer.getScene(SceneIndex))
                        .components[CompIndex]);
                    break;

            }
        }
    }

    public int getCompIndex()
    {
        int compIndex = -1;
        if (lbComponents.SelectedItem is SceneComponent selectedComponent)
        {
            compIndex = ((SceneComponent)TimeLine.scenesContainer.getScene(SceneIndex)).components.IndexOf(selectedComponent);
        }
        return compIndex;
    }
    
    //------------------------------------------------------------------------------
    
    public void SelectImageBorder(Character character)
    {
        ScrollViewer scrllviewSelectImage = new ScrollViewer()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden
        };
        
        WrapPanel wrapPanelSelectImage = new WrapPanel()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Orientation = Orientation.Vertical,
            Name = "wrpnlProperLists"
        };
        
        foreach (string imagePath in character.ImagesPath)
        {
            Button btnWrapImage = new Button()
            {
                Width = 300,
                Height = 350
            };
            
            Image image = new Image()
            {
                Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute)),
                Width = Double.NaN,
                Height = Double.NaN,
                Stretch = Stretch.Uniform,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            
            btnWrapImage.Click += btn_OnClick;
            btnWrapImage.Content = image;
            wrapPanelSelectImage.Children.Add(btnWrapImage);
        }
        scrllviewSelectImage.Content = wrapPanelSelectImage;

        void btn_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn) //&& wrapPanelTimeLine != null
            {
                SelectIndex = wrapPanelSelectImage.Children.IndexOf(btn);
            }
            timeLine.EditCharacterCurrentImage(SceneIndex, CompIndex, SelectIndex);
            RefreshLbReadyCommands();
        }
        
        brdrSelect.Child = null;
        brdrSelect.Child = scrllviewSelectImage;
    }
    
    public void SelectPositionBorder(Character character)
    {
        Grid gridSelectPosition = new Grid()
        {
            Name = "gridSelectPosition",
        };
        gridSelectPosition.RowDefinitions.Add(new RowDefinition { Height = new GridLength(4, GridUnitType.Star) });
        gridSelectPosition.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        
        gridSelectPosition.ColumnDefinitions.Add(new ColumnDefinition());
        gridSelectPosition.ColumnDefinitions.Add(new ColumnDefinition());

        for (int i = 0; i < 2; i++)
        {
            Border brdrPosition = new Border
            {
                CornerRadius = new CornerRadius(4),
                Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x0F, 0x0F, 0x0F)),
                Margin = new Thickness(5, 5, 5, 0),
                Width = double.NaN,
                Height = double.NaN
            };

            Button btnPosition = new Button
            {
                Background = Brushes.Transparent,
                Content = "POSITION " + (i + 1),
                FontSize = 18,
                Name = "btnPos" + (i + 1),
                Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x81, 0x80, 0x7E)),
                BorderThickness = new Thickness(0),
                FontWeight = FontWeights.Medium,
                FontFamily = new FontFamily("pack://application:,,,/fonts/windNewProject/#Roboto Mono"),
                Tag = i
            };
            brdrPosition.Child = btnPosition;
            btnPosition.Click += btn_OnClick;
            gridSelectPosition.Children.Add(brdrPosition);
            
            Grid.SetColumn(brdrPosition, i);
            Grid.SetRow(brdrPosition, 0);
        }
        
        Border brdrClear = new Border
        {
            CornerRadius = new CornerRadius(4),
            Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x0F, 0x0F, 0x0F)),
            Margin = new Thickness(5, 0, 5, 0),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Height = 43
        };
        
        Button btnClear = new Button
        {
            Background = Brushes.Transparent,
            Content = "CLEAR",
            FontSize = 18,
            Name = "btnClear",
            Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x81, 0x80, 0x7E)),
            BorderThickness = new Thickness(0),
            FontWeight = FontWeights.Medium,
            FontFamily = new FontFamily("pack://application:,,,/fonts/windNewProject/#Roboto Mono"),
            Tag = -1,
        };
        
        btnClear.Click += btn_OnClick;
        brdrClear.Child = btnClear;
        gridSelectPosition.Children.Add(brdrClear);
        Grid.SetColumn(brdrClear, 0);
        Grid.SetColumnSpan(brdrClear, 2);
        Grid.SetRow(brdrClear, 1);

        void btn_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn) //&& wrapPanelTimeLine != null
            {
                SelectIndex = Convert.ToInt32(btn.Tag);
            }
            timeLine.EditCharacterPosition(SceneIndex, CompIndex, SelectIndex);
            RefreshLbReadyCommands();
        }
        
        brdrSelect.Child = null;
        brdrSelect.Child = gridSelectPosition;
    }

    public void SelectBackground(Background background)
    {
        Grid gridSelectBackground = new Grid()
        {
            Name = "gridSelectBackground",
        };
        gridSelectBackground.RowDefinitions.Add(new RowDefinition { Height = new GridLength(4, GridUnitType.Star) });
        gridSelectBackground.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        
        Image imgSelectBackground = new Image()
        {
            Source = new BitmapImage(new Uri(background.ImagePath, UriKind.RelativeOrAbsolute)),
            Width = Double.NaN,
            Height = Double.NaN,
            Stretch = Stretch.Uniform,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = new Thickness(0, 5, 0, 0),
        };
        
        Border brdrSetBackground = new Border
        {
            CornerRadius = new CornerRadius(4),
            Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x0F, 0x0F, 0x0F)),
            Margin = new Thickness(5, 0, 5, 0),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Height = 43
        };
        
        Button btnSetBackground = new Button
        {
            Background = Brushes.Transparent,
            Content = "SET BACKGROUND",
            FontSize = 18,
            Name = "btnSetBackground",
            Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x81, 0x80, 0x7E)),
            BorderThickness = new Thickness(0),
            FontWeight = FontWeights.Medium,
            FontFamily = new FontFamily("pack://application:,,,/fonts/windNewProject/#Roboto Mono"),
            Tag = -1,
        };

        btnSetBackground.Click += btn_OnClick;
        
        brdrSetBackground.Child = btnSetBackground;
        gridSelectBackground.Children.Add(imgSelectBackground);
        gridSelectBackground.Children.Add(brdrSetBackground);
        
        Grid.SetRow(imgSelectBackground, 0);
        Grid.SetRow(brdrSetBackground, 1);
        
        void btn_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn) //&& wrapPanelTimeLine != null
            {
                timeLine.EditCurrentBackground(SceneIndex, CompIndex);
                RefreshLbReadyCommands();
            }
        }

        brdrSelect.Child = null;
        brdrSelect.Child = gridSelectBackground;
    }
    
    public void SelectDialog(Character character)
    {
        ScrollViewer scrllviewSelectDialog = new ScrollViewer()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
        };

        StackPanel stkpnlSelectDialog = new StackPanel()
        {
            VerticalAlignment = VerticalAlignment.Top,
            Orientation = Orientation.Vertical,
            Name = "stkpnlSelectDialog"
        };
        
        foreach (Dialog dialog in character.Dialogs)
        {
            Button btnWrapImage = new Button()
            {
                Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x0F, 0x0F, 0x0F)),
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 580,
                Height = 200,
            };

            Grid gridDialog = new Grid();
            
            gridDialog.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            gridDialog.RowDefinitions.Add(new RowDefinition { Height = new GridLength(4, GridUnitType.Star) });

            TextBlock txbDialogCaption = new TextBlock()
            {
                Text = dialog.Caption,
                FontSize = 18,
                FontWeight = FontWeights.Medium,
                FontFamily = new FontFamily("pack://application:,,,/fonts/windNewProject/#Roboto Mono"),
                Name = "txbDialogCaption",
                Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x81, 0x80, 0x7E)),
                TextAlignment = TextAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            
            TextBlock txbDialogText = new TextBlock()
            {
                Text = dialog.Text,
                FontSize = 18,
                FontWeight = FontWeights.Medium,
                FontFamily = new FontFamily("pack://application:,,,/fonts/windNewProject/#Roboto Mono"),
                Name = "txbDialogText",
                Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x81, 0x80, 0x7E)),
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            gridDialog.Children.Add(txbDialogCaption);
            gridDialog.Children.Add(txbDialogText);
            
            Grid.SetRow(txbDialogCaption, 0);
            Grid.SetRow(txbDialogText, 1);
            
            btnWrapImage.Content = gridDialog;
            btnWrapImage.Click += btn_OnClick;
            stkpnlSelectDialog.Children.Add(btnWrapImage);
        }
        
        scrllviewSelectDialog.Content = stkpnlSelectDialog;

        void btn_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn) //&& wrapPanelTimeLine != null
            {
                SelectIndex = stkpnlSelectDialog.Children.IndexOf(btn);
            }
            timeLine.EditCurrentDialog(SceneIndex, CompIndex, SelectIndex);
            RefreshLbReadyCommands();
        }
        
        brdrSelect.Child = null;
        brdrSelect.Child = scrllviewSelectDialog;
    }

    public void SelectWaitClick()
    {
        Border brdrWaitClick = new Border
        {
            CornerRadius = new CornerRadius(4),
            Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x0F, 0x0F, 0x0F)),
            Margin = new Thickness(5, 0, 5, 0),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Width = Double.NaN,
            Height = Double.NaN,
        };
        
        Button btnWaitClick = new Button()
        {
            Background = Brushes.Transparent,
            Content = "ADD",
            FontSize = 32,
            Name = "btnSelectWaitClick",
            Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x81, 0x80, 0x7E)),
            BorderThickness = new Thickness(0),
            FontWeight = FontWeights.Medium,
            FontFamily = new FontFamily("pack://application:,,,/fonts/windNewProject/#Roboto Mono")
        };
        
        btnWaitClick.Click += btn_OnClick;
        brdrWaitClick.Child = btnWaitClick;
        
        void btn_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn) //&& wrapPanelTimeLine != null
            {
                TimeLine.getInstance().WaitClick();
                RefreshLbReadyCommands();
            }
        }
        
        brdrSelect.Child = null;
        brdrSelect.Child = brdrWaitClick;
    }

    private void LbReadyCommands_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        if(lbReadyCommands.SelectedIndex != -1)
            timeLine.DeleteCommand(lbReadyCommands.SelectedIndex);
        RefreshLbReadyCommands();
    }
}