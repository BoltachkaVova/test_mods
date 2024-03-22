using System;
using UnityEngine;

namespace Configs
{
    [Serializable]
    public class SettingsMod
    {
        public Sprite icon;
        public string name;
        public string description;
        
        public bool isLoaded;
        public bool isActive;
    }
}