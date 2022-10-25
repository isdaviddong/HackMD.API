using Microsoft.VisualStudio.TestTools.UnitTesting;
using HackMD.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace HackMD.API.Tests
{
    [TestClass()]
    public class HackMDClinetTests
    {

        private string token = "_____________token_____________";
        private string tempNote = "________NoteId________";

        public HackMDClinetTests()
        {
            var config = System.IO.File.ReadAllText("appsettings.json");
            dynamic para = Newtonsoft.Json.Linq.JObject.Parse(config);
            token = para.token;
            tempNote = para.tempNote;
        }

        [TestMethod()]
        public void CreateNoteTest()
        {
            HackMDClinet c = new HackMDClinet(token);
            var ret = c.CreateNote(
                new Note()
                {
                    title = "test document " + DateTime.UtcNow.AddHours(8),
                    content = "",
                    commentPermission = CommentPermissionPermission.disabled,
                    readPermission = ReadWritePermission.owner,
                    writePermission = ReadWritePermission.owner
                });

            tempNote = ret.id;
            Assert.IsTrue(!string.IsNullOrEmpty(ret.id));
            c.DeleteNote(ret.id);
        }

        [TestMethod()]
        public void DeleteNoteTest()
        {
            HackMDClinet c = new HackMDClinet(token);
            var ret = c.CreateNote(
             new Note()
             {
                 title = "test document " + DateTime.UtcNow.AddHours(8),
                 content = "",
                 commentPermission = CommentPermissionPermission.disabled,
                 readPermission = ReadWritePermission.owner,
                 writePermission = ReadWritePermission.owner
             });

            tempNote = ret.id;
            Assert.IsTrue(c.DeleteNote(tempNote));
        }

        [TestMethod()]
        public void UpdateNoteTest()
        {
            HackMDClinet c = new HackMDClinet(token);
            var ret = c.CreateNote(
           new Note()
           {
               title = "test document " + DateTime.UtcNow.AddHours(8),
               content = "",
               commentPermission = CommentPermissionPermission.disabled,
               readPermission = ReadWritePermission.owner,
               writePermission = ReadWritePermission.owner
           });

            //updated
            var response = c.UpdateNote(ret.id, $"{DateTime.UtcNow.AddHours(8).ToString()} updated", ReadWritePermission.owner, ReadWritePermission.owner, "");
            Assert.IsTrue(response);
            c.DeleteNote(ret.id);
        }
    }
}