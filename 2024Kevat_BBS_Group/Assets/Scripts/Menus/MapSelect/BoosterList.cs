using System;
using System.Collections.Generic;
using System.Linq;
using GameState;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MapSelect
{
    public class BoosterList : MonoBehaviour
    {
        [SerializeField]
        private Image template;
        [SerializeField]
        private TMP_Text label;

        private GameStateManager manager;
        private readonly List<Image> instances = new();

        private void Start()
        {
            manager = FindObjectOfType<GameStateManager>();
        }

        private void Update()
        {
            var boosters = manager.currentSaveGame.boosters;
            if (boosters == null || boosters.Count == 0)
            {
                foreach (var instance in instances)
                {
                    instance.gameObject.SetActive(false);
                }
                label.gameObject.SetActive(false);
                return;
            }

            label.gameObject.SetActive(true);

            if (instances.Count < boosters.Count)
            {
                var toAdd = boosters.Count - instances.Count;
                for (var i = 0; i < toAdd; i++)
                {
                    instances.Add(Instantiate(template, transform));
                }
            }

            for (var i = 0; i < boosters.Count; i++)
            {
                instances[i].gameObject.SetActive(true);
                instances[i].sprite = boosters[i].booster.Sprite;
            }

            for (var i = boosters.Count; i < instances.Count; i++)
            {
                instances[i].gameObject.SetActive(false);
            }
        }
    }
}