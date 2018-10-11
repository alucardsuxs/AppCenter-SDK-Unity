using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AppCenterEditor
{
    public class AppCenterEditorHttp
    {
        internal static void MakeDownloadCall(string url, Action<string> resultCallback)
        {
            var www = new WWW(url);
            AppCenterEditor.RaiseStateUpdate(AppCenterEditor.EdExStates.OnHttpReq, url, AppCenterEditorHelper.MSG_SPIN_BLOCK);
            EditorCoroutine.Start(PostDownload(www, response => { WriteResultFile(url, resultCallback, response); }, AppCenterEditorHelper.SharedErrorCallback), www);
        }

        internal static void MakeGitHubApiCall(string url, Action<string> resultCallback)
        {
            var www = new WWW(url);
            EditorCoroutine.Start(Post(www, response => { OnGitHubSuccess(resultCallback, response); }, AppCenterEditorHelper.SharedErrorCallback), www);
        }

        private static IEnumerator Post(WWW www, Action<string> callBack, Action<string> errorCallback)
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                errorCallback(www.error);
            }
            else
            {
                callBack(www.text);
            }
        }

        private static IEnumerator PostDownload(WWW www, Action<byte[]> callBack, Action<string> errorCallback)
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                errorCallback(www.error);
            }
            else
            {
                callBack(www.bytes);
            }
        }

        private static void OnGitHubSuccess(Action<string> resultCallback, string response)
        {
            if (resultCallback == null)
            {
                return;
            }
            var jsonResponse = JsonWrapper.DeserializeObject<List<object>>(response);
            if (jsonResponse == null || jsonResponse.Count == 0)
            {
                return;
            }
            // list seems to come back in ascending order (oldest -> newest)
            var latestSdkTag = (JsonObject)jsonResponse[jsonResponse.Count - 1];
            object tag;
            if (latestSdkTag.TryGetValue("ref", out tag))
            {
                var startIndex = tag.ToString().LastIndexOf('/') + 1;
                var length = tag.ToString().Length - startIndex;
                resultCallback(tag.ToString().Substring(startIndex, length));
            }
            else
            {
                resultCallback(null);
            }
        }

        private static void WriteResultFile(string url, Action<string> resultCallback, byte[] response)
        {
            AppCenterEditor.RaiseStateUpdate(AppCenterEditor.EdExStates.OnHttpRes, url);
            string fileName;
            if (url.IndexOf("unity-edex") > -1)
            {
                fileName = AppCenterEditorHelper.EDEX_UPGRADE_PATH;
            }
            else if (url.IndexOf("unity-analytics-via-edex") > -1)
            {
                fileName = AppCenterEditorHelper.ANALYTICS_SDK_DOWNLOAD_PATH;
            }
            else if (url.IndexOf("unity-crashes-via-edex") > -1)
            {
                fileName = AppCenterEditorHelper.CRASHES_SDK_DOWNLOAD_PATH;
            }
            else if (url.IndexOf("unity-distribute-via-edex") > -1)
            {
                fileName = AppCenterEditorHelper.DISTRIBUTE_SDK_DOWNLOAD_PATH;
            }
            else
            {
                fileName = AppCenterEditorHelper.EDEX_PACKAGES_PATH;
            }
            var fileSaveLocation = AppCenterEditorHelper.EDEX_ROOT + fileName;
            var fileSaveDirectory = Path.GetDirectoryName(fileSaveLocation);
            Debug.Log("Saving " + response.Length + " bytes to: " + fileSaveLocation);
            if (!Directory.Exists(fileSaveDirectory))
            {
                Directory.CreateDirectory(fileSaveDirectory);
            }
            File.WriteAllBytes(fileSaveLocation, response);
            resultCallback(fileSaveLocation);
        }
    }
}
