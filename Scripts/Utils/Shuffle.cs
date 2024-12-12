
using System.Collections.Generic;
using Godot;

public static class Shuffle<T>
{
    /// <summary>
    /// Witchcraft that I found online. Shuffles a list
    /// </summary>
    /// <param name="list">The list to be shuffleD</param>
    public static void ShuffleList (List<T> list)
    {
        for (int n = list.Count - 1; n > 0; --n)
        {
            int k = (int)(GD.Randi() % (n+1));

            T temp = list[n];
            list[n] = list[k];
            list[k] = temp;
        }
    }
}
