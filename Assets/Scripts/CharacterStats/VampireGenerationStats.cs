using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VampireGenerationStats", menuName = "VtM/Vampire Generation Stats")]
public class VampireGenerationStats : ScriptableObject
{
    private Dictionary<int, (int maxStat, int maxBloodPool, int bloodLimit)> vampireGeneationsStats =
        new Dictionary<int, (int maxStat, int maxBloodPool, int bloodLimit)>
        {
            {3, (10, 100, 20)},
            {4, (9, 50, 10)},
            {5, (8, 40, 8)},
            {6, (7, 30, 6)},
            {7, (6, 20, 4)},
            {8, (5, 15, 3)},
            {9, (5, 14, 2)},
            {10, (5, 13, 1)},
            {11, (5, 12, 1)},
            {12, (5, 11, 1)},
            {13, (5, 10, 1)}
        };
    
    public int getMaxStat(int generation)
    {
        if (vampireGeneationsStats.TryGetValue(generation, out var stats))
            return stats.maxStat;
        return 0;
    }

    public int getMaxBloodPool(int generation)
    {
        if (vampireGeneationsStats.TryGetValue(generation, out var stats))
            return stats.maxBloodPool;
        return 0;
    }

    public int getBloodLimit(int generation)
    {
        if (vampireGeneationsStats.TryGetValue(generation, out var stats))
            return stats.bloodLimit;
        return 0;
    }
}
