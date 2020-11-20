﻿using Matcha.BackgroundService;
using MatrixXamarinApp.Models;
using MatrixXamarinApp.RestAPIClient;
using MatrixXamarinApp.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MatrixXamarinApp.ServicesHandler
{
   public class BackgroundService : IPeriodicTask
    {
        SQLiteAsyncConnection con = new SQLiteAsyncConnection(App.FilePath);
        public TimeSpan Interval { get; set; }

        public RestClient<viewsMessageL> _rest;

        public BackgroundService(int seconds)
        {
            Interval = TimeSpan.FromSeconds(seconds);
        }

        

        public async Task<bool> StartJob()
        {

            RestClient<viewsMessageL> _restClient = new RestClient<viewsMessageL>(); 
            RestClient<ViewsMessageList2> _restClient2 = new RestClient<ViewsMessageList2>();
            var a = "";
            var b = "";

            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                var cmd = conn.CreateCommand(cmdText, typeof(viewsMessageL).Name);
                var cmd2 = conn.CreateCommand(cmdText, typeof(viewsMessageL2).Name);


                if (cmd.ExecuteScalar<string>() != null)
                {
                   // await GetFullMessages();
                    await ViewsMessages(a, b);
                }
                if (cmd2.ExecuteScalar<string>() != null)
                {

                    await ViewsMessages2(a, b);
                }
                }
           
            return true;
        }

        public async Task<bool> ViewsMessages(string dir, string cond)
        {
            var username = await SecureStorage.GetAsync("userName");
            var url = await SecureStorage.GetAsync("url");
            var webguid = await SecureStorage.GetAsync("webGuid");
            var vID = await SecureStorage.GetAsync("ViewID1");
            dir = await SecureStorage.GetAsync("dir1");
            cond = await SecureStorage.GetAsync("con1");
            var a = "";
            var b = "";
            var viewID = 0;
            //var httpClient = new HttpClient();

            try
            {
                
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url + "/Message/viewmessages?userName=" + username + "&" + "webGUID=" + webguid + "&viewID=" + vID + "&direction=" + a + "&condition=" + b);

                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var json = content.ReadAsStringAsync();
                            JObject jwtDynamics = JsonConvert.DeserializeObject<dynamic>(json.Result);
                            var jwtDynamic = JsonConvert.DeserializeObject<ViewsMessageList>(json.Result);
                            viewID = jwtDynamics.Value<int>("ViewID");

                            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))

                            {

                                const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                                var cmd = conn.CreateCommand(cmdText, typeof(ViewsModel).Name);

                                var DB_query = "SELECT * FROM viewsMessageL WHERE ViewID=" + viewID + ";";
                                await con.CreateTableAsync<viewsMessageL>();
                                SQLiteAsyncConnection.ResetPool();
                                var vid = from s in conn.Table<viewsMessageL>()
                                          where s.ViewID == viewID
                                          select s;

                                if (jwtDynamic.INC == null && jwtDynamic.OUT == null)
                                {

                                    try
                                    {

                                        if (vid.Count() <= 0)
                                        {
                                            viewsMessageL viewsMessageList = new viewsMessageL()
                                            {
                                                ViewID = viewID
                                            };
                                            await con.CreateTableAsync<viewsMessageL>();
                                            int rowsAdded = await con.InsertAsync(viewsMessageList);
                                            SQLiteAsyncConnection.ResetPool();
                                        }
                                    }

                                    catch { }


                                }

                                else if (jwtDynamic.INC.Count > 0 && jwtDynamic.OUT == null)
                                {
                                    var count = 0;
                                    foreach (var item in jwtDynamic.INC)
                                    {
                                        var inc = from s in conn.Table<viewsMessageL>()
                                                  where s.INC == item
                                                  select s;
                                        if (inc.Count() <= 0 && count < 20)
                                        {
                                            viewsMessageL viewsMessageList = new viewsMessageL()
                                            {
                                                ViewID = viewID,
                                                INC = item
                                            };

                                            await con.CreateTableAsync<viewsMessageL>();
                                            int rowsAdded = await con.InsertAsync(viewsMessageList);
                                            SQLiteAsyncConnection.ResetPool();
                                            count++;
                                        }

                                    }

                                }

                                else if (jwtDynamic.OUT.Count > 0 && jwtDynamic.INC == null)
                                {
                                    var count = 0;
                                    foreach (var item in jwtDynamic.INC)
                                    {
                                        var inc = from s in conn.Table<viewsMessageL>()
                                                  where s.OUT == item
                                                  select s;
                                        if (inc.Count() <= 0 && count < 20)
                                        {
                                            viewsMessageL viewsMessageList = new viewsMessageL()
                                            {
                                                ViewID = viewID,
                                                OUT = item
                                            };

                                            await con.CreateTableAsync<viewsMessageL>();
                                            int rowsAdded = await con.InsertAsync(viewsMessageList);
                                            SQLiteAsyncConnection.ResetPool();
                                            count++;
                                        }
                                    }

                                }

                                else if (jwtDynamic.INC.Count > 0 && jwtDynamic.OUT.Count > 0)
                                {
                                    viewsMessageL viewsMessageList = new viewsMessageL();
                                    var count = 0;
                                    var count2 = 0;
                                    foreach (var item in jwtDynamic.INC)
                                    {
                                        var inc = from s in conn.Table<viewsMessageL>()
                                                  where s.INC == item
                                                  select s;

                                        if (inc.Count() <= 0 && count < 20)
                                        {
                                            viewsMessageList.ViewID = viewID;
                                            viewsMessageList.INC = item;

                                            await con.CreateTableAsync<viewsMessageL>();
                                            int rowsAdded = await con.InsertAsync(viewsMessageList);
                                            SQLiteAsyncConnection.ResetPool();
                                            count++;
                                        }
                                    }

                                    foreach (var item in jwtDynamic.OUT)
                                    {
                                        var inc = from s in conn.Table<viewsMessageL>()
                                                  where s.OUT == item
                                                  select s;

                                        if (inc.Count() <= 0 && count2 < 20)
                                        {
                                            viewsMessageList.ViewID = viewID;
                                            viewsMessageList.OUT = item;

                                            await  con.CreateTableAsync<viewsMessageL>();
                                            int rowsAdded = await con.InsertAsync(viewsMessageList);
                                            SQLiteAsyncConnection.ResetPool();
                                            count2++;
                                        }
                                    }

                                }

                            }


                        }
                        var ab = "";
                        var bb = "";

                        await GetMessages(ab, bb);
                        await GetFullMessages();
                        return response.IsSuccessStatusCode;
                    }

                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public async Task<bool> ViewsMessages2(string dir, string cond)
        {
            var username = await SecureStorage.GetAsync("userName");
            var url = await SecureStorage.GetAsync("url");
            var webguid = await SecureStorage.GetAsync("webGuid");
            var vID = await SecureStorage.GetAsync("ViewID2");
            dir = await SecureStorage.GetAsync("dir2");
            cond = await SecureStorage.GetAsync("con2");
            var ab = "";
            var bb = "";
            var viewID = 0;
            //var httpClient = new HttpClient();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url + "/Message/viewmessages?userName=" + username + "&" + "webGUID=" + webguid + "&viewID=" + vID + "&direction=" + ab + "&condition=" + bb);

                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var json = content.ReadAsStringAsync();
                            JObject jwtDynamics = JsonConvert.DeserializeObject<dynamic>(json.Result);
                            var jwtDynamic = JsonConvert.DeserializeObject<ViewsMessageList2>(json.Result);
                            viewID = jwtDynamics.Value<int>("ViewID");

                            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                            {

                                const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                                var cmd = conn.CreateCommand(cmdText, typeof(ViewsModel2).Name);

                                var DB_query = "SELECT * FROM viewsMessageL WHERE ViewID=" + viewID + ";";
                                await con.CreateTableAsync<viewsMessageL2>();
                                SQLiteAsyncConnection.ResetPool();
                                var vid = from s in conn.Table<viewsMessageL2>()
                                          where s.ViewID == viewID
                                          select s;

                                if (jwtDynamic.INC == null && jwtDynamic.OUT == null)
                                {

                                    try
                                    {
                                        if (vid.Count() <= 0)
                                        {
                                            viewsMessageL2 viewsMessageList = new viewsMessageL2()
                                            {
                                                ViewID = viewID
                                            };
                                            await con.CreateTableAsync<viewsMessageL2>();
                                            int rowsAdded = conn.Insert(viewsMessageList);
                                            SQLiteAsyncConnection.ResetPool();
                                        }
                                    }

                                    catch { }


                                }

                                else if (jwtDynamic.INC.Count > 0 && jwtDynamic.OUT == null)
                                {

                                    foreach (var item in jwtDynamic.INC)
                                    {
                                        var inc = from s in conn.Table<viewsMessageL2>()
                                                  where s.INC == item
                                                  select s;
                                        if (inc.Count() <= 0)
                                        {
                                            viewsMessageL2 viewsMessageList = new viewsMessageL2()
                                            {
                                                ViewID = viewID,
                                                INC = item
                                            };

                                            await con.CreateTableAsync<viewsMessageL2>();
                                            int rowsAdded = await con.InsertAsync(viewsMessageList);
                                            SQLiteAsyncConnection.ResetPool();
                                        }

                                    }

                                }

                                else if (jwtDynamic.OUT.Count > 0 && jwtDynamic.INC == null)
                                {
                                    foreach (var item in jwtDynamic.INC)
                                    {
                                        var inc = from s in conn.Table<viewsMessageL2>()
                                                  where s.OUT == item
                                                  select s;
                                        if (inc.Count() <= 0)
                                        {
                                            viewsMessageL2 viewsMessageList = new viewsMessageL2()
                                            {
                                                ViewID = viewID,
                                                OUT = item
                                            };

                                            await con.CreateTableAsync<viewsMessageL2>();
                                            int rowsAdded = await con.InsertAsync(viewsMessageList);
                                            SQLiteAsyncConnection.ResetPool();
                                        }
                                    }

                                }

                                else if (jwtDynamic.INC.Count > 0 && jwtDynamic.OUT.Count > 0)
                                {
                                    viewsMessageL2 viewsMessageList = new viewsMessageL2();

                                    foreach (var item in jwtDynamic.INC)
                                    {
                                        var inc = from s in conn.Table<viewsMessageL2>()
                                                  where s.INC == item
                                                  select s;
                                        if (inc.Count() <= 0)
                                        {
                                            viewsMessageList.ViewID = viewID;
                                            viewsMessageList.INC = item;

                                            await con.CreateTableAsync<viewsMessageL2>();
                                            int rowsAdded = await con.InsertAsync(viewsMessageList);
                                            SQLiteAsyncConnection.ResetPool();
                                        }
                                    }

                                    foreach (var item in jwtDynamic.OUT)
                                    {
                                        var inc = from s in conn.Table<viewsMessageL2>()
                                                  where s.OUT == item
                                                  select s;
                                        if (inc.Count() <= 0)
                                        {
                                            viewsMessageList.ViewID = viewID;
                                            viewsMessageList.OUT = item;

                                            await con.CreateTableAsync<viewsMessageL2>();
                                            int rowsAdded = await con.InsertAsync(viewsMessageList);
                                            SQLiteAsyncConnection.ResetPool();
                                        }
                                    }

                                }

                            }


                        }
                        var a = "";
                        var b = "";
                        await GetMessages2(a, b);
                        await GetFullMessages2();
                        return response.IsSuccessStatusCode;
                    }

                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public async Task<bool> GetMessages(string messageID, string messageDirection)
        {
            List<GetMessages> jwtDynamic = new List<GetMessages>();
            var username = await SecureStorage.GetAsync("userName");
            var url = await SecureStorage.GetAsync("url");
            var webguid = await SecureStorage.GetAsync("webGuid");

            try
            {
                viewsMessageL viewsMessageL = new viewsMessageL();
                var messageIdINC = "";
                var messageIdOUT = "";
                var ac = "";
                var bc = String.Empty;
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    var message = await con.Table<viewsMessageL>().ToListAsync();
                    var Getmessage = await con.Table<GetMessages>().ToListAsync();
                    GetMessages mess = new GetMessages();
                    var count = 0;
                    var count2 = 0;
                    foreach (var item in message)
                    {

                        if (item.INC != null && item.OUT == null)
                        {

                            var mes = await con.Table<GetMessages>()
                                       .Where(x => x.MessageId == item.INC).FirstOrDefaultAsync();

                            if (mes == null && count < 10)
                            {
                                count++;
                                var a = item.INC;
                                ac += a + ",";
                            }

                        }
                        if (item.OUT != null)
                        {
                            var mes = await con.Table<GetMessages>()
                                      .Where(x => x.MessageId == item.OUT)
                                       .FirstOrDefaultAsync();
                            if (mes == null && count2 < 10)
                            {
                                count2++;
                                var b = item.OUT;
                                bc += b + ",";
                            }
                        }


                    }

                    messageIdINC = ac.TrimEnd(',');
                    await SecureStorage.SetAsync("messageIdINC", messageIdINC);
                    var m = bc.TrimEnd(',');
                    messageIdOUT = m.TrimStart(',');
                    await SecureStorage.SetAsync("messageIdOUT", messageIdOUT);
                }
                using (var client = new HttpClient())
                {
                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                        var cmd = conn.CreateCommand(cmdText, typeof(GetMessages).Name);


                        if (cmd.ExecuteScalar<string>() != null)
                        {

                            using (var clients = new HttpClient())
                            {
                                var formFields = new Dictionary<string, object>(); //
                                formFields["FormId"] = "/Message/Get?msgIds=" + messageIdINC + "&messageDir=INC&userName=" + username + "&webGuid=" + webguid + "&msgPart=0";

                                var content = new ObjectContent(typeof(object), formFields, new JsonMediaTypeFormatter(),
                                new MediaTypeHeaderValue("application/json"));
                                var result = clients.PostAsync(url, content).Result;
                                var conten = await result.Content.ReadAsStringAsync();
                                jwtDynamic = JsonConvert.DeserializeObject<List<GetMessages>>(conten);

                            }


                            if (messageIdINC != null)
                            {
                                var response = client.GetAsync(url + "/Message/Get?msgIds=" + messageIdINC + "&messageDir=INC&userName=" + username + "&webGuid=" + webguid + "&msgPart=0").Result;
                                //var response = await httpClient.GetAsync(url + "/Message/Get?msgIds=" + messageID + "&messageDir=" + messageDirection + "&userName=" + username + "&webGuid=" + webguid + "&msgPart=0");
                                await SecureStorage.SetAsync("MessageID1", messageID);
                                await SecureStorage.SetAsync("MessageDirection1", messageDirection);

                                if (response.IsSuccessStatusCode)
                                {

                                    var responseContent = await response.Content.ReadAsStringAsync();
                                    jwtDynamic = JsonConvert.DeserializeObject<List<GetMessages>>(responseContent);
                                    GetMessages getMessages = new GetMessages();


                                    for (var i = 0; i < jwtDynamic.Count; i++)
                                    {
                                        getMessages = new GetMessages()
                                        {
                                            MessageId = jwtDynamic[i].MessageId,
                                            Subject = jwtDynamic[i].Subject,
                                            CreatedTime = jwtDynamic[i].CreatedTime,
                                            IO = "I",
                                            From = jwtDynamic[i].OrigFrom,
                                            Owner = jwtDynamic[i].Receipient

                                        };
                                        await con.CreateTableAsync<GetMessages>();
                                        int rowsAdded = await con.InsertAsync(getMessages);
                                        SQLiteAsyncConnection.ResetPool();


                                    }




                                }
                            }

                                if (messageIdOUT != null)
                                {
                                    var response2 = client.GetAsync(url + "/Message/Get?msgIds=" + messageIdOUT + "&messageDir=OUT&userName=" + username + "&webGuid=" + webguid + "&msgPart=0").Result;
                                    //var response = await httpClient.GetAsync(url + "/Message/Get?msgIds=" + messageID + "&messageDir=" + messageDirection + "&userName=" + username + "&webGuid=" + webguid + "&msgPart=0");
                                    await SecureStorage.SetAsync("MessageID1", messageID);
                                    await SecureStorage.SetAsync("MessageDirection1", messageDirection);

                                    if (response2.IsSuccessStatusCode)
                                    {

                                        var responseContent = await response2.Content.ReadAsStringAsync();
                                        jwtDynamic = JsonConvert.DeserializeObject<List<GetMessages>>(responseContent);
                                        GetMessages getMessages = new GetMessages();


                                        for (var i = 0; i < jwtDynamic.Count; i++)
                                        {
                                            getMessages = new GetMessages()
                                            {
                                                MessageId = jwtDynamic[i].MessageId,
                                                Subject = jwtDynamic[i].Subject,
                                                CreatedTime = jwtDynamic[i].CreatedTime,
                                                IO = "O",
                                                From = jwtDynamic[i].Receipients,
                                                Owner = jwtDynamic[i].From

                                            };
                                            await con.CreateTableAsync<GetMessages>();
                                            int rowsAdded = await con.InsertAsync(getMessages);
                                            SQLiteAsyncConnection.ResetPool();


                                    }


                                    }
                                }


                        }

                    }

                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> GetMessages2(string messageID, string messageDirection)
        {
            List<GetMessages2> jwtDynamic = new List<GetMessages2>();
            var username = await SecureStorage.GetAsync("userName");
            var url = await SecureStorage.GetAsync("url");
            var webguid = await SecureStorage.GetAsync("webGuid");

            try
            {
                viewsMessageL2 viewsMessageL = new viewsMessageL2();
                var messageIdINC = "";
                var messageIdOUT = "";
                var ac = "";
                var bc = String.Empty;
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    var message = await con.Table<viewsMessageL2>().ToListAsync();
                    var Getmessage = await con.Table<GetMessages2>().ToListAsync();
                    GetMessages2 mess = new GetMessages2();
                    var count = 0;
                    var count2 = 0;
                    foreach (var item in message)
                    {

                        if (item.INC != null && item.OUT == null)
                        {

                            var mes = await con.Table<GetMessages2>()
                                       .Where(x => x.MessageId == item.INC).FirstOrDefaultAsync();

                            if (mes == null && count < 10)
                            {
                                count++;
                                var a = item.INC;
                                ac += a + ",";
                            }

                        }
                        if (item.OUT != null)
                        {
                            var mes = await con.Table<GetMessages2>()
                                      .Where(x => x.MessageId == item.OUT)
                                       .FirstOrDefaultAsync();
                            if (mes == null && count2 < 10)
                            {
                                count2++;
                                var b = item.OUT;
                                bc += b + ",";
                            }
                        }


                    }

                    messageIdINC = ac.TrimEnd(',');
                    await SecureStorage.SetAsync("messageIdINC2", messageIdINC);
                    var m = bc.TrimEnd(',');
                    messageIdOUT = m.TrimStart(',');
                    await SecureStorage.SetAsync("messageIdOUT2", messageIdOUT);
                }
                using (var client = new HttpClient())
                {
                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                        var cmd = conn.CreateCommand(cmdText, typeof(GetMessages2).Name);


                        //if (cmd.ExecuteScalar<string>() != null)
                        //{

                            using (var clients = new HttpClient())
                            {
                                var formFields = new Dictionary<string, object>(); //
                                formFields["FormId"] = "/Message/Get?msgIds=" + messageIdINC + "&messageDir=INC&userName=" + username + "&webGuid=" + webguid + "&msgPart=0";

                                var content = new ObjectContent(typeof(object), formFields, new JsonMediaTypeFormatter(),
                                new MediaTypeHeaderValue("application/json"));
                                var result = clients.PostAsync(url, content).Result;
                                var conten = await result.Content.ReadAsStringAsync();
                                jwtDynamic = JsonConvert.DeserializeObject<List<GetMessages2>>(conten);

                            }


                        if (messageIdINC != null)
                        {
                            var response = client.GetAsync(url + "/Message/Get?msgIds=" + messageIdINC + "&messageDir=INC&userName=" + username + "&webGuid=" + webguid + "&msgPart=0").Result;
                            //var response = await httpClient.GetAsync(url + "/Message/Get?msgIds=" + messageID + "&messageDir=" + messageDirection + "&userName=" + username + "&webGuid=" + webguid + "&msgPart=0");
                            await SecureStorage.SetAsync("MessageID1", messageID);
                            await SecureStorage.SetAsync("MessageDirection1", messageDirection);

                            if (response.IsSuccessStatusCode)
                            {

                                var responseContent = await response.Content.ReadAsStringAsync();
                                jwtDynamic = JsonConvert.DeserializeObject<List<GetMessages2>>(responseContent);
                                GetMessages2 getMessages = new GetMessages2();


                                for (var i = 0; i < jwtDynamic.Count; i++)
                                {
                                    getMessages = new GetMessages2()
                                    {
                                        MessageId = jwtDynamic[i].MessageId,
                                        Subject = jwtDynamic[i].Subject,
                                        CreatedTime = jwtDynamic[i].CreatedTime,
                                        IO = "I",
                                        From = jwtDynamic[i].OrigFrom,
                                        Owner = jwtDynamic[i].Receipient

                                    };
                                    await con.CreateTableAsync<GetMessages2>();
                                    int rowsAdded = await con.InsertAsync(getMessages);
                                    SQLiteAsyncConnection.ResetPool();


                                }




                            }
                        }

                        if (messageIdOUT != null)
                        {
                            var response2 = client.GetAsync(url + "/Message/Get?msgIds=" + messageIdOUT + "&messageDir=OUT&userName=" + username + "&webGuid=" + webguid + "&msgPart=0").Result;
                            //var response = await httpClient.GetAsync(url + "/Message/Get?msgIds=" + messageID + "&messageDir=" + messageDirection + "&userName=" + username + "&webGuid=" + webguid + "&msgPart=0");
                            await SecureStorage.SetAsync("MessageID1", messageID);
                            await SecureStorage.SetAsync("MessageDirection1", messageDirection);

                            if (response2.IsSuccessStatusCode)
                            {

                                var responseContent = await response2.Content.ReadAsStringAsync();
                                jwtDynamic = JsonConvert.DeserializeObject<List<GetMessages2>>(responseContent);
                                GetMessages2 getMessages = new GetMessages2();


                                for (var i = 0; i < jwtDynamic.Count; i++)
                                {
                                    getMessages = new GetMessages2()
                                    {
                                        MessageId = jwtDynamic[i].MessageId,
                                        Subject = jwtDynamic[i].Subject,
                                        CreatedTime = jwtDynamic[i].CreatedTime,
                                        IO = "O",
                                        From = jwtDynamic[i].Receipients,
                                        Owner = jwtDynamic[i].From

                                    };
                                    await con.CreateTableAsync<GetMessages2>();
                                    int rowsAdded = await con.InsertAsync(getMessages);
                                    SQLiteAsyncConnection.ResetPool();


                                }


                            }
                        }

                          
                    }

                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> GetFullMessages()
        {
            List<GetFullMessages> jwtDynamic = new List<GetFullMessages>();
            var username = await SecureStorage.GetAsync("userName");
            var url = await SecureStorage.GetAsync("url");
            var webguid = await SecureStorage.GetAsync("webGuid");

            var messageIdINC = await SecureStorage.GetAsync("messageIdINC");
            var messageIdOUT = await SecureStorage.GetAsync("messageIdOUT");
            var messageDirection = await SecureStorage.GetAsync("MessageDirection1");

            var httpClient = new HttpClient();

            try
            {
                using (var client = new HttpClient())
                {
                    //-----

                        GetMessages viewsMessageL = new GetMessages();
                    var messageIdINCFull = "";
                    var messageIdOUTFull = "";
                    var ac = "";
                    var bc = String.Empty;
                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        var message = await con.Table<GetMessages>().ToListAsync();
                        var Getmessage = await con.Table<GetFullMessages>().ToListAsync();
                        GetFullMessages mess = new GetFullMessages();
                        var count = 0;
                        var count2 = 0;
                        foreach (var item in message)
                        {

                       
                                var mes = await con.Table<GetFullMessages>()
                                           .Where(x => x.MessageId == item.MessageId).FirstOrDefaultAsync();

                                if (mes == null && count < 10 && item.IO == "I")
                                {
                                    count++;
                                    var a = item.MessageId;
                                    ac += a + ",";
                                }
                       
                                var mesg = await con.Table<GetFullMessages>()
                                          .Where(x => x.MessageId == item.MessageId)
                                           .FirstOrDefaultAsync();
                                if (mesg == null && count2 < 10 && item.IO == "O")
                                {
                                    count2++;
                                    var b = item.MessageId;
                                    bc += b + ",";
                                }


                        }

                        messageIdINCFull = ac.TrimEnd(',');
                        await SecureStorage.SetAsync("messageIdINC", messageIdINCFull);
                        var m = bc.TrimEnd(',');
                        messageIdOUTFull = m.TrimStart(',');
                        await SecureStorage.SetAsync("messageIdOUT", messageIdOUTFull);
                    }

                    //-----




                   

                    var response = await client.GetAsync(url + "/Message/Get?msgIds=" + messageIdINCFull + "&messageDir=INC&userName=" + username + "&webGuid=" + webguid + "&msgPart=1");
                    //var response2 = client.GetAsync(url + "/Message/Get?msgIds=" + messageIdOUT + "&messageDir=OUT&userName=" + username + "&webGuid=" + webguid + "&msgPart=1").Result;
                    //var response = await httpClient.GetAsync("http://webapi.sd-matrix.com:5000/Message/Get?msgIds=1395692&messageDir=INC&userName=calebo&webGuid=141368b8-a89b-48f1-9c0d-c805875bd443&msgPart=1");
                    //var response = await httpClient.GetAsync(url + "/Message/Get?msgIds=" + messageID + "&messageDir=" + messageDirection + "&userName=" + username + "&webGuid=" + webguid + "&msgPart=1");



                    if (response.IsSuccessStatusCode)
                    {


                        var responseContent = await response.Content.ReadAsStringAsync();
                        jwtDynamic = JsonConvert.DeserializeObject<List<GetFullMessages>>(responseContent);
                        GetFullMessages getMessages = new GetFullMessages();

                        using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                        {
                            const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                            var cmd = conn.CreateCommand(cmdText, typeof(GetFullMessages).Name);

                            //--------


                            //--------

                            if (cmd.ExecuteScalar<string>() != null)
                            {
                                for (var i = 0; i < jwtDynamic.Count; i++)
                                {
                                    getMessages = new GetFullMessages()
                                    {
                                        MessageId = jwtDynamic[i].MessageId,
                                        EML = jwtDynamic[i].EML

                                    };
                                    await con.CreateTableAsync<GetFullMessages>();
                                    int rowsAdded = await con.InsertAsync(getMessages);
                                    SQLiteAsyncConnection.ResetPool();   

                                }


                            }
                            else
                            {
                                for (var i = 0; i < jwtDynamic.Count; i++)
                                {
                                    getMessages = new GetFullMessages()
                                    {
                                        MessageId = jwtDynamic[i].MessageId,
                                        EML = jwtDynamic[i].EML

                                    };
                                    await con.CreateTableAsync<GetFullMessages>();
                                    int rowsAdded = await con.InsertAsync(getMessages);
                                    SQLiteAsyncConnection.ResetPool();
                                }
                            }
                        }


                        //}


                    }

                    var response2 = await client.GetAsync(url + "/Message/Get?msgIds=" + messageIdOUTFull + "&messageDir=OUT&userName=" + username + "&webGuid=" + webguid + "&msgPart=1");

                    if (response2.IsSuccessStatusCode)

                        {


                            var responseContent = await response2.Content.ReadAsStringAsync();
                            jwtDynamic = JsonConvert.DeserializeObject<List<GetFullMessages>>(responseContent);
                            GetFullMessages getMessages = new GetFullMessages();

                            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                            {
                                const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                                var cmd = conn.CreateCommand(cmdText, typeof(GetFullMessages).Name);

                                if (response2.IsSuccessStatusCode && response.IsSuccessStatusCode == false)
                                {
                                    if (cmd.ExecuteScalar<string>() != null)
                                    {
                                        for (var i = 0; i < jwtDynamic.Count; i++)
                                        {
                                            getMessages = new GetFullMessages()
                                            {
                                                MessageId = jwtDynamic[i].MessageId,
                                                EML = jwtDynamic[i].EML

                                            };
                                            await con.CreateTableAsync<GetFullMessages>();
                                            int rowsAdded = await con.InsertAsync(getMessages);
                                            SQLiteAsyncConnection.ResetPool();
                                    }



                                    }
                                }
                                if (response2.IsSuccessStatusCode && response.IsSuccessStatusCode)
                                {
                                    if (cmd.ExecuteScalar<string>() != null)
                                    {
                                        for (var i = 0; i < jwtDynamic.Count; i++)
                                        {

                                            getMessages = new GetFullMessages()
                                            {
                                                MessageId = jwtDynamic[i].MessageId,
                                                EML = jwtDynamic[i].EML

                                            };
                                            await con.CreateTableAsync<GetFullMessages>();
                                            int rowsAdded = await con.InsertAsync(getMessages);
                                            SQLiteAsyncConnection.ResetPool();

                                    }

                                    }
                                }

                                else
                                {
                                    for (var i = 0; i < jwtDynamic.Count; i++)
                                    {

                                        getMessages = new GetFullMessages()
                                        {
                                            MessageId = jwtDynamic[i].MessageId,
                                            EML = jwtDynamic[i].EML

                                        };
                                        await con.CreateTableAsync<GetFullMessages>();
                                        int rowsAdded = await con.InsertAsync(getMessages);
                                        SQLiteAsyncConnection.ResetPool();

                                }
                                }

                            }


                            //}


                    }
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }


        }

        public async Task<bool> GetFullMessages2()
        {
            List<GetFullMessages2> jwtDynamic = new List<GetFullMessages2>();
            var username = await SecureStorage.GetAsync("userName");
            var url = await SecureStorage.GetAsync("url");
            var webguid = await SecureStorage.GetAsync("webGuid");

            var messageIdINC = await SecureStorage.GetAsync("messageIdINC2");
            var messageIdOUT = await SecureStorage.GetAsync("messageIdOUT2");
            //var messageDirection = await SecureStorage.GetAsync("MessageDirection1");

            //var httpClient = new HttpClient();

            try
            {


                using (var client = new HttpClient())
                {

                    //-----

                    GetMessages2 viewsMessageL = new GetMessages2();
                    var messageIdINCFull = "";
                    var messageIdOUTFull = "";
                    var ac = "";
                    var bc = String.Empty;
                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        var message = await con.Table<GetMessages2>().ToListAsync();
                        var Getmessage = await con.Table<GetFullMessages2>().ToListAsync();
                        GetFullMessages2 mess = new GetFullMessages2();
                        var count = 0;
                        var count2 = 0;
                        foreach (var item in message)
                        {


                            var mes = await con.Table<GetFullMessages2>()
                                       .Where(x => x.MessageId == item.MessageId).FirstOrDefaultAsync();

                            if (mes == null && count < 10 && item.IO == "I")
                            {
                                count++;
                                var a = item.MessageId;
                                ac += a + ",";
                            }

                            var mesg = await con.Table<GetFullMessages2>()
                                      .Where(x => x.MessageId == item.MessageId)
                                       .FirstOrDefaultAsync();
                            if (mesg == null && count2 < 10 && item.IO == "O")
                            {
                                count2++;
                                var b = item.MessageId;
                                bc += b + ",";
                            }


                        }

                        messageIdINCFull = ac.TrimEnd(',');
                        await SecureStorage.SetAsync("messageIdINC", messageIdINCFull);
                        var m = bc.TrimEnd(',');
                        messageIdOUTFull = m.TrimStart(',');
                        await SecureStorage.SetAsync("messageIdOUT", messageIdOUTFull);
                    }

                    //-----


                    var response = client.GetAsync(url + "/Message/Get?msgIds=" + messageIdINCFull + "&messageDir=INC&userName=" + username + "&webGuid=" + webguid + "&msgPart=1").Result;
                    //var response2 = client.GetAsync(url + "/Message/Get?msgIds=" + messageIdOUT + "&messageDir=OUT&userName=" + username + "&webGuid=" + webguid + "&msgPart=1").Result;
                    //var response = await httpClient.GetAsync("http://webapi.sd-matrix.com:5000/Message/Get?msgIds=1395692&messageDir=INC&userName=calebo&webGuid=141368b8-a89b-48f1-9c0d-c805875bd443&msgPart=1");
                    //var response = await httpClient.GetAsync(url + "/Message/Get?msgIds=" + messageID + "&messageDir=" + messageDirection + "&userName=" + username + "&webGuid=" + webguid + "&msgPart=1");



                    if (response.IsSuccessStatusCode)
                    {


                        var responseContent = await response.Content.ReadAsStringAsync();
                        jwtDynamic = JsonConvert.DeserializeObject<List<GetFullMessages2>>(responseContent);
                        GetFullMessages2 getMessages = new GetFullMessages2();

                        using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                        {
                            const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                            var cmd = conn.CreateCommand(cmdText, typeof(GetFullMessages2).Name);

                            //--------


                            //--------

                            if (cmd.ExecuteScalar<string>() != null)
                            {
                                for (var i = 0; i < jwtDynamic.Count; i++)
                                {
                                    getMessages = new GetFullMessages2()
                                    {
                                        MessageId = jwtDynamic[i].MessageId,
                                        EML = jwtDynamic[i].EML

                                    };
                                    await con.CreateTableAsync<GetFullMessages2>();
                                    int rowsAdded = await con.InsertAsync(getMessages);
                                    SQLiteAsyncConnection.ResetPool();



                                }


                            }
                            else
                            {
                                for (var i = 0; i < jwtDynamic.Count; i++)
                                {
                                    getMessages = new GetFullMessages2()
                                    {
                                        MessageId = jwtDynamic[i].MessageId,
                                        EML = jwtDynamic[i].EML

                                    };
                                    await con.CreateTableAsync<GetFullMessages2>();
                                    int rowsAdded = await con.InsertAsync(getMessages);
                                    SQLiteAsyncConnection.ResetPool();

                                }
                            }
                        }


                        //}


                    }
                    var response2 = client.GetAsync(url + "/Message/Get?msgIds=" + messageIdOUTFull + "&messageDir=OUT&userName=" + username + "&webGuid=" + webguid + "&msgPart=1").Result;

                    if (response2.IsSuccessStatusCode)
                    {


                        var responseContent = await response2.Content.ReadAsStringAsync();
                        jwtDynamic = JsonConvert.DeserializeObject<List<GetFullMessages2>>(responseContent);
                        GetFullMessages2 getMessages = new GetFullMessages2();

                        using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                        {
                            const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                            var cmd = conn.CreateCommand(cmdText, typeof(GetFullMessages2).Name);

                            if (response2.IsSuccessStatusCode && response.IsSuccessStatusCode == false)
                            {
                                if (cmd.ExecuteScalar<string>() != null)
                                {
                                    for (var i = 0; i < jwtDynamic.Count; i++)
                                    {
                                        getMessages = new GetFullMessages2()
                                        {
                                            MessageId = jwtDynamic[i].MessageId,
                                            EML = jwtDynamic[i].EML

                                        };
                                        await con.CreateTableAsync<GetFullMessages2>();
                                        int rowsAdded = await con.InsertAsync(getMessages);
                                        SQLiteAsyncConnection.ResetPool();


                                    }


                                }
                            }
                            if (response2.IsSuccessStatusCode && response.IsSuccessStatusCode)
                            {
                                if (cmd.ExecuteScalar<string>() != null)
                                {
                                    for (var i = 0; i < jwtDynamic.Count; i++)
                                    {
                                        getMessages = new GetFullMessages2()
                                        {
                                            MessageId = jwtDynamic[i].MessageId,
                                            EML = jwtDynamic[i].EML

                                        };
                                        await con.CreateTableAsync<GetFullMessages2>();
                                        int rowsAdded = await con.InsertAsync(getMessages);
                                        SQLiteAsyncConnection.ResetPool();

                                    }

                                }
                            }

                            else
                            {
                                for (var i = 0; i < jwtDynamic.Count; i++)
                                {
                                    getMessages = new GetFullMessages2()
                                    {
                                        MessageId = jwtDynamic[i].MessageId,
                                        EML = jwtDynamic[i].EML

                                    };
                                    await con.CreateTableAsync<GetFullMessages2>();
                                    int rowsAdded = await con.InsertAsync(getMessages);
                                    SQLiteAsyncConnection.ResetPool();

                                }
                            }

                        }


                        //}


                    }
                    var lastUpdate = DateTime.Now;
                    await SecureStorage.SetAsync("lastUpdate", lastUpdate.ToString());
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }


        }

        public async Task<bool> FullSync()
        {
            var username = await SecureStorage.GetAsync("userName");
            var url = await SecureStorage.GetAsync("url");
            var webguid = await SecureStorage.GetAsync("webGuid");
            var vID = await SecureStorage.GetAsync("ViewID1");
            var a = "";
            var b = "";
            var viewID = 0;
            //var httpClient = new HttpClient();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url + "/Message/viewmessages?userName=" + username + "&" + "webGUID=" + webguid + "&viewID=" + vID + "&direction=" + a + "&condition=" + b);

                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var json = content.ReadAsStringAsync();
                            JObject jwtDynamics = JsonConvert.DeserializeObject<dynamic>(json.Result);
                            var jwtDynamic = JsonConvert.DeserializeObject<ViewsMessageList>(json.Result);
                            viewID = jwtDynamics.Value<int>("ViewID");

                            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                            {

                                //const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                                //var cmd = conn.CreateCommand(cmdText, typeof(ViewsModel).Name);

                                //var DB_query = "SELECT * FROM viewsMessageL WHERE ViewID=" + viewID + ";";
                                //conn.CreateTable<viewsMessageL>();
                                //var vid = from s in conn.Table<viewsMessageL>()
                                //          where s.ViewID == viewID
                                //          select s;



                                if (jwtDynamic.INC.Count > 0 && jwtDynamic.OUT == null)
                                {

                                    var tab = conn.Table<viewsMessageL>().ToList();
                                    foreach (var items in tab)
                                    {
                                        if (!(jwtDynamic.INC.Contains((int)items.INC)))
                                        {
                                            conn.Table<viewsMessageL>().Delete(x => x.INC == items.INC);

                                            var getmess = from s in conn.Table<GetMessages>()
                                                      where s.MessageId == items.INC
                                                      select s;
                                            if (getmess.Count() > 0)
                                            {
                                                conn.Table<GetMessages>().Delete(x => x.MessageId == items.INC);
                                            }

                                            var getFulmess = from s in conn.Table<GetFullMessages>()
                                                          where s.MessageId == items.INC
                                                          select s;
                                            if(getFulmess.Count() > 0)
                                            {
                                                conn.Table<GetFullMessages>().Delete(x => x.MessageId == items.INC);
                                            }
                                            
                                        }

                                    }

                                }

                                else if (jwtDynamic.OUT.Count > 0 && jwtDynamic.INC == null)
                                {
                                    var tab = conn.Table<viewsMessageL>().ToList();
                                    foreach (var items in tab)
                                    {
                                        if (!(jwtDynamic.OUT.Contains((int)items.OUT)))
                                        {
                                            conn.Table<viewsMessageL>().Delete(x => x.INC == items.INC);

                                            var getmess = from s in conn.Table<GetMessages>()
                                                          where s.MessageId == items.INC
                                                          select s;
                                            if (getmess.Count() > 0)
                                            {
                                                conn.Table<GetMessages>().Delete(x => x.MessageId == items.INC);
                                            }

                                            var getFulmess = from s in conn.Table<GetFullMessages>()
                                                             where s.MessageId == items.INC
                                                             select s;
                                            if (getFulmess.Count() > 0)
                                            {
                                                conn.Table<GetFullMessages>().Delete(x => x.MessageId == items.INC);
                                            }
                                        }

                                    }

                                }

                                else if (jwtDynamic.INC.Count > 0 && jwtDynamic.OUT.Count > 0)
                                {
                                    var tab = conn.Table<viewsMessageL>().ToList();
                                    foreach (var items in tab)
                                    {
                                        if (!(jwtDynamic.INC.Contains((int)items.INC)))
                                        {
                                            conn.Table<viewsMessageL>().Delete(x => x.INC == items.INC);

                                            var getmess = from s in conn.Table<GetMessages>()
                                                          where s.MessageId == items.INC
                                                          select s;
                                            if (getmess.Count() > 0)
                                            {
                                                conn.Table<GetMessages>().Delete(x => x.MessageId == items.INC);
                                            }

                                            var getFulmess = from s in conn.Table<GetFullMessages>()
                                                             where s.MessageId == items.INC
                                                             select s;
                                            if (getFulmess.Count() > 0)
                                            {
                                                conn.Table<GetFullMessages>().Delete(x => x.MessageId == items.INC);
                                            }
                                        }

                                    }

                                    var tabe = conn.Table<viewsMessageL>().ToList();
                                    foreach (var items in tabe)
                                    {
                                        if (!(jwtDynamic.OUT.Contains((int)items.OUT)))
                                        {
                                            conn.Table<viewsMessageL>().Delete(x => x.INC == items.INC);

                                            var getmess = from s in conn.Table<GetMessages>()
                                                          where s.MessageId == items.INC
                                                          select s;
                                            if (getmess.Count() > 0)
                                            {
                                                conn.Table<GetMessages>().Delete(x => x.MessageId == items.INC);
                                            }

                                            var getFulmess = from s in conn.Table<GetFullMessages>()
                                                             where s.MessageId == items.INC
                                                             select s;
                                            if (getFulmess.Count() > 0)
                                            {
                                                conn.Table<GetFullMessages>().Delete(x => x.MessageId == items.INC);
                                            }
                                        }

                                    }

                                }

                            }


                        }
                        return response.IsSuccessStatusCode;
                    }

                }
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}