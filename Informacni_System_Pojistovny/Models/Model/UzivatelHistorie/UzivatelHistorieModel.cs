using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model.Uzivatele;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;

namespace Informacni_System_Pojistovny.Models.Model.UzivatelHistorie
{
    public class UzivatelHistorieModel
    {
        private readonly Db db;
        public UzivatelHistorieModel(Db db)
        {
            this.db = db;
        }
        public List<HistorieUzivatele> ListHistorie(PageInfo pageInfo)
        {
            List<HistorieUzivatele> list = new List<HistorieUzivatele>();

            int pageStart = pageInfo.PageIndex * pageInfo.PageSize;
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":pageStart", pageStart },
                { ":pageSize", pageInfo.PageSize }
            };

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from historie_uzivatele_view order by cas_historie DESC OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY", parameters);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string idHistorie = dr["historie_id"]?.ToString();

                    string old_mail = dr["old_mail"]?.ToString();
                    string old_jmeno = dr["old_jmeno"]?.ToString();
                    string old_prijmeni = dr["old_prijmeni"]?.ToString();
                    string old_role = dr["old_uzivatel_role"]?.ToString();
                    string old_idString = dr["OLD_uzivatel_id"]?.ToString();

                    string new_mail = dr["new_mail"]?.ToString();
                    string new_jmeno = dr["new_jmeno"]?.ToString();
                    string new_prijmeni = dr["new_prijmeni"]?.ToString();
                    string new_role = dr["new_uzivatel_role"]?.ToString();
                    string new_idString = dr["new_uzivatel_id"]?.ToString();

                    DMLTyp ddlTyp = DMLTypeRetriever.GetByName(dr["DDL_TYP"]?.ToString());

