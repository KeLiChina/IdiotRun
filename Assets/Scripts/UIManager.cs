using UnityEngine;
using System.Collections;
/// <summary>
/// UI管理器.
/// </summary>
public class UIManager : MonoBehaviour {

	private GameObject m_Start_UI;
	private GameObject m_Game_UI;
	private GameObject m_Play_btn;
	private GameObject m_Left;
	private GameObject m_Right;
	private GameObject m_Pause;
	private GameObject m_NotPause;
	private GameObject m_MainCamera;
	private GameObject m_Vol;
	private GameObject m_VolDown;
	private GameObject m_Exit;
	private GameObject m_Help;
	private GameObject m_Banne;
	private GameObject m_HelpLabel;

	private GameObject m_HelpBack;

	public GameObject m_BGM_StartGame;
	public GameObject m_BGM_Pause;
	public GameObject m_BGM_Val;
	public GameObject m_BGM_Exit;

	private PlayerController m_PlayerController;

	private UILabel m_ScoreLabel;
	private UILabel m_GameScoreLabel;
	private UILabel m_GemLabel;
	private UILabel m_GameGemLabel;
	private UILabel m_NormalScore;


	void Start () {
		m_HelpBack = GameObject.Find ("HelpBack");
		m_Help = GameObject.Find ("Help");
		m_HelpLabel = GameObject.Find ("HelpLabel");
		m_Banne = GameObject.Find ("Banne");

		m_Exit = GameObject.Find ("Exit");

		m_Vol = GameObject.Find ("Vol");
		m_VolDown = GameObject.Find ("VolDown");

		m_MainCamera = GameObject.Find ("Main Camera");

		m_Start_UI = GameObject.Find ("Start_UI");
		m_Game_UI = GameObject.Find ("Game_UI");

		m_Left = GameObject.Find ("Left");
		m_Right = GameObject.Find ("Right");

		m_Play_btn = GameObject.Find ("Play_btn");

		m_Pause = GameObject.Find ("Pause");
		m_NotPause = GameObject.Find ("NotPause");

		m_NormalScore = GameObject.Find ("NormalScore").GetComponent<UILabel> ();
		m_ScoreLabel = GameObject.Find ("ScoreLabel").GetComponent<UILabel> ();
		m_GemLabel = GameObject.Find ("GemLabel").GetComponent<UILabel> ();
		m_GameScoreLabel = GameObject.Find ("GameScoreLabel").GetComponent<UILabel> ();
		m_GameGemLabel = GameObject.Find ("GameGemLabel").GetComponent<UILabel> ();

		m_PlayerController = GameObject.Find ("Jing.wu").GetComponent<PlayerController> ();

	
		m_Vol.SetActive (true);

		m_VolDown.SetActive (false);

		m_Game_UI.SetActive (false);
		m_HelpBack.SetActive (false);
		m_HelpLabel.SetActive (false);
		m_Banne.SetActive (true);

		Init ();

		UIEventListener.Get (m_Play_btn).onClick = PlayButtonClick;

		UIEventListener.Get (m_Left).onClick = left;
		UIEventListener.Get (m_Right).onClick = Right;

		UIEventListener.Get (m_Pause).onClick = Pause;
		UIEventListener.Get (m_NotPause).onClick = NotPause;

		UIEventListener.Get (m_Vol).onClick = TrunVol;
		UIEventListener.Get (m_VolDown).onClick = TrunVolDown;

		UIEventListener.Get (m_Exit).onClick = Exit;

		UIEventListener.Get (m_Help).onClick = Help;
		UIEventListener.Get (m_HelpBack).onClick = HelpBack;
	}


	void Update () {
		

	}

	/// <summary>
	/// 初始化分数和宝石.
	/// </summary>
	private void Init ()
	{

		if(PlayerPrefs.GetInt ("scorecount", 0)>m_PlayerController.scorecount)
			m_ScoreLabel.text = ("" + PlayerPrefs.GetInt ("scorecount", 0));
		else
			m_ScoreLabel.text = ("" + m_PlayerController.scorecount);

		m_GemLabel.text = (PlayerPrefs.GetInt ("gem", 0)+"/100");


		m_GameScoreLabel.text = ("0");
		m_GameGemLabel.text = (PlayerPrefs.GetInt ("gem", 0)+"/100");
	//	m_NormalScore.text = ("0");
	}




