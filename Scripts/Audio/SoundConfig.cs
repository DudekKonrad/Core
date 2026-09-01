using System.Collections.Generic;
using UnityEngine;
namespace Application.Core.Scripts.Audio
{
    [CreateAssetMenu(menuName = "Core/Create SoundConfig", fileName = "SoundConfig", order = 0)]
    public class SoundConfig : ScriptableObject
    {
        [SerializeField] private Dictionary<Enums.AudioClipModel.Sounds, AudioClip> _sounds = new Dictionary<Enums.AudioClipModel.Sounds, AudioClip>();
        public Dictionary<Enums.AudioClipModel.Sounds, AudioClip> Sounds => _sounds;
    }
    
}