using System.Collections.Generic;

public class CommandsHandler
{
    private List<Command> _commands;
    private Character _character;
    private bool _next;
    private readonly IBattle _battle;

    public int Count => _commands.Count;
    public bool Next => _next;
    
    public CommandsHandler(IBattle battle) {
        _battle = battle;
    }

    public void Initialize(Character character) {
        _character = character;
        _commands = new List<Command>();
    }

    public void Add(Command command) {
        command.OnCompleted = () => Complete(command);
        _commands.Add(command);
    }
    
    public void AddNext(Command command) {
        command.OnCompleted = () => Complete(command);
        _commands.Insert(1, command);
    }
    
    public void Execute() {
        _next = false;
        _commands[0].Execute(_battle, _character);
    }
    
    public void Clear() {
        _commands.Clear();
        _next = true;
    }

    private void Complete(Command command) {
        _commands.Remove(command);
        _next = true;
    }
}