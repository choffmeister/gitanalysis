/*
 * Copyright (C) 2013 Christian Hoffmeister
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see {http://www.gnu.org/licenses/}.
 */
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
            LoadRepositories(100, 100);

            foreach (string userPath in Directory.EnumerateDirectories("repositories", "*", SearchOption.TopDirectoryOnly))
            {
                foreach (string repositoryPath in Directory.EnumerateDirectories(userPath, "*", SearchOption.TopDirectoryOnly))
                {
                    using (Repository repo = new Repository(repositoryPath))
                    {
                        GitCommitGraph graph = new GitCommitGraph(repo);

                        Console.WriteLine("{0} commits", graph.VertexCount);
                    }
                }
            }
        }

        private static void LoadRepositories(int minForks, int count)
        {
            IEnumerable<GitHubRepositoryInfo> repoInfos = GitHubHelper.SearchRepositories(minForks).Take(count);

            foreach (GitHubRepositoryInfo repoInfo in repoInfos)
            {
                using (IRepository repo = RepositoryHelper.Open(repoInfo.UserName, repoInfo.RepositoryName))
                {
                }
            }
        }
    }
}
