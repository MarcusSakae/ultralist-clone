using System.Runtime.Serialization;

public class Task
{
    public int Id { get; set; }
    public string Text { get; set; } = "";
    public string? Status { get; set; }
    public List<string> Notes { get; set; } = new();
    public List<string> Projects { get; set; } = new();  // i.e. +project +devops
    public List<string> Contexts { get; set; } = new();  // i.e. @work @bob 
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsArchived { get; set; }
    public bool IsPriority { get; set; }

    public Task(string text)
    {
        CreatedAt = DateTime.Now;
        Text = text;
    }

    public Task() : this("")
    {
    }


    //
    // Returns task as a string
    // Used in "list" command
    //
    public override string ToString()
    {
        string completed = IsCompleted ? "✔️" : "　"; // note utf8 space (to keep alignment)
        string date = DueDate != null ? DueDate.Value.ToString("ddd MMM dd") : "";
        string status = Status ?? "";
        string result =
            $"[yellow]{Id.ToString().PadRight(4)} [reset][{completed}]  "
            + $"[blue]{date.PadRight(12)}[green]{status} [reset]{Text}";

        foreach (string project in Projects)
        {
            result = result.Replace(project, $"[red]{project}[reset]");
        }
        foreach (string context in Contexts)
        {
            result = result.Replace(context, $"[purple]{context}[reset]");
        }
        return result;
    }

    //
    // Outputs task notes
    // Used in "list" command (when showing notes)
    //
    public string NotesToString()
    {
        string[] result = new string[Notes.Count];
        for (int i = 0; i < Notes.Count; i++)
        {
            result[i] = $"  [blue]{i.ToString().PadRight(4)} [reset]{Notes[i]}";
        }
        return string.Join("\n", result) + (Notes.Count > 0 ? "\n" : "");
    }

    public void ParseContent()
    {
        string[] words = Text.Split(' ');
        Contexts = words.Where(x => x.StartsWith("*")).ToList();
        Projects = words.Where(x => x.StartsWith("+")).ToList();
        string? dueDate = words.FirstOrDefault(x => x.StartsWith("due:"));
        if (dueDate != null)
        {
            DueDate = DateTime.Parse(dueDate[4..]);
            Text = Text.Replace(dueDate, "");
        }
    }
}