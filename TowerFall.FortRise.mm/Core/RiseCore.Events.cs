using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FortRise;

public static partial class RiseCore 
{
    public static partial class Events 
    {
        public static event Action OnLevelLoaded;
        internal static void Invoke_OnLevelLoaded()  
        {
            OnLevelLoaded?.Invoke();
        }
        public static event Action OnLevelEntered;
        internal static void Invoke_OnLevelEntered()  
        {
            OnLevelEntered?.Invoke();
        }

        public static event Action OnLevelExited;
        internal static void Invoke_OnLevelExited()  
        {
            OnLevelExited?.Invoke();
        }

        public static event Action OnBeforeDataLoad;
        internal static void Invoke_OnBeforeDataLoad()  
        {
            OnBeforeDataLoad?.Invoke();
        }

        public static event Action OnAfterDataLoad;
        internal static void Invoke_OnAfterDataLoad()  
        {
            OnAfterDataLoad?.Invoke();
        }

        public static event Action OnPreInitialize;
        public static event Action OnPostInitialize;
        internal static void Invoke_OnPreInitialize() 
        {
            OnPreInitialize?.Invoke();
        }

        internal static void Invoke_OnPostInitialize() 
        {
            OnPostInitialize?.Invoke();
        }

        public static event Action<GameTime> OnBeforeUpdate;
        internal static void Invoke_BeforeUpdate(GameTime gameTime) 
        {
            OnBeforeUpdate?.Invoke(gameTime);
        }
        public static event Action<GameTime> OnUpdate;
        internal static void Invoke_Update(GameTime gameTime) 
        {
            OnUpdate?.Invoke(gameTime);
        }
        public static event Action<GameTime> OnAfterUpdate;
        internal static void Invoke_AfterUpdate(GameTime gameTime) 
        {
            OnAfterUpdate?.Invoke(gameTime);
        }

        public static event Action<SpriteBatch> OnBeforeRender;
        internal static void Invoke_BeforeRender(SpriteBatch spriteBatch) 
        {
            OnBeforeRender?.Invoke(spriteBatch);
        }
        public static event Action<SpriteBatch> OnRender;
        internal static void Invoke_Render(SpriteBatch spriteBatch) 
        {
            OnRender?.Invoke(spriteBatch);
        }
        public static event Action<SpriteBatch> OnAfterRender; 
        internal static void Invoke_AfterRender(SpriteBatch spriteBatch) 
        {
            OnAfterRender?.Invoke(spriteBatch);
        }
    }
}