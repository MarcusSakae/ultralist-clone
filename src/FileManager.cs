using System.Diagnostics;

namespace Cheap.Ultralist.Knockoff
{

    class FileManager
    {
        static string FileName = "tasks.json";

        string FilePath = Path.Join(Directory.GetCurrentDirectory(), FileName);

        //
        public (bool, string) Init(string[] args)
        {
            Debug.WriteLine("# FileManager init");
            Debug.WriteLine("\tCurrent directory: " + FilePath);

            if (File.Exists(FilePath))
            {
                return (false, "File 'tasks.json' already exists in this folder!");
            }

            File.WriteAllText(FilePath, "[]");
            return (true, "File 'tasks.json' created in this folder!");
        }

        public string GetJsonString()
        {
            return File.Exists(FilePath) ? File.ReadAllText(FilePath) : "";
        }

        public void Save(string jsonString)
        {
            File.WriteAllText(FilePath, jsonString);
        }
    }

}