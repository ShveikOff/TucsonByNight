using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WoDHealthTrack : HealthTrack
{
    /// <summary>
    /// Инициализация массива ячеек на основе шаблона.
    /// Если шаблон задан, количество ячеек = количеству записей в шаблоне,
    /// иначе используется значение по умолчанию (7).
    /// </summary>
    public override void Initialize()
    {
        int cellCount = (template != null && template.BoxesStatus != null)
                        ? template.BoxesStatus.Count
                        : 7;
        boxes = new HealthBox[cellCount];
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i] = new HealthBox(DamageType.None);
        }
    }

    /// <summary>
    /// Возвращает тип урона в ячейке по индексу.
    /// </summary>
    public DamageType GetBoxDamage(int index)
    {
        if (index < 0 || index >= boxes.Length)
            return DamageType.None;
        return boxes[index].damageType;
    }

    /// <summary>
    /// Считает количество заполненных ячеек (где DamageType != None).
    /// </summary>
    public int CountFilledBoxes()
    {
        int count = 0;
        foreach (var box in boxes)
        {
            if (box.damageType != DamageType.None)
                count++;
        }
        return count;
    }

    /// <summary>
    /// Лечение: очищает указанное количество ячеек (устанавливая их в None),
    /// начиная с ячеек с наибольшим индексом.
    /// </summary>
    public override void HealDamage(int amount)
    {
        for (int i = boxes.Length - 1; i >= 0 && amount > 0; i--)
        {
            if (boxes[i].damageType != DamageType.None)
            {
                boxes[i].damageType = DamageType.None;
                amount--;
            }
        }
        SortByDamageSeverity();
    }

    /// <summary>
    /// Возвращает текущий уровень ранений – число заполненных ячеек,
    /// ограниченное максимальным количеством записей в шаблоне.
    /// </summary>
    public override int GetWoundLevel()
    {
        int filled = CountFilledBoxes();
        int maxSlots = (template != null && template.BoxesStatus != null)
                       ? template.BoxesStatus.Count
                       : boxes.Length;
        if (filled >= maxSlots)
            filled = maxSlots - 1;
        return filled;
    }

    /// <summary>
    /// Возвращает штраф к броскам, используя шаблон.
    /// Если шаблон не задан, используется упрощённая логика.
    /// </summary>
    public override int GetWoundPenalty()
    {
        int level = GetWoundLevel();
        if (template != null && template.BoxesStatus != null)
        {
            if (level >= template.BoxesStatus.Count)
                level = template.BoxesStatus.Count - 1;
            return template.BoxesStatus[level].penalty;
        }
        else
        {
            // Фолбэк: Bruised = 0, Hurt/Injured = -1, Wounded/Mauled = -2, Crippled = -5, Incap = -999
            switch (level)
            {
                case 0: return 0;
                case 1:
                case 2: return -1;
                case 3:
                case 4: return -2;
                case 5: return -5;
                default: return -999;
            }
        }
    }

    /// <summary>
    /// Выводит в консоль состояние всех ячеек.
    /// </summary>
    public override void DebugPrintBoxes()
    {
        string output = "HealthBar: [ ";
        for (int i = 0; i < boxes.Length; i++)
        {
            output += boxes[i].damageType.ToString();
            if (i < boxes.Length - 1)
                output += ", ";
        }
        output += " ]";
        Debug.Log(output);
    }

    /// <summary>
    /// Сортирует ячейки по тяжести урона так, что более тяжёлый урон (Aggravated) располагается ближе к началу.
    /// Порядок по убыванию: Aggravated > Lethal > Bashing > None.
    /// </summary>
    protected void SortByDamageSeverity()
    {
        Array.Sort(boxes, (a, b) => DamageSeverityValue(b.damageType).CompareTo(DamageSeverityValue(a.damageType)));
    }

    private int DamageSeverityValue(DamageType dt)
    {
        switch (dt)
        {
            case DamageType.Aggravated: return 3;
            case DamageType.Lethal:     return 2;
            case DamageType.Bashing:    return 1;
            default:                    return 0;
        }
    }

    // Оставляем метод ApplyDamage абстрактным – его будут реализовывать классы, наследующие от WoDHealthTrack.
    public override abstract void ApplyDamage(DamageType damageType, int amount);
}
