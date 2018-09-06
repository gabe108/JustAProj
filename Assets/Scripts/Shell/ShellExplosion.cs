using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.

        //Find all colliders within the range of the shell's explosion radius and add them into an array
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        //Iterate through them ALL AND HURT THEM
        for (int i = 0; i < colliders.Length; i++)
        {
            //Find their rigid bodies
            Rigidbody targetRigidBody = colliders[i].GetComponent<Rigidbody>();

            //If they don't have a rigid body, we can't do anything with them, move onto the next collided object
            if (!targetRigidBody)
                continue;

            //Add an explosion force
            targetRigidBody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            //Find the tankHealth script associated with the target gameobject using GetComponent
            TankHealth targetHealth = targetRigidBody.GetComponent<TankHealth>();

            //If the object does not have a tankHealth script, we move on to the next object
            if (!targetHealth)
                continue;

            //Calculate the amount of damage the object should take based on how close it is to the explosion's center
            float damage = CalculateDamage(targetRigidBody.position);

            //Deal the damage to the tank
            targetHealth.TakeDamage(damage);
        }

        //Unparent the particles from the shells
        m_ExplosionParticles.transform.parent = null;


        //Play the particle effect
        m_ExplosionParticles.Play();

        //Once the animation has finished, move on to destroy the object underneath them
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);

        //Destroy the bullet now that all of it's functions have finished
        Destroy(gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.

        //Creation of a vector to get the direction to the taret
        Vector3 explosionToTarget = targetPosition - transform.position;

        //Distance calculation
        float explosionDistance = explosionToTarget.magnitude;

        //Calculate the maximum distance the target is away from the target
        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        //Calculate damage as this proportion of the maximum possible damage
        float damage = relativeDistance * m_MaxDamage;

        //Make sure that the minimum damage is always 0
        damage = Mathf.Max(0f, damage);

        //Return the amount of damage done
        return damage;
    }
}