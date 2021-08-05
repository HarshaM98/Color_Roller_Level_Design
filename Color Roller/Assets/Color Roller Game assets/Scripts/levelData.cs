public class levelData
{
    public int levelNo;
    public int blocksAvailable;
    public int blocksColored;
    public int blocksLeft;
    public long timeAlloted;
    public long timeTaken;
    public int Score;

    public levelData(
        int levelNo,
        int blocksAvailable,
        int blocksColored,
        int blocksLeft,
        long timeAlloted,
        long timeTaken,
        int Score
        )
    {
        this.levelNo = levelNo;
        this.blocksAvailable = blocksAvailable;
        this.blocksColored = blocksColored;
        this.blocksLeft = blocksLeft;
        this.timeAlloted = timeAlloted;
        this.timeTaken = timeTaken;
        this.Score = Score;
    }

    public override string ToString()
    {
        return "Level No: " + levelNo +
            " Blocks Available: " + blocksAvailable +
            " Blocks Colored: " + blocksColored +
            " Blocks Left: " + blocksLeft +
            " Time Alloted: " + timeAlloted +
            " Time Taken: " + timeTaken +
            " Score " + Score;
    }
}