using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface LOLS_ICommand
{
    bool IsTagValid(string s);
    void Execute(string[] args);
}
