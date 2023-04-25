using UnityEngine;

namespace Extensions
{
    public static class CustomExtensions
    {
        private const float SecondsInMinute = 60f;
        
        public static void ShowCanvasGroup(this CanvasGroup canvasGroup, bool state)
        {
            canvasGroup.alpha = state ? 1 : 0;
            canvasGroup.interactable = state;
            canvasGroup.blocksRaycasts = state;
        }

        public static float MinutesToSeconds(this float minutes)
        {
            return minutes * SecondsInMinute;
        }
    }
}