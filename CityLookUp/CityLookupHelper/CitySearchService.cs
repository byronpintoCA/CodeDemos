using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLookupHelper
{
    public static class CitySearchService
    {
        private static bool _loaded;

        public static void Load()
        {
            if (_loaded == false)
            {
                var _masterList = Factory.GetAllCities();
                _loaded = true;
                BuildLookupOjects(_masterList);
            }

        }


        static Dictionary<string, List<CityInfo>> _threeAlphabetSearch = new Dictionary<string, List<CityInfo>>();
        static Dictionary<string, List<CityInfo>> _fourAlphabetSearch = new Dictionary<string, List<CityInfo>>();
        static Dictionary<string, List<CityInfo>> _fiveAlphabetSearch = new Dictionary<string, List<CityInfo>>();
        static Dictionary<string, List<CityInfo>> _sixAlphabetSearch = new Dictionary<string, List<CityInfo>>();
        static Dictionary<string, List<CityInfo>> _sevenAlphabetSearch = new Dictionary<string, List<CityInfo>>();
        static Dictionary<string, List<CityInfo>> _eightAlphabetSearch = new Dictionary<string, List<CityInfo>>();
        static Dictionary<string, List<CityInfo>> _nineAlphabetSearch = new Dictionary<string, List<CityInfo>>();
        static Dictionary<string, List<CityInfo>> _tenAlphabetSearch = new Dictionary<string, List<CityInfo>>();

        private static void BuildLookupOjects(List<CityInfo> masterList)
        {

            CreateGrouping(masterList, _threeAlphabetSearch, 3);
            CreateGrouping(masterList, _fourAlphabetSearch, 4);
            CreateGrouping(masterList, _fiveAlphabetSearch, 5);
            CreateGrouping(masterList, _sixAlphabetSearch, 6);
            CreateGrouping(masterList, _sevenAlphabetSearch, 7);
            CreateGrouping(masterList, _eightAlphabetSearch, 8);
            CreateGrouping(masterList, _nineAlphabetSearch, 9);
            CreateGrouping(masterList, _tenAlphabetSearch, 10);

            ApplyCitySearchStrinTransform(_threeAlphabetSearch);
            ApplyCitySearchStrinTransform(_fourAlphabetSearch);
            ApplyCitySearchStrinTransform(_fiveAlphabetSearch);
            ApplyCitySearchStrinTransform(_sixAlphabetSearch);
            ApplyCitySearchStrinTransform(_sevenAlphabetSearch);
            ApplyCitySearchStrinTransform(_eightAlphabetSearch);
            ApplyCitySearchStrinTransform(_nineAlphabetSearch);
            ApplyCitySearchStrinTransform(_tenAlphabetSearch);

        }

        private static void ApplyCitySearchStrinTransform(Dictionary<string, List<CityInfo>> collection)
        {
            foreach (var item in collection)
            {
                UpdateList(item.Value);
            }
        }

        private static void UpdateList(List<CityInfo> cityList)
        {
            foreach (var item in cityList)
            {
                item.ApplyCityNameTransform();
            }
        }

        private static void CreateGrouping(List<CityInfo> _masterList, Dictionary<string, List<CityInfo>> output, int no)
        {
            var first = _masterList.Where(it => it.City.Length >= no).GroupBy(cty => cty.City.Substring(0, no));

            foreach (var item in first)
            {
                output.Add(item.Key, item.ToList());
            }
        }

        public static List<CityInfo> SearchForCities(String Name, string searchCountry)
        {
            List<CityInfo> retVal = new List<CityInfo>() { new CityInfo() { City = "Not Found", Region = "Not Found" } };

            int length = Name.Trim().Length;

            if (length > 10)
            {
                Name = Name.Substring(0, 10);
                length = 10;
            }
            Name = Name.ToLower();

            try
            {
                switch (length)
                {
                    case 0:
                    case 1:
                    case 2:
                    default:
                        break;
                    case 3:
                        retVal = _threeAlphabetSearch[Name.ToLower().Trim()];
                        break;
                    case 4:
                        retVal = _fourAlphabetSearch[Name.ToLower().Trim()];
                        break;
                    case 5:
                        retVal = _fiveAlphabetSearch[Name.ToLower().Trim()];
                        break;
                    case 6:
                        retVal = _sixAlphabetSearch[Name.ToLower().Trim()];
                        break;
                    case 7:
                        retVal = _sevenAlphabetSearch[Name.ToLower().Trim()];
                        break;
                    case 8:
                        retVal = _eightAlphabetSearch[Name.ToLower().Trim()];
                        break;
                    case 9:
                        retVal = _nineAlphabetSearch[Name.ToLower().Trim()];
                        break;
                    case 10:
                        retVal = _tenAlphabetSearch[Name.ToLower().Trim()];
                        break;

                }
            }
            catch { }


            return retVal;

        }

    }
}
