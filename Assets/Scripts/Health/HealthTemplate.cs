using System.Collections.Generic;
using UnityEngine;

public abstract class HealthTemplate : ScriptableObject
{
    // Здесь можно определить общие поля и методы для всех шаблонов здоровья.
    public HealthBox[] boxes;
    public abstract Dictionary<int, (int penalty, string woundName)> BoxesStatus { get; }
    public abstract void Initialize();
}
