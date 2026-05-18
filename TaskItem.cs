public class TaskItem
{
    public int Id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public DateTime dateCreated { get; set; }
    public DateTime dateUpdated { get; set; }
    public bool IsCompleted { get; set; }
    public string priority {get;set;}

    public void UpdateTitle(string newTitle)
    {
        title = newTitle;
        dateUpdated = DateTime.Now;
    }

    public void UpdateDescription(string newDescription)
    {
        description = newDescription;
        dateUpdated = DateTime.Now;
    }
      public override string ToString()
    {
        string status = IsCompleted ? "✔️" : "❌";
        return $"{Id}. {title} [{status}]   Prioridad: {priority}";
    }
}


public class PriorityTask
{
    public int id {get;set;}
    public string title {get;set;}
}