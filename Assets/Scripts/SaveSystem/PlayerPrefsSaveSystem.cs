using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaveSystem
{
    public class PlayerPrefsSaveSystem : ISaveSystem
    {
        private const string KeyData = "Dataaaaaaaaaaaa";
        
        private Data.Data _data;

        public Data.Data GetData => _data;
        
        public async UniTask Initialised()
        {
            if (PlayerPrefs.HasKey(KeyData))
                await LoadData();
            else
                _data = new Data.Data();
        }

        public UniTask<bool> LoadData()
        {
            var jsonData = PlayerPrefs.GetString(KeyData);
            _data = JsonUtility.FromJson<Data.Data>(jsonData);
            
            return new UniTask<bool>(true); // todo ыыыыыыы
        }

        public void SaveData()
        {
            var jsonData = JsonUtility.ToJson(GetData);
            PlayerPrefs.SetString(KeyData, jsonData);
        }
    }
}