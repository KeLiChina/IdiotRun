using UnityEngine;
using System.Collections;

/// <summary>
/// 5秒后删除粒子特效及音效.
/// </summary>
public class BloodPtcLifeTime : MonoBehaviour {


	void Start () {
		Destroy (gameObject, 5f);
	
	}
	

}
