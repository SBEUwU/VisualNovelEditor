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
    public delegate void OnMouseDownHandler(object sender, MouseButtonEventArgs e);

    public void SetHandler(OnMouseDownHandler handler)
    {
        image.MouseDown += new MouseButtonEventHandler(handler);
        
    }
    private SupportViewPort(ScenesContainer scenesContainer, ListBox lbScenes, ListBox lbSceneComp, Image image)
    {
        this.scenesContainer = scenesContainer;
        this.lbScenes = lbScenes;
        this.lbSceneComp = lbSceneComp;
        this.image = image;
    }

    public static SupportViewPort getInstance(ScenesContainer scenesContainer, ListBox lbScenes, ListBox lbSceneComp, Image image)
    {
        if (_supportViewPort == null)
            _supportViewPort = new SupportViewPort(scenesContainer, lbScenes, lbSceneComp, image);
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