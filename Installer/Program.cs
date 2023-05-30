﻿using FortRise.Installer;
using System.Threading.Tasks;
using System.IO;
using System;
#if ANSI
using NativeFileDialogSharp;
using Spectre.Console;
#endif

internal class Program 
{
    public static string Version = "1.0.0";
    public static bool DebugMode = false;

    [STAThread]
    public async static Task Main(string[] args) 
    {
        Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        if (args.Length > 1) 
        {
            if (!File.Exists(args[1] + "/TowerFall.exe")) 
            {
                Console.WriteLine("TowerFall executable not found");
                return;
            }
            if (args[0] == "--patch") 
            {
                
                await Installer.Install(args[1]);
                return;
            }
            else if (args[0] == "--unpatch") 
            {
                await Installer.Uninstall(args[1]);
                return;
            }
            if (args.Length > 2 && args[3] == "--debug") 
            {
                DebugMode = true;
            }
        }

#if ANSI
        var panel = new Panel("FortRise Installer v" + Version) {
            Border = BoxBorder.Rounded,
            Padding = new Padding(2, 2, 2, 2),
            Expand = true
        };

        AnsiConsole.Write(panel);

        while (true) 
        {
            var stateSelection = AnsiConsole.Prompt(
                new SelectionPrompt<MenuState>()
                    .Title("[underline]Main Menu[/]")
                    .PageSize(10)
                    .AddChoices(new [] { MenuState.Patch, MenuState.Unpatch, MenuState.Quit})
            );

            switch (stateSelection) 
            {
            case MenuState.Patch:
                await StatePatch();
                break;
            case MenuState.Unpatch:
                await StateUnpatch();
                break;
            case MenuState.Quit:
                goto End;
            }
        }
        End:
        AnsiConsole.WriteLine("Goodbye!");
#endif
    }
#if ANSI
    public static async Task StateUnpatch() 
    {
        AnsiConsole.MarkupLine("Select a TowerFall directory to unpatch");
        var dialog = Dialog.FolderPicker();
        if (dialog.IsCancelled || dialog.IsError)  
        {
            AnsiConsole.MarkupLine("[red]Cancelled[/]");
            return;
        }
        var path = dialog.Path;
        AnsiConsole.WriteLine("Trying to find TowerFall executable in this directory.");
        if (!File.Exists(Path.Combine(path, "TowerFall.exe"))) 
        {
            AnsiConsole.MarkupLine("[underline][red]TowerFall not found! Aborting[/][/]");
            await Task.Delay(1000);
            return;
        }

        if (!AnsiConsole.Confirm($"""
        Are you sure you want to unpatch this directory?
        [yellow]{path}[/]
        """))
        {
            AnsiConsole.MarkupLine("[red]Cancelled[/]");
            return;
        }
        await Installer.Uninstall(path);
    }

    public static async Task StatePatch() 
    {
        AnsiConsole.MarkupLine("Select a TowerFall directory to patch");
        var dialog = Dialog.FolderPicker();
        if (dialog.IsCancelled || dialog.IsError)  
        {
            AnsiConsole.MarkupLine("[red]Cancelled[/]");
            return;
        }
        var path = dialog.Path;

        
        AnsiConsole.WriteLine("Trying to find TowerFall executable in this directory.");
        if (!File.Exists(Path.Combine(path, "TowerFall.exe"))) 
        {
            AnsiConsole.MarkupLine("[underline][red]TowerFall not found! Aborting[/][/]");
            await Task.Delay(1000);
            return;
        }
        AnsiConsole.MarkupLine("[underline][green]TowerFall found in this directory! [/][/]");
        await Task.Delay(1000);

        AnsiConsole.WriteLine("Checking if the TowerFall is from Steam");
        if (!File.Exists(Path.Combine(path, "Steamworks.NET.dll")))
            AnsiConsole.MarkupLine("[underline][green]TowerFall is pure[/][/]");
        else 
            AnsiConsole.MarkupLine("[underline][green]TowerFall is from Steam[/][/]");
        
        await Task.Delay(1000);


        if (!AnsiConsole.Confirm($"""
        Are you sure you want to patch this directory?
        [yellow]{path}[/]
        [gray]Make sure that this TowerFall has not been patched yet.[/]
        """))
        {
            AnsiConsole.MarkupLine("[red]Cancelled[/]");
            return;
        }
        await Installer.Install(path);
    }
#endif
}

#if ANSI
public enum MenuState 
{
    Patch,
    Unpatch,
    Quit
}
#endif