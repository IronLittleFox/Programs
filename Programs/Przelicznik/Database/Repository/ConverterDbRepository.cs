using Przelicznik.Database.Context;
using Przelicznik.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przelicznik.Database.Repository
{
    class ConverterDbRepository : IConverterRepository
    {
        private ConverterDbContext converterDbContext;

        public ConverterDbRepository()
        {
            converterDbContext = new ConverterDbContext();
            //InitDatabase();
        }

        private void InitDatabase()
        {
            UnitType unitTypeWaluta = CreateUnitType("Waluta");
            UnitType unitTypeMasa = CreateUnitType("Masa");

            Unit unitZloty = CreateUnit("Złoty", "zł", unitTypeWaluta.Id);
            Unit unitDolar = CreateUnit("Dolar", "$", unitTypeWaluta.Id);
            Unit unitFunt = CreateUnit("Funt", "£", unitTypeWaluta.Id);
            Unit unitGram = CreateUnit("gram", "g", unitTypeMasa.Id);
            Unit unitDekagram = CreateUnit("dekagram", "dag", unitTypeMasa.Id);
            Unit unitKilogram = CreateUnit("kilogram", "kg", unitTypeMasa.Id);
            Unit unitTona = CreateUnit("tona", "t", unitTypeMasa.Id);

            //Złoty na ...
            CreateUnitConverter(1, unitZloty.Id, unitZloty.Id);
            CreateUnitConverter(0.5F, unitZloty.Id, unitDolar.Id);
            CreateUnitConverter(5, unitZloty.Id, unitFunt.Id);

            //Dolar na ...
            CreateUnitConverter(2, unitDolar.Id, unitZloty.Id);
            CreateUnitConverter(1, unitDolar.Id, unitDolar.Id);
            CreateUnitConverter(4, unitDolar.Id, unitFunt.Id);

            //Funt na ...
            CreateUnitConverter(0.2F, unitFunt.Id, unitZloty.Id);
            CreateUnitConverter(0.25F, unitFunt.Id, unitDolar.Id);
            CreateUnitConverter(1, unitFunt.Id, unitFunt.Id);

            //gram na ...
            CreateUnitConverter(1, unitGram.Id, unitGram.Id);
            CreateUnitConverter(0.1F, unitGram.Id, unitDekagram.Id);
            CreateUnitConverter(0.001F, unitGram.Id, unitKilogram.Id);
            CreateUnitConverter(0.000001F, unitGram.Id, unitTona.Id);

            //dekagram na ...
            CreateUnitConverter(10, unitDekagram.Id, unitGram.Id);
            CreateUnitConverter(1, unitDekagram.Id, unitDekagram.Id);
            CreateUnitConverter(0.01F, unitDekagram.Id, unitKilogram.Id);
            CreateUnitConverter(0.00001F, unitDekagram.Id, unitTona.Id);

            //kilogram na ...
            CreateUnitConverter(1000, unitKilogram.Id, unitGram.Id);
            CreateUnitConverter(100, unitKilogram.Id, unitDekagram.Id);
            CreateUnitConverter(1, unitKilogram.Id, unitKilogram.Id);
            CreateUnitConverter(0.001F, unitKilogram.Id, unitTona.Id);

            //tona na ...
            CreateUnitConverter(1000000, unitTona.Id, unitGram.Id);
            CreateUnitConverter(100000, unitTona.Id, unitDekagram.Id);
            CreateUnitConverter(1000, unitTona.Id, unitKilogram.Id);
            CreateUnitConverter(1, unitTona.Id, unitTona.Id);
        }

        #region Create

        public UnitType CreateUnitType(string name)
        {
            UnitType unitType = new UnitType()
            {
                Name = name
            };

            converterDbContext.UnitTypes.Add(unitType);
            converterDbContext.SaveChanges();

            return unitType;
        }

        public Unit CreateUnit(string name, string symbol, int idUnitType)
        {
            Unit unit = new Unit()
            {
                Name = name,
                Symbol = symbol,
                UnitTypeId = idUnitType
            };

            converterDbContext.Units.Add(unit);
            converterDbContext.SaveChanges();
            return unit;
        }

        public UnitConverter CreateUnitConverter(float convertValue, int sourceUnitId, int targetUnitId)
        {
            UnitConverter unitConverter = new UnitConverter()
            {
                ConvertValue = convertValue,
                SourceUnitId = sourceUnitId,
                TargetUnitId = targetUnitId
            };
            converterDbContext.UnitConverters.Add(unitConverter);
            converterDbContext.SaveChanges();
            return unitConverter;
        }

        #endregion

        #region Read

        public List<UnitType> ReadAllUnitType()
        {
            /*
                select *
                  from UnitTypes
             */
            return converterDbContext.UnitTypes.ToList();
        }

        public List<Unit> ReadChosenUnit(int idUnitType)
        {
            /*
               select *
                 from Units u
                where u.UnitTypeId = idUnitType
             */
            return converterDbContext.Units.Where((Unit u) => u.UnitTypeId == idUnitType).ToList();
        }

        public UnitConverter ReadUnitConverter(int sourceUnitId, int targetUnitId)
        {
            /*
             select * 
              from UnitConverters uc
            where uc.SourceUnitId = sourceUnitId
              and uc.TargetUnitId = targetUnitId
             
             */
            return converterDbContext.UnitConverters
                .FirstOrDefault(uc => uc.SourceUnitId == sourceUnitId
                                && uc.TargetUnitId == targetUnitId);
        }

        #endregion
    }
}
