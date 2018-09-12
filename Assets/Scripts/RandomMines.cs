using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMines : MonoBehaviour
{
    public GameObject LandMinePrefab = null;
    public int PrefabSpawnCount = 10;
    public Rigidbody m_Mines;
    public int m_NumberOfMines = 15;

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
            int x = rnd.Next(-42, 30);
            int z = rnd.Next(-42, 30);
            Vector3 MinesPosition = new Vector3(x, -0.151f, z);
            if ((MinesPosition.x - m_PreviousMinePos.x) < 10.0f || (MinesPosition.z - m_PreviousMinePos.z) < 10.0f)
            {
                MinesPosition.x += 20.0f;
                MinesPosition.z += 20.0f;
                //if (MinesPosition.x > -42.0f && MinesPosition.x < 30.0f)
                //{
                Rigidbody mineInstance = Instantiate(m_Mines, MinesPosition, new Quaternion());
                    m_PreviousMinePos = MinesPosition;
                //}
            }
            //else if ()
            //{
            //    MinesPosition.z += 20.0f;
            //    //if (MinesPosition.z > -42.0f && MinesPosition.z < 30.0f)
            //    //{
            //        Rigidbody mineInstance = Instantiate(m_Mines, MinesPosition, new Quaternion());
            //        m_PreviousMinePos = MinesPosition;
            //    //}
            //}
            else
            {
                Rigidbody mineInstance = Instantiate(m_Mines, MinesPosition, new Quaternion());
            }
            //LandMines[i].transform.position = new Vector3(x, -0.151f, z);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
