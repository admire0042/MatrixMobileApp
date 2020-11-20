using MatrixXamarinApp.Models;
using MatrixXamarinApp.RestAPIClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MatrixXamarinApp.ServicesHandler
{
    public class RegisterService
    {
        // fetch the RestClient<T>
        RestClient<Register> _restClient = new RestClient<Register>();

        // Boolean function with the following parameters of username & password.
        public async Task<bool> CheckRegisterIfExists(string url, string username, string password)
        {
            var check = await _restClient.checkRegister(url, username, password);

            return check;
        }
    }
}
