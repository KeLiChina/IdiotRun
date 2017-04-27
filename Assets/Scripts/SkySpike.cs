using UnityEngine;
using System.Collections;
/// <summary>
/// 天空陷阱.
/// </summary>
public class SkySpike : MonoBehaviour {

	private Transform m_Transform;
	private Transform son_Transform;
	private Vector3 son_Position;
	private Vector3 target_Position;

	void Start () {
		m_Transform = gameObject.GetComponent<Transform>();
		son_Transform = m_Transform.FindChild ("smashing_spikes_b").GetComponent<Transform>();

		son_Position = son_Transform.position;
		target_Position = son_Position + new Vector3(0,0.6f,0);
		StartCoroutine ("UpAndDown");
	}


	void Update () {

	}
	/// <summary>
	/// 开启携程.
	/// </summary>
	/// <returns>The and down.</returns>
	private IEnumerator UpAndDown()
	{
		while (true) 
		{
			StopCoroutine ("Down");
			StartCoroutine ("Up");
			yield return new WaitForSeconds (1.6f);
			StopCoroutine ("Up");
			StartCoroutine ("Down");
			yield return new WaitForSeconds (0.3f);
		}
	}

	/// <summary>
	/// 上.
	/// </summary>
	private IEnumerator Up()
	{
		while (true) 
		{
			son_Transform.position = Vector3.Lerp (son_Transform.position, target_Position, Time.deltaTime * 40);
			yield return null;
		}
	}
	/// <summary>
	/// 下.
	/// </summary>
	private IEnumerator Down()
	{
		while (true) 
		{
			son_Transform.position = Vector3.Lerp (son_Transform.position, son_Position, Time.deltaTime * 40);
			yield return null;
		}
	}
	/// <summary>
	/// 停止天空陷阱协程.
	/// </summary>
	public void UpDownStopIE()
	{
		

		StopCoroutine ("UpAndDown");
		StopCoroutine ("Up");
		StopCoroutine ("Down");

		Debug.Log ("停止天空协程");
	}
}
