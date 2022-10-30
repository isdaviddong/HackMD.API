HackMD.API
===
[![Build status](https://dev.azure.com/twDevOpsLabs/HackMD.API/_apis/build/status/HackMD.API-ASP.NET%20Core-CI)](https://dev.azure.com/twDevOpsLabs/HackMD.API/_build/latest?definitionId=103)
[![package](https://img.shields.io/nuget/v/HackMD.API)](https://www.nuget.org/packages/HackMD.API)


## HackMD C# SDK  
> 支援版本: .net core 3.1+ 以上環境
> 請先 
>    1. 申請  HackMd API token
>    2. 在專案中引用套件

### 如何使用API:  
請先申請 HackMd API token:   
參考官方文件  https://hackmd.io/@hackmd-api/how-to-issue-an-api-token

🔥dotnet core 引用套件(VS Code/CLI):
```
dotnet add package HackMD.API 
```
🔥Visual Studio 引用套件:
<img src='https://i.imgur.com/jNVpKeU.png' />

### === 功能範例 ===
👉建立新文件
```cs
// 建立HackMDClient 物件
HackMDClient c = new HackMDClient(token); //須提供token
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
tempNote = ret.id;
```

👉取得文件
```cs
// 建立HackMDClient 物件
HackMDClient c = new HackMDClient(token); //須提供token
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
//Assert.IsTrue(note.content == "123");
```

👉更新文件
```cs
HackMDClient c = new HackMDClient(token);
//更新文件
var response = c.UpdateNote(
    ret.id,  //文件id
    $"{DateTime.UtcNow.AddHours(8).ToString()} updated", //文件內容
    ReadWritePermission.owner,  //讀取權限
    ReadWritePermission.owner,  //寫入權限
    "" //permalink
    );
```

👉刪除文件
```cs
// 建立HackMDClient 物件
HackMDClient c = new HackMDClient(token); //須提供token
//刪除文件
var result = c.DeleteNote(tempNoteId  ); //文件id
```
