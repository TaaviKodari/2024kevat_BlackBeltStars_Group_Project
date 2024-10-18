using System.Linq;
using System.Text;
using GameState;
using TMPro;
using UnityEngine;

namespace MapSelect
{
    // Manages a single map card in the map select scene
    public class MapCard : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private TMP_Text stats;

        public void Populate(MapStats mapStats)
        {
            var textBuilder = new StringBuilder();
            foreach (var modifier in mapStats.modifiers ?? Enumerable.Empty<IMapModifier>())
            {
                modifier.Describe(textBuilder);
                textBuilder.Append('\n');
            }

            stats.text = textBuilder.ToString();
            title.text = mapStats.goal?.Describe() ?? "<color:#ff0000>Missing goal";
        }
    }
}