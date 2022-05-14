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
            CONTENTS,
            VERSION
        }

        const string CLOUD = "https://filipvrba.github.io/hollow-knight-lang/src/bytes/";

        RequestState requestState;
        RestAPI restApi;
        Version version;

        bool isLoadLanguages;

        public void Awake()
        {
            requestState = RequestState.VERSION;
            restApi = new RestAPI( this );
            version = new Version( restApi );

            free();
        }

        public void Start()
        {
            version.Init();
        }

        void free()
        {
            isLoadLanguages = false;
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

                case RequestState.VERSION:

                    version.Verify( args[0], VersionVerifyFinish );
                    return;
            }
        }

        void VersionVerifyFinish( bool isLoadLanguages )
        {
            this.isLoadLanguages = isLoadLanguages;
            SwitchRequestState( RequestState.REPOS );

            string[] sheetTitles = Language.settings.sheetTitles;
            RestAPIResult( sheetTitles );
        }

        void RestAPIFinish()
        {
            if ( Language.GetLanguages().Count() < GetSupportedLanguages().Count() )
            {
                Language.LoadAvailableLanguages();
            }

            if ( isLoadLanguages ) {

                free();
                Language.LoadLanguage();
            }
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
                restApi.SetStartContentsCount( -1 );
                return;
            }

            pathContent = $"{ CLOUD }{ file }";
            restApi.StartRequest( pathContent );
        }

        string[] GetSupportedLanguages()
        {
            return Enum.GetNames( typeof( GlobalEnums.SupportedLanguages ));
        }

        void LanguageSheets( string[] sheetTitles, Action<string> callback )
        {
            string[] suppLang = GetSupportedLanguages();

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
