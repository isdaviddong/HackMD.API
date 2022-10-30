using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HackMD.API
{
    public class HackMDClient
    {
        private string token;

        private static readonly Lazy<SocketsHttpHandler> s_socketLazy = new Lazy<SocketsHttpHandler>(() =>
        {
            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(10),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                MaxConnectionsPerServer = 10
            };
            return socketsHandler;
        });

        private readonly string _token;

        public HackMDClient(string token)
        {
            this._token = token;
        }

        static HttpClient CreateHttpClient(string token)
        {
            return new HttpClient(s_socketLazy.Value)
            {
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme,
                        token)
                },
                BaseAddress = new Uri("https://api.hackmd.io/v1/")
            };
        }

        #region "User API"

        public User GetUserInformation() => this.GetUserInformationAsync().Result;

        public async Task<User> GetUserInformationAsync(CancellationToken cancel = default)
        {
            var client = CreateHttpClient(this._token);
            string uri = "me";

            var request = CreateDefaultHttpRequest(token, uri);
            var response = await client.SendAsync(request, cancel);
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<User>(responseBody);
            return result;
        }

        #endregion

        #region "User Notes API"

        /// <summary>
        /// Create a note
        /// </summary>
        /// <param name="newHackMDNote"></param>
        /// <returns></returns>
        public NoteResponse CreateNote(Note newHackMDNote) => this.CreateNoteAsync(newHackMDNote).Result;

        public async Task<NoteResponse> CreateNoteAsync(Note newHackMDNote, CancellationToken cancel = default)
        {
            var client = CreateHttpClient(this._token);
            string uri = "notes";

            var requestBody = System.Text.Json.JsonSerializer.Serialize(newHackMDNote);
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // Asynchronously call the REST API method.
            var response = await client.PostAsync(uri, requestContent, cancel);
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<NoteResponse>(responseBody);
            return result;
        }

        public NoteResponse GetNote(string noteId) => this.GetNoteAsync(noteId).Result;

        public async Task<NoteResponse> GetNoteAsync(string noteId)
        {
            var client = CreateHttpClient(this._token);
            string uri = $"notes/{noteId}";

            var response = await client.GetAsync(uri);
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<NoteResponse>(responseBody);
            return result;
        }

        public bool UpdateNote(string noteId, string content, ReadWritePermission readPermission,
            ReadWritePermission writePermission, string permalink) =>
            this.UpdateNoteAsync(noteId, content, readPermission, writePermission, permalink).Result;

        public async Task<bool> UpdateNoteAsync(string noteId, string content, ReadWritePermission readPermission,
            ReadWritePermission writePermission, string permalink)
        {
            var client = CreateHttpClient(this._token);

            string uri = $"notes/{noteId}";

            var requestBody = System.Text.Json.JsonSerializer.Serialize(new
            {
                content,
                readPermission = readPermission.ToString(),
                writePermission = writePermission.ToString(),
                permalink
            });
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await client.PatchAsync(uri, requestContent);
            var responseBody = await response.Content.ReadAsStringAsync();
            return response.StatusCode == System.Net.HttpStatusCode.Accepted;
        }

        /// <summary>
        /// Delete a note
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public bool DeleteNote(string noteId) => this.DeleteNoteAsync(noteId).Result;

        public async Task<bool> DeleteNoteAsync(string noteId)
        {
            var client = CreateHttpClient(this._token);

            string uri = $"notes/{noteId}";

            var response = await client.DeleteAsync(uri);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        #endregion

        private static HttpRequestMessage CreateDefaultHttpRequest(string token, string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Authorization", $"Bearer {token}");
            return request;
        }
    }
}