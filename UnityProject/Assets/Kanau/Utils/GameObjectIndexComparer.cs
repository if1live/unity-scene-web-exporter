using System.Collections;
using UnityEngine;

namespace Assets.Kanau.Utils
{
    public class GameObjectIndexComparer : IComparer
    {
        public int Compare(object x, object y) {
            var gox = x as GameObject;
            var goy = y as GameObject;
            var ix = gox.transform.GetSiblingIndex();
            var iy = goy.transform.GetSiblingIndex();
            return ix.CompareTo(iy);
        }
    }
}
