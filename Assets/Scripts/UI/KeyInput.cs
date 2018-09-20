using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyInput : MonoBehaviour {

    public GameObject pauseCanvas;

	// Use this for initialization
	void Start ()
    {
        pauseCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetButtonDown("MenuButton"))
        {
            if (Time.timeScale == 1.0f)
            {
                pauseCanvas.SetActive(true);
                Time.timeScale = 0.0f;
            }

            else
            {
                pauseCanvas.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
	}
}
