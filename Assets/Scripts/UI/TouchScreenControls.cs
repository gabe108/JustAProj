using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchScreenControls : MonoBehaviour
{
    public GameObject m_stick;
    public GameObject m_stickArea;

    public Image m_fireButton;

    public RectTransform m_cycleLeftTransform;
    public RectTransform m_cycleRightTransform;

    private float m_freeTouchArea;

    [HideInInspector] public GameObject m_tank;

    private Vector3 lastMousePosition = new Vector3(); // TEMP

    // Use this for initialization
    void Start ()
    {
        if (Application.platform != RuntimePlatform.Android || Application.platform != RuntimePlatform.IPhonePlayer)
        {
            gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_freeTouchArea = 180f * (Screen.height / 720f);
        UpdateMovement();
        UpdateFire();
        UpdateCycle();
    }

    private void UpdateMovement()
    {
        if (Input.touchCount > 0)
        {
            bool movement = false;
            foreach (Touch current in Input.touches)
            {
                if (current.position.x < Screen.width / 2 && current.position.y < Screen.height - m_freeTouchArea && movement == false)
                {
                    movement = true;
                    if (current.phase == TouchPhase.Began)
                    {
                        if (current.position.x < Screen.width / 2)
                        {
                            m_stickArea.transform.position = current.position;

                            m_stickArea.GetComponent<Image>().enabled = true;
                            m_stick.GetComponent<Image>().enabled = true;
                            m_stick.transform.localPosition = new Vector3(0, 0, 0);
                        }
                    }
                    else if (current.phase == TouchPhase.Ended)
                    {
                        m_stickArea.GetComponent<Image>().enabled = false;
                        m_stick.GetComponent<Image>().enabled = false;
                    }
                    else if (current.phase == TouchPhase.Moved)
                    {
                        m_stick.transform.localPosition = current.position - (Vector2)m_stickArea.transform.position;

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
                    if (fakeTouch.position.x < Screen.width / 2 && fakeTouch.position.y < Screen.height - m_freeTouchArea)
                    {
                        m_stickArea.transform.position = fakeTouch.position;

                        m_stickArea.GetComponent<Image>().enabled = true;
                        m_stick.GetComponent<Image>().enabled = true;
                        m_stick.transform.localPosition = new Vector3(0, 0, 0);
                    }
                }
                else if (fakeTouch.phase == TouchPhase.Moved)
                {
                    m_stick.transform.localPosition = fakeTouch.position - (Vector2)m_stickArea.transform.position;

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
                m_stickArea.GetComponent<Image>().enabled = false;
                m_stick.GetComponent<Image>().enabled = false;
            }
        }
    }

    private void UpdateFire()
    {
        if (Input.touchCount > 0)
        {
            bool fire = false;
            foreach (Touch current in Input.touches)
            {
                if (current.position.x >= Screen.width / 2 && current.position.y < Screen.height - m_freeTouchArea && fire == false)
                {
                    fire = true;

                    if (current.phase == TouchPhase.Began)
                    {
                        m_fireButton.enabled = true;
                        m_fireButton.transform.localPosition = current.position;
                    }
                    else if (current.phase == TouchPhase.Ended)
                    {
                        m_fireButton.enabled = false;
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

                if (fakeTouch.position.x >= Screen.width / 2 && fakeTouch.position.y < Screen.height - m_freeTouchArea)
                {
                    if (fakeTouch.phase == TouchPhase.Began)
                    {
                        m_fireButton.enabled = true;
                        m_fireButton.transform.position = fakeTouch.position;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_fireButton.enabled = false;
            }
        }
    }

    private void UpdateCycle()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch current in Input.touches)
            {

            }
        }
        else // TEMP
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_cycleLeftTransform.rect.Contains(m_cycleLeftTransform.InverseTransformPoint(Input.mousePosition)))
                {
                    m_tank.GetComponent<TankShooting>().CycleShellLeft();
                }
                else if (m_cycleRightTransform.rect.Contains(m_cycleRightTransform.InverseTransformPoint(Input.mousePosition)))
                {
                    m_tank.GetComponent<TankShooting>().CycleShellRight();
                }
            }
        }
    }
}
