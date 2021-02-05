﻿using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace test
{
	public partial class ModificaAnagraficaSocieta : System.Web.UI.Page
	{
        string token;
        int idSocieta;

        protected void Page_Load(object sender, EventArgs e)
		{
            if (Session["ruolo"].ToString() == "Atleta") Response.Redirect("ModificaAnagraficaAtleta.aspx");
            if (Session["ruolo"].ToString() == "Admin" || Session["ruolo"].ToString() == "Delegato") Response.Redirect("ModificaAnagraficaDelegato.aspx");
            token = Session["Token"].ToString();
            idSocieta = Convert.ToInt32(Session["idUtente"]);
            if (!this.IsPostBack)
                DownloadSocieta(idSocieta);
        }
        protected void DownloadSocieta(int idSocieta)
        {
            var client = new RestClient("https://aibvcapi.azurewebsites.net/api/v1/societa/GetAnagraficaSocieta/" + idSocieta);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer  " + token + "");
            request.AddHeader("Cookie", "ARRAffinity=e7fc3e897f5be57469671ac828c06570ef8d3ea8fb2416293fd2acc3f67e0ee6; ARRAffinitySameSite=e7fc3e897f5be57469671ac828c06570ef8d3ea8fb2416293fd2acc3f67e0ee6; ruolo=Societa");
            IRestResponse response = client.Execute(request);
            //deserializza il risultato ritornato
            dynamic deserialzied = JsonConvert.DeserializeObject(response.Content);
            if (deserialzied != null)
            {
                if (deserialzied[0].affiliata.ToString() == "True") affiliata.Checked = true;
                for (int i = 0; i < deserialzied.Count; i++)
                {
                    nome.Text = deserialzied[i].nomeSocieta;
                    indirizzo.Text = deserialzied[i].indirizzo;
                    CAP.Text = deserialzied[i].cap;
                    dataFondazione.Text = deserialzied[i].dataFondazione;
                    dataAffiliazione.Text = deserialzied[i].dataAffiliazione;
                    codiceAffiliazione.Text = deserialzied[i].codiceAffiliazione;
                    email.Text = deserialzied[i].email;
                    sito.Text = deserialzied[i].sito;
                    tel1.Text = deserialzied[i].tel1;
                    tel2.Text = deserialzied[i].tel2;
                    PEC.Text = deserialzied[i].pec;
                    PIVA.Text = deserialzied[i].piva;
                    CF.Text = deserialzied[i].cf;
                    CU.Text = deserialzied[i].cu;
                }
            }
        }
        protected void ModificaAnagrafica_Click(object sender, EventArgs e)
        {
            bool affiliataS = false;
            if (affiliata.Checked) affiliataS = true; 
            var client = new RestClient("https://aibvcapi.azurewebsites.net/api/v1/LoginRegister/UpdateSocieta");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=e7fc3e897f5be57469671ac828c06570ef8d3ea8fb2416293fd2acc3f67e0ee6; ARRAffinitySameSite=e7fc3e897f5be57469671ac828c06570ef8d3ea8fb2416293fd2acc3f67e0ee6; ruolo=Admin");
            request.AddParameter("application/json", "{\r\n  \"societa\": {\r\n    \"idSocieta\": " + idSocieta + ",\r\n \"nomeSocieta\": \"" + nome.Text + "\",\r\n    \"indirizzo\": \"" + indirizzo.Text + "\",\r\n    \"cap\": \"" + CAP.Text + "\",\r\n    \"dataFondazione\": \"" + dataFondazione.Text + "\",\r\n    \"dataAffiliazione\": \"" + dataAffiliazione.Text + "\",\r\n    \"codiceAffiliazione\": \"" + codiceAffiliazione.Text + "\",\r\n    \"affiliata\": \""+affiliataS+"\",\r\n    \"email\": \"" + email.Text + "\",\r\n    \"sito\": \"" + sito.Text + "\",\r\n    \"tel1\": \"" + tel1.Text + "\",\r\n    \"tel2\": \"" + tel2.Text + "\",\r\n    \"pec\": \"" + PEC.Text + "\",\r\n    \"piva\": \"" + PIVA.Text + "\",\r\n    \"cf\": \"" + CF.Text + "\",\r\n    \"cu\": \"" + CU.Text + "\"\r\n  },\r\n  \"comuneSocieta\": \""+Comune.Text+"\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }
    }
}