using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    [RequireComponent(typeof(Button))]
    public class SaveSelectButton : MonoBehaviour
    {
        internal LoadGameDialog Dialog;
        internal string SaveName;
        internal int Index;

        private void Start()
        {
            var button = GetComponent<Button>();
            button.GetComponentInChildren<TMP_Text>().text = SaveName;
            button.onClick.AddListener(() => Dialog.SelectSave(Index));
        }
    }
}