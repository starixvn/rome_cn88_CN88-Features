using System.Collections.Generic;
using System.Linq;
using CN88.Features.Shared.Criteria;
using CN88.Features.Shared.Models;
using Fanex.Common;
using Fanex.Data;

namespace CN88.Features.Shared.DataAccesses
{
    public interface SportGroupDataAccess
    {
        IEnumerable<SportGroup> GetSportGroups(GetSportGroupsCriteria criteria);
    }

    public class SportGroupDataAccessImpl : SportGroupDataAccess
    {
        public IEnumerable<SportGroup> GetSportGroups(GetSportGroupsCriteria criteria)
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
                                id: Converter.ToInt(reader["SportTypeGroup"]),
                                displaySportIds: Converter.ToString2(reader["DisplaySport"]).Split(',').Select(int.Parse));

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