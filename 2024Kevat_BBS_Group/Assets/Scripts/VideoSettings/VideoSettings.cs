using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VideoSettings
{
    public struct VideoSettings
    {
        public FullScreenMode FullScreenMode;
        public Resolution Resolution;
    }

    [Serializable]
    internal struct VideoSettingsSave
    {
        public FullScreenMode fullScreenMode;
        public string resolution;
    }
}