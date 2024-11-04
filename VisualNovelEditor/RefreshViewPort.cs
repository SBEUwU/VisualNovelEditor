using System.Windows.Controls;

namespace VisualNovelEditor;

public class RefreshViewPort
{
    private static RefreshViewPort refreshViewPort;
    public static ListBox lbScenes;
    public static ListBox lbSceneComp;

    private RefreshViewPort()
    {
    }

    public static RefreshViewPort getInstance()
    {
        if (refreshViewPort == null)
            refreshViewPort = new RefreshViewPort();
        return refreshViewPort;
    }

    public void Refresh()
    {
        int lbScenesSelectedIndex = lbScenes.SelectedIndex;
        int lbSceneCompSelectedIndex = lbSceneComp.SelectedIndex;
        lbScenes.SelectedIndex = -1;
        lbScenes.SelectedIndex = lbScenesSelectedIndex;
        lbSceneComp.SelectedIndex = lbSceneCompSelectedIndex;
    }
}