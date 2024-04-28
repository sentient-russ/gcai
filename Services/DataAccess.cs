using gcai.Models;
using MySql.Data.MySqlClient;
using MySql.Data;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;
using System;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Hosting;
using MimeKit;
using System.Data;
namespace gcai.Services;
public class DataAccess
{
    public string? connectionString;
    public DataAccess()
    {
        connectionString = Environment.GetEnvironmentVariable("DbConnectionString");
    }
    public AppUser getUserProfile(AppUser userIn)
    {
        AppUser? foundUser = new AppUser();
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM gcai.Users WHERE NormalizedUserName LIKE @UserId", conn1);
            cmd1.Parameters.AddWithValue("@UserId", "%" + userIn.UserName.ToUpper() + "%");
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                if (reader1.GetValue(1).Equals(DBNull.Value)) { } else { foundUser.UserName = reader1.GetString(1).ToString(); }
                if (reader1.GetValue(15).Equals(DBNull.Value)) { } else { foundUser.Contributions = reader1.GetInt16(15); }
                if (reader1.GetValue(16).Equals(DBNull.Value)) { } else { foundUser.IsBanned = reader1.GetBoolean(16); }
                if (reader1.GetValue(17).Equals(DBNull.Value)) { } else { foundUser.ScreenName = reader1.GetString(17); }
                if (reader1.GetValue(18).Equals(DBNull.Value)) { } else { foundUser.SobrietyDate = reader1.GetString(18); }
            }
            reader1.Close();
            conn1.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        //Get posts
        List<PostModel> foundList = new List<PostModel>();
        foundList = GetUserPosts(userIn.UserName);
        foundUser.Posts = foundList;
        //get favorites
        List<PostModel> favorites = new List<PostModel>();
        List<string> favStack = new List<string>();
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM gcai.VoteModels WHERE UserId LIKE @UserId", conn1);
            cmd1.Parameters.AddWithValue("@UserId", "%" + userIn.UserName.ToUpper() + "%");
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                if (reader1.GetValue(5).Equals(DBNull.Value)) { } else { favStack.Add(reader1.GetString(1).ToString()); }

            }
            reader1.Close();
            conn1.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        for (int i = 0; i <= favStack.Count - 1; i++)
        {
            favorites.Add(GetPost(favStack[i]));
        }
        foundUser.favorites = favorites;
        return foundUser;
    }
    /*
     * Gets recent posts for initial connection.
     * @param postNumStart lower range bounds for posts
     * @param postNumEnd uper range bounds for posts
     * @return returns a List of the most recent posts
     */
    public List<PostModel> GetUserPosts(int startPostNum, int endPostNum)
    {
        List<PostModel> foundPostsList = new List<PostModel>();
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM gcai.PostModel ORDER BY idPostModel DESC LIMIT @startPostNum, @endPostNum", conn1);
            cmd1.Parameters.AddWithValue("@startPostNum", startPostNum);
            cmd1.Parameters.AddWithValue("@endPostNum", endPostNum);
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                PostModel userPosts = new PostModel();
                userPosts.idPostModel = reader1.GetString(0);
                userPosts.UserId = reader1.GetString(1);
                userPosts.PostType = reader1.GetString(2);
                userPosts.Humor = reader1.GetString(3);
                userPosts.Problem = reader1.GetString(4);
                userPosts.Solution = reader1.GetString(5);
                userPosts.Truth = reader1.GetString(6);
                userPosts.PostDate = reader1.GetString(7);
                userPosts.NumPromotions = reader1.GetInt16(8);
                userPosts.NumDemotions = reader1.GetInt16(9);
                userPosts.NumFlags = reader1.GetInt16(10);
                userPosts.ScreenName = reader1.GetString(11);

                foundPostsList.Add(userPosts);
            }
            //returns a task error if no records are found.           
            if (reader1.HasRows == false && foundPostsList.Count == 0)
            {
                PostModel userPosts2 = new PostModel();
                userPosts2.idPostModel = "Not Found";
                foundPostsList.Add(userPosts2);
                return foundPostsList;
            }
            reader1.Close();

            conn1.Close();
        } catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        return foundPostsList;
    }
    /*
     * Gets a the total number of available pages for incriments of ten posts
     * @param none
     * @return returns a List of the requested posts.
     */
        public string GetTotalPostPages()
        {
            //get post count
            int postCount = 0;
            try
            {
                MySqlConnection conn1 = new MySqlConnection(connectionString);
                conn1.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT COUNT(*) FROM gcai.PostModel", conn1);
                MySqlDataReader reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    postCount = int.Parse(reader1.GetString(0));
                }
                reader1.Close();
                conn1.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Double pageCount = postCount / 10;
            return pageCount.ToString();
        } 

        /*
         * Gets a range of 10 posts based on a page number. 
         * @param pageNumIn
         * @return returns a List of the requested posts.
         */
        public List<PostModel> GetPostsByPage(int pageNumIn)
    {
    //get post count
    int postCount = 0;
    try
    {
        MySqlConnection conn1 = new MySqlConnection(connectionString);
        conn1.Open();
        MySqlCommand cmd1 = new MySqlCommand("SELECT COUNT(*) FROM gcai.PostModel", conn1);
        MySqlDataReader reader1 = cmd1.ExecuteReader();
        while (reader1.Read())
        {
            postCount = int.Parse(reader1.GetString(0));                
        }
        reader1.Close();
        conn1.Close();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
        Double pageCount = postCount / 10;
        int endPostNum = 0;
        int startPostNum = 0;
        if(pageNumIn == 1)
        {
            endPostNum = 9;
            startPostNum = 0;
        } else if (pageNumIn >= pageCount)
        {
            endPostNum = postCount;
            startPostNum = endPostNum - 10;
        }
        else
        {
            endPostNum = pageNumIn * 10;
            startPostNum = endPostNum - 10;
        }
        List<PostModel> foundPostsList = new List<PostModel>();
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM gcai.PostModel ORDER BY idPostModel DESC LIMIT @startPostNum, @endPostNum", conn1);
            cmd1.Parameters.AddWithValue("@startPostNum", startPostNum);
            cmd1.Parameters.AddWithValue("@endPostNum", endPostNum);
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                PostModel userPosts = new PostModel();
                userPosts.idPostModel = reader1.GetString(0);
                userPosts.UserId = reader1.GetString(1);
                userPosts.PostType = reader1.GetString(2);
                userPosts.Humor = reader1.GetString(3);
                userPosts.Problem = reader1.GetString(4);
                userPosts.Solution = reader1.GetString(5);
                userPosts.Truth = reader1.GetString(6);
                userPosts.PostDate = reader1.GetString(7);
                userPosts.NumPromotions = reader1.GetInt16(8);
                userPosts.NumDemotions = reader1.GetInt16(9);
                userPosts.NumFlags = reader1.GetInt16(10);
                userPosts.ScreenName = reader1.GetString(11);

                foundPostsList.Add(userPosts);
            }
            //returns a task error if no records are found.           
            if (reader1.HasRows == false && foundPostsList.Count == 0)
            {
                PostModel userPosts2 = new PostModel();
                userPosts2.idPostModel = "Not Found";
                foundPostsList.Add(userPosts2);
                return foundPostsList;
            }
            reader1.Close();

            conn1.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        return foundPostsList;
    }
    /*
     * Gets all of the posts for a secific user
     * @param userId a users email address 
     * @return returns a List of posts
     */
    public List<PostModel> GetUserPosts(string userIdIn)
    {
        List<PostModel> foundPostsList = new List<PostModel>();
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM gcai.PostModel WHERE UserId LIKE @UserName", conn1);
            cmd1.Parameters.AddWithValue("@UserName", "%" + userIdIn + "%");
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                PostModel userPosts = new PostModel();
                userPosts.idPostModel = reader1.GetString(0);
                userPosts.UserId = reader1.GetString(1);
                userPosts.PostType = reader1.GetString(2);
                userPosts.Humor = reader1.GetString(3);
                userPosts.Problem = reader1.GetString(4);
                userPosts.Solution = reader1.GetString(5);
                userPosts.Truth = reader1.GetString(6);
                userPosts.PostDate = reader1.GetString(7);
                userPosts.NumPromotions = reader1.GetInt16(8);
                userPosts.NumDemotions = reader1.GetInt16(9);
                userPosts.NumFlags = reader1.GetInt16(10);
                userPosts.ScreenName = reader1.GetString(11);
                foundPostsList.Add(userPosts);
            }
            //returns a task error if no records are found.           
            if (reader1.HasRows == false && foundPostsList.Count == 0)
            {
                PostModel userPosts2 = new PostModel();
                userPosts2.idPostModel = "Not Found";
                foundPostsList.Add(userPosts2);
                return foundPostsList;
            }
            reader1.Close();
            conn1.Close();
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }


        return foundPostsList;
    }

    /*
     * Gets a single post based on post id
     * @param userId a users email address 
     * @return returns a PostModel object
     */
    public PostModel GetPost(string postIdIn)
    {
        PostModel foundPost = new PostModel();
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM gcai.PostModel WHERE idPostModel LIKE @idPostModel", conn1);
            cmd1.Parameters.AddWithValue("@idPostModel", "%" + postIdIn + "%");
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                PostModel post = new PostModel();
                post.idPostModel = reader1.GetString(0);
                post.UserId = reader1.GetString(1);
                post.PostType = reader1.GetString(2);
                post.Humor = reader1.GetString(3);
                post.Problem = reader1.GetString(4);
                post.Solution = reader1.GetString(5);
                post.Truth = reader1.GetString(6);
                post.PostDate = reader1.GetString(7);
                post.NumPromotions = reader1.GetInt16(8);
                post.NumDemotions = reader1.GetInt16(9);
                post.NumFlags = reader1.GetInt16(10);
                post.ScreenName = reader1.GetString(11);
                foundPost = post;
            }
            //returns a task error if no records are found.           
            if (reader1.HasRows == false && foundPost.idPostModel.Equals(""))
            {
                PostModel notFound = new PostModel();
                notFound.idPostModel = "Not Found";
                foundPost = notFound;
                return foundPost;
            }
            reader1.Close();
            conn1.Close();
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return foundPost;
    }
    /*
     * Inserts posts in the database
     * @param postIn PostModel containing the users post. 
     * @return returns a post model object. idPostModel="Not Found" if the post was not a success.
     */
    public PostModel PutNewPost(PostModel postIn)
    {
        PostModel postWithId;
        try
        {
            string sqlPostString = "INSERT INTO gcai.PostModel (idPostModel, UserId, PostType, Humor, Problem, Solution, Truth, PostDate, NumPromotions, NumDemotions, NumFlags, ScreenName) VALUES (@idPostModel, @UserId, @PostType, @Humor, @Problem, @Solution, @Truth, @PostDate, @NumPromotions, @NumDemotions, @NumFlags, @ScreenName)";
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd2 = new MySqlCommand(sqlPostString, conn1);
            cmd2.Parameters.AddWithValue("@idPostModel", postIn.idPostModel);
            cmd2.Parameters.AddWithValue("@UserId", postIn.UserId);
            cmd2.Parameters.AddWithValue("@PostType", postIn.PostType);
            cmd2.Parameters.AddWithValue("@Humor", postIn.Humor);
            cmd2.Parameters.AddWithValue("@Problem", postIn.Problem);
            cmd2.Parameters.AddWithValue("@Solution", postIn.Solution);
            cmd2.Parameters.AddWithValue("@Truth", postIn.Truth);
            cmd2.Parameters.AddWithValue("@PostDate", DateTime.Now.ToString("yyyy-MM-dd"));
            cmd2.Parameters.AddWithValue("@NumPromotions", postIn.NumPromotions);
            cmd2.Parameters.AddWithValue("@NumDemotions", postIn.NumDemotions);
            cmd2.Parameters.AddWithValue("@NumFlags", postIn.NumFlags);
            cmd2.Parameters.AddWithValue("@ScreenName", postIn.ScreenName);
            cmd2.ExecuteNonQuery();
            conn1.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        postWithId = GetPost(postIn.idPostModel);
        string result = incrementUserContributions(postIn.UserId);
        return postWithId;
    }
    /*
     * Updates a users post
     */
    public PostModel UpdatePost(PostModel postIn)
    {
        try
        {
            if(postIn.Truth != "")
            {
                string sqlUpdateString = "UPDATE gcai.PostModel SET Truth = @Truth WHERE idPostModel = @idPostModel";
                MySqlConnection conn1 = new MySqlConnection(connectionString);
                conn1.Open();
                MySqlCommand cmd2 = new MySqlCommand(sqlUpdateString, conn1);
                cmd2.Parameters.AddWithValue("@idPostModel", postIn.idPostModel);
                cmd2.Parameters.AddWithValue("@Truth", postIn.Truth);
                cmd2.ExecuteNonQuery();
                conn1.Close();
            }
            else if(postIn.Humor != "")
            {
                string sqlUpdateString = "UPDATE gcai.PostModel SET Humor = @Humor WHERE idPostModel = @idPostModel";
                MySqlConnection conn1 = new MySqlConnection(connectionString);
                MySqlCommand cmd2 = new MySqlCommand(sqlUpdateString, conn1);
                conn1.Open();
                cmd2.Parameters.AddWithValue("@idPostModel", postIn.idPostModel);
                cmd2.Parameters.AddWithValue("@Humor", postIn.Humor);
                cmd2.ExecuteNonQuery();
                conn1.Close();
            }
            else
            {
                string sqlUpdateString = "UPDATE gcai.PostModel SET Problem = @Problem, Solution = @Solution WHERE idPostModel = @idPostModel";
                MySqlConnection conn1 = new MySqlConnection(connectionString);
                conn1.Open();
                MySqlCommand cmd2 = new MySqlCommand(sqlUpdateString, conn1);
                cmd2.Parameters.AddWithValue("@idPostModel", postIn.idPostModel);
                cmd2.Parameters.AddWithValue("@Problem", postIn.Problem);
                cmd2.Parameters.AddWithValue("@Solution", postIn.Solution);
                cmd2.ExecuteNonQuery();
                conn1.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        PostModel postWithId = new PostModel();
        postWithId = GetPost(postIn.idPostModel);
        string result = incrementUserContributions(postIn.UserId);
        return postWithId;
    }
    /*
     * Inserts ai questions and responses in the data base.
     * @param queryModel containing the user question and AI response
     * @returns a IAModel containing all of the saved information. 
     */
    public AIModel PutNewAIExchange(AIModel queryModel)
    {
        AIModel queryWithDate = queryModel;

        try
        {
            string sql = "INSERT INTO gcai.AIModel (idAIModel, UserId, Prompt, Answer, Context, PostDate, ScreenName) VALUES (@idAIModel, @UserId, @Prompt, @Answer, @Context, @PostDate, @ScreenName)";
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd2 = new MySqlCommand(sql, conn1);
            cmd2.Parameters.AddWithValue("@idAIModel", queryWithDate.idAIModel);
            cmd2.Parameters.AddWithValue("@UserId", queryWithDate.UserId);
            cmd2.Parameters.AddWithValue("@Prompt", queryWithDate.Prompt);
            cmd2.Parameters.AddWithValue("@Answer", queryWithDate.Answer);
            cmd2.Parameters.AddWithValue("@Context", queryWithDate.Context);
            cmd2.Parameters.AddWithValue("@PostDate", queryWithDate.PostDate = DateTime.Now.ToString("yyyy-MM-dd"));
            cmd2.Parameters.AddWithValue("@ScreenName", queryWithDate.ScreenName);
            cmd2.ExecuteNonQuery();
            conn1.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        return queryWithDate;
    }

    public void DeleteVote(string idVoteModelIn)
    {
        string value = "1";
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd1 = new MySqlCommand("DELETE FROM gcai.VoteModels WHERE idVoteModel = @idVoteModel", conn1);
            cmd1.Parameters.AddWithValue("@idVoteModel", idVoteModelIn);
            cmd1.ExecuteNonQuery();
            conn1.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
    /*
     * Gets a list of votes for a given post.
     */
    public List<VoteModel> GetVotes(string postRefIn)
    {
        List<VoteModel> foundList = new List<VoteModel>();
        MySqlDataReader reader1;
        MySqlConnection conn1 = new MySqlConnection(connectionString);
        conn1.Open();
        try
        {
            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM gcai.VoteModels WHERE PostRefNum LIKE @postRefIn", conn1);
            cmd1.Parameters.AddWithValue("@postRefIn", "%" + postRefIn + "%");
            reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                VoteModel vote = new VoteModel();
                if (reader1.GetValue(0).Equals(DBNull.Value)) { } else { vote.idVoteModel = reader1.GetString(0); }
                if (reader1.GetValue(1).Equals(DBNull.Value)) { } else { vote.PostRefNum = reader1.GetString(1); }
                if (reader1.GetValue(2).Equals(DBNull.Value)) { } else { vote.UserId = reader1.GetString(2); }
                if (reader1.GetValue(3).Equals(DBNull.Value)) { } else { vote.UpVoted = reader1.GetString(3); }
                if (reader1.GetValue(4).Equals(DBNull.Value)) { } else { vote.DownVoted = reader1.GetString(4); }
                if (reader1.GetValue(5).Equals(DBNull.Value)) { } else { vote.StarVoted = reader1.GetString(5); }
                if (reader1.GetValue(6).Equals(DBNull.Value)) { } else { vote.Flagged = reader1.GetString(6); }
                if (reader1.GetValue(7).Equals(DBNull.Value)) { } else { vote.DateVoted = reader1.GetString(7); }
                foundList.Add(vote);
            }
            //returns a task NOT FOUND if no records are found.           
            if (!reader1.HasRows && foundList.Count == 0)
            {
                VoteModel notFound = new VoteModel();
                notFound.idVoteModel = "Not Found";
                foundList.Add(notFound);
                reader1.Close();
                return foundList;
            }
            reader1.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        conn1.Close();
        return foundList;
    }
    /*
     * Gets a list of votes for a given post for tally. NEW
     */
    public List<VoteModel> GetPostVotesTally(string postRefIn)
    {
        List<VoteModel> foundList = new List<VoteModel>();
        MySqlDataReader reader1;
        MySqlConnection conn1 = new MySqlConnection(connectionString);
        conn1.Open();
        try
        {
            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM gcai.VoteModels WHERE PostRefNum LIKE @postRefIn", conn1);
            cmd1.Parameters.AddWithValue("@postRefIn", "%" + postRefIn + "%");
            reader1 = cmd1.ExecuteReader();   
            while (reader1.Read())
            {
                VoteModel vote = new VoteModel();
                if (reader1.GetValue(0).Equals(DBNull.Value)) { } else { vote.idVoteModel = reader1.GetString(0); }
                if (reader1.GetValue(1).Equals(DBNull.Value)) { } else { vote.PostRefNum = reader1.GetString(1); }
                if (reader1.GetValue(2).Equals(DBNull.Value)) { } else { vote.UserId = reader1.GetString(2); }
                if (reader1.GetValue(3).Equals(DBNull.Value)) { } else { vote.UpVoted = reader1.GetString(3); }
                if (reader1.GetValue(4).Equals(DBNull.Value)) { } else { vote.DownVoted = reader1.GetString(4); }
                if (reader1.GetValue(5).Equals(DBNull.Value)) { } else { vote.StarVoted = reader1.GetString(5); }
                if (reader1.GetValue(6).Equals(DBNull.Value)) { } else { vote.Flagged = reader1.GetString(6); }
                if (reader1.GetValue(7).Equals(DBNull.Value)) { } else { vote.DateVoted = reader1.GetString(7); }
                foundList.Add(vote);
            }
            //returns a task NOT FOUND if no records are found.           
            if (!reader1.HasRows && foundList.Count == 0)
            {
                VoteModel notFound = new VoteModel();
                notFound.idVoteModel = "Not Found";
                foundList.Add(notFound);
                reader1.Close();
                return foundList;
            }
            reader1.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        conn1.Close();
        return foundList;
    }
    /*
     * Tallies the votes for a given post
     */
    public VoteTallyModel tallyPostVotes(string postRefNumIn)
    {
        int? upVoted = 0;
        int? downVoted = 0;
        int? starVoted = 0;
        int? flagged = 0;
        List<VoteModel> foundVotes = GetPostVotesTally(postRefNumIn);
        VoteTallyModel returnTally = new VoteTallyModel();
        for(int i = 0; i < foundVotes.Count; i++)
        {
            if (foundVotes[i].UpVoted == "1")
            {
                upVoted = upVoted + 1;
            } else
            if (foundVotes[i].DownVoted == "1")
            {
                downVoted = downVoted + 1;
            } else
            if (foundVotes[i].StarVoted == "1")
            {
                starVoted = starVoted + 1;
            } else
            if (foundVotes[i].Flagged == "1")
            {
                flagged = flagged + 1;
            }
        }
        returnTally.PostRefNum = postRefNumIn;
        returnTally.UpVotedTotal = upVoted.ToString();
        returnTally.DownVotedTotal = downVoted.ToString();
        returnTally.StarVotedTotal = starVoted.ToString();
        returnTally.FlaggedTotal = flagged.ToString();
        return returnTally;
    }
    /*
     * Inserts vote in the database
     * @param voteIn VoteModel containing the users vote. 
     * @return returns a vote model object containing the updated vote tallies.
     */
    public async Task<PostModel> PutNewVote(VoteModel voteIn)
    {
        //This section deletes a equivilent vote if it exhists to prevent duplicate voting
        bool countVote = true;
        List<VoteModel> votesList = GetVotes(voteIn.PostRefNum);
        string voteType = "";
        if (votesList[0].idVoteModel == "Not Found")
        {
            
        }
        else {
            for (int i = 0; i < votesList.Count; i++)
            {
                if (voteIn.UpVoted == "1" && votesList[i].UpVoted == "1" && voteIn.UserId == votesList[i].UserId)
                {
                    voteType = "UpVoted";
                    DeleteVote(votesList[i].idVoteModel);
                    countVote = false;
                }
                if (voteIn.DownVoted == "1" && votesList[i].DownVoted == "1" && voteIn.UserId == votesList[i].UserId)
                {
                    voteType = "DownVoted";
                    DeleteVote(votesList[i].idVoteModel);
                    countVote = false;
                }
                if (voteIn.StarVoted == "1" && votesList[i].StarVoted == "1" && voteIn.UserId == votesList[i].UserId)
                {
                    voteType = "StarVoted";
                    DeleteVote(votesList[i].idVoteModel);
                    countVote = false;
                }
                if (voteIn.Flagged == "1" && votesList[i].Flagged == "1" && voteIn.UserId == votesList[i].UserId)
                {
                    voteType = "Flagged";
                    DeleteVote(votesList[i].idVoteModel);
                    countVote = false;
                }
            }
        }
        //This tracks the indevidual votes of each user in the VoteModel Table
        MySqlConnection conn2 = new MySqlConnection(connectionString);
        conn2.Open();
        try
        {
            string sqlVoteString = "INSERT INTO gcai.VoteModels (idVoteModel, PostRefNum, UserId, UpVoted, DownVoted, StarVoted, Flagged, DateVoted, ScreenName) VALUES (@idVoteModel, @PostRefNum, @UserId, @UpVoted, @DownVoted, @StarVoted, @Flagged, @DateVoted, @ScreenName)";

            MySqlCommand cmd3 = new MySqlCommand(sqlVoteString, conn2);
            cmd3.Parameters.AddWithValue("@idVoteModel", voteIn.idVoteModel);
            cmd3.Parameters.AddWithValue("@PostRefNum", voteIn.PostRefNum);
            cmd3.Parameters.AddWithValue("@UserId", voteIn.UserId);
            cmd3.Parameters.AddWithValue("@UpVoted", voteIn.UpVoted);
            cmd3.Parameters.AddWithValue("@DownVoted", voteIn.DownVoted);
            cmd3.Parameters.AddWithValue("@StarVoted", voteIn.StarVoted);
            cmd3.Parameters.AddWithValue("@Flagged", voteIn.Flagged);
            cmd3.Parameters.AddWithValue("@ScreenName", voteIn.ScreenName);
            if (voteIn.DateVoted == "")
            {
                cmd3.Parameters.AddWithValue("@DateVoted", DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else
            {
                cmd3.Parameters.AddWithValue("@DateVoted", voteIn.DateVoted);
            }
            cmd3.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        //Adds vote and stores them in the gcai.postModel totals.
        PostModel postModel = GetPost(voteIn.PostRefNum);
        MySqlConnection conn3 = new MySqlConnection(connectionString);
        conn3.Open();
        if (voteIn.UpVoted == "1")
        {
            postModel.NumPromotions += 1;
            try
            {
                string sqlVoteString = "UPDATE gcai.PostModel SET NumPromotions=@NumPromotions WHERE idPostModel=@idPostModel";
                MySqlCommand cmd3 = new MySqlCommand(sqlVoteString, conn3);
                cmd3.Parameters.AddWithValue("@idPostModel", postModel.idPostModel);
                cmd3.Parameters.AddWithValue("@NumPromotions", postModel.NumPromotions);
                cmd3.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }           
        }
        else if (voteIn.DownVoted == "1")
        {
            postModel.NumDemotions += 1;
            try
            {
                string sqlVoteString = "UPDATE gcai.PostModel SET NumDemotions=@NumDemotions WHERE idPostModel=@idPostModel";
                MySqlCommand cmd4 = new MySqlCommand(sqlVoteString, conn3);
                cmd4.Parameters.AddWithValue("@idPostModel", postModel.idPostModel);
                cmd4.Parameters.AddWithValue("@NumDemotions", postModel.NumDemotions);
                cmd4.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }           
        } 
        else if(voteIn.Flagged == "1")
        {
            postModel.NumFlags += 1;
            try
            {
                string sqlVoteString = "UPDATE gcai.PostModel SET NumFlags=@NumFlags WHERE idPostModel=@idPostModel";
                MySqlCommand cmd4 = new MySqlCommand(sqlVoteString, conn3);
                cmd4.Parameters.AddWithValue("@idPostModel", postModel.idPostModel);
                cmd4.Parameters.AddWithValue("@NumFlags", postModel.NumDemotions);
                cmd4.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        } else if (voteIn.StarVoted == "1")
        {
            try
            {
                string sqlVoteString = "INSERT INTO gcai.FavoritesModel(idPostModel, UserId, PostDate) VALUES(@idPostModel, @UserId, @PostDate)";              
                MySqlCommand cmd4 = new MySqlCommand(sqlVoteString, conn3);
                cmd4.Parameters.AddWithValue("@idPostModel", postModel.idPostModel);
                cmd4.Parameters.AddWithValue("@UserId", voteIn.UserId.ToUpper());
                cmd4.Parameters.AddWithValue("@PostDate", postModel.PostDate);
                cmd4.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        if (countVote)
        {
            string result1 = incrementUserContributions(voteIn.UserId);
        }
        return postModel;       
    }
    public string GetPepper(string userIdIn)
    {
        string returnStringPepper = "";
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT ConcurrencyStamp FROM gcai.Users WHERE NormalizedUserName LIKE @UserId", conn1);
            cmd1.Parameters.AddWithValue("@UserId", "%" + userIdIn.ToUpper() + "%");
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                if (reader1.GetValue(0).Equals(DBNull.Value)) { } else { returnStringPepper = reader1.GetString(0); }
            }
            reader1.Close();
            conn1.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return returnStringPepper;
    }
    //this method checks all inbound voting attemps to make sure that the user is authenticated.
    public bool ValidatePepper(string userIdIn, string pepperIn)
    {
        bool isFound = false;
        string foundString = "";
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT ConcurrencyStamp FROM gcai.Users WHERE NormalizedUserName LIKE @UserId", conn1);
            cmd.Parameters.AddWithValue("@UserId", "%" + userIdIn.ToUpper() + "%");
            MySqlDataReader reader1 = cmd.ExecuteReader();

            while (reader1.Read())
            {
                if (reader1.GetValue(0).Equals(DBNull.Value)) { } else { foundString = reader1.GetString(0); }
            }
            reader1.Close();
            conn1.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        isFound = foundString.Equals(pepperIn);
        return isFound;
    }
    //gets the users screen name
    public string GetScreenName(string userIdIn)
    {
        string returnStringScreenName = "";
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT ScreenName FROM gcai.Users WHERE NormalizedUserName LIKE @UserId", conn1);
            cmd1.Parameters.AddWithValue("@UserId", "%" + userIdIn.ToUpper() + "%");
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                if (reader1.GetValue(0).Equals(DBNull.Value)) { } else { returnStringScreenName = reader1.GetString(0); }
            }
            reader1.Close();
            conn1.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return returnStringScreenName;
    }
    //gets the users name
    public string GetUserName(string screenNameIn)
    {
        string returnUserName = "";
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT UserName FROM gcai.Users WHERE ScreenName LIKE @screenNameIn", conn1);
            cmd1.Parameters.AddWithValue("@screenNameIn", "%" + screenNameIn + "%");
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                if (reader1.GetValue(0).Equals(DBNull.Value)) { } else { returnUserName = reader1.GetString(0); }
            }
            reader1.Close();
            conn1.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return returnUserName;
    }
    public string GetUserContributions(string userIdIn)
    {
        string returnString = "0";
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT Contributions FROM gcai.Users WHERE NormalizedUserName LIKE @UserId", conn1);
            cmd1.Parameters.AddWithValue("@UserId", "%" + userIdIn.ToUpper() + "%");
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                if (reader1.GetValue(0).Equals(DBNull.Value)) { } else { returnString = reader1.GetInt16(0).ToString(); }
            }
            reader1.Close();
            conn1.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return returnString;
    }
    public string incrementUserContributions(string userIdIn)
    {
        int returnCurrentContributions = 0;
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            string userIdNormalized = userIdIn.ToUpper();
            string returnString = GetUserContributions(userIdIn);
            int currentContributions = Int32.Parse(returnString);
            currentContributions += 1;
            string sqlVoteString = "UPDATE gcai.Users SET Contributions=@currentContributions WHERE NormalizedUserName=@userIdNormalized";
            MySqlCommand cmd3 = new MySqlCommand(sqlVoteString, conn1);
            cmd3.Parameters.AddWithValue("@userIdNormalized", userIdNormalized);
            cmd3.Parameters.AddWithValue("@currentContributions", currentContributions);
            cmd3.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        return returnCurrentContributions.ToString(); ;
    }
    public string RemoveFavorite(string postRefIn, string userIdIn)
    {
        string returnString = "";
        string userIdNormalized = userIdIn.ToUpper();
        //delete from FavoritesModel table
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            string sqlFavString = "DELETE FROM gcai.FavoritesModel WHERE idPostModel = @idPostModel AND UserID = @UserId";
            MySqlCommand cmd4 = new MySqlCommand(sqlFavString, conn1);
            cmd4.Parameters.AddWithValue("@idPostModel", postRefIn);
            cmd4.Parameters.AddWithValue("@UserId", userIdNormalized);
            cmd4.ExecuteNonQuery();
            conn1.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        //delete from FavoritesModel table
        try
        {
            MySqlConnection conn2 = new MySqlConnection(connectionString);
            conn2.Open();
            string sqlFavString = "DELETE FROM gcai.VoteModels WHERE PostRefNum=@PostRefNum AND UserID=@UserId AND StarVoted='1'";
            MySqlCommand cmd5 = new MySqlCommand(sqlFavString, conn2);
            cmd5.Parameters.AddWithValue("@PostRefNum", postRefIn);
            cmd5.Parameters.AddWithValue("@UserId", userIdIn);
            cmd5.ExecuteNonQuery();
            conn2.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        return returnString;
    }
    //checks to see if a user has favorited a post and returns true if they have
    public bool CheckFavorite(string postRefIn, string userIdIn)
    {
        bool answere = false;
        string userIdNormalized = userIdIn.ToUpper();
        //delete from FavoritesModel table
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            string sqlFavString = "SELECT * FROM gcai.FavoritesModel WHERE idPostModel = @PostRefNum AND UserId = @UserId";
            MySqlCommand cmd4 = new MySqlCommand(sqlFavString, conn1);
            cmd4.Parameters.AddWithValue("@PostRefNum", postRefIn);
            cmd4.Parameters.AddWithValue("@UserId", userIdNormalized);
            MySqlDataReader reader1 = cmd4.ExecuteReader();
            while (reader1.Read())
            {
                if (reader1.GetValue(0).Equals(postRefIn)) { answere = true; } else { }
            }

            conn1.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return answere;
    }
    //Returns a vote model with the users votes for a given post
    public VoteModel GetUsersVotes(string postRefIn, string userIdIn)
    {
        VoteModel returnVote = new VoteModel();
        List<VoteModel> votesList = new List<VoteModel>();
        try
        {
            MySqlConnection conn12 = new MySqlConnection(connectionString);
            conn12.Open();
            string sqlFavString = "SELECT * FROM gcai.VoteModels WHERE PostRefNum = @PostRefNum AND UserId = @UserId";
            MySqlCommand cmd12 = new MySqlCommand(sqlFavString, conn12);
            cmd12.Parameters.AddWithValue("@PostRefNum", postRefIn);
            cmd12.Parameters.AddWithValue("@UserId", userIdIn);
            MySqlDataReader reader12 = cmd12.ExecuteReader();
            while (reader12.Read())
            {
                
                if (reader12.GetValue(0).Equals("")) { } else if (!reader12.GetValue(0).Equals("")) { returnVote.idVoteModel = reader12.GetValue(0).ToString(); }
                if (reader12.GetValue(1).Equals("")) { } else if (!reader12.GetValue(1).Equals("")) { returnVote.PostRefNum = reader12.GetValue(1).ToString(); }
                if (reader12.GetValue(2).Equals("")) { } else if (!reader12.GetValue(2).Equals("")) { returnVote.UserId = reader12.GetValue(2).ToString(); }
                if (reader12.GetValue(3).Equals("")) { } else if (!reader12.GetValue(3).Equals("")) { returnVote.UpVoted = reader12.GetValue(3).ToString(); }
                if (reader12.GetValue(4).Equals("")) { } else if (!reader12.GetValue(4).Equals("")) { returnVote.DownVoted = reader12.GetValue(4).ToString(); }
                if (reader12.GetValue(5).Equals("")) { } else if (!reader12.GetValue(5).Equals("")) { returnVote.StarVoted = reader12.GetValue(5).ToString(); }
                if (reader12.GetValue(6).Equals("")) { } else if (!reader12.GetValue(6).Equals("")) { returnVote.Flagged = reader12.GetValue(6).ToString(); }
                if (reader12.GetValue(7).Equals("")) { } else if (!reader12.GetValue(7).Equals("")) { returnVote.DateVoted = reader12.GetValue(7).ToString(); }
                
            }
            reader12.Close();
            conn12.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        
        Console.WriteLine(returnVote.ToString());
        return returnVote;
    }
    public void RemoveVote(string postRefIn, string userIdIn, string voteType)
    {
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            string sqlFavString = "DELETE FROM gcai.VoteModels WHERE PostRefNum = @idPostModel AND UserId = @UserId AND " + voteType + " = '1'";
            MySqlCommand cmd4 = new MySqlCommand(sqlFavString, conn1);
            cmd4.Parameters.AddWithValue("@idPostModel", postRefIn);
            cmd4.Parameters.AddWithValue("@UserId", userIdIn);
            cmd4.Parameters.AddWithValue("@VoteType", voteType);
            cmd4.Parameters.AddWithValue("@qty", "1");
            cmd4.ExecuteNonQuery();
            conn1.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
    public string NewTotalPagesPages()
    {
        int postCount = 0;
        try
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            conn1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT COUNT(*) FROM gcai.PostModel", conn1);
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                postCount = int.Parse(reader1.GetString(0));
            }
            reader1.Close();
            conn1.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        Double pageCount = postCount / 10;
        string newTotalPages = pageCount.ToString();

        return newTotalPages;

    }
    public List<string> PostsToRemoveList(int pageNumIn)
    {
        List<string> list = new List<string>();
        List<PostModel> posts = GetPostsByPage(pageNumIn);

        //find post id's that are for the current page and create a removal list of the 20 previos and 20 next to return for removal?
        return list;
    }

}
