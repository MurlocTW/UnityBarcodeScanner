using BarcodeScanner;
using BarcodeScanner.Scanner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using ZXing;
using System.IO;
using System;
using UnityEngine.Events;

public class QRCodeManager : MonoBehaviour
{
	[Serializable] public class OnScannedEvent : UnityEvent<string> { }

	[SerializeField] private RawImage sourceImage;
	[SerializeField] private RawImage scanImage;
	[SerializeField] private bool useScan;

	public OnScannedEvent ScannedEvent;

	private IScanner BarcodeScanner;
	public float RestartTime;

	public bool IsScan { get; private set; }

	#region UnityProcess
	public void Start()
	{
		if (useScan)
			StartScan();
	}

	public void Update()
	{
		if (BarcodeScanner != null)
		{
			BarcodeScanner.Update();
			if (RestartTime != 0 && RestartTime < Time.realtimeSinceStartup)
			{
				UpdateScanner();
				RestartTime = 0;
			}
		}
	}
	#endregion

	#region ScanEvent
	private void UpdateScanner()
	{
		BarcodeScanner.Scan((barCodeType, barCodeValue) => {
			BarcodeScanner.Stop();
			RestartTime += Time.realtimeSinceStartup + 1f;
			ScannedEvent?.Invoke(barCodeValue);
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
		});
	}

	private void StartScan()
	{
		scanImage.gameObject.SetActive(IsScan = true);

		// Create a basic scanner
		BarcodeScanner = new Scanner();
		BarcodeScanner.Camera.Play();

		// Display the camera texture through a RawImage
		BarcodeScanner.OnReady += (sender, arg) => {
			// Set Orientation & Texture



#if UNITY_EDITOR
			scanImage.rectTransform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
#else

#if UNITY_ADNROID
				float scaleY = 1;
#elif UNITY_IOS
				float scaleY = -1;
#endif
				float orient = BarcodeScanner.Camera.GetRotation() - 90.0f;
				float Ratio = (float)BarcodeScanner.Camera.Height / (float)BarcodeScanner.Camera.Width;
				scanImage.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
				scanImage.rectTransform.localScale = new Vector3(Ratio, scaleY, 1);
#endif

			scanImage.texture = BarcodeScanner.Camera.Texture;

			// Keep Image Aspect Ratio
			var rect = scanImage.GetComponent<RectTransform>();
			var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
			rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);

			RestartTime = Time.realtimeSinceStartup;
		};
	}

	private void StopScan()
	{
		// Stop Scanning
		scanImage.gameObject.SetActive(IsScan = false);

		if (BarcodeScanner != null)
			BarcodeScanner.Destroy();
		BarcodeScanner = null;

		RestartTime = 0;
	}

	public void SwitchScan()
	{
		if (IsScan)
			StopScan();
		else
			StartScan();
	}

	public void SetScan(bool enable)
	{
		if (enable)
			StartScan();
		else
			StopScan();
	}
#endregion

	public Texture2D Encode(string url, int size = 256)
	{
		BarcodeWriter writer = new BarcodeWriter()
		{
			Format = BarcodeFormat.QR_CODE,
			Options = new ZXing.QrCode.QrCodeEncodingOptions()
			{
				Width = size,
				Height = size,
				CharacterSet = "UTF-8",
				ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.M
			},
		};

		Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
		tex.SetPixels32(writer.Write(url));
		tex.Apply();
		return tex;
	}
}
