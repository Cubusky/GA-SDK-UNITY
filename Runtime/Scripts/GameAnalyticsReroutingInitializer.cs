using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameAnalyticsSDK
{
    public class GameAnalyticsReroutingInitializer : MonoBehaviour, IGameAnalyticsATTListener
    {
        public List<RuntimePlatformRerouter> rerouters;

        void Start()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                GameAnalytics.RequestTrackingAuthorization(this);
            }
            else
            {
                InitializeRerouted();
            }
        }

        public void GameAnalyticsATTListenerNotDetermined()
        {
            InitializeRerouted();
        }
        public void GameAnalyticsATTListenerRestricted()
        {
            InitializeRerouted();
        }
        public void GameAnalyticsATTListenerDenied()
        {
            InitializeRerouted();
        }
        public void GameAnalyticsATTListenerAuthorized()
        {
            InitializeRerouted();
        }

        public void InitializeRerouted()
        {
            var platform = Application.platform;
            foreach (var rerouter in rerouters)
            {
                if (rerouter.Reroute(platform, out platform))
                {
                    break;
                }
            }

            GameAnalytics.Initialize(platform);
        }

#if UNITY_EDITOR
#if (UNITY_WSA)
        [DllImport("GameAnalytics.UWP.dll")]
        private static extern void onQuit();
#endif

        void OnApplicationQuit()
        {
#if (UNITY_WSA)
            onQuit();
#elif UNITY_STANDALONE
            GameAnalyticsSDK.Net.GameAnalytics.OnQuit();
            System.Threading.Thread.Sleep(1500);
#endif
        }
#endif
    }

    [Serializable]
    public struct RuntimePlatformRerouter
    {
        public List<RuntimePlatform> reroutingPlatforms;
        public RuntimePlatform reroutedPlatform;

        public readonly bool Reroute(RuntimePlatform platform, out RuntimePlatform reroutedPlatform)
        {
            var isRerouting = reroutingPlatforms.Contains(platform);
            reroutedPlatform = isRerouting ? this.reroutedPlatform : platform;
            return isRerouting;
        }
    }
}