                    DateTime.TryParse(dr["cas_historie"]?.ToString(), out DateTime casHistorie);
                    int.TryParse(idHistorie, out var idHistorieInt);
                    int.TryParse(old_idString, out var old_idStringInt);
                    int.TryParse(new_idString, out var new_idStringInt);
                    list.Add(new HistorieUzivatele
                    {
                        OldId = old_idStringInt,
                        OldEmail = old_mail,
                        OldJmeno = old_jmeno,
                        OldPrijmeni = old_prijmeni,
                        OldRole = UzivateleRoleRetriever.GetByName(old_role),

                        NewId = new_idStringInt,
                        NewEmail = new_mail,
                        NewJmeno = new_jmeno,
                        NewPrijmeni = new_prijmeni,
                        NewRole = UzivateleRoleRetriever.GetByName(new_role),

                        CasHistorie = casHistorie,
                        DDLTyp = ddlTyp,

                        Id = idHistorieInt,
                    });
                }
            }
            db.Dispose();
            return list;
        }

        public List<HistorieUzivatele> ListHistorie()
        {
            List<HistorieUzivatele> list = new List<HistorieUzivatele>();

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from historie_uzivatele_view order by cas_historie DESC");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string idHistorie = dr["historie_id"]?.ToString();

                    string old_mail = dr["old_mail"]?.ToString();
                    string old_jmeno = dr["old_jmeno"]?.ToString();
                    string old_prijmeni = dr["old_prijmeni"]?.ToString();
                    string old_role = dr["old_uzivatel_role"]?.ToString();
                    string old_idString = dr["old_uzivatel_id"]?.ToString();

                    string new_mail = dr["new_mail"]?.ToString();
                    string new_jmeno = dr["new_jmeno"]?.ToString();
                    string new_prijmeni = dr["new_prijmeni"]?.ToString();
                    string new_role = dr["new_uzivatel_role"]?.ToString();
                    string new_idString = dr["new_uzivatel_id"]?.ToString();

                    DMLTyp ddlTyp = DMLTypeRetriever.GetByName(dr["DDL_TYP"]?.ToString());

                    DateTime.TryParse(dr["cas_zmeny"]?.ToString(), out DateTime casZmeny);
                    int.TryParse(idHistorie, out var idHistorieInt);
                    int.TryParse(old_idString, out var old_idStringInt);
                    int.TryParse(new_idString, out var new_idStringInt);
                    list.Add(new HistorieUzivatele
                    {
                        OldId = old_idStringInt,
                        OldEmail = old_mail,
                        OldJmeno = old_jmeno,
                        OldPrijmeni = old_prijmeni,
                        OldRole = UzivateleRoleRetriever.GetByName(old_role),

                        NewId = new_idStringInt,
                        NewEmail = new_mail,
                        NewJmeno = new_jmeno,
                        NewPrijmeni = new_prijmeni,
                        NewRole = UzivateleRoleRetriever.GetByName(new_role),

                        DDLTyp = ddlTyp,

                        Id = idHistorieInt,
                    });
                }
            }
            db.Dispose();
            return list;
        }

        public long GetCount()
        {
            long count = 0;
            Db db = new Db();
            OracleDataReader dr = db.ExecuteRetrievingCommand("select count(*) as count from historie_uzivatele_view");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    count = long.Parse(dr["count"].ToString());
                }
            }
            dr.Close();
            db.Dispose();
            return count;
        }

        public HistorieUzivatele GetHistorie(int id)
        {
            HistorieUzivatele historie = null;

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":historie_id", id },

            };

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from historie_uzivatele_view where historie_id = :historie_id", parameters);
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    string idHistorie = dr["historie_id"]?.ToString();

                    string old_mail = dr["old_mail"]?.ToString();
                    string old_jmeno = dr["old_jmeno"]?.ToString();
                    string old_prijmeni = dr["old_prijmeni"]?.ToString();
                    string old_role = dr["old_uzivatel_role"]?.ToString();
                    string old_idString = dr["old_uzivatel_id"]?.ToString();

                    string new_mail = dr["new_mail"]?.ToString();
                    string new_jmeno = dr["new_jmeno"]?.ToString();
                    string new_prijmeni = dr["new_prijmeni"]?.ToString();
                    string new_role = dr["new_uzivatel_role"]?.ToString();
                    string new_idString = dr["new_uzivatel_id"]?.ToString();

                    DMLTyp ddlTyp = DMLTypeRetriever.GetByName(dr["DDL_TYP"]?.ToString());

                    DateTime.TryParse(dr["cas_historie"]?.ToString(), out DateTime casZmeny);

                    DateTime.TryParse(dr["old_cas_zmeny"].ToString(), out DateTime oldCasZmeny);
                    DateTime? nullableOldCasZmeny = oldCasZmeny;
                    if (oldCasZmeny.Equals(DateTime.MinValue))
                    {
                        nullableOldCasZmeny = null;
                    }

                    DateTime.TryParse(dr["new_cas_zmeny"].ToString(), out DateTime newCasZmeny);
                    DateTime? nullableNewCasZmeny = newCasZmeny;
                    if (newCasZmeny.Equals(DateTime.MinValue))
                    {
                        nullableNewCasZmeny = null;
                    }


                    int.TryParse(idHistorie, out var idHistorieInt);
                    int.TryParse(old_idString, out var old_idStringInt);
                    int.TryParse(new_idString, out var new_idStringInt);
                    historie = new HistorieUzivatele
                    {
                        OldId = old_idStringInt,
                        OldEmail = old_mail,
                        OldJmeno = old_jmeno,
                        OldPrijmeni = old_prijmeni,
                        OldRole = UzivateleRoleRetriever.GetByName(old_role),

                        NewId = new_idStringInt,
                        NewEmail = new_mail,
                        NewJmeno = new_jmeno,
                        NewPrijmeni = new_prijmeni,
                        NewRole = UzivateleRoleRetriever.GetByName(new_role),

                        DDLTyp = ddlTyp,
                        CasHistorie = casZmeny,
                        Id = idHistorieInt,
                        NewCasZmeny = nullableNewCasZmeny,
                        OldcasZmeny = nullableOldCasZmeny,
                    };
                }
            }
            db.Dispose();
            return historie;
        }

    }


}
