﻿using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Informacni_System_Pojistovny.Models.Model.PojistnyProduktModels
{
    public class PojistnyProduktModel
    {
        private readonly Db db;
        public PojistnyProduktModel(Db db)
        {
            this.db = db;
        }

        /* pri false se ctou jen active */
        public List<PojistnyProdukt> ReadInsuranceProducts(bool activeInactive = true) {
            List<PojistnyProdukt> pojistnyProdukts = new List<PojistnyProdukt>();

            string sql;
            if (activeInactive)
            {
                sql = "select * from view_pojistne_produkty";
            }
            else {
                sql = "select * from view_pojistne_produkty where status = 'a'";
            }

            OracleDataReader dr = db.ExecuteRetrievingCommand(sql);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    PojistnyProdukt pojistnyProdukt = new PojistnyProdukt();
                    pojistnyProdukt.ID = int.Parse(dr["Produkt_id"].ToString());
                    pojistnyProdukt.Status = true;
                    pojistnyProdukt.MaximalniVysePlneni = int.Parse(dr["MAX_VYSE_PLNENI"].ToString());
                    pojistnyProdukt.Popis = dr["popis"].ToString();
                    pojistnyProdukt.Nazev = dr["nazev"].ToString();
                    string status = dr["status"].ToString();
                    if (status != null && status.Equals("a"))
                    {
                        pojistnyProdukt.Status = true;
                    }
                    else
                    {
                        pojistnyProdukt.Status = false;
                    }
                    pojistnyProdukts.Add(pojistnyProdukt);
                }
            }
            dr.Close();
            db.Dispose();
            return pojistnyProdukts;
        }
        public List<SelectListItem> ReadInsuranceProductsAsSelectListItems(bool activePassive)
        {
            List<PojistnyProdukt> pojistneProdukty = ReadInsuranceProducts(activePassive);
            List<SelectListItem> pojistneProduktySelectList = new List<SelectListItem>();
            pojistneProdukty.ForEach(p => { pojistneProduktySelectList.Add(new SelectListItem { Value = p.ID.ToString(), Text = p.Nazev }); });
            return pojistneProduktySelectList;
        }

        public bool CreateInsuranceProduct(PojistnyProduktInputModel pojistnyProduktInputModel) {
            Dictionary<string, object> pojistnyProduktParametry = new Dictionary<string, object>();
            pojistnyProduktParametry.Add(":v_nazev", pojistnyProduktInputModel.Nazev);
            pojistnyProduktParametry.Add(":v_popis", pojistnyProduktInputModel.Popis);
            pojistnyProduktParametry.Add(":v_max_vyse_plneni", pojistnyProduktInputModel.MaximalniVysePlneni);

            int pobockaId = db.ExecuteNonQuery("vytvorit_pojistny_produkt", pojistnyProduktParametry, false, true);
            return true;
        }

        public bool EditInsuranceProduct(PojistnyProduktInputModel pojistnyProduktInputModel, int id)
        {
            Dictionary<string, object> pojistnyProduktParametry = new Dictionary<string, object>();
            pojistnyProduktParametry.Add(":v_id", id);
            pojistnyProduktParametry.Add(":v_nazev", pojistnyProduktInputModel.Nazev);
            pojistnyProduktParametry.Add(":v_popis", pojistnyProduktInputModel.Popis);
            pojistnyProduktParametry.Add(":v_max_vyse_plneni", pojistnyProduktInputModel.MaximalniVysePlneni);

            int pobockaId = db.ExecuteNonQuery("zmenit_pojistny_produkt", pojistnyProduktParametry, false, true);
            return true;
        }

        public bool ChangeInsuranceProductStatus(int id)
        {
            Dictionary<string, object> pojistnyProduktParametry = new Dictionary<string, object>();
            pojistnyProduktParametry.Add(":v_id", id);

            db.ExecuteNonQuery("zmenit_stav_pojistny_produkt", pojistnyProduktParametry, false, true);
            return true;
        }

        public PojistnyProduktInputModel ReadInsuranceProductAsInputModel(int id) {
            Dictionary<string, object> pojistnyProduktParametry = new Dictionary<string, object>();
            pojistnyProduktParametry.Add(":v_id", id);
            PojistnyProduktInputModel pojistnyProdukt = null;
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from view_pojistne_produkty where produkt_id = :v_id", pojistnyProduktParametry);
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    pojistnyProdukt = new PojistnyProduktInputModel();
                    pojistnyProdukt.MaximalniVysePlneni = int.Parse(dr["MAX_VYSE_PLNENI"].ToString());
                    pojistnyProdukt.Popis = dr["popis"].ToString();
                    pojistnyProdukt.Nazev = dr["nazev"].ToString();
                }
            }
            dr.Close();
            db.Dispose();
            return pojistnyProdukt;
        }

        public PojistnyProdukt ReadInsuranceProduct(int id)
        {
            Dictionary<string, object> pojistnyProduktParametry = new Dictionary<string, object>();
            pojistnyProduktParametry.Add(":v_id", id);
            PojistnyProdukt pojistnyProdukt = null;
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from view_pojistne_produkty where produkt_id = :v_id", pojistnyProduktParametry);
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    pojistnyProdukt = new PojistnyProdukt();
                    pojistnyProdukt.ID = int.Parse(dr["Produkt_id"].ToString());
                    string status = dr["status"].ToString();
                    if (status != null && status.Equals("a"))
                    {
                        pojistnyProdukt.Status = true;
                    }
                    else
                    {
                        pojistnyProdukt.Status = false;
                    }
                    pojistnyProdukt.MaximalniVysePlneni = int.Parse(dr["MAX_VYSE_PLNENI"].ToString());
                    pojistnyProdukt.Popis = dr["popis"].ToString();
                    pojistnyProdukt.Nazev = dr["nazev"].ToString();
                }
            }
            dr.Close();
            db.Dispose();
            return pojistnyProdukt;
        }
    }
}
