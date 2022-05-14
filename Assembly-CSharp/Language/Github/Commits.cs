using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using Newtonsoft.Json;
using UnityEngine;

namespace Language.Github
{
    public static class Commits
    {
        public static Json.Root[] GetCommits( string jsonContent )
        {
            return JsonConvert.DeserializeObject<Json.Root[]>(jsonContent);
        }

        public static Json.Version GetVersion( string jsonContent )
        {
            return JsonUtility.FromJson<Json.Version>( jsonContent );
        }

        public static Json.Version GetFromFile( string path )
        {
            if ( !File.Exists( path ))
                return null;

            string jsonContent = File.ReadAllText( path, GetEncoding );
            return GetVersion( jsonContent );
        }

        public static void WriteToFile( string file, Json.Root root )
        {
            string filePath = AbsolutePath( file );
            
            if ( File.Exists( filePath ))
                File.Delete( filePath );
            else if ( !Directory.Exists( AbsolutePath() ) )
                Directory.CreateDirectory( AbsolutePath() );

            Json.Version version = new Json.Version();
            version.sha = root.sha;

            WriteFile( filePath, version );
        }

        static void WriteFile( string path, Json.Version version )
        {
            string json = JsonUtility.ToJson( version );
            File.WriteAllText( path, json, GetEncoding );
        }

        public static string AbsolutePath( string file = "" )
        {
            return Path.Combine( Application.dataPath, Language.LANG_DIRECTORY + file );
        }

        static Encoding GetEncoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
}