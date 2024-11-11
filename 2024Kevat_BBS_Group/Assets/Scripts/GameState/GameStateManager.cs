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
            if(currentSaveGame.inventory.healthBoosts == null) currentSaveGame.inventory.healthBoosts = new List<HealthBoost>();
            if(currentSaveGame.inventory.speedBoosts == null) currentSaveGame.inventory.speedBoosts = new List<SpeedBoost>();
            
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
            
            var modifiers = new List<IMapModifier>();
            for (var i = 0; i < random.NextInt(1, 5); i++)
            {
                var value = random.NextFloat();
                if (value < 0.2f)
                {
                    if (modifiers.OfType<GoldAmountMapModifier>().Any()) continue;
                    modifiers.Add(new GoldAmountMapModifier(random.NextFloat(2)));
                }
                else
                {
                    var types = VariantManager.Instance.TerrainObstacles.Values;
                    var type = types.ElementAt(random.NextInt(types.Count));
                    if (modifiers.OfType<ObstacleCountMapModifier>().Any(it => it.obstacleType == type.id)) continue;
                    modifiers.Add(new ObstacleCountMapModifier(type.id, random.NextFloat(2)));
                }
            }

            var goalChooser = random.NextFloat();
            IMapGoal goal;
            if (goalChooser < 0.5f)
            {
                goal = new KillAntsMapGoal { amount = random.NextInt(2, 5) * 10 };
            }
            else
            {
                goal = new SurviveWavesMapGoal { amount = random.NextInt(5, 10) };
            }

            return new MapStats
            {
                modifiers = modifiers,
                goal = goal,
                seed = random.NextInt()
            };
        }
    }
}