using UnityEngine;
using System.Collections;

/// <summary>
/// 控制角色生成及位移.
/// </summary>
public class PlayerController : MonoBehaviour {

	public GameObject m_prefabBlood;
	public GameObject m_prefab_BGM_Gem;
	public GameObject m_prefab_BGM_Faling;
	public GameObject m_prefab_BGM_Moving;


	private MapManager m_MapManager;
	private UIManager m_UIMaganer;
	private CameraFlow m_CameraFlow;
	private TileSpike m_TileSpike;
	private SkySpike m_SkySpike;

	private Transform m_Transform;

	private Vector3 m_position;


	private Vector3 rot = new Vector3 (0f, 45f, 0f);


	public int z = 9;

	private int x = 2;
	private int yushu;

	private int gemcount=0;
	public int scorecount=0;

	private bool life = true;

	private bool ispause = false;

	//藏蓝.深色
	private Color colorone= new Color(37/255f,56/255f,107/255f);
	//藏蓝+5.浅色
	private Color colortwo= new Color(42f/255f,61f/255f,112f/255f);

	void Start () {
		

		//获取存储数据.
		gemcount = PlayerPrefs.GetInt ("gem",0);

		m_Transform = gameObject.GetComponent<Transform> ();
		m_position = m_Transform.position;
		m_MapManager = GameObject.Find ("MapManager").GetComponent<MapManager> ();
		m_UIMaganer = GameObject.Find ("UI Root").GetComponent<UIManager> ();
		m_CameraFlow = GameObject.Find ("Main Camera").GetComponent<CameraFlow> ();

	}
	

	void Update () {
		
		if (Input.GetKeyDown (KeyCode.M))
			StartGame ();
		if (life) 
		{
			
			SetNextTransform ();

		}
	
	}
	/// <summary>
	/// 生成人物，设置初始坐标.
	/// </summary>
	public void StartGame()
	{	life = true;
		if (life) {
		 
			Transform find_Transform = m_MapManager.mapList [z] [x].GetComponent<Transform> ();

			m_Transform.position = find_Transform.position;

			m_Transform.rotation = Quaternion.Euler (rot);

			m_MapManager.StartCoroutine ("TileDowm");



			m_CameraFlow.StartFlow = true;
		}

		
	}

	/// <summary>
	/// Player向左移动.
	/// </summary>
	public void MoveLeft()
	{	if(life){
			if (!ispause) {

				Instantiate (m_prefab_BGM_Moving, gameObject.transform.position, Quaternion.identity);

				yushu = z % 2;

				if (x > 0) {	

					rot = new Vector3 (0f, -45f, 0f);

					ScoreCount ();

					z++;
					if (yushu == 0)
						x--;

					//Debug.Log (yushu);
					//Debug.Log ("x:" + x);

					SetTransform ();
				}
			}
		}
	}

	/// <summary>
	/// Player向右移动;
	/// </summary>
	public void MoveRight()
	{
		if(life){
			if (!ispause) {

				Instantiate (m_prefab_BGM_Moving, gameObject.transform.position, Quaternion.identity);

				if (x == 4 && yushu == 0 || x != 4) {	

					rot = new Vector3 (0f, 45f, 0f);

					ScoreCount ();

					z++;

					if (yushu == 1) {
						x++;
					}
					//Debug.Log ("yushu:" + yushu);
					//Debug.Log ("z:" + z+"--"+"x:"+x);
					if (x < 5)
						SetTransform ();
				}
			}
		}
	}

