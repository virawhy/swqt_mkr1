using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace swqt_mkr1
{
    public partial class Form1 : Form
    {
        private List<ListBox> taskLists = new List<ListBox>();
        private ContextMenuStrip listContextMenu;
        private ContextMenuStrip formContextMenu;
        private int nextListX = 10; // X position for new lists
        private int nextListY = 10; // Y position for new lists
        private ListBox activeListBox = null; // Stores the right-clicked ListBox

        public Form1()
        {
            // Context menu for task lists
            listContextMenu = new ContextMenuStrip();
            listContextMenu.Items.Add("Add Task", null, AddTask_Click);
            listContextMenu.Items.Add("Edit Task", null, EditTask_Click);
            listContextMenu.Items.Add("Delete Task", null, DeleteTask_Click);

            // Context menu for creating new lists
            formContextMenu = new ContextMenuStrip();
            formContextMenu.Items.Add("Create New List", null, CreateNewList_Click);
            this.ContextMenuStrip = formContextMenu;

            this.MouseDown += Form_MouseDown;

            Text = "Task Manager";
            Width = 600;
            Height = 400;
        }

        private void CreateNewList_Click(object sender, EventArgs e)
        {
            ListBox newList = new ListBox()
            {
                Left = nextListX,
                Top = nextListY,
                Width = 250,
                Height = 200,
                ContextMenuStrip = listContextMenu // Each list has its own context menu
            };

            newList.MouseDown += ListBoxTasks_MouseDown; // Capture right-clicked list

            taskLists.Add(newList);
            Controls.Add(newList);

            // Position next list beside the previous one
            nextListX += 260;
            if (nextListX + 260 > this.Width) // If out of bounds, move to next row
            {
                nextListX = 10;
                nextListY += 210;
            }
        }

        private void AddTask_Click(object sender, EventArgs e)
        {
            if (activeListBox == null) return;

            using (AddTaskForm addTaskForm = new AddTaskForm())
            {
                if (addTaskForm.ShowDialog() == DialogResult.OK)
                {
                    activeListBox.Items.Add(addTaskForm.NewTask);
                }
            }
        }

        private void EditTask_Click(object sender, EventArgs e)
        {
            if (activeListBox == null || activeListBox.SelectedItem == null) return;

            Task selectedTask = activeListBox.SelectedItem as Task;
            using (AddTaskForm editForm = new AddTaskForm(selectedTask))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    int index = activeListBox.SelectedIndex;
                    activeListBox.Items[index] = editForm.NewTask;
                }
            }
        }

        private void DeleteTask_Click(object sender, EventArgs e)
        {
            if (activeListBox == null || activeListBox.SelectedItem == null) return;

            activeListBox.Items.Remove(activeListBox.SelectedItem);
        }

        private void ListBoxTasks_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                activeListBox = listBox; // Set the active list box

                if (e.Button == MouseButtons.Right)
                {
                    int index = listBox.IndexFromPoint(e.Location);
                    if (index != ListBox.NoMatches)
                    {
                        listBox.SelectedIndex = index; // Select task on right-click
                    }
                }
            }
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                formContextMenu.Show(this, e.Location);
            }
        }
    }
}
