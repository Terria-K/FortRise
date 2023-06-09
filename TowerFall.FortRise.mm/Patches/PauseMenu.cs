using Microsoft.Xna.Framework;

namespace TowerFall;

public class patch_PauseMenu : PauseMenu
{
    private PauseMenu.MenuType menuType;
    private Level level;


    public patch_PauseMenu(Level level, Vector2 position, MenuType menuType, int controllerDisconnected = -1) : base(level, position, menuType, controllerDisconnected)
    {
    }

    private extern void orig_DarkWorldMap();

    private void DarkWorldMap() 
    {
        patch_DarkWorldControl.DisableTempVariants(level);
        patch_SaveData.AdventureActive = false;
        orig_DarkWorldMap();       
    }

    private extern void orig_DarkWorldMapAndSave();

    private void DarkWorldMapAndSave() 
    {
        patch_DarkWorldControl.DisableTempVariants(level);
        patch_SaveData.AdventureActive = false;
        orig_DarkWorldMapAndSave();       
    }

    private extern void orig_Quit();

    private void Quit() 
    {
        patch_DarkWorldControl.DisableTempVariants(level);
        if (menuType == MenuType.DarkWorldPause || 
        menuType == MenuType.DarkWorldComplete || 
        menuType == MenuType.DarkWorldGameOver)
        {
            patch_SaveData.AdventureActive = false;
        }
        orig_Quit();
    }

    private extern void orig_QuitAndSave();

    public void QuitAndSave() 
    {
        patch_DarkWorldControl.DisableTempVariants(level);
        if (menuType == MenuType.DarkWorldPause || 
        menuType == MenuType.DarkWorldComplete || 
        menuType == MenuType.DarkWorldGameOver) 
        {
            patch_SaveData.AdventureActive = false;
        }
        orig_QuitAndSave();
    }
}