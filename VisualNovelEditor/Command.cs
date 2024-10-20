using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VisualNovelEditor;

// 1. Визначення об'єктів з різними властивостями

// 2. Інтерфейс команди

public interface ICommand
{
    void Execute();
}

// 3. Receiver - Клас, який виконує фактичну логіку

public class PropertyDisplayer
{
    static public StackPanel stkpnlProperties;

    public PropertyDisplayer()
    {
    }

    public void DisplaySceneComponentProperties(SceneComponent scene)
    {
        TextBox tbProperName = Invoker.FindTextBoxInPanel(stkpnlProperties, "Name");
        tbProperName.Text = scene.Name;
    }

    public void DisplayBackgroundProperties(Background background)
    {
        // Property - Name
        checkNameProperty(background.Name);

        // Property - ImagePath
        CreateStringProperty("Imagepath", "Image Path", background.imagePath);
    }

    public void DisplayDialogBoxProperties(DialogBox dialogBox)
    {
        // Property - Name
        checkNameProperty(dialogBox.Name);

        // Property - ImagePath
        CreateStringProperty("Imagepath", "Image Path", dialogBox.imagePath);

        // Property - Height
        CreateStringProperty("Height", "Height", dialogBox.height.ToString());

        // Property - Visible
        CreateStringProperty("Visible", "Visible", dialogBox.visible.ToString());

        // Property - Opacity
        CreateStringProperty("Opacity", "Opacity", dialogBox.opacity.ToString("F2"));

        // Property - BackgroundColor
        CreateStringProperty("Backgroundcolor", "Background Color", dialogBox.BackgroundColor.ToString());
    }

    public void DisplayCharacterProperties(Character character)
    {
        // Property - Name
        checkNameProperty(character.Name);

        // Property - ImagePath
        CreateStringProperty("Caption", "Caption", character.Caption);

        // Property - Height
        CreateStringProperty("Height", "Height", character.Height.ToString());

        // Property - Visible
        CreateStringProperty("Width", "Width", character.Width.ToString());

        // Property - Opacity
        CreateStringProperty("X", "X", character.X.ToString("F2"));

        // Property - BackgroundColor
        CreateStringProperty("Y", "Y", character.Y.ToString());
    }

    private void checkNameProperty(string NameValue)
    {
        TextBox tbName = new TextBox();
        tbName = Invoker.FindTextBoxInPanel(stkpnlProperties, "Name");
        tbName.Text = NameValue;
    }

    private void CreateStringProperty(string PropertyName, string VisualPropertyName, string value)
    {
        StackPanel stkpnl;
        TextBox tb;
        TextBlock tbk;

        stkpnl = new StackPanel()
        {
            Margin = new Thickness(0, 10, 0, 0),
            Name = "stkpnlProper" + PropertyName
        };
        tbk = new TextBlock()
        {
            Text = VisualPropertyName,
            FontSize = 14,
            Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#F3DFD8"),
            Margin = new Thickness(0, 0, 0, 10),
            FontWeight = FontWeights.Medium,
            FontFamily = (FontFamily)Application.Current.Resources["RobotoMono"],
            Name = "lblProper" + PropertyName,
        };
        tb = new TextBox()
        {
            Text = value,
            Width = 312,
            Height = 29,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#0F0F0F"),
            Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF"),
            Name = "tbProper" + PropertyName
        };

        stkpnl.Children.Add(tbk);
        stkpnl.Children.Add(tb);
        stkpnlProperties.Children.Add(stkpnl);
    }
}

// 4. Конкретні команди

public class DisplaySceneComponentCommand : ICommand
{
    private PropertyDisplayer _displayer;
    private SceneComponent _scene;

    public void set(PropertyDisplayer displayer, SceneComponent scene)
    {
        _displayer = displayer;
        _scene = scene;
    }

    public void Execute()
    {
        _displayer.DisplaySceneComponentProperties(_scene);
    }
}

public class DisplayBackgroundCommand : ICommand
{
    private PropertyDisplayer _displayer;
    private Background _background;

    public void set(PropertyDisplayer displayer, Background background)
    {
        _displayer = displayer;
        _background = background;
    }

    public void Execute()
    {
        _displayer.DisplayBackgroundProperties(_background);
    }
}

public class DisplayDialogBoxCommand : ICommand
{
    private PropertyDisplayer _displayer;
    private DialogBox _dialogBox;

    public void set(PropertyDisplayer displayer, DialogBox dialogBox)
    {
        _displayer = displayer;
        _dialogBox = dialogBox;
    }

    public void Execute()
    {
        _displayer.DisplayDialogBoxProperties(_dialogBox);
    }
}

public class DisplayCharacterCommand : ICommand
{
    private PropertyDisplayer _displayer;
    private Character _character;

