using System;
using System.Collections.Generic;
using System.Text;

namespace HackMD.API
{
    public enum ReadWritePermission
    {
        owner, signed_in, guest
    }

    public enum CommentPermissionPermission
    {
     disabled, forbidden, owners, signed_in_users, everyone
    }

    public class Note
    {
        public string title { get; set; }
        public string content { get; set; }
        public ReadWritePermission readPermission { get; set; }
        public ReadWritePermission writePermission { get; set; }
        public CommentPermissionPermission commentPermission { get; set; }
    }

    public class User
    {
        public string name { get; set; }
        public string photo { get; set; }
        public object biography { get; set; }
        public string userPath { get; set; }
    }

    public class NoteResponse
    {
        public string id { get; set; }
        public string title { get; set; }
        public List<string> tags { get; set; }
        public long createdAt { get; set; }
        public string publishType { get; set; }
        public object publishedAt { get; set; }
        public object permalink { get; set; }
        public string shortId { get; set; }
        public string content { get; set; }
        public long lastChangedAt { get; set; }
        public User lastChangeUser { get; set; }
        public string userPath { get; set; }
        public object teamPath { get; set; }
        public string readPermission { get; set; }
        public string writePermission { get; set; }
        public string publishLink { get; set; }
    }

}
