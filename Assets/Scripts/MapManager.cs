using UnityEngine;
using System.Collections;
//应用List命名空间.
using System.Collections.Generic;

/// <summary>
/// 地图管理器
/// </summary>
public class MapManager : MonoBehaviour {
	
	public GameObject m_prefab_BGM_Faling;

	private GameObject m_prefab_tile;
	private GameObject m_prefab_wall;
	private GameObject m_prefab_moving_spikes;
	private GameObject m_prefab_smashing_spikes;
	private GameObject m_prefab_gem;

	private PlayerController m_PlayerController;

	//地图数据存储.
	public List<GameObject[]> mapList = new List<GameObject[]> ();

	private Transform m_Transform;

	public float bottonLength = 0.254f * Mathf.Sqrt (2);

	private int index=0;
	private int pr_hole = 0;
	private int pr_tile_spike = 0;
	private int pr_sky_spike = 0;
	private int pr_gem = 0;

	//花青.
	private Color colorOne = new Color (84/255f,107/255f,131/255f);
	//沙青.
	private Color colorTwo = new Color (43/255f,94/255f,125/255f);
	//藏蓝.
	private Color colorThree = new Color (37/255f,56/255f,107/255f);


	void Start () {
		//文件夹中加载prefabs.
		m_prefab_tile = Resources.Load ("tile_white") as GameObject;
		m_prefab_wall = Resources.Load ("wall2") as GameObject;
		m_prefab_moving_spikes= Resources.Load ("moving_spikes") as GameObject;
		m_prefab_smashing_spikes = Resources.Load ("smashing_spikes") as GameObject;
		m_prefab_gem = Resources.Load ("gem 2") as GameObject;


		m_Transform = gameObject.GetComponent<Transform> ();
		m_PlayerController = GameObject.Find ("Jing.wu").GetComponent<PlayerController> ();

		CreateMapItem (0);
	}

