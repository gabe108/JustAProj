using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
	public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
	public float m_MinSize = 6.5f;                  // The smallest orthographic size the camera can be.
	[HideInInspector] public Transform[] m_Targets;					// All the targets the camera needs to encompass.


	private Camera m_Camera = null;                        // Used for referencing the camera.
	private float m_ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
	private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
	private Vector3 m_DesiredPosition;              // The position the camera is moving towards.

	private void Awake()
    {
		m_Camera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {
		Move();
		Zoom();
    }


    private void Move()
    {
		FindAveragePosition();

		transform.position = Vector3.SmoothDamp(
			transform.position, 
			m_DesiredPosition,
			ref m_MoveVelocity, 
			m_DampTime);
    }


    private void FindAveragePosition()
    {
		Vector3 averagePos = new Vector3();
		int numTargets = 0;

		for(int i = 0; i < m_Targets.Length; i++)
		{
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			averagePos += m_Targets[i].position;
			numTargets++;
		}

		if (numTargets > 0)
			averagePos /= numTargets;

		averagePos.y = transform.position.y;

		m_DesiredPosition = averagePos;
    }


    private void Zoom()
    {
		float requiredSize = FindRequiredSize();
		m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
	}


    private float FindRequiredSize()
    {
		Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);
		float size = 0.0f;

		for (int i = 0; i < m_Targets.Length; i++)
		{
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);
			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);

		}

		size += m_ScreenEdgeBuffer;
		size = Mathf.Max(size, m_MinSize);

		return size;
    }


    public void SetStartPositionAndSize()
    {
		FindAveragePosition();

		transform.position = m_DesiredPosition;

		m_Camera.orthographicSize = FindRequiredSize();
	}
}