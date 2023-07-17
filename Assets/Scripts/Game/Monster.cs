using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Animator m_anim;
    [SerializeField] Player m_player;
    SpriteRenderer m_render;
    BoxCollider2D m_collider;
    Rigidbody2D m_rigid;
    [Range(1f, 10f)] [SerializeField] float m_monSpeed = 1f;
    public Transform m_detectPos;
    public bool m_isdtect;
    // Start is called before the first frame update
    void Start()
    {
        m_render = GetComponent<SpriteRenderer>();
        m_rigid = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector3.left * 5f, Color.green);
    }
    public void InitMonster(Player player, Transform detectPos)
    {
        m_player = player;
        m_detectPos = detectPos;
    }
    void Move()
    {
        //인식거리 되면 좌로 이동
        float detectDist = (m_detectPos.position - transform.position).magnitude;
        float dist = (m_player.transform.position - transform.position).magnitude;
        if(dist <= detectDist)
        {
            Debug.Log("플레이어인식");
            m_isdtect = true;
            transform.position += Vector3.left * m_monSpeed * Time.deltaTime;
        }
        else
        {
            if (m_isdtect)
            {
                Debug.Log("인식후멀어짐");
                //인식 후 좌로 전진
                m_render.flipX = true;
                transform.position += Vector3.right * m_monSpeed * 1.5f * Time.deltaTime;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 5f, LayerMask.NameToLayer("Ground"));
            }
        }
    }
    
    public void MonsterDie()
    {
        m_render.color = Color.red;
        m_render.flipY = true;
        m_collider.enabled = false;
        m_rigid.AddForce(Vector3.down * 3f, ForceMode2D.Impulse);
        Invoke("MonsterDeactive", 3f);
    }
    void MonsterDeactive()
    {
        gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("DeadZone"))
        {
            MonsterDeactive();
        }
        if(collision.CompareTag("Bullet"))
        {
            MonsterDie();
        }
    }
    
}
