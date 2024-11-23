using GameState;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapGoalUI : MonoBehaviour
{
    [SerializeField]
    private Slider goalSlider;
    [SerializeField]
    private TMP_Text text;

    private GameStateManager manager;

    private void Start()
    {
        manager = FindObjectOfType<GameStateManager>();
    }

    private void Update()
    {
        var progress = InGameManager.Instance.GetGoalProgress();
        var target = InGameManager.Instance.GetGoalTarget();
        goalSlider.value = progress / (float)target;

        var currentGoal = manager.currentMap.goal;
        if (progress >= target)
        {
            text.text = "Goal complete: Exit when ready";
        }
        else if (currentGoal == null)
        {
            text.text = "Goal not set";
        }
        else
        {
            text.text = $"Current goal: {currentGoal.Describe()} ({progress}/{target})";
        }
    }
}