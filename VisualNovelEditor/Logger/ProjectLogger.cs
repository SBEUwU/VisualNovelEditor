using System.IO;

namespace VisualNovelEditor;

public class ProjectLogger
{
    private string ProjectsListFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "saves\\ProjectsList.txt");
    public List<String> ProjectFilepaths;

    public ProjectLogger()
    {
        ProjectFilepaths = new List<String>();
    }
    
    public void AddProjectPath(string projectPath)
    {
        string savesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "saves");
        
        if (!Directory.Exists(savesDir))
            Directory.CreateDirectory(savesDir);

        if (!File.Exists(ProjectsListFilePath))
            File.Create(ProjectsListFilePath).Close();

        GetProjectFilepaths();

        if (!ProjectFilepaths.Contains(projectPath))
            File.AppendAllText(ProjectsListFilePath, projectPath + Environment.NewLine);
    }
    
    public void RemovePath(string pathToRemove)
    {
        if (!File.Exists(ProjectsListFilePath))
            return;

        var allPaths = File.ReadAllLines(ProjectsListFilePath).ToList();

        if (allPaths.Remove(pathToRemove))
        {
            File.WriteAllLines(ProjectsListFilePath, allPaths);
        }

        
        ProjectFilepaths = allPaths;
    }

    public void GetProjectFilepaths()
    {
        ProjectFilepaths = File.ReadAllLines(ProjectsListFilePath).ToList();
    }
}