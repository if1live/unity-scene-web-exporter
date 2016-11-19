// Ŭnicode please
using System;
using UnityEngine;

namespace Assets.Kanau.Utils
{
    public class Conversions
    {

        public static float RoundWithDecimalPlace(float v, int prec)
        {
            int[] precTable = new int[] { 1, 10, 100, 1000 };
            int precValue = precTable[prec];
            return (float)Math.Round(v * precValue) / precValue;
        }

        public static Vector3 RoundVector(Vector3 v, int prec)
        {
            float x = RoundWithDecimalPlace(v.x, prec);
            float y = RoundWithDecimalPlace(v.y, prec);
            float z = RoundWithDecimalPlace(v.z, prec);
            return new Vector3(x, y, z);
        }

        public static Vector2 RoundVector(Vector2 v, int prec)
        {
            float x = RoundWithDecimalPlace(v.x, prec);
            float y = RoundWithDecimalPlace(v.y, prec);
            return new Vector2(x, y);
        }
    }
}
