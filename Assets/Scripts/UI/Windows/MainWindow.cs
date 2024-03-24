using System;
using System.Collections.Generic;
using Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    [Serializable]
    public class MainWindow : ISubscription, IWindow
    {
        [SerializeField] private List<ConfigData> configData;
        
        [Header("MainSettings")] 
        [SerializeField] private GameObject gameObject;
        [SerializeField] private TypeWindow typeWindow;

        [Header("Buttons")]
        [SerializeField] private Button openPolicyButton;
        [SerializeField] private Button openAllModsButton;
        [SerializeField] private Button onSoundButton;
        [SerializeField] private Button instructionButton;
        [SerializeField] private Button setLanguageButton;
        [SerializeField] private SettingsSoundButton settingsSoundButton;
        [SerializeField] private Image languageIcon;
        
        [Header("Texts")] 
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI policyButtonText;
        [SerializeField] private TextMeshProUGUI allModsButtonText;
        [SerializeField] private TextMeshProUGUI instructionButtonText;

        [Header("AnimationSettings")]  // todo to animationSystem
        [SerializeField] private RectTransform icon;
        [SerializeField] private Vector2 fromIcon;
        
        [SerializeField] private RectTransform mods;
        [SerializeField] private Vector2 fromMods;
        
        [SerializeField] private RectTransform instruction;
        [SerializeField] private Vector2 fromInstruction;
        
        [SerializeField] private RectTransform sound;
        [SerializeField] private Vector2 fromSound;
        
        [SerializeField] private RectTransform lang;
        [SerializeField] private Vector2 fromLang;
       

        [SerializeField] private float duration;
        [SerializeField] private CanvasGroup textN;
        [SerializeField] private CanvasGroup textD;
        [SerializeField] private CanvasGroup policy;

        public TypeWindow TypeWindow => typeWindow;
        public event Action<TypeWindow> ChangeWindow;
        public event Action<ConfigData> ChangeLanguage;
        
        private int _id;
        private bool _isSound;
        
        private Vector2 _startPosIcon;
        private Vector2 _startPosMods;
        private Vector2 _startPosInstruction;
        private Vector2 _startPosSound;
        private Vector2 _startPosLang;

        private Sequence _sequence;
        private ISaveSystem _system;
        
        public void Initialized()
        {
            SetAnimationData();
            
            openPolicyButton.onClick.AddListener(() => ChangeWindow?.Invoke(TypeWindow.Policy));
            openAllModsButton.onClick.AddListener(() => ChangeWindow?.Invoke(TypeWindow.AllMods));
            
            onSoundButton.onClick.AddListener(OnSound);
            setLanguageButton.onClick.AddListener(OnSetLanguage);
            instructionButton.onClick.AddListener(OnInstruction);

            _system = AppController.Instance.SaveSystem;
            
            _id = _system.GetData.Id;
            _isSound = _system.GetData.IsSound;
            
            UpdateLanguage();
            OnSound();
        }

        public void Disposable()
        {
            openPolicyButton.onClick.RemoveListener(() => ChangeWindow?.Invoke(TypeWindow.Policy));
            openAllModsButton.onClick.RemoveListener(() => ChangeWindow?.Invoke(TypeWindow.AllMods));
            
            onSoundButton.onClick.RemoveListener(OnSound);
            setLanguageButton.onClick.RemoveListener(OnSetLanguage);
        }
        
        public async UniTask Open()
        {
            _sequence = DOTween.Sequence();
            gameObject.SetActive(true);

            await _sequence.Append(icon.DOAnchorPos(_startPosIcon, duration)).Join(textN.DOFade(1, duration)).Join(textD.DOFade(1, duration))
                .Append(mods.DOAnchorPos(_startPosMods, duration)).Join(instruction.DOAnchorPos(_startPosInstruction, duration)).Join(policy.DOFade(1, duration))
                .Append(sound.DOAnchorPos(_startPosSound, duration)).Join(lang.DOAnchorPos(_startPosLang, duration)).SetEase(Ease.Linear);
        }
        
        public UniTask Close()
        {
            gameObject.SetActive(false);
            
            _sequence?.Kill();
            SetFromPosition();
            
            return new UniTask();
        }
        
        private void OnSound()
        {
            _system.GetData.IsSound = _isSound;
            _system.SaveData();
            
            _isSound = !_isSound;
            settingsSoundButton.SetSound(_isSound);
        }
        
        private void OnSetLanguage()
        {
            if (++_id >= configData.Count)
                _id = 0;

            _system.GetData.Id = _id;
            _system.SaveData();
            
            UpdateLanguage();
        }
        
        private void OnInstruction()
        {
            Debug.Log("On Instruction Click");
        }
        
        private void UpdateLanguage()
        {
            var language = configData.Find(o => o.id == _id);

            ChangeLanguage?.Invoke(language);
            
            descriptionText.text = language.mainDescription;
            policyButtonText.text = language.mainPolicyButton;
            allModsButtonText.text = language.mainAllModButton;
            instructionButtonText.text = language.mainInstructionButton;
            
            languageIcon.sprite = language.icon;
        }

        private void SetFromPosition()
        {
            icon.anchoredPosition = fromIcon;
            mods.anchoredPosition = fromMods;
            instruction.anchoredPosition = fromInstruction;
            sound.anchoredPosition = fromSound;
            lang.anchoredPosition = fromLang;

            textN.alpha = 0;
            textD.alpha = 0;
            policy.alpha = 0;
        }

        private void SetAnimationData()
        {
            _startPosIcon = icon.anchoredPosition;
            _startPosMods = mods.anchoredPosition;
            _startPosInstruction = instruction.anchoredPosition;
            _startPosSound = sound.anchoredPosition;
            _startPosLang = lang.anchoredPosition;
            
            SetFromPosition();
            duration /= 6; // todo
        }
    }
    
    [Serializable]  
    public class SettingsSoundButton  // todo
    {
        [SerializeField] private Image icon;
        [SerializeField] private Sprite spritesTrue;
        [SerializeField] private Sprite spritesFalse;
        
        public void SetSound(bool isOn)
        {
            icon.sprite = isOn ? spritesTrue : spritesFalse;
        }
    }
}