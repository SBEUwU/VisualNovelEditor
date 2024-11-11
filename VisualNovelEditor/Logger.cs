using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace VisualNovelEditor;
public enum Commands
{
    ButtonExit = 1,
    ButtonOpen = 2,
    ButtonSave = 3,
}
public class Logger
{
    private static Logger logger;
    List<string> logList = new();

    private Logger()
    {
        
    }

    public static Logger getInstance()
    {
        if (logger == null)
            logger = new();
        return logger;
    }

    public void addLog(string log)
    {
        logList.Add(log);
    }
    public void saveLog(string folderPath = "")
    {
        string filePath;
        string exePath = AppDomain.CurrentDomain.BaseDirectory;;
        string fileNameBase = "log";
        string fileExtension = ".flog";
        int fileIndex = 1;
        
        if (folderPath == "")
        {
            
            filePath = @$"{exePath}saves\";  
            
        }
        else
        {
            filePath = @$"{folderPath}\";
        }
        while (File.Exists(filePath+$"{fileNameBase}{fileIndex}{fileExtension}"))
        {
            fileIndex++;
        }
        filePath += $"{fileNameBase}{fileIndex}{fileExtension}";
        
        // string filePath = Path.Combine(folderPath, $"{fileNameBase}{fileIndex}{fileExtension}");
        
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
        {
            writer.Write(logList.Count);
            
            foreach (var item in logList)
            {
                writer.Write(item);
            }
        }
    }
    
    public void Txt_Serialize(string filePath, ScenesContainer scenesContainer)
{
    using (var writer = new StreamWriter(File.Open(filePath + "\\savelog.txt", FileMode.Create)))
    {
        // Записываем maxSize
        writer.WriteLine(scenesContainer.maxSize);

        // Записываем количество сцен
        writer.WriteLine(scenesContainer.scenes.Count);

        foreach (var scene in scenesContainer.scenes)
        {
            // Проверка типа сцены и сохранение соответствующих данных
            if (scene is SceneComponent sceneComponent)
            {
                writer.WriteLine("SceneComponent");
                writer.WriteLine(sceneComponent.Name);
                
                writer.WriteLine(sceneComponent.cmds.Count);

                foreach (var cmd in sceneComponent.cmds)
                {
                    writer.WriteLine(cmd.NameCommand);
                }
                
                writer.WriteLine(sceneComponent.components.Count);

                // Сохранение данных для каждого компонента внутри сцены
                foreach (var component in sceneComponent.components)
                {
                    switch (component)
                    {
                        case Character character:
                            {
                                writer.WriteLine("Character");
                                writer.WriteLine(character.Name);
                                writer.WriteLine(character.Caption);
                                writer.WriteLine(character.Height);
                                writer.WriteLine(character.Width);
                                writer.WriteLine(character.X);
                                writer.WriteLine(character.Y);
                                
                                writer.WriteLine(character.Position);
                                writer.WriteLine(character.currentImageIndex);
                                writer.WriteLine(character.currentDialogIndex);
                                
                                writer.WriteLine(character.ImagesPath.Count);
                                foreach (string images in character.ImagesPath)
                                {
                                    writer.WriteLine(images);
                                }

                                writer.WriteLine(character.Dialogs.Count);
                                foreach (Dialog dialog in character.Dialogs)
                                {
                                    writer.WriteLine(dialog.Caption);
                                    writer.WriteLine(dialog.Text);
                                }
                                
                                writer.WriteLine(character.DialogBox.Name);
                                writer.WriteLine(character.DialogBox.ImagePath);
                                writer.WriteLine(character.DialogBox.Height);
                                writer.WriteLine(character.DialogBox.Visible);
                                writer.WriteLine(character.DialogBox.Opacity);
                                writer.WriteLine(character.DialogBox.BackgroundColor.A);
                                writer.WriteLine(character.DialogBox.BackgroundColor.R);
                                writer.WriteLine(character.DialogBox.BackgroundColor.G);
                                writer.WriteLine(character.DialogBox.BackgroundColor.B);
                            }
                            break;
                        case Background background:
                            {
                                writer.WriteLine("Background");
                                writer.WriteLine(background.Name);
                                writer.WriteLine(background.ImagePath);
                                writer.WriteLine(background.currentBackground);
                            }
                            break;
                        case DialogBox dialogBox:
                            {
                                writer.WriteLine("DialogBox");
                                writer.WriteLine(dialogBox.Name);
                                writer.WriteLine(dialogBox.ImagePath);
                                writer.WriteLine(dialogBox.Height);
                                writer.WriteLine(dialogBox.Visible);
                                writer.WriteLine(dialogBox.Opacity);
                                writer.WriteLine(dialogBox.BackgroundColor.A);
                                writer.WriteLine(dialogBox.BackgroundColor.R);
                                writer.WriteLine(dialogBox.BackgroundColor.G);
                                writer.WriteLine(dialogBox.BackgroundColor.B);
                            }
                            break;
                    }
                }
            }
        }
    }
}
    
