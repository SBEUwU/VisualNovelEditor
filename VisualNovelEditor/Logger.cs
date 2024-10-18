using System.Diagnostics;
using System.IO;

namespace VisualNovelEditor;
public enum Commands
{
    ButtonExit = 1,
    ButtonOpen = 2,
    ButtonSave = 3,
}
public class Logger
{
    private static Logger logger;
    List<string> logList = new();

    private Logger()
    {
        
    }

    public static Logger getInstance()
    {
        if (logger == null)
            logger = new();
        return logger;
    }

    public void addLog(string log)
    {
        logList.Add(log);
    }
    public void saveLog(string folderPath = "")
    {
        string filePath;
        string exePath = AppDomain.CurrentDomain.BaseDirectory;;
        string fileNameBase = "log";
        string fileExtension = ".flog";
        int fileIndex = 1;
        
        if (folderPath == "")
        {
            
            filePath = @$"{exePath}saves\";  
            
        }
        else
        {
            filePath = @$"{folderPath}\";
        }
        while (File.Exists(filePath+$"{fileNameBase}{fileIndex}{fileExtension}"))
        {
            fileIndex++;
        }
        filePath += $"{fileNameBase}{fileIndex}{fileExtension}";
        
        // string filePath = Path.Combine(folderPath, $"{fileNameBase}{fileIndex}{fileExtension}");
        
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
        {
            writer.Write(logList.Count);
            
            foreach (var item in logList)
            {
                writer.Write(item);
            }
        }
    }
    
    public void readLog()
    {
        
    }
}