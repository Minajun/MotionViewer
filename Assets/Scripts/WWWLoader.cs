using UnityEngine;
using System.Collections;
using System.Text; //Encoding
using UnityEngine.UI;

public class WWWLoader : MonoBehaviour {
	public bool flag = false;
	private string text = null;
	private string url = null;

	void Start(){

	}

	public void SetURL(string url){
		this.url = url;
	}

	public void StartHttpGet(){
		flag = false;
		StartCoroutine(HttpGet());
	}

	public string GetResponse(){
		return text;
	}

	IEnumerator HttpGet () {
		WWW www = new WWW(url);

		while (!www.isDone) { // ダウンロードの進捗を表示
			//print(Mathf.CeilToInt(www.progress*100));
			yield return null;
		}

		if (!string.IsNullOrEmpty(www.error)) { // ダウンロードでエラーが発生した
			//print(www.error);
		} else { // ダウンロードが正常に完了した
			//print(www.text);
			//Debug.Log (www.text);
			text = www.text;
			string[] lines = text.Split ("\n"[0]);
			int i;
			for (i = 0; i < lines.Length; i++) {
				//Debug.Log (lines [i]);
			}
			flag = true;
		}
	}
}