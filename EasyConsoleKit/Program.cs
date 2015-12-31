using System;
using System.Collections.Generic;

using ConsoleKit;

namespace EasyConsoleKit
{
    class Program
    {
        private static Menu _menu;
        private static List<User> _users;

        static void Main(string[] args)
        {
            Console.Title = "Console Kit Demonstration";

            _users = new List<User> { };
            _menu = new Menu()
            {
                HighlightColor = ConsoleColor.Red,

                Options = new string[]
                {
                    "Create new user",
                    "Search users",
                    "View all users",
                    "Exit"
                }
            };

            PresentMenu();
        }

        private static void PresentMenu()
        {
            var choice = _menu.AwaitInput();
            
            switch (choice)
            {
                case 0:
                    CreateUser();
                    break;
                case 1:
                    SearchUser();
                    break;
                case 2:
                    ViewUsers();
                    break;
                default:
                    break;
            }
        }

        private static void CreateUser()
        {
            var name = Validator.GetInput<string>("Enter a name", "Please retry");
            var age = Validator.ValidateInput<int>("Enter age", "Input must be a number over 13", i => i > 13);

            var names = name.Split(' ');

            var user = new User()
            {
                Name = names[0],
                Surname = names[1],

                Age = age
            };

            _users.Add(user);

            DisplayInterstitial("User successfully added");
        }

        private static void SearchUser()
        {
            var name = Validator.GetInput<string>("Enter a first name to search by", "Please retry");
            var userIndex = _users.FindIndex(u => u.Name == name);

            if (userIndex != -1)
            {
                var user = _users[userIndex];

                var table = new Table(50, 2);
                table.PrintRow(new [] { user.Name, user.Surname, user.Age.ToString() });

                DisplayInterstitial("Displaying user");
            }
            else
            {
                DisplayInterstitial("User not found");
            }
        }

        private static void ViewUsers()
        {
            var table = new Table(50, 2);
            table.BuildTable(_users);

            DisplayInterstitial(_users.Count + " found");
        }

        private static void DisplayInterstitial(string completedPrompt)
        {
            Console.WriteLine("\n{0}, press any key to return to the menu...", completedPrompt);
            Console.ReadKey();

            Console.Clear();
            PresentMenu();
        }
    }
}
