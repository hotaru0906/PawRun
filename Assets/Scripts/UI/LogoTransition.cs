using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class LogoSceneTransition : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup transitionCanvasGroup;
    [SerializeField] private Image logoImage;

    [Header("Settings")]
    [SerializeField] private float scaleUp = 10f;
    [SerializeField] public float transitionDuration = 1.5f;
    [SerializeField] public float reverseTransitionDuration = 1.5f;
    [SerializeField] public bool autoPlayReverseOnStart = false;

    [Header("Model Camera")]
    [SerializeField] private Camera modelCamera;
    [SerializeField] private string sceneWithModelCamera = "MainMenu"; // Tên scene có camera model

    private void Awake()
    {
        if (transitionCanvasGroup == null || logoImage == null) return;

        if (modelCamera != null)
        {
            modelCamera.enabled = false;
        }
        transitionCanvasGroup.alpha = 0f;
        transitionCanvasGroup.interactable = false;
        transitionCanvasGroup.blocksRaycasts = false;
        logoImage.transform.localScale = new Vector3(scaleUp, scaleUp, scaleUp);
    }

    private void Start()
    {
        // if (GameManager.Instance.IsGameJustLaunched)
        //     return;

        if (autoPlayReverseOnStart && AreReferencesValid())
        {
            StartCoroutine(DoReverseLogoTransition());
        }
    }

    private bool AreReferencesValid()
    {
        return transitionCanvasGroup != null && logoImage != null;
    }

    public void TransitionToScene(string nextScene)
    {
        if (!AreReferencesValid() || string.IsNullOrEmpty(nextScene)) return;
        StartCoroutine(DoLogoTransition(nextScene));
    }

    public void DoLogoTransitionOnly()
    {
        if (!AreReferencesValid()) return;
        modelCamera.enabled = false;
        StartCoroutine(DoLogoTransition());
    }

    private IEnumerator DoLogoTransition(string nextScene)
    {
        if (!AreReferencesValid()) yield break;

        // ✅ DISABLE SFX during scene transition
        // if (SFXController.Instance != null)
        // {
        //     SFXController.Instance.SetIsSceneLoading(true);
        // }

        transitionCanvasGroup.alpha = 1f;
        transitionCanvasGroup.interactable = true;
        transitionCanvasGroup.blocksRaycasts = true;
        logoImage.transform.localScale = new Vector3(scaleUp, scaleUp, scaleUp);

        yield return logoImage.transform.DOScale(Vector3.one, transitionDuration)
            .SetEase(Ease.OutExpo)
            .SetUpdate(true)
            .WaitForCompletion();

        SceneManager.LoadScene(nextScene);
        yield return null;

        if (logoImage != null)
        {
            yield return logoImage.transform.DOScale(Vector3.zero, transitionDuration)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .WaitForCompletion();
        }

        if (transitionCanvasGroup != null)
        {
            transitionCanvasGroup.alpha = 0f;
            transitionCanvasGroup.interactable = false;
            transitionCanvasGroup.blocksRaycasts = false;
        }

        // ✅ RE-ENABLE SFX after transition complete
        // if (SFXController.Instance != null)
        // {
        //     SFXController.Instance.SetIsSceneLoading(false);
        // }

        // Enable model camera after transition (only in specific scene)
        EnableModelCameraIfInCorrectScene();
    }

    private IEnumerator DoLogoTransition()
    {
        if (!AreReferencesValid()) yield break;

        // ✅ DISABLE SFX during logo transition
        // if (SFXController.Instance != null)
        // {
        //     SFXController.Instance.SetIsSceneLoading(true);
        // }

        transitionCanvasGroup.alpha = 1f;
        transitionCanvasGroup.interactable = true;
        transitionCanvasGroup.blocksRaycasts = true;
        logoImage.transform.localScale = new Vector3(scaleUp, scaleUp, scaleUp);

        yield return logoImage.transform.DOScale(Vector3.one, transitionDuration)
            .SetEase(Ease.OutExpo)
            .SetUpdate(true)
            .WaitForCompletion();

        // No scene loading here, just animation
        yield return new WaitForSecondsRealtime(0.5f); // Small pause

        if (logoImage != null)
        {
            yield return logoImage.transform.DOScale(Vector3.zero, transitionDuration)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .WaitForCompletion();
        }

        if (transitionCanvasGroup != null)
        {
            transitionCanvasGroup.alpha = 0f;
            transitionCanvasGroup.interactable = false;
            transitionCanvasGroup.blocksRaycasts = false;
        }

        // ✅ RE-ENABLE SFX after transition complete
        // if (SFXController.Instance != null)
        // {
        //     SFXController.Instance.SetIsSceneLoading(false);
        // }

        // Enable model camera after transition (only in specific scene)
        EnableModelCameraIfInCorrectScene();
    }

    private IEnumerator DoReverseLogoTransition()
    {
        if (!AreReferencesValid()) yield break;

        // ✅ DISABLE SFX during reverse transition
        // if (SFXController.Instance != null)
        // {
        //     SFXController.Instance.SetIsSceneLoading(true);
        // }

        transitionCanvasGroup.alpha = 1f;
        transitionCanvasGroup.interactable = true;
        transitionCanvasGroup.blocksRaycasts = true;
        logoImage.transform.localScale = Vector3.one;

        if (logoImage != null && logoImage.transform != null)
        {
            yield return logoImage.transform.DOScale(Vector3.zero, reverseTransitionDuration)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .WaitForCompletion();
        }
        else
        {
            yield return new WaitForSecondsRealtime(reverseTransitionDuration);
        }

        if (transitionCanvasGroup != null)
        {
            transitionCanvasGroup.alpha = 0f;
            transitionCanvasGroup.interactable = false;
            transitionCanvasGroup.blocksRaycasts = false;
        }

        // ✅ RE-ENABLE SFX after reverse transition complete
        // if (SFXController.Instance != null)
        // {
        //     SFXController.Instance.SetIsSceneLoading(false);
        // }

        // Enable model camera after reverse transition (only in specific scene)
        EnableModelCameraIfInCorrectScene();
    }

    public bool IsReadyForTransition()
    {
        return AreReferencesValid();
    }

    public void StopAllTransitions()
    {
        StopAllCoroutines();

        if (logoImage != null)
        {
            logoImage.transform.DOKill();
        }

        if (transitionCanvasGroup != null)
        {
            transitionCanvasGroup.alpha = 0f;
            transitionCanvasGroup.interactable = false;
            transitionCanvasGroup.blocksRaycasts = false;
        }

        // ✅ RE-ENABLE SFX if transitions are stopped
        // if (SFXController.Instance != null)
        // {
        //     SFXController.Instance.SetIsSceneLoading(false);
        // }

        // Enable model camera if transitions are stopped (only in specific scene)
        EnableModelCameraIfInCorrectScene();
    }

    private void EnableModelCameraIfInCorrectScene()
    {
        // Only enable camera if we're in the correct scene and camera exists
        if (modelCamera != null && !string.IsNullOrEmpty(sceneWithModelCamera))
        {
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == sceneWithModelCamera)
            {
                modelCamera.enabled = true;
            }
        }
    }
}