using System;
using System.Collections.Generic;

namespace Application.Core.Services
{
    [Serializable]
    public class LeaderboardSaveData
    {
        public List<float> EasyTimes = new List<float>();
        public List<float> IntermediateTimes = new List<float>();
        public List<float> ExpertTimes = new List<float>();
        public List<float> InfinityTimes = new List<float>();
    }
}