using UnityEngine;
using System.Collections;

/// <summary>
///摄像机跟随.
/// </summary>
public class CameraFlow : MonoBehaviour {

	private Transform m_Transform;
	private Transform player_Transform;

	private Vector3 m_position;

	public bool StartFlow=true;

	void Start () {
		m_Transform = gameObject.GetComponent<Transform> ();
		player_Transform = GameObject.Find("Jing.wu").GetComponent<Transform> ();
		m_position = m_Transform.position;
	}
	

	void Update () {
		if(StartFlow)
			SetToNextPosition ();
	}

	/// <summary>
	/// 摄像机移动.
	/// </summary>
	void SetToNextPosition()
	{
		Vector3 nextPosition = new Vector3 (m_Transform.position.x,m_Transform.position.y,player_Transform.position.z-1.226446f);
		m_Transform.position = Vector3.Lerp (m_Transform.position,nextPosition,Time.deltaTime);
	}

	/// <summary>
	/// 摄像机重置.
	/// </summary>
	public void ResetCamera()
	{	StartFlow = false;
		m_Transform.position = m_position;
	//	Debug.Log("1111111111");
	}
}
