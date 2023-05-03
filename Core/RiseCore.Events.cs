using Microsoft.Xna.Framework;
using Monocle;
using TowerFall;

namespace FortRise;

public static partial class RiseCore 
{
    public delegate bool QuestSpawnPortal_FinishSpawnHandler(string entityName, Vector2 position, Facing facing, Level level);

    public static event QuestSpawnPortal_FinishSpawnHandler OnQuestSpawnPortal_FinishSpawn;

    internal static void InvokeQuestSpawnPortal_FinishSpawn(string name, Vector2 position, Facing facing, Level level) 
    {
        if (Loader.TryGetValue(name, out EnemyLoader loader)) 
        {
            level.Add(loader?.Invoke(position + new Vector2(0f, 2f), facing));
        }
        if (name.Contains("Skeleton") || name.Contains("Jester")) 
        {
            ArrowTypes arrows = ArrowTypes.Normal;
            bool hasShields = false;
            bool hasWings = false;
            bool canMimic = false;
            bool jester = false;
            bool boss = false;

            if (name.EndsWith("S"))
                hasShields = true;

            if (name.Contains("Wing"))
                hasWings = true;

            if (name.Contains("Mimic"))
                canMimic = true;

            if (name.Contains("Boss"))
                boss = true;
            
            if (name.Contains("Jester"))
                jester = true;
            
            arrows = GetArrowTypes(name);
            level.Add(new Skeleton(position + new Vector2(0f, 2f), facing, arrows, hasShields, hasWings, canMimic, jester, boss));
            return;
        }
        var invoked = OnQuestSpawnPortal_FinishSpawn?.Invoke(name, position, facing, level);
        if (invoked == null || !invoked.Value)
        {
            Engine.Instance.Commands.Log($"Entity name: {name} failed to spawn as it does not exists!");
        }

        ArrowTypes GetArrowTypes(string name) 
        {
            ArrowTypes types = ArrowTypes.Normal;
            if (name.Contains("Bomb"))
                types = ArrowTypes.Bomb;
            else if (name.Contains("SuperBomb"))
                types = ArrowTypes.SuperBomb;
            else if (name.Contains("Bramble"))
                types = ArrowTypes.Bramble;
            else if (name.Contains("Drill"))
                types = ArrowTypes.Drill;
            else if (name.Contains("Trigger"))
                types = ArrowTypes.Trigger;
            else if (name.Contains("Toy"))
                types = ArrowTypes.Toy;
            else if (name.Contains("Feather"))
                types = ArrowTypes.Feather;
            else if (name.Contains("Laser"))
                types = ArrowTypes.Laser;
            else if (name.Contains("Prism"))
                types = ArrowTypes.Prism;
            return types;
        }
    }

    public delegate void DarkWorldComplete_ResultHandler(
        int levelID, DarkWorldDifficulties difficulties,
        int playerAmount, long time, int continues, int deaths, int curses);

    public static event DarkWorldComplete_ResultHandler OnDarkWorldComplete_Result;

    internal static void InvokeDarkWorldComplete_Result(
        int levelID, DarkWorldDifficulties difficulties,
        int playerAmount, long time, int continues, int deaths, int curses) 
    {
        if (patch_SaveData.AdventureActive)
        {
            patch_GameData.AdventureWorldTowers[levelID].Stats.Complete(
                difficulties, playerAmount, time,
                continues, deaths, curses);
            return;
        }
        SaveData.Instance.DarkWorld.Towers[levelID].Complete(
            difficulties, playerAmount, time, continues, deaths, curses
        );
        OnDarkWorldComplete_Result?.Invoke(levelID, difficulties, playerAmount, time, continues, deaths, curses);
    }
}