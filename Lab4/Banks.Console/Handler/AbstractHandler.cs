using System.ComponentModel.Design;

namespace Banks.Console.Handler;

public abstract class AbstractHandler
{
    public AbstractHandler? NextHendler { get; set; }

    public AbstractHandler SetNext(AbstractHandler handler)
    {
        NextHendler = handler;
        return handler;
    }

    public abstract void Handle(string request);
}