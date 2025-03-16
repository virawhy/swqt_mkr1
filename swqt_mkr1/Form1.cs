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
        private int nextListX = 10;
        private int nextListY = 10;
        private ListBox activeListBox = null;
        private ToolStripMenuItem moveTaskMenuItem;

        public Form1()
        {
            // Context menu for task lists
            listContextMenu = new ContextMenuStrip();
            listContextMenu.Items.Add("Add Task", null, AddTask_Click);
            listContextMenu.Items.Add("Edit Task", null, EditTask_Click);
            listContextMenu.Items.Add("Delete Task", null, DeleteTask_Click);
            listContextMenu.Items.Add("Delete List", null, DeleteList_Click);

            // "Move Task To" submenu
            moveTaskMenuItem = new ToolStripMenuItem("Move Task To");
            listContextMenu.Items.Add(moveTaskMenuItem);

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
                ContextMenuStrip = listContextMenu
            };

            newList.MouseDown += ListBoxTasks_MouseDown;

            taskLists.Add(newList);
            Controls.Add(newList);

            UpdateMoveTaskMenu();

            nextListX += 260;
            if (nextListX + 260 > this.Width)
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

        private void DeleteList_Click(object sender, EventArgs e)
        {
            if (activeListBox == null) return;

            if (MessageBox.Show("Are you sure you want to delete this list?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Controls.Remove(activeListBox);
                taskLists.Remove(activeListBox);
                activeListBox.Dispose();
                activeListBox = null;
                UpdateMoveTaskMenu();
            }
        }

        private void MoveTask_Click(object sender, EventArgs e)
        {
            if (activeListBox == null || activeListBox.SelectedItem == null) return;

            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            if (clickedItem != null && clickedItem.Tag is ListBox targetList)
            {
                object taskToMove = activeListBox.SelectedItem;
                activeListBox.Items.Remove(taskToMove);
                targetList.Items.Add(taskToMove);
            }
        }

        private void ListBoxTasks_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                activeListBox = listBox;

                if (e.Button == MouseButtons.Right)
                {
                    int index = listBox.IndexFromPoint(e.Location);
                    if (index != ListBox.NoMatches)
                    {
                        listBox.SelectedIndex = index;
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

        private void UpdateMoveTaskMenu()
        {
            moveTaskMenuItem.DropDownItems.Clear();

            for (int i = 0; i < taskLists.Count; i++)
            {

                    ToolStripMenuItem listItem = new ToolStripMenuItem($"List{i + 1}");
                    listItem.Tag = taskLists[i];
                    listItem.Click += MoveTask_Click;
                    moveTaskMenuItem.DropDownItems.Add(listItem);
            }
        }

    }
}
