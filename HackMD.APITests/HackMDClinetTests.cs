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
        private string name = "________name________";
        public HackMDClinetTests()
        {
            var config = System.IO.File.ReadAllText("appsettings.json");
            dynamic para = Newtonsoft.Json.Linq.JObject.Parse(config);
            token = para.token;
            tempNote = para.tempNote;
            name = para.name;
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

        [TestMethod()]
        public void GetNoteTest()
        {
            HackMDClinet c = new HackMDClinet(token);
            var ret = c.CreateNote(
                new Note()
                {
                    title = "",
                    content = "123",
                    commentPermission = CommentPermissionPermission.disabled,
                    readPermission = ReadWritePermission.owner,
                    writePermission = ReadWritePermission.owner
                });

            tempNote = ret.id;

            var note = c.GetNote(ret.id);
            Assert.IsTrue(note.content == "123");
            c.DeleteNote(ret.id);
        }

        [TestMethod()]
        public void GetUserInformationTest()
        {
            HackMDClinet c = new HackMDClinet(token);
            var ret = c.GetUserInformation();
            Assert.IsTrue(ret.name.ToString() == this.name);
        }
    }
}