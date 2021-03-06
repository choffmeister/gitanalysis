﻿/*
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
            RepositoryHelper.RepositoriesBasePath = "repositories";

            int minForks = 100;
            int count = 100;
            LoadRepositories(minForks, count);

            using (StreamWriter output = new StreamWriter(new FileStream("result.csv", FileMode.Create, FileAccess.Write)))
            {
                output.WriteLine("username;repositoryname;commitcount;forkcount;watchercount;authors;authortimezones;nonmergecommitcount");

                foreach (string userPath in Directory.EnumerateDirectories("repositories", "*", SearchOption.TopDirectoryOnly))
                {
                    string userName = Path.GetFileName(userPath);

                    foreach (string repositoryPath in Directory.EnumerateDirectories(userPath, "*", SearchOption.TopDirectoryOnly))
                    {
                        string repositoryName = Path.GetFileName(repositoryPath);

                        using (Repository repo = new Repository(repositoryPath))
                        {
                            GitHubRepositoryInfo repoInfo = GitHubHelper.LoadGitHubRepositoryInfo(userName, repositoryName);

                            int commitCount = 0;
                            int nonMergeCommitCount = 0;
                            HashSet<string> authorEmails = new HashSet<string>();
                            HashSet<int> authorTimezones = new HashSet<int>();

                            foreach (Commit commit in repo.Commits)
                            {
                                commitCount++;
                                if (commit.Parents.Count() <= 1)
                                {
                                    nonMergeCommitCount++;
                                }

                                authorEmails.Add(commit.Author.Email);
                                authorTimezones.Add((int)Math.Round(commit.Author.When.Offset.TotalMinutes));
                            }

                            Console.WriteLine("{0}/{1} | {2} commits, {3} forks, {4} watchers, {5} authors, {6} authortimezones, {7} nonmergecommitcount", userName, repositoryName, commitCount, repoInfo.ForkCount, repoInfo.WatcherCount, authorEmails.Count, authorTimezones.Count, nonMergeCommitCount);

                            output.Write(userName);
                            output.Write(";");
                            output.Write(repositoryName);
                            output.Write(";");
                            output.Write(commitCount);
                            output.Write(";");
                            output.Write(repoInfo.ForkCount);
                            output.Write(";");
                            output.Write(repoInfo.WatcherCount);
                            output.Write(";");
                            output.Write(authorEmails.Count);
                            output.Write(";");
                            output.Write(authorTimezones.Count);
                            output.Write(";");
                            output.Write(nonMergeCommitCount);
                            output.WriteLine();
                        }
                    }
                }
            }
        }

        private static void LoadRepositories(int minForks, int count)
        {
            int i = 0;
            IEnumerable<GitHubRepositoryInfo> repoInfos = GitHubHelper.SearchRepositories(minForks).Take(count);

            foreach (GitHubRepositoryInfo repoInfo in repoInfos)
            {
                Console.WriteLine("# {0}/{1}", i++, count);

                using (IRepository repo = RepositoryHelper.Open(repoInfo.UserName, repoInfo.RepositoryName))
                {
                    GitHubHelper.SaveGitHubRepositoryInfo(repoInfo.UserName, repoInfo.RepositoryName, repoInfo);
                }
            }
        }
    }
}