    public ScenesContainer Txt_Deserialize(string filePath)
{
    ScenesContainer scenesContainer = new ScenesContainer();

    using (var reader = new StreamReader(File.Open(filePath + "\\savelog.txt", FileMode.Open)))
    {
        // Чтение maxSize
        scenesContainer.maxSize = Convert.ToInt32(reader.ReadLine());

        // Чтение количества сцен
        int sceneCount = Convert.ToInt32(reader.ReadLine());
        for (int i = 0; i < sceneCount; i++)
        {
            string componentType = reader.ReadLine();

            if (componentType == "SceneComponent")
            {
                SceneComponent sceneComponent = new SceneComponent
                {
                    Name = reader.ReadLine(),
                };
                
                int cmdsCount = Convert.ToInt32(reader.ReadLine());
                
                for (int ii = 0; ii < cmdsCount; ii++)
                {
                    TimeLineCommand timeLineCommand = new TimeLineCommand()
                    {
                        NameCommand = reader.ReadLine()
                    };
                    sceneComponent.cmds.Add(timeLineCommand);
                }

                int componentCount = Convert.ToInt32(reader.ReadLine());
                for (int j = 0; j < componentCount; j++)
                {
                    componentType = reader.ReadLine();
                    switch (componentType)
                    {
                        case "Character":
                            {
                                Character character = new Character()
                                {
                                    Name = reader.ReadLine(),
                                    Caption = reader.ReadLine(),
                                    Height = Convert.ToInt32(reader.ReadLine()),
                                    Width = Convert.ToInt32(reader.ReadLine()),
                                    X = Convert.ToInt32(reader.ReadLine()),
                                    Y = Convert.ToInt32(reader.ReadLine()),
                                    Position = Convert.ToInt32(reader.ReadLine()),
                                    currentImageIndex = Convert.ToInt32(reader.ReadLine()),
                                    currentDialogIndex = Convert.ToInt32(reader.ReadLine())
                                };

                                int ImagePathsCount = Convert.ToInt32(reader.ReadLine());

                                for (int ii = 0; ii < ImagePathsCount; ii++)
                                {
                                    string imagePath = reader.ReadLine();
                                    character.ImagesPath.Add(imagePath);
                                    character.NewWrapBtn(imagePath);
                                }

                                int DialogsCount = Convert.ToInt32(reader.ReadLine());

                                for (int ii = 0; ii < DialogsCount; ii++)
                                {
                                    Dialog dialog = new Dialog()
                                    {
                                        Caption = reader.ReadLine(),
                                        Text = reader.ReadLine()
                                    };
                                    character.Dialogs.Add(dialog);
                                    character.lbDialogs.Items.Add(dialog.Caption);
                                }

                                DialogBox dialogBox = new DialogBox()
                                {
                                    Name = reader.ReadLine(),
                                    ImagePath = reader.ReadLine(),
                                    Height = Convert.ToInt32(reader.ReadLine()),
                                    Visible = Convert.ToBoolean(reader.ReadLine()),
                                };
                                dialogBox.Opacity = float.Parse(reader.ReadLine());

                                byte a = Convert.ToByte(reader.ReadLine());
                                byte r = Convert.ToByte(reader.ReadLine());
                                byte g = Convert.ToByte(reader.ReadLine());
                                byte b = Convert.ToByte(reader.ReadLine());
                                dialogBox.BackgroundColor = Color.FromArgb(a, r, g, b);

                                sceneComponent.components.Add(character);
                            }
                            break;
                        case "Background":
                            {
                                Background background = new Background()
                                {
                                    Name = reader.ReadLine(),
                                    ImagePath = reader.ReadLine(),
                                    currentBackground = Convert.ToBoolean(reader.ReadLine())
                                };

                                sceneComponent.components.Add(background);
                            }
                            break;
                        case "DialogBox":
                            {
                                DialogBox dialogBox = new DialogBox()
                                {
                                    Name = reader.ReadLine(),
                                    ImagePath = reader.ReadLine(),
                                    Height = Convert.ToInt32(reader.ReadLine()),
                                    Visible = Convert.ToBoolean(reader.ReadLine()),
                                };
                                dialogBox.Opacity = float.Parse(reader.ReadLine());

                                byte a = Convert.ToByte(reader.ReadLine());
                                byte r = Convert.ToByte(reader.ReadLine());
                                byte g = Convert.ToByte(reader.ReadLine());
                                byte b = Convert.ToByte(reader.ReadLine());
                                dialogBox.BackgroundColor = Color.FromArgb(a, r, g, b);

                                sceneComponent.components.Add(dialogBox);
                            }
                            break;
                    }
                }
                scenesContainer.scenes.Add(sceneComponent);
            }
            // Добавьте другие типы компонентов по необходимости
        }
    }

    return scenesContainer;
}
    
