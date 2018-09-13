using UnityEngine;
public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;
    public UnityEngine.UI.Image m_touchMovement = null;

    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private float m_OriginalPitch;
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }
    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }
    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;
        m_OriginalPitch = m_MovementAudio.pitch;
    }
    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        //m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        //m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        if (m_touchMovement != null)
        {
            if (m_touchMovement.enabled)
            {
                m_MovementInputValue = m_touchMovement.transform.localPosition.y / 50;
                m_TurnInputValue = m_touchMovement.transform.localPosition.x / 50;
            }
            else
            {
                m_MovementInputValue = 0;
                m_TurnInputValue = 0;
            }
        }

        if ((m_MovementInputValue == 0 && m_TurnInputValue == 0) || m_touchMovement == null)
        {
            m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
            m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
        }

        EngineAudio();
    }
    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        //If the tank isn't currently moving
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            //If the current audio clip is the movement clip
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                //Then change the clip to the idle clip and play it
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            //Otherwise, if the idling audip is playing and the tank starts moving, change the audio to the driving clip
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }
    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
        //Now that's a hella simple function
    }
    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        //Creation of a vector using the direction the tank is currently facing. C# is stupidly simple I hate it
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        //Applying the movement to the object
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }
    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
}