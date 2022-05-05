using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

namespace Language
{
    public class RestAPI
    {
        const string RESULT_MESSAGE = "RestAPIResult";

        MonoBehaviour parent { get; }

        public RestAPI( MonoBehaviour parent )
        {
            this.parent = parent;
        }

        public void StartRequest( string uri )
        {
            parent.StartCoroutine( GetRequest( uri ));
        }

        IEnumerator GetRequest( string uri )
        {
            using ( UnityWebRequest webRequest = UnityWebRequest.Get( uri ))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if ( !webRequest.isHttpError )
                {
                    ResultCallback( webRequest.downloadHandler, uri );
                }
            }
        }

        void ResultCallback( DownloadHandler downloadHandler, string uri )
        {
            string[] args = { downloadHandler.text, uri };
            parent.SendMessage( RESULT_MESSAGE, args );
        }
    }
}
