using Przelicznik.Database.Entities;
using System.Collections.Generic;

namespace Przelicznik.Database.Repository
{
    interface IConverterRepository
    {
        Unit CreateUnit(string name, string symbol, int idUnitType);
        UnitConverter CreateUnitConverter(float convertValue, int sourceUnitId, int targetUnitId);
        UnitType CreateUnitType(string name);
        List<UnitType> ReadAllUnitType();
        List<Unit> ReadChosenUnit(int idUnitType);
        UnitConverter ReadUnitConverter(int sourceUnitId, int targetUnitId);
    }
}