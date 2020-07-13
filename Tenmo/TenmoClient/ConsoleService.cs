using System;
using System.Collections.Generic;
using TenmoClient.Data;

namespace TenmoClient
{
    public class ConsoleService
    {
        private static readonly AuthService authService = new AuthService();
        private static API_User apiUser = new API_User();


        public void Run()
        {

            while (true)
            {
                Console.WriteLine("Welcome to TEnmo!");
                Console.WriteLine("1: Login");
                Console.WriteLine("2: Register");
                Console.WriteLine("3: Exit");
                Console.Write("Please choose an option: ");

                int loginRegister = -1;

                try
                {
                    if (!int.TryParse(Console.ReadLine(), out loginRegister))
                    {
                        Console.WriteLine("Invalid input. Please enter only a number.");
                    }

                    else if (loginRegister == 1)
                    {
                        LoginUser loginUser = PromptForLogin();
                        API_User user = authService.Login(loginUser);
                        if (user != null)
                        {
                            UserService.SetLogin(user);
                            MenuSelection();
                        }
                    }

                    else if (loginRegister == 2)
                    {
                        LoginUser registerUser = PromptForLogin();
                        bool isRegistered = authService.Register(registerUser);
                        if (isRegistered)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Registration successful. You can now log in.");
                        }
                    }

                    else if (loginRegister == 3)
                    {
                        Console.WriteLine("Goodbye!");
                        Environment.Exit(0);
                    }

                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error - " + ex.Message);
                }
            }
        }

        private void MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers"); //view details through here
                Console.WriteLine("3: View your pending requests"); //ability to approve/reject through here
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: View list of users");
                Console.WriteLine("7: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                try
                {
                    if (!int.TryParse(Console.ReadLine(), out menuSelection))
                    {
                        Console.WriteLine("Invalid input. Please enter only a number.");
                    }

                    else if (menuSelection == 1)
                    {
                        decimal balance = authService.GetBalance();
                        Console.WriteLine("Your current account balance is: " + balance);
                    }

                    else if (menuSelection == 2)
                    {
                        ViewPastTransfers();
                    }

                    else if (menuSelection == 3)
                    {
                        ViewPendingRequests();
                    }

                    else if (menuSelection == 4)
                    {
                        SendTEBucks();
                    }

                    else if (menuSelection == 5)  //****REQUEST TE BUCKS
                    {
                        RequestTEBucks();
                    }

                    else if (menuSelection == 6)
                    {
                        List<string> userList = authService.AllUsers();

                        Console.WriteLine("-------------------------------------------");
                        Console.WriteLine("Users");
                        Console.WriteLine("ID\t\tName");
                        Console.WriteLine("-------------------------------------------");
                        
                        foreach (string user in userList)
                        {
                            Console.WriteLine(user);
                        }
                    }
                    else if (menuSelection == 7)
                    {
                        Console.WriteLine("");
                        UserService.SetLogin(new API_User()); //wipe out previous login info
                        return; //return to register/login menu
                    }
                    else if (menuSelection == 0)
                    {
                        Console.WriteLine("Goodbye!");
                        Environment.Exit(0);
                    }

                    else
                    {
                        Console.WriteLine("Please try again");
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error - " + ex.Message);
                    Console.WriteLine();
                }
            }
        }

        public int PromptForTransferID(string action)
        {
            Console.WriteLine("");
            Console.Write("Please enter transfer ID to " + action + " (0 to cancel): ");
            if (!int.TryParse(Console.ReadLine(), out int auctionId))
            {
                Console.WriteLine("Invalid input. Only input a number.");
                return 0;
            }
            else
            {
                return auctionId;
            }
        }

