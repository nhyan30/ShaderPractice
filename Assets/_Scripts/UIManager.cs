using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    [Header("City Images (Arabic / English)")]
    [Tooltip("Arabic images in order for each city (1–9)")]
    public List<CanvasGroup> arabicImages = new List<CanvasGroup>();

    [Tooltip("English images in order for each city (1–9)")]
    public List<CanvasGroup> englishImages = new List<CanvasGroup>();

    [Header("Settings")]
    public float displayTime = 3f; // Arabic and English display time
    private Coroutine currentTransitionCoroutine;
    public float transitionVideoDuration = 3f; // Transition video duration

    private bool isTransitioning = false;
    private Dictionary<KeyCode, int> keyMap = new Dictionary<KeyCode, int>();
    public VideoPlayer TransitionVideoPlayer;

    private void Awake()
    {
        SetupKeyMap();
        HideAllCities();
    }

    private void SetupKeyMap()
    {
        keyMap.Clear();

        keyMap.Add(KeyCode.A, 0);
        keyMap.Add(KeyCode.B, 1);
        keyMap.Add(KeyCode.C, 2);
        keyMap.Add(KeyCode.D, 3);
        keyMap.Add(KeyCode.E, 4);
        keyMap.Add(KeyCode.F, 5);
        keyMap.Add(KeyCode.G, 6);
        keyMap.Add(KeyCode.H, 7);
        keyMap.Add(KeyCode.I, 8);
    }

    void Update()
    {
        HandleKeyInput();
    }
    private void HandleKeyInput()
    {
        foreach (var key in keyMap.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                int cityIndex = keyMap[key];
                if (cityIndex < arabicImages.Count && cityIndex < englishImages.Count)
                {
                    // Stop any currently running transition first
                    if (currentTransitionCoroutine != null)
                    {
                        StopCoroutine(currentTransitionCoroutine);
                        ResetTransitionState();
                    }

                    currentTransitionCoroutine = StartCoroutine(PlayTransitionSequence(cityIndex));
                }
            }
        }
    }
    private IEnumerator PlayTransitionSequence(int cityIndex)
    {
        isTransitioning = true;

        // --- STEP 1: Play Normal Transition (BG → Arabic) ---
        Fade(arabicImages[cityIndex], true);

        TransitionVideoPlayer.Play();

        yield return new WaitForSeconds((float)TransitionVideoPlayer.clip.length);

        // --- STEP 2: Show Arabic image ---
        yield return new WaitForSeconds(displayTime);

        // --- STEP 3: Fade Arabic → English ---
        Fade(englishImages[cityIndex], true, () => Fade(arabicImages[cityIndex], false));
        yield return new WaitForSeconds(displayTime);


        yield return new WaitForSeconds(transitionVideoDuration);
        // --- STEP 5: Hide English ---
        Fade(englishImages[cityIndex], false);

        // --- STEP 6: Return to BG looping ---
        isTransitioning = false;
        currentTransitionCoroutine = null;
    }
    private void ResetTransitionState()
    {
        // Fade out everything and reset transitions
        HideAllCities();

        isTransitioning = false;
    }


    private void HideAllCities()
    {
        foreach (var img in arabicImages) Fade(img, false);
        foreach (var img in englishImages) Fade(img, false);
    }
    public void Fade(CanvasGroup canvasGroup, bool visible, UnityAction callback = null)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        canvasGroup.DOFade(visible ? 1 : 0, 0.3f).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                if (visible)
                {
                    canvasGroup.blocksRaycasts = true;
                    canvasGroup.interactable = true;
                }
                callback?.Invoke();
            });
    }
}