    // public void Bin_Serialize(string filePath, ScenesContainer scenesContainer)
    // {
    //     using (var writer = new BinaryWriter(File.Open(filePath + "\\savelog.bin", FileMode.Create)))
    //     {
    //         List<string> lines = new List<string>();
    //         
    //         // Записуємо maxSize
    //         //writer.Write(scenesContainer.maxSize);
    //         lines.Add(scenesContainer.maxSize.ToString());
    //         
    //         // Записуємо кількість сцен
    //         //writer.Write(scenesContainer.scenes.Count);
    //         lines.Add(scenesContainer.scenes.Count.ToString());
    //
    //         foreach (var scene in scenesContainer.scenes)
    //         {
    //             // Перевірка типу сцени і збереження відповідних даних
    //             if (scene is SceneComponent sceneComponent)
    //             {
    //                 writer.Write("SceneComponent");
    //                 writer.Write(sceneComponent.Name);
    //                 writer.Write(sceneComponent.components.Count);
    //                 //when read a file just create new canvas with true property
    //
    //                 // Збереження даних для кожного компонента всередині сцени
    //                 foreach (var component in sceneComponent.components)
    //                 {
    //                     switch (component)
    //                     {
    //                         case Character character:
    //                         {
    //                             writer.Write("Character");
    //                             writer.Write(character.Name);
    //                             writer.Write(character.Caption);
    //                             writer.Write(character.Height);
    //                             writer.Write(character.Width);
    //                             writer.Write(character.X);
    //                             writer.Write(character.Y);
    //                             
    //                             writer.Write(character.ImagesPath.Count);
    //                             foreach (string images in character.ImagesPath)
    //                             {
    //                                 writer.Write(images);
    //                             }
    //
    //                             writer.Write(character.Dialogs.Count);
    //                             foreach (Dialog dialog in character.Dialogs)
    //                             {
    //                                 writer.Write(dialog.Caption);
    //                                 writer.Write(dialog.Text);
    //                             }
    //                             
    //                             writer.Write(character.DialogBox.Name);
    //                             writer.Write(character.DialogBox.ImagePath);
    //                             writer.Write(character.DialogBox.Height);
    //                             writer.Write(character.DialogBox.Visible);
    //                             writer.Write(character.DialogBox.Opacity);
    //                             writer.Write(character.DialogBox.BackgroundColor.A);
    //                             writer.Write(character.DialogBox.BackgroundColor.R);
    //                             writer.Write(character.DialogBox.BackgroundColor.G);
    //                             writer.Write(character.DialogBox.BackgroundColor.B);
    //                         } break;
    //                         case Background background:
    //                         {
    //                             writer.Write("Background");
    //                             writer.Write(background.Name);
    //                             writer.Write(background.ImagePath);
    //                         } break;
    //                         case DialogBox dialogBox:
    //                         {
    //                             writer.Write("DialogBox");
    //                             writer.Write(dialogBox.Name);
    //                             writer.Write(dialogBox.ImagePath);
    //                             writer.Write(dialogBox.Height);
    //                             writer.Write(dialogBox.Visible);
    //                             writer.Write(dialogBox.Opacity);
    //                             writer.Write(dialogBox.BackgroundColor.A);
    //                             writer.Write(dialogBox.BackgroundColor.R);
    //                             writer.Write(dialogBox.BackgroundColor.G);
    //                             writer.Write(dialogBox.BackgroundColor.B);
    //                         } break;
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }
    
    // public ScenesContainer Bin_Deserialize(string filePath)
    // {
    //     ScenesContainer scenesContainer = new ScenesContainer();
    //
    //     using (var reader = new BinaryReader(File.Open(filePath + "\\savelog.bin", FileMode.Open)))
    //     {
    //         // Читання maxSize
    //         scenesContainer.maxSize = reader.ReadInt32();
    //
    //         // Читання кількості сцен
    //         int sceneCount = reader.ReadInt32();
    //         for (int i = 0; i < sceneCount; i++)
    //         {
    //             string componentType = reader.ReadString();
    //
    //             if (componentType == "SceneComponent")
    //             {
    //                 SceneComponent sceneComponent = new SceneComponent
    //                 {
    //                     Name = reader.ReadString()
    //                 };
    //
    //                 int componentCount = reader.ReadInt32();
    //                 for (int j = 0; j < componentCount; j++)
    //                 {
    //                     componentType = reader.ReadString();
    //                     switch (componentType)
    //                     {
    //                         case "Character":
    //                         {
    //                             Character character = new Character()
    //                             {
    //                                 Name = reader.ReadString(),
    //                                 Caption = reader.ReadString(),
    //                                 Height = Convert.ToInt32(reader.ReadString()),
    //                                 Width = Convert.ToInt32(reader.ReadString()),
    //                                 X = Convert.ToInt32(reader.ReadString()),
    //                                 Y = Convert.ToInt32(reader.ReadString())
    //                             };
    //
    //                             int ImagePathsCount = reader.ReadInt32();
    //
    //                             for (int ii = 0; ii < ImagePathsCount; ii++)
    //                             {
    //                                 string imagePath = reader.ReadString();
    //                                 character.ImagesPath.Add(imagePath);
    //                             }
    //
    //                             int DialogsCount = reader.ReadInt32();
    //
    //                             for (int ii = 0; ii < DialogsCount; ii++)
    //                             {
    //                                 Dialog dialog = new Dialog()
    //                                 {
    //                                     Caption = reader.ReadString(),
    //                                     Text = reader.ReadString()
    //                                 };
    //                                 character.Dialogs.Add(dialog);
    //                             }
    //
    //                             DialogBox dialogBox = new DialogBox()
    //                             {
    //                                 Name = reader.ReadString(),
    //                                 ImagePath = reader.ReadString(),
    //                                 Height = Convert.ToInt32(reader.ReadString()),
    //                                 Visible = Convert.ToBoolean(reader.ReadString()),
    //                             };
    //                             float.TryParse(reader.ReadString(), out dialogBox.Opacity);
    //
    //                             byte a = reader.ReadByte();
    //                             byte r = reader.ReadByte();
    //                             byte g = reader.ReadByte();
    //                             byte b = reader.ReadByte();
    //                             dialogBox.BackgroundColor = Color.FromArgb(a, r, g, b);
    //
    //                             sceneComponent.components.Add(character);
    //                         }
    //                             break;
    //                         case "Background":
    //                         {
    //                             Background background = new Background()
    //                             {
    //                                 Name = reader.ReadString(),
    //                                 ImagePath = reader.ReadString(),
    //                             };
    //
    //                             sceneComponent.components.Add(background);
    //                         }
    //                             break;
    //                         case "DialogBox":
    //                         {
    //                             DialogBox dialogBox = new DialogBox()
    //                             {
    //                                 Name = reader.ReadString(),
    //                                 ImagePath = reader.ReadString(),
    //                                 Height = Convert.ToInt32(reader.ReadString()),
    //                                 Visible = Convert.ToBoolean(reader.ReadString()),
    //                             };
    //                             float.TryParse(reader.ReadString(), out dialogBox.Opacity);
    //
    //                             byte a = reader.ReadByte();
    //                             byte r = reader.ReadByte();
    //                             byte g = reader.ReadByte();
    //                             byte b = reader.ReadByte();
    //                             dialogBox.BackgroundColor = Color.FromArgb(a, r, g, b);
    //
    //                             sceneComponent.components.Add(dialogBox);
    //                         }
    //                             break;
    //                     }
    //                 }
    //                 scenesContainer.scenes.Add(sceneComponent);
    //             }
    //             // Додайте інші типи компонентів за потреби
    //         }
    //     }
    //
    // return scenesContainer;
    // }
}