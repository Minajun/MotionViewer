using UnityEngine;
using System.Collections;

public class FileSelect : MonoBehaviour {
Vector2 scrollPosition = Vector2.zero;
   public int selGridInt = 0;
    public string[] selStrings = new string[] {"radio1"};
    void OnGUI(){
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true,  GUILayout.Width(400),  GUILayout.Height(300)); 
       
        selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 1);
       
        GUILayout.EndScrollView();
    }
}
