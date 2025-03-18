using System;
using UnityEngine;

public abstract class HealthTrack
{
    [SerializeField] protected HealthBox[] boxes;      // Массив ячеек здоровья
    [SerializeField] protected HealthTemplate template;  // Шаблон (конфиг) для определения уровней, штрафов и т.д.


    protected HealthTrack(HealthTemplate template)
    {
        this.template = template;
        int cellCount = (template != null && template.BoxesStatus != null)
                        ? template.BoxesStatus.Count
                        : 7;
        boxes = new HealthBox[cellCount-1];
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i] = new HealthBox(DamageType.None);
        }
    }

    protected HealthTrack()
    {
        int cellCount = 7;
        boxes = new HealthBox[cellCount];
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i] = new HealthBox(DamageType.None);
        }
    }
    
    /// <summary>
    /// Инициализация системы здоровья.
    /// Обычно создаёт массив ячеек на основе данных из template.
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Применяет урон определённого типа (например, Bashing, Lethal, Aggravated) в заданном количестве.
    /// </summary>
    public abstract void ApplyDamage(DamageType damageType, int amount);

    /// <summary>
    /// Лечит заданное количество ячеек (переводит их в состояние None).
    /// </summary>
    public abstract void HealDamage(int amount);

    /// <summary>
    /// Возвращает текущий уровень ранений (например, число заполненных ячеек).
    /// </summary>
    public abstract int GetWoundLevel();

    /// <summary>
    /// Возвращает текущий штраф к броскам, основанный на заполненных ячейках и шаблоне.
    /// </summary>
    /// 
    public abstract string GetWoundName();
    public abstract int GetWoundPenalty();

    /// <summary>
    /// Отладочный метод, выводящий в консоль состояние всех ячеек.
    /// </summary>
    public abstract void DebugPrintBoxes();
}
