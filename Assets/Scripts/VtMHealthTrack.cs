using System;
using UnityEngine;

[System.Serializable]
public class VtMHealthTrack : WoDHealthTrack
{
    /// <summary>
    /// Реализация метода ApplyDamage:
    /// для каждого единичного урона вызывается ApplySingleDamage,
    /// после чего происходит сортировка ячеек по тяжести урона.
    /// </summary>
    /// <param name="damageType">Тип входящего урона.</param>
    /// <param name="amount">Количество единиц урона.</param>
    public override void ApplyDamage(DamageType damageType, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            ApplySingleDamage(damageType);
        }
        SortByDamageSeverity();
    }

    /// <summary>
    /// Применяет один экземпляр урона согласно логике VtM:
    /// - Рассматриваем ячейки от 0 до boxes.Length-2 как обычный диапазон,
    ///   а последняя ячейка (индекс boxes.Length-1) – как Incap.
    /// - Если входящий урон Bashing и в обычном диапазоне есть ячейка с Bashing,
    ///   меняем её на Lethal; если такой ячейки нет – заполняем Incap ячейку значением Bashing.
    /// - Аналогичная логика для Lethal и Aggravated.
    /// </summary>
    /// <param name="damageType">Тип урона (Bashing, Lethal, Aggravated).</param>
    private void ApplySingleDamage(DamageType damageType)
    {
        // Индексы от 0 до boxes.Length - 2 считаются обычными ячейками,
        // а последняя ячейка (boxes.Length - 1) используется для Incap.
        int nonIncapRange = boxes.Length - 1;
        int incapIndex = boxes.Length - 1;

        switch (damageType)
        {
            case DamageType.Bashing:
                {
                    int indexBashing = FindFirstIndexOfInRange(DamageType.Bashing, 0, nonIncapRange);
                    if (indexBashing != -1)
                    {
                        boxes[indexBashing].damageType = DamageType.Lethal;
                    }
                    else
                    {
                        boxes[incapIndex].damageType = DamageType.Bashing;
                    }
                }
                break;
            case DamageType.Lethal:
                {
                    int indexBashing = FindFirstIndexOfInRange(DamageType.Bashing, 0, nonIncapRange);
                    if (indexBashing != -1)
                    {
                        boxes[indexBashing].damageType = DamageType.Lethal;
                    }
                    else
                    {
                        boxes[incapIndex].damageType = DamageType.Lethal;
                    }
                }
                break;
            case DamageType.Aggravated:
                {
                    int indexBashing = FindFirstIndexOfInRange(DamageType.Bashing, 0, nonIncapRange);
                    if (indexBashing != -1)
                    {
                        boxes[indexBashing].damageType = DamageType.Aggravated;
                    }
                    else
                    {
                        boxes[incapIndex].damageType = DamageType.Aggravated;
                    }
                }
                break;
            default:
                // Если DamageType.None, ничего не делаем.
                break;
        }
    }

    private int FindFirstIndexOfInRange(DamageType target, int start, int end)
    {
        for (int i = start; i < end && i < boxes.Length; i++)
        {
            if (boxes[i].damageType == target)
                return i;
        }
        return -1;
    }
}
