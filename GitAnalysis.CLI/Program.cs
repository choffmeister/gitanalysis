using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

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
                    foreach (Commit commit in repo.Head.Commits)
                    {
                        //Console.WriteLine("[{0}] {1}", commit.Sha.Substring(0, 8), commit.MessageShort);
                    }
                }
            }
        }
    }
}
