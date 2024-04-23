using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;

namespace AaristaAcademyPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateAzureADAcount(RegistrationModel model)
        {
            // Define your Azure AD credentials
            string clientId = "aedf69b3-1b4f-4c01-a4be-06ab419cfe58";
            string clientSecret = ".DI8Q~k_m0EqrBEgrp5ibH_GpH-vCTiJkuRgrdkX";
            string tenantId = "9b0bd24b-f063-4739-aa57-59e116bd92dd";

            // using Azure.Identity;
            var options = new ClientSecretCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            };

            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);

            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);


            // Create a new user object
            var newUser = new User
            {
                AccountEnabled = true,
                DisplayName = model.DisplayName,
                MailNickname = model.AccountName,
                UserPrincipalName = $"{model.AccountName}@hoangvo1311gmail.onmicrosoft.com",
                
                PasswordProfile = new PasswordProfile
                {
                    Password = "AaristaAcademy123!", // Set the initial password
                    ForceChangePasswordNextSignIn = true // Force user to change password on next sign-in
                }
            };

            try
            {
                // Create the user
                var user = await graphClient.Users.PostAsync(newUser);

                return Created("", user!.UserPrincipalName);
            }
            catch (Exception ex)
            {
                if (ex is Microsoft.Graph.Models.ODataErrors.ODataError error)
                {
                    return StatusCode(error.ResponseStatusCode);
                }
                return StatusCode(500, ex.Message);
            }
        }
    }
}
