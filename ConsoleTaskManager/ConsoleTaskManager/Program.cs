﻿using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ConsoleTaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            ToDo toDo = new ToDo();
            Reminder reminder = new Reminder();
            TaskManager taskManager = new TaskManager();
            
            taskManager.ToRemind("Reminders.json");

            do
            {
                Console.WriteLine("\nToDo - create ToDo task \nRE - create remainder \nAllToDo - look all saved ToDo tasks" +
                    "\nAllRem - look all saved reminders \nNewToDo - look all new ToDo tasks " +
                    "\nNewRem - look all new reminders \n");
                switch (Console.ReadLine().ToString().ToLower())
                {
                    case "todo":
                        toDo.CreateTask(taskManager.toDoList);
                        break;
                    case "re":
                        reminder.CreateTask(taskManager.remList);
                        break;
                    case "alltodo":
                        taskManager.AllSavedToDo("ToDo.json");
                        break;
                    case "allrem":
                        taskManager.AllSavedRem("Reminders.json");
                        break;
                    case "newtodo":
                        taskManager.AllNewToDoTasks();
                        taskManager.EditOrDeleteToDo();
                        break;
                    case "newrem":
                        taskManager.AllNewReminders();
                        taskManager.EditOrDeleteRem();
                        break;
                }
                Console.WriteLine("Click 'E' to close program, or other key to continue");
            }
            while (Console.ReadKey().Key.ToString().ToLower() != "e");

            Console.WriteLine("Save tasks to file?  Click 'Y' to save");
            if (Console.ReadKey().Key.ToString().ToLower() == "y")
            {
                taskManager.AllToDoToFile("ToDo.json");
                taskManager.AllRemToFile("Reminders.json");
            }
        }
    }

    class Task
    {
        private string title;

        public void DeleteTask(Dictionary<string, string> taskList)
        {
            Console.WriteLine("Print title of task to delete it");
            while (!taskList.ContainsKey(title))
            {
                Console.WriteLine("Incorrect title, try again");
                title = Console.ReadLine();
            }
            taskList.Remove(title);
            Console.WriteLine("Your task was deleted");
        }
    }

    class ToDo : Task
    {
        private string title;
        private string description;

        public void CreateTask(Dictionary<string, string> taskList)
        {
            Console.WriteLine("Title:");
            title = Console.ReadLine();
            while (taskList.ContainsKey(title))
            {
                Console.WriteLine("This title already exists, choose another");
                title = Console.ReadLine();
            }
            Console.WriteLine("Description:");
            description = Console.ReadLine();
            Console.WriteLine("Title: " + title + "\nDescription: " + description);
            taskList.Add(title, description);
        }

        public void EditTask(Dictionary<string, string> taskList)
        {
            Console.WriteLine("Print title of task to edit it");
            while (!taskList.ContainsKey(title))
            {
                Console.WriteLine("Incorrect title, try again");
                title = Console.ReadLine();
            }
            taskList.TryGetValue(title, out description);
            Console.WriteLine("Your task:" + description);
            Console.WriteLine("Your new task:");
            description = Console.ReadLine();
            taskList[title] = description;
            Console.WriteLine("Edited task: " + title + " " + description);
        }
    }

    class Reminder : Task
    {
        private string title;
        private string description;
        public DateTime deadline;

        public void CreateTask(Dictionary<string, string> remList)
        {
            Console.WriteLine("Title:");
            title = Console.ReadLine();
            while (remList.ContainsKey(title))
            {
                Console.WriteLine("This title already exists, choose another");
                title = Console.ReadLine();
            }
            Console.WriteLine("Description:");
            description = Console.ReadLine();
            Console.WriteLine("Deadline:");
            if (DateTime.TryParse(Console.ReadLine(), out deadline)) Console.WriteLine("\nDeadline: " + deadline);
            else Console.WriteLine("You have entered an incorrect value.");
            remList.Add(title, description + "| Deadline: |" + deadline);
        }

        public void EditTask(Dictionary<string, string> taskList)
        {
            Console.WriteLine("Print title of task to edit it");
            while (!taskList.ContainsKey(title))
            {
                Console.WriteLine("Incorrect title, try again");
                title = Console.ReadLine();
            }
            taskList.TryGetValue(title, out description);
            Console.WriteLine("Your task:" + description);
            Console.WriteLine("Your new task:");
            description = Console.ReadLine();
            Console.WriteLine("Your new deadline:");
            if (DateTime.TryParse(Console.ReadLine(), out deadline)) Console.WriteLine("\nDeadline: " + deadline);
            else Console.WriteLine("You have entered an incorrect value.");
            taskList[title] = description + "\nDeadline: " + deadline;
            Console.WriteLine("Edited task: " + title + " " + description + "\nDeadline: " + deadline);
        }
    }

    class TaskManager
    {
        public Dictionary<string, string> toDoList = new Dictionary<string, string>();
        public Dictionary<string, string> remList = new Dictionary<string, string>();

        public void EditOrDeleteToDo()
        {
            ToDo toDo = new ToDo();
            Console.WriteLine("Click 'E' to edid any task, 'D' to delete");
            string comm1 = Console.ReadKey().Key.ToString().ToLower();
            if (comm1 == "e") toDo.EditTask(toDoList);
            else if (comm1 == "d") toDo.DeleteTask(toDoList);
        }

        public void EditOrDeleteRem()
        {
            Reminder rem = new Reminder();
            Console.WriteLine("Click 'E' to edid any task, 'D' to delete");
            string comm1 = Console.ReadKey().Key.ToString().ToLower();
            if (comm1 == "e") rem.EditTask(toDoList);
            else if (comm1 == "d") rem.DeleteTask(toDoList);
        }

        public void AllSavedToDo(string fileName)
        {
            string currdir = Directory.GetCurrentDirectory();
            string path = Path.Combine(currdir, fileName);
            //if (File.Exists(path))
            //{
                string content;
                content = File.ReadAllText(path);
                toDoList = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                foreach(KeyValuePair<string, string> kvp in toDoList)
                {
                    Console.WriteLine("Title = {0}, Description: {1}", kvp.Key, kvp.Value);
                }
            //}
            //else Console.WriteLine("You have no saved tasks");
        }

        public void AllSavedRem(string fileName)
        {
            string currdir = Directory.GetCurrentDirectory();
            string path = Path.Combine(currdir, fileName);
            //if (File.Exists(path))
            //{
                string content;
                content = File.ReadAllText(path);
                remList = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                foreach (KeyValuePair<string, string> kvp in remList)
                {
                    Console.WriteLine("Title = {0}, Description: {1}", kvp.Key, kvp.Value);
                }
            //}
            //else Console.WriteLine("You have no saved tasks");
        }

        public void AllNewToDoTasks()
        {
            foreach (KeyValuePair<string, string> kvp in toDoList)
            {
                Console.WriteLine("Title: {0}, Description: {1}", kvp.Key, kvp.Value);
            }
        }

        public void AllNewReminders()
        {
            foreach (KeyValuePair<string, string> kvp in remList)
            {
                Console.WriteLine("Title: {0}, Description: {1}", kvp.Key, kvp.Value);
            }
        }

        public void AllToDoToFile(string fileName)
        {
            string task = JsonConvert.SerializeObject(toDoList, Formatting.Indented);
            string currdir = Directory.GetCurrentDirectory();
            string path = Path.Combine(currdir, fileName);
            File.AppendAllText(path, task);
        }

        public void AllRemToFile(string fileName)
        {
            string task = JsonConvert.SerializeObject(remList, Formatting.Indented);
            string currdir = Directory.GetCurrentDirectory();
            string path = Path.Combine(currdir, fileName);
            File.AppendAllText(path, task);
        }

        public void ToRemind(string fileName)
        {
            DateTime today = DateTime.Now.Date;
            DateTime deadline;
            string currdir = Directory.GetCurrentDirectory();
            string path = Path.Combine(currdir, fileName);
            string content;
            content = File.ReadAllText(path);
            remList = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            foreach (KeyValuePair<string, string> kvp in remList)
            {
                string task = kvp.Value.Substring(kvp.Value.IndexOf("| Deadline: |") + "| Deadline: |".Length, 10);
                if (DateTime.TryParse(task, out deadline))
                {
                    if (DateTime.Compare(deadline, today) == 0)
                    {
                        Console.WriteLine("Your tasks for today:");
                        Console.WriteLine(kvp.Key);
                        Console.WriteLine(kvp.Value);
                    }
                }
            }
        }
    }
}
