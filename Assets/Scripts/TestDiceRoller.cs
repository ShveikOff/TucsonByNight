using UnityEngine;

public class TestDiceRoller : MonoBehaviour
{
    private DiceRoller diceRoller;

    private void Awake()
    {
        diceRoller = new DiceRoller();
        diceRoller.OnDiceRolled += HandleDiceRollResult;
    }

    private void OnDestroy()
    {
        diceRoller.OnDiceRolled -= HandleDiceRollResult;
    }

    private void OnMouseDown()
    {
        diceRoller.RequestStandardRoll(6, 6);
    }

    private void HandleDiceRollResult(DiceRollResult result)
    {
        Debug.Log("=== Dice Roll Completed ===");
        Debug.Log("Dice Rolls: " + string.Join(", ", result.rolls));
        Debug.Log($"rolledSuccesses = {result.rolledSuccesses}, onesCount = {result.onesCount}, netSuccesses = {result.netSuccesses}");
        Debug.Log($"Final result = {result.finalResult}");

    }
}
