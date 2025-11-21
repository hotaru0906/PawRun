using UnityEngine;
using DG.Tweening;

public class UIVFX : MonoBehaviour
{
    [Header("Canvas Group")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Animation Settings")]
    [SerializeField] private float fadeInDuration = 0.8f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private Vector3 startScale = Vector3.zero;
    [SerializeField] private Vector3 endScale = Vector3.one;

    void Start()
    {
        InitializeCanvas();
    }

    private void InitializeCanvas()
    {
        if (canvasGroup == null) return;
    }

    public void FadeInUI()
    {
        if (canvasGroup == null) return;

        canvasGroup.alpha = 0f;
        canvasGroup.transform.localScale = startScale;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        var sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1f, fadeInDuration).SetUpdate(true));
        sequence.Join(canvasGroup.transform.DOScale(endScale, fadeInDuration).SetEase(Ease.OutBack).SetUpdate(true));

        sequence.SetUpdate(true);
    }

    public void FadeOutUI()
    {
        if (canvasGroup == null) return;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        var sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(0f, fadeOutDuration).SetUpdate(true));
        sequence.Join(canvasGroup.transform.DOScale(startScale, fadeOutDuration).SetEase(Ease.InBack).SetUpdate(true));

        sequence.SetUpdate(true);
    }

    private void OnDestroy()
    {
        if (canvasGroup == null) return;
        canvasGroup.DOKill();
    }

}