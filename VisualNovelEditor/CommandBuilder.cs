using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace VisualNovelEditor;

public class CommandBuilder : TimeLine
{
    public static CommandBuilder commandBuilder;

    public int SceneIndex { set; get; }
    //public List<TimeLineCommand> cmds;

    private CommandBuilder()
    {
        
    }
    
    public static CommandBuilder getInstance()
    {
        if (commandBuilder == null)
            commandBuilder = new CommandBuilder();
        return commandBuilder;
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
    
    
    public override void Delete(int cmdIndex)
    {
        ((SceneComponent)scenesContainer.getScene(SceneIndex)).cmds.RemoveAt(cmdIndex);
    }
    
    public override void Swap(int index1, int index2)
    {
        TimeLineCommand temp = ((SceneComponent)scenesContainer.getScene(SceneIndex)).cmds[index1];
        ((SceneComponent)scenesContainer.getScene(SceneIndex)).cmds[index1] = ((SceneComponent)scenesContainer.getScene(SceneIndex)).cmds[index2];
        ((SceneComponent)scenesContainer.getScene(SceneIndex)).cmds[index2] = temp;
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