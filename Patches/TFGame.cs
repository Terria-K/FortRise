// #pragma warning disable CS0626
// #pragma warning disable CS0108
// namespace TowerFall;

// public class patch_TFGame : TFGame
// {
//     public patch_TFGame(bool noIntro) : base(noIntro)
//     {
//     }

//     public extern static void orig_Main(string[] args) ;

//     public static void Main(string[] args) 
//     {
//         orig_Main(args);
//         FortRise.RiseCore.ModuleEnd();
//     }

//     protected extern void orig_Initialize();

//     protected override void Initialize() 
//     {
//         FortRise.RiseCore.ModuleStart();
//         FortRise.RiseCore.Initialize();
//         orig_Initialize();
//         FortRise.RiseCore.LogAllTypes();
//     }

//     protected extern void orig_LoadContent();

//     protected override void LoadContent()
//     {
//         FortRise.RiseCore.LoadContent();
//         orig_LoadContent();
//     }

//     protected extern void orig_UnloadContent();

//     protected override void UnloadContent()
//     {
//         orig_UnloadContent();
//     }
// }