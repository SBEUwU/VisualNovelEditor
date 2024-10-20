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
        CreateStringProperty("ImagePath", "Image Path", background.ImagePath);
    }

    public void DisplayDialogBoxProperties(DialogBox dialogBox)
    {
        // Property - Name
        checkNameProperty(dialogBox.Name);

        // Property - ImagePath
        CreateStringProperty("Imagepath", "Image Path", dialogBox.ImagePath);

        // Property - Height
        CreateStringProperty("Height", "Height", dialogBox.Height.ToString());

        // Property - Visible
        CreateStringProperty("Visible", "Visible", dialogBox.Visible.ToString());

        // Property - Opacity
        CreateStringProperty("Opacity", "Opacity", dialogBox.Opacity.ToString("F2"));

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
        if(propertyName == "Name") component.Name = value;
        switch (component)
        {
            case Character character:
            {
                switch (propertyName)
                {
                    case "Caption": character.Caption = value; break;
                    case "Height": try { character.Height = Convert.ToInt32(value); } catch (Exception e) {} break;
                    case "Width": try { character.Width = Convert.ToInt32(value); } catch (Exception e) {} break;
                    case "X": try { character.X = Convert.ToInt32(value); } catch (Exception e) {} break;
                    case "Y": try { character.Y = Convert.ToInt32(value); } catch (Exception e) {} break;
                }
                break;
            }
            case DialogBox dialogBox:
            {
                switch (propertyName)
                {
                    case "ImagePath": dialogBox.ImagePath = value; break;
                    case "Height": try { dialogBox.Height = Convert.ToInt32(value); } catch (Exception e) {} break;
                    case "Visible": try { dialogBox.Visible = Convert.ToBoolean(value); } catch (Exception e) {} break;
                    case "Opacity": try { dialogBox.Opacity = float.Parse(value); } catch (Exception e) {} break;
                    case "BackgroundColor": try { dialogBox.BackgroundColor = (System.Drawing.Color)ColorConverter.ConvertFromString(value); } catch (Exception e) {} break;
                }
                break;
            }
            case Background background:
            {
                switch (propertyName)
                {
                    case "ImagePath": background.ImagePath = value; break;
                }
                break;
            }
        }
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