using UnityEngine;

namespace Application.Core.Scripts
{
    [CreateAssetMenu(menuName = "Core/Create MusicConfig", fileName = "MusicConfig", order = 0)]
    public class MusicConfig : ScriptableObject
    {
        [SerializeField] private MusicClipModel[] _musicClipModels = {};

        public MusicClipModel[] MusicClipModels => _musicClipModels;
    }

    [System.Serializable]
    public class MusicClipModel
    {
        [SerializeField] private string _id;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private bool _loop;

        public string Id => _id;
        public AudioClip AudioClip => _audioClip;
        public bool Loop => _loop;
    }
}