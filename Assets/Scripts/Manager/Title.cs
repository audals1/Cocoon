using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Title : MonoBehaviour
{
    [SerializeField] GameObject m_titleImg;
    [SerializeField] GameObject m_presskey;
    public bool m_isPresskey;
    // Start is called before the first frame update
    void Start()
    {
        m_presskey.gameObject.SetActive(true);
        SoundManager.Instance.PlayBGM(SoundManager.ClipBGM.Intro);
    }

    // Update is called once per frame
    void Update()
    {
        GameStart();
    }
    void GameStart()
    {
        if(Input.anyKey)
        {
            SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.item);
            StartCoroutine("Coroutin_GameStart");
        }
    }
    IEnumerator Coroutin_GameStart()
    {
        int count = 0;
        while (count < 3)
        {
            m_presskey.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            m_presskey.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            count++;
            if(count >= 3)
            {
                m_isPresskey = true;
                if (m_isPresskey)
                {
                    SceneManager.LoadScene("Game");
                }
            }
        }
    }
}
