using UnityEngine;
using System.Collections;
using System.IO; //System.IO.FileInfo, System.IO.StreamReader, System.IO.StreamWriter
using System; //Exception
using UnityEngine.UI;

public class MotionController : MonoBehaviour {
	private GameObject objWWWLoader = null;
	private WWWLoader WD = null;
	private int nFrameNum = 0;
	private FrameData[] data = null;
	public static int nCurrentFrame = 0; //現在のフレーム番号
	private Vector3[] data3 = null;
	private GameObject[] gameobjNode = null;
	private int[] sph_data = null;
	private Vector2[] data4 = null;
	private GameObject[] gameobjLink = null;
	private int state = -1;
	private SliderController sc = null;
	private string fileName = null;
	private int sphNum = -1;
	private int linkNum = -1;

	public InputField IF = null;

	public Transform Field = null;

	private MotionDataEntity motionData = null;

	// Use this for initialization
	void Start () {
		fileName = MotionData.Instance.fileName;
	}
	
	// Update is called once per frame
	void Update () {
		int df = 0;
		if (state == -1) { //待機中
			StartFileLoad();
		} else if (state == -2) { //sphロード開始
			//フィールドの角度をもとに戻す
			Field.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
			LoadSphData (fileName);
			state = -3;
		} else if (state == -3) { //sphロード中
			if (WD.flag == true) { //sphロード終了
				Debug.Log (WD.GetResponse ());
				WD.flag = false;
				InputSphData ();
				state = -4;
			}
		} else if (state == -4) { //linkロード開始
			LoadLinkData (fileName);
			state = -5;
		} else if (state == -5) { //linkロード中
			if (WD.flag == true) { //linkロード終了
				Debug.Log (WD.GetResponse ());
				WD.flag = false;
				InputLinkData ();
				state = -6;
			}
		} else if (state == -6) { //trcロード開始
			LoadTrcData (fileName);
			state = -7;
		} else if (state == -7) { //trcロード中
			if (WD.flag == true) { //trcロード終了
				Debug.Log (WD.GetResponse ());
				WD.flag = false;
				InputTrcData ();
				state = 0;
			}
		} else {
			if (state == 0) { //停止
				df = 0;
			} else if (state == 1) { //再生
				df = 1;
			} else if (state == 2) { //1コマ戻る
				df = -1;
				state = 0;
			} else if (state == 3) { //1コマ進む
				df = 1;
				state = 0;
			}
			nCurrentFrame += df;
			if (nCurrentFrame >= nFrameNum - 1) {
				nCurrentFrame = 0;
			}
			//スライダーの位置を設定
			if (state >= 0) {
				sc.SetCurrentFramePos (nCurrentFrame);
			}

			//描画
			if (state >= 0) {
				for (int i = 0; i < sphNum; i++) {
					Point3D p = data [nCurrentFrame].getPoint (sph_data [i]);
					Vector3 p2 = new Vector3 ((float)p.x / 2000.0f * 5.0f, (float)p.y / 2000.0f * 5.0f, (float)p.z / 2000.0f * 5.0f);
					gameobjNode [i].transform.localPosition = p2;
					data3 [i] = p2;
				}
				for (int i = 0; i < linkNum; i++) {
					LineRenderer line = gameobjLink [i].GetComponent<LineRenderer> ();
					line.transform.parent = this.Field;
					line.SetVertexCount (2);
					line.SetPosition (0, data3 [(int)data4 [i].x]);
					line.SetPosition (1, data3 [(int)data4 [i].y]);
				}
			}
		}
	}

	//ボタン関係
	public void frameStop(){
		state = 0;
	}

	public void frameStart(){
		state = 1;
	}

	public void backOneFrame(){
		state = 2;
	}

	public void fowardOneFrame(){
		state = 3;
	}

	public void StartFileLoad(){
		if (state != -1) {
			for (int i = 0; i < gameobjNode.Length; i++) {
				Destroy (gameobjNode [i]);
			}
			for (int i = 0; i < gameobjLink.Length; i++) {
				Destroy (gameobjLink[i]);
			}
		}
		if (fileName != null) {
			nCurrentFrame = 0;
			state = -2;
		}
	}

	public void SetFirstFrame (){
		state = 0;
		nCurrentFrame = 0;
		sc.SetCurrentFramePos (nCurrentFrame);
	}

	public void SetEndFrame (){
		state = 0;
		nCurrentFrame = nFrameNum - 2;
		sc.SetCurrentFramePos (nCurrentFrame);
	}

	//フレーム位置セット用
	public void SetFrameNum (int num){
		if (nFrameNum > num && num >= 0) {
			nCurrentFrame = num;
		}
	}

	//SPHファイルの読み込み
	private void LoadSphData(string fileName){
		string url = "file:///D:\\kousen\\MOCAP_DATA\\tmp\\"+fileName;

		//SPHデータの読み込み
		objWWWLoader = GameObject.Find ("WWWLoader");
		WD = objWWWLoader.GetComponent<WWWLoader> ();
		WD.SetURL (url + ".sph");
		WD.StartHttpGet ();
	}

	//TRCデータの読み込み
	private void LoadTrcData(string fileName){
		string url = "file:///D:\\kousen\\MOCAP_DATA\\tmp\\"+fileName;

		//TRCデータの読み込み
		WD.SetURL (url + ".trc");
		WD.StartHttpGet ();
	}

