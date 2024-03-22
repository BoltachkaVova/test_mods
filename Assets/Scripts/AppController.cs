using System;
using Cysharp.Threading.Tasks;
using SaveSystem;
using UnityEngine;

public class AppController : MonoBehaviour
{
    public static AppController Instance { get; private set; }
    
    private ISaveSystem _saveSystem;
    
    public ISaveSystem SaveSystem => _saveSystem;
    
    public event Action<bool> LoadedData;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else DestroyImmediate(this);
    }

    private void Start()
    {
        LoadData();
    }
    
    private async void LoadData()
    {
        _saveSystem = new PlayerPrefsSaveSystem();
        await _saveSystem.Initialised();
        
        await UniTask.Delay(TimeSpan.FromSeconds(1)); // todo
        LoadedData?.Invoke(true); // todo
    }
    
}