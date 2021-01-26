using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Inferastructure.IRepository
{
    public interface ICustomAuthicationManager
    {
        string Authenicate(string username, string password);

        IDictionary<string, string> Tokens;
    }
}
