
namespace TodoApp
{
    public class TaskItem
    {
        public string Name { get; set; }
        public bool IsCompleted { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public int Number { get; set; } // Calculated only in memory
    }
}