	//LINKデータの読み込み
	private void LoadLinkData(string fileName){
		string url = "file:///D:\\kousen\\MOCAP_DATA\\tmp\\"+fileName;

		//TRCデータの読み込み
		WD.SetURL (url + ".lin");
		WD.StartHttpGet ();
	}

	//生データからSPHデータを格納する
	public void InputSphData(){
		string sphData = WD.GetResponse ();
		//一行ごとにパース
		string[] lines = sphData.Split ("\n"[0]);
		Debug.Log ("SPH LINES " + lines.Length);
		int n;
		for (n = 0; n < lines.Length && lines [n].Length > 0; n++) {
			Debug.Log (lines[n].Length);
		}
		sphNum = n-1;

		//SPHデータの格納
		char[] delimiterChars = {' ', '\t'};
		sph_data = new int[sphNum];
		for (int i = 0; i < sphNum; i++) {
			string[] info = lines [i+1].Split (delimiterChars, StringSplitOptions.RemoveEmptyEntries);
			sph_data[i] = (int.Parse (info [1]) + 1) / 3 - 1;
		}
	}

	//生データからLINKデータを格納する
	public void InputLinkData(){
		string sphData = WD.GetResponse ();
		//一行ごとにパース
		string[] lines = sphData.Split ("\n"[0]);
		Debug.Log ("LINK LINES " + lines.Length);
		int n;
		for (n = 0; n < lines.Length && lines [n].Length > 0; n++) {
			Debug.Log (lines[n].Length);
		}
		linkNum = n-1;

		//SPHデータの格納
		char[] delimiterChars = {' ', '\t'};
		data4 = new Vector2[linkNum];
		for (int i = 0; i < linkNum; i++) {
			string[] info = lines [i+1].Split (delimiterChars, StringSplitOptions.RemoveEmptyEntries);
			data4[i].x = int.Parse (info [1]);
			data4[i].y = int.Parse (info [2]);
		}
	}

	//生データから配列に格納する
	public void InputTrcData(){
		int cnt = 0;
		int nMarkerNum = 0;

		//TRCデータの読み込み
		string rawData = WD.GetResponse ();

		//一行ごとにパース
		string[] lines = rawData.Split ("\n"[0]);

		//フレーム数の取得
		char[] delimiterChars = {' ', '\t'};
		string[] info = lines [2].Split (delimiterChars, StringSplitOptions.RemoveEmptyEntries);
		nFrameNum = int.Parse (info [2]);
		nMarkerNum = int.Parse(info[3]);

		//読み込みデータの準備
		data = new FrameData[nFrameNum];
		for(cnt = 0; cnt < nFrameNum; cnt++){
			//1フレーム分の格納領域を作成
			data[cnt] = new FrameData(nMarkerNum);
			//パース
			string[] coordinates = lines[cnt + 6].Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < nMarkerNum; i++) {
				float x = float.Parse (coordinates[i*3 + 2]);
				float y = float.Parse (coordinates[i*3 + 3]);
				float z = float.Parse (coordinates[i*3 + 4]);
				data [cnt].setPoint (i, new Point3D (x, y, z));
				Point3D p = data [cnt].getPoint (i);
			}
		}

		data3 = new Vector3[sphNum];
		gameobjNode = new GameObject[sphNum];

		for (int i = 0; i < sphNum; i++) {
			Point3D p = data [0].getPoint (sph_data[i]);
			int num = i + 1;
			string str = "Marker_1";
			gameobjNode[i] = Instantiate (Resources.Load (str)) as GameObject;
			Vector3 p2 = new Vector3((float)p.x / 2000.0f * 5.0f, (float)p.y / 2000.0f * 5.0f, (float)p.z / 2000.0f * 5.0f);
			gameobjNode[i].transform.localPosition = p2;
			data3 [i] = p2;
			gameobjNode [i].transform.parent = this.Field;
		}

		gameobjLink = new GameObject[linkNum];
		for(int i = 0; i < linkNum; i++){
			gameobjLink[i] = Instantiate (Resources.Load ("Line")) as GameObject;
			gameobjLink [i].transform.parent = this.Field;
			LineRenderer line = gameobjLink[i].GetComponent<LineRenderer> ();
			line.transform.parent = this.Field;
			line.SetVertexCount(2);
			line.SetPosition (0, data3 [(int)data4[i].x]);
			line.SetPosition (1, data3 [(int)data4[i].y]);
		}
		state = 1;

		//スライダーの設定
		GameObject objSlider = GameObject.Find( "Slider" );
		sc = objSlider.GetComponent<SliderController> ();
		sc.setMaxMinVal (nFrameNum);
	}

	class FrameData{
		private Point3D[] points = null;
		int nMarkers = 0;

		//フレームの作成
		public FrameData(int n){
			nMarkers = n;
			points = new Point3D[nMarkers];
			for(int i = 0; i < nMarkers; i++){
				points[i] = new Point3D(0,0,0);
			}
		}

		//座標の格納
		public void setPoint(int marker_id, Point3D p){
			points [marker_id] = p;
		}

		//座標の取得
		public Point3D getPoint(int marker_id){
			return points [marker_id];
		}
	}

	//マーカー座標
	class Point3D{
		public double x = 0;
		public double y = 0;
		public double z = 0;

		public Point3D(double x, double y, double z){
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}
}
