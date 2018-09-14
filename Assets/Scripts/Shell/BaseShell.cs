using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShell : MonoBehaviour
{
    protected Rigidbody m_rigidBody = null;
    public float m_minLaunchForce;
    public float m_maxLaunchForce;

    protected virtual void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public virtual void Fire(Vector3 position, Quaternion rotation, Vector3 velocity, GameObject tank)
    {
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        m_rigidBody.velocity = velocity;
    }
}
