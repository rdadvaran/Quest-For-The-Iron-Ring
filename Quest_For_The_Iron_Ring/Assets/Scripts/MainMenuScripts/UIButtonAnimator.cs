using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonAnimator : MonoBehaviour, IPointerClickHandler
{
    public float clickScale = 1.1f;
    public float duration = 0.1f;

    private Vector3 originalScale;
    private bool isAnimating = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isAnimating)
        {
            StartCoroutine(ClickAnimation());
        }
    }

    private IEnumerator ClickAnimation()
    {
        isAnimating = true;

        Vector3 targetScale = originalScale * clickScale;

        float t = 0;
        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        t = 0;
        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        isAnimating = false;
    }
}