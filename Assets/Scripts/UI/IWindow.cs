using System;
using Configs;
using Cysharp.Threading.Tasks;

namespace UI
{
    public interface IWindow
    {
        public TypeWindow TypeWindow { get; }
        
        public event Action<TypeWindow> ChangeWindow;

        public UniTask Open();
        public UniTask Close();
    }
}