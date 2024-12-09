using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameState
{
    public static class SaveManager
    {
        private static readonly string SavePath = $"{Application.persistentDataPath}/saves/";

        public static void SaveGame(SaveGame saveGame)
        {
            var path = $"{SavePath}{saveGame.SaveName}.json";
            Directory.CreateDirectory(SavePath.Remove(SavePath.LastIndexOf("/", StringComparison.Ordinal)));

            var writer = File.CreateText(path);
            writer.Write(JsonUtility.ToJson(saveGame));
            writer.Close();
        }

        public static List<SaveGame> LoadGames()
        {
            var saves = new List<SaveGame>();
            if (Directory.Exists(SavePath))
            {
                foreach (var saveFile in Directory.EnumerateFiles(SavePath, "*.json", SearchOption.AllDirectories))
                {
                    var text = File.ReadAllText(saveFile);
                    var save = JsonUtility.FromJson<SaveGame>(text);
                    save.SaveName = saveFile.Remove(saveFile.Length - 5).Remove(0, SavePath.Length);
                    save.name ??= save.SaveName; // Old version didn't save the save name
                    saves.Add(save);
                }
            }

            return saves;
        }
    }
}