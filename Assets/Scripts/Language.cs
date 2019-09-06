using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Verb
{
    Default,Move,Scatter,Get,Put,What,Sing
}
public enum Noun
{
    Default,Me,You,This
}
public struct Command
{
    public Verb verb;
    public Noun noun;
    public SoundMaker speaker;
    public string custom;
    public Transform subject;
    public string plain;
    public Command(SoundMaker sp, Verb v = Verb.Default, Noun n = Noun.Default, string custom = "", Transform subject = null, string plain = "")
    {
        this.speaker = sp;
        this.verb = v;
        this.noun = n;
        this.subject = subject;
        this.custom = custom;
        this.plain = plain;
    }
}
public static class Language
{
    static Dictionary<string, Verb> verbs = new Dictionary<string, Verb>()
    {
        {"wa",Verb.Move },
        {"ws",Verb.Scatter },
        {"da",Verb.Get},
        {"ds",Verb.Put },
        {"wd", Verb.What },
        {"aa", Verb.Sing }
    };
    static Dictionary<string, Noun> nouns = new Dictionary<string, Noun>()
    {
        {"a",Noun.Me },
        {"s", Noun.You},
        {"w",Noun.This}
    };
    public static void TakeMessage(string msg,SoundMaker talker, Transform subject = null)
    {
        //deal with simon
        if (talker.simonSayer)
        {
            if(Time.time > talker.timeWhenSimonDies)
            {
                talker.simonSayer = false;
                talker.simonSayee.simon = false;
                talker.simonSayee = null;
            }
            else
            {
                talker.timeWhenSimonDies = Time.time + talker.simonTimeCheck;
            }
        }
        //end
        Command cmd = new Command(talker);
        string[] message = msg.Split(' ');
        bool hasVerb = false;
        bool hasNoun = false;
        foreach(string word in message)
        {
            //this ain't a verb or a noun...
            if (word.Length == 3 || word.Length == 5 || talker.simonSayer)
            {
                cmd.custom = word;
                continue;
            }
            if (hasVerb && hasNoun)
            {
                break;
            }
            if (verbs.ContainsKey(word))//its a verb!!
            {
                hasVerb = true;
                cmd.verb = verbs[word];
                continue;
            }
            if (nouns.ContainsKey(word))//its a noun!!
            {
                hasNoun = true;
                cmd.noun = nouns[word];
                continue;
            }
        }
        if (subject)
        {
            cmd.subject = subject;
            Debug.Log("su");
        }
        cmd.plain = msg;
        if(hasVerb && hasNoun)//congrats you have a whole command!
        {
            talker.MakeSound(cmd);
        }
        else//uh oh not a whole command
        {
            /*if (hasVerb)
            {
                talker.MakeSound(cmd);
            }else if (talker.simonSayer)//you're playing simon says
            {
                talker.MakeSound(cmd);
            }*/
            talker.MakeSound(cmd);
        }

    }
    public static string ShowVerbs()
    {
        string outString = "";
        foreach (KeyValuePair<string, Verb> kvp in verbs)
        {
            outString += kvp.Value + " - " + kvp.Key.ToString().ToUpper() + "\n";
        }
        return outString;
    }
    public static string ShowNouns()
    {
        string outString = "";
        foreach (KeyValuePair<string, Noun> kvp in nouns)
        {
            outString += kvp.Value + " - " + kvp.Key.ToString().ToUpper() + "\n";
        }
        return outString;
    }
}
