namespace VisualNovelEditor;

public class TimeLineCommand
{
    public string NameCommand;
    public int SceneIndex;
    public int ComponentIndex;
}

public class TimeLineCommandEditCurrentDialog : TimeLineCommand
{
    public int CharacterIndex;
    
    public int DialogIndex;
    
    public TimeLineCommandEditCurrentDialog()
    { 
        DialogIndex = -1;
    }
}

public class TimeLineCommandEditCharacterPosition : TimeLineCommand
{
    public int CharacterIndex;
    
    public int PositionIndex;
    
    public TimeLineCommandEditCharacterPosition()
    { 
        PositionIndex = -1;
    }
}

public class TimeLineCommandEditCharacterCurrentImage : TimeLineCommand
{
    public int CharacterIndex;
    
    public int CurrentImageIndex;
    
    public TimeLineCommandEditCharacterCurrentImage()
    { 
        CurrentImageIndex = -1;
    }
}

public class TimeLineCommandEditCurrentBackground : TimeLineCommand
{
    public int BackgroundIndex;
}