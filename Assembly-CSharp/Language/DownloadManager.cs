using System;
using System.IO;
using System.Linq;

using UnityEngine;

namespace Language
{
    public class DownloadManager : MonoBehaviour
    {
        enum RequestState
        {
            REPOS,
            CONTENTS
        }

        const string CLOUD = "https://filipvrba.github.io/hollow-knight-lang/src/bytes/";
        const string REPOS = "https://api.github.com/repos/filipvrba/hollow-knight-lang/git/trees/37953c96b6e72b0139c5a4c11a08819331bf91d1";

        RequestState requestState;
        RestAPI restApi;

        public void Awake()
        {
            requestState = RequestState.REPOS;
            restApi = new RestAPI( this );
        }

        public void Start()
        {
            string[] sheetTitles = Language.settings.sheetTitles;
            RestAPIResult( sheetTitles );
        }

        void RestAPIResult( string[] args )
        {
            switch( requestState )
            {
                case RequestState.REPOS:

                    SwitchRequestState( RequestState.CONTENTS );
                    LanguageSheets( args, StartGetContent );
                    return;

                case RequestState.CONTENTS:

                    WriteContents( args );
                    return;
            }
        }

        void RestAPIFinish()
        {
            Language.LoadAvailableLanguages();
        }

        void WriteContents( string[] args )
        {
            string fileName = args[1].Replace( CLOUD, "" );
            string absolutePathFile = Path.Combine( AbsolutePathDirectory(), fileName );
            string contents = args[0];

            CreateLangDirectory();

            File.WriteAllText( absolutePathFile, contents );
        }

        void CreateLangDirectory()
        {
            if ( !Directory.Exists( AbsolutePathDirectory() )) 
            {
                Directory.CreateDirectory( AbsolutePathDirectory() );
            }
        }

        string AbsolutePathDirectory()
        {
            return Local.AbsolutePathFile( Language.LANG_DIRECTORY, true );
        }

        void SwitchRequestState( RequestState state )
        {
            requestState = state;
        }

        void StartGetContent( string file )
        {
            string pathContent = AbsolutePathDirectory() + file;

            if ( File.Exists( pathContent ) )
            {
                restApi.SetStartContentsCount( restApi.GetStartContentsCount() - 1 );
                return;
            }

            pathContent = $"{ CLOUD }{ file }";
            restApi.StartRequest( pathContent );
        }

        void LanguageSheets( string[] sheetTitles, Action<string> callback )
        {
            string[] suppLang = Enum.GetNames( typeof( GlobalEnums.SupportedLanguages ));

            int titlesCount = sheetTitles.Count();
            int suppLangCount = suppLang.Count();

            restApi.Free();
            restApi.SetStartContentsCount( suppLangCount * titlesCount );

            foreach( string sheetTitle in sheetTitles )
            {
                foreach( string lang in suppLang )
                {
                    string file = $"{ lang }_{ sheetTitle }{ Local.TYPE_FILE }";
                    callback( file );
                }
            }
        }
    }
}
