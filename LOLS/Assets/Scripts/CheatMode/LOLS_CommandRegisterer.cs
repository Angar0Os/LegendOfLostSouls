using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UnityEngine;

public class LOLS_CommandRegisterer : MonoBehaviour
{
    public List<LOLS_ICommand> Commands = new List<LOLS_ICommand>();

    public void RegisterCommand(LOLS_ICommand command)
    { Commands.Add(command); }

    public bool Parse(string s)
    {
        s = s.ToLower();

        if (!s.StartsWith("/"))
            return false;

        s = s.Substring(1);
        var tokens = s.Split(' ');

        foreach (var c in Commands)
            if (c.IsTagValid(tokens[0]))
            {
                c.Execute(tokens.Skip(1).ToArray());
                return true;
            }

        return false;
    }
}
