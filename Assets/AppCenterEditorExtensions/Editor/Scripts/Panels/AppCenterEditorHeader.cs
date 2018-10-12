using UnityEngine;
using UnityEditor;

namespace AppCenterEditor
{
    public class AppCenterEditorHeader : Editor
    {
        public static void DrawHeader(float progress = 0f)
        {
            if (AppCenterEditorHelper.uiStyle == null)
                return;

            //using Begin Vertical as our container.
            using (new UnityHorizontal(GUILayout.Height(52)))
            {
                //Set the image in the container
                if (EditorGUIUtility.currentViewWidth < 375)
                {
                    EditorGUILayout.LabelField("", AppCenterEditorHelper.uiStyle.GetStyle("acLogo"), GUILayout.MaxHeight(40), GUILayout.Width(40));
                }
                else
                {
                    EditorGUILayout.LabelField("", AppCenterEditorHelper.uiStyle.GetStyle("acLogo"), GUILayout.MaxHeight(50), GUILayout.Width(50));
                }

                EditorGUILayout.LabelField("App Center", AppCenterEditorHelper.uiStyle.GetStyle("gpStyleGray2"), GUILayout.MinHeight(32));

                float gmAnchor = EditorGUIUtility.currentViewWidth - 30;


                if (EditorGUIUtility.currentViewWidth > 375)
                {
                    gmAnchor = EditorGUIUtility.currentViewWidth - 140;
                    GUILayout.BeginArea(new Rect(gmAnchor, 10, 140, 42));
                    GUILayout.BeginHorizontal();
                }
                else
                {
                    GUILayout.BeginArea(new Rect(gmAnchor, 10, EditorGUIUtility.currentViewWidth * .25f, 42));
                    GUILayout.BeginHorizontal();
                }

                GUILayout.EndHorizontal();
                GUILayout.EndArea();

                //end the vertical container
            }

            ProgressBar.Draw();
        }
    }
}
