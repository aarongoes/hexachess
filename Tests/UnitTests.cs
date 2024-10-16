using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hexachess.Models;
using Logic.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Models.Enums;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        private static global::Factory.Factory factory = new global::Factory.Factory(new Dictionary<Engine, string>());
        private static IGameLogic gameLogic = factory.GetGameLogic(Engine.Memory);
        private  static IMoveLogic moveLogic = factory.GetMoveLogic(Engine.Memory);
        private  static IBoardLogic boardLogic = factory.GetBoardLogic(factory.GetHexagonLogic());
        private  static IUserLogic userLogic = factory.GetUserLogic(Engine.Memory);
        private  static IApplicationLogic applicationLogic = factory.GetApplicationLogic(gameLogic, moveLogic, boardLogic, userLogic);
        private static User user1;
        private static User user2;
        private static User user3;
        private static Game game1;
        private static Game game2;
        
        
        [TestInitialize]
        public void Setup()
        {
            var hasher = new PasswordHasher<User>();
            user1 = userLogic.Add(userLogic.Create("testuser1", hasher.HashPassword(userLogic.CreateEmtpy(), "testpassword1")));
            user2 = userLogic.Add(userLogic.Create("testuser2", hasher.HashPassword(userLogic.CreateEmtpy(), "testpassword2")));
            user3 = userLogic.Add(userLogic.Create("testuser3", hasher.HashPassword(userLogic.CreateEmtpy(), "testpassword3")));
            game1 = applicationLogic.CreateGame(user1.Token.ToString(), 1);
            game2 = applicationLogic.CreateGame(user1.Token.ToString(), 2);
            applicationLogic.JoinGame(game2.Token.ToString(), user3.Token.ToString());
        }
        
        [TestMethod]
        public void RegisterNewUser()//TC01
        {
            var model = new RegisterViewModel();
            model.Username = "account";
            model.Password = "test12345";
            model.PasswordRepeated = "test12345";
            var context = new ValidationContext(model, 
                serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(model, context, results, true);

            Assert.IsTrue(isValid);
            var hasher = new PasswordHasher<User>();
            var userToAdd = userLogic.Create(model.Username, hasher.HashPassword(userLogic.CreateEmtpy(), model.Password));
            User user = null;
            if (!userLogic.Exists(new KeyValuePair<string, object>("Name", userToAdd.Name)))
            {
                user = userLogic.Add(userToAdd);
            }
            Assert.IsNotNull(user);
        }
        
        [TestMethod]
        public void RegisterExistingUser()//TC02
        {
            var model = new RegisterViewModel();
            model.Username = "testuser1";
            model.Password = "testpassword1";
            model.PasswordRepeated = "testpassword1";
            var context = new ValidationContext(model, 
                serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(model, context, results, true);

            Assert.IsTrue(isValid);
            var hasher = new PasswordHasher<User>();
            var userToAdd = userLogic.Create(model.Username, hasher.HashPassword(userLogic.CreateEmtpy(), model.Password));
            User user = null;
            if (!userLogic.Exists(new KeyValuePair<string, object>("Name", userToAdd.Name)))
            {
                user = userLogic.Add(userToAdd);
            }
            Assert.IsNull(user);
        }
        
        [TestMethod]
        public void RegisterWithIncorrectValues()//TC03
        {
            var user = new RegisterViewModel();
            user.Username = "ac";
            user.Password = "test12345";
            user.PasswordRepeated = "test12345";
            var context = new ValidationContext(user, 
                serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(user, context, results, true);

            Assert.IsFalse(isValid);
        }
        
        [TestMethod]
        public void RegisterWithIncorrectPasswords()//TC04
        {
            var model = new RegisterViewModel();
            model.Username = "account";
            model.Password = "test12345";
            model.PasswordRepeated = "test123";
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(model, context, results, true);

            Assert.IsFalse(isValid);
        }
        
        [TestMethod]
        public void AccessAccountWithValidLoginCredentials()//TC05
        {
            var model = new LoginViewModel();
            model.Username = "testuser1";
            model.Password = "testpassword1";
            var context = new ValidationContext(model, 
                serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(model, context, results, true);

            Assert.IsTrue(isValid);
            
            var user = userLogic.Read(new KeyValuePair<string, object>("Name", model.Username));

            UserViewModel viewModel = null;
            
            var hasher = new PasswordHasher<User>();
            if (hasher.VerifyHashedPassword(user, user.Password, model.Password) != PasswordVerificationResult.Failed)
            {
                viewModel = new UserViewModel(user);
            }
            Assert.IsNotNull(viewModel);
        }
        
        [TestMethod]
        public void RestrictAccessToAccountWithInvalidLoginCredentials()//TC06
        {
            var model = new LoginViewModel();
            model.Username = "testuser1";
            model.Password = "testpassword";
            var context = new ValidationContext(model, 
                serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(model, context, results, true);

            Assert.IsTrue(isValid);
            
            var user = userLogic.Read(new KeyValuePair<string, object>("Name", model.Username));
            
            var hasher = new PasswordHasher<User>();
            Assert.IsTrue(hasher.VerifyHashedPassword(user, user.Password, model.Password) == PasswordVerificationResult.Failed);
        }
        
        [TestMethod]
        public void LoginWithIncorrectValues()//TC07
        {
            var model = new LoginViewModel();
            model.Username = "";
            model.Password = "";
            var context = new ValidationContext(model, 
                serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(model, context, results, true);

            Assert.IsFalse(isValid);
        }
        
        [TestMethod]
        public void GetGamesFromUser()//TC08, TC10
        {
            var games = gameLogic.ReadAll(new Dictionary<string, object>{{"FirstPlayerId", user1.Id},{"SecondPlayerId", user1.Id}});
            Assert.IsTrue(games.Count == 2);
        }
        
        [TestMethod]
        public void GetEmptyGameListFromUser()//TC09, TC12
        {
            var games = gameLogic.ReadAll(new Dictionary<string, object>{{"FirstPlayerId", user2.Id},{"SecondPlayerId", user2.Id}});
            Assert.IsTrue(games.Count == 0);
        }
        
        [TestMethod]
        public void GetGamesFromNonExistantUser()//TC11
        {
            var games = gameLogic.ReadAll(new Dictionary<string, object>{{"FirstPlayerId", 69},{"SecondPlayerId", 69}});
            Assert.IsTrue(games.Count == 0);
        }
        
        [TestMethod]
        public void CreateSinglePlayerGame()//TC13
        {
            var game = applicationLogic.CreateGame(user1.Token.ToString(), 1);

            Assert.IsNotNull(game);
        }
        
        [TestMethod]
        public void CreateMultiPlayerGame()//TC14
        {
            var game = applicationLogic.CreateGame(user1.Token.ToString(), 2);

            Assert.IsNotNull(game);
        }
        
        [TestMethod]
        public void OpenGame()//TC15
        {
            var game = gameLogic.Read(new KeyValuePair<string, object>("Token", game1.Token));
            var moves = moveLogic.ReadAll(new Dictionary<string, object>{{"GameId", game.Id}});
            game = gameLogic.AssignMoves(game, moves);
            var returnBoard = boardLogic.LoadBoard(game);
            var nextPlayer = boardLogic.GetNextPlayer(returnBoard);

            Assert.IsTrue(nextPlayer != 0);
        }
        
        [TestMethod]
        public void JoinGame()//TC16
        {
            var game = applicationLogic.CreateGame(user1.Token.ToString(), 2);
            var token = applicationLogic.JoinGame(game.Token.ToString(), user3.Token.ToString());

            Assert.IsNotNull(token);
        }
        
        [TestMethod]
        public void JoinGameThatAlreadyHasASecondPlayer()//TC17
        {
            var token = applicationLogic.JoinGame(game2.Token.ToString(), user2.Token.ToString());

            Assert.IsNull(token);
        }
        
        [TestMethod]
        public void JoinGameThatPlayerIsAlreadyAssignedTo()//TC18
        {
            var token = applicationLogic.JoinGame(game2.Token.ToString(), user1.Token.ToString());

            Assert.IsNull(token);
        }
        
        [TestMethod]
        public void PerformMove()//TC19
        {
            var move = new Move(0,-1,0,0);
            var board = applicationLogic.MovePiece(move, game1.Token.ToString(), user1.Token.ToString());
            var game = gameLogic.Read(new KeyValuePair<string, object>("Token", game1.Token));
            GameViewModel viewModel = new GameViewModel(game);

            Assert.IsTrue(viewModel.Moves.Count == 1);
            Assert.IsTrue(board.Succes);
        }
        
        [TestMethod]
        public void PerformMoveButPlayerIsNotAssignedToGame()//TC20
        {
            var move = new Move(0,-1,0,0);
            var board = applicationLogic.MovePiece(move, game1.Token.ToString(), user2.Token.ToString());

            Assert.IsFalse(board.Succes);
        }
        
        [TestMethod]
        public void PerformMoveButPlayerIsNotAanDeBeurt()//TC21
        {
            var move = new Move(0,-1,0,0);
            var board = applicationLogic.MovePiece(move, game2.Token.ToString(), user3.Token.ToString());

            Assert.IsFalse(board.Succes);
        }
        
        [TestMethod]
        public void PerformMoveButPieceIsNotAssignedToPlayer()//TC22
        {
            var move = new Move(0,1,0,0);
            var board = applicationLogic.MovePiece(move, game2.Token.ToString(), user1.Token.ToString());

            Assert.IsFalse(board.Succes);
        }
        
        [TestMethod]
        public void PerformMoveButPositionIsNotAllowed()//TC23
        {
            var move = new Move(0,-1,0,1);
            var board = applicationLogic.MovePiece(move, game2.Token.ToString(), user1.Token.ToString());

            Assert.IsFalse(board.Succes);
        }
        
//        [TestMethod]
//        public void WinGameMethod1()//TC24
//        {
//            var move = new Move(0,1,0,0);
//            var board = applicationLogic.MovePiece(move, game2.Token.ToString(), user1.Token.ToString());
//
//            Assert.IsFalse(board.Game.Winner == 0);
//        }
//        
//        [TestMethod]
//        public void WinGameMethod2()//TC25
//        {
//            var move = new Move(0,1,0,0);
//            var board = applicationLogic.MovePiece(move, game2.Token.ToString(), user1.Token.ToString());
//
//            Assert.IsFalse(board.Game.Winner == 0);
//        }
    }
}