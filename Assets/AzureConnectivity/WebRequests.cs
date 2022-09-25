using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequests
{
    private class WebRequestsMono : MonoBehaviour { }
    private static WebRequestsMono webRequestsMono;

    private static void Init()
    {
        if(webRequestsMono == null)
        {
            GameObject gameObject = new GameObject("WebRequests");
            webRequestsMono = gameObject.AddComponent<WebRequestsMono>();
        }
    }

    public async static Task Get(string url, Action<string> onError, Action<string> onSuccess)
    {
        Init();
        webRequestsMono.StartCoroutine(GetCoroutine(url, onError, onSuccess));
    }

    private static IEnumerator GetCoroutine(string url, Action<string> onError, Action<string> onSuccess)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
        {
            yield return unityWebRequest.SendWebRequest();

            if(unityWebRequest.result == UnityWebRequest.Result.ConnectionError ||
               unityWebRequest.result == UnityWebRequest.Result.DataProcessingError ||
               unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(unityWebRequest.error);
            }
            else
            {
                onSuccess(unityWebRequest.downloadHandler.text);
            }
        }
    }

    public async static Task PostJson(string url, string jsonData, Action<string> onError, Action<string> onSuccess)
    {
        Init();
        webRequestsMono.StartCoroutine(GetCoroutinePostJson(url, jsonData, onError, onSuccess));
    }

    private static IEnumerator GetCoroutinePostJson(string url, string jsonData, Action<string> onError, Action<string> onSuccess)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            unityWebRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            unityWebRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError ||
               unityWebRequest.result == UnityWebRequest.Result.DataProcessingError ||
               unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(unityWebRequest.error);
            }
            else
            {
                onSuccess(unityWebRequest.downloadHandler.text);
            }
        }
    }
}
