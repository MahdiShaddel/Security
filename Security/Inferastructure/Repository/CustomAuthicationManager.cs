using Security.Inferastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Inferastructure.Repository
{
    public class CustomAuthicationManager : ICustomAuthicationManager
    {
        private readonly IDictionary<string, string> users = new Dictionary<string, string>
        {{"test1","password1"},{"test2","password2"}};

        private readonly IDictionary<string, string> tokens = new Dictionary<string, string>();

        public readonly IDictionary<string, string> Tokens => tokens;

        public string Authenicate(string username, string password)
        {
            if (!users.Any(u => u.Key == username && u.Value == password))
            {
                return null;
            }
            var token = Guid.NewGuid().ToString();

            tokens.Add(token, password);

            return token;
        }
    }
}
