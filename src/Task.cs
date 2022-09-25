class Task
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string Status { get; set; }
    public List<string> Notes { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsArchived { get; set; }
    public bool IsPriority { get; set; }

    public Task(string name)
    {
        Text = name;
        CreatedAt = DateTime.Now;
    }

    public Task() : this("")
    {
    }

    public string ToString()
    {
        string completed = IsCompleted ? "✔️" : " ";
        string shortDate = "Sat Sep 24";
        return $"{Id.ToString().PadRight(4)} [{completed}]  {shortDate}\t{Status}\t{Text}";
    }
}