using System;
using System.IO;
using LibGit2Sharp;

namespace GitAnalysis.CLI
{
    public static class RepositoryHelper
    {
        public static IRepository Open(string userName, string repositoryName)
        {
            string sourceUrl = string.Format("git://github.com/{0}/{1}", userName, repositoryName);
            string targetPath = string.Format("repositories/{0}/{1}", userName, repositoryName);

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

