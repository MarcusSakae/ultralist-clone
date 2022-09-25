using System.Diagnostics;

namespace Cheap.Ultralist.KnockOff
{

    class FileManager
    {
        static string FileName = "tasks.json";

        string FilePath = Path.Join(Directory.GetCurrentDirectory(), FileName);

        //
        public CommandResult Init(string[] args)
        {

            if (File.Exists(FilePath))
            {
                return new CommandResult(false, "File 'tasks.json' already exists in this folder!");
            }

            File.WriteAllText(FilePath, "[]");
            return new CommandResult(true, "File [green]'tasks.json'[reset] created in this folder!");
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