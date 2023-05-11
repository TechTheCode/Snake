using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public interface ICommand
{
    void Execute();
    void Undo();
}