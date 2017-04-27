using UnityEngine;
using System.Collections;

public class VoxAmin : MonoBehaviour {

	public AnimationCurve AC;

	public float suofang = 3.0f;

	private Vector3 m_Scale;

	void Start () {
		m_Scale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		float r = AC.Evaluate (Time.time * suofang);
		transform.localScale = new Vector3 (m_Scale.x, m_Scale.y * r, m_Scale.z);
	}
}
