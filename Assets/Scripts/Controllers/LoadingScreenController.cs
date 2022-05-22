using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreenController : Singleton<LoadingScreenController>
{
    // this GameObject will be enabled and disabled to open and close the loading
    // screen
    public GameObject loadingScreen;

    public Slider loadingProgressSlider;
    public TMP_Text text;
    public Animator pressAnyButtonToContinueAnimator;
    public Image image;
    public Animator animator; 

    static LoadingScreenInfo nextLoadingScreenInfo;

    public void PrepareToShow(LoadingScreenInfo info)
    {
        nextLoadingScreenInfo = info;
    }

    public void Show()
    {
        loadingScreen.SetActive(true);
        animator.Rebind();
        if (nextLoadingScreenInfo != null)
        {
            text.text = nextLoadingScreenInfo.text;
            if (nextLoadingScreenInfo.image != null)
            {
                image.sprite = nextLoadingScreenInfo.image;
            }
        }
        animator.SetTrigger("Open");
    }

    public void Close()
    {
        pressAnyButtonToContinueAnimator.Rebind();
        StartCoroutine("DoClose");
    }

    IEnumerator DoClose()
    {
        animator.SetTrigger("Close");
        float t = 0;
        while (t < animator.GetCurrentAnimatorClipInfo(0)[0].clip.length)
        {
            t += Time.deltaTime;
            yield return null;
        }

        // destroy this LoadingScreenController, because there is a new one
        // in the scene that was just loaded
        Destroy(gameObject);
    }

    // 'progress' must be a float between 0 and 1
    public void ShowLoadProgress(float progress)
    {
        if (progress < 0 || progress > 1)
        {
            Debug.LogError("Loading bar progress is invalid");
            return;
        }

        loadingProgressSlider.value = progress;
        if (Mathf.Approximately(progress, 1))
        {
            pressAnyButtonToContinueAnimator.SetTrigger("Open");
        }
    }
}
