using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    Rigidbody2D m_rigid;
    [SerializeField] GameObject m_bulletWall;
    [SerializeField] Transform m_targetPos;
    float m_time;
    Vector3 m_targetDir;
    // Start is called before the first frame update
    void Start()
    {
        m_rigid = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        RemoveBullet();
    }
    void FixedUpdate()
    {
        m_rigid.AddForce((m_targetPos.position - transform.position) * 0.5f, ForceMode2D.Impulse);    
    }

    public void RemoveBullet()
    {
        m_time += Time.deltaTime;
        if(m_time >= 3f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hole"))
        {
            var wall = Instantiate(m_bulletWall, collision.transform.position, collision.transform.rotation);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Ground") || collision.CompareTag("Monster"))
        {
            Destroy(gameObject);
        }
    }
}
