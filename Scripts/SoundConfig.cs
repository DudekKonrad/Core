using UnityEngine;
namespace Application.Core.Scripts
{
    [CreateAssetMenu(menuName = "Core/Create SoundConfig", fileName = "SoundConfig", order = 0)]
    public class SoundConfig : ScriptableObject
    {
        [SerializeField] private AudioClipModel[] _audioClipModels = {};

        public AudioClipModel[] AudioClipModels => _audioClipModels;
    }
    
}