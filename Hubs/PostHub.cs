using gcai.Data;
using gcai.Models;
using gcai.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using gcai.Migrations;

namespace gcio.Hubs
{
    public class PostHub : Hub
    {
        public async Task SendMessages(int startPostNum, int endPostNum)
        {
            string connectionId = Context.ConnectionId;
            List<PostModel> foundPosts = new List<PostModel>();
            DataAccess data = new DataAccess();
            foundPosts = data.GetUserPosts(startPostNum, endPostNum); //the number of posts for display

            for (int i = foundPosts.Count - 1; i >= 0; i--)
            {
                if (foundPosts[i].PostType == "Truth")
                {
                    string? posterContributionsTotal = data.GetUserContributions(foundPosts[i].UserId);
                    await SendTruthInit(foundPosts[i].UserId, foundPosts[i].Truth, foundPosts[i].idPostModel, foundPosts[i].ScreenName, posterContributionsTotal);
                }
                if (foundPosts[i].PostType == "Humor")
                {
                    string? posterContributionsTotal = data.GetUserContributions(foundPosts[i].UserId);
                    await SendHumorInit(foundPosts[i].UserId, foundPosts[i].Humor, foundPosts[i].idPostModel, foundPosts[i].ScreenName, posterContributionsTotal);
                }
                if (foundPosts[i].PostType == "Problem/Solution")
                {
                    string? posterContributionsTotal = data.GetUserContributions(foundPosts[i].UserId);
                    await SendProbSolInit(foundPosts[i].UserId, foundPosts[i].Problem, foundPosts[i].Solution, foundPosts[i].idPostModel, foundPosts[i].ScreenName, posterContributionsTotal);
                }
            }
        }
        public async Task SendTruthInit(string user, string message, string id, string screenname, string posterContributionsTotal)
        {
            await Clients.All.SendAsync("ReceiveMessageTruth", user, message, id, screenname, posterContributionsTotal);
        }
        public async Task SendHumorInit(string user, string message, string id, string screenname, string posterContributionsTotal)
        {
            await Clients.All.SendAsync("ReceiveMessageHumor", user, message, id, screenname, posterContributionsTotal);
        }
        public async Task SendProbSolInit(string user, string problem, string solution, string id, string screenname, string posterContributionsTotal)
        {
            await Clients.All.SendAsync("ReceiveMessageProblemSolution", user, problem, solution, id, screenname, posterContributionsTotal);
        }
        //sends indevidual message to all clients
        public async Task SendMessageTruth(string user, string message, string id, string screenname)
        {
            string? dateTime = DateAndTime.Now.ToString("yyyy:MM:dd:hh:mm:ss.FFFF");
            string newId = dateTime.Replace(":", "").Replace(".", "");
            PostModel post;
            PostModel postModel = new PostModel();
            DataAccess toPost = new DataAccess();
            postModel.PostType = "Truth";
            postModel.UserId = user;
            postModel.Truth = message;
            postModel.PostDate = DateTime.Now.ToString("MM-dd-yyy");
            if(id != "0")
            {
                postModel.idPostModel = id;
            } else {
                postModel.idPostModel = newId;
            }
            postModel.ScreenName = screenname;
            
            post = toPost.PutNewPost(postModel);
            string? totalUserContributions = toPost.GetUserContributions(user);
            await Clients.All.SendAsync("ReceiveMessageTruth", user, message, post.idPostModel, post.ScreenName, totalUserContributions);
        }
        //sends indevidual message to all clients
        public async Task SendMessageHumor(string user, string message, string id, string screenname)
        {
            string? dateTime = DateAndTime.Now.ToString("yyyy:MM:dd:hh:mm:ss.FFFF");
            string newId = dateTime.Replace(":", "").Replace(".", "");
            PostModel post;
            PostModel postModel = new PostModel();
            DataAccess toPost = new DataAccess();
            postModel.PostType = "Humor";
            postModel.UserId = user;
            postModel.Humor = message;
            postModel.PostDate = DateTime.Now.ToString("MM-dd-yyy");
            if (id != "0")
            {
                postModel.idPostModel = id;
            }
            else
            {
                postModel.idPostModel = newId;
            }
            postModel.ScreenName = screenname;
            post = toPost.PutNewPost(postModel);
            string? totalUserContributions = toPost.GetUserContributions(user);
            await Clients.All.SendAsync("ReceiveMessageHumor", user, message, post.idPostModel, post.ScreenName, totalUserContributions);
        }
        //sends indevidual message to all clients
        public async Task SendMessageProblemSolution(string user, string problem, string solution, string id, string screenname)
        {
            string? dateTime = DateAndTime.Now.ToString("yyyy:MM:dd:hh:mm:ss.FFFF");
            string newId = dateTime.Replace(":", "").Replace(".", "");
            PostModel post;
            PostModel postModel = new PostModel();
            DataAccess toPost = new DataAccess();
            postModel.PostType = "Problem/Solution";
            postModel.UserId = user;
            postModel.Problem = problem;
            postModel.Solution = solution;
            postModel.PostDate = DateTime.Now.ToString("MM-dd-yyy");
            if (id != "0")
            {
                postModel.idPostModel = id;
            }
            else
            {
                postModel.idPostModel = newId;
            }
            postModel.ScreenName = screenname;
            post = toPost.PutNewPost(postModel);
            string? totalUserContributions = toPost.GetUserContributions(user);
            await Clients.All.SendAsync("ReceiveMessageProblemSolution", user, problem, solution, post.idPostModel, post.ScreenName, totalUserContributions);
        }
        //AI Proccess //sends indevidual message to all clients
        public async Task ProccessAI(string user, string queryAI, string id, string screenname)
        {
            string? dateTime = DateAndTime.Now.ToString("yyyy:MM:dd:hh:mm:ss.FFFF");
            string newId = dateTime.Replace(":", "").Replace(".", "");
            
            AIModel queryModel = new AIModel();
            queryModel.UserId = user;
            queryModel.Prompt = queryAI;
            queryModel.PostDate = DateTime.Now.ToString("MM-dd-yyy");
            if (id != "0")
            {
                queryModel.idAIModel = id;
            }
            else
            {
                queryModel.idAIModel = newId;
            }
            queryModel.ScreenName = screenname;
            AIAccess aiAccess = new AIAccess();
            AIModel aiResponse = new AIModel();
            aiResponse =  await aiAccess.QueryAI(queryModel);
            AIModel storedResponse = new AIModel();
            DataAccess storeQuery = new DataAccess();
            storedResponse = storeQuery.PutNewAIExchange(aiResponse);
            await Clients.All.SendAsync("ReceiveAIResponse", user, storedResponse.Prompt, storedResponse.Answer, storedResponse.idAIModel, storedResponse.ScreenName);
        }
        //Take in vote
        public async Task CastVote(string voteTypeIn, string pepperIn, string userNameIn, string messageIdIn, string screenname)
        {
            DataAccess vote = new DataAccess();
            if (vote.ValidatePepper(userNameIn, pepperIn))//Checks to make sure user is an authenticated user.
            {
                string? dateTime = DateAndTime.Now.ToString("yyyy:MM:dd:hh:mm:ss.FFFF");
                string newId = dateTime.Replace(":", "").Replace(".", "");
                VoteModel voteModel = new VoteModel();
                voteModel.idVoteModel = newId;
                voteModel.PostRefNum = messageIdIn;
                voteModel.UserId = userNameIn;
                if (voteTypeIn == "Up"){
                    voteModel.UpVoted = "1";
                } else if (voteTypeIn == "Down"){
                    voteModel.DownVoted = "1";
                } else if (voteTypeIn == "Star"){
                    voteModel.StarVoted = "1";
                } else{
                    voteModel.Flagged = "1";}
                voteModel.DateVoted = DateTime.Now.ToString("MM-dd-yyy");
                voteModel.ScreenName = screenname;
                PostModel postModel = new PostModel();
                postModel = await vote.PutNewVote(voteModel);
                //update tally
                if(userNameIn != "anonymous@magandigi.com")
                {
                    string? totalUserContributions = vote.GetUserContributions(userNameIn);
                    await Clients.All.SendAsync("UpdateContributions", userNameIn, totalUserContributions);
                }
            }
            else
            {
                //do nothing because the request was not from a valid user
            }
        }
        //invokeIAHello
        //Take in vote
        public async Task helloAI(string userIdIn, string screennameIn)
        {
            string prompt = "Hello AI, I am a fellow traveler!";
            string answer = "Hello there! *smiles* My name is Alice, and I'm here to help you with any questions or concerns you may have as an AA member. It's great that you're reaching out for support and guidance in your journey towards sobriety. Please feel free to ask me anything, and I will do my best to provide you with helpful and accurate information based on the Big Book and the 12 Steps and 12 Traditions of Alcoholics Anonymous. *nod*";
            string id = "001";

            await Clients.All.SendAsync("ReceiveAIResponse", userIdIn, prompt, answer, id, screennameIn);
        }
    }
}
