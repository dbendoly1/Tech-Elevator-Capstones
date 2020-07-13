using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Authorize]
    [Route("transfer")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferDao transferDAO;
        private readonly IAccountDao accountDAO;
        private readonly IUserDAO userDAO;

        private int UserIdFromToken()
        {

            int user_id = -1;
            foreach (var claim in User.Claims)
            {

                if (claim.Type == "sub")
                {
                    user_id = int.Parse(claim.Value);
                    break;
                }

            }
            return user_id;

        }


        public TransferController(ITransferDao _transferDAO, IAccountDao _accountDAO, IUserDAO _userDAO)
        {
            transferDAO = _transferDAO;
            accountDAO = _accountDAO;
            userDAO = _userDAO;
        }

        [HttpGet("userhistory")]
        public ActionResult<List<string>> GetUserTransHistory()
        {
            List<string> result = new List<string>();
            List<Transfer> fullList = transferDAO.GetFullTransList();
            //Get UserID
            int user_id = UserIdFromToken();
            //GET AccountID
            int account_id = accountDAO.GetAccountId(user_id);
            //loop the List of Transfers
            foreach (Transfer t in fullList)
            {
                if (t.AccountFrom == account_id)
                {
                    int otherUser_id = accountDAO.GetUserId(t.TransferTo);
                    string username = userDAO.GetUserName(otherUser_id);
                    string s = t.TransferId + "\t\tTo: " + username + "\t\t$" + t.Amount;

                    result.Add(s);
                }
                else if (t.TransferTo == account_id)
                {
                    int otherUser_id = accountDAO.GetUserId(t.AccountFrom);
                    string username = userDAO.GetUserName(otherUser_id);
                    string s = t.TransferId + "\t\tFrom: " + username + "\t\t$" + t.Amount;

                    result.Add(s);
                }
            }
            return result;
        }

        [HttpGet("{transfer_id}")]
        public ActionResult<List<string>> GetTransferDetails(int transfer_id)
        {
            Transfer t = transferDAO.GetSpecificTransfer(transfer_id);

            List<string> transDetails = new List<string>();
            string id = "Id: " + t.TransferId;
            transDetails.Add(id);

            int authid = -1;
            foreach (var claim in User.Claims)
            {
                if (claim.Type == "sub")
                {
                    authid = int.Parse(claim.Value);
                    break;
                }
            }

            if (t.AccountFrom == authid)
            {
                string from = "From: Me Myselfandi";
                transDetails.Add(from);
            }
            else
            {
                int fromUserId = accountDAO.GetUserId(t.AccountFrom);
                string fromUsername = userDAO.GetUserName(fromUserId);
                string from = "From: " + fromUsername;
                transDetails.Add(from);
            }

            if (t.TransferTo == authid)
            {
                string to = "To: Me Myselfandi";
                transDetails.Add(to);
            }
            else
            {
                int toUserId = accountDAO.GetUserId(t.TransferTo);
                string toUsername = userDAO.GetUserName(toUserId);
                string to = "To: " + toUsername;
                transDetails.Add(to);
            }

            if (t.TypeId == 1)
            {
                string type = "Type: Request";
                transDetails.Add(type);
            }
            else if (t.TypeId == 2)
            {
                string type = "Type: Send";
                transDetails.Add(type);
            }

            if (t.StatusId == 1)
            {
                string status = "Status: Pending";
                transDetails.Add(status);
            }
            else if (t.StatusId == 2)
            {
                string status = "Status: Approved";
                transDetails.Add(status);
            }
            else if (t.StatusId == 3)
            {
                string status = "Status: Rejected";
                transDetails.Add(status);
            }

            string amount = "Amount: $" + t.Amount;
            transDetails.Add(amount);

            return transDetails;
        }


        [HttpPost("transferfunds")]
        public ActionResult<bool> SendTransfer(Transfer transfer)
        {
            bool successful = false;

            int transferFrom = -1;

            foreach (var claim in User.Claims)
            {
                if (claim.Type == "sub")
                {
                    transferFrom = int.Parse(claim.Value);
                    break;
                }
            }
            //get sending balance          
            decimal sendingAccount = accountDAO.GetAccountBalance(transferFrom);
            if (sendingAccount >= transfer.Amount)
            {
                try
                {
                    accountDAO.MoveFundsFrom(transferFrom, transfer.Amount);
                    accountDAO.MoveFundsTo(transfer.TransferTo, transfer.Amount);
                    transferDAO.AddSendTransfer(transfer.TransferTo, transferFrom, transfer.Amount);
                    successful = true;
                }
                catch (Exception ex)
                {
                    return successful;
                }
            }
            return successful;
        }

        [HttpPost("requestfunds")]
        public ActionResult<bool> RequestTransfer(Transfer transfer)
        {
            bool successful = false;

            int transferTo = -1;

            foreach (var claim in User.Claims)
            {
                if (claim.Type == "sub")
                {
                    transferTo = int.Parse(claim.Value);
                    break;
                }
            }
            //get sending balance        
            try
            {
                transferDAO.AddRequestTransfer(transferTo, transfer.TransferTo, transfer.Amount);
                successful = true;
            }
            catch (Exception ex)
            {
                return successful;
            }
            return successful;
        }

        [HttpGet("pending")]
        public ActionResult<List<string>> GetUserPendingTrans()
        {
            List<string> result = new List<string>();
            List<Transfer> PendingList = transferDAO.GetFullTransList();
            //Get UserID
            int user_id = UserIdFromToken();
            //GET AccountID
            int account_id = accountDAO.GetAccountId(user_id);
            //loop the List of Transfers
            foreach (Transfer t in PendingList)
            {
                if (t.AccountFrom == account_id && t.StatusId == 1)
                {
                    int otherUser_id = accountDAO.GetUserId(t.TransferTo);
                    string username = userDAO.GetUserName(otherUser_id);
                    string s = t.TransferId + "\t\tTo: " + username + "\t\t$" + t.Amount;

                    result.Add(s);
                }
                else if (t.TransferTo == account_id && t.StatusId == 1)
                {
                    int otherUser_id = accountDAO.GetUserId(t.AccountFrom);
                    string username = userDAO.GetUserName(otherUser_id);
                    string s = t.TransferId + "\t\tFrom: " + username + "\t\t$" + t.Amount;

                    result.Add(s);
                }
            }
            return result;
        }


        [HttpPut("approve{transfer_id}")]
        public ActionResult<bool> ApproveTransfer(int transfer_id)
        {
            Transfer transfer = transferDAO.GetSpecificTransfer(transfer_id);
            bool successful = false;

            int transferFrom = -1;
            
            foreach (var claim in User.Claims)
            {
                if (claim.Type == "sub")
                {
                    transferFrom = int.Parse(claim.Value);
                    break;
                }
            }
            //get sending balance          
            decimal sendingAccount = accountDAO.GetAccountBalance(transferFrom);
            if (sendingAccount >= transfer.Amount)
            {
                try
                {
                    accountDAO.MoveFundsFrom(transferFrom, transfer.Amount);
                    accountDAO.MoveFundsTo(transfer.TransferTo, transfer.Amount);
                    transferDAO.ApproveRequest(transfer_id);
                    successful = true;
                }
                catch (Exception ex)
                {
                    return successful;
                }
            }
            return successful;
        }

        [HttpPut("reject{transfer_id}")]
        public ActionResult<bool> RejectTransfer(int transfer_id)
        {
            bool successful = false;
            
                try
                {
                    transferDAO.DenyRequest(transfer_id);
                    successful = true;
                }
                catch (Exception ex)
                {
                    return successful;
                }
            
            return successful;
        }
    }
}
