using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : DontDestroy<LoadSceneManager>
{
    public enum SceneState
    {
        None = -1,
        Title,
        Lobby,
        Game
    }
    AsyncOperation m_loadinginfo;
    SceneState m_scene;
    SceneState m_loadScene = SceneState.None;
    public SceneState Scene { get { return m_scene; } }
    public void LoadSceneAsync(SceneState scene)
    {
        if (m_loadScene != SceneState.None) return;
        m_loadScene = scene;
        m_loadinginfo = SceneManager.LoadSceneAsync((int)scene);
        gameObject.SetActive(true);
    }

    protected override void OnStart()
    {
        gameObject.SetActive(false);
    }

    void HideUI()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_loadinginfo != null)
        {
            if (m_loadinginfo.isDone)
            {
                m_scene = m_loadScene;
                m_loadScene = SceneState.None;
                m_loadinginfo = null;
                gameObject.SetActive(false);
                Invoke("HideUI", 0.5f);
            }
        }
    }
}