using System.Collections.Generic;
using UnityEngine;

namespace GgAccel
{
    public static class Helpers
    {
        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
        public static WaitForSeconds GetWaitForSeconds(float time)
        {
            if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }

        public static void DeleteChildren(this Transform t)
        {
            foreach (Transform child in t) Object.Destroy(child.gameObject);
        }
    }
}

