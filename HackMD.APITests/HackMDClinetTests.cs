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

        const string token = "________________token____________________";
        private string tempNote = "_______Test_doc_id___________";

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
        }

        [TestMethod()]
        public void DeleteNoteTest()
        {
            HackMDClinet c = new HackMDClinet(token);
            var ret = c.DeleteNote(tempNote);
            Assert.IsTrue(ret);
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