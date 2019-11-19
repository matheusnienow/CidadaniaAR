using Enum;
using UnityEngine;

namespace Model
{
    public class GarbageImage
    {
        public Sprite Sprite { get; private set; }
        public GarbageType GarbageType { get; set; }

        public GarbageImage(Sprite sprite, GarbageType garbageType)
        {
            Sprite = sprite;
            GarbageType = garbageType;
        }
    }
}