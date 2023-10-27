using UnityEngine;

namespace GameAnalyticsSDK
{
    public class GameAnalyticsInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BeforeSceneLoad()
        {
            var gameAnalyticsPrefab = Resources.Load<GameObject>("Prefabs/" + nameof(GameAnalytics));
            var gameAnalyticsInstance = Object.Instantiate(gameAnalyticsPrefab);
            Object.DontDestroyOnLoad(gameAnalyticsInstance);
        }
    }
}
