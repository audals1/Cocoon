using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGContorller : MonoBehaviour
{   
    SpriteRenderer m_spriteRenderer;
    [SerializeField]float m_speed = 0.1f;
    float m_scale = 1f;
    public void SetScale(float scale)
    {
        m_scale = scale;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        m_spriteRenderer.material.mainTextureOffset += m_speed * m_scale * Vector2.right * Time.deltaTime;
    }
}