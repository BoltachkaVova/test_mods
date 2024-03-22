using System;

namespace Data
{
    [Serializable]
    public class Data
    {
        public int Id;
        public bool IsSound;

        public Data()
        {
            Id = 0;
            IsSound = true;
        }

        public Data(int id, bool isSound)
        {
            IsSound = isSound;
            Id = id;
        }

    }
}