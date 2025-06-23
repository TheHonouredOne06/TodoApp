using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace TodoApp
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<TaskItem> Tasks { get; set; } = new ObservableCollection<TaskItem>();
        private readonly string saveFilePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            LoadTasks();
        }

        void OnAddClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskEntry.Text))
            {
                Tasks.Add(new TaskItem
                {
                    Name = TaskEntry.Text,
                    IsCompleted = false,
                    Number = Tasks.Count + 1
                });

                TaskEntry.Text = string.Empty;
                SaveTasks();
            }
        }

        void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var task = (TaskItem)button.CommandParameter;

            if (task != null && Tasks.Contains(task))
            {
                Tasks.Remove(task);
                RenumberTasks();   // 🔁 Re-number remaining tasks
                SaveTasks();       // 💾 Save after deletion
            }
        }

        void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
        {
            SaveTasks(); // Save when checkbox is toggled
        }

        void RenumberTasks()
        {
            for (int i = 0; i < Tasks.Count; i++)
            {
                Tasks[i].Number = i + 1;
            }
        }

        void SaveTasks()
        {
            var json = JsonSerializer.Serialize(Tasks);
            File.WriteAllText(saveFilePath, json);
        }

        void LoadTasks()
        {
            if (File.Exists(saveFilePath))
            {
                var json = File.ReadAllText(saveFilePath);
                var savedTasks = JsonSerializer.Deserialize<List<TaskItem>>(json);

                if (savedTasks != null)
                {
                    Tasks.Clear();
                    foreach (var task in savedTasks)
                        Tasks.Add(task);

                    RenumberTasks(); // ✅ Make sure numbers are consistent
                }
            }
        }
    }
}


