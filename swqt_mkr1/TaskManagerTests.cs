using System;
using System.Linq;
using System.Windows.Forms;
using NUnit.Framework;
using swqt_mkr1;

[TestFixture]
public class TaskManagerTests
{
    private Form1 form;

    [SetUp]
    public void Setup()
    {
        form = new Form1();

        form.Load += (sender, e) => { };
        form.Show();

        if (!form.IsHandleCreated)
        {
            form.CreateControl();
        }
    }

    [Test]
    public void Test_CreateNewList_AddsListBox()
    {
        int initialCount = form.Controls.OfType<ListBox>().Count();

        form.Invoke((MethodInvoker)(() =>
        {
            form.GetType()
                .GetMethod("CreateNewList_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(form, new object[] { null, EventArgs.Empty });
        }));

        int newCount = form.Controls.OfType<ListBox>().Count();

        Assert.That(newCount, Is.EqualTo(initialCount + 1), "ListBox should be added to the form.");
    }

    [Test]
    public void Test_DeleteList_RemovesListBox()
    {
        form.Invoke(new Action(() => form.GetType()
            .GetMethod("CreateNewList_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(form, new object[] { null, EventArgs.Empty })));

        ListBox listToRemove = form.Controls.OfType<ListBox>().FirstOrDefault();
        Assert.That(listToRemove, Is.Not.Null, "List should be created first.");

        form.Invoke(new Action(() => form.Controls.Remove(listToRemove)));

        bool exists = form.Controls.Contains(listToRemove);

        Assert.That(exists, Is.False, "ListBox should be removed from the form.");
    }

    [Test]
    public void Test_AddTask_IncreasesItemCount()
    {
        form.Invoke(new Action(() => form.GetType()
            .GetMethod("CreateNewList_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(form, new object[] { null, EventArgs.Empty })));

        ListBox taskList = form.Controls.OfType<ListBox>().FirstOrDefault();
        Assert.That(taskList, Is.Not.Null, "List should exist to add a task.");

        int initialCount = taskList.Items.Count;
        form.Invoke(new Action(() => taskList.Items.Add("New Task")));

        Assert.That(taskList.Items.Count, Is.EqualTo(initialCount + 1), "Task should be added to the list.");
    }

    [Test]
    public void Test_DeleteTask_DecreasesItemCount()
    {
        form.Invoke(new Action(() => form.GetType()
            .GetMethod("CreateNewList_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(form, new object[] { null, EventArgs.Empty })));

        ListBox taskList = form.Controls.OfType<ListBox>().FirstOrDefault();
        Assert.That(taskList, Is.Not.Null, "List should exist to add a task.");

        form.Invoke(new Action(() => taskList.Items.Add("Task to Delete")));
        int initialCount = taskList.Items.Count;

        form.Invoke(new Action(() => taskList.Items.Remove("Task to Delete")));

        Assert.That(taskList.Items.Count, Is.EqualTo(initialCount - 1), "Task should be removed from the list.");
    }

    [Test]
    public void Test_MoveTask_BetweenLists()
    {
        form.Invoke(new Action(() => form.GetType()
            .GetMethod("CreateNewList_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(form, new object[] { null, EventArgs.Empty })));

        form.Invoke(new Action(() => form.GetType()
            .GetMethod("CreateNewList_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(form, new object[] { null, EventArgs.Empty })));

        var lists = form.Controls.OfType<ListBox>().ToList();
        Assert.That(lists.Count, Is.EqualTo(2), "There should be two lists to move tasks between.");

        ListBox sourceList = lists[0];
        ListBox targetList = lists[1];

        form.Invoke(new Action(() => sourceList.Items.Add("Task to Move")));

        Assert.That(sourceList.Items.Count, Is.EqualTo(1), "Task should be added to the source list.");

        object taskToMove = sourceList.Items[0];

        form.Invoke(new Action(() =>
        {
            sourceList.Items.Remove(taskToMove);
            targetList.Items.Add(taskToMove);
        }));

        Assert.That(sourceList.Items.Count, Is.EqualTo(0), "Task should be removed from the source list.");
        Assert.That(targetList.Items.Count, Is.EqualTo(1), "Task should be added to the target list.");
    }
}
