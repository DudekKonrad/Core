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
            OnReveal = 3,
            OnMineExplode = 4,
        }

        public Sounds _sounds;
        [SerializeField] private AudioClip[] _audioClips;
        public AudioClip[] AudioClips => _audioClips;
    }
}