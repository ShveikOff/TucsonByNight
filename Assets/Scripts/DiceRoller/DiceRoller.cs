using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Результат броска кубов: содержит всю информацию, которую хотим сообщить подписчикам.
/// </summary>
public class DiceRollResult
{
    public int dicePool;
    public int difficulty;
    public int finalResult;
    public int netSuccesses;
    public int rolledSuccesses;
    public int onesCount;
    public List<int> rolls;

    public DiceRollResult(int dicePool, int difficulty, int finalResult,
                          int netSuccesses, int rolledSuccesses, int onesCount,
                          List<int> rolls)
    {
        this.dicePool = dicePool;
        this.difficulty = difficulty;
        this.finalResult = finalResult;
        this.netSuccesses = netSuccesses;
        this.rolledSuccesses = rolledSuccesses;
        this.onesCount = onesCount;
        this.rolls = rolls;
    }
}

/// <summary>
/// Статический класс для броска кубиков. 
/// Событие OnDiceRolled позволяет подписываться на результаты броска.
/// </summary>
public static class DiceRoller
{
    public static event Action<DiceRollResult> OnDiceRolled;

    public static void RequestStandardRoll(int dicePool, int difficulty)
    {
        DiceRollResult result = PerformRoll(dicePool, difficulty);
        OnDiceRolled?.Invoke(result);
    }

    private static DiceRollResult PerformRoll(int dicePool, int difficulty)
    {
        List<int> allRolls = new List<int>();
        int rolledSuccesses = 0;
        int onesCount = 0;
        int pool = dicePool;

        while (pool > 0)
        {
            pool--;
            int roll = UnityEngine.Random.Range(1, 11);
            allRolls.Add(roll);

            if (roll == 1)
            {
                onesCount++;
            }
            else if (roll == 10)
            {
                rolledSuccesses++;
                pool++; // Дополнительный бросок за 10
            }
            else
            {
                if (roll >= difficulty)
                    rolledSuccesses++;
            }
        }

        int netSuccesses = rolledSuccesses - onesCount;
        int final;
        if (netSuccesses < 1)
        {
            if (rolledSuccesses == 0 && onesCount > 0)
                final = -1;
            else
                final = 0;
        }
        else
        {
            final = netSuccesses;
        }

        Debug.Log($"Dice Rolls: {string.Join(", ", allRolls)}");
        Debug.Log($"rolledSuccesses = {rolledSuccesses}, onesCount = {onesCount}, netSuccesses = {netSuccesses}, final = {final}");

        return new DiceRollResult(
            dicePool,
            difficulty,
            final,
            netSuccesses,
            rolledSuccesses,
            onesCount,
            allRolls
        );
    }
}
