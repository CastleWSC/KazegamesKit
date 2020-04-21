using System;
using UnityEngine;

namespace KazegamesKit
{
    public class GameKit
    {
        public static EPlatform Platform
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                    case RuntimePlatform.WindowsPlayer:
                        return EPlatform.Windows;

                    case RuntimePlatform.Android:
                        return EPlatform.Android;

                    case RuntimePlatform.IPhonePlayer:
                        return EPlatform.iOS;

                    case RuntimePlatform.WebGLPlayer:
                        return EPlatform.WebGL;
                }

                throw new Exception($"GameKit: Undefined platform= {Application.platform}.");
            }
        }

        public static bool IsMobilePlatform
        {
            get
            {
                return Application.isMobilePlatform;
            }
        }

        public static int FrameRate
        {
            get
            {
                return Application.targetFrameRate;
            }

            set
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = value;
            }
        }
    }
}
