using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace Application.Core.Scripts
{
    [Serializable]
    public class AudioClipModel
    {
        public enum Sounds
        {
            OnMove = 0,
            OnButtonHover = 1,
            OnChoose = 2,
            OnSelect = 3,
            OnMerge = 4,
            OnSpawn = 5,
            OnDespawn = 6,
        }

        public Sounds _sounds;
        [SerializeField] private AudioClip[] _audioClips;
        public AudioClip[] AudioClips => _audioClips;
    }
}