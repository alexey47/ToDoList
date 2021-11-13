using System;
using System.Collections.ObjectModel;
using ToDoList.Models;
using Xamarin.Forms;

namespace ToDoList.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            Title = "To-Do List";

            NewItem = string.Empty;
            Items = new ObservableCollection<Item>(DataStore.GetItemsAsync().Result);
        }

        public ObservableCollection<Item> Items { get; set; }

        private string _newItem;
        public string NewItem
        {
            get => _newItem;
            set
            {
                _newItem = value;
                OnPropertyChanged(nameof(NewItem));
            }
        }

        public Command AddItem
        {
            get
            {
                return new Command(() =>
                {
                    if (NewItem.Replace(" ", "") == string.Empty)
                    {
                        return;
                    }
                    
                    var newItem = new Item
                    {
                        Id = Guid.NewGuid().ToString(),
                        Date = DateTime.Now,
                        IsCompleted = false,
                        Text = NewItem,
                    };
                    Items.Add(newItem);
                    DataStore.AddItemAsync(newItem);

                    NewItem = string.Empty;
                });
            }
        }
        public Command DeleteItem
        {
            get
            {
                return new Command<Item>(item =>
                {
                    Items.Remove(item);
                    DataStore.DeleteItemAsync(item.Id);
                });
            }
        }
        public Command CompleteItem
        {
            get
            {
                return new Command<Item>(item =>
                {
                    item.IsCompleted = !item.IsCompleted;
                    DataStore.UpdateItemAsync(item);
                    
                    Items.Move(0, 0);
                });
            }
        }
        public Command UpdateItem
        {
            get
            {
                return new Command<Item>(item =>
                {
                    DataStore.UpdateItemAsync(item);
                });
            }
        }
    }
}