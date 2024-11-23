using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TooltipGenerator : MonoBehaviour
{
    [SerializeField]
    private BuildingData buildingData;

    private TMP_Text[] tmpTexts;

    private void Awake()
    {
        tmpTexts = GetComponentsInChildren<TMP_Text>();
    }

    private void Update()
    {
        if (tmpTexts.Length > 1)
        {
            tmpTexts[1].text = FormatBuildingStats(buildingData);
        }
    }

    private string FormatBuildingStats(BuildingData building)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Durability: {building.durability}");

        sb.AppendLine("Costs:");
        foreach (var cost in building.costs)
        {
            sb.AppendLine($"{cost.amount}x {cost.type.id}");
        }

        return sb.ToString();
    }
}
