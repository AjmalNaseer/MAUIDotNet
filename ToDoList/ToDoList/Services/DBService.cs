using SQLite;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class DBService
    {
        private readonly SQLiteAsyncConnection _database;

        // Singleton Instance
        private static DBService _instance;
        private static readonly object LockObject = new object();


        public static DBService Instance
        {
            get
            {
                // Initialize DBService if it hasn't been initialized yet
                if (_instance == null)
                {
                    lock (LockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new DBService();

                        }
                    }
                }
                return _instance;
            }
        }


        // Constructor
        private DBService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ToDoList.db3");
            _database = new SQLiteAsyncConnection(dbPath);
        }

        // Ensure tables are created in the database
        public async Task InitializeAsync()
        {
            try
            {
                await CreateTablesAsync();  // Create tables asynchronously
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during DB initialization: {ex.Message}");
                throw;  // Optionally handle the exception as needed
            }
        }

        public async Task<UserModel> GetCurrentUser()
        {
            // Query the database to get the first user (or the currently logged-in user, depending on your logic)
            var user = await _database.Table<UserModel>().FirstOrDefaultAsync(); // Ensure this is async
            return user;
        }

        // Ensure tables are created in the database asynchronously
        private async Task CreateTablesAsync()
        {
            await _database.CreateTableAsync<UserModel>();
            await _database.CreateTableAsync<TodoItemModel>(); // Create TodoItemModel table
        }

        // Save User to DB
        public Task<int> SaveUserAsync(UserModel user)
        {
            return _database.InsertAsync(user);
        }

        // Get User by Email and Password
        public async Task<UserModel> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            // Query the database for a user with the given email and password
            var user = await _database.Table<UserModel>()
                                       .Where(u => u.Email == email && u.Password == password)
                                       .FirstOrDefaultAsync();
            return user;
        }

        // Get all Todo items
        public Task<List<TodoItemModel>> GetItemsAsync()
        {
            return _database.Table<TodoItemModel>().ToListAsync();
        }

        // Get incomplete Todo items (Done = 0)
        public Task<List<TodoItemModel>> GetItemsNotDoneAsync()
        {
            return _database.QueryAsync<TodoItemModel>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        // Get a single Todo item by ID
        public Task<TodoItemModel> GetItemAsync(int id)
        {
            return _database.Table<TodoItemModel>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        // Save or update Todo item
        public Task<int> SaveItemAsync(TodoItemModel item)
        {
            if (item.ID != 0)
            {
                return _database.UpdateAsync(item); // Update if it already exists
            }
            else
            {
                return _database.InsertAsync(item); // Insert new item
            }
        }

        // Delete a Todo item
        public Task<int> DeleteItemAsync(TodoItemModel item)
        {
            return _database.DeleteAsync(item);
        }
    }
}
