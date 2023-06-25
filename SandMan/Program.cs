
using OpenTK.Windowing.Desktop;
using SandMan;

var gameSettings = GameWindowSettings.Default;
var nativeSettings = NativeWindowSettings.Default;

gameSettings.UpdateFrequency = 60;

Game game = new(gameSettings, nativeSettings);
game.Run();