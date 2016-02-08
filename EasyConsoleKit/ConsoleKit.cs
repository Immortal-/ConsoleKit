using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleKit
{
    public class Menu
    {
        /// <summary>
        ///  A list of options to choose. Indexing and numeric quantifying is handled automatically.
        /// </summary>
        public string[] Options;

        /// <summary>
        ///  The selected color to highlight the option with.
        /// </summary>
        public ConsoleColor HighlightColor;

        /// <summary>
        /// A static string that will display at the top of the menu.
        /// </summary>
        public string Banner;

        /// <summary>Prompts for input, handles everything. Returns the corresponding choice against the Options.</summary>
        /// <param name="selected"> Which item/position should be highlighted on print. Ignore for first.</param>
        /// <code>int choice = menu.AwaitInput();</code>
        public int AwaitInput(int selected = 0)
        {
            Console.WriteLine(Banner);
            Console.WriteLine();

            for (var i = 0; i < Options.Length; i++)
            {
                if (selected == i)
                    Console.ForegroundColor = HighlightColor;

                Console.WriteLine(string.Format("{0}.\t{1}", i + 1, Options[i]));
                Console.ResetColor();
            }

            var input = Console.ReadKey();
            var info = TranslateKeyInput(input.Key, selected);

            Console.Clear();

            if (info == -1)
                return selected;
            else
                return this.AwaitInput(info);
        }

        private int TranslateKeyInput(ConsoleKey key, int selected)
        {
            var max = Options.Length - 1;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    return selected == 0 ? max : selected - 1;
                case ConsoleKey.DownArrow:
                    return selected == max ? 0 : selected + 1;
                case ConsoleKey.Enter:
                    return -1;
                default:
                    return selected;
            }
        }
    }

    public class Table
    {
        private readonly int _width;
        private readonly int _indent;

        // <summary>Initializes the Table.</summary>
        // <param name="width"> Width of the table.</param>
        // <param name="indent"> A spacing between the console window border and the start of the grid - purely aesthetic.</param>

        /// <summary>
        /// Initializes the Table.
        /// </summary>
        /// <param name="width">Width of the table.</param>
        /// <param name="indent">Spacing between the console border and the start of the grid.</param>
        public Table(int width, int indent)
        {
            _width = width;
            _indent = indent;
        }

        /// <summary>
        /// Builds a table from a list of custom types. Automatically pulls property names and all corresponding values.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="items">A List<typeparamref name="T"/>> to build a table from.</param>
        public void BuildTable<T>(List<T> items)
        {
            for (var i = 0; i < _indent; i++)
                Console.WriteLine();

            var props = typeof(T).GetProperties();
            var names = props.Select(p => p.Name);

            PrintRow(names);

            for (var i = 0; i < items.Count; i++)
                PrintRow(props.Select(p => p.GetValue(items[i]).ToString()));
        }

        /// <summary>
        /// Prints a row for formatting. 
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="items">A collection to print from. Must hold at least 1 item.</param>
        /// <param name="divider">Whether or not to include a divider below the entry.</param>
        public void PrintRow<T>(IEnumerable<T> items, bool divider = true)
        {
            PrintIndent();

            var length = items.Count();
            var width = (_width - length) / length;

            foreach (var column in items)
                Console.Write(BuildCellString(column.ToString(), width) + '|');

            Console.WriteLine();

            if (divider)
                PrintDivider();
        }

        private void PrintIndent()
        {
            Console.Write(new string(' ', _indent * 2));
        }

        private void PrintDivider()
        {
            PrintIndent();
            Console.WriteLine(new string('-', (_width - 1)));
        }

        private string BuildCellString(string text, int width)
        {
            if (text.Length > width)
                text = Ellipsis(text, width);

            if (string.IsNullOrEmpty(text))
                return new string(' ', width);
            else
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }

        private string Ellipsis(string text, int width)
        {
            return text.Substring(0, width - 3) + new string('.', 3);
        }
    }

    public static class Validator
    {
        /// <summary>
        /// Prompts and validates input against a lambda expression
        /// </summary>
        /// <typeparam name="T">Desired type</typeparam>
        /// <param name="prompt">Instructions for the user</param>
        /// <param name="retry">A message to display if conversion or validation has failed</param>
        /// <param name="comparer">Lambda expression to compare input against. Predicate variable must match target type.</param>
        /// <returns>Appropriated type from console.</returns>
        public static T GetInput<T>(string prompt, string retry, Func<T, bool> comparer)
        {
            T input;
            var valid = false;

            do
            {
                input = PromptForInput<T>(prompt, retry);
                valid = comparer(input);

                if (!valid)
                    Console.WriteLine(retry);
            } while (!valid);

            return (T)Convert.ChangeType(input, typeof(T));
        }

        private static T PromptForInput<T>(string prompt, string retry)
        {
            Console.Write((prompt.EndsWith(" ") ? prompt : prompt + " "));

            var input = Console.ReadLine();

            try
            {
                return (T)Convert.ChangeType(input, typeof(T));
            }
            catch
            {
                Console.WriteLine(retry);
                Console.WriteLine();
                
                return PromptForInput<T>(prompt, retry);
            }
        }
    }
}
