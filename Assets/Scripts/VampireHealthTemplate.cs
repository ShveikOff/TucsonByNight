using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VampireHealthTemplate", menuName = "VtM/Vampire Health Template")]
public class VampireHealthTemplate : HealthTemplate
{
    private Dictionary<int, (int penalty, string woundName)> boxesStatus =
        new Dictionary<int, (int penalty, string woundName)>
        {
            {0, (0, "Bruised")},
            {1, (-1, "Hurt")},
            {2, (-1, "Injured")},
            {3, (-2, "Wounded")},
            {4, (-2, "Mauled")},
            {5, (-5, "Crippled")},
            {6, (-999, "Incap")}
        };

    public override Dictionary<int, (int penalty, string woundName)> BoxesStatus => boxesStatus;

    public VampireHealthTemplate()
    {
        boxes = new HealthBox[boxesStatus.Count];
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i] = new HealthBox(DamageType.None);
        }
    }

    public override void Initialize()
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].damageType = DamageType.None;
        }
    }
}
