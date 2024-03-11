using gcai.Data;
using gcai.Models;
using gcai.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using gcai.Migrations;
using Microsoft.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;

namespace gcio.Hubs
{
    public class PostHub : Hub
    {
        public async Task SendMessages(int startPostNum, int endPostNum, string userIn)
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
                    VoteTallyModel postVotesTally = data.tallyPostVotes(foundPosts[i].idPostModel.ToString());
                    VoteModel userVotes = data.GetUsersVotes(foundPosts[i].idPostModel.ToString(), userIn);
                    List<string> postData = new List<string>();
                    postData.Add(foundPosts[i].idPostModel);
                    postData.Add(foundPosts[i].Truth);
                    postData.Add(foundPosts[i].Humor);
                    postData.Add(foundPosts[i].Problem);
                    postData.Add(foundPosts[i].Solution);
                    postData.Add(foundPosts[i].UserId);
                    postData.Add(foundPosts[i].ScreenName);
                    postData.Add(posterContributionsTotal);
                    postData.Add(postVotesTally.UpVotedTotal.ToString());
                    postData.Add(postVotesTally.DownVotedTotal.ToString());
                    postData.Add(postVotesTally.FlaggedTotal);
                    postData.Add(userVotes.UpVoted);
                    postData.Add(userVotes.DownVoted);
                    postData.Add(userVotes.StarVoted);
                    postData.Add(userVotes.Flagged);

                    await SendTruthInit(JsonConvert.SerializeObject(postData));
                }
                if (foundPosts[i].PostType == "Humor")
                {
                    string? posterContributionsTotal = data.GetUserContributions(foundPosts[i].UserId);
                    VoteTallyModel postVotesTally = data.tallyPostVotes(foundPosts[i].idPostModel.ToString());
                    VoteModel userVotes = data.GetUsersVotes(foundPosts[i].idPostModel.ToString(), userIn);
                    List<string> postData = new List<string>();
                    postData.Add(foundPosts[i].idPostModel);
                    postData.Add(foundPosts[i].Truth);
                    postData.Add(foundPosts[i].Humor);
                    postData.Add(foundPosts[i].Problem);
                    postData.Add(foundPosts[i].Solution);
                    postData.Add(foundPosts[i].UserId);
                    postData.Add(foundPosts[i].ScreenName);
                    postData.Add(posterContributionsTotal);
                    postData.Add(postVotesTally.UpVotedTotal.ToString());
                    postData.Add(postVotesTally.DownVotedTotal.ToString());
                    postData.Add(postVotesTally.FlaggedTotal);
                    postData.Add(userVotes.UpVoted);
                    postData.Add(userVotes.DownVoted);
                    postData.Add(userVotes.StarVoted);
                    postData.Add(userVotes.Flagged);

                    await SendHumorInit(JsonConvert.SerializeObject(postData));
                }
                if (foundPosts[i].PostType == "Problem/Solution")
                {
                    string? posterContributionsTotal = data.GetUserContributions(foundPosts[i].UserId);
                    VoteTallyModel postVotesTally = data.tallyPostVotes(foundPosts[i].idPostModel.ToString());
                    VoteModel userVotes = data.GetUsersVotes(foundPosts[i].idPostModel.ToString(), userIn);
                    List<string> postData = new List<string>();
                    postData.Add(foundPosts[i].idPostModel);
                    postData.Add(foundPosts[i].Truth);
                    postData.Add(foundPosts[i].Humor);
                    postData.Add(foundPosts[i].Problem);
                    postData.Add(foundPosts[i].Solution);
                    postData.Add(foundPosts[i].UserId);
                    postData.Add(foundPosts[i].ScreenName);
                    postData.Add(posterContributionsTotal);
                    postData.Add(postVotesTally.UpVotedTotal.ToString());
                    postData.Add(postVotesTally.DownVotedTotal.ToString());
                    postData.Add(postVotesTally.FlaggedTotal);
                    postData.Add(userVotes.UpVoted);
                    postData.Add(userVotes.DownVoted);
                    postData.Add(userVotes.StarVoted);
                    postData.Add(userVotes.Flagged);

                    await SendProbSolInit(JsonConvert.SerializeObject(postData));
                }
            }
        }
        public async Task SendTruthInit(string postData)
        {
            await Clients.All.SendAsync("ReceiveMessageTruth", postData);
        }
        public async Task SendHumorInit(string postData)
        {
            await Clients.All.SendAsync("ReceiveMessageHumor", postData);
        }
        public async Task SendProbSolInit(string postData)
        {
            await Clients.All.SendAsync("ReceiveMessageProblemSolution", postData);
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

            DataAccess data = new DataAccess();         
            bool isFavorite = data.CheckFavorite(post.idPostModel, user);
            string? favorite = "0";
            if (isFavorite) { favorite = "*"; } else { favorite = "0"; }
            VoteTallyModel postVotesTally = data.tallyPostVotes(post.idPostModel);

            await Clients.All.SendAsync("ReceiveMessageTruth", user, message, post.idPostModel, post.ScreenName, totalUserContributions, postVotesTally.UpVotedTotal, postVotesTally.DownVotedTotal, postVotesTally.StarVotedTotal, postVotesTally.FlaggedTotal);
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

            DataAccess data = new DataAccess();
            bool isFavorite = data.CheckFavorite(post.idPostModel, user);
            string? favorite = "0";
            if (isFavorite) { favorite = "*"; } else { favorite = "0"; }
            VoteTallyModel postVotesTally = data.tallyPostVotes(post.idPostModel);

            await Clients.All.SendAsync("ReceiveMessageHumor", user, message, post.idPostModel, post.ScreenName, totalUserContributions, postVotesTally.UpVotedTotal, postVotesTally.DownVotedTotal, postVotesTally.StarVotedTotal, postVotesTally.FlaggedTotal);
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
            DataAccess data = new DataAccess();
            bool isFavorite = data.CheckFavorite(post.idPostModel, user);
            string? favorite = "0";
            if (isFavorite) { favorite = "*"; } else { favorite = "0"; }
            VoteTallyModel postVotesTally = data.tallyPostVotes(post.idPostModel);
            await Clients.All.SendAsync("ReceiveMessageProblemSolution", user, problem, solution, post.idPostModel, post.ScreenName, totalUserContributions, postVotesTally.UpVotedTotal, postVotesTally.DownVotedTotal, postVotesTally.StarVotedTotal, postVotesTally.FlaggedTotal);
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
                    voteModel.Flagged = "1";
                }
                voteModel.DateVoted = DateTime.Now.ToString("MM-dd-yyy");
                voteModel.ScreenName = screenname;
                PostModel postModel = new PostModel();
                postModel = await vote.PutNewVote(voteModel);

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
            string prompt = "Hello AI!";
            string answer = "Hello there! *smiles* My name is Alex AI.  My primary aim is to help by answering any questions you have about recovery from alcoholism. It's great that you're reaching out for guidance on your journey. Please feel free to ask me anything and I will do my best to provide you with helpful and accurate information based on Alcoholics Anonymous literature and what has been shared by our members. Please remember to check with your local AA group, sponsor, and healthcare professional regarding the information that I provide. I do make mistakes.";
            string id = "001";

            await Clients.All.SendAsync("ReceiveAIResponse", userIdIn, prompt, answer, id, screennameIn);
        }
    }
}
