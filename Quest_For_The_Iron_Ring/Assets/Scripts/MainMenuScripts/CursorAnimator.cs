using UnityEngine;
using System.Collections;

public class CursorAnimator : MonoBehaviour
{
    public float clickScale = 0.6f;
    public float duration = 0.1f;
    
    private Vector3 originalScale;
    private bool isAnimating = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void PlayClickAnimation()
    {
        Debug.Log("Cursor animation triggered");

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