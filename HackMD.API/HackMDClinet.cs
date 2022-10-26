using System;
using System.Net.Http;
using System.Text;

namespace HackMD.API
{
    public class HackMDClinet
    {
        private string token;

        public string Token
        {
            get { return token; }
            set { token = value; }
        }

        public HackMDClinet(string token)
        {
            this.Token = token;
        }

        private string endpoint = "https://api.hackmd.io/v1/";
        #region "User API"
        public User GetUserInformation()
        {
            HttpClient client = new HttpClient();
            string uri = endpoint + $"/me";

            // Request headers.
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.Token}");
            var response = client.GetAsync(uri).Result;
            var ResponseBody = response.Content.ReadAsStringAsync().Result;
            var UserObj = System.Text.Json.JsonSerializer.Deserialize<User>(ResponseBody);
            return UserObj;
        }
        #endregion

        #region "User Notes API"
        /// <summary>
        /// Create a note
        /// </summary>
        /// <param name="newHackMDNote"></param>
        /// <returns></returns>
        public NoteResponse CreateNote(Note newHackMDNote)
        {
            HttpClient client = new HttpClient();
            string uri = endpoint + "/notes";

            // Request headers.
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.Token}");
            var JSON = System.Text.Json.JsonSerializer.Serialize(newHackMDNote);
            var content = new StringContent(JSON, Encoding.UTF8, "application/json");
            // Asynchronously call the REST API method.
            var response = client.PostAsync(uri, content).Result;
            var ResponseBody = response.Content.ReadAsStringAsync().Result;
            var CreateNoteResponseObj = System.Text.Json.JsonSerializer.Deserialize<NoteResponse>(ResponseBody);
            return CreateNoteResponseObj;
        }
        public NoteResponse GetNote(string noteId)
        {
            HttpClient client = new HttpClient();
            string uri = endpoint + $"/notes/{noteId}";

            // Request headers.
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.Token}");
            var response = client.GetAsync(uri).Result;
            var ResponseBody = response.Content.ReadAsStringAsync().Result;
            var CreateNoteResponseObj = System.Text.Json.JsonSerializer.Deserialize<NoteResponse>(ResponseBody);
            return CreateNoteResponseObj;
        }
        public bool UpdateNote(string noteId, string content, ReadWritePermission readPermission, ReadWritePermission writePermission, string permalink)
        {
            HttpClient client = new HttpClient();
            string uri = endpoint + $"/notes/{noteId}";

            var obj = new
            {
                content = content,
                readPermission = readPermission.ToString(),
                writePermission = writePermission.ToString(),
                permalink = permalink
            };

            // Request headers.
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.Token}");
            var JSON = System.Text.Json.JsonSerializer.Serialize(obj);
            var body = new StringContent(JSON, Encoding.UTF8, "application/json");
            var response = client.PatchAsync(uri, body).Result;
            var ResponseBody = response.Content.ReadAsStringAsync().Result;
            return response.StatusCode == System.Net.HttpStatusCode.Accepted;
        }

        /// <summary>
        /// Delete a note
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public bool DeleteNote(string noteId)
        {
            HttpClient client = new HttpClient();
            string uri = endpoint + $"/notes/{noteId}";

            // Request headers.
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.Token}");

            var response = client.DeleteAsync(uri).Result;
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
        #endregion
    }
}
