using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class CommandInvoker : MonoBehaviour 
{
    Stack<ICommand> commandHistory = new Stack<ICommand>();

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandHistory.Push(command);
    }

    public void Undo()
    {
        if (commandHistory.Count > 0)
        {
            ICommand lastCommand = commandHistory.Pop();
            lastCommand.Undo();
        }
    }
}