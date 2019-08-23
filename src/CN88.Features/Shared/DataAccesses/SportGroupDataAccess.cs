namespace CN88.Features.Shared.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CN88.Features.BetLists.Criteria.WagerMonitoring;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public interface SportGroupDataAccess
    {
        IEnumerable<MonitorSportGroup> GetSportGroups(GetMonitorSportGroupCriteria criteria, Func<int, string> getSportGroupDisplayName);

        IEnumerable<SportGroup> GetSportGroups(ICriteria criteria, Func<int, string> getSportGroupDisplayName);
    }

    public class SportGroupDataAccessImpl : SportGroupDataAccess
    {
        public IEnumerable<SportGroup> GetSportGroups(ICriteria criteria, Func<int, string> getSportGroupDisplayName)
        {
            var groups = new List<SportGroup>();

            using (var objectDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var group = new SportGroup(
                                productId: Converter.ToInt(reader["ProductId"]),
                                id: Converter.ToInt(reader["SportTypeGroup"]),
                                displaySportIds: Converter.ToString2(reader["DisplaySport"]).Split(',').Select(int.Parse),
                                getSportGroupDisplayName: getSportGroupDisplayName);

                            groups.Add(group);
                        }

                        reader.Close();
                    }
                }
            }

            return groups;
        }

        public IEnumerable<MonitorSportGroup> GetSportGroups(GetMonitorSportGroupCriteria criteria, Func<int, string> getSportGroupDisplayName)
        {
            var groups = new List<MonitorSportGroup>();

            using (var objectDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var group = new MonitorSportGroup(
                                id: Converter.ToInt(reader["SportTypeGroup"]),
                                displaySportIds: Converter.ToString2(reader["DisplaySport"]).Split(',').Select(int.Parse),
                                getSportGroupDisplayName: getSportGroupDisplayName);

                            groups.Add(group);
                        }

                        reader.Close();
                    }
                }
            }

            return groups;
        }
    }
}