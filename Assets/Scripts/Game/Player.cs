using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    #region Property
    [Range(1f,10f)] [SerializeField] float m_speed = 1f;
    [Range(1f, 20f)] [SerializeField] float m_jumpPow = 1f;
    [Range(0.1f, 0.8f)] [SerializeField] float m_secondJumpPow = 1f;
    [SerializeField] float m_knockBack = 3f;
    Vector3 m_dir;
    Vector3 m_bulletDir;
    public bool m_isFinished;
    [SerializeField] int m_jumpCount;
    [SerializeField] bool m_isJumping;
    [SerializeField] float m_idleTime;
    [SerializeField] Transform m_startPos;
    [SerializeField] Transform m_firePos;
    [SerializeField] Transform m_endPos;
    Rigidbody2D m_rigid;
    BoxCollider2D m_cocoonCollider;
    CapsuleCollider2D m_funCollider;
    Animator m_anim;
    SpriteRenderer m_render;
    [SerializeField]GameObject m_finishPos;
    [SerializeField] GameObject m_bulletPrefab;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM(SoundManager.ClipBGM.Stage01);
        m_rigid = GetComponent<Rigidbody2D>();
        m_render = GetComponentInChildren<SpriteRenderer>();
        m_anim = GetComponent<Animator>();
        m_cocoonCollider = GetComponent<BoxCollider2D>();
        m_funCollider = GetComponent<CapsuleCollider2D>();
        //InitPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Action();
    }
    public void InitPlayer()
    {
        GameManager.Instance.m_playerHp = 3;
        m_jumpCount = 0;
        m_isJumping = false;
        PlayerReposition();
    }
    public void SetTarget(Transform start, Transform target)
    {
        m_firePos = start;
        m_endPos = target;
    }
    void SetIdle()
    {
        m_jumpCount = 0;
        m_isJumping = false;
    }
    void Move()
    {
        if(!m_isFinished)
        {
            m_dir = new Vector3(1, 0, 0);
            transform.position += m_dir * m_speed * Time.deltaTime;
        }
        else
        {
            EvolutionMove();
        }
    }
    void Jump()
    {
        if(!m_isFinished)
        {
            if (Input.GetKeyDown(KeyCode.Space) && m_jumpCount < 1 && !m_isJumping)
            {
                SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.jump);
                m_rigid.AddForce(Vector3.up * m_jumpPow, ForceMode2D.Impulse);
                m_jumpCount = 1;
                Debug.Log("점프카운팅" + m_jumpCount);
                m_isJumping = true;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && m_jumpCount >= 1 && m_isJumping)
            {
                SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.jump);
                m_rigid.AddForce(Vector3.up * m_jumpPow * m_secondJumpPow, ForceMode2D.Impulse);
                m_jumpCount = 2;
                m_isJumping = false;
                Debug.Log("점프카운팅" + m_jumpCount);
            }
        }
    }
    void Action()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameManager.Instance.m_restartBtn.IsActive())
            {
                GameManager.Instance.HideRestart();
            }
            else
            {
                GameManager.Instance.ShowRestart();
            }

        }
        if(GameManager.Instance.m_stageNum < 2)
        {
            //1스테이지의 경우 점프
            Jump();
        }
        else
        {
            Fire();    
        }
    }
    void Fire()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !m_isFinished)
        {
            if(GameManager.Instance.m_bulletCount > 0)
            {

                SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.shoot);
                var bull = Instantiate(m_bulletPrefab);
                bull.transform.position = m_firePos.transform.position;
                SetTarget(m_firePos, m_endPos);
                m_bulletDir = m_endPos.transform.position - m_firePos.transform.position;
                Rigidbody2D rigi = bull.GetComponent<Rigidbody2D>();
                rigi.AddForce(m_bulletDir * m_speed, ForceMode2D.Force);
                GameManager.Instance.m_bulletCount--;
            }
            else if(GameManager.Instance.m_bulletCount <= 0)
            {
                SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.stomp);
                GameManager.Instance.ShowReload();
            }
            
        }
    }
    void Attack(Transform target)
    {
        Monster monster = target.GetComponent<Monster>();
        monster.MonsterDie();
    }
    void Die()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.die);
        m_render.color = Color.red;
        m_render.flipY = true;
        m_cocoonCollider.enabled = false;
        m_funCollider.enabled = false;
        m_rigid.AddForce(Vector3.down * 3f, ForceMode2D.Impulse);
        gameObject.SetActive(false);
    }
    void SetDamage()
    {
        if(GameManager.Instance.m_playerHp <= 0)
        {
            Die();
        }
        else
        {
            GameManager.Instance.m_playerHp--;
            gameObject.layer = 6;
            m_render.color = new Color(1, 1, 1, 0.4f);
            m_rigid.AddForce(new Vector2(-1,1) * m_knockBack, ForceMode2D.Impulse);
            SetIdle();
            Invoke("OffAmor", 3f);
        }
    }
    void OffAmor()
    {
        gameObject.layer = 3;
        m_render.color = new Color(1, 1, 1, 1f);
    }
    void PlayerChange()
    {
        if(GameManager.Instance.m_stageNum == 1)
        {
            m_anim.SetTrigger("Cocoon");
            m_cocoonCollider.enabled = true;
            m_funCollider.enabled = false;
        }
        if (GameManager.Instance.m_stageNum == 2)
        {
            m_anim.SetTrigger("Pupa");
            m_funCollider.enabled = true;
            m_cocoonCollider.enabled = false;
        }
    }
    void PlayerReposition()
    {
        m_rigid.gravityScale = 1;
        m_isFinished = false;
        transform.position = m_startPos.position;
    }
    void EvolutionMove()
    {
        //골인 후 떠오르는 액션
        m_rigid.gravityScale = 0;
        transform.position += Vector3.up * m_speed * Time.deltaTime;
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //낙사
        if(collision.CompareTag("DeadZone"))
        {
            Die();
        }
        //나뭇잎먹음
        if (collision.CompareTag("Leaf"))
        {
            SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.item);
            collision.gameObject.SetActive(false);
            GameManager.Instance.GetLeaf();
        }
        if (collision.CompareTag("Flower"))
        {
            SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.item);
            collision.gameObject.SetActive(false);
            GameManager.Instance.GetHp();
            GameManager.Instance.ResetBullet();
        }
        //도착
        if (collision.CompareTag("Finish"))
        {
            if (!m_isFinished)
            {
                m_isFinished = true;
                SoundManager.Instance.StopBGM();
                SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.flower);
                if (GameManager.Instance.m_stageNum < 2)
                {
                    GameManager.Instance.m_stageNum++;
                    GameManager.Instance.m_bulletCount = 5;
                }
                else
                {
                    SceneManager.LoadScene("Ending");
                }
                GameManager.Instance.StageCheck();
            }
        }
        if(collision.CompareTag("EvolArea"))
        {
            transform.position = m_finishPos.transform.position;
            SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.item);
            PlayerChange();
            Invoke("PlayerReposition", 1f);
            SoundManager.Instance.PlayBGM(SoundManager.ClipBGM.Stage02);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //플레이어가 땅에 닿음
        if(collision.collider.CompareTag("Ground"))
        {
            m_jumpCount = 0;
            m_isJumping = false;
        }
        //몬스터에 충돌
        if (collision.collider.CompareTag("Monster"))
        {
            //머리밟음
            if(m_rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.hit);
                Attack(collision.transform);
                GameManager.Instance.KillMon();
            }
            //그외충돌
            else
            {
                SoundManager.Instance.PlaySFX(SoundManager.ClipSFX.hit);
                SetDamage();
            }
        }
    }
}