	/// <summary>
	/// 游戏开始后的移动.
	/// </summary>
	void SetNextTransform()
	{	yushu = z % 2;
		//向右.

			if (Input.GetKeyDown (KeyCode.D)) {	
				
			MoveRight ();
			}
		//向左.

			if (Input.GetKeyDown (KeyCode.A)) {	
				
			MoveLeft ();
		}
		CreatMap ();			
	}
	/// <summary>
	/// 设置player的状态及蜗牛痕迹.
	/// </summary>
	void SetTransform()
	{
		Transform find_Transform = m_MapManager.mapList [z] [x].GetComponent<Transform> ();

		//主角位置.
		m_Transform.position = find_Transform.position;
		m_Transform.rotation= Quaternion.Euler(rot);

		MeshRenderer find_MeshRenderer = null;

		if(find_Transform.tag=="Tile")
			find_MeshRenderer=find_Transform.FindChild ("normal_a2").GetComponent<MeshRenderer> ();
		else if(find_Transform.tag=="Spike")
			find_MeshRenderer=find_Transform.FindChild ("moving_spikes_a2").GetComponent<MeshRenderer> ();
		else if(find_Transform.tag=="Sky_Spike")
			find_MeshRenderer=find_Transform.FindChild ("smashing_spikes_a2").GetComponent<MeshRenderer> ();
		
		if (find_MeshRenderer != null) {
			//改变颜色（蜗牛痕迹）.
			if (yushu == 1)
				find_MeshRenderer.material.color = colortwo;
			else
				find_MeshRenderer.material.color = colorone;
		} else {
			//gameObject.AddComponent<Rigidbody> ().mass=0.5f;
			gameObject.AddComponent<Rigidbody> ().angularVelocity=new Vector3 (Random.Range (0f, 0.5f), Random.Range (0f, 0.5f), Random.Range (0f, 0.5f)) * Random.Range (1f, 5f);

			 StartCoroutine ("GameOver", true);
		}
	}
	/// <summary>
	/// 生成地图.
	/// </summary>
	void CreatMap()
	{
		if(m_MapManager.mapList.Count-z<11)
		{	
			m_MapManager.AddPr ();
			float offsetz = m_MapManager.mapList [m_MapManager.mapList.Count-1] [0].transform.position.z+m_MapManager.bottonLength*0.5f;
				
			m_MapManager.CreateMapItem (offsetz);
		}
	}
	/// <summary>
	/// 检测碰撞并执行相应逻辑.
	/// </summary>
	/// <param name="coll">Coll.</param>
	private void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Spike_Attack") 
		{	
			
			Instantiate (m_prefabBlood, transform.position, Quaternion.identity);
	
	//		Debug.Log (coll.name);
			life = false;
			StartCoroutine ("GameOver", false);
			m_CameraFlow.StartFlow = false;

			m_Transform.position = new Vector3(0f,0f,0f);

		}
		if (coll.tag == "Gem") 
		{
			Instantiate (m_prefab_BGM_Gem, transform.position, Quaternion.identity);
			GemCount ();
			Destroy (coll.gameObject.transform.parent.gameObject);
		}
	}
	/// <summary>
	/// 游戏结束;
	/// </summary>
	private IEnumerator GameOver(bool falling)
	{
		m_MapManager.StopIE ();
		life = false;
		if (falling) {
			Instantiate (m_prefab_BGM_Faling, transform.position, Quaternion.identity);
			life = false;
			yield return new WaitForSeconds (0.01f);
		}
		//Time.timeScale ;
		Debug.Log ("游戏结束");

		SaveData ();

		StartCoroutine ("ResetGame");
		//Time.timeScale = 0;
	}

	/// <summary>
	/// 等两秒,重置游戏，重新开始.
	/// </summary>
	/// <returns>The game.</returns>
	private IEnumerator ResetGame()
	{
		yield return new WaitForSeconds (1f);

		ResetMyGame ();
	}

	/// <summary>
	/// 宝石+1.
	/// </summary>
	private void GemCount()
	{
		gemcount++;
	//	Debug.Log ("宝石：" + gemcount);
		m_UIMaganer.UpdateData (scorecount,gemcount);
	}
	/// <summary>
	/// 分数+1.
	/// </summary>
	private void ScoreCount()
	{
		scorecount++;
	//	Debug.Log ("分数"+scorecount);
		m_UIMaganer.UpdateData (scorecount,gemcount);
	}
	/// <summary>
	/// 保存游戏数据.
	/// </summary>
	private void SaveData()
	{
		PlayerPrefs.SetInt ("gem",gemcount);
		if (scorecount > PlayerPrefs.GetInt ("scorecount", 0)) 
		{
			PlayerPrefs.SetInt ("scorecount",scorecount);

		}
	}
	/// <summary>
	/// 控制全部重新开始游戏.
	/// </summary>
	private void ResetMyGame()
	{
		m_UIMaganer.ResetUI ();
		ResetPlayer ();

		m_MapManager.ResetMap ();
		m_CameraFlow.ResetCamera ();


	}

	/// <summary>
	/// 重置角色状态.
	/// </summary>
	private void ResetPlayer()
	{
		GameObject.Destroy (gameObject.GetComponent<Rigidbody>());

		z = 9;
		x = 2;
		Vector3 v3=new Vector3(0f,45f,0f);
		m_Transform.position = new Vector3(0.89802f,0f,1.61644f);
		m_Transform.rotation = Quaternion.Euler (v3);
		scorecount = 0;
	}


	/// <summary>
	/// 暂停.
	/// </summary>
	public void Pause()
	{
		ispause = true;
		Time.timeScale = 0;
	}
	/// <summary>
	/// 不暂停.
	/// </summary>
	public void NotPause()
	{
		ispause = false;
		Time.timeScale = 1;
	}
}
