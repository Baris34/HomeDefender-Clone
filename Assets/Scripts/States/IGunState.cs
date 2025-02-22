public interface IGunState
{
    void EnterState(GunController gun);
    void UpdateState();
    void ExitState();
}