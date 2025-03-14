using System;
using UnityEngine;
namespace Application.Core.Scripts
{
    [Serializable]
    public class AudioClipModel
    {
        public enum UISounds
        {
            OnBack = 0,
            OnHover = 1,
            OnChoose = 2,
            OnPreview = 3,
            OnNextOption = 4,
            OnPreviousOption = 5,
            OnNavigationSuccess = 6,
            OnNavigationFailed = 7,
            OnWin = 8,
            OnLose = 9,
            OnScoreCalculating = 10,
            OnOpenShop = 11,
            OnAchievementUnlock = 12,
        }

        public UISounds _uiSounds;
        [SerializeField] private AudioClip[] _audioClips;
        public AudioClip[] AudioClips => _audioClips;
    }
}