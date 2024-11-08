using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace VisualNovelEditor;

public class TimeLine
{
    public static TimeLine timeLine;
    public static ScenesContainer scenesContainer;

    public int SceneIndex { set; get; }
    //public List<TimeLineCommand> cmds;

    private TimeLine()
    {
        
    }
    
    public static TimeLine getInstance()
    {
        if (timeLine == null)
            timeLine = new TimeLine();
        return timeLine;
    }

    public void Parser()
    {
        foreach (TimeLineCommand cmd in ((SceneComponent)scenesContainer.getScene(SceneIndex)).cmds)
        {
            switch (cmd.NameCommand.Split(' ')[0])
            {
                case "EDIT":
                {
                    switch (cmd.NameCommand.Split(' ')[1])
                    {
                        case "DIALOG":
                        {
                            int SceneIndex = int.Parse(cmd.NameCommand.Split(' ')[2]);
                            int CharacterIndex = int.Parse(cmd.NameCommand.Split(' ')[3]);
                            int DialogIndex = int.Parse(cmd.NameCommand.Split(' ')[4]);

                            PropertyDisplayer.VPtbkDialogCaption.Text =
                                ((Character)((SceneComponent)scenesContainer.getScene(SceneIndex))
                                    .components[CharacterIndex]).Caption;

                            PropertyDisplayer.VPtbkDialogText.Text =
                                ((Character)((SceneComponent)scenesContainer.getScene(SceneIndex))
                                    .components[CharacterIndex])
                                .Dialogs[DialogIndex].Text;
                            
                        } break;
                        case "POSITION":
                        {
                            int SceneIndex = int.Parse(cmd.NameCommand.Split(' ')[2]);
                            int CharacterIndex = int.Parse(cmd.NameCommand.Split(' ')[3]);
                            int PositionIndex = int.Parse(cmd.NameCommand.Split(' ')[4]);

                            SupportViewPort.getInstance().ClearCurrentImage(SceneIndex, PositionIndex);

                            ((Character)((SceneComponent)scenesContainer.getScene(SceneIndex))
                                .components[CharacterIndex]).Position = PositionIndex;

                            // int currentImageIndex =
                            //     ((Character)((SceneComponent)scenesContainer.getScene(
                            //             SceneIndex))
                            //         .components[CharacterIndex]).currentImageIndex;

                            // switch (PositionIndex)
                            // {
                            //     case 0:
                            //         PropertyDisplayer.VPimageCharacter1.Source = new BitmapImage(new Uri(
                            //             ((Character)((SceneComponent)scenesContainer.getScene(
                            //                     SceneIndex))
                            //                 .components[CharacterIndex])
                            //             .ImagesPath[currentImageIndex], UriKind.RelativeOrAbsolute));
                            //         break;
                            //     case 1:
                            //         PropertyDisplayer.VPimageCharacter2.Source = new BitmapImage(new Uri(
                            //             ((Character)((SceneComponent)scenesContainer.getScene(
                            //                     SceneIndex))
                            //                 .components[CharacterIndex])
                            //             .ImagesPath[currentImageIndex], UriKind.RelativeOrAbsolute));
                            //         break;
                            // }
                        } break;
                        case "IMAGE":
                        {
                            int SceneIndex = int.Parse(cmd.NameCommand.Split(' ')[2]);
                            int CharacterIndex = int.Parse(cmd.NameCommand.Split(' ')[3]);
                            int CurrentImageIndex = int.Parse(cmd.NameCommand.Split(' ')[4]);

                            ((Character)((SceneComponent)scenesContainer.getScene(SceneIndex))
                                .components[CharacterIndex]).currentImageIndex = CurrentImageIndex;

                            // switch (((Character)((SceneComponent)scenesContainer.getScene(
                            //                 int.Parse(cmd.NameCommand.Split(' ')[1])))
                            //             .components[int.Parse(cmd.NameCommand.Split(' ')[2])])
                            //         .Position)
                            // {
                            //     case 0:
                            //     {
                            //         PropertyDisplayer.VPimageCharacter1.Source = new BitmapImage(new Uri(((Character)((SceneComponent)scenesContainer.getScene(
                            //                     SceneIndex))
                            //                 .components[CharacterIndex])
                            //             .ImagesPath[CurrentImageIndex], UriKind.RelativeOrAbsolute));
                            //     }
                            //         break;
                            //     case 1:
                            //     {
                            //         PropertyDisplayer.VPimageCharacter2.Source = new BitmapImage(new Uri(((Character)((SceneComponent)scenesContainer.getScene(
                            //                     SceneIndex))
                            //                 .components[CharacterIndex])
                            //             .ImagesPath[CurrentImageIndex], UriKind.RelativeOrAbsolute));
                            //     }
                            //         break;
                            // }
                        } break;
                        case "BACKGROUND":
                        {
                            int SceneIndex = int.Parse(cmd.NameCommand.Split(' ')[2]);
                            int BackgroundIndex = int.Parse(cmd.NameCommand.Split(' ')[3]);
                            PropertyDisplayer.VPimageBackground.Source = new BitmapImage(new Uri(
                                ((Background)((SceneComponent)scenesContainer.getScene(
                                        SceneIndex))
                                    .components[BackgroundIndex]).ImagePath, UriKind.RelativeOrAbsolute));
                        } break;
                    } break;
                }
                case "WAIT":
                {
                    switch (cmd.NameCommand.Split(' ')[1])
                    {
                        case "CLICK":
                        {
                            
                        } break;
                    }
                } break;
            }
        }
    }
    
