using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public enum ScenesState
    {
        None = -1,
        Title,
        Game,
        Ending
    };
    public int m_stageNum = 1;
    public int m_totalScore;
    public int m_leafScore;
    public int m_playerHp = 3;
    public int m_bulletCount;
    [SerializeField] GameObject m_stage1;
    [SerializeField] GameObject m_stage2;
    [SerializeField] GameObject m_stageUI;
    [SerializeField] Player m_player;
    [SerializeField] Text m_totalScoreUI;
    [SerializeField] Text m_leafUIText;
    [SerializeField] Text m_stageUIText;
    [SerializeField] Text m_bulletUIText;
    [SerializeField] Text m_reloadText;
    [SerializeField] Text m_restartText;
    [SerializeField] Button m_pauseBtn;
    public Button m_restartBtn;
    [SerializeField]Image[] m_hpImgs = new Image[3];

    void Start()
    {
        m_reloadText.gameObject.SetActive(false);    
    }
    void Update()
    {
        HpCheck();
        TotalScoreCheck();
    }
    public void StageCheck()
    {
        if(m_stageNum == 1)
        {
            m_stage1.gameObject.SetActive(true);
            m_stage2.gameObject.SetActive(false);
        }
        if (m_stageNum == 2)
        {
            m_stage2.gameObject.SetActive(true);
            m_stage1.gameObject.SetActive(false);
        }
        if(m_stageNum == 3)
        {
            SceneManager.LoadScene("Ending");
        }
    }
    public void TotalScoreCheck()
    {
        m_totalScoreUI.text = m_totalScore.ToString();
    }
    public void KillMon()
    {
        m_totalScore += 200;
    }
    public void GetLeaf()
    {
        m_leafScore++;
        m_totalScore += 100;
        m_leafUIText.text = m_leafScore.ToString();
    }
    public void GetHp()
    {
        m_totalScore += 100;
        if(m_playerHp < 3)
        {
            m_playerHp++;
        }
        else
        {
            m_totalScore += 100;
        }   
    }
    public void ResetBullet()
    {
        m_bulletCount += 5;
        m_bulletUIText.text = m_bulletCount.ToString();
    }
    public void HpCheck()
    {
        switch (m_playerHp)
        {
            case 3:
                m_hpImgs[0].fillAmount = 1;
                m_hpImgs[1].fillAmount = 1;
                m_hpImgs[2].fillAmount = 1;
                break;
            case 2:
                m_hpImgs[0].fillAmount = 0;
                m_hpImgs[1].fillAmount = 1;
                m_hpImgs[2].fillAmount = 1;
                break;
            case 1:
                m_hpImgs[0].fillAmount = 0;
                m_hpImgs[1].fillAmount = 0;
                m_hpImgs[2].fillAmount = 1;
                break;
            default:
                m_hpImgs[0].fillAmount = 0;
                m_hpImgs[1].fillAmount = 0;
                m_hpImgs[2].fillAmount = 0;
                break;
        }
    }
    public void ShowReload()
    {
        m_reloadText.gameObject.SetActive(true);
    }
    public void HideReload()
    {
        m_reloadText.gameObject.SetActive(false);
    }
    public void ShowRestart()
    {
        m_restartBtn.gameObject.SetActive(true);
    }
    public void HideRestart()
    {
        m_restartBtn.gameObject.SetActive(false);
    }
    public void PauseGame()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Title");
    }
}