	/// <summary>
	/// 更新数据.
	/// </summary>
	/// <param name="score">Score.</param>
	/// <param name="gem">Gem.</param>
	public void UpdateData(int score,int gem)
	{
		m_GameScoreLabel.text = score.ToString();
		m_GameGemLabel.text = gem+"/100";
		m_GemLabel.text = gem+"/100";
	}
	/// <summary>
	/// Play_btn按钮点击开始，UI跳转.
	/// 
	/// </summary>
	/// <param name="go">Go.</param>
	private void PlayButtonClick(GameObject go)
	{
		Debug.Log ("点击开始按钮");

		Instantiate (m_BGM_StartGame, m_MainCamera.transform.position, Quaternion.identity);

		m_Start_UI.SetActive (false);

		m_Game_UI.SetActive (true);

		m_NotPause.SetActive (false);



		m_PlayerController.StartGame ();

	}
	/// <summary>
	/// 点击向左移动
	/// </summary>
	/// <param name="go">Go.</param>
	private void left(GameObject go)
	{
		m_PlayerController.MoveLeft ();
	}
	/// <summary>
	/// 点击向右移动
	/// </summary>
	/// <param name="go">Go.</param>
	private void Right(GameObject go)
	{
		m_PlayerController.MoveRight ();
	}

	/// <summary>
	///重置UI.
	/// </summary>
	public void ResetUI()
	{
		m_HelpLabel.SetActive (false);
		m_Banne.SetActive (true);
		m_HelpBack.SetActive (false);
		m_Start_UI.SetActive (true);
		m_Game_UI.SetActive (false);
		m_NormalScore.text = m_PlayerController.scorecount.ToString();
		m_GameScoreLabel.text = ("0");

		if(PlayerPrefs.GetInt ("scorecount", 0)>m_PlayerController.scorecount)
			m_ScoreLabel.text = ("" + PlayerPrefs.GetInt ("scorecount", 0));
		else
			m_ScoreLabel.text = ("" + m_PlayerController.scorecount);
	}
	/// <summary>
	/// 暂停.
	/// </summary>
	private void Pause(GameObject go)
	{
		Instantiate (m_BGM_Pause, m_MainCamera.transform.position, Quaternion.identity);

		m_PlayerController.Pause ();
		m_Pause.SetActive (false);
		m_NotPause.SetActive (true);
	}
	/// <summary>
	/// 不暂停.
	/// </summary>
	private void NotPause(GameObject go)
	{
		Instantiate (m_BGM_Pause, m_MainCamera.transform.position, Quaternion.identity);

		m_PlayerController.NotPause ();
		m_Pause.SetActive (true);
		m_NotPause.SetActive (false);
	}
	/// <summary>
	/// 关音量.
	/// </summary>
	/// <param name="go">Go.</param>
	private void TrunVol(GameObject go)
	{
		m_MainCamera.GetComponent<AudioSource> ().volume = 0f;

		Instantiate (m_BGM_Val, m_MainCamera.transform.position, Quaternion.identity);

		m_Vol.SetActive (false);
		m_VolDown.SetActive (true);
	}

	/// <summary>
	/// 开音量.
	/// </summary>
	/// <param name="go">Go.</param>
	private void TrunVolDown(GameObject go)
	{
		m_MainCamera.GetComponent<AudioSource> ().volume = 0.25f;

		Instantiate (m_BGM_Val, m_MainCamera.transform.position, Quaternion.identity);

		m_VolDown.SetActive (false);
		m_Vol.SetActive (true);
	}

	/// <summary>
	/// 退出APP.
	/// </summary>
	/// <param name="go">Go.</param>
	private void Exit(GameObject go)
	{
		Instantiate (m_BGM_Exit, m_MainCamera.transform.position, Quaternion.identity);
		Application.Quit();
	}

	/// <summary>
	/// 点击获取帮助.
	/// </summary>
	/// <param name="go">Go.</param>
	private void Help(GameObject go)
	{
		Instantiate (m_BGM_Exit, m_MainCamera.transform.position, Quaternion.identity);
		m_HelpLabel.SetActive (true);
		m_Banne.SetActive (false);
		m_HelpBack.SetActive (true);
		m_Help.SetActive (false);
	}
	/// <summary>
	/// 点击获取帮助退出.
	/// </summary>
	/// <param name="go">Go.</param>
	private void HelpBack(GameObject go)
	{
		Instantiate (m_BGM_Exit, m_MainCamera.transform.position, Quaternion.identity);
		m_HelpLabel.SetActive (false);
		m_Banne.SetActive (true);
		m_Help.SetActive (true);
		m_HelpBack.SetActive (false);
	}

}
