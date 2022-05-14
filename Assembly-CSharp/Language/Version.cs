using System;
using System.IO;

using UnityEngine;
using Language.Github;

namespace Language
{
    public class Version
    {
        const string FILE_VERSION = "version.json";
        const int MIN_MINUTE = 5;

        RestAPI restApi { get; }

        public Version( RestAPI restApi )
        {
            // Ziskat info sha a time z githubu a ulozit je do local folder.
            // Tim se bude kontrolovat pristupnost nove verze language.
            // Sha - identification commit
            // Time - v jakem case commit byt pridan
            // Pomoci time zamezime pristupnost po dobu 5 minut, nez se nahraji soubory na cloud

            this.restApi = restApi;
        }

        public void Init()
        {
            restApi.StartRequest( Github.Json.Constants.COMMIT_URI );
        }

        public void Verify( string json, Action<bool> callback )
        {
            var firstCommit = Commits.GetCommits( json )[0];
            string commitSha = firstCommit.sha;

            string absolutePathFile = Commits.AbsolutePath( FILE_VERSION );
            var version = Commits.GetFromFile( absolutePathFile );

            Debug.Log( version );

            if ( version == null )
            {
                Commits.WriteToFile( FILE_VERSION, firstCommit );
                callback(false);
            }
            else
            {
                DateTime dateCommit = firstCommit.commit.committer.date;
                int minuteDifference = (int)DateTime.UtcNow.Subtract( dateCommit ).TotalMinutes;

                if ( commitSha != version.sha && minuteDifference >= MIN_MINUTE )
                {
                    Debug.Log( $"{version.sha} (New language version)" );
                    Local.RemoveAllFiles();

                    Commits.WriteToFile( FILE_VERSION, firstCommit );
                    callback(true);
                }
            }
        }
    }
}
