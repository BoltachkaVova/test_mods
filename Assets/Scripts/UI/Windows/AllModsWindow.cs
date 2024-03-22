using System;
using System.Collections.Generic;
using Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI.Windows
{
    [Serializable]
    public class AllModsWindow : IWindow, ISubscription, ILanguage
    {
        [Header("MainSettings")] 
        [SerializeField] private GameObject gameObject;
        [SerializeField] private TypeWindow typeWindow;
        [SerializeField] private Button returnButton;
        
        [Header("Mods")] 
        [SerializeField] private ModItem prefab;
        [SerializeField] private Transform content;

        [Header("Texts")] 
        [SerializeField] private TextMeshProUGUI header;
        
        [Header("AnimationSettings")] // todo to animationSystem
        [SerializeField] private RectTransform head;
        [SerializeField] private Vector2 fromPosHead;
        [SerializeField] private float duration;
        
        private Vector2 _startPosHead;
        
        private List<ModItem> _modItems = new List<ModItem>(32);
        
        public TypeWindow TypeWindow => typeWindow;
        public event Action<TypeWindow> ChangeWindow;
        public event Action<SettingsMod> SelectModItem;

        public void Initialized()
        {
            SetAnimationData();
            returnButton.onClick.AddListener(() =>  ChangeWindow?.Invoke(TypeWindow.Main));
        }

        public void Disposable()
        {
            returnButton.onClick.RemoveListener(() =>  ChangeWindow?.Invoke(TypeWindow.Main));
            foreach (var item in _modItems)
                item.SelectMod -= OnSelectMod;
        }
        
        public async UniTask Open()
        {
            gameObject.SetActive(true);
            await head.DOAnchorPos(_startPosHead, duration);
            
            foreach (var item in _modItems)
                await item.Open(true);
        }

        public UniTask Close()
        {
            gameObject.SetActive(false);
            SetFromPosition();
            
            return new UniTask();
        }

        public void UpdateLanguage(ConfigData configData)
        {
            header.text = configData.allModsHeader;
            SetItems(configData.SettingsMods);
        }

        private void SetItems(IReadOnlyList<SettingsMod> mods)
        {
            if (_modItems.Count != 0)
            {
                for (var i = 0; i < _modItems.Count; i++)
                {
                    var settings = mods[i];
                    if(settings != null)
                        _modItems[i].Initialised(settings);
                }
                
                return;
            }

            foreach (var settingsMod in mods)
            {
                var item = Object.Instantiate(prefab, content);
                item.Initialised(settingsMod);
                item.SelectMod += OnSelectMod;
                
                _modItems.Add(item);
            }
        }

        private void OnSelectMod(SettingsMod mod)
        {
            SelectModItem?.Invoke(mod);
            ChangeWindow?.Invoke(TypeWindow.SelectMod);
        }
        
        private void SetFromPosition()
        {
            head.anchoredPosition = fromPosHead;
            
            foreach (var modItem in _modItems)
                modItem.Open(false).Forget();
        }

        private void SetAnimationData()
        {
            _startPosHead = head.anchoredPosition;
            SetFromPosition();
        }
    }
}