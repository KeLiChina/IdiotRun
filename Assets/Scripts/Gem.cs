using UnityEngine;
using System.Collections;
/// <summary>
/// 控制宝石旋转.
/// </summary>
public class Gem : MonoBehaviour {

	private Transform m_Transform;
	private Transform son_Transform;


	void Start () {
		m_Transform = gameObject.GetComponent<Transform>();
		son_Transform = m_Transform.FindChild ("gem 3");
	}
	
	// Update is called once per frame
	void Update () {
		son_Transform.Rotate(new Vector3 (Random.Range (0f, 0.5f), Random.Range (0f, 0.5f), Random.Range (0f, 0.5f)) * Random.Range (1f, 5f));
		//son_Transform.rotation = Quaternion.Euler(new Vector3 (Random.Range (0f, 0.3f), Random.Range (0f, 0.3f), Random.Range (0f, 0.3f)) * Random.Range (1f, 3f));
	}
}
