using UnityEngine;

public class TestDiceRoller : MonoBehaviour
{
    private void OnMouseDown()
    {
        DiceRoller.RequestStandardRoll(6, 6);
    }
}
