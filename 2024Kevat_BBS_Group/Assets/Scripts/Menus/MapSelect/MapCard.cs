using System.Text;
using GameState;
using TMPro;
using UnityEngine;

namespace MapSelect
{
    public class MapCard : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private TMP_Text stats;

        public void Populate(MapStats mapStats)
        {
            var textBuilder = new StringBuilder();
            foreach (var modifier in mapStats.modifiers)
            {
                modifier.Describe(textBuilder);
                textBuilder.Append('\n');
            }

            stats.text = textBuilder.ToString();
        }
    }
}