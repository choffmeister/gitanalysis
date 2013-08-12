using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;
using GitAnalysis.Common;
using QuickGraph;

namespace GitAnalysis.CLI
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            IEnumerable<GitHubRepositoryInfo> repoInfos = GitHubHelper.SearchRepositories(10000).Take(10);

            foreach (GitHubRepositoryInfo repoInfo in repoInfos)
            {
                using (IRepository repo = RepositoryHelper.Open(repoInfo.UserName, repoInfo.RepositoryName))
                {
                    GitCommitGraph graph = new GitCommitGraph(repo);

                    Console.WriteLine("{0} commits", graph.VertexCount);
                }
            }
        }
    }
}
