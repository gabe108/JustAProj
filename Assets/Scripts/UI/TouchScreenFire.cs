using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchScreenFire : MonoBehaviour
{
    private Image m_image = null;
	// Use this for initialization
	void Start ()
    {
        m_image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.touchCount > 0)
        {
            bool fire = false;
            foreach (Touch current in Input.touches)
            {
                if (current.position.x >= Screen.width / 2 && fire == false)
                {
                    fire = true;

                    if (current.phase == TouchPhase.Began)
                    {
                        m_image.enabled = true;
                        m_image.transform.localPosition = current.position;
                    }
                    else if (current.phase == TouchPhase.Ended)
                    {
                        m_image.enabled = false;
                    }
                }
            }
        }
        else // TEMP
        {
            Touch fakeTouch;

            if (Input.GetMouseButton(0))
            {
                fakeTouch = new Touch();
                fakeTouch.fingerId = 10;
                fakeTouch.position = Input.mousePosition;
                fakeTouch.deltaTime = Time.deltaTime;
                fakeTouch.deltaPosition = new Vector2(0, 0);
                fakeTouch.phase = (Input.GetMouseButtonDown(0) ? TouchPhase.Began :
                                    (fakeTouch.deltaPosition.sqrMagnitude > 1f ? TouchPhase.Moved : TouchPhase.Stationary));

                if (fakeTouch.position.x >= Screen.width / 2)
                {
                    if (fakeTouch.phase == TouchPhase.Began)
                    {
                        m_image.enabled = true;
                        transform.position = fakeTouch.position;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_image.enabled = false;
            }
        }
    }
}
