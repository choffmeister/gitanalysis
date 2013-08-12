using System;
using QuickGraph;
using LibGit2Sharp;

namespace GitAnalysis.Common
{
    public class GitCommitGraph : AdjacencyGraph<Commit, Edge<Commit>>
    {
        public GitCommitGraph(IRepository repo)
        {
            foreach (Commit commit in repo.Commits)
            {
                this.AddVertex(commit);

                foreach (Commit parent in commit.Parents)
                {
                    this.AddVertex(parent);
                    this.AddEdge(new Edge<Commit>(parent, commit));
                }
            }
        }
    }
}

