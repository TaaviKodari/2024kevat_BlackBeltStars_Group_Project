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
        var fadeInStart = Time.time;
        while (fadeInStart + fadeDuration / 2 > Time.time)
        {
            group.alpha = Mathf.Lerp(0, 1, (Time.time - fadeInStart) / fadeDuration * 2);
            yield return null;
        }
        
        var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        if (operation == null)
        {
            Debug.LogError("Fade failed: cannot load requested scene");
            group.alpha = 0;
            yield break;
        }

        while (!operation.isDone)
        {
            if (fadeInStart + fadeDuration < Time.time && loadingText != null)
            {
                loadingText.SetActive(true);
            }
            yield return null;
        }

        var fadeOutStart = Time.time;
        while (fadeOutStart + fadeDuration / 2 > Time.time)
        {
            group.alpha = Mathf.Lerp(1, 0, (Time.time - fadeOutStart) / fadeDuration * 2);
            yield return null;
        }
        
        Destroy(this);
    }
}
