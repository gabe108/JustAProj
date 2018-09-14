using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellTypeDisplay : MonoBehaviour
{
    public Transform m_camera = null;
    public Sprite[] m_shellTypes = null;

    private SpriteRenderer m_sprite = null;
	// Use this for initialization
	void Start ()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        m_camera = GameObject.Find("Main Camera").transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.forward = m_camera.forward;
    }

    public void ChangeType(int index)
    {
        m_sprite.sprite = m_shellTypes[index];
    }
}
