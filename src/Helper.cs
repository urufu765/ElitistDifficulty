using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ElitistDiff;
using UnityEngine;
using static Guardian.CloudLogger;

namespace EliteHelper;

public static class Helper
{
    /// <summary>
    /// A simple list that expands upon having an index greater than the size
    /// </summary>
    public class ExpandableList<T> : List<T>
    {
        private readonly List<T> _list = new();

        public new T this[int index]
        {
            get
            {
                while (index >= _list.Count)
                {
                    _list.Add(default);
                }
                return _list[index];
            }
            set
            {
                if (index >= _list.Count)
                {
                    while (index > _list.Count)
                    {
                        _list.Add(default);
                    }
                    _list.Add(value);
                }
                else
                {
                    _list[index] = value;
                }
            }
        }
    }

    private static string prevLog = "";
    private static int logRepeat;
    private static readonly ExpandableList<string> prevLogs = new();
    private static readonly ExpandableList<int> logRepeats = new();
    public static int logImportance = 4;

    public static void L(string message, int logPrio = 3, bool ignoreRepeats = false, [CallerMemberName] string callerName = "")
    {
        if (logPrio <= logImportance)
        {
            if (message != prevLog || ignoreRepeats)
            {
                if (logRepeat > 0)
                {
                    Debug.Log($"-> Elitist [{callerName}]: Previous message repeated {logRepeat} times: {prevLog}");
                }
                prevLog = message;
                logRepeat = 0;
                Debug.Log($"-> Elitist [{callerName}]: {message}");
            }
            else
            {
                logRepeat++;
            }
        }
    }


    public static void L(Exception exception, string message = "Caught error!", int logPrio = 0, [CallerMemberName] string callerName = "")
    {
        if (logPrio <= logImportance)
        {
            string toSend = $"-> Elitist [{callerName}]: {message}";
            Debug.LogError(toSend);
            Debug.LogException(exception);
            if (Plugin.Patch_Guardian)
            {
                Throw_Exception_At_Vigaro(new Exception(toSend, exception));
            }
        }
    }

    public static void L(this Player self, string message, int logPrio = 3, bool ignoreRepeats = false, [CallerMemberName] string callerName = "")
    {
        if (self is null)
        {
            L(message, logPrio, ignoreRepeats, callerName);
            return;
        }
        try
        {
            if (logPrio <= logImportance)
            {
                if (message != prevLogs[self.playerState.playerNumber] || ignoreRepeats)
                {
                    if (logRepeats[self.playerState.playerNumber] > 0)
                    {
                        Debug.Log($"-> Elitist [{callerName}|{self.playerState.playerNumber}]: Previous message repeated {logRepeats[self.playerState.playerNumber]} times: {prevLogs[self.playerState.playerNumber]}");
                    }
                    prevLogs[self.playerState.playerNumber] = message;
                    logRepeat = 0;
                    Debug.Log($"-> Elitist [{callerName}|{self.playerState.playerNumber}]: {message}");
                }
                else
                {
                    logRepeats[self.playerState.playerNumber]++;
                }
            }
        }
        catch (Exception e)
        {
            L(message, logPrio, ignoreRepeats, callerName);
            L(e, "Logging player fail!", logPrio: 4);
        }
    }

    public static void L(this Player self, Exception exception, string message = "Caught error!", int logPrio = 0, [CallerMemberName] string callerName = "")
    {
        if (self is null)
        {
            L(exception, message, logPrio, callerName);
            return;
        }
        try
        {
            if (logPrio <= logImportance)
            {
                string toSend = $"-> Elitist [{callerName}|{self.playerState.playerNumber}]: {message}";
                Debug.LogError(toSend);
                Debug.LogException(exception);
                if (Plugin.Patch_Guardian)
                {
                    Throw_Exception_At_Vigaro(new Exception(toSend, exception));
                }
            }
        }
        catch (Exception e)
        {
            L(exception, message, logPrio, callerName);
            L(e, "Logging player fail!", logPrio: 4);
        }
    }

    private static void Throw_Exception_At_Vigaro(Exception exception)
    {
        UploadException(exception);
    }

    public record Difficulty
    {
        public const int TRYHARD = 1984;
        public const int ELITIST = 420;
        public const int MADLAD = 666;
        public const int CUSTOM = -1;
    }

    public class EliteSave : DeathPersistentSaveData.Tutorial
    {
        public static readonly EliteSave DunkKarma = new("Elitist_Throw_Karma_Away");

        public EliteSave(string value, bool register = true) : base(value, register)
        {
        }
    }

    public static string Swapper(this string text, params string[] with)
    {
        text = RWCustom.Custom.rainWorld.inGameTranslator.Translate(text).Replace("<LINE>", System.Environment.NewLine);
        for (int i = 0; i < with.Length; i++)
        {
            text = text.Replace($"<REPLACE{i}>", RWCustom.Custom.rainWorld.inGameTranslator.Translate(with[i]));
        }
        return text;
    }
}
