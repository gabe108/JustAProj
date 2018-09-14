using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : BaseShell
{
    public LayerMask m_TankMask;
    public GameObject m_BallonPrefab;
    public float m_lifeTime;
    public float m_force;

    private Rigidbody m_parent = null;
    private float m_timer = 0.0f;
    private Rigidbody targetRigidBody;

    // Use this for initialization
    void Start ()
    {
        m_parent = GetComponentInParent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_timer += Time.time;

        if (m_timer > m_lifeTime)
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
            targetRigidBody.constraints = RigidbodyConstraints.None;
            
        }

		if (m_parent)
        {
            m_parent.AddForce(new Vector3(0, m_force * Time.deltaTime, 0));
        }
        
	}

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.active == true)
        {

            //Find all colliders in the explosoin radius and add them into an array, we're keeping this smaller than
            // a usual shell explosion as this is the balloon variant
            Collider[] colliders = Physics.OverlapCapsule(transform.position, other.transform.position, m_TankMask);

            for (int i = 0; i < colliders.Length; i++)
            {

                //Find their rigid bodies
                targetRigidBody = colliders[i].GetComponent<Rigidbody>();

                //If they don't have a rigid body, we can't do anything with them, move onto the next collided object
                if (!targetRigidBody)
                    continue;

                //Find the tankHealth script associated with the target gameobject using GetComponent
                TankHealth targetHealth = targetRigidBody.GetComponent<TankHealth>();

                //If the object does not have a tankHealth script, we move on to the next object
                // We don't plan on doing any actual damage here, yet we want to check the health to make sure
                // we're not affecting something other than the player, where the health is a unique identifier
                if (!targetHealth)
                    continue;

                Vector3 tankPos = targetRigidBody.transform.position;
                tankPos.Set(+0, +5, +0);

                //ATTACH BALLOON TO TANK HERE!
                GameObject balloon;
                //Creating the balloon
                balloon = Instantiate(m_BallonPrefab, tankPos, targetRigidBody.transform.rotation);

                //Setting the balloon to be a child of the collided object
                balloon.transform.parent = targetRigidBody.transform;

                //Locking all controls of the player when they are affected by the balloon
                targetRigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX |
                    RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

            }
        }
    }
}
