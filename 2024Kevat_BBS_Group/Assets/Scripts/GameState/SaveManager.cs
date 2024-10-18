using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameState
{
    public static class SaveManager
    {
        public static void SaveGame(SaveGame saveGame)
        {
            var path = $"saves/{saveGame.SaveName}.json";
            Directory.CreateDirectory("saves");

            var writer = File.CreateText(path);
            writer.Write(JsonUtility.ToJson(saveGame));
            writer.Close();
        }

        public static List<SaveGame> LoadGames()
        {
            var saves = new List<SaveGame>();
            if (Directory.Exists("saves"))
            {
                foreach (var saveFile in Directory.EnumerateFiles("saves", "*.json", SearchOption.AllDirectories))
                {
                    var text = File.ReadAllText(saveFile);
                    var save = JsonUtility.FromJson<SaveGame>(text);
                    save.SaveName = saveFile.Remove(saveFile.Length - 5).Remove(0, 6);
                    saves.Add(save);
                }
            }

            return saves;
        }
    }
}