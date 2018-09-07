using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public float m_lifeTime;
    public float m_force;

    private Rigidbody m_parent = null;
    private float m_timer = 0.0f;
	// Use this for initialization
	void Start ()
    {
        m_parent = GetComponentInParent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_timer += Time.deltaTime;

        if (m_timer > m_lifeTime)
        {
            Destroy(gameObject);
        }

		if (m_parent)
        {
            m_parent.AddForce(new Vector3(0, m_force * Time.deltaTime, 0));
        }
        
	}
}
