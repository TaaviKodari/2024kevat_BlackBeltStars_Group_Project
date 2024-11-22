using JetBrains.Annotations;
using Pause;
using UnityEngine;

namespace Overlay
{
    [RequireComponent(typeof(CanvasGroup), typeof(PauseMarker))]
    public class GameEndOverlay : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private bool fading;
        private float fadeTime;
        [SerializeField]
        private float fadeDuration;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (fading && fadeTime < fadeDuration)
            {
                fadeTime += Time.unscaledDeltaTime;
                canvasGroup.alpha = fadeTime / fadeDuration;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        [UsedImplicitly]
        public void StartFade()
        {
            fading = true;
            GetComponent<PauseMarker>().enabled = true;
        }
    }
}