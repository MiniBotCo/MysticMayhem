public class GameStatistics
{
    // Values
    public static int difficulty = 1;
    
    // Stats
    public static int highestLevel = 0;
    public static int totalLevelsCleared = 0;
    public static int totalEnemiesDefeated = 0;
    public static int totalUpgradesGotten = 0;
    public static int deaths = 0;

    // Run Statistics
    public static int finalLevel = 0;
    public static int enemiesDefeated = 0;
    public static int upgradesGotten = 0;

    public static void ResetRunStatistics()
    {
        finalLevel = 0;
        enemiesDefeated = 0;
        upgradesGotten = 0;
    }

}