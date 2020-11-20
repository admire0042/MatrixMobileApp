using MatrixXamarinApp.Models;
using MatrixXamarinApp.RestAPIClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MatrixXamarinApp.ServicesHandler
{
    public class Message
    {
        // fetch the RestClient<T>
        RestClient<Inbox> _restClient = new RestClient<Inbox>();
        RestClient<Outbox> _outMailClient = new RestClient<Outbox>();
        RestClient<viewsMessageL> _viewMailClient = new RestClient<viewsMessageL>();
        RestClient<viewsMessageL2> _viewMailClient2 = new RestClient<viewsMessageL2>();
        RestClient<GetFullMessages> _viewGetMessages = new RestClient<GetFullMessages>();
        RestClient<GetFullMessages2> _viewGetMessages2 = new RestClient<GetFullMessages2>();

        // Boolean function with the following parameters of username & password.
        public async Task<bool> CheckMessageIfExists(string num)
        {
            var check = await _restClient.inbox(num);

            return check;
        }

        public async Task<bool> CheckOutMailMessageIfExists(string num)
        {
            var check = await _outMailClient.outbox(num);

            return check;
        }

        public async Task<bool> CheckOutViewMessageIfExists(string dir, string cond)
        {
            var check = await _viewMailClient.ViewsMessages(dir, cond);

            return check;
        }

        public async Task<bool> CheckOutViewMessageIfExists2(string dir, string cond)
        {
            var check = await _viewMailClient2.ViewsMessages2(dir, cond);

            return check;
        }

        public async Task<bool> CheckGetviewsIfExists(string messageID, string messageDirection)
        {
            var check = await _viewGetMessages.GetMessages(messageID,messageDirection);
            return check;
        }

        public async Task<bool> CheckGetviewsIfExists2(string messageID, string messageDirection)
        {
            var check = await _viewGetMessages2.GetMessages2(messageID, messageDirection);
            return check;
        }

        public async Task<bool> CheckGetFullviewsIfExists()
        {
            var check = await _viewGetMessages.GetFullMessages();
            return check;
        }

        public async Task<bool> CheckGetFullviewsIfExists2()
        {
            var check = await _viewGetMessages2.GetFullMessages2();
            return check;
        }
    }
}
