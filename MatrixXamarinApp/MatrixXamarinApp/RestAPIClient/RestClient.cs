using MatrixXamarinApp.Models;
using MatrixXamarinApp.Views;
using MimeKit;
using MimeKit.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatrixXamarinApp.RestAPIClient
{
    public class RestClient<T>
    {

        #region views
        public async Task<bool> views()
    {
            try
            {
                var httpClient = new HttpClient();
                UserDetailCredentials userDetailCredentials = new UserDetailCredentials();
                var username = await SecureStorage.GetAsync("userName");
                var webguid = await SecureStorage.GetAsync("webGuid");
                var url = await SecureStorage.GetAsync("url");
                userDetailCredentials.userName = username; // register.userName;
                userDetailCredentials.webGuid = webguid; // register.webGuid;
                var response = await httpClient.GetAsync(url + "/Message/views?" + "userName=" + userDetailCredentials.userName + "&" + "webGuid=" + userDetailCredentials.webGuid);
               
                using (HttpContent content = response.Content)
                {
                    var json = content.ReadAsStringAsync();
                    JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(json.Result);
                    var viewID = jwtDynamic.Value<int>("ViewID");
                    var viewName = jwtDynamic.Value<string>("ViewName");
                    ViewsModel viewsModel = new ViewsModel()
                    {
                        ViewID = viewID,
                        ViewName = viewName
                    };

                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        conn.CreateTable<ViewsModel>();
                        int rowsAdded = conn.Insert(viewsModel);
                    }

                    return response.IsSuccessStatusCode; // return either true or false
                }
            }
            catch
            {
                return false;
            }
       
    }
        #endregion

        #region checkRegister
        public async Task<bool> checkRegister(string url, string username, string password)
        {
            Register reg = new Register();
            var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync(url + "/Account/register?userName=" + username + "&" + "oTP=" + password);
                var accessToken = string.Empty;
                if (response.IsSuccessStatusCode)
                {

                    try
                    {
                        using (HttpContent content = response.Content)
                        {
                            var json = content.ReadAsStringAsync();
                            JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(json.Result);
                            accessToken = jwtDynamic.Value<string>("webGuid");
                        }
                        await SecureStorage.SetAsync("webGuid", accessToken);
                        await SecureStorage.SetAsync("userName", username);
                        await SecureStorage.SetAsync("url", url);
                        return response.IsSuccessStatusCode;
                    }
                    catch (InvalidOperationException ex)
                    {
                        return false;
                    }


                }
                else if (!response.IsSuccessStatusCode)
                {
                    try
                    {

                    }
                    catch (InvalidOperationException ex)
                    {

                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        #endregion

        #region inbox
        public async Task<bool> inbox(string num)
        {
            var username = await SecureStorage.GetAsync("userName");
            var url = await SecureStorage.GetAsync("url");
            var webguid = await SecureStorage.GetAsync("webGuid");
            Inbox inbox = new Inbox();
            var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync(url + "/Message/Incoming/" + num + "?userName=" + username + "&" + "webGUID=" + webguid);
                 var subject = string.Empty;
                    var number = 0;
                    var direction = 0;
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            using (HttpContent content = response.Content)
                            {
                                var json = content.ReadAsStringAsync();
                                JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(json.Result);
                                subject = jwtDynamic.Value<string>("subject");
                                number = jwtDynamic.Value<int>("number");
                                direction = jwtDynamic.Value<int>("direction");

                                InboxMessages inboxMessages = new InboxMessages()
                                {
                                    subject = subject,
                                    number = number,
                                    direction = direction
                                };
                                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                                {
                                    conn.CreateTable<InboxMessages>();
                                    int rowsAdded = conn.Insert(inboxMessages);
                                }

                            }

                            await SecureStorage.SetAsync("subject", subject);
                            return response.IsSuccessStatusCode;
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }

                    }
            }
            catch (Exception ex)
            {

            }
            
         
            return false;
        }

        #endregion

        #region outbox
        public async Task<bool> outbox(string num)
        {
            var username = await SecureStorage.GetAsync("userName");
            var url = await SecureStorage.GetAsync("url");
            var webguid = await SecureStorage.GetAsync("webGuid");
            Outbox inbox = new Outbox();
            var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync(url + "/Message/Outgoing/" + num + "?userName=" + username + "&" + "webGUID=" + webguid);
                var subject = string.Empty;
                var number = 0;
                var direction = 0;
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        using (HttpContent content = response.Content)
                        {
                            var json = content.ReadAsStringAsync();
                            JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(json.Result);
                            subject = jwtDynamic.Value<string>("subject");
                            number = jwtDynamic.Value<int>("number");
                            direction = jwtDynamic.Value<int>("direction");

                            OutboxMessages outboxMessages = new OutboxMessages()
                            {
                                subject = subject,
                                number = number,
                                direction = direction
                            };
                            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                            {
                                conn.CreateTable<OutboxMessages>();
                                int rowsAdded = conn.Insert(outboxMessages);
                            }
                        }
                        await SecureStorage.SetAsync("subj", subject);
                        return response.IsSuccessStatusCode;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }

                }
            }
            catch
            {
                return false;
            }
           
            return false;
        }

        #endregion

        #region ViewsMessages

        //View one implementatiom
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
                                conn.CreateTable<viewsMessageL>();
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
                                            conn.CreateTable<viewsMessageL>();
                                            int rowsAdded = conn.Insert(viewsMessageList);
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

                                            conn.CreateTable<viewsMessageL>();
                                            int rowsAdded = conn.Insert(viewsMessageList);
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

                                            conn.CreateTable<viewsMessageL>();
                                            int rowsAdded = conn.Insert(viewsMessageList);
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

                                            conn.CreateTable<viewsMessageL>();
                                            int rowsAdded = conn.Insert(viewsMessageList);
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

                                            conn.CreateTable<viewsMessageL>();
                                            int rowsAdded = conn.Insert(viewsMessageList);
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
                                conn.CreateTable<viewsMessageL2>();
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
                                            conn.CreateTable<viewsMessageL2>();
                                            int rowsAdded = conn.Insert(viewsMessageList);
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

                                            conn.CreateTable<viewsMessageL2>();
                                            int rowsAdded = conn.Insert(viewsMessageList);
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

                                            conn.CreateTable<viewsMessageL2>();
                                            int rowsAdded = conn.Insert(viewsMessageList);
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

                                            conn.CreateTable<viewsMessageL2>();
                                            int rowsAdded = conn.Insert(viewsMessageList);
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

                                            conn.CreateTable<viewsMessageL2>();
                                            int rowsAdded = conn.Insert(viewsMessageList);
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


        

        #region commented

        //public async Task<bool> ViewsMessages(string dir, string cond)
        //{
        //    var username = await SecureStorage.GetAsync("userName");
        //    var url = await SecureStorage.GetAsync("url");
        //    var webguid = await SecureStorage.GetAsync("webGuid");
        //    var vID = await SecureStorage.GetAsync("ViewID1");
        //    dir = await SecureStorage.GetAsync("dir1");
        //    cond = await SecureStorage.GetAsync("con1");
        //    var viewID = 0;

        //    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
        //    {
        //        conn.DeleteAll<ViewsModel>();
        //        conn.DeleteAll<viewsMessageL>();
        //    }
        //    var httpClient = new HttpClient();
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {

        //            var response = client.GetAsync(url + "/Message/viewmessages?userName=" + username + "&" + "webGUID=" + webguid + "&viewID=" + vID + "&direction=" + dir + "&condition=" + cond).Result;

        //            if (response.IsSuccessStatusCode)
        //            {
        //                using (HttpContent content = response.Content)
        //                {
        //                    var json = content.ReadAsStringAsync();
        //                    JObject jwtDynamics = JsonConvert.DeserializeObject<dynamic>(json.Result);
        //                    var jwtDynamic = JsonConvert.DeserializeObject<ViewsMessageList>(json.Result);
        //                    viewID = jwtDynamics.Value<int>("ViewID");

        //                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
        //                    {

        //                        const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
        //                        var cmd = conn.CreateCommand(cmdText, typeof(ViewsModel).Name);

        //                        var DB_query = "SELECT * FROM viewsMessageL WHERE ViewID=" + viewID + ";";
        //                        conn.CreateTable<viewsMessageL>();
        //                        var vid = from s in conn.Table<viewsMessageL>()
        //                                  where s.ViewID == viewID
        //                                  select s;

        //                        if (jwtDynamic.INC == null && jwtDynamic.OUT == null)
        //                        {

        //                            try
        //                            {

        //                                viewsMessageL viewsMessageList = new viewsMessageL()
        //                                {
        //                                    ViewID = viewID
        //                                };
        //                                conn.CreateTable<viewsMessageL>();
        //                                int rowsAdded = conn.Insert(viewsMessageList);

        //                            }

        //                            catch { }


        //                        }

        //                        else if (jwtDynamic.INC.Count > 0 && jwtDynamic.OUT == null)
        //                        {

        //                            foreach (var item in jwtDynamic.INC)
        //                            {

        //                                viewsMessageL viewsMessageList = new viewsMessageL()
        //                                {
        //                                    ViewID = viewID,
        //                                    INC = item
        //                                };

        //                                conn.CreateTable<viewsMessageL>();
        //                                int rowsAdded = conn.Insert(viewsMessageList);


        //                            }

        //                        }

        //                        else if (jwtDynamic.OUT.Count > 0 && jwtDynamic.INC == null)
        //                        {
        //                            foreach (var item in jwtDynamic.INC)
        //                            {

        //                                viewsMessageL viewsMessageList = new viewsMessageL()
        //                                {
        //                                    ViewID = viewID,
        //                                    OUT = item
        //                                };

        //                                conn.CreateTable<viewsMessageL>();
        //                                int rowsAdded = conn.Insert(viewsMessageList);

        //                            }

        //                        }

        //                        else if (jwtDynamic.INC.Count > 0 && jwtDynamic.OUT.Count > 0)
        //                        {
        //                            viewsMessageL viewsMessageList = new viewsMessageL();

        //                            foreach (var item in jwtDynamic.INC)
        //                            {

        //                                viewsMessageList.ViewID = viewID;
        //                                viewsMessageList.INC = item;

        //                                conn.CreateTable<viewsMessageL>();
        //                                int rowsAdded = conn.Insert(viewsMessageList);

        //                            }

        //                            foreach (var item in jwtDynamic.OUT)
        //                            {

        //                                viewsMessageList.ViewID = viewID;
        //                                viewsMessageList.OUT = item;

        //                                conn.CreateTable<viewsMessageL>();
        //                                int rowsAdded = conn.Insert(viewsMessageList);

        //                            }

        //                        }

        //                    }


        //                }

        //                return response.IsSuccessStatusCode;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //    return false;
        //}



        //public async Task<bool> ViewsMessages2(string dir, string cond)
        //{
        //    var username = await SecureStorage.GetAsync("userName");
        //    var url = await SecureStorage.GetAsync("url");
        //    var webguid = await SecureStorage.GetAsync("webGuid");
        //    var vID = await SecureStorage.GetAsync("ViewID2");
        //    dir = await SecureStorage.GetAsync("dir2");
        //    cond = await SecureStorage.GetAsync("con2");
        //    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
        //    {
        //        conn.DeleteAll<ViewsModel2>();
        //        conn.DeleteAll<viewsMessageL2>();
        //    }

        //    var viewID = 0;
        //    var httpClient = new HttpClient();
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {

        //            var response = client.GetAsync(url + "/Message/viewmessages?userName=" + username + "&" + "webGUID=" + webguid + "&viewID=" + vID + "&direction=" + dir + "&condition=" + cond).Result;

        //            //var response = await httpClient.GetAsync(url + "/Message/viewmessages?userName=" + username + "&" + "webGUID=" + webguid + "&viewID=" + vID + "&direction=" + dir + "&condition=" + cond);

        //            if (response.IsSuccessStatusCode)
        //            {

        //                using (HttpContent content = response.Content)
        //                {
        //                    var json = content.ReadAsStringAsync();
        //                    JObject jwtDynamics = JsonConvert.DeserializeObject<dynamic>(json.Result);
        //                    var jwtDynamic = JsonConvert.DeserializeObject<ViewsMessageList2>(json.Result);
        //                    viewID = jwtDynamics.Value<int>("ViewID");

        //                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
        //                    {

        //                        if (jwtDynamic.INC == null && jwtDynamic.OUT == null)
        //                        {

        //                            viewsMessageL2 viewsMessageList = new viewsMessageL2()
        //                            {
        //                                ViewID = viewID
        //                            };
        //                            conn.CreateTable<viewsMessageL2>();
        //                            int rowsAdded = conn.Insert(viewsMessageList);

        //                        }

        //                        else if (jwtDynamic.INC.Count > 0 && jwtDynamic.OUT == null)
        //                        {
        //                            foreach (var item in jwtDynamic.INC)
        //                            {

        //                                viewsMessageL2 viewsMessageList = new viewsMessageL2()
        //                                {
        //                                    ViewID = viewID,
        //                                    INC = item
        //                                };

        //                                conn.CreateTable<viewsMessageL2>();
        //                                int rowsAdded = conn.Insert(viewsMessageList);

        //                            }

        //                        }

        //                        else if (jwtDynamic.OUT.Count > 0 && jwtDynamic.INC == null)
        //                        {
        //                            foreach (var item in jwtDynamic.OUT)
        //                            {

        //                                viewsMessageL2 viewsMessageList = new viewsMessageL2()
        //                                {
        //                                    ViewID = viewID,
        //                                    OUT = item
        //                                };

        //                                conn.CreateTable<viewsMessageL2>();
        //                                int rowsAdded = conn.Insert(viewsMessageList);

        //                            }

        //                        }

        //                        else if (jwtDynamic.INC.Count > 0 && jwtDynamic.OUT.Count > 0)
        //                        {
        //                            viewsMessageL2 viewsMessageList = new viewsMessageL2();

        //                            foreach (var item in jwtDynamic.INC)
        //                            {

        //                                viewsMessageList.ViewID = viewID;
        //                                viewsMessageList.INC = item;

        //                                conn.CreateTable<viewsMessageL2>();
        //                                int rowsAdded = conn.Insert(viewsMessageList);

        //                            }

        //                            foreach (var item in jwtDynamic.OUT)
        //                            {

        //                                viewsMessageList.ViewID = viewID;
        //                                viewsMessageList.OUT = item;

        //                                conn.CreateTable<viewsMessageL2>();
        //                                int rowsAdded = conn.Insert(viewsMessageList);

        //                            }

        //                        }


        //                    }


        //                }

        //                return response.IsSuccessStatusCode;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }


        //    return false;
        //}

        //----------------------
        #endregion
        #endregion

        #region GetMessages

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
                    var message = conn.Table<viewsMessageL>().ToList();
                    //var Getmessage = conn.Table<GetMessages>().ToList();
                    GetMessages mess = new GetMessages();
                    var count = 0;
                    var count2 = 0;
                    foreach (var item in message)
                    {
                        
                        if(item.INC != null && item.OUT == null)
                        {
                            conn.CreateTable<GetMessages>();
                            var mes = conn.Table<GetMessages>()
                                       .Where(x => x.MessageId == item.INC).FirstOrDefault();

                            if (mes == null && count < 10)
                            {
                                count++;
                                var a = item.INC;
                                ac += a + ",";
                            }
                            
                        }
                        if(item.OUT != null)
                        {
                            var mes = conn.Table<GetMessages>()
                                      .Where(x => x.MessageId == item.OUT)
                                       .FirstOrDefault();
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
                            conn.DeleteAll<GetMessages>();
                        }

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
                                    conn.CreateTable<GetMessages>();
                                    int rowsAdded = conn.Insert(getMessages);


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
                                    conn.CreateTable<GetMessages>();
                                    int rowsAdded = conn.Insert(getMessages);


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
                    var message = conn.Table<viewsMessageL2>().ToList();
                    //var Getmessage = conn.Table<GetMessages2>().ToList();
                    GetMessages2 mess = new GetMessages2();
                    var count = 0;
                    var count2 = 0;
                    foreach (var item in message)
                    {

                        if (item.INC != null && item.OUT == null)
                        {

                            var mes = conn.Table<GetMessages2>()
                                       .Where(x => x.MessageId == item.INC).FirstOrDefault();

                            if (mes == null && count < 10)
                            {
                                count++;
                                var a = item.INC;
                                ac += a + ",";
                            }

                        }
                        if (item.OUT != null)
                        {
                            var mes = conn.Table<GetMessages2>()
                                      .Where(x => x.MessageId == item.OUT)
                                       .FirstOrDefault();
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


                        if (cmd.ExecuteScalar<string>() != null)
                        {
                            conn.DeleteAll<GetMessages2>();
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
                                    conn.CreateTable<GetMessages2>();
                                    int rowsAdded = conn.Insert(getMessages);


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
                                    conn.CreateTable<GetMessages2>();
                                    int rowsAdded = conn.Insert(getMessages);


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
                    var response = await client.GetAsync(url + "/Message/Get?msgIds=" + messageIdINC + "&messageDir=INC&userName=" + username + "&webGuid=" + webguid + "&msgPart=1");
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
                                conn.DeleteAll<GetFullMessages>();
                                for (var i = 0; i < jwtDynamic.Count; i++)
                                {
                                        getMessages = new GetFullMessages()
                                        {
                                            MessageId = jwtDynamic[i].MessageId,
                                            EML = jwtDynamic[i].EML

                                        };
                                        conn.CreateTable<GetFullMessages>();
                                        int rowsAdded = conn.Insert(getMessages);
                                        //SecureStorage.Remove("EML");
                                        //await SecureStorage.SetAsync("EML", jwtDynamic[i].EML);   
                                    

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
                                        conn.CreateTable<GetFullMessages>();
                                        int rowsAdded = conn.Insert(getMessages);
                                        //SecureStorage.Remove("EML");
                                       // await SecureStorage.SetAsync("EML", jwtDynamic[i].EML);
                                }
                            }
                        }


                        //}


                    }
                    var response2 = await client.GetAsync(url + "/Message/Get?msgIds=" + messageIdOUT + "&messageDir=OUT&userName=" + username + "&webGuid=" + webguid + "&msgPart=1");

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
                                        conn.DeleteAll<GetFullMessages>();
                                        for (var i = 0; i < jwtDynamic.Count; i++)
                                        {
                                            getMessages = new GetFullMessages()
                                            {
                                                MessageId = jwtDynamic[i].MessageId,
                                                EML = jwtDynamic[i].EML

                                            };
                                            conn.CreateTable<GetFullMessages>();
                                            int rowsAdded = conn.Insert(getMessages);
                                            //SecureStorage.Remove("EML");
                                            //await SecureStorage.SetAsync("EML", jwtDynamic[i].EML);
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
                                            conn.CreateTable<GetFullMessages>();
                                            int rowsAdded = conn.Insert(getMessages);
                                            //SecureStorage.Remove("EML");
                                            //await SecureStorage.SetAsync("EML", jwtDynamic[i].EML);
                                        
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
                                        conn.CreateTable<GetFullMessages>();
                                        int rowsAdded = conn.Insert(getMessages);
                                       //SecureStorage.Remove("EML");
                                        //await SecureStorage.SetAsync("EML", jwtDynamic[i].EML);

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
            var messageDirection = await SecureStorage.GetAsync("MessageDirection1");

            var httpClient = new HttpClient();

            try
            {


                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(url + "/Message/Get?msgIds=" + messageIdINC + "&messageDir=INC&userName=" + username + "&webGuid=" + webguid + "&msgPart=1").Result;
                    var response2 = client.GetAsync(url + "/Message/Get?msgIds=" + messageIdOUT + "&messageDir=OUT&userName=" + username + "&webGuid=" + webguid + "&msgPart=1").Result;
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
                                conn.DeleteAll<GetFullMessages2>();
                                for (var i = 0; i < jwtDynamic.Count; i++)
                                {
                                    getMessages = new GetFullMessages2()
                                    {
                                        MessageId = jwtDynamic[i].MessageId,
                                        EML = jwtDynamic[i].EML

                                    };
                                    conn.CreateTable<GetFullMessages2>();
                                    int rowsAdded = conn.Insert(getMessages);
                                    SecureStorage.Remove("EML");
                                    await SecureStorage.SetAsync("EML", jwtDynamic[i].EML);                                    //if (await SecureStorage.GetAsync("EML") == null)
                                           
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
                                    conn.CreateTable<GetFullMessages2>();
                                    int rowsAdded = conn.Insert(getMessages);
                                    SecureStorage.Remove("EML");
                                    await SecureStorage.SetAsync("EML", jwtDynamic[i].EML);
                                }
                            }
                        }


                        //}


                    }
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
                                    conn.DeleteAll<GetFullMessages2>();
                                    for (var i = 0; i < jwtDynamic.Count; i++)
                                    {
                                        getMessages = new GetFullMessages2()
                                        {
                                            MessageId = jwtDynamic[i].MessageId,
                                            EML = jwtDynamic[i].EML

                                        };
                                        conn.CreateTable<GetFullMessages2>();
                                        int rowsAdded = conn.Insert(getMessages);
                                        SecureStorage.Remove("EML");
                                        await SecureStorage.SetAsync("EML", jwtDynamic[i].EML);
                                        //if (await SecureStorage.GetAsync("EML") == null)
                                        //{
                                        //    SecureStorage.Remove("EML");
                                        //}
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
                                        conn.CreateTable<GetFullMessages2>();
                                        int rowsAdded = conn.Insert(getMessages);
                                        SecureStorage.Remove("EML");
                                        await SecureStorage.SetAsync("EML", jwtDynamic[i].EML);
                                        //if (await SecureStorage.GetAsync("EML") == null)
                                        //{
                                        //    SecureStorage.Remove("EML");
                                        //}


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
                                    conn.CreateTable<GetFullMessages2>();
                                    int rowsAdded = conn.Insert(getMessages);
                                    SecureStorage.Remove("EML");
                                    await SecureStorage.SetAsync("EML", jwtDynamic[i].EML);

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

       

        #endregion

    }
    //http://webapi.sd-matrix.com:5000/Message/viewmessages?userName=calebo&webGUID=1bd5b90a-8caa-4953-9c62-a54fa8fbc527&viewID=5111&direction=inc1222&condition=out1112
    //http://webapi.sd-matrix.com:5000/Account/login?userName=MARKM&webGuid=2f31faee-396e-4964-a2ea-13e3e44545ce
    ///http://webapi.sd-matrix.com:5000/Message/Outgoing/249406?userName=calebo&webGUID=fca12116-224a-4c9a-9c62-6e358322950d
}