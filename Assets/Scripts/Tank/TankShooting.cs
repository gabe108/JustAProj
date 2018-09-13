using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;    
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 15f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f;

    public Image m_fire = null;

    private bool m_lastFireState = false;

    private string m_FireButton;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired = true;


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
		m_FireButton = "Fire" + m_PlayerNumber; // Gabe's fix

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }


    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.

        //The slider should have the default value of the minimum launch force.
        m_AimSlider.value = m_MinLaunchForce;

        if (m_fire != null)
        {
            //If the max force has been exceeded and the shell hasn't yet been fired,
            if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
            {
                //then fire the shell using the max force
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire();
            }

            //Otherwise, if the fire button has just started being pressed,
            else if ((m_fire.enabled && m_lastFireState == false) || Input.GetButtonDown(m_FireButton))
            {
                //then reset the fired flag and reset the launch force
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;

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
            if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
            {
                //then fire the shell using the max force
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire();
            }

            //Otherwise, if the fire button has just started being pressed,
            else if (Input.GetButtonDown(m_FireButton))
            {
                //then reset the fired flag and reset the launch force
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;

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
    }


    private void Fire()
    {
        // Instantiate and launch the shell.

        //Set the fired flag so that the fire is only called once
        m_Fired = true; //<-- boolean value constantly reffered to in above function

        if (m_fire != null)
            m_fire.enabled = false;

        //Create an instance of the shell and store a reference to it's rigidBody
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        //Set the shells velocity to the launch force in the fire positions forward direction.
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        //Change the audio clip to the firing soundclip and play it
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        //Reset the launch force. This is a safeguard against the event of missing buttons
        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}