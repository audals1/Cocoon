using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SingletonMonoBehaviour<Bullet>
{
    [SerializeField] Player m_player;
    [SerializeField] Transform m_firePos; // bullet 발사 위치
    [SerializeField] GameObject m_bullet; // bullet
    [SerializeField] GameObject m_wall; // bulletWall
    Rigidbody2D m_rigid;

    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hole"))
        {
            //조건 - 닿았을때 ;: 생성까지
            var wall = Instantiate(m_wall, collision.transform.position, collision.transform.rotation);
        }
        if(collision.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }
}