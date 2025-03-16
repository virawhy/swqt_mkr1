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
    public partial class AddTaskForm : Form
    {
        private TextBox txtName, txtDescription, txtPersons;
        private ComboBox cmbStatus;
        private DateTimePicker dtpDueDate;
        private Button btnSave, btnCancel;

        public Task NewTask { get; private set; }

        public AddTaskForm(Task taskToEdit = null)
        {
            Text = taskToEdit == null ? "Add New Task" : "Edit Task";
            Width = 300;
            Height = 400;

            Label lblName = new Label() { Text = "Task Name:", Top = 10, Left = 10 };
            txtName = new TextBox() { Top = 35, Left = 10, Width = 260 };

            Label lblDesc = new Label() { Text = "Description:", Top = 70, Left = 10 };
            txtDescription = new TextBox() { Top = 95, Left = 10, Width = 260 };

            Label lblPersons = new Label() { Text = "Assigned Persons (comma-separated):", Top = 130, Left = 10 };
            txtPersons = new TextBox() { Top = 155, Left = 10, Width = 260 };

            Label lblStatus = new Label() { Text = "Status:", Top = 190, Left = 10 };
            cmbStatus = new ComboBox() { Top = 215, Left = 10, Width = 150 };
            cmbStatus.Items.AddRange(new string[] { "To Do", "In Progress", "Completed" });
            cmbStatus.SelectedIndex = 0;

            Label lblDueDate = new Label() { Text = "Due Date (optional):", Top = 250, Left = 10 };
            dtpDueDate = new DateTimePicker() { Top = 275, Left = 10, Width = 150, Format = DateTimePickerFormat.Short };
            dtpDueDate.Checked = false;

            btnSave = new Button() { Text = "Save", Top = 310, Left = 10 };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button() { Text = "Cancel", Top = 310, Left = 100 };
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblDesc);
            Controls.Add(txtDescription);
            Controls.Add(lblPersons);
            Controls.Add(txtPersons);
            Controls.Add(lblStatus);
            Controls.Add(cmbStatus);
            Controls.Add(lblDueDate);
            Controls.Add(dtpDueDate);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);

            if (taskToEdit != null)
            {
                // Pre-fill form with existing task details
                txtName.Text = taskToEdit.Name;
                txtDescription.Text = taskToEdit.Description;
                txtPersons.Text = string.Join(", ", taskToEdit.AssignedPersons);
                cmbStatus.SelectedItem = taskToEdit.Status;
                if (taskToEdit.DueDate.HasValue)
                {
                    dtpDueDate.Value = taskToEdit.DueDate.Value;
                    dtpDueDate.Checked = true;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();
            List<string> persons = new List<string>(txtPersons.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            string status = cmbStatus.SelectedItem.ToString();
            DateTime? dueDate = dtpDueDate.Checked ? dtpDueDate.Value : (DateTime?)null;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Task name is required.");
                return;
            }

            if (persons.Count == 0)
            {
                MessageBox.Show("At least one person must be assigned.");
                return;
            }

            NewTask = new Task(name, description, persons, status, dueDate);
            this.DialogResult = DialogResult.OK;
        }
    }
}
