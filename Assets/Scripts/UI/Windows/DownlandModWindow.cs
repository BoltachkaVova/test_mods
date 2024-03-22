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
    public class DownlandModWindow : IWindow, ILanguage, ISubscription
    {
        [Header("MainSettings")] 
        [SerializeField] private GameObject gameObject;
        [SerializeField] private TypeWindow typeWindow;
        
        [Header("Buttons")] 
        [SerializeField] private Button openModeButton;
        [SerializeField] private Button rateUsButton;
        [SerializeField] private Button returnButton;
        [SerializeField] private Button restartButton;

        [Header("Progress")] 
        [SerializeField] private TextMeshProUGUI data;
        [SerializeField] private Image fill;
        
        [Header("Texts")] 
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI openMode;
        [SerializeField] private TextMeshProUGUI rateUs;
        
        [Header("AnimationSettings")] // todo to animationSystem
        [SerializeField] private RectTransform head;
        [SerializeField] private Vector2 fromPosHead;
        [SerializeField] private RectTransform buttons;
        [SerializeField] private Vector2 fromPosButtons;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float duration;
        
        private Vector2 _startPosHead;
        private Vector2 _startPosButtons;
        private Sequence _sequence;

        private ProgressBar _progressBar;
        
        public TypeWindow TypeWindow => typeWindow;
        public event Action<TypeWindow> ChangeWindow;

        public void SetItemData(SettingsMod item)
        {
            header.text = item.name;
        }

        public void Initialized()
        {
            SetAnimationData();
            
            _progressBar = new ProgressBar(data, fill);
            _progressBar.Reset();
            
            openModeButton.onClick.AddListener(OnOpenMode);
            returnButton.onClick.AddListener(()=> ChangeWindow?.Invoke(TypeWindow.AllMods));
            rateUsButton.onClick.AddListener(OnRateUs);
            restartButton.onClick.AddListener(OnRestart);
        }
        
        public void Disposable()
        {
            openModeButton.onClick.RemoveListener(OnOpenMode);
            returnButton.onClick.RemoveListener(()=> ChangeWindow?.Invoke(TypeWindow.AllMods));
            rateUsButton.onClick.RemoveListener(OnRateUs);
            restartButton.onClick.RemoveListener(OnRestart);
        }
        
        public async UniTask Open()
        {
            _sequence = DOTween.Sequence();
            gameObject.SetActive(true);

            await _sequence.Append(head.DOAnchorPos(_startPosHead, duration))
                .Append(canvasGroup.DOFade(1, duration)).Join(buttons.DOAnchorPos(_startPosButtons, duration)).SetEase(Ease.Linear);
        }

        public UniTask Close()
        {
            gameObject.SetActive(false);
            
            SetFromPosition();
            _sequence?.Kill();
            _progressBar.Reset();
            
            return new UniTask();
        }

        public void UpdateLanguage(ConfigData configData)
        {
            header.text = configData.downlandHeader;
            openMode.text = configData.downlandOpenModButton;
            rateUs.text = configData.downlandRateUsButton;
        }
        
        private void OnRateUs()
        {
            Debug.Log("Links Rate Us");
        }
        
        private void OnRestart()
        {
            _progressBar.Reset();
            _progressBar.Start(5);
        }
        
        private void OnOpenMode()
        {
            _progressBar.Start(5);
        }
        
        private void SetFromPosition()
        {
            head.anchoredPosition = fromPosHead;
            buttons.anchoredPosition = fromPosButtons;
            
            canvasGroup.alpha = 0;
        }

        private void SetAnimationData()
        {
            _startPosHead = head.anchoredPosition;
            _startPosButtons = buttons.anchoredPosition;
            
            SetFromPosition();
        }
    }

    public class ProgressBar  // todo class
    {
        private readonly TextMeshProUGUI _data;
        private readonly Image _fill;

        private Sequence _sequence;
        
        public ProgressBar(TextMeshProUGUI data, Image fill)
        {
            _data = data;
            _fill = fill;
        }

        public void Reset()
        {
            _sequence?.Kill();
            _fill.fillAmount = 0;

            _data.text = "0%";
        }
        
        public void Start(float time) 
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(_fill.DOFillAmount(1, time))
                .OnUpdate(() =>
                {
                    if(_fill.fillAmount >= 0.99) return;
                    var amount = (int) (_fill.fillAmount * 100);
                    _data.text = $"{amount}%";
                })
                .OnComplete(() =>
            {
                _data.text = $"{100}%";
            }).SetEase(Ease.Linear);
        }
    }
}