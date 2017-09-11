using UnityEngine;
using System.Collections;

public class SampleButtonController : BaseButtonController
{
	private GameObject objMotionController = null;
	private MotionController MC = null;

	void Start(){
		objMotionController = GameObject.Find ("MotionController");
		MC = objMotionController.GetComponent<MotionController> ();
	}

	protected override void OnClick(string objectName)
	{
		// 渡されたオブジェクト名で処理を分岐
		//（オブジェクト名はどこかで一括管理した方がいいかも）
		if ("Button2".Equals(objectName))
		{
			// Button2がクリックされたとき
			this.Button2Click();
		}
		else if ("Button3".Equals(objectName))
		{
			// Button2がクリックされたとき
			this.Button3Click();
		}
		else if ("Button4".Equals(objectName))
		{
			// Button2がクリックされたとき
			this.Button4Click();
		}
		else if ("Button5".Equals(objectName))
		{
			// Button2がクリックされたとき
			this.Button5Click();
		}
		else if ("Button6".Equals(objectName))
		{
			// Button2がクリックされたとき
			this.Button6Click();
		}
		else if ("Button7".Equals(objectName))
		{
			// Button2がクリックされたとき
			this.Button7Click();
		}
		else if ("Button8".Equals(objectName))
		{
			// Button2がクリックされたとき
			this.Button8Click();
		}else if ("Button9".Equals(objectName))
		{
			// Button2がクリックされたとき
			this.Button9Click();
		}
		else
		{
			throw new System.Exception("Not implemented!!");
		}
	}

	private void Button2Click()
	{
		Debug.Log("Button2 Click");
		MC.frameStop ();
	}

	private void Button3Click()
	{
		Debug.Log("Button3 Click");
		MC.frameStart ();
	}

	private void Button4Click()
	{
		Debug.Log("Button4 Click");
		MC.fowardOneFrame ();
	}

	private void Button5Click()
	{
		Debug.Log("Button5 Click");
		MC.backOneFrame ();
	}

	private void Button6Click()
	{
		Debug.Log("Button6 Click");
		MC.StartFileLoad ();
	}

	private void Button7Click()
	{
		Debug.Log("Button7 Click");
		MC.SetFirstFrame ();
	}

	private void Button8Click()
	{
		Debug.Log("Button8 Click");
		MC.SetEndFrame ();
	}

	private void Button9Click()
	{
		Application.LoadLevel (0);
	}
}
