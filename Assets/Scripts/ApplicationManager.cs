using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : Singleton<ApplicationManager>
{
    public static ApplicationData applicationData
    {
        get
        {
            if (_applicationData == null)
            {
                _applicationData = Resources.Load("ApplicationData") as ApplicationData;
            }
            return _applicationData;
        }
    }

    static ApplicationData _applicationData;

    private bool loadingScene = false;
    private float sceneTransitionDuration = .5f;

    protected override void OnAwake()
    {
        applicationData.LoadPlayerData();
        Application.targetFrameRate = 90;
        Input.multiTouchEnabled = false;
    }

    private void OnApplicationQuit()
    {
        applicationData.SavePlayerData();
    }

    public void ReloadActiveScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int buildIndex)
    {
        if (!loadingScene)
        {
            StartCoroutine(ExecuteLoadScene(buildIndex));
        }
    }

    public static Action OnSceneChanged;

    private IEnumerator ExecuteLoadScene(int buildIndex)
    {
        loadingScene = true;
        applicationData.SavePlayerData();
      
        yield return new WaitForSecondsRealtime(sceneTransitionDuration);

        AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            yield return null;
        }
        OnSceneChanged?.Invoke();
        applicationData.LoadPlayerData();
        loadingScene = false;
    }

    public static void ToggleSounds()
    {
        applicationData.playerData.soundsOn ^= true;
        AudioManager.Instance.UpdateVolume();
        applicationData.SavePlayerData();
    }

    public static void ToggleHaptic()
    {
        applicationData.playerData.hapticOn = !applicationData.playerData.hapticOn;
        applicationData.SavePlayerData();
    }

    public void ResetApplication()
    {
        applicationData.ClearPlayerData();
        applicationData.LoadPlayerData();
        ReloadActiveScene();
    }
}