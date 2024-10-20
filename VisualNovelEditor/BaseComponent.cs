using System.Configuration.Internal;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Windows.Controls;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace VisualNovelEditor;

public class BaseComponent
{
    public string Name;

    public virtual void getInfo()
    {
        Console.WriteLine($"Component: {Name}");
    }
}

public class SceneComponent : BaseComponent
{
    public List<SceneComponent> components;
    public Canvas canvas;
    //public Commands commands;

    public SceneComponent()
    {
        components = new List<SceneComponent>();
        canvas = new Canvas();
        canvas.Height = Double.NaN;
        canvas.Width = Double.NaN;
        canvas.Background = Brushes.Transparent;
    }
        
    public void setCanvasSize(int width, int height)
    {
        this.canvas.Width = width;
        this.canvas.Height = height;
    }
    public virtual void addComponent(SceneComponent component)
    {
        component.canvas = this.canvas;
        components.Add(component);
    }

    public virtual void removeComponent(int index)
    {
        components.RemoveAt(index);
    }

    public void createCanvas()
    {
        
    }
}

public class Character : SceneComponent
{
    public string Caption;
    public List<string> images;
    public List<Dialog> dialogs;
    public DialogBox DialogBox;
    
    public int Height;
    public int Width;
    public int X;
    public int Y;

    public Character()
    {
    }
    
    public virtual void addImage(string imagepath)
    {
        images.Add(imagepath);
    }

    public virtual void removeImage(string imagepath)
    {
        images.Remove(imagepath);
    }

    public void move(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}

public class Dialog
{
    public string dialog;
    public Color fontColor;
    public float dialogRenderSpeed;
    public int fontSize;
    
    public void setFontSize(int size)
    {
        this.fontSize = size;
    }
    public void setFontColor(Color newColor)
    {
        this.fontColor = newColor;
    }
    
    public void renderText()
    {
        
    }
}

public class DialogBox : SceneComponent
{
    public Color BackgroundColor;
    public string imagePath;
    public int height;
    public bool visible;
    public float opacity;

    public void changeBackgroundColor(Color newColor)
    {
        this.BackgroundColor = newColor;
    }
    
    public void changeImage(string imagePath)
    {
        this.imagePath = imagePath;
    }
    
    public void changeHeight(int h)
    {
        this.height = h;
    }
    
    public void VisibleTrue()
    {
        this.visible = true;
        
    }
    
    public void VisibleFalse()
    {
        this.visible = false;
    }
}

public class Background : SceneComponent
{
    public string imagePath;
    
    public void changeImage(string imagePath)
    {
        this.imagePath = imagePath;
    }
}

public class Music : SceneComponent
{
    public string audioFilePath;
    public float volume;

    public void play()
    {
        
    }

    public void pause()
    {
        
    }

    public void stop()
    {
        
    }

    public void setVolume(float volume)
    {
        this.volume = volume;
    }
}