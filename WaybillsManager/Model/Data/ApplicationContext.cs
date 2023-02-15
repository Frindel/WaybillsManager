using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using WaybillsManager.Model.Data.Entitys;

namespace WaybillsManager.Model.Data
{
	internal class ApplicationContext : DbContext
	{
		public DbSet<Waybill> Waybills { get; set; } = null!;

		public DbSet<Car> Cars { get; set; } = null!;

		public DbSet<Driver> Drivers { get; set; } = null!;

		public DbSet<RoutePoint> RoutePoints { get; set; } = null!;

		public ApplicationContext() : base()
		{
			Database.EnsureDeleted();
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//получение директории БД
			string dbDirectory = SettingsStorage.GetStorage().DbDirectory;

			//подключение к БД
			string connectionStr = $"Data Source='{dbDirectory}\\Waybills.db'";
			optionsBuilder.UseSqlite(connectionStr);
			
			// todo: установка уникальной комбинации номера и года
		}
	}
}