        public LoginUser PromptForLogin()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            string password = GetPasswordFromConsole("Password: ");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }

        private string GetPasswordFromConsole(string displayMessage)
        {
            string pass = "";
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Remove(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");
            return pass;
        }

        //MENU SELECTION METHODS

        public void ViewPastTransfers()
        {
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Transfers");
            Console.WriteLine("ID\t\tName\t\t\tAmount");
            Console.WriteLine("-------------------------------------------");

            List<string> list = authService.ListTransactionHistory();
            foreach (string s in list)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("---------------");
            Console.Write("Please enter transfer ID to view details (0 to cancel):  ");

            if (!int.TryParse(Console.ReadLine(), out int TransferSelection))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            else if (TransferSelection == 0) { MenuSelection(); }

            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Transfer Details");
            Console.WriteLine("-------------------------------------------");

            List<string> details = authService.GetTransferDetails(TransferSelection);

            if (details.Count == 0) { Console.WriteLine("No transfer history to display."); }

            foreach (string str in details)
            {
                Console.WriteLine(str);
            }
        }

        public void ViewPendingRequests()
        { 
        Console.WriteLine("-------------------------------------------");
                        Console.WriteLine("Pending Requests");
                        Console.WriteLine("ID\t\tName\t\t\tAmount");
                        Console.WriteLine("-------------------------------------------");

                        List<string> list = authService.ListPendingTransactions();
                        foreach (string s in list)
                        {
                            Console.WriteLine(s);
                        }

    Console.WriteLine("---------------");
                        Console.Write("Please enter Request ID to approve/reject (0 to cancel):  ");

                        if (!int.TryParse(Console.ReadLine(), out int TransferSelection))
                        {
                            Console.WriteLine("Invalid input. Please enter only a number.");
                        }
                        else if (TransferSelection == 0) { MenuSelection(); }

                        //approve or reject pending transfer
                        Console.WriteLine("1: Approve");
                        Console.WriteLine("2: Reject");
                        Console.WriteLine("0: Don't approve or reject");
                        Console.WriteLine("---------------");
                        Console.Write("Please choose an option: ");
                        if (!int.TryParse(Console.ReadLine(), out int approveOrReject))
                        {
                            Console.WriteLine("Invalid input. Please enter only a number.");
                        }
                        switch(approveOrReject)
                        {
                            case 1:
                                try
                                {
                                    authService.ApproveRequest(TransferSelection);
                                    Console.WriteLine("Your transfer was completed successfully");
                                }
                                catch
                                {
                                    Console.WriteLine("Your transfer did not complete");
                                }
                                break;
                            case 2:
                                try
                                {
                                    authService.RejectRequest(TransferSelection);
                                    Console.WriteLine("Your transfer was canceled successfully");
                                }
                                catch
                                {
                                    Console.WriteLine("Your cancel request did not complete");
                                }
                                break;
                            case 0:
                                {}
                                break;
                            default:
                                Console.WriteLine("Invalid input");
                                break;
                        }
    }

        public void SendTEBucks()
        {
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Users");
            Console.WriteLine("ID\t\tName");
            Console.WriteLine("-------------------------------------------");

            //pull user list and cw each one. 
            List<string> userList = authService.ListUsers();
            foreach (string user in userList)
            {
                Console.WriteLine(user);
            }

            Console.WriteLine("---------------");
            Console.Write("Enter ID of user you are sending to (0 to cancel):  ");

            if (!int.TryParse(Console.ReadLine(), out int userSelection))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            else if (userSelection == 0) { MenuSelection(); }
            
            Console.Write("Enter amount:  ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal enteredAmount))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            //Transfer fundz
            bool transferResult = authService.TransferFunds(userSelection, enteredAmount);
            if (transferResult)
            {
                Console.WriteLine("Transfer Successful");
            }
            else
            {
                Console.WriteLine("Transfer Failed");
            }
        }

        public void RequestTEBucks()
        {
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Users");
            Console.WriteLine("ID\t\tName");
            Console.WriteLine("-------------------------------------------");

            List<string> userList = authService.ListUsers();
            foreach (string user in userList)
            {
                Console.WriteLine(user);
            }

            Console.WriteLine("---------------");

            Console.Write("Enter ID of user you are requesting from (0 to cancel):  ");
            if (!int.TryParse(Console.ReadLine(), out int userSelection))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            else if (userSelection == 0) { MenuSelection(); }

            Console.Write("Enter amount:  ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal enteredAmount))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }

            bool requestResult = authService.RequestFunds(userSelection, enteredAmount);
            if (requestResult)
            {
                Console.WriteLine("Request Sent");
            }
            else
            {
                Console.WriteLine("Request Did Not Send");
            }
        }
    }
}
