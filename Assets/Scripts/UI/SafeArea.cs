using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public static class SafeArea
    {
        public static void SetSafeAreaPortrait(List<RectTransform> transforms)
        {
            var safeArea = Screen.safeArea;
            
            var min = safeArea.position;
            var max = min + safeArea.size;

            var width = Screen.width;
            var height = Screen.height;

            min.x /= width;
            min.y /= height;

            max.x /= width;
            max.y /= height;

            foreach (var rectTransform in transforms)
            {
                rectTransform.anchorMax = max;
                rectTransform.anchorMin = min;
            }
        }
    }
}