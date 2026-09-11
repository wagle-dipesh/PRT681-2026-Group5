namespace TaskManagerMVC.Models
{
    public class TasksIndexViewModel
    {
        public IEnumerable<TaskItem> Tasks { get; set; } = Enumerable.Empty<TaskItem>();

        public string? SuccessMessage { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
