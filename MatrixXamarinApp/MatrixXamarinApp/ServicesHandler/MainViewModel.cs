using MatrixXamarinApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Extended;

namespace MatrixXamarinApp.ServicesHandler
{
    class MainViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        private const int PageSize = 10;
        readonly DataService _dataService = new DataService();

        public event PropertyChangedEventHandler PropertyChanged;

        public InfiniteScrollCollection<GetMessages> Items { get; set; }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            Items = new InfiniteScrollCollection<GetMessages>
            {
                OnLoadMore = async () =>
                {
                    IsBusy = true;
                    // load the next page
                    var page = Items.Count / PageSize;
                    var items = await _dataService.GetItemAsync(page, PageSize);
                    IsBusy = false;

                    //return the items that need to be added
                    return items;
                },
                OnCanLoadMore = () =>
                {
                    using (SQLiteConnection con = new SQLiteConnection(App.FilePath))
                    {
                        var s = con.Table<GetMessages>().Count();
                        return Items.Count < s;
                    }
                }
            };

            DownloadDataAsync();
        }

        private async Task DownloadDataAsync()
        {
            var items = await _dataService.GetItemAsync(pageIndex: 0, pagesize: PageSize);
            Items.AddRange(items);
        }

       // public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }



    public class DataService
    {
        SQLiteAsyncConnection con = new SQLiteAsyncConnection(App.FilePath);
        public async Task<List<GetMessages>> GetItemAsync(int pageIndex, int pagesize)
        {
            await Task.Delay(1000);
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                await con.CreateTableAsync<GetMessages>();
                var message = await con.Table<GetMessages>().OrderByDescending(x => x.CreatedTime).ToListAsync();

                return message.Skip(pageIndex * pagesize).Take(pagesize).ToList();
                SQLiteAsyncConnection.ResetPool();
            }
           
        }
    }

    class MainViewModel2 : INotifyPropertyChanged
    {
        private bool _isBusy;
        private const int PageSize = 10;
        readonly DataService2 _dataService = new DataService2();

        public event PropertyChangedEventHandler PropertyChanged;

        public InfiniteScrollCollection<GetMessages2> Items { get; set; }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel2()
        {
            Items = new InfiniteScrollCollection<GetMessages2>
            {
                OnLoadMore = async () =>
                {
                    IsBusy = true;
                    // load the next page
                    var page = Items.Count / PageSize;
                    var items = await _dataService.GetItemAsync(page, PageSize);
                    IsBusy = false;

                    //return the items that need to be added
                    return items;
                },
                OnCanLoadMore = () =>
                {
                    using (SQLiteConnection con = new SQLiteConnection(App.FilePath))
                    {
                        var s = con.Table<GetMessages2>().Count();
                        return Items.Count < s;
                    }
                        
                }
            };

            DownloadDataAsync();
        }

        private async Task DownloadDataAsync()
        {
            var items = await _dataService.GetItemAsync(pageIndex: 0, pagesize: PageSize);
            Items.AddRange(items);
        }

        // public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DataService2
    {
        SQLiteAsyncConnection con = new SQLiteAsyncConnection(App.FilePath);
        public async Task<List<GetMessages2>> GetItemAsync(int pageIndex, int pagesize)
        {
            await Task.Delay(1000);
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                await con.CreateTableAsync<GetMessages2>();
                var message = await con.Table<GetMessages2>().OrderByDescending(x => x.CreatedTime).ToListAsync();

                return message.Skip(pageIndex * pagesize).Take(pagesize).ToList();
                SQLiteAsyncConnection.ResetPool();
            }

        }
    }
}
