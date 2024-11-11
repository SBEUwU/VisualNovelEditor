using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace VisualNovelEditor;

public class Play : TimeLine
{
    private bool WaitActive;
    
    public Play()
    {
        SupportViewPort.getInstance().SetHandler(WaitClick_OnMouseDown);
        WaitActive = false;
    }
    public async void Parser(int sceneIndex)
    {
        foreach (TimeLineCommand cmd in ((SceneComponent)scenesContainer.getScene(sceneIndex)).cmds)
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
                            
                            SupportViewPort.getInstance().Refresh();

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

                            SupportViewPort.getInstance().Refresh();
                            
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

                            SupportViewPort.getInstance().Refresh();
                        } break;
                    } break;
                }
             case "WAIT":
             {
                 switch (cmd.NameCommand.Split(' ')[1])
                 {
                     case "CLICK":
                     { 
                         WaitActive = true;
                         await pause();
                     } break;
                 }
             } break;
            }
        }
    }
    
    private void WaitClick_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        WaitActive = false;
    }

    public void play()
    {
        if (Scene.PlaySwitch)
        {
            for (int i = 0; i < scenesContainer.scenes.Count; i++)
            {
                Parser(i);
            }
        }
    }

    public async Task pause()
    {
        while (WaitActive)
        {
            await Task.Delay(100);
        }
    }
}