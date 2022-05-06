using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

namespace Language
{
    public class RestAPI
    {
        const string RESULT_MESSAGE = "RestAPIResult";
        const string FINISH_MESSAGE = "RestAPIFinish";

        int requestCount;
        int startContentsCount;

        MonoBehaviour parent { get; }

        public RestAPI( MonoBehaviour parent )
        {
            this.parent = parent;

            Free();
        }

        public void StartRequest( string uri )
        {
            parent.StartCoroutine( GetRequest( uri ));
        }

        public void Free()
        {
            requestCount = 0;
            startContentsCount = 0;
        }

        public int GetRequestCount()
        {
            return requestCount;
        }

        public void SetStartContentsCount( int count )
        {
            startContentsCount = count;
        }

        public int GetStartContentsCount()
        {
            return startContentsCount;
        }

        IEnumerator GetRequest( string uri )
        {
            using ( UnityWebRequest webRequest = UnityWebRequest.Get( uri ))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                SetRequestCount();

                if ( !webRequest.isHttpError )
                {
                    ResultCallback( webRequest.downloadHandler, uri );
                }

                if ( GetStartContentsCount() == GetRequestCount() )
                {
                    parent.SendMessage( FINISH_MESSAGE );
                }
            }
        }

        void ResultCallback( DownloadHandler downloadHandler, string uri )
        {
            string[] args = { downloadHandler.text, uri };
            parent.SendMessage( RESULT_MESSAGE, args );
        }

        void SetRequestCount( int number = 1 )
        {
            requestCount += number;
        }
    }
}
