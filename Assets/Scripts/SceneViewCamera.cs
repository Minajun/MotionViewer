using UnityEngine;

/// <summary>
/// GameビューにてSceneビューのようなカメラの動きをマウス操作によって実現する
/// </summary>
[RequireComponent(typeof(Camera))]
public class SceneViewCamera : MonoBehaviour
{
	private Vector3 preMousePos;

	public Transform Target;
	private float DistanceToPlayerM = 20f;    // カメラとプレイヤーとの距離[m]
	private float SlideDistanceM = 0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
	private float HeightM = 1.5f;            // 注視点の高さ[m]
	private float RotationSensitivity = 100f;// 感度
	private bool dragged = false;

	private void Start(){
		SetCameraPosition();
	}

	private void Update()
	{
		MouseUpdate();
		return;
	}

	private void MouseUpdate()
	{
		float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
		if(scrollWheel != 0.0f)
			MouseWheel(scrollWheel);

		if (Input.GetMouseButtonDown (0)) {
			dragged = true;
		}
		if (Input.GetMouseButtonUp (0)) {
			dragged = false;
		}

	}

	private void MouseWheel(float delta)
	{
		var lookAt = Target.position + Vector3.up * HeightM;
		// カメラとプレイヤーとの間の距離を調整
		DistanceToPlayerM -= delta;
		transform.position = lookAt - transform.forward * DistanceToPlayerM;
		return;
	}

	void FixedUpdate () {
		if (dragged) {
			SetCameraPosition();
		}
	}

	void SetCameraPosition(){
			var rotX = Input.GetAxis ("Mouse X") * Time.deltaTime * RotationSensitivity;
			var rotY = Input.GetAxis ("Mouse Y") * Time.deltaTime * RotationSensitivity;

			var lookAt = Target.position + Vector3.up * HeightM;

			// 回転
			transform.RotateAround (lookAt, Vector3.up, rotX);
			// カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
			if (transform.forward.y > 0.9f && rotY < 0) {
				rotY = 0;
			}
			if (transform.forward.y < -0.9f && rotY > 0) {
				rotY = 0;
			}
			transform.RotateAround (lookAt, transform.right, rotY);

			// カメラとプレイヤーとの間の距離を調整
			transform.position = lookAt - transform.forward * DistanceToPlayerM;

			// 注視点の設定
			transform.LookAt (lookAt);

			// カメラを横にずらして中央を開ける
			transform.position = transform.position + transform.right * SlideDistanceM;
	}
}
