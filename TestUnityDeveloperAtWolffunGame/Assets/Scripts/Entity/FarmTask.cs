public class FarmTask
{
    public string Description;
    public System.Action Execute;

    public FarmTask(string desc, System.Action action)
    {
        Description = desc;
        Execute = action;
    }
}