    //--------------------Commands---------------------------
    
    // Add
    // ADD {SceneIndex} {ComponentIndex}
    // public void AddCommand(int SceneIndex, int ComponentIndex)
    // {
    //     TimeLineCommand cmd = new TimeLineCommand()
    //     {
    //         NameCommand = $"ADD {SceneIndex} {ComponentIndex}",
    //         SceneIndex = SceneIndex,
    //         ComponentIndex = ComponentIndex
    //     };
    //     cmds.Add(cmd);
    // }
    
    
    public void DeleteCommand(int cmdIndex)
    {
        ((SceneComponent)scenesContainer.getScene(SceneIndex)).cmds.RemoveAt(cmdIndex);
    }
    
    // EditCurrentDialog
    // EDIT DIALOG {SceneIndex} {CharacterIndex} {DialogIndex}
    public void EditCurrentDialog(int SceneIndex, int CharacterIndex, int DialogIndex)
    {
        TimeLineCommand cmd = new TimeLineCommandEditCurrentDialog()
        {
            NameCommand = $"EDIT DIALOG {SceneIndex} {CharacterIndex} {DialogIndex} ",
            SceneIndex = SceneIndex,
            CharacterIndex = CharacterIndex,
            DialogIndex = DialogIndex
        };
        ((SceneComponent)scenesContainer.getScene(SceneIndex)).cmds.Add(cmd);
    }
    
    // EditCharacterPosition
    // EDIT POSITION {SceneIndex} {CharacterIndex} {PositionIndex}
    public void EditCharacterPosition(int SceneIndex, int CharacterIndex, int PositionIndex)
    {
        TimeLineCommand cmd = new TimeLineCommandEditCharacterPosition()
        {
            NameCommand = $"EDIT POSITION {SceneIndex} {CharacterIndex} {PositionIndex} ",
            SceneIndex = SceneIndex,
            CharacterIndex = CharacterIndex,
            PositionIndex = PositionIndex,
        };
        ((SceneComponent)scenesContainer.getScene(SceneIndex)).cmds.Add(cmd);
    }
    
    // EditCharacterCurrentImage
    // EDIT IMAGE {SceneIndex} {CharacterIndex} {CurrentImageIndex}
    public void EditCharacterCurrentImage(int SceneIndex, int CharacterIndex, int CurrentImageIndex)
    {
        TimeLineCommand cmd = new TimeLineCommandEditCharacterCurrentImage()
        {
            NameCommand = $"EDIT IMAGE {SceneIndex} {CharacterIndex} {CurrentImageIndex} ",
            SceneIndex = SceneIndex,
            CharacterIndex = CharacterIndex,
            CurrentImageIndex = CurrentImageIndex,
        };
        ((SceneComponent)scenesContainer.getScene(SceneIndex)).cmds.Add(cmd);
    }
    
    // EditCurrentBackground
    // EDIT BACKGROUND {SceneIndex} {BackgroundIndex}
    public void EditCurrentBackground(int SceneIndex, int BackgroundIndex)
    {
        TimeLineCommand cmd = new TimeLineCommandEditCurrentBackground()
        {
            NameCommand = $"EDIT BACKGROUND {SceneIndex} {BackgroundIndex} ",
            SceneIndex = SceneIndex,
            BackgroundIndex = BackgroundIndex
        };
        ((SceneComponent)scenesContainer.getScene(SceneIndex)).cmds.Add(cmd);
    }

    // WaitClick
    // WAITCLICK
    public void WaitClick()
    {
        TimeLineCommand cmd = new TimeLineCommand()
        {
            NameCommand = "WAIT CLICK",
        };
        ((SceneComponent)scenesContainer.getScene(SceneIndex)).cmds.Add(cmd);
    }
}