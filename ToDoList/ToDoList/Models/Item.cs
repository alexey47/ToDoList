using System;

namespace ToDoList.Models
{
    public class Item
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public string Text { get; set; }
    }
}