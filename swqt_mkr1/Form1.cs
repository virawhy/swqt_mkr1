using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace swqt_mkr1
{
    public partial class Form1 : Form
    {
        private ListBox listBoxTasks;
        private Button buttonAddTask;
        private List<Task> tasks;

        public Form1()
        {
            tasks = new List<Task>();

            listBoxTasks = new ListBox() { Top = 10, Left = 10, Width = 350, Height = 200 };
            buttonAddTask = new Button() { Text = "Add Task", Top = 220, Left = 10 };

            buttonAddTask.Click += ButtonAddTask_Click;

            Controls.Add(listBoxTasks);
            Controls.Add(buttonAddTask);

            Text = "Task Manager";
            Width = 400;
            Height = 300;
        }

        private void ButtonAddTask_Click(object sender, EventArgs e)
        {
            using (AddTaskForm addTaskForm = new AddTaskForm())
            {
                if (addTaskForm.ShowDialog() == DialogResult.OK)
                {
                    Task newTask = addTaskForm.NewTask;
                    tasks.Add(newTask);
                    listBoxTasks.Items.Add(newTask);
                }
            }
        }
    }
}