    public void set(PropertyDisplayer displayer, Character character)
    {
        _displayer = displayer;
        _character = character;
    }

    public void Execute()
    {
        _displayer.DisplayCharacterProperties(_character);
    }
}

// 5. Інвокер

public class Invoker
{
    private ICommand _command;
    static public ScenesContainer scenesContainer;
    private StackPanel _stackPanel;

    public Invoker(StackPanel stkpnlProperties)
    {
        _stackPanel = stkpnlProperties;
    }

    public void Edit(int sceneIndex, int componentIndex, string propertyName, string value)
    {
        BaseComponent component = ((SceneComponent)scenesContainer.scenes[sceneIndex]).components[componentIndex];
        switch (component)
        {
            case Character character:
            {
                ChangeProperty(character, propertyName, value);

                break;
            }
            // case DialogBox dialogBox:
            // {
            //     ChangeProperty(dialogBox, propertyName, value);
            //     break;
            // }
            // case Background background:
            // {
            //     ChangeProperty(background, propertyName, value);
            //     break;
            // }
        }

        void ChangeProperty(BaseComponent component, string propertyName, string value)
        {
            // var property = component.GetType().GetProperty(propertyName, 
            //     BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            // if (property != null && property.CanWrite)
            // {
            //     if (property.PropertyType == typeof(string))
            //     {
            //         property.SetValue(component, value);
            //     }
            //     else if (property.PropertyType == typeof(int) && int.TryParse(value, out int intValue))
            //     {
            //         property.SetValue(component, intValue);
            //     }
            //     else if (property.PropertyType == typeof(float) && float.TryParse(value, out float floatValue))
            //     {
            //         property.SetValue(component, floatValue);
            //     }
            //     else if (property.PropertyType == typeof(bool) && bool.TryParse(value, out bool boolValue))
            //     {
            //         property.SetValue(component, boolValue);
            //     }
            //     else if (property.PropertyType == typeof(Color))
            //     {
            //         ColorConverter colorConverter = new ColorConverter();
            //         //var color = (Color)colorConverter.ConvertFromString(value);
            //         var color = (SolidColorBrush)new BrushConverter().ConvertFromString(value);
            //         property.SetValue(component, color);
            //     }
            // }
            // switch (propertyName)
            // {
            //     case "Name":
            //         if (component is Character character)
            //             character.Name = value;
            //         else if (component is DialogBox dialogBox)
            //             dialogBox.Name = value;
            //         else if (component is Background background)
            //             background.Name = value;
            //         break;
            //     
            //     case "Caption":
            //         if (component is Character character)
            // }
        }

        TextBox tb = FindTextBoxInPanel(_stackPanel, propertyName);
        tb.Text = value;
    }

    public void SetCommand(ICommand command)
    {
        _command = command;
    }

    public void ExecuteCommand()
    {
        if (_command != null)
        {
            _command.Execute();
        }
    }

    public static TextBox FindTextBoxInPanel(StackPanel mainStackPanel, string textBoxName)
    {
        // Проходим по дочерним элементам главного StackPanel
        foreach (var child in mainStackPanel.Children)
        {
            // Если дочерний элемент - StackPanel, проверяем его содержимое
            if (child is StackPanel childStackPanel)
            {
                // Проходим по элементам внутреннего StackPanel
                foreach (var innerChild in childStackPanel.Children)
                {
                    // Если находим TextBox с нужным именем - возвращаем его
                    if (innerChild is TextBox textBox && textBox.Name == $"tbProper{textBoxName}")
                    {
                        return textBox;
                    }
                }
            }
        }

        // Если ничего не нашли - возвращаем null
        return null;
    }

    // public static TextBox FindTextBoxInPanel(StackPanel stackpanel, string textBoxName)
    // {
    //     foreach (var child in stackpanel.Children)
    //     {
    //         if (child is TextBox textBox && textBox.Name == textBoxName)
    //         {
    //             return textBox;
    //         }
    //         else if (child is StackPanel childStackPanel)
    //         {
    //             // Рекурсивний пошук у дочірніх панелях
    //             TextBox result = FindTextBoxInPanel(childStackPanel, textBoxName);
    //             if (result != null)
    //             {
    //                 return result;
    //             }
    //         }
    //     }
    //     return null;
    // }
    // 1. Створення TextBox для вибраного компонента //
    // 2. Edit() - передавати index сцени, як lbScenes.SelectedIndex.
    //    index компонента, як lbSceneComp.SelectedIndex.
    // 3. Додати до створення Textbox обработчик событий textchanged або keydown.
    //    в реалізацію обработчика событий додати edit()
    // 4. При вибору компонента видалити всі stackpanel окрім name, і створювати нові stackpanel, які потрібно для вибраного компонента //
}