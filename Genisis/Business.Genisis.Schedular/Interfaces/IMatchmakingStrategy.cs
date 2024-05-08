namespace Business.Genesis.Scheduler.Interfaces
{
    public interface IMatchmakingStrategy
    {
        /// <summary>
        /// Algorithm to create a Roster on the Teams in a Category.
        /// </summary>
        /// <param name="teams"></param>
        /// <returns></returns>
        IEnumerable<(string First, string Second)> RoundRobin(List<string> teams);
    }
}
