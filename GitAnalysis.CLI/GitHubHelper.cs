using System;
using System.Net;
using System.Collections;
using System.IO;
using System.Linq;
using ServiceStack.Text.Json;
using System.Collections.Generic;

namespace GitAnalysis.CLI
{
    public static class GitHubHelper
    {
        public static IEnumerable<GitHubRepositoryInfo> SearchRepositories(int minForks = 100)
        {
            int page = 0;
            int pageSize = 25;

            while (true)
            {
                string url = string.Format("https://api.github.com/search/repositories?q=forks:>={2} fork:false&sort=forks&page={0}&per_page={1}", page, pageSize, minForks);

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.Accept = "application/vnd.github.preview.text-match+json";

                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    RepositorySearchResponse res = (RepositorySearchResponse)JsonReader<RepositorySearchResponse>.Parse(reader.ReadToEnd());

                    foreach (RepositorySearchItem item in res.items)
                    {
                        string[] fullNameSplit = item.full_name.Split(new char[] { '/' });

                        yield return new GitHubRepositoryInfo()
                        {
                            UserName = fullNameSplit[0],
                            RepositoryName = fullNameSplit[1]
                        };
                    }

                    if (res.items.Count < pageSize)
                    {
                        yield break;
                    }
                    else
                    {
                        page++;
                    }
                }
            }
        }

        private class RepositorySearchResponse
        {
            public int total_count { get; set; }

            public List<RepositorySearchItem> items { get; set; }
        }

        private class RepositorySearchItem
        {
            public string full_name { get; set; }
        }
    }

    public class GitHubRepositoryInfo
    {
        public string UserName { get; set; }

        public string RepositoryName { get; set; }
    }
}

