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
        private ContextMenuStrip contextMenu;
        private List<Task> tasks;

        public Form1()
        {
            tasks = new List<Task>();

            listBoxTasks = new ListBox() { Top = 10, Left = 10, Width = 350, Height = 200 };
            listBoxTasks.MouseDown += ListBoxTasks_MouseDown; // Detect right-click

            contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Add Task", null, AddTask_Click);
            contextMenu.Items.Add("Delete Task", null, DeleteTask_Click);

            listBoxTasks.ContextMenuStrip = contextMenu;

            Controls.Add(listBoxTasks);

            Text = "Task Manager";
            Width = 400;
            Height = 300;
        }

        private void AddTask_Click(object sender, EventArgs e)
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

        private void DeleteTask_Click(object sender, EventArgs e)
        {
            if (listBoxTasks.SelectedItem != null)
            {
                Task selectedTask = (Task)listBoxTasks.SelectedItem;
                tasks.Remove(selectedTask);
                listBoxTasks.Items.Remove(selectedTask);
            }
        }

        private void ListBoxTasks_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = listBoxTasks.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    listBoxTasks.SelectedIndex = index; // Select the task under the cursor
                }
            }
        }
    }
}
