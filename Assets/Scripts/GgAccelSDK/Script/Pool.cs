using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace GgAccel
{
    public class Pool : MonoSingleton<Pool>
    {
        public const string TAG = "Pool";
        private readonly Dictionary<string, IObjectPool<MonoBehaviour>> _poolDictionary = new();

        private MonoBehaviour CreateObject(MonoBehaviour prefab, Transform parent)
        {
            var instantiatedPrefab = Instantiate(prefab, parent ? parent : transform, true);
            return instantiatedPrefab;
        }

        private void OnGetObject(MonoBehaviour item)
        {
            item.gameObject.SetActive(true);
        }

        private void OnReleaseObject(MonoBehaviour item)
        {
            item.gameObject.SetActive(false);
        }

        private void OnDestroyObject(MonoBehaviour item)
        {
            Destroy(item.gameObject);
        }

        public static void Release(MonoBehaviour item)
        {
            Instance._poolDictionary[item.GetType().Name].Release(item);
        }

        // Store Pool in dictionary with key is MonoBehavior script name
        public static T Get<T>(T prefab, Transform parent = null) where T : MonoBehaviour
        {
            if (!Instance._poolDictionary.ContainsKey(prefab.GetType().Name))
            {
                Instance._poolDictionary[prefab.GetType().Name] = new ObjectPool<MonoBehaviour>(
                    () => Instance.CreateObject(prefab, parent), Instance.OnGetObject,
                    Instance.OnReleaseObject,
                    Instance.OnDestroyObject, false,
                    100);
            }

            return (T)Instance._poolDictionary[prefab.GetType().Name].Get();
        }
    }
}