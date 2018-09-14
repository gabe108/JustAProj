using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAsteroids : MonoBehaviour
{
	public Rigidbody[] m_Asteroids = new Rigidbody[3];
	System.Random rnd;
	float m_TimeSpawned;

	// Use this for initialization
	void Start ()
	{
		//m_Asteroids[0] = GameObject.FindWithTag("Asteroid 1").GetComponent<Rigidbody>();
		//m_Asteroids[1] = GameObject.FindWithTag("Asteroid 2").GetComponent<Rigidbody>();
		//m_Asteroids[2] = GameObject.FindWithTag("Asteroid 3").GetComponent<Rigidbody>();
		rnd = new System.Random();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time - m_TimeSpawned > 1.0f )
		{
			int x = rnd.Next(-50, 30);
			int z = rnd.Next(-50, 30);

			int whichToSpawn = rnd.Next(0, 2);
			Vector3 AsteroidsPos = new Vector3(x, 155, z);

			Instantiate(m_Asteroids[whichToSpawn], AsteroidsPos, new Quaternion());
			Instantiate(m_Asteroids[whichToSpawn], new Vector3(42, 155, 0), new Quaternion());

			m_TimeSpawned = Time.time;
		}
	}
}
