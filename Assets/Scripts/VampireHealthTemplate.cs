using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VampireHealthTemplate", menuName = "VtM/Vampire Health Template")]
public class VampireHealthTemplate : HealthTemplate
{
    private Dictionary<int, (int penalty, string woundName)> boxesStatus =
        new Dictionary<int, (int penalty, string woundName)>
        {
            {0, (0, "Not Injured")},
            {1, (0, "Bruised")},
            {2, (-1, "Hurt")},
            {3, (-1, "Injured")},
            {4, (-2, "Wounded")},
            {5, (-2, "Mauled")},
            {6, (-5, "Crippled")},
            {7, (-999, "Incap")}
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
