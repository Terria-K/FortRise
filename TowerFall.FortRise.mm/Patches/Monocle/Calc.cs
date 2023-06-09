using System;
using System.Xml;
using System.Reflection;
using MonoMod;
using FortRise;
using TowerFall;

namespace Monocle;

public static class patch_Calc 
{
    [MonoModReplace]
    public static T StringToEnum<T>(string str) where T : struct 
    {
        if (Enum.IsDefined(typeof(T), str)) 
        {
            return (T)((object)Enum.Parse(typeof(T), str));
        }
        // Try to get the pickup value
        else if (RiseCore.PickupID.TryGetValue(str, out var s) && s is T custmPickup) 
        {
            return custmPickup;
        }
        throw new Exception("The string cannot be converted to the enum type");
    }

    [MonoModReplace]
    public static Delegate GetMethod<T>(object obj, string method) where T : class 
    {
        if (obj.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null)
        {
            return Delegate.CreateDelegate(typeof(T), obj, method);
        }
        if (obj.GetType().BaseType
            .GetMethod(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null)
        {
            return Delegate.CreateDelegate(typeof(T), obj, method);
        }
        return null;
    }

    public static T[] ChildEnumArray<T>(this XmlElement xml, string childName)
    where T : struct
    {
        var childs = xml[childName].InnerText.Split(',');
        if (childs == null)
            return Array.Empty<T>();
        var array = new T[childs.Length];
        int i = 0;
        foreach (var child in childs) 
        {
            if (System.Enum.TryParse<T>(child, out T result)) 
            {
                array[i] = result;
            }
            i++;
        }
        return array;
    }

    public static string[] ChildStringArray(this XmlElement xml, string childName) 
    {
        if (!xml.HasChild(childName))
            return null;
        var childs = xml[childName].InnerText.Split(',');
        if (childs == null)
            return Array.Empty<string>();
        
        var array = new string[childs.Length];
        for (int i = 0; i < childs.Length; i++) 
        {
            array[i] = childs[i];
        }
        return array;
    }

    public static void IncompatibleWith(this Variant variant, Variant targetVariant) 
    {
        variant.Links.Add(targetVariant);
        targetVariant.Links.Add(variant);
    }

    public static void IncompatibleWith(this Variant variant, params Variant[] variants) 
    {
        foreach (var varia in variants) 
        {
            variant.Links.Add(varia);
            varia.Links.Add(variant);
        }
    }

    [MonoModReplace]
    public static void Log(params object[] obj) 
    {
        if (!RiseCore.DebugMode)
            return;

        foreach (object obj2 in obj)
        {
            Logger.Log(obj2);
        }
    }
}