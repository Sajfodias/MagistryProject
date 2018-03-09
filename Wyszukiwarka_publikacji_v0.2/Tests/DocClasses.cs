using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class DocClasses
    {
        /// <summary>
        /// In what way we can chose the classes for document assign?
        /// -The organizations assigned to documents - not all documents has assigned organizations
        /// -Using title/abstract/keywords
        /// How can we automate the process of class assigment do documents?
        /// </summary>
        /// <returns></returns>
        public static List<string> ArchitectureClasseOfDocuments_ListCreations()
        {
            List<string> ArchitectureClass = new List<string>();

            #region Architectrure_Class_Documnets
            /*
            ArchitectureClass.Add(" architektura "+ 
                "  Wystawa prac graficznych, rysunków i malarstwa obrazujacych rolę sztuk plastycznych w procesie kreowania form architektonicznych. Prezentacja autorskiej metod nauczania: Zapis-Interpretacja-transformacja oraz metody ideograficznej. "+ 
                "  ARCHITEKTURA ARCHITEKTURA WSPÓŁCZESNA IDEOGRAM KOMPOZYCJA SZTUKA  ");
            ArchitectureClass.Add("  Projekt stanowisk badawczych siłowni kogeneracyjnych i wirującej tarczy " +
                "  Projekt budowlany "+
                "  ARCHITEKTURA PRZEMYSŁOWA  ");
            ArchitectureClass.Add("  Projekt fundamentów pod urządzenia siłowni kogeneracyjnych "+
                "  Projekt wykonawczy "+
                "  ARCHITEKTURA PRZEMYSŁOWA  ");
            ArchitectureClass.Add("  Budynek mieszkalny wielorodzinny "+
                "  Projekt budowlany zamienny "+
                "  ARCHITEKTURA MIESZKANIOWA  ");
            ArchitectureClass.Add("  Projekt zamienny do projektu zmiana funkcji domu mieszkalnego z funkcją agroturystyczną na funkcję hotelową " +
                "  Projekt budowlany "+
                "  ARCHITEKTURA HOTELOWA  ");
            ArchitectureClass.Add("  Projekt zamienny - zmiana funkcji domu mieszkalnego z funkcją agroturystyczną na funkcję hotelową "+
                "  Projekt budowlany "+
                "  ARCHITEKTURA HOTELOWA  ");
            ArchitectureClass.Add("  Budynek mieszkalny jednorodzinny " +
                "  Projekt budowlany " +
                "  ARCHITEKTURA MIESZKANIOWA  ");
            ArchitectureClass.Add("  Areszt śledczy w Starogardzie GdańskimRozbudowa budynku penitencjarnego o pomieszczenia ambulatorium " +
                "  Projekt architektoniczno - budowlany. "+
                "  ARCHITEKTURA PENITENCJARNA ARCHITEKTURA SŁUZBY ZDROWIA  ");
            ArchitectureClass.Add("  Przebudowa i rozbudowa budynku Instytutu Pamięci Narodowej ze zmiana funkcji z produkcyjnej na administracyjno-magazynową "+
                "  Projekt architektoniczno-budowlany "+
                "  ARCHITEKTURA UŻYTECZNOŚCI PUBLICZNEJ  ");
            ArchitectureClass.Add("  Przebudowa i rozbudowa budynku Instytutu Pamieci Narodowej - Komisja Scigania Zbrodni przeciwko Narodowi Polskiemu ze zmianą funkcji z produlcji na administracyjno-magazynową. "+
                "  Projekt architektoniczno-budowlany "+
                "  ARCHITEKTURA MIESZKANIOWA  ");
            ArchitectureClass.Add("  WATER CUBE - inżynieryjna metafora wody "+
                "  Artykuł prezentuje innowacyjne rozwiązania w inteligentnym obiekcie pływalni olimpijskiej w Pekinie. "+
                    "  ARCHITEKTURA OBIEKTÓW SPORTOWYCH BUDYNEK INTELIGENTNY  ");
            ArchitectureClass.Add("  Budynek hotelu z wbudowaną kotłownią "+
                "  Projekt budowlany "+
                "  ARCHITEKTURA HOTELOWA  ");
            ArchitectureClass.Add("  Budynek mieszkalny jednorodzinny "+
                "  Projekt budowlany "+
                "  ARCHITEKTURA MIESZKANIOWA  ");
            ArchitectureClass.Add("  Budynek mieszkalny jednorodzinny z podziemnym zbiornikiem na ścieki sanitarne "+
                "  Projekt budowlany "+
                "  ARCHITEKTURA MIESZKANIOWA  ");
            ArchitectureClass.Add("  Projekt budowlany remont elewacji oraz docieplenie scian zewnętrznych i stropodachu " +
                "  Projekt budowlany "+
                "  ARCHITEKTURA  ");
            ArchitectureClass.Add("  Remont klatki schodowej Ministerstwo Sprawiedliwości "+
                "  Projekt wykonawczy "+
                "  ARCHITEKTURA UŻYTECZNOŚCI PUBLICZNEJ  ");
                */
            #endregion

            using (var dbContext = new ArticleDBDataModelContainer())
            {
                var content = dbContext.PG_ArticlesSet.SqlQuery(@"SELECT * FROM dbo.PG_ArticlesSet WHERE PG_ArticlesSet.abstractText LIKE '%ARCHITEKT%' OR PG_ArticlesSet.keywords LIKE '%ARCHITEKT%' OR PG_ArticlesSet.abstractText LIKE '%ARCHITEKT%';");
                foreach (var item in content)
                {
                    ArchitectureClass.Add(item.title + item.abstractText + item.keywords);
                }
            }

            return ArchitectureClass;
        }

        public static List<string> GeodesyClasseOfDocuments_ListCreations()
        {
            List<string> GeodesyClass = new List<string>();

            #region Geodesy_Class_Documnets
            /*
            GeodesyClass.Add("  Metody analizy obiektowej w badanich środowiska morskiego "+
                "  Monografia przedstawia metody klasyfikacji obrazów opierające się na analizie obiektowej. Autorzy prezentują wyniki eksperymentu dające podstawy do oceny analizy obiektowej jako konkurencyjne do klasyfikacji prowadzonej przez człowieka metodami manualnymi. "+
                "  GEODEZJA MORSKA GEOMATYKA SYSTEMY INFORMACJI PRZESTRZENNEJ TELEDETEKCJA  ");
            GeodesyClass.Add("  Metody analizy obiektowej w badanich środowiska morskiego "+
                "  Monografia przedstawia metody klasyfikacji obrazów opierające się na analizie obiektowej. Autorzy prezentują wyniki eksperymentu dające podstawy do oceny analizy obiektowej jako konkurencyjne do klasyfikacji prowadzonej przez człowieka metodami manualnymi. Autorzy: Katarzyna Mokwa, Marek Przyborski, Jerzy Pyrchla. Redaktor serii: Jakub Szulwic "+
                "  GEODEZJA MORSKA GEOMATYKA SYSTEMY INFORMACJI PRZESTRZENNEJ TELEDETEKCJA  ");
            GeodesyClass.Add("  Propozycja wykorzystania intensywności do wspomagania przetwarzania oryginalnej i zoptymalizowanej chmury punktów ALS "+
                "  Skaning lotniczy i przetwarzanie wyników - optymalizacja i klasyfikacja danych. "+
                "  GEODEZJA SKANING LASEROWY  ");
            GeodesyClass.Add("  M-Split Estimation in Laser Scanning Data Modeling "+
                "  Publikacja traktuje o wykorzystaniu estymacji M-Split do modelowania danych pozyskanych w wyniku skaningu laserowego. Autorzy prezentują rozwiązanie w oparciu o detekcję krawędzi dwóch płaszczyzn. "+
                "  DETEKCJA KRAWĘDZI GEODEZJA M-SPLIT SKANING LASEROWY  ");
                */
            #endregion

            using (var dbContext = new ArticleDBDataModelContainer())
            {
                var content = dbContext.PG_ArticlesSet.SqlQuery(@"SELECT * FROM dbo.PG_ArticlesSet WHERE (PG_ArticlesSet.abstractText LIKE '%GEODE%' OR PG_ArticlesSet.keywords LIKE '%GEODE%') OR (PG_ArticlesSet.abstractText LIKE '%GEODE%' OR PG_ArticlesSet.keywords LIKE '%GEODE%')");
                foreach (var item in content)
                {
                    GeodesyClass.Add(item.title + item.abstractText + item.keywords);
                }
            }

            return GeodesyClass;
        }

        public static List<string> SurveyAndMeasurementsClassOfDocuments_ListCreations()
        {
            List<string> SurveyAndMeasurementsClass = new List<string>();

            using (var dbContext = new ArticleDBDataModelContainer())
            {
                var content = dbContext.PG_ArticlesSet.SqlQuery(@"SELECT * FROM dbo.PG_ArticlesSet WHERE (PG_ArticlesSet.abstractText LIKE '%BADAN%' OR PG_ArticlesSet.keywords LIKE '%BADAN%') OR (PG_ArticlesSet.abstractText LIKE '%POMIAR%' OR PG_ArticlesSet.keywords LIKE '%POMIAR%')");
                foreach(var item in content)
                {
                    SurveyAndMeasurementsClass.Add(item.title + item.abstractText + item.keywords);
                }
            }

            return SurveyAndMeasurementsClass;
        }

        public static List<List<string>> ListOfClasses()
        {
            List<List<string>> ListOfClasses = new List<List<string>>();
            ListOfClasses.Add(ArchitectureClasseOfDocuments_ListCreations());
            ListOfClasses.Add(GeodesyClasseOfDocuments_ListCreations());
            ListOfClasses.Add(SurveyAndMeasurementsClassOfDocuments_ListCreations());
            return ListOfClasses;
        }
    }
}
