using System;
using System.IO;

using UnityEngine;

namespace Language
{
    public static class Local
    {
        public const string TYPE_FILE = ".bytes";

        public static string GetLanguageFileContents( string pathFile )
        {
            return File.ReadAllText( AbsolutePathFile( pathFile ) );
        }

        public static bool HasLanguageFile( string pathFile, bool isType = false )
        {
            return File.Exists( AbsolutePathFile( pathFile, isType ) );
        }

        public static string AbsolutePathFile( string pathFile, bool isType = false )
        {
            string absolutePathFile = Path.Combine( Application.dataPath, pathFile );
            
            if ( isType )
                return absolutePathFile;
            else
                return absolutePathFile + TYPE_FILE;
        }
    }
}