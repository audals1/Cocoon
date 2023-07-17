using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    [SerializeField]Animator m_endAnim;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM(SoundManager.ClipBGM.Ending);
        m_endAnim = GetComponent<Animator>();
        m_endAnim.SetTrigger("Ending");
    }

}
