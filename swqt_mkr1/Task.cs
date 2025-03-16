using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swqt_mkr1
{
    public class Task
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> AssignedPersons { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }

        public Task(string name, string description, List<string> persons, string status, DateTime? dueDate)
        {
            Name = name;
            Description = description;
            AssignedPersons = persons ?? new List<string>();
            Status = status;
            DueDate = dueDate;
        }

        public override string ToString()
        {
            string dueDateText = DueDate.HasValue ? $" Due: {DueDate.Value.ToShortDateString()}" : "";
            return $"{Name} - {Status} ({string.Join(", ", AssignedPersons)}){dueDateText}";
        }
    }
}
