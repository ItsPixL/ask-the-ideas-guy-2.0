using UnityEngine;
using LevelManager;
using MonsterManager;
using System.Collections.Generic;

public class Monster_Controller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Level levelObject;
    private int monsterTurns = 0;
    public int allowedTurns;
    private List<Thing> monsterThings;
    private float flagTime = float.NaN;
    private float animationTime = 2f;
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Instance.CurrentInGameSubState == GameStateManager.InGameSubState.MonsterTurn)
        {
            if (float.IsNaN(flagTime) || Time.time - flagTime >= animationTime)
            {
                monsterThings = levelObject.returnThingsByType(typeof(Monster), typeof(MonsterSpawner));
                monsterTurns += 1;
                decideAction();
                performAnimation();
                checkTurnCount();
            }
        }
    }

    private void decideAction()
    {
        // Will code later.
    }

    private void performAnimation()
    {
        flagTime = Time.time;
        // Will code later.
    }
    
    private void checkTurnCount()
    {
        Debug.Log($"Monster turns used: {monsterTurns}");
        Debug.Log($"Allowed turns: {allowedTurns}");
        if (monsterTurns >= allowedTurns)
        {
            GameStateManager.Instance.SetInGameSubState(GameStateManager.InGameSubState.PlayerTurn);
            monsterTurns = 0;
        }
    } 
}
