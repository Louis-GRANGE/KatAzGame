using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Text;

public class GetRecognizedSpeech : MonoBehaviour
{
	[SerializeField] private Text ButtonText;
	public CameraCapture ccCameraCapture;

	private bool bStartRecognized;

	[DllImport("__Internal")]
	private static extern void RecognizedSpeech();

	public void StartOrStopRecognize()
	{
		StartCoroutine(CallAzureCognitivesServiceAPI());
		/*bStartRecognized = !bStartRecognized;
        if (bStartRecognized)
        {
            ButtonText.text = "Stop Recognize";
        }
        else
        {
            ButtonText.text = "Start Recognize";
            GameManager.getInstance().goJavascriptHook.GetComponent<JavascriptHook>().SetStopImageInfo();
        }
        RecognizedSpeech();*/
	}

	float timer = 0;
	bool bstart = false;
	private void Update()
    {
		if (bstart)
		{
			timer += Time.deltaTime;
			print(timer);
		}
		else
			timer = 0;
	}

    byte[] WaveAudio;
	private void Start()
	{
		WaveAudio = File.ReadAllBytes(@"C:\Users\doudo\Desktop\TournerDroite4Pas.wav");
	}

	string urlAPI = "https://api-cogntive.azurewebsites.net/api/HttpTrigger_API_Cognitive?code=JPqGhlBaNy9OoMG7IH5q7bMJGnD1TJSCAbgRrum96DSOCE5AqcpiFg==" + "&typeCS=STT"; // LUIS ou OCR
	public IEnumerator CallAzureCognitivesServiceAPI()
	{
		//POST
		print("POST");
		WWWForm webForm = new WWWForm();
		using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(urlAPI, webForm))
		{
			//------------------ LUIS
			/*unityWebRequest.SetRequestHeader("Content-Type", "application/json");
			unityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes("{\r\n\"text\" : \"je veux aller à droite\"\r\n}"));
			

			//unityWebRequest.SetRequestHeader("Content-Type", "application/octet-stream");

			//------------------ IMAGE
			byte[] image = ccCameraCapture.Capture();
			print("Taille de l'image: " + image.Length);
			unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
			unityWebRequest.uploadHandler = new UploadHandlerRaw(image);
			unityWebRequest.uploadHandler.contentType = "application/octet-stream";*/

			//------------------ AUDIO
			bstart = true;
			unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
			unityWebRequest.uploadHandler = new UploadHandlerRaw(WaveAudio);
			print("Taille de l'audio: " + WaveAudio.Length);
			unityWebRequest.uploadHandler.contentType = "application/octet-stream";
			yield return unityWebRequest.SendWebRequest();

			long responseCode = unityWebRequest.responseCode;

			try
			{
				/*sResponseHeader = unityWebRequest.GetResponseHeaders()["Operation-Location"];
				StartCoroutine(AnalyseResult(sResponseHeader));*/

				string jsonResponse = null;
				jsonResponse = unityWebRequest.downloadHandler.text;

				//print($"jsonResponse = {jsonResponse}");
				print($"jsonResponse = {jsonResponse}");
				JsonDataOfLUIS.Root analysedQuery = JsonConvert.DeserializeObject<JsonDataOfLUIS.Root>(unityWebRequest.downloadHandler.text);

				//analyse the elements of the response 
				LuisManager.getInstance().AnalyseResponseElements(analysedQuery);
				/*
								// The response will be in Json format
								// therefore it needs to be deserialized into the classes AnalysedObject and TagData
								JsonDataOfOCR.Root analysedObject;// = new AnalysedObject();
								analysedObject = JsonUtility.FromJson<JsonDataOfOCR.Root>(jsonResponse);

								if (analysedObject.Regions == null)
								{
									Debug.Log("analysedObject.Regions is null");
								}
								else
								{
									string sFullText = "";
									foreach (var region in analysedObject.Regions)
									{
										foreach (var line in region.Lines)
										{
											foreach (var word in line.Words)
											{
												sFullText += word.Text + " ";
											}
										}
									}
									print($"sFullText = {sFullText}");
								}*/
			}
			catch (Exception exception)
			{
				Debug.Log("Json exception.Message: " + exception.Message);
			}
			bstart = false;
			yield return null;
		}
	}
}
