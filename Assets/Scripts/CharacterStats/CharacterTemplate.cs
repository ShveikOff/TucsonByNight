using UnityEngine;

public abstract class CharacterTemplate: ScriptableObject
{
    [Header("Общая информация")]
    [Tooltip("Отображаемое имя персонажа.")]
    public string displayName;

    [Tooltip("Краткое описание или биография персонажа.")]
    [TextArea(2, 5)]
    public string description;

    [Tooltip("Возраст персонажа.")]
    public int age;

    [Tooltip("Пол персонажа.")]
    public string gender;

    [Header("Optional Fields")]
    [Tooltip("Спрайт/иконка персонажа, если нужно показывать портрет.")]
    public Sprite portrait;
}