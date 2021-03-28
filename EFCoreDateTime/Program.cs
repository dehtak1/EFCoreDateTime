using EFCoreDateTime.DbModel;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace EFCoreDateTime
{
	class Program
	{
		static void Main(string[] args)
		{
			using EfcoredatetimeContext db = new EfcoredatetimeContext();
			int dbTestId = 0;
			try
			{

				string json = "{"
					+ "dtCasvUTC:\"2021-03-18T23:30:00Z\","			//UTC
					+ "dtCasvCET:\"2021-03-19T00:30:00+01:00\","	//CR
					+ "dtCasvMSK:\"2021-03-19T02:30:00+03:00\""		//Moskva
					+ "}";

				var vstup = JsonConvert.DeserializeObject<VstupJson>(json);

				Console.WriteLine("Nacteni z json:");
				Console.WriteLine(json);

				Vypis(vstup);

				var vstupOffset = JsonConvert.DeserializeObject<VstupJsonOffset>(json);
				
				Vypis(vstupOffset);

				DateTimeTest dbTestDoDB = new DateTimeTest
				{
					dt1 = vstup.dtCasvUTC,
					dt2 = vstup.dtCasvCET,
					dt3 = vstup.dtCasvMSK,
					dfoff1 = vstupOffset.dtCasvUTC,
					dfoff2 = vstupOffset.dtCasvCET,
					dfoff3 = vstupOffset.dtCasvMSK,
					dfoffMSKDateTime = vstup.dtCasvMSK
				};

				db.Add(dbTestDoDB);
				db.SaveChanges();
				dbTestId = dbTestDoDB.Id;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			//na stejnem contextu si panatuje casove zony
			using EfcoredatetimeContext db1 = new EfcoredatetimeContext();
			try
			{
				Console.WriteLine("Nacteni z DB:");
				Console.WriteLine("z typu DateTime");

				DateTimeTest dbTestZDB = db1.DateTimeTest
					.Where(x => x.Id == dbTestId)
					.FirstOrDefault();
				Console.WriteLine(JsonConvert.SerializeObject(dbTestZDB));
				Console.WriteLine();

				
				var zDateTime = new VstupJson
				{
					dtCasvUTC = dbTestZDB.dt1,
					dtCasvCET = dbTestZDB.dt2,
					dtCasvMSK = dbTestZDB.dt3
				};

				Vypis(zDateTime);

				Console.WriteLine();
				Console.WriteLine("Nacteni z DB:");
				Console.WriteLine("z typu DateTimeOffset");

				var zDateTimeOffset = new VstupJsonOffset
				{
					dtCasvUTC = dbTestZDB.dfoff1,
					dtCasvCET = dbTestZDB.dfoff2,
					dtCasvMSK = dbTestZDB.dfoff3
				};

				DateTimeOffset a = new DateTimeOffset();

				Vypis(zDateTimeOffset);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			Console.ReadLine();
		}
		public static void Vypis(VstupJson vstup)
		{
			string format = "dd.MM.yyyy HH:mm:ss K";

			Console.WriteLine("DateTime");
			Console.WriteLine();
			Console.WriteLine($"dtCasvUTC: {vstup.dtCasvUTC.ToString(format)}");
			Console.WriteLine($"dtCasvCET: {vstup.dtCasvCET.ToString(format)}");
			Console.WriteLine($"dtCasvMSK: {vstup.dtCasvMSK.ToString(format)}");

			Console.WriteLine();
			Console.WriteLine("Prevedeno na UTC");
			Console.WriteLine($"dtCasvUTC: {TimeZoneInfo.ConvertTimeToUtc(vstup.dtCasvUTC).ToString(format)}");
			Console.WriteLine($"dtCasvCET: {TimeZoneInfo.ConvertTimeToUtc(vstup.dtCasvCET).ToString(format)}");
			Console.WriteLine($"dtCasvMSK: {TimeZoneInfo.ConvertTimeToUtc(vstup.dtCasvMSK).ToString(format)}");

			Console.WriteLine();
			Console.WriteLine("dtCasvUTC == dtCasvCET");
			Console.WriteLine((vstup.dtCasvUTC == vstup.dtCasvCET).ToString());

			Console.WriteLine();
			Console.WriteLine("dtCasvCET == dtCasvMSK");
			Console.WriteLine((vstup.dtCasvCET == vstup.dtCasvMSK).ToString());

			Console.WriteLine();
			TimeSpan rozdilCasu = vstup.dtCasvUTC - vstup.dtCasvCET;
			Console.WriteLine("dtCasvUTC - dtCasvCET [sec]");
			Console.WriteLine(rozdilCasu.TotalSeconds.ToString());

			Console.WriteLine();
			rozdilCasu = vstup.dtCasvCET - vstup.dtCasvMSK;
			Console.WriteLine("dtCasvCET - dtCasvMSK [sec]");
			Console.WriteLine(rozdilCasu.TotalSeconds.ToString());

			Console.WriteLine();
			Console.WriteLine();
		}

		public static void Vypis(VstupJsonOffset vstup)
		{
			string format = "dd.MM.yyyy HH:mm:ss K";

			Console.WriteLine("DateTimeOffset");
			Console.WriteLine();
			Console.WriteLine($"dtCasvUTC: {vstup.dtCasvUTC.ToString(format)}");
			Console.WriteLine($"dtCasvCET: {vstup.dtCasvCET.ToString(format)}");
			Console.WriteLine($"dtCasvMSK: {vstup.dtCasvMSK.ToString(format)}");

			Console.WriteLine();
			Console.WriteLine("Prevedeno na UTC");
			Console.WriteLine($"dtCasvUTC: {vstup.dtCasvUTC.UtcDateTime.ToString(format)}");
			Console.WriteLine($"dtCasvCET: {vstup.dtCasvCET.UtcDateTime.ToString(format)}");
			Console.WriteLine($"dtCasvMSK: {vstup.dtCasvMSK.UtcDateTime.ToString(format)}");

			Console.WriteLine();
			Console.WriteLine("dtCasvUTC == dtCasvCET");
			Console.WriteLine((vstup.dtCasvUTC == vstup.dtCasvCET).ToString());

			Console.WriteLine();
			Console.WriteLine("dtCasvCET == dtCasvMSK");
			Console.WriteLine((vstup.dtCasvCET == vstup.dtCasvMSK).ToString());

			Console.WriteLine();
			TimeSpan rozdilCasu = vstup.dtCasvUTC - vstup.dtCasvCET;
			Console.WriteLine("dtCasvUTC - dtCasvCET [sec]");
			Console.WriteLine(rozdilCasu.TotalSeconds.ToString());

			Console.WriteLine();
			rozdilCasu = vstup.dtCasvCET - vstup.dtCasvMSK;
			Console.WriteLine("dtCasvCET - dtCasvMSK [sec]");
			Console.WriteLine(rozdilCasu.TotalSeconds.ToString());

			Console.WriteLine();
			Console.WriteLine();
		}
	}

	public class VstupJson
	{
		public DateTime dtCasvUTC { get; set; }
		public DateTime dtCasvCET { get; set; }
		public DateTime dtCasvMSK { get; set; }
	}
	public class VstupJsonOffset
	{
		public DateTimeOffset dtCasvUTC { get; set; }
		public DateTimeOffset dtCasvCET { get; set; }
		public DateTimeOffset dtCasvMSK { get; set; }
	}
}
