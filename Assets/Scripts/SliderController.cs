using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class SliderController : MonoBehaviour 
{
	public Slider mainSlider;
	GameObject refObj;

	public void Start()
	{
		//Adds a listener to the main slider and invokes a method when the value changes.
		mainSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
		refObj = GameObject.Find( "MotionController" );
	}

	// Invoked when the value of the slider changes.
	public void ValueChangeCheck()
	{
		//Debug.Log (mainSlider.value);
		MotionController MC = refObj.GetComponent<MotionController>();
		MC.SetFrameNum ((int)mainSlider.value);
	}

	//シークバーの最大値の設定
	public void setMaxMinVal(int max){
		mainSlider.maxValue = max;
	}

	//シークバーの位置の設定
	public void SetCurrentFramePos(int num){
		mainSlider.value = num;
	}
}