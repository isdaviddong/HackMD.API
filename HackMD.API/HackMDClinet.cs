using System;
using System.Net.Http;
using System.Text;

namespace HackMD.API
{
    public class HackMDClient
    {
        private string token;

        private readonly Lazy<SocketsHttpHandler> s_socketLazy = new Lazy<SocketsHttpHandler>(() =>
        {
            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(10),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                MaxConnectionsPerServer = 10
            };
            return socketsHandler;
        });

        public string Token
        {
            get { return token; }
            set { token = value; }
        }

        public HackMDClient(string token)
        {
            this.Token = token;
        }

        HttpClient CreateHttpClient()
        {
            return new HttpClient(this.s_socketLazy.Value)
            {
                BaseAddress = new Uri("https://api.hackmd.io/v1/")
            };
        }

        #region "User API"

        public User GetUserInformation()
        {
            using HttpClient client = this.CreateHttpClient();
            string uri = "me";

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
            using HttpClient client = this.CreateHttpClient();
            string uri = "notes";

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
            using HttpClient client = this.CreateHttpClient();
            string uri =  $"notes/{noteId}";

            // Request headers.
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.Token}");
            var response = client.GetAsync(uri).Result;
            var ResponseBody = response.Content.ReadAsStringAsync().Result;
            var CreateNoteResponseObj = System.Text.Json.JsonSerializer.Deserialize<NoteResponse>(ResponseBody);
            return CreateNoteResponseObj;
        }

        public bool UpdateNote(string noteId, string content, ReadWritePermission readPermission,
            ReadWritePermission writePermission, string permalink)
        {
            using HttpClient client = this.CreateHttpClient();

            string uri =  $"notes/{noteId}";

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
            using HttpClient client = this.CreateHttpClient();

            string uri =  $"notes/{noteId}";

            // Request headers.
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.Token}");

            var response = client.DeleteAsync(uri).Result;
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        #endregion
    }
}