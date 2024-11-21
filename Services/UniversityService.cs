using System;
using System.Collections.Generic;
using System.Linq;
using csiro_mvc.Models;

namespace csiro_mvc.Services
{
    public interface IUniversityService
    {
        IEnumerable<University> GetTop100Universities();
        bool IsUniversityInTop100(string universityName);
    }

    public class UniversityService : IUniversityService
    {
        private readonly List<University> _top100Universities = new()
        {
            new University { Name = "Massachusetts Institute of Technology (MIT)", Country = "USA" },
            new University { Name = "University of Cambridge", Country = "UK" },
            new University { Name = "Stanford University", Country = "USA" },
            new University { Name = "University of Oxford", Country = "UK" },
            new University { Name = "Harvard University", Country = "USA" },
            new University { Name = "California Institute of Technology", Country = "USA" },
            new University { Name = "Imperial College London", Country = "UK" },
            new University { Name = "ETH Zurich", Country = "Switzerland" },
            new University { Name = "National University of Singapore", Country = "Singapore" },
            new University { Name = "University College London", Country = "UK" },
            new University { Name = "University of California, Berkeley", Country = "USA" },
            new University { Name = "University of Chicago", Country = "USA" },
            new University { Name = "University of Tokyo", Country = "Japan" },
            new University { Name = "University of Toronto", Country = "Canada" },
            new University { Name = "Peking University", Country = "China" },
            new University { Name = "Yale University", Country = "USA" },
            new University { Name = "Princeton University", Country = "USA" },
            new University { Name = "Columbia University", Country = "USA" },
            new University { Name = "Cornell University", Country = "USA" },
            new University { Name = "University of Pennsylvania", Country = "USA" },
            new University { Name = "Tsinghua University", Country = "China" },
            new University { Name = "University of Edinburgh", Country = "UK" },
            new University { Name = "University of Michigan", Country = "USA" },
            new University { Name = "Johns Hopkins University", Country = "USA" },
            new University { Name = "McGill University", Country = "Canada" },
            new University { Name = "Technical University of Munich", Country = "Germany" },
            new University { Name = "Delft University of Technology", Country = "Netherlands" },
            new University { Name = "Australian National University", Country = "Australia" },
            new University { Name = "University of Melbourne", Country = "Australia" },
            new University { Name = "Northwestern University", Country = "USA" },
            new University { Name = "University of British Columbia", Country = "Canada" },
            new University { Name = "London School of Economics", Country = "UK" },
            new University { Name = "University of Manchester", Country = "UK" },
            new University { Name = "King's College London", Country = "UK" },
            new University { Name = "Seoul National University", Country = "South Korea" },
            new University { Name = "University of Wisconsin-Madison", Country = "USA" },
            new University { Name = "University of Illinois at Urbana-Champaign", Country = "USA" },
            new University { Name = "Kyoto University", Country = "Japan" },
            new University { Name = "University of Sydney", Country = "Australia" },
            new University { Name = "Hong Kong University", Country = "Hong Kong" },
            new University { Name = "University of Amsterdam", Country = "Netherlands" },
            new University { Name = "Ludwig Maximilian University of Munich", Country = "Germany" },
            new University { Name = "University of Texas at Austin", Country = "USA" },
            new University { Name = "University of Copenhagen", Country = "Denmark" },
            new University { Name = "University of Queensland", Country = "Australia" },
            new University { Name = "Monash University", Country = "Australia" },
            new University { Name = "Shanghai Jiao Tong University", Country = "China" },
            new University { Name = "University of Washington", Country = "USA" },
            new University { Name = "University of Bristol", Country = "UK" },
            new University { Name = "Georgia Institute of Technology", Country = "USA" },
            new University { Name = "University of Glasgow", Country = "UK" },
            new University { Name = "Durham University", Country = "UK" },
            new University { Name = "University of California, Los Angeles", Country = "USA" },
            new University { Name = "University of Adelaide", Country = "Australia" },
            new University { Name = "University of Oslo", Country = "Norway" },
            new University { Name = "University of Helsinki", Country = "Finland" },
            new University { Name = "University of Geneva", Country = "Switzerland" },
            new University { Name = "University of Auckland", Country = "New Zealand" },
            new University { Name = "Trinity College Dublin", Country = "Ireland" },
            new University { Name = "University of Vienna", Country = "Austria" },
            new University { Name = "University of Zurich", Country = "Switzerland" },
            new University { Name = "University of São Paulo", Country = "Brazil" },
            new University { Name = "University of Barcelona", Country = "Spain" },
            new University { Name = "University of Warsaw", Country = "Poland" },
            new University { Name = "University of Stockholm", Country = "Sweden" },
            new University { Name = "Leiden University", Country = "Netherlands" },
            new University { Name = "University of California, San Diego", Country = "USA" },
            new University { Name = "University of Alberta", Country = "Canada" },
            new University { Name = "Rice University", Country = "USA" },
            new University { Name = "Vanderbilt University", Country = "USA" },
            new University { Name = "University of Notre Dame", Country = "USA" },
            new University { Name = "Boston University", Country = "USA" },
            new University { Name = "University of Southern California", Country = "USA" },
            new University { Name = "University of Virginia", Country = "USA" },
            new University { Name = "University of North Carolina at Chapel Hill", Country = "USA" },
            new University { Name = "University of California, Davis", Country = "USA" },
            new University { Name = "University of Maryland, College Park", Country = "USA" },
            new University { Name = "University of Pittsburgh", Country = "USA" },
            new University { Name = "Ohio State University", Country = "USA" },
            new University { Name = "Penn State University", Country = "USA" },
            new University { Name = "University of Minnesota", Country = "USA" },
            new University { Name = "Purdue University", Country = "USA" },
            new University { Name = "Rutgers University", Country = "USA" },
            new University { Name = "University of California, Irvine", Country = "USA" },
            new University { Name = "Michigan State University", Country = "USA" },
            new University { Name = "University of Colorado Boulder", Country = "USA" },
            new University { Name = "University of Florida", Country = "USA" },
            new University { Name = "Indian Institute of Science", Country = "India" },
            new University { Name = "University of Cape Town", Country = "South Africa" },
            new University { Name = "Technical University of Denmark", Country = "Denmark" },
            new University { Name = "University of Groningen", Country = "Netherlands" },
            new University { Name = "Uppsala University", Country = "Sweden" },
            new University { Name = "University of Leeds", Country = "UK" },
            new University { Name = "University of Sheffield", Country = "UK" },
            new University { Name = "University of Birmingham", Country = "UK" },
            new University { Name = "University of Nottingham", Country = "UK" },
            new University { Name = "Cardiff University", Country = "UK" },
            new University { Name = "University of Liverpool", Country = "UK" },
            new University { Name = "University of Southampton", Country = "UK" },
            new University { Name = "Hebrew University of Jerusalem", Country = "Israel" },
            new University { Name = "Tel Aviv University", Country = "Israel" },
            new University { Name = "Technion - Israel Institute of Technology", Country = "Israel" },
            new University { Name = "National Taiwan University", Country = "Taiwan" }
        };

        public IEnumerable<University> GetTop100Universities()
        {
            return _top100Universities.OrderBy(u => u.Ranking);
        }

        public bool IsUniversityInTop100(string universityName)
        {
            return _top100Universities.Any(u => u.Name == universityName);
        }
    }
}
