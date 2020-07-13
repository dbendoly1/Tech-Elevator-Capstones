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
    [Route("/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserDAO userDAO;

        public UserController(IUserDAO _userDAO)
        {
            userDAO = _userDAO;
        }
        [HttpGet("userlist")]
        public List<string> GetUserList()
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
            List<User> userlist = userDAO.GetUsers();
            List<string> result = new List<string>();
            foreach (User user in userlist)
            {
                if (user.UserId != id)
                { 
                string userString = user.UserId + "\t\t" + user.Username;
                result.Add(userString);
                }

            }
            return result;
        }

        [HttpGet("allusers")]
        public List<string> ListAllUsers()
        {
            
            List<User> userlist = userDAO.GetUsers();
            List<string> result = new List<string>();
            foreach (User user in userlist)
            {
                    string userString = user.UserId + "\t\t" + user.Username;
                    result.Add(userString);
            }
            return result;
        }

    }
}



    