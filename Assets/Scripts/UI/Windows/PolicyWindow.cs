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
    public class PolicyWindow : IWindow, ISubscription, ILanguage
    {
        [Header("MainSettings")] 
        [SerializeField] private GameObject gameObject;
        [SerializeField] private TypeWindow typeWindow;
        
        [Header("Buttons")] 
        [SerializeField] private Button returnButton;

        [Header("Texts")] 
        [SerializeField] private TextMeshProUGUI data;
        [SerializeField] private TextMeshProUGUI header;

        [Header("AnimationSettings")] // todo to animationSystem
        [SerializeField] private RectTransform head;
        [SerializeField] private Vector2 fromPosHead;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float duration;

        private Sequence _sequence;
        private Vector2 _startPosHead;
        
        public TypeWindow TypeWindow => typeWindow;
        public event Action<TypeWindow> ChangeWindow;

        public void Initialized()
        {
            SetAnimationData();
            returnButton.onClick.AddListener(() => ChangeWindow?.Invoke(TypeWindow.Main));
        }

        public void Disposable()
        {
            returnButton.onClick.RemoveListener(() => ChangeWindow?.Invoke(TypeWindow.Main));
        }
        
        public async UniTask Open()
        {
            _sequence = DOTween.Sequence();
            gameObject.SetActive(true);
            
            await _sequence.Append(head.DOAnchorPos(_startPosHead, duration))
                .Join(canvasGroup.DOFade(1, duration)).SetEase(Ease.Linear);
        }

        public UniTask Close()
        {
            gameObject.SetActive(false);
            SetFromPosition();
            
            return new UniTask();
        }

        public void UpdateLanguage(ConfigData configData)
        {
            header.text = configData.policyHeader;
            data.text = configData.policyData;
        }

        private void SetFromPosition()
        {
            head.anchoredPosition = fromPosHead;
            canvasGroup.alpha = 0;
        }

        private void SetAnimationData()
        {
            _startPosHead = head.anchoredPosition;
            SetFromPosition();
        }
    }
}