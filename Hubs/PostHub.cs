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
using MySqlX.XDevAPI;

namespace gcio.Hubs
{
    public class PostHub : Hub
    {

        public async Task AdjustPosts(int newPage, string user, string connectionId)
        {
            SendMessagesUser(newPage, user);
            DataAccess data = new DataAccess();
            string newTotalPages = data.NewTotalPagesPages();
            List<PostModel> posts = data.GetPostsByPage(newPage);
            List<string> postsToKeepList = new List<string>();

            for(int i = 0; i < posts.Count; i++)
            {
                postsToKeepList.Add(posts[i].idPostModel);
            }

            await Clients.Client(connectionId).SendAsync("adjustClientNavPages", user, newPage, newTotalPages, postsToKeepList);
        }
        //sends client available page count for post navigation on first load.
        public async Task GetPages()
        {
            DataAccess data = new DataAccess();
            string totalAvailablePages = data.GetTotalPostPages();
            string connectionId = Context.ConnectionId.ToString();
            await SendTotalAvailablePageCount(totalAvailablePages, connectionId);
        }
        public async Task SendTotalAvailablePageCount(string totalAvailablePages, string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("ReceivTotalAvailablePageCount", totalAvailablePages);
        }
        

        //sends messages to a specific user based on selecting next or previous nav options
        public async Task SendMessagesUser(int pageNumIn, string userIn)
        {
            string connectionId = Context.ConnectionId.ToString();
            List<PostModel> foundPosts = new List<PostModel>();
            DataAccess data = new DataAccess();
            foundPosts = data.GetPostsByPage(pageNumIn); //the number of posts for display

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

                    await SendTruthInitUser(JsonConvert.SerializeObject(postData), connectionId);
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

                    await SendHumorInitUser(JsonConvert.SerializeObject(postData), connectionId);
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

                    await SendProbSolInitUser(JsonConvert.SerializeObject(postData), connectionId);
                }
            }
        }
        public async Task SendTruthInitUser(string postData, string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessageTruth", postData);
        }
        public async Task SendHumorInitUser(string postData, string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessageHumor", postData);
        }
        public async Task SendProbSolInitUser(string postData, string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessageProblemSolution", postData);

        }



        public async Task SendMessages(int pageNumIn, string userIn)
        {
            string connectionId = Context.ConnectionId.ToString();
            List<PostModel> foundPosts = new List<PostModel>();
            DataAccess data = new DataAccess();
            foundPosts = data.GetPostsByPage(pageNumIn); //the number of posts for display

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
            VoteTallyModel postVotesTally = data.tallyPostVotes(post.idPostModel);
            VoteModel userVotes = data.GetUsersVotes(post.idPostModel.ToString(), user);
            List<string> postData = new List<string>();
            postData.Add(post.idPostModel);
            postData.Add(post.Truth);
            postData.Add(post.Humor);
            postData.Add(post.Problem);
            postData.Add(post.Solution);
            postData.Add(post.UserId);
            postData.Add(post.ScreenName);
            postData.Add(totalUserContributions);
            postData.Add(postVotesTally.UpVotedTotal.ToString());
            postData.Add(postVotesTally.DownVotedTotal.ToString());
            postData.Add(postVotesTally.FlaggedTotal);
            postData.Add(userVotes.UpVoted);
            postData.Add(userVotes.DownVoted);
            postData.Add(userVotes.StarVoted);
            postData.Add(userVotes.Flagged);




            await Clients.All.SendAsync("ReceiveMessageTruth", JsonConvert.SerializeObject(postData));
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
            VoteTallyModel postVotesTally = data.tallyPostVotes(post.idPostModel);
            VoteModel userVotes = data.GetUsersVotes(post.idPostModel.ToString(), user);
            List<string> postData = new List<string>();
            postData.Add(post.idPostModel);
            postData.Add(post.Truth);
            postData.Add(post.Humor);
            postData.Add(post.Problem);
            postData.Add(post.Solution);
            postData.Add(post.UserId);
            postData.Add(post.ScreenName);
            postData.Add(totalUserContributions);
            postData.Add(postVotesTally.UpVotedTotal.ToString());
            postData.Add(postVotesTally.DownVotedTotal.ToString());
            postData.Add(postVotesTally.FlaggedTotal);
            postData.Add(userVotes.UpVoted);
            postData.Add(userVotes.DownVoted);
            postData.Add(userVotes.StarVoted);
            postData.Add(userVotes.Flagged);

            await Clients.All.SendAsync("ReceiveMessageHumor", JsonConvert.SerializeObject(postData));
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
            VoteTallyModel postVotesTally = data.tallyPostVotes(post.idPostModel);
            VoteModel userVotes = data.GetUsersVotes(post.idPostModel.ToString(), user);
            List<string> postData = new List<string>();
            postData.Add(post.idPostModel);
            postData.Add(post.Truth);
            postData.Add(post.Humor);
            postData.Add(post.Problem);
            postData.Add(post.Solution);
            postData.Add(post.UserId);
            postData.Add(post.ScreenName);
            postData.Add(totalUserContributions);
            postData.Add(postVotesTally.UpVotedTotal.ToString());
            postData.Add(postVotesTally.DownVotedTotal.ToString());
            postData.Add(postVotesTally.FlaggedTotal);
            postData.Add(userVotes.UpVoted);
            postData.Add(userVotes.DownVoted);
            postData.Add(userVotes.StarVoted);
            postData.Add(userVotes.Flagged);


            await Clients.All.SendAsync("ReceiveMessageProblemSolution", JsonConvert.SerializeObject(postData));
        }
        //AI Proccess //sends indevidual message to all clients
        public async Task ProccessAI(string user, string queryAI, string id, string screenname, string connectionId)
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
            aiResponse = await aiAccess.QueryAI(queryModel);
            AIModel storedResponse = new AIModel();
            DataAccess storeQuery = new DataAccess();
            storedResponse = storeQuery.PutNewAIExchange(aiResponse);
            Console.WriteLine(connectionId);
            await Clients.Client(connectionId).SendAsync("ReceiveAIResponse", user, storedResponse.Prompt, storedResponse.Answer, storedResponse.idAIModel, storedResponse.ScreenName);
        }
        //RemoveVote removeUp, removeDown, removeStared, removeFlagged, 
        public async Task RemoveVote(string voteTypeIn, string pepperIn, string userNameIn, string messageIdIn, string screenname)
        {
            DataAccess vote = new DataAccess();
            if (vote.ValidatePepper(userNameIn, pepperIn))//Checks to make sure user is an authenticated user.
            {
                string postRefIn = messageIdIn;
                string userIdIn = userNameIn;
                string voteType = voteTypeIn;
                vote.RemoveVote(postRefIn, userIdIn, voteType);
            }
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
                if (voteTypeIn == "UpVoted")
                {
                    voteModel.UpVoted = "1";
                }
                else if (voteTypeIn == "DownVoted")
                {
                    voteModel.DownVoted = "1";
                }
                else if (voteTypeIn == "StarVoted")
                {
                    voteModel.StarVoted = "1";
                }
                else if (voteTypeIn == "Flagged")
                {
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
        public async Task helloAI(string userIdIn, string screennameIn, string connectionId)
        {
            string prompt = "Hello AI!";
            string answer = "Hello there! *smiles* My name is Alex AI.  My primary aim is to help by answering any questions you have about recovery from alcoholism. It's great that you're reaching out for guidance on your journey. Please feel free to ask me anything and I will do my best to provide you with helpful and accurate information based on Alcoholics Anonymous literature and what has been shared by our members. Please remember to check with your local AA group, sponsor, and healthcare professional regarding the information that I provide. I do make mistakes.";
            string id = "001";
            Console.WriteLine(connectionId);
            await Clients.Client(connectionId).SendAsync("ReceiveAIResponse", userIdIn, prompt, answer, id, screennameIn);

        }
    }
}

