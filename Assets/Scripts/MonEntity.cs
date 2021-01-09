using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

public class MonEntity : MonoBehaviour {

    // display & graphics
    public Texture2D sprite;

    // mon prototype
    public string monName;
    public int maxHealth;
    public int maxEnergy;
    public GameObject nextEvolutionPrefab;
    public int nextEvolutionLevel;
    public List<string> learnableMovesConfig; //formatted "<MoveName>,<LevelToLearn>"
    List<KeyValuePair<int, Move>> learnableMoves = new List<KeyValuePair<int, Move>>();

    // mon instance
    public int currentLevel = 1;
    public int currentHealth;
    public int currentEnergy;
    public int remainingActions = 1;
    public List<Move> activeMoves;
    public int attackStack = 0;
    public int defenseStack = 0;

    // ugly status effects code :) I love it
    public int poisonStack = 0;
    public int paralysisStack = 0;
    public int dodgeStack = 0;
    public void confuse() {
        var stacks = this.GetType()
                .GetFields().ToList()
                .Where(field => field.Name == "attackStack" || field.Name == "defenseStack")
                .Where(field => (int) field.GetValue(this) > 0)
                .ToList();

        if (stacks.Count > 0) {
            stacks[new System.Random().Next(0, stacks.Count)].SetValue(this, 0);
        }
    }

    public void constructMoves() {
        foreach (string learnableMoveConfig in learnableMovesConfig) {
            var moveName = learnableMoveConfig.Split(',')[0];
            var level = int.Parse(learnableMoveConfig.Split(',')[1]);

            var constructedMove = new Move().getMoveByName(moveName);
            learnableMoves.Add(new KeyValuePair<int, Move>(level, constructedMove));
            activeMoves = learnableMoves.Where(pair => pair.Key <= currentLevel).Select(pair => pair.Value).ToList();
        }
    }

    public int generateEnergy() {
        var bottomBound = Mathf.FloorToInt((maxEnergy + Mathf.CeilToInt(0.2f * maxEnergy * attackStack)) / 4);
        bottomBound = Mathf.Max(0, bottomBound + Mathf.CeilToInt((0.2f * defenseStack) * maxEnergy));

        // guarantee a mon has enough energy for their cheapest move
        bottomBound = Mathf.Max(bottomBound, activeMoves.Select(move => move.cost).Min());
        var generatedEnergy = new System.Random().Next(
            minValue: Mathf.Min(bottomBound, maxEnergy),
            maxValue: maxEnergy + Mathf.CeilToInt(0.2f * maxEnergy * attackStack) + 1
        );
        Mathf.CeilToInt(generatedEnergy /= 1 + paralysisStack);
        return generatedEnergy;
    }

    public void refreshTurn() {
        if (poisonStack > 0) {
            Debug.Log($"{monName} poisoned {poisonStack} time(s)");
            currentHealth -= Mathf.CeilToInt(0.05f * maxHealth) * poisonStack;
            poisonStack--;
        }

        currentEnergy = generateEnergy();
        paralysisStack = 0;

        remainingActions = 1;
    }
}
