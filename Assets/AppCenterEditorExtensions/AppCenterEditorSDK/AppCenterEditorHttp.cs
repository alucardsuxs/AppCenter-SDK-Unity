using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace AppCenterEditor
{
    public class AppCenterEditorHttp : UnityEditor.Editor
    {
        public static void MakeGitHubApiCall(string url, Action<string> resultCallback)
        {
            //var www = new WWW(url);
            //PlayFabEditor.RaiseStateUpdate(PlayFabEditor.EdExStates.OnHttpReq, url, PlayFabEditorHelper.MSG_SPIN_BLOCK);
            //EditorCoroutine.Start(PostDownload(www, (response) => { WriteResultFile(url, resultCallback, response); }, PlayFabEditorHelper.SharedErrorCallback), www);
        }
    }
}