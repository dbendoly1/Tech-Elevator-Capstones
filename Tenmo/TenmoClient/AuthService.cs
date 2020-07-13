using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using TenmoClient.Data;

namespace TenmoClient
{
    public class AuthService
    {
        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private static readonly IRestClient client = new RestClient();

        //login endpoints
        public bool Register(LoginUser registerUser)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "login/register");
            request.AddJsonBody(registerUser);
            IRestResponse<API_User> response = client.Post<API_User>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("An error occurred communicating with the server.");
            }
            else if (!response.IsSuccessful)
            {
                if (!string.IsNullOrWhiteSpace(response.Data.Message))
                {
                    throw new Exception("An error message was received: " + response.Data.Message);
                }
                else
                {
                    throw new Exception("An error response was received from the server. The status code is " + (int)response.StatusCode);
                }
            }
            else
            {
                return true;
            }
        }

        public API_User Login(LoginUser loginUser)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "login");
            request.AddJsonBody(loginUser);
            IRestResponse<API_User> response = client.Post<API_User>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("An error occurred communicating with the server.");

            }
            else if (!response.IsSuccessful)
            {
                if (!string.IsNullOrWhiteSpace(response.Data.Message))
                {
                    throw new Exception("An error message was received: " + response.Data.Message);
                }
                else
                {
                    throw new Exception("An error response was received from the server. The status code is " + (int)response.StatusCode);
                }

            }
            else
            {
                client.Authenticator = new JwtAuthenticator(response.Data.Token);
                return response.Data;
            }
        }

        public decimal GetBalance()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "balance");

            IRestResponse<decimal> response = client.Get<decimal>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("An error occurred communicating with the server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("An error response was received from the server. The status code is " + (int)response.StatusCode);

            }
            else
            {
                return response.Data;
            }
        }

        public List<string> ListUsers()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "userlist");

            IRestResponse<List<string>> response = client.Get<List<string>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("An error occurred communicating with the server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("An error response was received from the server. The status code is " + (int)response.StatusCode);

            }
            else
            {
                return response.Data;
            }
        }

        public bool TransferFunds(int userId, decimal amount)
        {
            API_Transfer transfer = new API_Transfer();
            transfer.TransferTo = userId;
            transfer.Amount = amount;
            RestRequest request = new RestRequest(API_BASE_URL + "transfer/transferfunds");
            request.AddJsonBody(transfer);
            IRestResponse<bool> response = client.Post<bool>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("An error occurred communicating with the server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("An error response was received from the server. The status code is " + (int)response.StatusCode);

            }
            else
            {
                return response.Data;
            }

        }

        public List<string> ListTransactionHistory()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "transfer/userhistory");
            IRestResponse<List<string>> response = client.Get<List<string>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("An error occurred communicating with the server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("An error response was received from the server. The status code is " + (int)response.StatusCode);

            }
            else
            {
                return response.Data;
            }
        }

        public bool RequestFunds(int userId, decimal amount)
        {
            API_Transfer transfer = new API_Transfer();
            transfer.TransferTo = userId;
            transfer.Amount = amount;
            RestRequest request = new RestRequest(API_BASE_URL + "transfer/requestfunds");
            request.AddJsonBody(transfer);
            IRestResponse<bool> response = client.Post<bool>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("An error occurred communicating with the server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("An error response was received from the server. The status code is " + (int)response.StatusCode);

            }
            else
            {
                return response.Data;
            }

        }

        public List<string> AllUsers()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "allusers");

            IRestResponse<List<string>> response = client.Get<List<string>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("An error occurred communicating with the server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("An error response was received from the server. The status code is " + (int)response.StatusCode);

            }
            else
            {
                return response.Data;
            }
        }

        public List<string> GetTransferDetails(int transfer_id)
        {
            API_TransferDetails transfer = new API_TransferDetails();
            transfer.TransferId = transfer_id;
            RestRequest request = new RestRequest(API_BASE_URL + $"transfer/{transfer_id}");
            
            IRestResponse<List<string>> response = client.Get<List<string>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("An error occurred communicating with the server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("An error response was received from the server. The status code is " + (int)response.StatusCode);

            }
            else
            {
                return response.Data;
            }
        }

        public List<string> ListPendingTransactions()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "transfer/pending");
            IRestResponse<List<string>> response = client.Get<List<string>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("An error occurred communicating with the server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("An error response was received from the server. The status code is " + (int)response.StatusCode);

            }
            else
            {
                return response.Data;
            }
        }

        public bool ApproveRequest(int transfer_id)
        {
            //API_TransferDetails transfer = new API_TransferDetails();
            //transfer.TransferId = transfer_id;
            
            RestRequest request = new RestRequest(API_BASE_URL + $"transfer/approve{transfer_id}");
            //request.AddJsonBody(transfer);
            IRestResponse<bool> response = client.Put<bool>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("An error occurred communicating with the server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("An error response was received from the server. The status code is " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }

        }
        public bool RejectRequest(int transfer_id)
        {

            RestRequest request = new RestRequest(API_BASE_URL + $"transfer/reject{transfer_id}");
            IRestResponse<bool> response = client.Put<bool>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("An error occurred communicating with the server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("An error response was received from the server. The status code is " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }

        }
    }
}
