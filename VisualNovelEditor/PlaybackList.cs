using System.Windows.Controls.Primitives;

namespace VisualNovelEditor;

public class PlaybackList : TimeLine
{
    public List<List<TimeLineCommand>> playbackList = new();

    public void AddCommands(List<TimeLineCommand> list)
    {
        playbackList.Add(list);
    }
    
    public override void Swap(int index1, int index2)
    {
        List<TimeLineCommand> temp = playbackList[index1];
        playbackList[index1] = playbackList[index2];
        playbackList[index2] = temp;
    }
    
    public override void Delete(int index)
    {
        playbackList.RemoveAt(index);
    }
    
}