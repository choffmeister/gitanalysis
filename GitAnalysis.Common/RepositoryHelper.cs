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
using LibGit2Sharp;

namespace GitAnalysis.Common
{
    public static class RepositoryHelper
    {
        public static IRepository Open(string userName, string repositoryName)
        {
            string sourceUrl = string.Format("git://github.com/{0}/{1}", userName, repositoryName);
            string targetPath = string.Format(Path.Combine("repositories", userName, repositoryName));

            try
            {
                if (!Directory.Exists(targetPath))
                {
                    Console.WriteLine("* Cloning from {0}...", sourceUrl);
                    IRepository repository = Repository.Clone(sourceUrl, targetPath, true, onTransferProgress: TransferProgress);
                    Console.WriteLine("* Done.");

                    return repository;
                }
                else
                {
                    Console.WriteLine("* Fetching from {0}...", sourceUrl);
                    IRepository repositroy = new Repository(targetPath);
                    repositroy.Network.Fetch(repositroy.Network.Remotes ["origin"], onTransferProgress: TransferProgress);
                    Console.WriteLine("* Done.");

                    return repositroy;
                }
            }
            catch (Exception ex)
            {
                DeleteDirectoryIfExists(targetPath);
                throw ex;
            }
        }

        private static void DeleteDirectoryIfExists(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        private static int TransferProgress(TransferProgress progress)
        {
            Console.Write("{0:0.00}%\r", (double)progress.ReceivedObjects / (double)progress.TotalObjects * 100.0);

            return 0;
        }
    }
}
