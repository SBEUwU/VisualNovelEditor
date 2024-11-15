using System.Windows.Controls;
using System.Windows.Input;

namespace VisualNovelEditor;

public class SupportViewPort
{
    private static SupportViewPort _supportViewPort;
    public ListBox lbScenes;
    public ListBox lbSceneComp;
    public ScenesContainer scenesContainer;
    public Image image;
    public Button btnPlay;
    public static int cmdIndex = 0;
    public static int sceneIndex = 0;
    public delegate void OnMouseDownHandler(object sender, MouseButtonEventArgs e);

    public void SetHandler(OnMouseDownHandler handler)
    {
        image.MouseDown += new MouseButtonEventHandler(handler);
        
    }
    
    public void refreshLbScenes()
    {
        lbScenes.Items.Clear();
        
        foreach (SceneComponent scene in scenesContainer.scenes)
        {
            lbScenes.Items.Add(scene.Name);
        }
    }
    
    private SupportViewPort(ScenesContainer scenesContainer, ListBox lbScenes, ListBox lbSceneComp, Image image, Button btnPlay)
    {
        this.scenesContainer = scenesContainer;
        this.lbScenes = lbScenes;
        this.lbSceneComp = lbSceneComp;
        this.image = image;
        this.btnPlay = btnPlay;
    }

    public static SupportViewPort getInstance(ScenesContainer scenesContainer, ListBox lbScenes, ListBox lbSceneComp, Image image, Button btnPlay)
    {
        if (_supportViewPort == null)
            _supportViewPort = new SupportViewPort(scenesContainer, lbScenes, lbSceneComp, image, btnPlay);
        return _supportViewPort;
    }

    public static SupportViewPort getInstance()
    {
        return _supportViewPort;
    }

    public void Refresh()
    {
        int lbScenesSelectedIndex = lbScenes.SelectedIndex;
        int lbSceneCompSelectedIndex = lbSceneComp.SelectedIndex;
        lbScenes.SelectedIndex = -1;
        lbScenes.SelectedIndex = lbScenesSelectedIndex;
        lbSceneComp.SelectedIndex = lbSceneCompSelectedIndex;
    }

    public void ClearCurrentDialog(int lbScenesSelectedIndex)
    {
        foreach (BaseComponent character in ((SceneComponent)scenesContainer.scenes[lbScenesSelectedIndex]).components)
        {
            if (character is Character characterComponent && characterComponent.currentDialogIndex != -1)
            {
                characterComponent.currentDialogIndex = -1;
                break;
            }
        }
    }
    
    public void ClearCurrentImage(int lbScenesSelectedIndex, int position)
    {
        foreach (BaseComponent character in ((SceneComponent)scenesContainer.scenes[lbScenesSelectedIndex]).components)
        {
            if (character is Character characterComponent && characterComponent.Position == position)
            {
                characterComponent.Position = -1;
                break;
            }
        }
    }
    
    public void ClearCurrentBackground(int lbScenesSelectedIndex)
    {
        foreach (BaseComponent background in ((SceneComponent)scenesContainer.scenes[lbScenesSelectedIndex]).components)
        {
            if (background is Background backgroundComponent && backgroundComponent.currentBackground != false)
            {
                backgroundComponent.currentBackground = false;
                break;
            }
        }
    }
}