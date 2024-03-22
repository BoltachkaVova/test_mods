using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ConfigData", menuName = "Configs/ConfigData", order = 0)]
    public class ConfigData : ScriptableObject
    {
        public int id;
        public Sprite icon;

        [SerializeField] private List<SettingsMod> settingsMods;
        public IReadOnlyList<SettingsMod> SettingsMods => settingsMods;
        
        [Header("Main")] 
        public string mainDescription;
        public string mainAllModButton;
        public string mainInstructionButton;
        public string mainPolicyButton;
        
        [Header("Policy")] 
        public string policyHeader;
        public string policyData;
        
        [Header("AllMods")] 
        public string allModsHeader;
        
        [Header("SelectMods")] 
        public string selectMods;
        
        [Header("Downland")] 
        public string downlandHeader;
        public string downlandOpenModButton;
        public string downlandRateUsButton;
    }
}