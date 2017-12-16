# Re-creating `grep` with IronRure

This tutorial demonstrates simple use of the `IronRure` regular expression package. We will be attempting to re-create part of the functionality of the popular `grep` command line program.

    ourgrep <pattern> <file>...

Our program will accept a regular expression and list of file names from the command line. For each file it prints out the lines which match the regular expression.

## Using this Tutorial

Throughout this tutorial I will be demonstrating how to create a simple program using the [`dotnet` toolchain](https://www.microsoft.com/net/learn/get-started). Throughout I will use `$ foo` to represent commands which should be from the command line. To follow this tutorial you should make sure you have access to:

 * A terminal or console from which you can run the `dotnet` command line too.
 * A text editor.

You should be able to follow this tutorial on Windows, macOS, and Linux.

## Creating the Project

The first step in creating a new project is to set up a blank folder to work in. Begin by creating a new folder, either from the command line or through your operating system's file browser. Once you have a new clean directory open a command line, change to the folder, and create a new .NET Core console project:

    $ dotnet new console

Once `dotnet` completes you should have a folder structure something like this:

```
.
├── Program.cs
├── grep.csproj
└── obj/
```

Next we need to add a reference to `IronRure` to the project. `IronRure` is available on [NuGet](https://www.nuget.org/packages/IronRure/), so we can install it with `dotnet add package`:

    $ dotnet add package IronRure

If all goes well you should see some information about the package being downloaded. To check that the package was installed correctly you can open the `.csproj` file in your project folder and look for a line similar to `<PackageReference Include="IronRure" Version="0.1.7" />`.

## Using IronRure

Now we have the project structure created, and a reference to IronRure added it's time to use it. Begin by adding the following line to the start of `Program.cs`:

```csharp
using IronRure
```

This brings all of the useful IronRure classes, such as `Regex` into scope. Next replace the body of the `Main` method with the following line:

```csharp
var reg = new Regex(args[0]);
```

This will take the first command line parameter and compile a regular expression object from it. Test this out by running the program from the command line:

    $ dotnet run -- simple

When using `dotnet run` anything after the `--` is passed to the CLI application `Main` method. If all goes well the program should compile and run successfully, producing no output.

Next we will read the contents of any files specified on the command line and find out which parts match our regex. To do this we're going to need some new using statements. Add the following to the beginning of the file:

```csharp
using System.IO;
using System.Linq;
```

Finally in the body of the main method add the following `foreach` loops:

```csharp
foreach (var path in args.Skip(1))
{
    int lineNo = 1;
    foreach (var line in File.ReadLines(path))
    {
        if (reg.IsMatch(line))
        {
            Console.WriteLine("{0}:{1}:{2}", path, lineNo, line);
        }
        lineNo++;
    }
}
```

The interesting part here is the call to `reg.IsMMatch`. This method just checks if the regex pattern matches somewhere in the text, without returning _where_ it matched. By not having to keep track of the location of the match this provides the best speed of matching. If the expression _does_ match the line we write out a line, along with the file and line location.

To test this out, and make use of `IronRure` for the first time let's use our new program to find all the `var` declarations in it's source code:

    $ dotnet run -- '\bvar\b' Program.cs

You should see output similar to this:

    Program.cs:12:            var reg = new Regex(args[0]);
    Program.cs:13:            foreach (var path in args.Skip(1))
    Program.cs:16:                foreach (var line in File.ReadLines(path))

That's it! If you've been following along you've just written your first C# program using the `IronRure` regular expression package.

## Example Source Code

The source code for this example can be found [next to this file GitHub](./).
