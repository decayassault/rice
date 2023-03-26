using System.Net;
using System;
namespace Logic
{
    public interface IAuthenticationLogic
    {
        bool AccessGranted(in string token);
        Pair GetPair(in string token);
        string Accept(in IPAddress ip, in Pair pair);
        Tuple<bool, int> AccessGrantedEntended(in string token);
    }
}