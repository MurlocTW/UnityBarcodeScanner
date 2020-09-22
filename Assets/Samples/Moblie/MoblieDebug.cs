using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MoblieDebug : MonoBehaviour
{
	public InputField scaX;
	public InputField scaY;
	public InputField scaZ;

	public InputField rotX;
	public InputField rotY;
	public InputField rotZ;

	public QRCodeManager codeManager;
	public RawImage scanImage;

	private void Start()
	{
		WebCamDevice[] devices = WebCamTexture.devices;
		Debug.Log("Number of web cams connected: " + devices.Length);
		foreach (var cam in devices)
			Debug.Log("webcam name: " + cam.name);

		//Open Camera
		codeManager.SetScan(true);

		scaX.text = (scanImage.rectTransform.localScale.x).ToString();
		scaY.text = (scanImage.rectTransform.localScale.y).ToString();
		scaZ.text = (scanImage.rectTransform.localScale.z).ToString();

		//SetUI
		rotX.text = (scanImage.rectTransform.localEulerAngles.x).ToString();
		rotY.text = (scanImage.rectTransform.localEulerAngles.y).ToString();
		rotZ.text = (scanImage.rectTransform.localEulerAngles.z).ToString();

		//ListenerUI
		scaX.onValueChanged.AddListener((x) => scanImage.rectTransform.SetSizeX(float.Parse(x)));
		scaY.onValueChanged.AddListener((y) => scanImage.rectTransform.SetSizeY(float.Parse(y)));
		scaZ.onValueChanged.AddListener((z) => scanImage.rectTransform.SetSizeZ(float.Parse(z)));

		rotX.onValueChanged.AddListener((x) => scanImage.rectTransform.SetRotX(float.Parse(x)));
		rotY.onValueChanged.AddListener((y) => scanImage.rectTransform.SetRotY(float.Parse(y)));
		rotZ.onValueChanged.AddListener((z) => scanImage.rectTransform.SetRotZ(float.Parse(z)));
	}

}

public static class UnityExtension
{
	public static T SetSizeX<T>(this T selfComponent, float x) where T : Component
	{
		var size = selfComponent.gameObject.transform.localScale;
		selfComponent.gameObject.transform.localScale = new Vector3(x, size.y, size.z);
		return selfComponent;
	}

	public static T SetSizeY<T>(this T selfComponent, float y) where T : Component
	{
		var size = selfComponent.gameObject.transform.localScale;
		selfComponent.gameObject.transform.localScale = new Vector3(size.x, y, size.z);
		return selfComponent;
	}

	public static T SetSizeZ<T>(this T selfComponent, float z) where T : Component
	{
		var size = selfComponent.gameObject.transform.localScale;
		selfComponent.gameObject.transform.localScale = new Vector3(size.x, size.y, z);
		return selfComponent;
	}

	public static T SetRotX<T>(this T selfComponent, float x) where T : Component
	{
		var angle = selfComponent.gameObject.transform.localRotation.eulerAngles;
		var rot = Quaternion.Euler(x, angle.y, angle.z);
		selfComponent.gameObject.transform.localRotation = rot;
		return selfComponent;
	}

	public static T SetRotY<T>(this T selfComponent, float y) where T : Component
	{
		var angle = selfComponent.gameObject.transform.localRotation.eulerAngles;
		var rot = Quaternion.Euler(angle.x, y, angle.z);
		selfComponent.gameObject.transform.localRotation = rot;
		return selfComponent;
	}

	public static T SetRotZ<T>(this T selfComponent, float z) where T : Component
	{
		var angle = selfComponent.gameObject.transform.localRotation.eulerAngles;
		var rot = Quaternion.Euler(angle.x, angle.y, z);
		selfComponent.gameObject.transform.localRotation = rot;
		return selfComponent;
	}
}
