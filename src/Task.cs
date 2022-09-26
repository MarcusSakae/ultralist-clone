class Task
{
    public int Id { get; set; }
    public string Text { get; set; }
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
        
        // Collect projects and contexts
        Projects = text.Split(" ").Where(x => x.StartsWith("+")).ToList();
        Contexts = text.Split(" ").Where(x => x.StartsWith("@")).ToList();
        
        // parse due date and remove from text
        string? dueDate = text.Split(" ").FirstOrDefault(x => x.StartsWith("due:"));
        if (dueDate != null)
        {
            text = text.Replace(dueDate, "");
            DueDate = DateTime.Parse(dueDate.Replace("due:", ""));
        }  
        Text = text;  
    }

    public Task() : this("")
    {
    }

    public string CleanText()
    {
        return Text.Replace("", "").Trim();
    }

    //
    // Returns task as a string
    // Used in "list" command
    //
    public override string ToString()
    {
        string completed = IsCompleted ? "✔️" : "　"; // note utf8 space (to keep alignment)
        string date = DueDate != null ? DueDate.Value.ToString("ddd MMM dd") : "";
        return $"[yellow]{Id.ToString().PadRight(4)} [reset][{completed}]  [red]{date}\t[blue]{Status}\t[reset]{Text}";
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
}