	/// <summary>
	/// 创建地图元素.
	/// </summary>
	public void CreateMapItem(float offsetz)
	{	
		//juli = mapList.

		//生成第一排.
		for(int i=0; i < 10; i++)
		{	
			//定义一个数组.
			GameObject[] item = new GameObject[6];
			for(int j=0; j < 6; j++)
			{
				Vector3 pos = new Vector3 (j*bottonLength,0.0f,i*bottonLength+offsetz );
				Vector3 rot = new Vector3 (-90, 45, 0);
				GameObject tile = null;
				//实例化地板.
				if (j == 0 || j == 5)
				{//生成墙.
					tile= GameObject.Instantiate (m_prefab_wall, pos, Quaternion.Euler (rot)) as GameObject;
					tile.GetComponent<MeshRenderer> ().material.color = colorThree;
				} 
				else //生成地面和陷阱.
				{	
					int calcpr =CalcPr();
					int calcprgem = CalcPrGem ();


					if (calcpr == 0) {
						//生产陷阱.
						tile = GameObject.Instantiate (m_prefab_tile, pos, Quaternion.Euler (rot)) as GameObject;
					
						//改变颜色.

						tile.GetComponent<Transform> ().FindChild ("normal_a2").GetComponent<MeshRenderer> ().material.color = colorOne;
						tile.GetComponent<MeshRenderer> ().material.color = colorOne;
						//生成奖励物体.
						if (calcprgem == 1) 
						{
							GameObject gem =	GameObject.Instantiate (m_prefab_gem, pos + new Vector3 (0f, 0.06f, 0), Quaternion.identity) as GameObject;
							gem.GetComponent<Transform> ().SetParent (tile.GetComponent<Transform>());
						}

					}
					else if(calcpr==1){
						//生成空洞.
						tile = new GameObject();
						tile.transform.position=pos;
						tile.transform.rotation=Quaternion.Euler (rot);
					}
					else if(calcpr==2)
						//生成地面陷阱.
						tile = GameObject.Instantiate (m_prefab_moving_spikes, pos, Quaternion.Euler (rot)) as GameObject;
					else if(calcpr==3)
						//生产天空陷阱.
						tile = GameObject.Instantiate (m_prefab_smashing_spikes, pos, Quaternion.Euler (rot)) as GameObject;
						
				}
					//设为子物体.
					tile.GetComponent<Transform> ().SetParent (m_Transform);
					//生成数组.
					item [j] = tile;

			}
			//加入集合.
			mapList.Add (item);
			//生成第二排.
			GameObject[] item2= new GameObject[5];
			for(int j=0;j < 5; j++)
			{
				Vector3 pos = new Vector3 (j*bottonLength+bottonLength/2,0.0f,i*bottonLength+bottonLength/2+offsetz );
				Vector3 rot = new Vector3 (-90, 45, 0);
				//生成地面和陷阱.

				GameObject tile = null;

				int pr =CalcPr();
				int calcprgem = CalcPrGem ();

				if (pr == 0) {	
					//生成地面
					tile = GameObject.Instantiate (m_prefab_tile, pos, Quaternion.Euler (rot)) as GameObject;

					//改变颜色.

					tile.GetComponent<Transform> ().FindChild ("normal_a2").GetComponent<MeshRenderer> ().material.color = colorTwo;
					tile.GetComponent<MeshRenderer> ().material.color = colorTwo;
					//生成奖励物体.
					if (calcprgem == 1) 
					{
						GameObject gem =	GameObject.Instantiate (m_prefab_gem, pos + new Vector3 (0f, 0.06f, 0), Quaternion.identity) as GameObject;
						gem.GetComponent<Transform> ().SetParent (tile.GetComponent<Transform>());
					}
				}
				else if(pr==1){
					//生成空洞.
					tile = new GameObject();
					tile.transform.position=pos;
					tile.transform.rotation=Quaternion.Euler (rot);
				}
				else if(pr==2)
					//生产地面陷阱.
					tile = GameObject.Instantiate (m_prefab_moving_spikes, pos, Quaternion.Euler (rot)) as GameObject;
				else if(pr==3)
					//生成天空陷阱.
					tile = GameObject.Instantiate (m_prefab_smashing_spikes, pos, Quaternion.Euler (rot)) as GameObject;


				tile.GetComponent<Transform>().SetParent(m_Transform);

				item2 [j] = tile;

			}
			mapList.Add (item2);

		}
		//生成第二排.

	}


	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			string str = "";
			for (int i = 0; i < mapList.Count; i++) 
			{
				for (int j = 0; j < mapList [i].Length; j++) 
				{
					str += mapList [i] [j].name;
					mapList [i] [j].name = i + "--" + j;
				}
				str += "\n";
			}
			Debug.Log (str);
		}
	}
	/// <summary>
	/// 开始协程.
	/// </summary>
	private void StartIE()
	{
		StartCoroutine ("TileDowm");
	}
	/// <summary>
	/// 停止协程.
	/// </summary>
	public void StopIE()
	{
		StopCoroutine ("TileDowm");
	}

	/// <summary>
	/// 协程给地面加刚体，塌陷.
	/// </summary>
	private IEnumerator TileDowm()
	{	while(true)
		{
		yield return new WaitForSeconds (0.215f);
			for (int i = 0; i < mapList [index].Length; i++) 
			{
				Rigidbody rb= mapList [index] [i].AddComponent<Rigidbody> ();
				//掉落时随机旋转.
				if (rb.tag == "Spike") 
				{
					Debug.Log ("地面陷阱掉落");

					rb.gameObject.GetComponent<TileSpike> ().StopAllCoroutines ();

					Destroy (rb.transform.FindChild ("moving_spikes_b").gameObject);

					Debug.Log ("地面陷阱掉落结束");

				}
				if (rb.tag == "Sky_Spike") 
				{
					Debug.Log ("天空陷阱掉落");

					rb.gameObject.GetComponent<SkySpike> ().StopAllCoroutines ();
					Destroy (rb.transform.FindChild ("smashing_spikes_b").gameObject);

					Debug.Log ("天空陷阱掉落结束");

				}
					
				rb.angularVelocity = new Vector3 (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f)) * Random.Range (1f, 10f);
				GameObject.Destroy (mapList[index][i],1.0f);
			}
			if (index == m_PlayerController.z) 
			{
				Instantiate (m_prefab_BGM_Faling, transform.position, Quaternion.identity);
				StopIE ();

				m_PlayerController.gameObject.AddComponent<Rigidbody> ().angularVelocity=new Vector3 (Random.Range (0f, 0.5f), Random.Range (0f, 0.5f), Random.Range (0f, 0.5f)) * Random.Range (1f, 5f);

				m_PlayerController.StartCoroutine ("GameOver",true);
			}
			index++;
		}
	}

	/// <summary>
	/// 计算路障生成慨率
	/// 1.生产塌陷.
	/// 2.生成地面陷阱.
	/// 3.生产天空陷阱.
	///  0.正常.
	/// </summary>

	private int CalcPr()
	{
		int pr = Random.Range (0, 100);
		if (pr < pr_hole)
			return 1;
		else if (30 < pr && pr< 30+pr_tile_spike)
			return 2;
		else if (60 < pr && pr < 60+pr_sky_spike)
			return 3;
		return 0;
	}

	/// <summary>
	/// 计算奖励生成概率.
	/// </summary>
	/// <returns>The pr gem.</returns>
	private int CalcPrGem()
	{
		int pr = Random.Range (0, 99);

		if (pr < pr_gem)
			return 1;
		else
			return 0;
	}
	/// <summary>
	/// 增加概率.
	/// </summary>
	public void AddPr()
	{	if (pr_hole < 10)
			pr_hole += 3;
		if (pr_tile_spike < 13)
			pr_tile_spike += 3;
		if (pr_sky_spike < 13)
			pr_sky_spike += 3;
		if (pr_gem < 2)
			pr_gem += 2;
	}

	/// <summary>
	/// 删除当前地图.
	/// 重置概率.
	/// 重置塌陷脚标index；
	/// 重新创建地图.
	/// </summary>
	public void ResetMap()
	{
		Transform[] sonTransform = m_Transform.GetComponentsInChildren<Transform> ();
		for (int i = 1; i < sonTransform.Length; i++)
			GameObject.Destroy (sonTransform [i].gameObject);
		
		pr_hole = 0;
		pr_tile_spike = 0;
		pr_sky_spike = 0;
		pr_gem = 0;

		index = 0;
		mapList.Clear ();

		CreateMapItem (0);
		//for(int i)
		//Destroy (gameObject);
	}

}
