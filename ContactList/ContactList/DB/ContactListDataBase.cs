using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactList.Models;
using SQLite;

namespace Todo
{
    public class ContactListDataBase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public ContactListDataBase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Person).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Person)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public Task<List<Person>> GetItemsAsync()
        {
            return Database.Table<Person>().ToListAsync();
        }

        public Task<List<Person>> GetItemsNotDoneAsync()
        {
            return Database.QueryAsync<Person>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<Person> GetItemAsync(int id)
        {
            return Database.Table<Person>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Person item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Person item)
        {
            return Database.DeleteAsync(item);
        }
    }
}

