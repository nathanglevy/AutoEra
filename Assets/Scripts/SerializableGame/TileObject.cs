using System;

namespace Assets.Scripts.SerializableGame
{
    [Serializable]
    public class TileObject
    {
        public TileObject(int x, int y, string tileName)
        {
            this.x = x;
            this.y = y;
            this.tileName = tileName;
        }
//    [SerializeField]
        public int x;
        public int y;
        public string tileName;
    }
}