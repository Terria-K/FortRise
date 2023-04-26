using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using Monocle;
using TowerFall;

namespace FortRise;

public static class RiseCore 
{
    private static List<Assembly> ModAssemblies = new List<Assembly>();

    private static Type[] Types;

    public readonly static Type[] EmptyTypeArray = new Type[0];
    public readonly static object[] EmptyObjectArray = new object[0];

    internal static void Register(this RiseModule module) 
    {
        foreach (var type in module.GetType().Assembly.GetTypes()) 
        {
            if (type is null)
                continue;

            foreach (EnemyAttribute attrib in type.GetCustomAttributes<EnemyAttribute>()) 
            {
                if (attrib is null)
                    continue;
                var name = attrib.Name;
                var arg = attrib.FuncArg;
                EnemyDataArg dataArg = (EnemyDataArg)
                    type.GetMethod(arg).Invoke(null, Array.Empty<object>());

                ConstructorInfo ctor;
                EnemyLoader loader = null;

                ctor = type.GetConstructor(
                    new Type[] { typeof(Vector2), typeof(Facing), typeof(int), typeof(int), typeof(int), typeof(ArrowTypes) }
                );
                if (ctor != null) 
                {
                    loader = (position, facing) => {
                        var invoked = (patch_Enemy)ctor.Invoke(new object[] {
                            position,
                            facing,
                            dataArg.states,
                            dataArg.health,
                            dataArg.bounty,
                            dataArg.arrows
                        });
                        invoked.Load(dataArg);
                        return invoked;
                    };
                    goto Loaded;
                }
                ctor = type.GetConstructor(
                    new Type[] { typeof(Vector2), typeof(Facing), typeof(Slime.SlimeColors) }
                );
                if (ctor != null) 
                {
                    loader = (position, facing) => {
                        var invoked = (patch_Enemy)ctor.Invoke(new object[] {
                            position,
                            facing,
                            Slime.SlimeColors.Red
                        });
                        invoked.Load(dataArg);
                        return invoked;
                    };
                    goto Loaded;
                }
                Loaded:
                patch_QuestSpawnPortal.Loader.Add(name, loader);
            }
        }
    }

    internal static void ModuleStart() 
    {
        if (!File.Exists("Mods/ModsList.txt")) 
        {
            using var x = File.CreateText("Mods/ModsList.txt");
            x.Write("");
        }
        var allLines = File.ReadAllLines("Mods/ModsList.txt");
        if (allLines.Length == 0) 
        {
            Types = EmptyTypeArray;
        }
        else 
        {
            Types = new Type[allLines.Length];
            for (int i = 0; i < allLines.Length; i++) 
            {
                if (allLines[i] == string.Empty) { continue; }
                if (!File.Exists(Path.GetFullPath("Mods/" + allLines[i])))
                    continue;
                Assembly asm = Assembly.LoadFile(Path.GetFullPath("Mods/" + allLines[i]));
                ModAssemblies.Add(asm);
                Type t = asm.GetType("Entry");
                Types[i] = t;
                RiseModule obj = (RiseModule)Activator.CreateInstance(t);
                obj.Register();
                var method = t.GetMethod("Load");
                method.Invoke(obj, null);
            }
        }
    }

    internal static void LogAllTypes() 
    {
        Commands commands = Engine.Instance.Commands;
        int i = 0;
        foreach (var t in Types) 
        {
            if (t is null)
                continue;
            commands.Log(t.Assembly.FullName);
            i++;
        }
        commands.Log($"{i} total of mods loaded");
    }

    internal static void LoadContent() 
    {
        foreach (var t in Types) 
        {
            if (t is null)
                continue;
            var obj = Activator.CreateInstance(t);
            var method = t.GetMethod("LoadContent");
            method.Invoke(obj, null);
        }
    }

    internal static void Initialize() 
    {
        foreach (var t in Types) 
        {
            if (t is null)
                continue;
            var obj = Activator.CreateInstance(t);
            var method = t.GetMethod("Initialize");
            method.Invoke(obj, null);
        }
    }

    internal static void ModuleEnd() 
    {
        foreach (var t in Types) 
        {
            if (t is null)
                continue;
            var obj = Activator.CreateInstance(t);
            var method = t.GetMethod("Unload");
            method.Invoke(obj, null);
        }
    }
}

// Work In Progress
public class ModuleHandler
{
    public Assembly ModuleAssembly;
    public object Instance;
    private Type type;

    public ModuleHandler(Assembly assembly, object instance)  
    {
        ModuleAssembly = assembly;
        Instance = instance;
        type = ModuleAssembly.GetType("Entry");
    }

    public void InvokeMethod(string methodName) 
    {
        var method = type.GetMethod(methodName);
        method.Invoke(Instance, null);
    }
}