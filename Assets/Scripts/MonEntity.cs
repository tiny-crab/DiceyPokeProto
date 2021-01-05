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
    public List<string> learnableMovesConfig; //formatted "<MoveName>,<LevelToLearn>"
    Dictionary<int, Move> learnableMoves = new Dictionary<int, Move>();

    // mon instance
    public int currentLevel = 1;
    public int currentHealth;
    public int currentEnergy;
    public List<Move> activeMoves;

    public void constructMoves() {
        foreach (string learnableMoveConfig in learnableMovesConfig) {
            var moveName = learnableMoveConfig.Split(',')[0];
            var level = int.Parse(learnableMoveConfig.Split(',')[1]);

            var moveType = Type.GetType(moveName);
            var constructedMove = (Move) Activator.CreateInstance(moveType);
            learnableMoves.Add(level, constructedMove);
            activeMoves = learnableMoves.Where(pair => pair.Key <= currentLevel).Select(pair => pair.Value).ToList();
        }
    }

}
