using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SingletonMonoBehaviour<Bullet>
{
    [SerializeField] Player m_player;
    [SerializeField] Transform m_firePos; // bullet �߻� ��ġ
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
            //���� - ������� ;: ��������
            var wall = Instantiate(m_wall, collision.transform.position, collision.transform.rotation);
        }
        if(collision.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }
}