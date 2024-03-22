using System;
using Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ModItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameMod;
        [SerializeField] private GameObject textGO;

        private Button _button;
        private SettingsMod _settingsMod;
        private Sequence _sequence;
        
        public event Action<SettingsMod> SelectMod;
        
        public void Initialised(SettingsMod settingsMod)
        {
            _settingsMod = settingsMod;
            
            icon.sprite = settingsMod.icon;
            nameMod.text = settingsMod.name;
        }
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
            
            icon.fillAmount = 0;
            textGO.SetActive(false);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            SelectMod?.Invoke(_settingsMod);
        }

        public async UniTask Open(bool isOn) // todo
        {
            if (isOn)
            {
                _sequence = DOTween.Sequence();
                await _sequence.Append(icon.DOFillAmount(1, 0.2f)).SetEase(Ease.Linear);
            }
            else
            {
                _sequence?.Kill();
                icon.fillAmount = 0;
            }

            textGO.SetActive(isOn);
        }
    }
}