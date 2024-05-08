using Business.Genesis.Scheduler.Interfaces;

namespace Business.Genesis.Scheduler.Stratagies
{
    public class MatchmakingStrategy : IMatchmakingStrategy
    {
        public IEnumerable<(string First, string Second)> RoundRobin(List<string> teams)
        {
            List<(string, string)> matches = new List<(string, string)>();
            if (teams == null || teams.Count < 2)
                return matches;

            if (teams.Count % 2 != 0)
                teams.Add("Bye");

            List<string> shuffledList = teams;

            var restTeams = new List<string>(shuffledList.Skip(1));
            var teamsCount = shuffledList.Count;

            for (var day = 0; day < teamsCount - 1; day++)
            {
                if (restTeams[day % restTeams.Count]?.Equals(default) == false)
                    matches.Add((shuffledList[0], restTeams[day % restTeams.Count]));

                for (var index = 1; index < teamsCount / 2; index++)
                {
                    var firstTeam = restTeams[(day + index) % restTeams.Count];
                    var secondTeam = restTeams[(day + restTeams.Count - index) % restTeams.Count];
                    if (firstTeam?.Equals(default) == false && secondTeam?.Equals(default) == false)
                        matches.Add((firstTeam, secondTeam));
                }
            }

            return matches;
        }
    }
}
