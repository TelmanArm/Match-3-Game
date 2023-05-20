using System;
using UnityEngine;

namespace Services.Log
{
    public class LogService
    {
        public bool IsLog { get; set; }
        public bool IsErrorLog { get; set; }

        public void Log(string massage)
        {
            if (IsLog)
            {
                Debug.Log(massage);
            }
        }

        public void ErrorLog(string errorMassage)
        {
            if (IsErrorLog)
            {
                Debug.LogError(errorMassage);
            }
        }
    }
}