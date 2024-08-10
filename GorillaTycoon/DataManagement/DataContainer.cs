namespace GorillaTycoon.DataManagement;

public class DataContainer
{
    public static DataContainer Ins;
    
    // main
    public float Coins = 0;
    
    // upgrades
    public int ValuableBananas = 1;
    public int BananaCooldown = 1;
    public int Drone = 0;
    public int Collection = 1;

    public void Start()
    {
        Ins = this;
    }
}