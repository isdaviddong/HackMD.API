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
        private string TempNoteId = "________NoteId________";
        private string name = "________name________";
        public HackMDClinetTests()
        {
            var config = System.IO.File.ReadAllText("appsettings.json");
            dynamic para = Newtonsoft.Json.Linq.JObject.Parse(config);
            token = para.token;
            TempNoteId = para.tempNote;
            name = para.name;
        }

        [TestMethod()]
        public void CreateNoteTest()
        {
            // 建立HackMDClinet 物件
            HackMDClinet c = new HackMDClinet(token); //須提供token
            //建立新 Note
            var ret = c.CreateNote(
                new Note()
                {
                    title = "test document " + DateTime.UtcNow.AddHours(8), //標題
                    content = "> content", //內文
                    commentPermission = CommentPermissionPermission.disabled, //comment權限
                    readPermission = ReadWritePermission.owner, //讀取權限
                    writePermission = ReadWritePermission.owner //寫入權限
                });
            //取得 NoteId
            TempNoteId = ret.id;
            Assert.IsTrue(!string.IsNullOrEmpty(ret.id));
            //c.DeleteNote(ret.id);
        }

        [TestMethod()]
        public void DeleteNoteTest()
        {
            // 建立HackMDClinet 物件
            HackMDClinet c = new HackMDClinet(token); //須提供token
            var ret = c.CreateNote(
             new Note()
             {
                 title = "test document " + DateTime.UtcNow.AddHours(8),
                 content = "", //內文
                 commentPermission = CommentPermissionPermission.disabled, //comment權限
                 readPermission = ReadWritePermission.owner, //讀取權限
                 writePermission = ReadWritePermission.owner //寫入權限
             });
            //取得文件id
            TempNoteId = ret.id;
            //刪除文件
            var result = c.DeleteNote(TempNoteId); //文件id
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void UpdateNoteTest()
        {
            // 建立HackMDClinet 物件
            HackMDClinet c = new HackMDClinet(token); //須提供token
            var ret = c.CreateNote(
               new Note()
               {
                   title = "test document " + DateTime.UtcNow.AddHours(8),
                   content = "", //內文
                   commentPermission = CommentPermissionPermission.disabled, //comment權限
                   readPermission = ReadWritePermission.owner, //讀取權限
                   writePermission = ReadWritePermission.owner //寫入權限
               });

            //updated
            var response = c.UpdateNote(
                ret.id,  //文件id
                $"{DateTime.UtcNow.AddHours(8).ToString()} updated", //文件內容
                ReadWritePermission.owner,  //讀取權限
                ReadWritePermission.owner,  //寫入權限
                "" //permalink
                );
            Assert.IsTrue(response);
            c.DeleteNote(ret.id);
        }

        [TestMethod()]
        public void GetNoteTest()
        {
            // 建立HackMDClinet 物件
            HackMDClinet c = new HackMDClinet(token); //須提供token
            //建立文件
            var ret = c.CreateNote(
                new Note()
                {
                    title = "", //標題
                    content = "123", //內文
                    commentPermission = CommentPermissionPermission.disabled, //comment權限
                    readPermission = ReadWritePermission.owner, //讀取權限
                    writePermission = ReadWritePermission.owner //寫入權限
                });
            //取得文件id
            TempNoteId = ret.id;
            //取得文件
            var note = c.GetNote(TempNoteId); //傳入文件id
            Assert.IsTrue(note.content == "123");
            c.DeleteNote(ret.id);
        }

        [TestMethod()]
        public void GetUserInformationTest()
        {
            // 建立HackMDClinet 物件
            HackMDClinet c = new HackMDClinet(token); //須提供token
            var ret = c.GetUserInformation();
            Assert.IsTrue(ret.name.ToString() == this.name);
        }
    }
}