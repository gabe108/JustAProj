using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellRotation : MonoBehaviour
{
    private Rigidbody m_rigidBody = null;
	// Use this for initialization
	void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.forward = m_rigidBody.velocity;
    }
}
