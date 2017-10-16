using UnityEngine;
using System.Collections;
using System.IO; //System.IO.FileInfo, System.IO.StreamReader, System.IO.StreamWriter
using System; //Exception
using System.Text; //Encoding

public class FileSelect : MonoBehaviour {
Vector2 scrollPosition = new Vector2(50, 50);
   public int selGridInt = 0;
    private string[] selStrings = {};

	void Start(){
		ReadFile();
	}

    void OnGUI(){
       
        selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 2);
       
    }

	public void ButtonPush() {
		Debug.Log(selStrings[selGridInt] + " selected");
		MotionData.Instance.fileName = selStrings[selGridInt];
		Application.LoadLevel (1);
	}

	void ReadFile(){
		string rawdata = "";
        // FileReadTest.txtファイルを読み込む
        FileInfo fi = new FileInfo(Application.dataPath + "/" + "FileReadTest.txt");
        try {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8)){
				rawdata = sr.ReadToEnd();
            }
        } catch (Exception e){
            
        }
		//改行を削除
		rawdata = System.Text.RegularExpressions.Regex.Replace(rawdata, @"\n", "");
		char[] delimiterChars = {','};
		string[] fileNames = rawdata.Split (delimiterChars, StringSplitOptions.RemoveEmptyEntries);
		
		for(int i = 0; i < fileNames.Length; i++){
			selStrings.CopyTo(selStrings = new string[selStrings.Length + 1], 0);
			selStrings[selStrings.Length - 1] = fileNames[i];
			Debug.Log(fileNames[i]);
		}
    }
}
