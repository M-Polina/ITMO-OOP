namespace Banks.Client;

public interface IBuilder
{
    void SetAddress(string address);

    void SetPassport(int id);

    ClientAccount CreateAndGetClient();
}