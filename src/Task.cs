class Task
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string? Status { get; set; }
    public List<string> Notes { get; set; } = new();
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsArchived { get; set; }
    public bool IsPriority { get; set; }

    public Task(string text)
    {
        Text = text;
        CreatedAt = DateTime.Now;
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