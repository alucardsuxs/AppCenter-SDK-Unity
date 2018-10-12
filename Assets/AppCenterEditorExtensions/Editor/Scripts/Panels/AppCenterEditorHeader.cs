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
                    EditorGUILayout.LabelField("", AppCenterEditorHelper.uiStyle.GetStyle("pfLogo"), GUILayout.MaxHeight(40), GUILayout.Width(186));
                }
                else
                {
                    EditorGUILayout.LabelField("", AppCenterEditorHelper.uiStyle.GetStyle("pfLogo"), GUILayout.MaxHeight(50), GUILayout.Width(233));
                }

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



