using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;

namespace TenmoServer.Controllers
{
    [Authorize]
    [Route("/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDao accountDAO;

        public AccountController( IAccountDao _accountDAO)
        {
            accountDAO = _accountDAO;
        }

        [HttpGet("balance")]
        public decimal GetBalance()
        {
            int id = -1;

            foreach (var claim in User.Claims)
            {
                if (claim.Type == "sub")
                {
                    id = int.Parse(claim.Value);
                    break;
                }
            }

            decimal userAccountBal = accountDAO.GetAccountBalance(id);
            return userAccountBal;
        }
    }
}