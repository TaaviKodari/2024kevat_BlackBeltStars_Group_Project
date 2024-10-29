using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class FadeOverlayTransition : SceneTransition
{
    private CanvasGroup group;
    [SerializeField]
    private float fadeDuration = 0.5f;
    [SerializeField, CanBeNull]
    private GameObject loadingText;

    private void Start()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0;
        if (loadingText != null)
        {
            loadingText.SetActive(false);
        }
    }

    public override void LoadScene(string scene)
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(Fade(scene));
    }

    private IEnumerator Fade(string sceneName)
    {
        Time.timeScale = 0;
        
        var fadeInStart = Time.unscaledTime;
        while (fadeInStart + fadeDuration / 2 > Time.unscaledTime)
        {
            group.alpha = Mathf.Lerp(0, 1, (Time.unscaledTime - fadeInStart) / fadeDuration * 2);
            yield return null;
        }
        group.alpha = 1;
        
        var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        if (operation == null)
        {
            Debug.LogError("Fade failed: cannot load requested scene");
            group.alpha = 0;
            yield break;
        }

        while (!operation.isDone)
        {
            if (fadeInStart + fadeDuration < Time.unscaledTime && loadingText != null)
            {
                loadingText.SetActive(true);
            }
            yield return null;
        }

        var fadeOutStart = Time.unscaledTime;
        while (fadeOutStart + fadeDuration / 2 > Time.unscaledTime)
        {
            group.alpha = Mathf.Lerp(1, 0, (Time.unscaledTime - fadeOutStart) / fadeDuration * 2);
            yield return null;
        }
        group.alpha = 0;

        Time.timeScale = 1;
        
        Destroy(gameObject);
    }
}
