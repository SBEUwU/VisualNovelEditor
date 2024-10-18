using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VisualNovelEditor;

public partial class Scene : Window
{
    ScenesContainer scenesContainer;
    Canvas currentCanvas;
    PropertyDisplayer propertyDisplayer;
    DisplaySceneComponentCommand displaySceneComponentCommand;
    Invoker invoker;

    public Scene()
    {
        InitializeComponent();
        scenesContainer = new ScenesContainer();
        propertyDisplayer = new PropertyDisplayer();
        displaySceneComponentCommand = new DisplaySceneComponentCommand();
        invoker = new Invoker(stkpnlProperties);
        
        PropertyDisplayer.stkpnlProperties = stkpnlProperties;
        Invoker.scenesContainer = scenesContainer;
    }


    private void BtnNewScene_OnClick(object sender, RoutedEventArgs e)
    {
        scenesContainer.addComponent(new SceneComponent());
        lbScenes.Items.Add(scenesContainer.getInfoLast());
        // if (lbScenes.SelectedIndex == -1 && lbScenes.Items.Count > 0)
        // {
        //     lbScenes.SelectedIndex = 0;
        // }
        // if (lbScenes.SelectedIndex != -1)
        // {
        //     currentCanvas = ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).canvas;
        // }
        lbScenes.SelectedIndex = lbScenes.Items.Count - 1;
        
        currentCanvas = ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).canvas;
    }

    private void lbScenes_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        lbSceneComp.Items.Clear();

        refreshLbSceneComp();
        
        cvsScene.Children.Clear();
        currentCanvas = ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).canvas;
        cvsScene.Children.Add(currentCanvas);
        
        
        
        displaySceneComponentCommand.set(propertyDisplayer, (SceneComponent)scenesContainer.scenes[lbScenes.SelectedIndex]);
        
        invoker.SetCommand(displaySceneComponentCommand);
        invoker.ExecuteCommand();
        
        if(currentCanvas != null)
        MessageBox.Show("current Canvas childrens: " + currentCanvas.Children.Count.ToString()
                         + "\ncurrent Scene components: " + ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components.Count.ToString());
    }

    private void btnNewCharacter_OnClick(object sender, RoutedEventArgs e)
    {
        // Button button = new Button
        // {
        //     Content = ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).canvas.Name,
        //     Width = 300,
        //     Height = 300
        // };
        //
        // // Установка позиции кнопки на Canvas
        // Canvas.SetLeft(button, 50); // Отступ слева
        // Canvas.SetTop(button, 50); // Отступ сверху

        Character newCharacter = new Character()
        {
            Name = "Character" + (((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components.Count + 1)
        };

        ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components.Add(newCharacter);
        
        
        // Добавление кнопки на Canvas
        //currentCanvas.Children.Add(newCharacter);

        refreshLbSceneComp();
    }

    private void btnNewBackground_OnClick(object sender, RoutedEventArgs e)
    {
        Background background = new Background()
        {
            Name = "Background" + (((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components.Count + 1)
        };

        ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components.Add(background);
        
        //currentCanvas.Children.Add(newCharacter);
        
        refreshLbSceneComp();
    }

    private void lbSceneComp_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
        
        // if (lbSceneComp.SelectedIndex == -1 && lbSceneComp.Items.Count > 0)
        // {
        //     lbSceneComp.SelectedIndex = 0;
        // }
        //
        // cvsScene.Children.Clear();
        // currentCanvas = ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).canvas;
        // cvsScene.Children.Add(currentCanvas);
        // MessageBox.Show(cvsScene.Children.Count.ToString());
    }

    public void refreshLbSceneComp()
    {
        lbSceneComp.Items.Clear();

        if (lbScenes.SelectedIndex != -1)
        {
            foreach (BaseComponent component in
                     ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components)
            {
                lbSceneComp.Items.Add(component.Name);
            }
        }
    }
    
    
}

//2 listbox для сцен і компонентів сцени
//