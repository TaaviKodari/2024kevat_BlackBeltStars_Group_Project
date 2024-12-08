using System;
using System.Collections.Generic;
using System.Reflection;

namespace AtomicAssembly.GeneratedCommands
{
    public static class AtomicCommands
    {
        public static List<MethodInfo> commandMethods = new()
        {
            Type.GetType("PlayerController, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("HealCommandCallback"),
            Type.GetType("ResourceManager, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("IncrementResources"),
            Type.GetType("Portal.PortalManager, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("SpawnPortal"),
            Type.GetType("GameState.InGameManager, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("EndGame"),
            Type.GetType("GameState.InGameManager, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("PrepareNextRound"),
            Type.GetType("GameState.InGameManager, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("RollBackGame"),
            Type.GetType("GameState.InGameManager, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("AbandonGame"),
            Type.GetType("AtomicConsole.Engine.AtomicConsoleEngine, AtomicConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("Close"),
            Type.GetType("AtomicConsole.Engine.AtomicConsoleEngine, AtomicConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("ToggleFPS"),
            Type.GetType("AtomicConsole.Engine.AtomicConsoleEngine, AtomicConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("ProjectInfo"),
            Type.GetType("AtomicConsole.Engine.AtomicConsoleEngine, AtomicConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("Clear"),
            Type.GetType("AtomicConsole.Engine.AtomicConsoleEngine, AtomicConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("Quit"),
            Type.GetType("AtomicConsole.Engine.AtomicConsoleEngine, AtomicConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("ToggleWireframe"),
            Type.GetType("AtomicConsole.Engine.AtomicConsoleEngine, AtomicConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("ToggleCulling"),
            Type.GetType("AtomicConsole.Engine.AtomicConsoleEngine, AtomicConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("screenshot"),
            Type.GetType("AtomicConsole.Engine.AtomicConsoleEngine, AtomicConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("Help"),
            Type.GetType("AtomicConsole.Engine.AtomicConsoleEngine, AtomicConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("Quality"),
            Type.GetType("AtomicConsole.Engine.AtomicConsoleEngine, AtomicConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("VSync"),
            Type.GetType("AtomicConsole.Engine.AtomicConsoleEngine, AtomicConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("TimeScale"),
            Type.GetType("AtomicConsole.Engine.AtomicConsoleEngine, AtomicConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("FrameRate"),
            Type.GetType("AtomicConsole.Engine.AtomicConsoleEngine, AtomicConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetMethod("FullScreen")
        };

        public static List<FieldInfo> setFields = new()
        {
            Type.GetType("ResourceManager, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetField("resourceMultiplier"),
            Type.GetType("WaveManager, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", true).GetField("currentWave")
        };
    }
}