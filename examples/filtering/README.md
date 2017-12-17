# Filtering User Input with `RegexSet`

In this tutorial we will be creating a program which uses IronRure's `RegexSet` to filter lines of text to remove 'bad' words. The `RegexSet` provides an optimised way of finding which of a large group of expressions match a given piece of text. Just what we want!

## Using this Tutorial

Throughout this tutorial I will be demonstrating how to create a simple program using the [`dotnet` toolchain](https://www.microsoft.com/net/learn/get-started). Throughout I will use `$ foo` to represent commands which should be from the command line. To follow this tutorial you should make sure you have access to:

 * A terminal or console from which you can run the `dotnet` command line too.
 * A text editor.

You should be able to follow this tutorial on Windows, macOS, and Linux.

## Creating the Project

Let's begin by creating a new empty console application with `dotnet`. Open a terminal or console and run:

    $ dotnet new console --name filtering

This should create a new directory called `filtering/` that looks something like this:

    filtering
    ├── Program.cs
    ├── README.md
    └── filtering.csproj

Make sure everything is working by changing into this directory in your terminal and running the project:

    $ dotnet run

You should see the familiar `Hello world!` printed out.

## Adding IronRure

IronRure is available on [NuGet](https://nuget.org/packages/ironrure). The simplest way to install it is with the `dotnet add package` command:

    $ dotnet add package IronRure

To bring the code within this package into scope add the following to the beginning of `Program.cs`:

```csharp
using System.Linq;
using IronRure;
```

## Creating the `RegexSet`

For this example we will be filtering out three colours: "red", "green" and "blue". Add the following declaration to the beginning of the `Program` class, just after the `static void Main` line:

```csharp
private static readonly string[] BadWords = new[] { "red", "green","blue" };
```

Next we will create two sets of `IronRure` regular expressions from these. Replace the contents of the of the `Main` method with these variable declarations:

```csharp
var set = new RegexSet(BadWords);
var expressions = BadWords.Select(p => new Regex(p)).ToList();
```

The first line declares a `RegexSet`. This will search for each of the `badWords` in a single pass and should be super fast. The `expressions` list contains a standard `Regex` for each pattern. We'll use this to find out exactly _where_ in each line the bad words are.

## Checking if the Set Matches

To filter each line we will begin by only echoing lines to the console if they don't contain any of our `BadWords`. If a line does contain one of the `BadWords` the line contents are replaced by asterisk (`*`) characters.

After the `expression` declaration in the `Main` method add the following `while` loop:

```csharp
string line;
while ((line = Console.ReadLine()) != null)
{
    if (set.IsMatch(line))
    {
        Console.WriteLine(new String('*', line.Length));
    }
    else
    {
        Console.WriteLine(line);
    }
}
```

This loops through each line of standard input and checks if it matches any of our "bad word" patterns (`set.Ismatch`). If the pattern does match the contents will be replaced with a new `String` with the same number of `*`.

You should now be able to test this out. Run the program and type some text in. Each time you press enter the line should be echoed back to you. Try typing lines which do and don't contain  `BadWords`:

    $ dotnet run
    hello world
    hello world
    red things
    **********

To stop press `C-d` (control + d) on macOS and Linux or `C-z` (control + z) on Windows.

