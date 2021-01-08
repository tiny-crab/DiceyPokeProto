using System;
using System.Collections;
using System.Collections.Generic;
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

    // ugly status effects code :) I love it
    public int poisonStack = 0;

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
        Debug.Log($"new max energy {maxEnergy + Mathf.CeilToInt(0.2f * maxEnergy * attackStack)}");
        Debug.Log($"new min energy {(maxEnergy + Mathf.CeilToInt(0.2f * maxEnergy * attackStack)) / 2}");
        var bottomBound = Mathf.FloorToInt((maxEnergy + Mathf.CeilToInt(0.2f * maxEnergy * attackStack)) / 2);
        return new System.Random().Next(bottomBound, maxEnergy + Mathf.CeilToInt(0.2f * maxEnergy * attackStack) + 1);
    }

    public void refreshTurn() {

        if (poisonStack > 0) {
            Debug.Log($"{monName} poisoned {poisonStack} time(s)");
            currentHealth -= Mathf.CeilToInt(0.05f * maxHealth) * poisonStack;
            poisonStack--;
        }

        currentEnergy = generateEnergy();
        remainingActions = 1;
    }
}
