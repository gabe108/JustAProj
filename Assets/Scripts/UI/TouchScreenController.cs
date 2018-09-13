using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchScreenController : MonoBehaviour
{
    public GameObject m_stick = null;

    private Vector3 lastMousePosition = new Vector3(); // TEMP
    private Image m_image = null;
	// Use this for initialization
	void Start ()
    {
        m_image = GetComponent<Image>();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            bool movement = false;
            foreach (Touch current in Input.touches)
            {
                if (current.position.x < Screen.width / 2 && movement == false)
                {
                    movement = true;
                    if (current.phase == TouchPhase.Began)
                    {
                        if (current.position.x < Screen.width / 2)
                        {
                            transform.position = current.position;

                            m_image.enabled = true;
                            m_stick.GetComponent<Image>().enabled = true;
                            m_stick.transform.localPosition = new Vector3(0, 0, 0);
                        }
                    }
                    else if (current.phase == TouchPhase.Ended)
                    {
                        m_image.enabled = false;
                        m_stick.GetComponent<Image>().enabled = false;
                    }
                    else if (current.phase == TouchPhase.Moved)
                    {
                        m_stick.transform.localPosition = current.position - (Vector2)transform.position;

                        if (m_stick.transform.localPosition.magnitude > 50)
                        {
                            Vector3 position = m_stick.transform.localPosition;
                            position.Normalize();
                            position *= 50;
                            m_stick.transform.localPosition = position;
                        }
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
                fakeTouch.deltaPosition = Input.mousePosition - lastMousePosition;
                fakeTouch.phase = (Input.GetMouseButtonDown(0) ? TouchPhase.Began :
                                    (fakeTouch.deltaPosition.sqrMagnitude > 1f ? TouchPhase.Moved : TouchPhase.Stationary));

                if (fakeTouch.phase == TouchPhase.Began)
                {
                    if (fakeTouch.position.x < Screen.width / 2)
                    {
                        transform.position = fakeTouch.position;

                        m_image.enabled = true;
                        m_stick.GetComponent<Image>().enabled = true;
                        m_stick.transform.localPosition = new Vector3(0, 0, 0);
                    }
                }
                else if (fakeTouch.phase == TouchPhase.Ended)
                {
                    gameObject.SetActive(false);
                    m_image.enabled = false;
                }
                else if (fakeTouch.phase == TouchPhase.Moved)
                {       
                    m_stick.transform.localPosition = fakeTouch.position - (Vector2)transform.position;

                    if (m_stick.transform.localPosition.magnitude > 50)
                    {
                        Vector3 position = m_stick.transform.localPosition;
                        position.Normalize();
                        position *= 50;
                        m_stick.transform.localPosition = position;
                    }
                }

                lastMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_image.enabled = false;
                m_stick.GetComponent<Image>().enabled = false;
            }
        }
    }
}
