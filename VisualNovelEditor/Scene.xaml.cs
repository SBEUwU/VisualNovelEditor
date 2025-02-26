﻿using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace VisualNovelEditor;

public partial class Scene : Window
{
    
    private Logger logger;
    ScenesContainer scenesContainer;
    //Canvas currentCanvas;
    PropertyDisplayer propertyDisplayer;
    DisplaySceneComponentCommand displaySceneComponentCommand;
    DisplayBackgroundCommand displayBackgroundCommand;
    DisplayDialogBoxCommand displayDialogBoxCommand;
    DisplayCharacterCommand displayCharacterCommand;
    SupportViewPort _supportViewPort;
    Invoker invoker;
    Play play;
    public static bool PlaySwitch;

    public Scene()
    {
        InitializeComponent();
        PlaySwitch = false;
        scenesContainer = new ScenesContainer();
        propertyDisplayer = new PropertyDisplayer();
        displaySceneComponentCommand = new DisplaySceneComponentCommand();
        displayBackgroundCommand = new DisplayBackgroundCommand();
        displayDialogBoxCommand = new DisplayDialogBoxCommand();
        displayCharacterCommand = new DisplayCharacterCommand();
        invoker = new Invoker(stkpnlProperties);
        logger = Logger.getInstance();

        PropertyDisplayer.stkpnlProperties = stkpnlProperties;
        //PropertyDisplayer.wrpnlProperLists = wrpnlProperLists;
        //PropertyDisplayer.brdrProperLists = brdrProperLists;
        //PropertyDisplayer.gridProperLists = gridProperLists;
        PropertyDisplayer.brdrLeftProperLists = brdrLeftProperLists;
        PropertyDisplayer.brdrRightProperLists = brdrRightProperLists;
        Invoker.scenesContainer = scenesContainer;

        PropertyDisplayer.VPtbkDialogCaption = VPtbkDialogCaption;
        PropertyDisplayer.VPtbkDialogText = VPtbkDialogText;
        PropertyDisplayer.VPimageBackground = VPimageBackground;
        PropertyDisplayer.VPimageCharacter1 = VPimageCharacter1;
        PropertyDisplayer.VPimageCharacter2 = VPimageCharacter2;
        PropertyDisplayer.VPbrdrDialogBox = VPbrdrDialogBox;
        
        _supportViewPort = SupportViewPort.getInstance(scenesContainer, lbScenes, lbSceneComp, VPimageBackground, btnPlay);
        
        TimeLine.scenesContainer = scenesContainer;
        play = new Play();
    }

    private void BtnNewScene_OnClick(object sender, RoutedEventArgs e)
    {
        //gridProperLists.Children.Clear();
        brdrLeftProperLists.Child = null;
        brdrRightProperLists.Child = null;
        
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

        //currentCanvas = ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).canvas;
    }

