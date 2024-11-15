using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace VisualNovelEditor;

public class Play : TimeLine
{
    public PlaybackList playlist;
    private bool WaitActive;

    public Play()
    {
        SupportViewPort.getInstance().SetHandler(WaitClick_OnMouseDown);
        WaitActive = false;
        playlist = new PlaybackList();
    }

    public void Parser()
    {
        //foreach (TimeLineCommand cmd in ((SceneComponent)scenesContainer.getScene(sceneIndex)).cmds)
        while (SupportViewPort.sceneIndex < playlist.playbackList.Count &&
               WaitActive == false)
        {
            // TimeLineCommand cmd =
            //     ((SceneComponent)scenesContainer.getScene(SupportViewPort.sceneIndex)).cmds[SupportViewPort.cmdIndex];
            
            TimeLineCommand cmd = playlist.playbackList[SupportViewPort.sceneIndex][SupportViewPort.cmdIndex];

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

                            //SupportViewPort.getInstance().Refresh();
                        }
                            break;
                        case "POSITION":
                        {
                            int SceneIndex = int.Parse(cmd.NameCommand.Split(' ')[2]);
                            int CharacterIndex = int.Parse(cmd.NameCommand.Split(' ')[3]);
                            int PositionIndex = int.Parse(cmd.NameCommand.Split(' ')[4]);

                            SupportViewPort.getInstance().ClearCurrentImage(SceneIndex, PositionIndex);

                            ((Character)((SceneComponent)scenesContainer.getScene(SceneIndex))
                                .components[CharacterIndex]).Position = PositionIndex;

                            //SupportViewPort.getInstance().Refresh();

                            //refresh(SceneIndex, PositionIndex);

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
                        }
                            break;
                        case "IMAGE":
                        {
                            int SceneIndex = int.Parse(cmd.NameCommand.Split(' ')[2]);
                            int CharacterIndex = int.Parse(cmd.NameCommand.Split(' ')[3]);
                            int CurrentImageIndex = int.Parse(cmd.NameCommand.Split(' ')[4]);

                            ((Character)((SceneComponent)scenesContainer.getScene(SceneIndex))
                                .components[CharacterIndex]).currentImageIndex = CurrentImageIndex;

                            switch (((Character)((SceneComponent)scenesContainer.getScene(SceneIndex))
                                        .components[CharacterIndex])
                                    .Position)
                            {
                                case 0:
                                {
                                    PropertyDisplayer.VPimageCharacter1.Source = new BitmapImage(new Uri(
                                        ((Character)((SceneComponent)scenesContainer.getScene(
                                                SceneIndex))
                                            .components[CharacterIndex])
                                        .ImagesPath[CurrentImageIndex], UriKind.RelativeOrAbsolute));
                                }
                                    break;
                                case 1:
                                {
                                    PropertyDisplayer.VPimageCharacter2.Source = new BitmapImage(new Uri(
                                        ((Character)((SceneComponent)scenesContainer.getScene(
                                                SceneIndex))
                                            .components[CharacterIndex])
                                        .ImagesPath[CurrentImageIndex], UriKind.RelativeOrAbsolute));
                                }
                                    break;
                            }
                        }
                            break;
                        case "BACKGROUND":
                        {
                            int SceneIndex = int.Parse(cmd.NameCommand.Split(' ')[2]);
                            int BackgroundIndex = int.Parse(cmd.NameCommand.Split(' ')[3]);
                            PropertyDisplayer.VPimageBackground.Source = new BitmapImage(new Uri(
                                ((Background)((SceneComponent)scenesContainer.getScene(
                                        SceneIndex))
                                    .components[BackgroundIndex]).ImagePath, UriKind.RelativeOrAbsolute));

                            //SupportViewPort.getInstance().Refresh();
                        }
                            break;
                    }

                    break;
                }
                case "WAIT":
                {
                    switch (cmd.NameCommand.Split(' ')[1])
                    {
                        case "CLICK":
                        {
                            //SupportViewPort.cmdIndex++;
                            WaitActive = true;
                            //pause();
                        }
                            break;
                    }
                }
                    break;
            }

            SupportViewPort.cmdIndex++;
            if (SupportViewPort.cmdIndex >= playlist.playbackList[SupportViewPort.sceneIndex].Count)
            {
                SupportViewPort.cmdIndex = 0;
                SupportViewPort.sceneIndex++;
            }
        }
    }

    private void WaitClick_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        WaitActive = false;
        Parser();
        if (playlist.playbackList.Count <= SupportViewPort.sceneIndex)
        {
            SupportViewPort.getInstance().btnPlay.IsEnabled = true;
            //CloneOpenSave();
        }
    }

    public void play()
    {
        if (Scene.PlaySwitch)
        {
            Parser();
        }
    }
    
    // public void CloneOpenSave()
    // {
    //     ScenesContainer temp = new ScenesContainer();
    //     
    //     temp = Logger.getInstance().Txt_Deserialize("C:\\test\\");
    //     
    //     scenesContainer.scenes.Clear();
    //     
    //     foreach (SceneComponent scene in temp.scenes)
    //     {
    //         scenesContainer.addComponent(scene);
    //     }
    //     
    //     Invoker.scenesContainer = scenesContainer;
    //     SupportViewPort.getInstance().scenesContainer = scenesContainer;
    // }
    
    public void refresh(int sceneIndex, int PositionIndex)
    {
        foreach (BaseComponent character in ((SceneComponent)scenesContainer.scenes[sceneIndex]).components)
        {
            if (character is Character characterComponent)
            {
                switch (PositionIndex)
                {
                    case 0:
                        if (characterComponent.Position == 1)
                        {
                            characterComponent.Position = -1;
                        }

                        break;
                    case 1:
                        if (characterComponent.Position == 0)
                        {
                            characterComponent.Position = -1;
                        }

                        break;
                }
            }
        }
    }
}

// винести в окремий метод refresh