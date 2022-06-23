using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

// SceneTransitionManager plays an animation before transitioning to the next scene. It also
// sends scene loading data asynchronously to LoadingScreenController
public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    public Animator sceneTransitionAnimator;
    public AsyncOperation loadSceneOperation;
    public AsyncOperation unloadSceneOperation;

    public bool showLoadScreen = true;

    public void TransitionToScene(SceneReference scene)
    {
        StartCoroutine("PlaySceneTransitionAnimation", scene.ScenePath);
    }

    public void TransitionToSceneInstant(SceneReference scene)
    {
        SceneManager.LoadScene(scene.ScenePath);
    }

    IEnumerator PlaySceneTransitionAnimation(string s)
    {
        if (sceneTransitionAnimator == null)
        {
            StartCoroutine("LoadNextScene");
        }
        else
        {
            // play the animation
            sceneTransitionAnimator.SetTrigger("Close");

            // wait for the animatino clip info to be ready
            while (sceneTransitionAnimator.GetCurrentAnimatorClipInfo(0).Length == 0)
            {
                yield return null;
            }

            // wait for the clip to complete before transitioning to the next scene
            float animTime = sceneTransitionAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            float extraWaitTime = 1.0f; // wait an extra second
            float t = animTime + extraWaitTime;
            while (t > 0)
            {
                t -= Time.deltaTime;
                yield return null;
            }

            StartCoroutine("LoadNextScene", s);
        }
    }

    // Returns 'true' when the scene has finished loading. This function is called from
    // PlayerController.cs in OnLoadingScreenContinue
    public bool IsSceneDoneLoading()
    {
        return GetSceneLoadProgress() > 1 || Mathf.Approximately(GetSceneLoadProgress(), 1);
    }

    public float GetSceneLoadProgress()
    {
        float loadProgress = 0;
        float unloadProgress = 0;

        // get the progress of both the load and unload operations
        if (loadSceneOperation != null)
        {
            loadProgress = loadSceneOperation.isDone ? 1 : loadSceneOperation.progress;
        }
        if (unloadSceneOperation != null)
        {
            unloadProgress = unloadSceneOperation.isDone ? 1 : unloadSceneOperation.progress;
        }
        float result = (unloadProgress + loadProgress) / 2.0f;
        return result;
    }

    IEnumerator LoadNextScene(string s)
    {
        SceneManager.LoadScene(GameManager.Instance.loadingScene, LoadSceneMode.Additive);
        yield return new WaitForSecondsRealtime(0.2f);
        LoadingScreenController.Instance.Show();
        yield return new WaitForSecondsRealtime(2.0f); // wait a sec for the loading screen to appear

        // load the new scene
        loadSceneOperation = SceneManager.LoadSceneAsync(s, LoadSceneMode.Additive);
        while (loadSceneOperation.progress < 1)
        {
            LoadingScreenController.Instance.ShowLoadProgress(GetSceneLoadProgress());
            yield return null;
        }

        // move the loading screen to the next scene
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByPath(s));

        unloadSceneOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);

        // unload the previous scene
        while (unloadSceneOperation.progress < 1)
        {
            LoadingScreenController.Instance.ShowLoadProgress(GetSceneLoadProgress());
            yield return null;
        }
        LoadingScreenController.Instance.ShowLoadProgress(GetSceneLoadProgress());
        LoadingScreenController.Instance.OnReadyToLaunchScene();
        Destroy(gameObject);
    }
}