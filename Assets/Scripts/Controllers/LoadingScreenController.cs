using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;

public class LoadingScreenController : Singleton<LoadingScreenController>
{
    // this GameObject will be enabled and disabled to open and close the loading
    // screen
    GameObject loadingScreen;

    public Slider loadingProgressSlider;
    public TMP_Text text;
    public Animator pressAnyButtonToContinueAnimator;
    public Image image;
    public Animator animator;

    public PlayerInput playerInput;

    static LoadingScreenInfo nextLoadingScreenInfo;

    // this is set to true by SceneTransitionManager when the next
    // scene is ready to launch
    bool ready = false;

    private void Start()
    {
        loadingScreen = animator.gameObject;
        Time.timeScale = 0;
    }

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
        Time.timeScale = 1;
        animator.SetTrigger("Close");
        float t = 0;
        while (t < animator.GetCurrentAnimatorClipInfo(0)[0].clip.length)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        SceneManager.UnloadSceneAsync(gameObject.scene);
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
    }

    // This function is called when the scene is done loading and ready to be launched.
    // Called by SceneTransitionManager
    public void OnReadyToLaunchScene()
    {
        pressAnyButtonToContinueAnimator.SetTrigger("Open");
        ready = true;
    }

    // Called when the player presses the continue button
    public void OnContinue(CallbackContext context)
    {
        if (ready)
        {
            Close();
        }
    }
}
