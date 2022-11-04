using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Account
{
    public string PlayerName { get; set; }
    public string EncrpytedPassword { get; set; }
}

public class AccountList
{
    public List<Account> accounts;
}