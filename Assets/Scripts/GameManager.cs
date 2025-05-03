using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public static List<float> LeaderboardTimes = new List<float>();
    public static List<string> LeaderboardNames = new List<string>();
    public static string CurrentPlayerName = "---"; // Store the current player's name

    public static void AddTimeToLeaderboard(float time)
    {
        // Add the new time and the current player's name
        LeaderboardTimes.Add(time);
        LeaderboardNames.Add(CurrentPlayerName);

        // Sort the leaderboard by time
        List<(float time, string name)> sortedLeaderboard = new List<(float, string)>();
        for (int i = 0; i < LeaderboardTimes.Count; i++)
        {
            sortedLeaderboard.Add((LeaderboardTimes[i], LeaderboardNames[i]));
        }
        sortedLeaderboard.Sort((a, b) => a.time.CompareTo(b.time));

        // Update LeaderboardTimes and LeaderboardNames with sorted values
        LeaderboardTimes.Clear();
        LeaderboardNames.Clear();
        foreach (var entry in sortedLeaderboard)
        {
            LeaderboardTimes.Add(entry.time);
            LeaderboardNames.Add(entry.name);
        }

        // Only keep the top 3 times
        if (LeaderboardTimes.Count > 3)
        {
            LeaderboardTimes.RemoveRange(3, LeaderboardTimes.Count - 3);
            LeaderboardNames.RemoveRange(3, LeaderboardNames.Count - 3);
        }
    }
}
