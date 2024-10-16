// using Game.Net;
// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking;
//
// public static class XNetwork
// {
//     public static string AppUrl => GameNetCfg.current.getValue("LoginServer");
//     public static void HttpGetApi(string api, Action<DownloadHandler> onFinished = null, Action<string> onError = null, Action<UnityWebRequest> onDownload = null)
//     {
//         HttpGet(AppUrl + "/" + api, onFinished, onError, onDownload);
//     }
//
//     public static void HttpGetApi(string url, string api, Action<DownloadHandler> onFinished = null, Action<string> onError = null, Action<UnityWebRequest> onDownload = null)
//     {
//         HttpGet(url + "/" + api, onFinished, onError, onDownload);
//     }
//
//     public static void HttpPostApi(string api, string json, Action<DownloadHandler> onFinished = null, Action<string> onError = null, Action<float> onUpload = null)
//     {
//         HttpPost(AppUrl + "/" + api, json, onFinished, onError, onUpload);
//     }
//
//     public static void HttpPostApi(string url, string api, string json, Action<DownloadHandler> onFinished = null, Action<string> onError = null, Action<float> onUpload = null)
//     {
//         HttpPost(url + "/" + api, json, onFinished, onError, onUpload);
//     }
//     
//     /// <summary>
//     /// 发送请求
//     /// </summary>
//     public static void HttpGet(string url, Action<DownloadHandler> onFinished = null, Action<string> onError = null, Action<UnityWebRequest> onDownload = null)
//     {
//         URequest request = new URequest();
//         request.RequestFinishedAction = onFinished;
//         request.RequestErrorAction = onError;
//         request.DownloadProgressAction = onDownload;
//         request.Get(url);
//     }
//
//     public static void HttpDownload(string url, Action<DownloadHandler> onFinished = null, Action<string> onError = null, Action<UnityWebRequest> onDownload = null)
//     {
//         UResDownload.Instance.Download(url, onDownload, onFinished, onError);
//     }
//
//     public static void HttpPost(string url, string json, Action<DownloadHandler> onFinished = null, Action<string> onError = null, Action<float> onUpload = null)
//     {
//         URequest request = new URequest();
//         request.PostFinishedAction = onFinished;
//         request.RequestErrorAction = onError;
//         request.UploadProgressAction = onUpload;
//         request.PostJson(url, json);
//     }
//    
// }
