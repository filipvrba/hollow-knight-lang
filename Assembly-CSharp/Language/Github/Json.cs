using System;
using System.Collections.Generic;

namespace Language.Github.Json
{
    public class Constants
    {
        public const string REPOS_URI = "https://api.github.com/repos/filipvrba/hollow-knight-lang";
        public const string COMMIT_URI = $"{ REPOS_URI }/commits?sha=main";
    }

    [Serializable]
    public class Version
    {
        public string sha;
    }

    [Serializable]
    public class Author
    {
        public string name;
        public string email;
        public DateTime date;
        public string login;
        public int id;
        public string node_id;
        public string avatar_url;
        public string gravatar_id;
        public string url;
        public string html_url;
        public string followers_url;
        public string following_url;
        public string gists_url;
        public string starred_url;
        public string subscriptions_url;
        public string organizations_url;
        public string repos_url;
        public string events_url;
        public string received_events_url;
        public string type;
        public bool site_admin;
    }

    [Serializable]
    public class Commit
    {
        public Author author;
        public Committer committer;
        public string message;
        public Tree tree;
        public string url;
        public int comment_count;
        public Verification verification;
    }

    [Serializable]
    public class Committer
    {
        public string name;
        public string email;
        public DateTime date;
        public string login;
        public int id;
        public string node_id;
        public string avatar_url;
        public string gravatar_id;
        public string url;
        public string html_url;
        public string followers_url;
        public string following_url;
        public string gists_url;
        public string starred_url;
        public string subscriptions_url;
        public string organizations_url;
        public string repos_url;
        public string events_url;
        public string received_events_url;
        public string type;
        public bool site_admin;
    }

    [Serializable]
    public class Parent
    {
        public string sha;
        public string url;
        public string html_url;
    }

    [Serializable]
    public class Root
    {
        public string sha;
        public string node_id;
        public Commit commit;
        public string url;
        public string html_url;
        public string comments_url;
        public Author author;
        public Committer committer;
        public List<Parent> parents;
    }

    [Serializable]
    public class Tree
    {
        public string sha;
        public string url;
    }

    [Serializable]
    public class Verification
    {
        public bool verified;
        public string reason;
        public string signature;
        public string payload;
    }

    [Serializable]
    public class Repos
    {
        public string sha;
        public string url;
        public List<Content> tree;
        public bool truncated;
    }

    [Serializable]
    public class Content
    {
        public string path;
        public string mode;
        public string type;
        public string sha;
        public int size;
        public string url;
    }
}
