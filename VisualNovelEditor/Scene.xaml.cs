using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VisualNovelEditor;

public partial class Scene : Window
{
    ScenesContainer scenesContainer;
    Canvas currentCanvas;
    PropertyDisplayer propertyDisplayer;
    DisplaySceneComponentCommand displaySceneComponentCommand;
    DisplayBackgroundCommand displayBackgroundCommand;
    DisplayDialogBoxCommand displayDialogBoxCommand;
    DisplayCharacterCommand displayCharacterCommand;
    Invoker invoker;

    public Scene()
    {
        InitializeComponent();
        scenesContainer = new ScenesContainer();
        propertyDisplayer = new PropertyDisplayer();
        displaySceneComponentCommand = new DisplaySceneComponentCommand();
        displayBackgroundCommand = new DisplayBackgroundCommand();
        displayDialogBoxCommand = new DisplayDialogBoxCommand();
        displayCharacterCommand = new DisplayCharacterCommand();
        invoker = new Invoker(stkpnlProperties);
        
        PropertyDisplayer.stkpnlProperties = stkpnlProperties;
        PropertyDisplayer.wrpnlProperLists = wrpnlProperLists;
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

        while (stkpnlProperties.Children.Count > 1)
        {
            stkpnlProperties.Children.RemoveAt(stkpnlProperties.Children.Count - 1);
        }
        
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
        while (stkpnlProperties.Children.Count > 1)
        {
            stkpnlProperties.Children.RemoveAt(stkpnlProperties.Children.Count - 1);
        }

        if (lbSceneComp.SelectedIndex != -1)
        {
            switch (((SceneComponent)scenesContainer.scenes[lbScenes.SelectedIndex]).components[
                        lbSceneComp.SelectedIndex])
            {
                case VisualNovelEditor.Background background:
                {
                    displayBackgroundCommand.set(propertyDisplayer,
                        (Background)((SceneComponent)scenesContainer.scenes[lbScenes.SelectedIndex]).components[
                            lbSceneComp.SelectedIndex]);
                    invoker.SetCommand(displayBackgroundCommand);
                } break;
                case VisualNovelEditor.DialogBox dialogBox:
                {
                    displayDialogBoxCommand.set(propertyDisplayer,
                        (DialogBox)((SceneComponent)scenesContainer.scenes[lbScenes.SelectedIndex]).components[
                            lbSceneComp.SelectedIndex]);
                    invoker.SetCommand(displayDialogBoxCommand);
                } break;
                case VisualNovelEditor.Character character:
                {
                    displayCharacterCommand.set(propertyDisplayer,
                        (Character)((SceneComponent)scenesContainer.scenes[lbScenes.SelectedIndex]).components[
                            lbSceneComp.SelectedIndex]);
                    invoker.SetCommand(displayCharacterCommand);
                } break;
            }
        }
        invoker.ExecuteCommand();
        
        InitializeTextBoxes();
        
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


    private void BtnNewDialogBox_OnClick(object sender, RoutedEventArgs e)
    {
        DialogBox dialogBox = new DialogBox()
        {
            Name = "DialogBox" + (((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components.Count + 1)
        };

        ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components.Add(dialogBox);
        
        //currentCanvas.Children.Add(newCharacter);
        
        refreshLbSceneComp();
    }
    public void tbProper_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            // Получаем ссылку на TextBox, который вызвал событие
            TextBox textBox = sender as TextBox;

            // Здесь можно выполнять логику обновления свойств
            if (textBox != null)
            {
                // Предположим, что мы хотим обновить какое-то свойство компонента
                int sceneIndex = lbScenes.SelectedIndex;
                int componentIndex = lbSceneComp.SelectedIndex;

                if (sceneIndex >= 0 && componentIndex >= 0)
                {
                    string propertyName = textBox.Name.Replace("tbProper", ""); // Имя свойства из имени TextBox
                    string value = textBox.Text; // Получаем значение из TextBox

                    // Вызов метода Edit для обновления свойств
                    invoker.Edit(sceneIndex, componentIndex, propertyName, value);
                    refreshLbSceneComp();
                }
            }
        }
    }
    private void InitializeTextBoxes()
    {
        foreach (var child in stkpnlProperties.Children)
        {
            if (child is StackPanel stackPanel)
            {
                foreach (var innerChild in stackPanel.Children)
                {
                    if (innerChild is TextBox textBox)
                    {
                        // Проверяем, установлен ли обработчик
                        if (!IsHandlerSet(textBox, nameof(textBox.KeyDown)))
                        {
                            textBox.KeyDown += tbProper_KeyDown;
                        }
                    }
                }
            }
        }
    }

    private bool IsHandlerSet(TextBox textBox, string eventName)
    {
        // Используем рефлексию, чтобы получить информацию о событиях
        var eventField = typeof(TextBox).GetEvent(eventName);
        if (eventField != null)
        {
            var fieldInfo = typeof(TextBox).GetField(eventName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fieldInfo != null)
            {
                var invocationList = (MulticastDelegate)fieldInfo.GetValue(textBox);
                return invocationList != null && invocationList.GetInvocationList().Contains((EventHandler<KeyEventArgs>)tbProper_KeyDown);
            }
        }
        return false;
    }

    private void TbProperName_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        
    }
}

//Binding для lbSceneComp
//