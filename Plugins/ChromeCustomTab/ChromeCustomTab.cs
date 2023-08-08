using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneDevApp.CustomTabPlugin
{
    public class ChromeCustomTab : MonoBehaviour
    {
        /// <summary>
        /// UnityMainActivity current activity name or main activity name
        /// Modify only if this UnityPlayer.java class is extends or used any other default class
        /// </summary>
        [Tooltip("Android Launcher Activity")]
        [SerializeField] private string m_unityMainActivity = "com.unity3d.player.UnityPlayer";

		/// <summary>
        /// This method can be called from a UnityEvent
        /// </summary>
        /// <param name="url"></param>
		public void OpenCustomTab(string url)
        {
			OpenCustomTab(url, "#000000", "#ff0000");
		}

        /// <summary>
        /// This method can't be called from a UnityEvent
        /// </summary>
        /// <param name="url"></param>
        /// <param name="colorCode"></param>
        /// <param name="secColorCode"></param>
        /// <param name="showTitle"></param>
        /// <param name="showUrlBar"></param>
		public void OpenCustomTab(string url, string colorCode = "#000000", string secColorCode = "#ff0000", bool showTitle = false, bool showUrlBar = false)
        {
			if (Application.isEditor)
			{
				Application.OpenURL(url);
				return;
			}
			else if (Application.platform == RuntimePlatform.Android)
            {
                using (var javaUnityPlayer = new AndroidJavaClass(m_unityMainActivity))
                {
                    using (var mContext = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        using (AndroidJavaClass jc = new AndroidJavaClass("com.onedevapp.customchrometabs.CustomTabPlugin"))
                        {
                            var mAuthManager = jc.CallStatic<AndroidJavaObject>("getInstance");
                            mAuthManager.Call<AndroidJavaObject>("setActivity", mContext);
                            mAuthManager.Call<AndroidJavaObject>("setUrl", url);
                            mAuthManager.Call<AndroidJavaObject>("setColorString", colorCode);
                            mAuthManager.Call<AndroidJavaObject>("setSecondaryColorString", secColorCode);
                            mAuthManager.Call<AndroidJavaObject>("ToggleShowTitle", showTitle);
                            mAuthManager.Call<AndroidJavaObject>("ToggleUrlBarHiding", showUrlBar);
                            mAuthManager.Call("openCustomTab");
                        }
                        /*using (var androidPlugin = new AndroidJavaObject("com.onedevapp.customchrometabs.ChromeCustomTab", currentActivity, value))
                        {
                            //AndroidJavaObject aObject = androidPlugin.Call<AndroidJavaObject>("ChromeCustomTab", currentActivity, value);
                            androidPlugin.Call("show", "#FF0000");
                        }*/
                    }
                }
            }
        }
    }
}
