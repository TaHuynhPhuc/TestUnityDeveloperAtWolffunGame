public enum GameLoadState
{
    None,
    Start,
    DataLoaded,
    RuntimeInitialized,
    SceneSpawned
}


public interface IGameLoadStep
{
    GameLoadState TargetState { get; }
    void OnGameStateEntered();
}
