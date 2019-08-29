using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Verb
{
    Default,Move,Scatter,Get,Put
}
public enum Noun
{
    Default,Me,You
}
public struct Command
{
    public Verb verb;
    public Noun noun;
    public Transform speaker;
    public Command(Transform sp, Verb v, Noun n)
    {
        this.speaker = sp;
        this.verb = v;
        this.noun = n;
    }
}
public static class Language
{
    static Dictionary<string, Verb> verbs = new Dictionary<string, Verb>()
    {
        {"wa",Verb.Move },
        {"ws",Verb.Scatter },
        {"da",Verb.Get},
        {"ds",Verb.Put }
    };
    static Dictionary<string, Noun> nouns = new Dictionary<string, Noun>()
    {
        {"a",Noun.Me },
        {"s", Noun.You}
    };
    public static void TakeMessage(string msg,SoundMaker talker)
    {
        Command cmd = new Command(talker.transform, Verb.Default,Noun.Default);
        string[] message = msg.Split(' ');
        bool hasVerb = false;
        bool hasNoun = false;
        foreach(string word in message)
        {
            if(hasVerb && hasNoun)
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
        if(hasVerb && hasNoun)//congrats you have a whole command!
        {
            talker.MakeSound(cmd);
        }
        else//uh oh not a whole command
        {
            if (hasVerb)
            {
                //assume that the noun is me
                cmd.noun = Noun.Me;
                talker.MakeSound(cmd);
            }
        }

    }
}
