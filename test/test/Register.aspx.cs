﻿using RestSharp;
using System;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace test
{
    public partial class RegisterAtleta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_registerAtleta_Click(object sender, EventArgs e)
        {
            int cont = 0;
            object parameter = "";
            if (ruolo.SelectedValue=="Atleta")
                 parameter = "{\r\n  \"atleta\": {\r\n";
            else if(ruolo.SelectedValue=="Allenatore")
                 parameter = "{\r\n  \"allenatore\": {\r\n";
            bool check = false;//controlla se sono arrivate le credenziali
            bool check_sesso = false;//controlla se arrivato il sesso
            foreach (Control c in formregister.Controls)//Controllo se tutte le textbox sono riempite
            {  
                if (c is TextBox)
                {
                    TextBox textBox = c as TextBox;
                    if (textBox.Text != string.Empty && textBox.AccessKey != "")//controllo se i campi obbligatori sono riempiti
                        cont++;
                    if ((textBox.ID== "comuneNascita" || textBox.ID == "comuneResidenza" || textBox.ID == "nomeSocieta" || textBox.ID == "password") && !check) //inserisco inizo credenziali
                    {
                        parameter = parameter.ToString().Substring(0, parameter.ToString().Length - 3);
                        parameter += "\r\n}\r\n}";
                        parameter += "},\r\n  \"cred\": {\r\n ";
                        check = !check;
                    }
                    if (textBox.Text != string.Empty)//Creo object body
                        parameter += " \"" + textBox.ID + "\": \"" + textBox.Text + "\",\r\n";

                }
                else if(c is RadioButton)
                {
                    RadioButton radioButton = c as RadioButton;
                    if (sesso1.Checked == true && !check_sesso)//Creo object body
                    { parameter += " \"sesso\": \"M\",\r\n"; check_sesso = !check_sesso; }
                    else if (sesso2.Checked == true&& !check_sesso)
                    { parameter += " \"sesso\": \"F\",\r\n"; check_sesso = !check_sesso; }  
                }
            }
            parameter= parameter.ToString().Substring(0,parameter.ToString().Length - 3);
            parameter += "\r\n}\r\n}";
            if ((ruolo.SelectedValue == "Atleta"&&cont == 8)|| (ruolo.SelectedValue == "Allenatore" && cont == 9))//ci sono 8/9 campi obbligatori
            {
                var client=new RestClient();
                //----------------------RegisterAtleta------------------------//
                if (ruolo.SelectedValue == "Atleta")
                    client = new RestClient("https://aibvcapi.azurewebsites.net/api/v1/registrati/RegistraAtleta");
                else if (ruolo.SelectedValue == "Allenatore")
                    client = new RestClient("https://aibvcapi.azurewebsites.net/api/v1/registrati/RegistraAllenatore");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json",parameter, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                    risultato.Text = response.Content;
                else
                    risultato.Text = response.ErrorMessage;
                //------------------------------------------------------------//
            }
            else
                risultato.Text = "Completare i campi obbligatori";
        }

        protected void ruolo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ruolo.SelectedValue == "Allenatore")
            {
                grado.Visible = true;
                lblgrado.Visible = true;
                altezza.Visible = false;
                lblAltezza.Visible = false;
                peso.Visible = false;
                lblPeso.Visible = false;
                dataScadenzaCertificato.Visible = false;
                lblDataScadCert.Visible = false;
            }
            else if (ruolo.SelectedValue == "Atleta") Page.Response.Redirect(Page.Request.Url.ToString(), true);//ricarica pagina base
        }
    }
}