using System.Data;

namespace Banks.Banks;

public interface ISubscriber
{
    Guid Id { get; }
    void Update(IObservable observable);
}