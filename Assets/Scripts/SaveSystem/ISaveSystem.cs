using Cysharp.Threading.Tasks;

namespace SaveSystem
{
    public interface ISaveSystem
    {
        public Data.Data GetData { get; }
        public UniTask<bool> LoadData();
        public UniTask Initialised();
        public void SaveData();
    }
}