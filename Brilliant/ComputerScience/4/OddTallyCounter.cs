using GenericDefs.Classes;
using GenericDefs.DotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brilliant.ComputerScience._4
{
    public class OddTallyCounter : LongRunningTask, ISolve, IRunSaved, ISaveProgress
    {
        void ISolve.Init()
        {
            ((IRunSaved)this).IsResume = true;
            IsParallelProcess = true;
            DegreeOfParallelism = 3;

            if (((ISaveProgress)this).CanSave)
            {
                SaveContext = new SaveProgressContext();
            }
        }

        public OddTallyCounter()
        {
            ((Brilliant)this).DirectHttpBrilliantLink = "https://brilliant.org/problems/odd-tally-counter/";
        }

        public OddTallyCounter(string link) : base(link)
        {

        }

        void ISolve.Solve()
        {
            Solve2Parallel();
        }

        void Solve1()
        {
            Stopwatch w = new Stopwatch();
            w.Start();
            long sumFi = 0;
            for (int i = 1; i <= 9999; i++)
            {
                int b = 0;
                int? start = null;
                int counter = 0;
                while (true)
                {
                    b += i;
                    b %= 10000;

                    if (!start.HasValue)
                    {
                        start = b;
                        counter++;
                        continue;
                    }
                    if (start.Value == b) break;
                    counter++;
                }
                sumFi += counter;
                sumFi %= 10000;
            }
            w.Stop();
            Console.WriteLine("Time taken : {0}", w.ElapsedMilliseconds);
            Console.WriteLine("Total sum :: {0}", sumFi);
            Console.ReadKey();
        }

        void Solve1Parallel()
        {
            Stopwatch w = new Stopwatch();
            w.Start();
            List<int> items = new List<int>();
            for (int i = 1; i <= 99999; i++)
            {
                items.Add(i);
            }

            Parallel.ForEach(items, item => CountAndCollate(item, 100000));
            w.Stop();
            Console.WriteLine("Time taken : {0}", w.ElapsedMilliseconds);
            Console.WriteLine("Total sum :: {0}", sumFiMod1000 % 1000);

            string answer = string.Format("Last three digits is :: {0}", (sumFiMod1000 % 1000).ToString());

            LoggerContext lc = new LoggerContext();
            lc.AppendCurrentLog("https://brilliant.org/problems/odd-long-tally-counter/");
            lc.AppendNewLine();
            lc.AppendCurrentLog("OddtallyCounter.");
            lc.AppendNewLine();
            lc.AppendCurrentLog(answer);
            Logger.Log(lc);

            QueuedConsole.WriteFinalAnswer(answer);
        }

        private object _synclock = new object();
        private long sumFiMod1000 = 0;
        void CountAndCollate(int i, int imax)
        {
            int c = Counter(i, imax);
            lock (_synclock)
            {
                sumFiMod1000 += c;
                sumFiMod1000 %= 1000;
                CurrentExecutionCounter++;
            }
        }

        private int CurrentExecutionCounter
        {
            get; set;
        }

        bool IRunSaved.IsResume
        {
            get; set;
        }

        bool ISaveProgress.CanSave
        {
            get { return true; }
            set { ((ISaveProgress)this).CanSave = value; }
        }

        int Counter(int i, int max)
        {
            int b = 0;
            int? start = null;
            int counter = 0;
            while (true)
            {
                b += i;
                b %= max;

                if (!start.HasValue)
                {
                    start = b;
                    counter++;
                    continue;
                }
                if (b == start.Value) break;
                counter++;
            }

            return counter;
        }

        /// <summary>
        /// https://brilliant.org/problems/odd-long-tally-counter/
        /// </summary>
        void Solve2()
        {
            long sumFi = 0;
            for (int i = 1; i <= 9999999; i++)
            {
                int b = 0;
                int? start = null;
                int counter = 0;
                while (true)
                {
                    b += i;
                    b %= 10000000;

                    if (!start.HasValue)
                    {
                        start = b;
                        counter++;
                        continue;
                    }
                    if (start.Value == b) break;
                    counter++;
                }
                sumFi += counter;
                sumFi %= 1000;
            }

            string answer = string.Format("Last three digits is :: {0}", (sumFi % 1000).ToString());
            Logger.Log("https://brilliant.org/problems/odd-long-tally-counter/");
            Logger.Log("OddtallyCounter. ");
            Logger.Log(answer);
            QueuedConsole.WriteFinalAnswer(answer);
            Console.WriteLine();
            Console.ReadKey();
        }

        /// <summary>
        /// https://brilliant.org/problems/odd-long-tally-counter/
        /// </summary>
        void Solve2Parallel()
        {
            Func<StatusUpdater, StatusUpdater> initializer = delegate (StatusUpdater su)
            {
                string message = "Total numbers done checking " + CurrentExecutionCounter;
                message += Environment.NewLine;
                message += "Percentage completed : " + (CurrentExecutionCounter * 100.0 / Math.Pow(10, 7)).ToString("0.00");
                su.SetMessage(message);
                return su;
            };

            StatusUpdater s = new StatusUpdater(30000, initializer);
            s.Start();

            List<int> items = new List<int>();
            int iMax = (int)Math.Pow(10, 7) - 1;
            for (int i = 1; i <= iMax; i++)
            {
                items.Add(i);
            }

            Batch batch = new Batch() { MaxSize = 100 };

            if (((IRunSaved)this).IsResume || ((ISaveProgress)this).CanSave)
            {
                int init = 1;
                Answer<long> savedAnswer = null;
                if (((IRunSaved)this).IsResume)
                {
                    savedAnswer = Serialization.DeSerialize<long>(SaveContext);
                    QueuedConsole.WriteImmediate("Loaded successfuly.");
                    Batch lastProcessedBatch = savedAnswer.Batches[savedAnswer.Batches.Count - 1];
                    CurrentExecutionCounter = lastProcessedBatch.CurrentMax;
                    init = 1 + (lastProcessedBatch.CurrentMax + 1) / batch.MaxSize;
                    sumFiMod1000 = savedAnswer.LastSavedAnswer;
                    QueuedConsole.WriteImmediate("Last saved value : " + sumFiMod1000);
                    QueuedConsole.WriteImmediate("Will execute from here : " + init);
                }
                else {
                    savedAnswer = new Answer<long>();
                    savedAnswer.Batches = new List<Batch>();
                }
                for (int i = init; i <= (int)Math.Ceiling(1.0 * iMax / batch.MaxSize); i++)
                {
                    batch.CurrentMin = batch.MaxSize * (i - 1);
                    batch.CurrentMax = ((batch.MaxSize * i) - 1) > iMax ? iMax : ((batch.MaxSize * i) - 1);
                    batch.CurrentRange = batch.CurrentMax - batch.CurrentMin + 1;
                    if (i == (int)Math.Ceiling(1.0 * iMax / batch.MaxSize)) batch.CurrentRange = batch.CurrentMax - batch.CurrentMin;

                    if (batch.CurrentRange <= 0) break;
                    ParallelLoopResult result = Parallel.ForEach(items.GetRange(batch.CurrentMin, batch.CurrentRange),
                        new ParallelOptions() { MaxDegreeOfParallelism = 2 },
                        item => CountAndCollate(item, iMax + 1));

                    if (HaltInvoked)
                    {
                        Pausing = true;
                        Paused = false;
                        savedAnswer.Batches.Add(batch);
                        savedAnswer.LastSavedAnswer = sumFiMod1000;
                        if (Serialization.Serialize(savedAnswer, SaveContext))
                        {
                            SaveSuccess = true;
                            QueuedConsole.WriteImmediate("Saved progress successfully.");
                            QueuedConsole.WriteImmediate("Last saved value : + " + sumFiMod1000);
                            QueuedConsole.WriteImmediate("Last batch executed was : + " + batch.CurrentMin + " - " + batch.CurrentMax);
                        }
                        Pausing = false;
                        Paused = true;
                    }
                }
            }
            else {
                Parallel.ForEach(items, new ParallelOptions() { MaxDegreeOfParallelism = DegreeOfParallelism }, item => CountAndCollate(item, iMax + 1));
            }

            string answer = string.Format("Last three digits is :: {0}", (sumFiMod1000 % 1000).ToString());

            LoggerContext lc = new LoggerContext();
            lc.AppendCurrentLog("https://brilliant.org/problems/odd-long-tally-counter/");
            lc.AppendCurrentLog("OddtallyCounter.");
            lc.AppendCurrentLog(answer);
            Logger.Log(lc);
            Completed = true;
            QueuedConsole.WriteFinalAnswer(answer);
        }

        void IRunSaved.Resume()
        {
            SaveContext = new SaveProgressContext("e1662a38-3c1f-46e8-a5a9-ee29c900aa60");
            if (string.IsNullOrEmpty(SaveContext.Context)) { throw new Exception("No file specified for resumption"); }
            else if (!System.IO.File.Exists(SaveContext.FileName)) { throw new Exception("File specified for resumption was not found!!"); }

            Solve2Parallel();
        }

        async Task<bool> ISaveProgress.Save()
        {
            HaltInvoked = true;
            if (Completed) return await Task.FromResult(Completed);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (Paused) { break; }
                if (sw.ElapsedMilliseconds > 30000) {
                    break;
                }
            }
            
            return await Task.FromResult(SaveSuccess);
        }
    }
}