    private void lbScenes_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lbScenes.SelectedIndex != -1)
        {
            ClearViewport();

            //gridProperLists.Children.Clear();
            brdrLeftProperLists.Child = null;
            brdrRightProperLists.Child = null;

            lbSceneComp.Items.Clear();

            while (stkpnlProperties.Children.Count > 1)
            {
                stkpnlProperties.Children.RemoveAt(stkpnlProperties.Children.Count - 1);
            }


            propertyDisplayer.DisplaySceneComponentViewport(
                (SceneComponent)scenesContainer.scenes[lbScenes.SelectedIndex]);

            refreshLbSceneComp();

            // cvsScene.Children.Clear();
            // currentCanvas = ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).canvas;
            // cvsScene.Children.Add(currentCanvas);


            displaySceneComponentCommand.set(propertyDisplayer,
                (SceneComponent)scenesContainer.scenes[lbScenes.SelectedIndex]);

            invoker.SetCommand(displaySceneComponentCommand);
            invoker.ExecuteCommand(tbProper_KeyDown);

            // if (currentCanvas != null)
            //     MessageBox.Show("current Canvas childrens: " + currentCanvas.Children.Count.ToString()
            //                                                  + "\ncurrent Scene components: " +
            //                                                  ((SceneComponent)scenesContainer.getScene(
            //                                                      lbScenes.SelectedIndex)).components.Count.ToString());
        }
    }

    private void btnNewCharacter_OnClick(object sender, RoutedEventArgs e)
    {
        if (lbScenes.SelectedIndex != -1)
        {
            Character newCharacter = new Character()
            {
                Name = "Character" +
                       (((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components.Count + 1)
            };

            ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components.Add(newCharacter);

            
            //currentCanvas.Children.Add(newCharacter);

            refreshLbSceneComp();
        }
    }

    private void btnNewBackground_OnClick(object sender, RoutedEventArgs e)
    {
        if (lbScenes.SelectedIndex != -1)
        {
            Background background = new Background()
            {
                Name = "Background" +
                       (((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components.Count + 1)
            };

            ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components.Add(background);

            //currentCanvas.Children.Add(newCharacter);

            refreshLbSceneComp();
        }
    }

    private void lbSceneComp_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lbScenes.SelectedIndex != -1)
        {
            //brdrProperLists.Child = null;

            // if (brdrProperLists.Child is Panel panel)
            //     panel.Children.Clear();
            // else
            //     brdrProperLists.Child = null;
            brdrLeftProperLists.Child = null;
            brdrRightProperLists.Child = null;


            while (stkpnlProperties.Children.Count > 1)
            {
                stkpnlProperties.Children.RemoveAt(stkpnlProperties.Children.Count - 1);
            }

            if (lbSceneComp.SelectedIndex != -1)
            {
                if (((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components[
                        lbSceneComp.SelectedIndex]
                    is Character)
                {
                    propertyDisplayer.ShowWrapPanelProperty(
                        (Character)((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components[
                            lbSceneComp.SelectedIndex]);
                    propertyDisplayer.ShowListBoxProperty(
                        (Character)((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components[
                            lbSceneComp.SelectedIndex]);
                }

                switch (((SceneComponent)scenesContainer.scenes[lbScenes.SelectedIndex]).components[
                            lbSceneComp.SelectedIndex])
                {
                    case VisualNovelEditor.Background background:
                    {
                        displayBackgroundCommand.set(propertyDisplayer,
                            (Background)((SceneComponent)scenesContainer.scenes[lbScenes.SelectedIndex]).components[
                                lbSceneComp.SelectedIndex]);
                        invoker.SetCommand(displayBackgroundCommand);
                    }
                        break;
                    case VisualNovelEditor.DialogBox dialogBox:
                    {
                        displayDialogBoxCommand.set(propertyDisplayer,
                            (DialogBox)((SceneComponent)scenesContainer.scenes[lbScenes.SelectedIndex]).components[
                                lbSceneComp.SelectedIndex]);
                        invoker.SetCommand(displayDialogBoxCommand);
                    }
                        break;
                    case VisualNovelEditor.Character character:
                    {
                        displayCharacterCommand.set(propertyDisplayer,
                            (Character)((SceneComponent)scenesContainer.scenes[lbScenes.SelectedIndex]).components[
                                lbSceneComp.SelectedIndex]);
                        invoker.SetCommand(displayCharacterCommand);
                    }
                        break;
                }
            }

            invoker.ExecuteCommand(tbProper_KeyDown);

            //InitializeTextBoxes();

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
    }

    // public void refreshLbScenes()
    // {
    //     lbScenes.Items.Clear();
    //     
    //         foreach (SceneComponent scene in scenesContainer.scenes)
    //         {
    //             lbScenes.Items.Add(scene.Name);
    //         }
    // }
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
    
    public void refreshLbSceneCompTest()
    {
        lbSceneComp.Items.Clear();

        lbScenes.SelectedIndex = 0;
        foreach (BaseComponent component in
                 ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components)
        {
            lbSceneComp.Items.Add(component.Name);
        }
    }

    public void ClearViewport()
    {
        VPimageBackground.Source = null;
        VPimageCharacter1.Source = null;
        VPimageCharacter2.Source = null;
        VPtbkDialogText.Text = "";
        VPtbkDialogCaption.Text = "";
    }


    private void BtnNewDialogBox_OnClick(object sender, RoutedEventArgs e)
    {
        if (lbScenes.SelectedIndex != -1)
        {
            DialogBox dialogBox = new DialogBox()
            {
                Name = "DialogBox" +
                       (((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components.Count + 1)
            };

            ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components.Add(dialogBox);

            //currentCanvas.Children.Add(newCharacter);

            refreshLbSceneComp();
        }
    }

    public void tbProper_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            TextBox textBox = sender as TextBox;
            
            if (textBox != null)
            {
                int sceneIndex = lbScenes.SelectedIndex;
                int componentIndex = lbSceneComp.SelectedIndex;

                if (sceneIndex >= 0 && componentIndex >= 0)
                {
                    string propertyName = textBox.Name.Replace("tbProper", "");
                    string value = textBox.Text;
                    
                    invoker.Edit(sceneIndex, componentIndex, propertyName, value);
                    int lbSceneCompSelectedIndex = lbSceneComp.SelectedIndex;
                    refreshLbSceneComp();
                    lbSceneComp.SelectedIndex = lbSceneCompSelectedIndex;
                    SupportViewPort.getInstance().Refresh();
                }
            }
        }
    }

    // private void InitializeTextBoxes()
    // {
    //     foreach (var child in stkpnlProperties.Children)
    //     {
    //         if (child is StackPanel stackPanel)
    //         {
    //             foreach (var innerChild in stackPanel.Children)
    //             {
    //                 if (innerChild is TextBox textBox)
    //                 {
    //                     // Проверяем, установлен ли обработчик
    //                     if (!IsHandlerSet(textBox, nameof(textBox.KeyDown)))
    //                     {
    //                         textBox.KeyDown += tbProper_KeyDown;
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }
    //
    // private bool IsHandlerSet(TextBox textBox, string eventName)
    // {
    //     var eventField = typeof(TextBox).GetEvent(eventName);
    //     if (eventField != null)
    //     {
    //         var fieldInfo = typeof(TextBox).GetField(eventName,
    //             System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    //         if (fieldInfo != null)
    //         {
    //             var invocationList = (MulticastDelegate)fieldInfo.GetValue(textBox);
    //             return invocationList != null && invocationList.GetInvocationList()
    //                 .Contains((EventHandler<KeyEventArgs>)tbProper_KeyDown);
    //         }
    //     }
    //
    //     return false;
    // }

    // private void TbProperName_OnMouseDown(object sender, MouseButtonEventArgs e)
    // {
    // }

    // private void BtnNewScene_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    // {
    //     brdrProperLists.Child = null;
    // }

    private void BtnNewCompMenu_OnClick(object sender, RoutedEventArgs e)
    {
        btnNewCompMenu.ContextMenu.IsOpen = true;
    }
    
    // private void FileOpenPlay_OnClick()
    // {
    //     scenesContainer = logger.Txt_Deserialize("C:\\test\\");
    //     Invoker.scenesContainer = scenesContainer;
    //     SupportViewPort.getInstance().scenesContainer = scenesContainer;
    //     CommandBuilder.scenesContainer = scenesContainer;
    //     _supportViewPort.refreshLbScenes();
    // }

    private void FileSavePlay_OnClick()
    {
        if (scenesContainer.scenes != null && scenesContainer.scenes.Count > 0)
        {
            logger.Txt_Serialize("C:\\test\\savelog.txt", scenesContainer);
        }
    }
    
    // private void mmFileOpen_OnClick(object sender, RoutedEventArgs e)
    // {
    //     scenesContainer = logger.Txt_Deserialize(Logger.filePathConst);
    //     Invoker.scenesContainer = scenesContainer;
    //     SupportViewPort.getInstance().scenesContainer = scenesContainer;
    //     CommandBuilder.scenesContainer = scenesContainer;
    //     _supportViewPort.refreshLbScenes();
    // }
    
    private void OpenAs(string filePath)
    {
        scenesContainer = logger.Txt_Deserialize(filePath);
        Invoker.scenesContainer = scenesContainer;
        SupportViewPort.getInstance().scenesContainer = scenesContainer;
        CommandBuilder.scenesContainer = scenesContainer;
        _supportViewPort.refreshLbScenes();
    }

    private void mmFileSave_OnClick(object sender, RoutedEventArgs e)
    {
        if (scenesContainer.scenes != null && scenesContainer.scenes.Count > 0)
        {
            logger.Txt_Serialize(Logger.filePathConst, scenesContainer);
        }
    }

    private void mmEditDeleteScene_OnClick(object sender, RoutedEventArgs e)
    {
        if (lbScenes.SelectedIndex != -1)
        {
            scenesContainer.scenes.RemoveAt(lbScenes.SelectedIndex);
            _supportViewPort.refreshLbScenes();
            refreshLbSceneComp();
            ClearViewport();
        }
    }

    private void mmEditDeleteComponent_OnClick(object sender, RoutedEventArgs e)
    {
        if (lbScenes.SelectedIndex != -1 && lbSceneComp.SelectedIndex != -1)
        {
            ((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).components.RemoveAt(lbSceneComp.SelectedIndex);
            refreshLbSceneComp();
            SupportViewPort.getInstance().Refresh();
        }
    }

    private void btnEditCommandsTimeLine_OnClick(object sender, RoutedEventArgs e)
    {
        if (lbScenes.SelectedIndex != -1)
        {
            windEditCommandsTimeLine windEditCommandsTimeLine = new windEditCommandsTimeLine();

            windEditCommandsTimeLine.SceneIndex = lbScenes.SelectedIndex;
            CommandBuilder.getInstance().SceneIndex = lbScenes.SelectedIndex;
            windEditCommandsTimeLine.RefreshLbReadyCommands();
            windEditCommandsTimeLine.ShowDialog();
        }
    }

    private void BtnPlay_OnClick(object sender, RoutedEventArgs e)
    {
        ((Button)sender).IsEnabled = false;
        SupportViewPort.sceneIndex = 0;
        SupportViewPort.cmdIndex = 0;
        // mmFileSave_OnClick(sender, e);
        FileSavePlay_OnClick();
        PlaySwitch = true;
        play.play();
        PlaySwitch = false;
        // mmFileOpen_OnClick(sender, e);
    }

    private void BtnAddPlaybackScene_OnClick(object sender, RoutedEventArgs e)
    {
        if (lbScenes.SelectedIndex > -1)
        {
            if (((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).cmds.Count > 0)
            {
                play.playlist.AddCommands(((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).cmds);
                lbPlaybackScenes.Items.Add(((SceneComponent)scenesContainer.getScene(lbScenes.SelectedIndex)).Name);
            }
        }
    }

    private void mmFileSaveAs_OnClick(object sender, RoutedEventArgs e)
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "Text Files (*.txt)|*.txt";
        if (sfd.ShowDialog() == true)
        {
            logger.Txt_Serialize(sfd.FileName, scenesContainer);
        }
    }
    
    private void mmFileOpenAs_OnClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "Text Files (*.txt)|*.txt";
        if (ofd.ShowDialog() == true)
        {
            OpenAs(ofd.FileName);
        }
    }

    private void mmFileNew_OnClick(object sender, RoutedEventArgs e)
    {
        scenesContainer.scenes.Clear();
        lbPlaybackScenes.Items.Clear();
        lbScenes.Items.Clear();
        lbSceneComp.Items.Clear();
        VPimageBackground.Source = null;
        VPimageCharacter1.Source = null;
        VPimageCharacter2.Source = null;
        VPtbkDialogText.Text = "";
        VPtbkDialogCaption.Text = "";
        play.playlist.playbackList.Clear();
        tbProperName.Text = "";
        scenesContainer.maxSize = 0;
        brdrLeftProperLists.Child = null;
        brdrRightProperLists.Child = null;
        int i = stkpnlProperties.Children.Count - 1;
        for (; stkpnlProperties.Children.Count != 1; i--)
        {
            stkpnlProperties.Children.RemoveAt(i);
        }
    }
}

//----------------------------------------НИЗЬКИЙ ПРИОРІТЕТ--------------------------------------

//Баг після вибору компоненте та потім вибору сцени, немає обробника подій tbName --
//якщо сцени нема, не можна добавляти компоненти
//Exit
// зоборона завантаження всіх файлів в катинки окрім png, jpg
// перевіряти на наявність файлу перед завантаженням сейву
// зробити максимальний розмір завантаженої картинки

// зробити swap commandbuilder

//----------------------------------------ВИСОКИЙ ПРИОРІТЕТ--------------------------------------

// доробити save
//Відкриття проекту з іншого вікна
// - питати куди хоче користувач зберігати проект

//--------------------------------------ВИКОНАНІ-------------------------------------------------

//Зробити видалення компонентів
//Виправити баг з видаленням персонажа, діалогу в який вже встановлений position, currentDialog
//перевірки на вже встановлені картинки, background.