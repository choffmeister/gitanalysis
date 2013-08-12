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
using System.Net;
using System.Collections;
using System.IO;
using System.Linq;
using ServiceStack.Text.Json;
using System.Collections.Generic;

namespace GitAnalysis.Common
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
