using System.Collections.Generic;
using GgAccel;
using UnityEngine;

namespace GgAccelSDK.Script
{
    public static class ExtensionFunction
    {
        public static void PlayRandomClips(this AudioClip[] audioClips, float volumeScale = 1f)
        {
            AudioManager.PlaySound(audioClips[Random.Range(0, audioClips.Length)], volumeScale);
        }

        public static void PlayRandomClips(this List<AudioClip> audioClips, float volumeScale = 1f)
        {
            AudioManager.PlaySound(audioClips[Random.Range(0, audioClips.Count)], volumeScale);
        }
    }
}