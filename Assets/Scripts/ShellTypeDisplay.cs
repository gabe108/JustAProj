using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellTypeDisplay : MonoBehaviour
{
    public string[] m_shellTypes = null;

    private Transform m_camera = null;
    private TextMesh m_text = null;
	// Use this for initialization
	void Start ()
    {
        m_text = GetComponent<TextMesh>();
        m_camera = GameObject.Find("Main Camera").transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.forward = m_camera.forward;
    }

    public void ChangeType(int index)
    {
        m_text.text = m_shellTypes[index];
    }
}
