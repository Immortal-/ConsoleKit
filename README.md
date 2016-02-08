# Console Kit
A handy tool kit for making the console more user friendly with interactive menus, easy validation and tables.

**Somebody botted the repo stars, this isn't genuinely trending :( **

![Demo](http://orig08.deviantart.net/f75d/f/2015/365/5/3/2015_12_31_16_26_15_by_oduslabs-d9m6imp.gif)

Cool right?

##Interactive Menu
Offers a far more pleasant experience selecting an option than prompting the user to enter a choice corresponding to a printed menu.

All you have to to is set your options and console highlight color. The menu can be reused without having to be reinstantiated by setting the Options property and calling AwaitInput() again. It returns the selected option zero-based, iterator is only visually inflated to show as an ordered list.

Using is very simple to set up:
```C#
Menu menu = new Menu()
{
    Banner = "Easy Console Kit Demonstration",
    HighlightColor = ConsoleColor.Red,
    
    Options = new string[] {"Menu item", "Another item", "One more should do it"}
};

int selection = menu.AwaitInput();
```
##Validator
Writing a bunch of while loops and constant checking can get very boring, so it's wrapped up nicely here for you. To keep things neat, a pace is automatically appended to your prompt if it doesn't already end with one.

Validation supports a lambda expression to act as a validator. This could be handy for anything, such as:
- A string is lower case, entirely alphabetical, contains < x characters, etc.
- An int is within a certain range
- You get the idea

```C#
int age = Validator.ValidateInput<int>("Enter your age", "Input must be a number", (i) => i > 18);
```

So long as the expression returns a `bool`, you're good to go.

##Table
For easy data formatting, a table builder is also included specifically for custom data models. Our test case model looks like this:
```C#
public struct User
{
    public string Name { get; set; }
    public string Surname { get; set; }

    public int Age { get; set; }
}
```

Building the table is very easy, can be done in only 2 lines:
```C#
List<User> users = new List<User> { }; //assume populated

Table table = new Table(50, 2);
table.BuildTable(users);
```

Constructor args:
- Width: span of the table across the console
- Indent: A spacing between the console window border and the bounds of the grid - purely aesthetic.

Which produces:
```
       Name      |    Surname    |      Age      |
  -------------------------------------------------
       Odus      |     Labs      |      18       |
  -------------------------------------------------
       Test      |     Case      |      22       |
  -------------------------------------------------
```

As you can see, it fetches all public properties' names and values used to populate the table.
