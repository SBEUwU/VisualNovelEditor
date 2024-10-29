namespace VisualNovelEditor;

public class ScenesContainer : BaseComponent
{
    public List<BaseComponent> scenes  { get; set; } = new List<BaseComponent>();
    public int maxSize  { get; set; } = 0;
    
    public virtual void addComponent(BaseComponent scene)
    {
        maxSize++;
        scene.Name = "Scene" + maxSize;
        ((SceneComponent)scene).canvas.Name = scene.Name;
        scenes.Add(scene);
    }

    public virtual void removeComponent(int index)
    {
        scenes.RemoveAt(index);
    }

    public string getInfoLast()
    {
        return scenes.Last().Name;
    }

    public BaseComponent getScene(int index) => scenes[index];
}