using System.Collections.Generic;
using Configs;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject text;
        [SerializeField] private Image bg;
        [SerializeField] private Sprite mainBg;
        [SerializeField] private Sprite otherBg; // todo up
        
        [SerializeField] private AppController appController;
        [Header("Windows")]
        [SerializeField] private MainWindow mainWindow;
        [SerializeField] private PolicyWindow policyWindow;
        [SerializeField] private AllModsWindow allModsWindow;
        [SerializeField] private SelectModWindow selectModWindow;
        [SerializeField] private DownlandModWindow downlandModWindow;

        private IWindow _currentWindow;
        
        private List<IWindow> _windows;
        private List<ILanguage> _languages = new List<ILanguage>(8);
        
        private SubscriptionSystem _subscriptionSystem;
        
        private void Awake()
        {
            text.SetActive(true); // todo
            
            appController.LoadedData += OnLoaded;
            allModsWindow.SelectModItem += OnSelectItem;
            mainWindow.ChangeLanguage += OnChangeLanguage;
            
            InitialisedWindows();
        }
        private void Start()
        {
            _subscriptionSystem.Initialised();

            foreach (var window in _windows)
                window.ChangeWindow += OnChangeWindow;
        }
        
        private void OnDestroy()
        {
            appController.LoadedData -= OnLoaded;
            allModsWindow.SelectModItem -= OnSelectItem;
            mainWindow.ChangeLanguage -= OnChangeLanguage;
            
            _subscriptionSystem.Disposable();
            
            foreach (var window in _windows)
                window.ChangeWindow -= OnChangeWindow;
        }

        private void OnLoaded(bool isOn)
        {
            _currentWindow = mainWindow;
            
            text.SetActive(false);  // todo
            
            _currentWindow.Open();
        }
        
        private async void OnChangeWindow(TypeWindow typeWindow)
        {
            if(typeWindow == _currentWindow.TypeWindow) return;
            bg.sprite = typeWindow != TypeWindow.Main ? otherBg : mainBg; // todo
            
            await _currentWindow.Close();
            _currentWindow = _windows.Find(o => o.TypeWindow == typeWindow);
            await _currentWindow.Open();
        }

        private void InitialisedWindows()
        {
            _windows = new List<IWindow>(9) {mainWindow, policyWindow, allModsWindow, selectModWindow, downlandModWindow};
            
            foreach (var window in _windows) // todo
            {
                if(window is ILanguage o)
                    _languages.Add(o);
            }
            
            _subscriptionSystem = new SubscriptionSystem(_windows);
        }

        private void OnSelectItem(SettingsMod mod)
        {
             selectModWindow.SetItem(mod); 
             downlandModWindow.SetItemData(mod);
        }

        private void OnChangeLanguage(ConfigData data)
        {
            foreach (var window in _languages)
                window.UpdateLanguage(data);
        }

    }
}