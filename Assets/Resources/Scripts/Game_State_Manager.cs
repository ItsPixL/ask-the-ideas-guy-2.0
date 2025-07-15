using UnityEngine;

public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance { get; private set; } // Singleton instance

    public enum GameState {
        InMenu,
        InLevelSelector,
        InPreview,
        InGame,
        InStory
    }

    public enum InGameSubState { // Sub-states for InGame, automatically set to None when not in InGame state
        None,
        PlayerTurn,
        MonsterTurn
    }
    // pre-setting the gamestates
    public GameState CurrentState { get; private set; } = GameState.InMenu;
    public InGameSubState CurrentInGameSubState { get; private set; } = InGameSubState.None;

    private void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetState(GameState newState) {
        CurrentState = newState;
        if (newState != GameState.InGame) {
            CurrentInGameSubState = InGameSubState.None;
        }
        Debug.Log($"Game state changed to: {CurrentState}");
    }

    public void SetInGameSubState(InGameSubState subState) {
        if (CurrentState == GameState.InGame) {
            CurrentInGameSubState = subState;
            Debug.Log($"InGame sub-state changed to: {CurrentInGameSubState}");
        } else {
            Debug.LogWarning("Cannot set InGame sub-state when not in InGame state.");
        }
    }
}