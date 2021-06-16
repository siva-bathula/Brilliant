using GenericDefs.DotNet;
namespace Brilliant
{
    public interface ISolve
    {
        void Solve();
        void Init();
    }
    interface IBrilliant
    {
        Brilliant ProblemDefOnBrilliantOrg { get; }
    }
    public interface ISaveProgress
    {
        System.Threading.Tasks.Task<bool> Save();
        bool CanSave { get; set; }
    }
    public interface IRunSaved
    {
        void Resume();
        bool IsResume { get; set; }
    }
    public class Brilliant
    {
        internal string DirectHttpBrilliantLink;
        internal Brilliant() { }
        internal Brilliant(string link)
        {
            DirectHttpBrilliantLink = link;
        }
    }
    public interface IProblemName
    {
        string GetName();
    }
    public class LongRunningTask : Brilliant
    {
        internal LongRunningTask() : base() { }
        internal LongRunningTask(string link) : base(link)
        {

        }
        internal bool IsParallelProcess { get; set; }
        internal bool HaltInvoked { get; set; }
        internal bool Completed { get; set; }
        internal bool Pausing { get; set; }
        internal bool Paused { get; set; }
        internal bool SaveSuccess { get; set; }
        internal int DegreeOfParallelism { get; set; }
        internal SaveProgressContext SaveContext { get; set; }
    }
}