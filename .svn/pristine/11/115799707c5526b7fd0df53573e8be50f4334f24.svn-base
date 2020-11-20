using MatrixXamarinApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MatrixXamarinApp.ServicesHandler
{
    public class BackgroundService2
    {
        SQLiteAsyncConnection con = new SQLiteAsyncConnection(App.FilePath);
        public async Task<bool> FullSync()
        {
            var username = await SecureStorage.GetAsync("userName");
            var url = await SecureStorage.GetAsync("url");
            var webguid = await SecureStorage.GetAsync("webGuid");
            var vID = await SecureStorage.GetAsync("ViewID1");
            var a = "";
            var b = "";
            var viewID = 0;
            var httpClients = new HttpClient();
            var responsed = await httpClients.GetAsync(url + "/Message/viewmessages?userName=" + username + "&" + "webGUID=" + webguid + "&viewID=" + vID + "&direction=" + a + "&condition=" + b);


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
                                            if (getFulmess.Count() > 0)
                                            {
                                                conn.Table<GetFullMessages>().Delete(x => x.MessageId == items.INC);
                                            }

                                        }

                                    }

                                }

                                else if (jwtDynamic.OUT.Count > 0 && jwtDynamic.INC == null)
                                {
                                    var tab = await con.Table<viewsMessageL>().ToListAsync();
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
                                    var tab = await con.Table<viewsMessageL>().ToListAsync();
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

                                    var tabe = await con.Table<viewsMessageL>().ToListAsync();
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
                    }

                }
                var lastUpdate = DateTime.Now;
                await SecureStorage.SetAsync("FullSync", lastUpdate.ToString());
                return true;
               
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> FullSync2()
        {
            var username = await SecureStorage.GetAsync("userName");
            var url = await SecureStorage.GetAsync("url");
            var webguid = await SecureStorage.GetAsync("webGuid");
            var vID = await SecureStorage.GetAsync("ViewID2");
            var a = "";
            var b = "";
            var viewID = 0;
            var httpClients = new HttpClient();
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
                            var jwtDynamic = JsonConvert.DeserializeObject<ViewsMessageList2>(json.Result);
                            viewID = jwtDynamics.Value<int>("ViewID");

                            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                            {


                                if (jwtDynamic.INC.Count > 0 && jwtDynamic.OUT == null)
                                {

                                    var tab = conn.Table<viewsMessageL2>().ToList();
                                    foreach (var items in tab)
                                    {
                                        if (!(jwtDynamic.INC.Contains((int)items.INC)))
                                        {
                                            conn.Table<viewsMessageL2>().Delete(x => x.INC == items.INC);

                                            var getmess = from s in conn.Table<GetMessages2>()
                                                          where s.MessageId == items.INC
                                                          select s;
                                            if (getmess.Count() > 0)
                                            {
                                                conn.Table<GetMessages2>().Delete(x => x.MessageId == items.INC);
                                            }

                                            var getFulmess = from s in conn.Table<GetFullMessages2>()
                                                             where s.MessageId == items.INC
                                                             select s;
                                            if (getFulmess.Count() > 0)
                                            {
                                                conn.Table<GetFullMessages2>().Delete(x => x.MessageId == items.INC);
                                            }

                                        }

                                    }

                                }

                                else if (jwtDynamic.OUT.Count > 0 && jwtDynamic.INC == null)
                                {
                                    var tab = await con.Table<viewsMessageL2>().ToListAsync();
                                    foreach (var items in tab)
                                    {
                                        if (!(jwtDynamic.OUT.Contains((int)items.OUT)))
                                        {
                                            conn.Table<viewsMessageL2>().Delete(x => x.INC == items.INC);

                                            var getmess = from s in conn.Table<GetMessages2>()
                                                          where s.MessageId == items.INC
                                                          select s;
                                            if (getmess.Count() > 0)
                                            {
                                                conn.Table<GetMessages2>().Delete(x => x.MessageId == items.INC);
                                            }

                                            var getFulmess = from s in conn.Table<GetFullMessages2>()
                                                             where s.MessageId == items.INC
                                                             select s;
                                            if (getFulmess.Count() > 0)
                                            {
                                                conn.Table<GetFullMessages2>().Delete(x => x.MessageId == items.INC);
                                            }
                                        }

                                    }

                                }

                                else if (jwtDynamic.INC.Count > 0 && jwtDynamic.OUT.Count > 0)
                                {
                                    var tab = await con.Table<viewsMessageL2>().ToListAsync();
                                    foreach (var items in tab)
                                    {
                                        if (!(jwtDynamic.INC.Contains((int)items.INC)))
                                        {
                                            conn.Table<viewsMessageL2>().Delete(x => x.INC == items.INC);

                                            var getmess = from s in conn.Table<GetMessages2>()
                                                          where s.MessageId == items.INC
                                                          select s;
                                            if (getmess.Count() > 0)
                                            {
                                                conn.Table<GetMessages2>().Delete(x => x.MessageId == items.INC);
                                            }

                                            var getFulmess = from s in conn.Table<GetFullMessages2>()
                                                             where s.MessageId == items.INC
                                                             select s;
                                            if (getFulmess.Count() > 0)
                                            {
                                                conn.Table<GetFullMessages2>().Delete(x => x.MessageId == items.INC);
                                            }
                                        }

                                    }

                                    var table = await con.Table<viewsMessageL2>().ToListAsync();
                                    foreach (var items in table)
                                    {
                                        if (!(jwtDynamic.OUT.Contains((int)items.OUT)))
                                        {
                                            conn.Table<viewsMessageL2>().Delete(x => x.INC == items.INC);

                                            var getmess = from s in conn.Table<GetMessages2>()
                                                          where s.MessageId == items.INC
                                                          select s;
                                            if (getmess.Count() > 0)
                                            {
                                                conn.Table<GetMessages2>().Delete(x => x.MessageId == items.INC);
                                            }

                                            var getFulmess = from s in conn.Table<GetFullMessages2>()
                                                             where s.MessageId == items.INC
                                                             select s;
                                            if (getFulmess.Count() > 0)
                                            {
                                                conn.Table<GetFullMessages2>().Delete(x => x.MessageId == items.INC);
                                            }
                                        }

                                    }

                                }

                            }


                        }
                    }

                }
                var lastUpdate = DateTime.Now;
                await SecureStorage.SetAsync("FullSync", lastUpdate.ToString());
                return true;

            }
            catch
            {
                return false;
            }
        }
    }
}
