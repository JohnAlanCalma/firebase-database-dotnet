﻿using System.Threading.Tasks;
using Firebase.Auth.REST;

namespace Firebase.Auth.Tests
{
    using System;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public class IntegrationTests
    {
        private const string ApiKey = "AIzaSyCsyUX5TmSFK_8JZU35VZiGfY_0CHLJeBc";

        private const string FacebookAccessToken = "<FACEBOOK USER ACCESS TOKEN>";
        private const string FacebookTestUserFirstName = "Mark";

        private const string GoogleAccessToken = "<GOOGLE USER ACCESS TOKEN>";
        private const string GoogleTestUserFirstName = "Mark";

        private const string FirebaseEmail = "<TEST USER EMAIL>";
        private const string FirebasePassword = "<TEST USER PASSWORD>";

        [TestMethod]
        public void FacebookTest()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

            var auth = authProvider.SignInWithOAuthAsync(FirebaseAuthType.Facebook, FacebookAccessToken).Result;

            auth.User.FirstName.ShouldBeEquivalentTo(FacebookTestUserFirstName);
            auth.FirebaseToken.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void GoogleTest()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

            var auth = authProvider.SignInWithOAuthAsync(FirebaseAuthType.Google, GoogleAccessToken).Result;

            auth.User.FirstName.ShouldBeEquivalentTo(GoogleTestUserFirstName);
            auth.FirebaseToken.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void EmailTest()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

            var auth = authProvider.SignInWithEmailAndPasswordAsync(FirebaseEmail, FirebasePassword).Result;

            auth.User.Email.ShouldBeEquivalentTo(FirebaseEmail);
            auth.FirebaseToken.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void Unknown_email_address_should_be_reflected_by_failure_reason()
        {
            using (var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey)))
            {
                try
                {
                    authProvider.SignInWithEmailAndPasswordAsync("someinvalidaddressxxx@foo.com", FirebasePassword).Wait();
                    Assert.Fail("Sign-in should fail with invalid email.");
                }
                catch (Exception e)
                {
                    var exception = (FirebaseAuthException) e.InnerException;
                    exception.Reason.Should().Be(AuthErrorReason.UnknownEmailAddress);
                }
            }
        }

        [TestMethod]
        public async Task CanLoginWithOldPersistedRefreshTokenForAnonymousLogin()
        {
            string refreshToken = "AK2wQ-w89JKmnCjHAYLqP3u74WSmJyXt7OcU1OQMTrQEnn4bYejS19CfCdHWI-G3m7kciA2RGYIdBv3-rHlhlZ6zC41khjXk8gVDjeb4CJGpWAhVuazNMIW9ksmBisJt6lhejXCw5DwLTOZk3wG06R_oZONEUG-DUDF3B3EIPfkGQuFJOiGBm-bMEQU1Qlan5qUXL9B4QSeWdZmnep9kfsxCX9kIx5tALXKTZGikl5KJaQ-XK6cNJGY";

            using (var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey)))
            {
                FirebaseAuthLink refreshedAuth = await authProvider.RefreshAuthAsync(new FirebaseAuth
                    {
                        User = new User(),
                        RefreshToken = refreshToken
                    }
                );
                refreshedAuth.User = await authProvider.GetUserAsync(refreshedAuth.FirebaseToken);

                Console.WriteLine(
                    $"Refreshed anonymous log in: LocalID [{refreshedAuth.User.LocalId}] FederatedID [{refreshedAuth.User.FederatedId}] Access token [{refreshedAuth.FirebaseToken}] Refresh token [{refreshedAuth.RefreshToken}] Expires [{refreshedAuth.Created.AddSeconds(refreshedAuth.ExpiresIn)}]");
            }
        }

        [TestMethod]
        public async Task CanLoginWithPersistedAnonymousLogin()
        {
            string refreshToken;
            using (var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey)))
            {
                FirebaseAuthLink anonAuth = await authProvider.SignInAnonymouslyAsync();
                refreshToken = anonAuth.RefreshToken;
                Console.WriteLine(
                    $"Anonymous log in: LocalID [{anonAuth.User.LocalId}] FederatedID [{anonAuth.User.FederatedId}] Access token [{anonAuth.FirebaseToken}] Refresh token [{anonAuth.RefreshToken}] Expires [{anonAuth.Created.AddSeconds(anonAuth.ExpiresIn)}]");
            }

            using (var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey)))
            {
                FirebaseAuthLink refreshedAuth = await authProvider.RefreshAuthAsync(new FirebaseAuth
                    {
                        User = new User(),
                        RefreshToken = refreshToken
                    }
                );
                refreshedAuth.User = await authProvider.GetUserAsync(refreshedAuth.FirebaseToken);

                Console.WriteLine(
                    $"Refreshed anonymous log in: LocalID [{refreshedAuth.User.LocalId}] FederatedID [{refreshedAuth.User.FederatedId}] Access token [{refreshedAuth.FirebaseToken}] Refresh token [{refreshedAuth.RefreshToken}] Expires [{refreshedAuth.Created.AddSeconds(refreshedAuth.ExpiresIn)}]");
            }
        }

        [TestMethod]
        public void Invalid_email_address_format_should_be_reflected_by_failure_reason()
        {
            using (var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey)))
            {
                try
                {
                    authProvider.SignInWithEmailAndPasswordAsync("notanemailaddress", FirebasePassword).Wait();
                    Assert.Fail("Sign-in should fail with invalid email.");
                }
                catch (Exception e)
                {
                    var exception = (FirebaseAuthException)e.InnerException;
                    exception.Reason.Should().Be(AuthErrorReason.InvalidEmailAddress);
                }
            }
        }



        [TestMethod]
        public void Invalid_password_should_be_reflected_by_failure_reason()
        {
            using (var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey)))
            {
                try
                {
                    authProvider.SignInWithEmailAndPasswordAsync(FirebaseEmail, "xx" + FirebasePassword).Wait();
                    Assert.Fail("Sign-in should fail with invalid password.");
                }
                catch (Exception e)
                {
                    var exception = (FirebaseAuthException)e.InnerException;
                    exception.Reason.Should().Be(AuthErrorReason.WrongPassword);
                }
            }
        }

        [TestMethod]
        public void CreateUserTest()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var email = $"abcd{new Random().Next()}@test.com"; 

            var auth = authProvider.SignInWithEmailAndPasswordAsync(email, "test1234").Result;

            auth.User.Email.ShouldBeEquivalentTo(email);
            auth.FirebaseToken.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void LinkAccountsTest()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var email = $"abcd{new Random().Next()}@test.com";

            var auth = authProvider.SignInAnonymouslyAsync().Result;
            var newAuth = auth.LinkToAsync(email, "test1234").Result;

            newAuth.User.Email.ShouldBeEquivalentTo(email);
            newAuth.User.LocalId.Should().Be(auth.User.LocalId);
            newAuth.FirebaseToken.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void LinkAccountsFacebookTest()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

            var auth = authProvider.SignInAnonymouslyAsync().Result;
            var newAuth = auth.LinkToAsync(FirebaseAuthType.Facebook, FacebookAccessToken).Result;

            newAuth.User.LocalId.Should().Be(auth.User.LocalId);
            newAuth.User.FirstName.ShouldBeEquivalentTo(FacebookTestUserFirstName);
            newAuth.FirebaseToken.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void GetLinkedAccountsTest()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var email = $"abcd{new Random().Next()}@test.com";

            var auth = authProvider.CreateUserWithEmailAndPasswordAsync(email, "test1234").Result;
            var linkedAccounts = authProvider.GetLinkedAccountsAsync(email).Result;

            linkedAccounts.IsRegistered.Should().BeTrue();
            linkedAccounts.Providers.Single().ShouldBeEquivalentTo(FirebaseAuthType.EmailAndPassword);
        }

        [TestMethod]
        public void RefreshAccessToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

            var auth = authProvider.SignInWithOAuthAsync(FirebaseAuthType.Facebook, FacebookAccessToken).Result;
            var originalToken = auth.FirebaseToken;
            
            // simulate the token already expired
            auth.Created = DateTime.MinValue;
            
            var freshAuth = auth.GetFreshAuthAsync().Result;

            freshAuth.FirebaseToken.Should().NotBe(originalToken);
        }
    }
}
