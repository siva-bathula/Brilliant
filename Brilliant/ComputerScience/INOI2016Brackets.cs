using GenericDefs.DotNet;
using System;

namespace Brilliant.ComputerScience
{
    public class INOI2016Brackets : ISolve, IBrilliant, IProblemName
    {
        Brilliant thisProblem;
        Brilliant IBrilliant.ProblemDefOnBrilliantOrg
        {
            get
            {
                return thisProblem;
            }
        }

        string IProblemName.GetName()
        {
            return "INOI 2016, Brackets";
        }

        void ISolve.Init()
        {
            thisProblem = new Brilliant("https://brilliant.org/problems/inoi-2016-brackets/");
        }

        int N, K;
        int[] V, B;
        int[,] dp;
 
        int ans(int l, int r)
        {
            if (dp[l,r] != -1) return dp[l,r];
            if (l >= r) return 0;
            if(r - l == 1)
            {
                if(B[r] - B[l] == K) return V[l] + V[r];
                else return 0;
            }

            int answer = 0;

            for (int i = l; i <= r; i++)
            {
                if (B[l] + K == B[i]) answer = Math.Max(answer, ans(l + 1, i - 1) + V[l] + V[i] + ans(i + 1, r));
            }

            answer = Math.Max(answer, ans(l + 1, r));
            
            dp[l,r] = answer;
            return answer;
        }

        void ISolve.Solve()
        {
            string html = Http.Request.GetHtmlResponse("https://gist.githubusercontent.com/anonymous/ddd8ee38e7924d933f93/raw/753c999b818556e8263c93654becaea3cf96bf31/2_7.in");
            string[] input = html.Splitter(StringSplitter.SplitUsing.Space, StringSplitOptions.RemoveEmptyEntries);
            int VCounter = -1, BCounter = -1;
            for (int i = 0; i < input.Length; i++)
            {
                if (VCounter < 0)
                {
                    if (i == 0) { N = int.Parse(input[0]);
                        B = new int[N+2];
                        V = new int[N+2];
                        dp = new int[N+2, N+2];
                        for (int a = 1; a <= N+1; a++)
                        {
                            for (int b = 1; b <= N+1; b++)
                            {
                                dp[a, b] = -1;
                            }
                        }
                    } else if (i == 1) { VCounter = 0; K = int.Parse(input[1]); }
                } else if(VCounter < N) {
                    VCounter++;
                    V[VCounter] = int.Parse(input[i]);
                    if (VCounter == 700) BCounter = 0;
                } else if (BCounter < N) {                    
                    BCounter++;
                    B[BCounter] = int.Parse(input[i]);
                }
            }

            System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
            s.Start();
            QueuedConsole.WriteImmediate("{0}", ans(1, N));
            s.Stop();
            QueuedConsole.WriteImmediate("Time taken : {0} ms.", s.ElapsedMilliseconds);
        }
    }
}