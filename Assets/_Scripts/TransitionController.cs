using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class TransitionController : MonoBehaviour
{
    internal VideoPlayer player;
    public CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        player = GetComponent<VideoPlayer>();
        player.time = 0;
        player.targetTexture.DiscardContents();
    }
    public void PlayTransition()
    {
        player.time = 0;
        player.Play();
        StartCoroutine(nameof(FadeInCoroutine));
    }
    IEnumerator FadeInCoroutine()
    {
        for (int i = 0; i < 5; i++)
            yield return new WaitForEndOfFrame();
        canvasGroup.alpha = 1;
    }
    public void FadeOut()
    {
        StartCoroutine(nameof(FadeOutCoroutine));
    }
    public IEnumerator FadeOutCoroutine()
    {
        for (int i = 0; i < 5; i++)
            yield return new WaitForEndOfFrame();
        canvasGroup.alpha = 0;
    }
}