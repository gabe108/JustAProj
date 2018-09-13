using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMines : MonoBehaviour
{
    public GameObject LandMinePrefab = null;
    public int PrefabSpawnCount = 10;
    public Rigidbody m_Mines;
    public int m_NumberOfMines = 15;
	public Transform[] PlayerSpawnPoints;

    private Vector3 m_PreviousMinePos;

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        System.Random rnd = new System.Random();

        for (int i = 1; i < m_NumberOfMines; i++)
        {
            int x = rnd.Next(-50, 30);
            int z = rnd.Next(-50, 30);
            Vector3 MinesPosition = new Vector3(x, -0.151f, z);
			if(MinesPosition.x > PlayerSpawnPoints[1].position.x - 10 || MinesPosition.x < PlayerSpawnPoints[1].position.x + 10 ||
				MinesPosition.x > PlayerSpawnPoints[2].position.x - 10 || MinesPosition.x < PlayerSpawnPoints[2].position.x + 10)
			{
				MinesPosition = new Vector3(x + 20, -0.151f, z);
			}
			else if(MinesPosition.z > PlayerSpawnPoints[1].position.z - 10 || MinesPosition.z < PlayerSpawnPoints[1].position.z + 10 ||
					MinesPosition.z > PlayerSpawnPoints[2].position.z - 10 || MinesPosition.z < PlayerSpawnPoints[2].position.z + 10)
			{
				MinesPosition = new Vector3(x, -0.151f, z + 20);
			}

			if ((MinesPosition.x - m_PreviousMinePos.x) < 10.0f || 
				(MinesPosition.z - m_PreviousMinePos.z) < 10.0f )
			{
				x = rnd.Next(-50, 30);
				z = rnd.Next(-50, 30);
				MinesPosition = new Vector3(x, -0.151f, z);
				if (MinesPosition.x > -42.0f && MinesPosition.x < 30.0f)
                {
                Rigidbody mineInstance = Instantiate(m_Mines, MinesPosition, new Quaternion());
                    m_PreviousMinePos = MinesPosition;
                }
            }
            else
            {
                Rigidbody mineInstance = Instantiate(m_Mines, MinesPosition, new Quaternion());
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
