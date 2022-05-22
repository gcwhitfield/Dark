using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// SceneTransitionManager is a little helpful class that plays an animation before transitioning
// to the next scene.
//
// SceneTransitionManager was was written by George Whitfield in Fall of 2021 for a previous game project,
// called "Lingua Litis"
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
            SceneManager.LoadScene(s);
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
        float loadProgress = loadSceneOperation != null ? loadSceneOperation.progress : 0;
        float unloadProgress = unloadSceneOperation != null ? unloadSceneOperation.progress : 0;
        return (unloadProgress + loadProgress) / 2.0f;
    }

    IEnumerator LoadNextScene(string s)
    {
        LoadingScreenController.Instance.Show();
        yield return new WaitForSeconds(1.0f); // wait a sec for the loading screen to appear


        // load the new scene
        loadSceneOperation = SceneManager.LoadSceneAsync(s, LoadSceneMode.Additive);
        while (loadSceneOperation.progress < 1)
        {
            LoadingScreenController.Instance.ShowLoadProgress(GetSceneLoadProgress());
            yield return null;
        }

        // move the loading screen to the next scene
        Debug.Log("Destination scene: " + s + " : " + SceneManager.GetSceneByName(s).name);
        SceneManager.MoveGameObjectToScene(LoadingScreenController.Instance.gameObject, SceneManager.GetSceneByName(s));

        unloadSceneOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name, UnloadSceneOptions.None);

        // unload the previous scene
        while (unloadSceneOperation.progress < 1)
        {
            LoadingScreenController.Instance.ShowLoadProgress(GetSceneLoadProgress());
            yield return null;
        }

        // the loading screen will be closed by the player from PlayerController.cs when
        // they hit a button
    }
}
