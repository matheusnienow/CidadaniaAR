namespace Command
{
    public interface ICommand<out T>
    {
        T Execute();
        void Undo();
    }
}