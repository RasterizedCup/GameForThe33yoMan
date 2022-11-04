using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AccountGrabber : MonoBehaviour
{
    private static int MaxLengthUsername = 25;
    private static int MinLengthUsername = 4;
    public async Task RegisterAccount(Account newAccount)
    {
        if (await checkProfanity(newAccount.PlayerName))
        {
            NameChangeButton.nameState = "No gamer words in the username please";
            return;
        }
        if(newAccount.PlayerName.Length > MaxLengthUsername)
        {
            NameChangeButton.nameState = $"Username too long. It'll break my UI. Please choose something {MaxLengthUsername} characters or less.";
            return;
        }

        if (newAccount.PlayerName.Length <= MinLengthUsername)
        {
            NameChangeButton.nameState = $"Username too short. Expand him beyond {MinLengthUsername} characters pls.";
            return;
        }

        newAccount.EncrpytedPassword = Sha256Encrypter(newAccount.EncrpytedPassword);
        await WebRequests.PostJson("https://filgame-stats.azurewebsites.net/api/RegisterAccount?code=xB9qf6eRHRxaa00GSA48bEAsb1X04BBxZP1DtiBZ9_jcAzFuQQQQBA==",
               JsonConvert.SerializeObject(newAccount),
               (string error) =>
               {
                   Debug.Log("error:" + error);
               },
               (string resp) =>
               {
                   Debug.Log("Response:" + resp);
                   NameChangeButton.nameState = resp;
               }
          );
    }

    public async Task ValidateLogin(Account account)
    {
        account.EncrpytedPassword = Sha256Encrypter(account.EncrpytedPassword);
        await WebRequests.PostJson("https://filgame-stats.azurewebsites.net/api/ValidateLogin?code=ytToh8c-7qGpN89GePAVoENxQU_6PhD35SoTgMT6MaXyAzFu5XFuzw==",
               JsonConvert.SerializeObject(account),
               (string error) =>
               {
                   Debug.Log("error:" + error);
               },
               (string resp) =>
               {
                   Debug.Log("Response:" + resp);
                   NameChangeButton.nameState = resp;
               }
          );
    }

    string Sha256Encrypter(string password)
    {
        Debug.Log($"pre: {password}");
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convert byte array to a string   
            StringBuilder encryptedPass = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                encryptedPass.Append(bytes[i].ToString("x2"));
            }
            Debug.Log($"post: {encryptedPass.ToString()}");
            return encryptedPass.ToString();
        }
    }

    async Task<bool> checkProfanity(string username)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://community-purgomalum.p.rapidapi.com/containsprofanity?text=" + username),
            Headers =
            {
                { "X-RapidAPI-Key", "d8f83c3d34msh535cf1bf159f664p1d3d3bjsn101818aa0106" },
                { "X-RapidAPI-Host", "community-purgomalum.p.rapidapi.com" },
            },
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var respString = await response.Content.ReadAsStringAsync();
            Debug.Log(respString);
            var body = JsonConvert.DeserializeObject<bool>(respString);
            Debug.Log(body);
            return body;
        }
    }
}
