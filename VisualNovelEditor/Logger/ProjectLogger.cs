using System.IO;

namespace VisualNovelEditor;

public class ProjectLogger
{
    private const string filePaths = "//";
    public static List<String> ProjectFilepaths = new List<String>();
    
    public void AddProjectPath(string projectPath)
    {
        if (!Directory.Exists("saves"))
            Directory.CreateDirectory("saves");

        if (!File.Exists(filePaths))
            File.Create(filePaths).Close();

        ProjectFilepaths = File.ReadAllLines(filePaths).ToList();

        if (!ProjectFilepaths.Contains(projectPath))
            File.AppendAllText(filePaths, projectPath + Environment.NewLine);
    }
}