using UnityEngine;
using System.Collections;
/// <summary>
/// 地面陷阱.
/// </summary>
public class TileSpike : MonoBehaviour {

	private Transform m_Transform;
	private Transform son_Transform;
	private Vector3 son_Position;
	private Vector3 target_Position;



	void Start () {
		m_Transform = gameObject.GetComponent<Transform>();
		son_Transform = m_Transform.FindChild ("moving_spikes_b").GetComponent<Transform>();

		son_Position = son_Transform.position;
		target_Position = son_Position + new Vector3(0,0.15f,0);
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
			yield return new WaitForSeconds (0.5f);
			StopCoroutine ("Up");
			StartCoroutine ("Down");
			yield return new WaitForSeconds (1.8f);
		}
	}

	/// <summary>
	/// 上.
	/// </summary>
	private IEnumerator Up()
	{
		while (true) 
		{
			son_Transform.position = Vector3.Lerp (son_Transform.position, target_Position, Time.deltaTime * 35);
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
			son_Transform.position = Vector3.Lerp (son_Transform.position, son_Position, Time.deltaTime * 35);
			yield return null;
		}
	}

	/// <summary>
	/// 停止地面陷阱协程.
	/// </summary>
	public void UpDownStopIE()
	{
		//son_Transform.StopCoroutine ("UpAndDown");
		StopCoroutine ("Up");
		StopCoroutine ("Down");
		StopAllCoroutines ();

		Debug.Log ("停止地面协程");

	}
}
