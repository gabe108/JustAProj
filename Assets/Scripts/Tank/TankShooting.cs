﻿using UnityEngine;
using UnityEngine.UI;

public enum ShellType
{
    BASE_SHELL = 0,
    LAND_MINE = 1,
    BALLOON = 2,
}

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    [HideInInspector] public GameObject[] m_Shells;
    [HideInInspector] public int[] m_Ammo;
    public Transform m_FireTransform;    
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 15f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f;

    public Image m_fire = null;

    public ShellTypeDisplay m_shellDisplay = null;

    private bool m_lastFireState = false;

    private string m_FireButton;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired = true;
    private int m_shellIndex;

    private KeyCode m_cycleShellLeft;
    private KeyCode m_cycleShellRight;



    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
		m_FireButton = "Fire" + m_PlayerNumber; // Gabe's fix

        m_MaxLaunchForce = m_Shells[0].GetComponent<BaseShell>().m_maxLaunchForce;
        m_MinLaunchForce = m_Shells[0].GetComponent<BaseShell>().m_minLaunchForce;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

        if (m_PlayerNumber == 1)
        {
            m_cycleShellLeft = KeyCode.Q;
            m_cycleShellRight = KeyCode.E;
        }
        else
        {
            m_cycleShellLeft = KeyCode.Keypad1;
            m_cycleShellRight = KeyCode.Keypad2;
        }

        m_Ammo = new int[m_Shells.Length];

        m_Ammo[(int)ShellType.BASE_SHELL] = -1;
        m_Ammo[(int)ShellType.LAND_MINE] = 2;
        //m_Ammo[(int)ShellType.BALLOON] = 3;
    }


    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.

        //The slider should have the default value of the minimum launch force.
        m_AimSlider.value = m_MinLaunchForce;

        if (m_fire != null)
        {
            //If the max force has been exceeded and the shell hasn't yet been fired,
            if ((m_CurrentLaunchForce >= m_MaxLaunchForce || m_MaxLaunchForce == 0) && !m_Fired)
            {
                //then fire the shell using the max force
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire();
            }

            //Otherwise, if the fire button has just started being pressed,
            else if (((m_fire.enabled && m_lastFireState == false) || Input.GetButtonDown(m_FireButton)) && (m_Ammo[m_shellIndex] > 0 || m_Ammo[m_shellIndex] == -1))
            {
                //then reset the fired flag and reset the launch force
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;

                if (m_Ammo[m_shellIndex] != -1)
                {
                    m_Ammo[m_shellIndex]--;
                }

                //Change the audio clip to the charging up clip and start playing it
                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play();
            }

            //Otherwise, if the fire button is being held and the shell hasn't been launched yet
            else if (((m_fire.enabled && m_lastFireState == true) || Input.GetButton(m_FireButton)) && !m_Fired)
            {
                //Increment the launch force and update the slider
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

                m_AimSlider.value = m_CurrentLaunchForce;
            }

            //Otherwise, if the fire button is released and the shell hasn't been launched yet,
            else if ((!m_fire.enabled || Input.GetButtonUp(m_FireButton)) && !m_Fired)
            {
                //Then you can finally fire the shell
                Fire();
            }

            m_lastFireState = m_fire.enabled;
        }
        else
        {
            //If the max force has been exceeded and the shell hasn't yet been fired,
            if ((m_CurrentLaunchForce >= m_MaxLaunchForce || m_MaxLaunchForce == 0) && !m_Fired)
            {
                //then fire the shell using the max force
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire();
            }

            //Otherwise, if the fire button has just started being pressed,
            else if (Input.GetButtonDown(m_FireButton) && (m_Ammo[m_shellIndex] > 0 || m_Ammo[m_shellIndex] == -1))
            {
                //then reset the fired flag and reset the launch force
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;

                if (m_Ammo[m_shellIndex] != -1)
                {
                    m_Ammo[m_shellIndex]--;
                }

                //Change the audio clip to the charging up clip and start playing it
                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play();
            }

            //Otherwise, if the fire button is being held and the shell hasn't been launched yet
            else if (Input.GetButton(m_FireButton) && !m_Fired)
            {
                //Increment the launch force and update the slider
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

                m_AimSlider.value = m_CurrentLaunchForce;
            }

            //Otherwise, if the fire button is released and the shell hasn't been launched yet,
            else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
            {
                //Then you can finally fire the shell
                Fire();
            }
        }
        if (m_Fired)
        {
            if (Input.GetKeyDown(m_cycleShellLeft))
            {
                CycleShellLeft();
            }
            if (Input.GetKeyDown(m_cycleShellRight))
            {
                CycleShellRight();
            }

        }
    }


    private void Fire()
    {
        // Instantiate and launch the shell.

        //Set the fired flag so that the fire is only called once
        m_Fired = true; //<-- boolean value constantly reffered to in above function

        if (m_fire != null)
            m_fire.enabled = false;

        //Create an instance of the shell and store a reference to it's rigidBody
        //Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        GameObject shellInstance = Instantiate(m_Shells[m_shellIndex]);
        shellInstance.GetComponent<BaseShell>().Fire(m_FireTransform.position, m_FireTransform.rotation, m_CurrentLaunchForce * m_FireTransform.forward, gameObject);

        //Set the shells velocity to the launch force in the fire positions forward direction.
        //shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        //Change the audio clip to the firing soundclip and play it
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        //Reset the launch force. This is a safeguard against the event of missing buttons
        m_CurrentLaunchForce = m_MinLaunchForce;
    }

    public void CycleShellLeft()
    {
        if (m_shellIndex - 1 < 0)
        {
            m_shellIndex = m_Shells.Length - 1;
        }
        else
        {
            m_shellIndex--;
        }
        m_MaxLaunchForce = m_Shells[m_shellIndex].GetComponent<BaseShell>().m_maxLaunchForce;
        m_MinLaunchForce = m_Shells[m_shellIndex].GetComponent<BaseShell>().m_minLaunchForce;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

        m_shellDisplay.ChangeType(m_shellIndex);
    }
    public void CycleShellRight()
    {
        if (m_shellIndex + 1 == m_Shells.Length)
        {
            m_shellIndex = 0;
        }
        else
        {
            m_shellIndex++;
        }
        m_MaxLaunchForce = m_Shells[m_shellIndex].GetComponent<BaseShell>().m_maxLaunchForce;
        m_MinLaunchForce = m_Shells[m_shellIndex].GetComponent<BaseShell>().m_minLaunchForce;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

        m_shellDisplay.ChangeType(m_shellIndex);
    }

    public void AddAmmo(ShellType shellType)
    {
        if ((int)shellType <= m_Ammo.Length)
        {
            m_Ammo[(int)shellType]++;
        }
    }
}