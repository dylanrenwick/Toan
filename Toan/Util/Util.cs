using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toan;

public class Util
{
    public static void EnsureLength<T>(ref T[] arr, int minLength, int maxLength = int.MaxValue)
    {
        if (minLength >= arr.Length)
        {
            int newLength = arr.Length;
            if (newLength == 0)
                newLength++;

            do
            {
                newLength *= 2;
                if (newLength < 0)
                {
                    newLength = minLength + 1;
                }
            }
            while (newLength < minLength);
            newLength = Math.Min(newLength, maxLength);
            Array.Resize(ref arr, newLength);
        }
    }
}
