using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace GameState
{
    // Stores the current savegame and map
    // Is placed in a DontDestroyOnLoad object
    public class GameStateManager : MonoBehaviour
    {
        public SaveGame currentSaveGame;
        public MapStats currentMap;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void NewGame(string saveName)
        {
            currentSaveGame = default;
            currentMap = default;
            currentSaveGame.SaveName = saveName;
        }

        public void LoadGame(SaveGame game)
        {
            currentSaveGame = game;
            currentSaveGame.boosters ??= new List<BoosterInstance>();
            currentSaveGame.shopItems ??= new List<ShopItemConfig>();
            currentSaveGame.boughtItems ??= new List<bool>();
        }
        
        public void Save()
        {
            SaveManager.SaveGame(currentSaveGame);
        }

        public void GenerateMaps()
        {
            currentSaveGame.maps.map1 = GenerateMap();
            currentSaveGame.maps.map2 = GenerateMap();
            currentSaveGame.maps.map3 = GenerateMap();
            Save();
        }

        private static MapStats GenerateMap()
        {
            var random = new Random((uint) UnityEngine.Random.Range(int.MinValue, int.MaxValue));

            var difficulty = 0f;

            var modifiers = new List<IMapModifier>();
            for (var i = 0; i < random.NextInt(1, 5); i++)
            {
                var value = random.NextFloat();
                if (value < 0.2f)
                {
                    if (modifiers.OfType<GoldAmountMapModifier>().Any()) continue;
                    var amount = random.NextFloat(2);
                    modifiers.Add(new GoldAmountMapModifier(amount));
                    difficulty -= (amount - 1) / 2;
                }
                else
                {
                    var types = VariantManager.Instance.TerrainObstacles.Values;
                    var type = types.ElementAt(random.NextInt(types.Count));
                    if (modifiers.OfType<ObstacleCountMapModifier>().Any(it => it.obstacleType == type.id)) continue;
                    var amount = random.NextFloat(2);
                    modifiers.Add(new ObstacleCountMapModifier(type.id, amount));
                    difficulty -= (amount - 1) * 4;
                }
            }

            var goalChooser = random.NextFloat();
            IMapGoal goal;
            if (goalChooser < 0.5f)
            {
                var amount = random.NextInt(2, 5) * 10;
                goal = new KillAntsMapGoal { amount = amount };
                difficulty += (amount - 30) / 2f;
            }
            else
            {
                var amount = random.NextInt(5, 10);
                goal = new SurviveWavesMapGoal { amount = amount };
                difficulty += (amount - 7) * 2;
            }

            return new MapStats
            {
                modifiers = modifiers,
                goal = goal,
                seed = random.NextInt(),
                diamondCount = (int) Math.Round(difficulty + 20)
            };
        }
    }
}