using System;
using Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    [Serializable]
    public class SelectModWindow : IWindow, ISubscription, ILanguage
    {
        [Header("MainSettings")] 
        [SerializeField] private GameObject gameObject;
        [SerializeField] private TypeWindow typeWindow;
        
        [Header("Buttons")] 
        [SerializeField] private Button returnButton;
        [SerializeField] private Button downlandButton;

        [Header("SettingsMod")] 
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI nameMode;

        [Header("AnimationSettings")] // todo to animationSystem
        [SerializeField] private RectTransform head;
        [SerializeField] private Vector2 fromPosHead;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float duration;
        
        private Vector2 _startPosHead;
        
        private Sequence _sequence;
        
        
        public TypeWindow TypeWindow => typeWindow;
        
        public event Action<TypeWindow> ChangeWindow;
        
        public void Initialized()
        {
            SetAnimationData();
                
            returnButton.onClick.AddListener(() => ChangeWindow?.Invoke(TypeWindow.AllMods)); 
            downlandButton.onClick.AddListener(() => ChangeWindow?.Invoke(TypeWindow.DownlandMod));
        }

        public void Disposable()
        {
            returnButton.onClick.RemoveListener(() => ChangeWindow?.Invoke(TypeWindow.AllMods)); 
            downlandButton.onClick.RemoveListener(() => ChangeWindow?.Invoke(TypeWindow.DownlandMod));
        }
        
        public async UniTask Open()
        {
            _sequence = DOTween.Sequence();
            gameObject.SetActive(true);
            await _sequence.Append(head.DOAnchorPos(_startPosHead, duration)).Append
                (icon.DOFillAmount(1, duration)).Join(canvasGroup.DOFade(1, duration)).SetEase(Ease.Linear);
        }

        public UniTask Close()
        {
            gameObject.SetActive(false);
            
            _sequence?.Kill();
            SetFromPosition();
            
            return new UniTask();
        }
        
        public void SetItem(SettingsMod settingsMod)
        {
            icon.sprite = settingsMod.icon;
            nameMode.text = settingsMod.name;
            description.text = settingsMod.description;
        }

        public void UpdateLanguage(ConfigData configData)
        {
            Debug.Log($"Not Update Language to {GetType()}:");
        }
        
        private void SetFromPosition()
        {
            head.anchoredPosition = fromPosHead;
            canvasGroup.alpha = 0;
            icon.fillAmount = 0;
        }

        private void SetAnimationData()
        {
            _startPosHead = head.anchoredPosition;
            SetFromPosition();
        }
        
    }
}