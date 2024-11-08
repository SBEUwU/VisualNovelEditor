using System.Windows.Controls.Primitives;

namespace VisualNovelEditor;

public class PlaybackList : TimeLine
{
    public List<List<CommandBuilder>> playbackList = new();

    public void AddCommands(List<CommandBuilder> list)
    {
        playbackList.Add(list);
    }
    
    public override void Swap(int index1, int index2)
    {
        List<CommandBuilder> temp = playbackList[index1];
        playbackList[index1] = playbackList[index2];
        playbackList[index2] = temp;
    }
    
    public override void Delete(int index)
    {
        playbackList.RemoveAt(index);
    }
    
}