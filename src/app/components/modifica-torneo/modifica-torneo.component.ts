import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { NavbarService } from '../../services/navbar.service';
import { SocietaApiService } from '../../services/societa-api.service';
import { TorneoAPIService } from '../../services/torneo-api.service';
import { CookieService } from 'ngx-cookie-service';
import { FormBuilder, Validators, FormGroup, FormArray, FormControl } from '@angular/forms';
import { TipoTorneo } from 'src/app/models/tipoTorneo.model';
import { FormulaTorneo } from 'src/app/models/formulaTorneo.model';
import { ParametroTorneo } from 'src/app/models/parametroTorneo.model';
import { Impianto } from 'src/app/models/impianto.model';
import { Torneo } from 'src/app/models/torneo.model';

@Component({
  selector: 'app-modifica-torneo',
  templateUrl: './modifica-torneo.component.html',
  styleUrls: ['./modifica-torneo.component.scss']
})
export class ModificaTorneoComponent implements OnInit {

  id:any;
  formrcrea: FormGroup;
  TipiTorneo!: TipoTorneo[];
  FormulaTorneo!: FormulaTorneo[];
  ParametroTorneo!: ParametroTorneo[];
  Impianti!: Impianto[];
  Info:any;
  elements:any;
  options = { year: 'numeric', month: 'numeric', day: 'numeric' }as const;//opzioni formato data
  sessocheck!:string;

  constructor(private TorneoAPIService: TorneoAPIService, private SocietaApiService: SocietaApiService, private router: Router, private activatedRoute: ActivatedRoute, private cookieService: CookieService, private navbarService: NavbarService, private formBuilder: FormBuilder) {
    
    this.id=this.activatedRoute.snapshot.paramMap.get('id');
    this.sessocheck='F';
    this.formrcrea = this.formBuilder.group({
      tipoTorneo: ['', Validators.required],
      titolo: ['', Validators.required],
      quotaIscrizione: ['', Validators.required],
      puntiVittoria: ['15', Validators.required],
      dataChiusuraIscrizioni: [(new Date()).toISOString().substring(0, 10), Validators.required],
      dataInizio: [(new Date()).toISOString().substring(0, 10), Validators.required],
      oraInizio: ['', Validators.required],
      dataFine: [(new Date()).toISOString().substring(0, 10), Validators.required],
      outdoor: ['false', Validators.required],
      riunioneTecnica: ['false', Validators.required],
      genere: ['', Validators.required],
      numMaxTeamMainDraw: ['', Validators.required],
      numMaxTeamQualifiche: ['', Validators.required],
      numTeamQualificati: ['', Validators.required],
      numWildCard: ['', Validators.required],
      montepremi: ['', Validators.required],
      formulaTorneo: ['', Validators.required],
      parametriTorneo: this.formBuilder.array([], [Validators.required]),
      idImpianto: ['', Validators.required],
      tour: ['', Validators.required]
    });
  }



  ngOnInit(): void {
    //scarico tutti i tipi torneo
    this.TorneoAPIService.getTipiTorneo().subscribe(
      obj => {
        this.TipiTorneo = obj;
      },
      error => {
        console.log("error", error);
      })
    //scarico tutte le formule torneo
    this.TorneoAPIService.getFormulaTorneo().subscribe(
      obj => {
        this.FormulaTorneo = obj;
      },
      error => {
        console.log("error", error);
      })
    //scarico tutti i paramerti torneo
    this.TorneoAPIService.getParametriTorneo().subscribe(
      obj => {
        this.ParametroTorneo = obj;
      },
      error => {
        console.log("error", error);
      })
    //scarico tutti gli impianti di una societa
    this.TorneoAPIService.getImpiantiSocieta(parseInt(this.cookieService.get('id')), this.cookieService.get('token')).subscribe(
      obj => {
        this.Impianti = obj;
      },
      error => {
        console.log("error", error);
      })

      this.TorneoAPIService.getInfoTorneo(parseInt(this.id)).subscribe(
        obj => {
        this.Info=obj  as Torneo;
        console.log("INFO", this.Info);
        this.setData();
        },
        error => {
          console.log("error", error);
      })
  }

  setData(){
    this.formrcrea = this.formBuilder.group({
      tipoTorneo: [this.Info[0]["idTipoTorneo"], Validators.required],
      titolo: [this.Info[0]["titolo"], Validators.required],
      quotaIscrizione: [this.Info[0]["quotaIscrizione"], Validators.required],
      puntiVittoria: [this.Info[0]["puntiVittoria"], Validators.required],
      dataChiusuraIscrizioni: [new Date(this.Info[0]["dataChiusuraIscrizioni"]).toLocaleString('en-CA',this.options), Validators.required],
      dataInizio: [new Date(this.Info[0]["dataInizio"]).toLocaleString('en-CA',this.options), Validators.required],
      oraInizio: [this.Info[0]["oraInizio"], Validators.required],
      dataFine: [new Date(this.Info[0]["dataFine"]).toLocaleString('en-CA',this.options), Validators.required],
      outdoor: [this.Info[0]["outdoor"], Validators.required],
      riunioneTecnica: [this.Info[0]["riunioneTecnica"], Validators.required],
      genere: [this.Info[0]["gender"], Validators.required],
      numMaxTeamMainDraw: [this.Info[0]["numMaxTeamMainDraw"], Validators.required],
      numMaxTeamQualifiche: [this.Info[0]["numMaxTeamQualifiche"], Validators.required],
      numTeamQualificati: [this.Info[0]["numTeamQualificati"], Validators.required],
      numWildCard: [this.Info[0]["numWildCard"], Validators.required],
      montepremi: [this.Info[0]["montepremi"], Validators.required],
      formulaTorneo: [this.Info[0]["idFormula"], Validators.required],
      parametriTorneo: this.formBuilder.array([], [Validators.required]),
      idImpianto: [this.Info[0]["idImpianto"], Validators.required],
      tour: [this.Info[0]["tour"], Validators.required]
    });
    
  }

  onCheckboxChange(e) {
    const checkArray: FormArray = this.formrcrea.get('parametriTorneo') as FormArray;
    if (e.target.checked) {
      checkArray.push(new FormControl(e.target.value));
    } else {
      let i: number = 0;
      checkArray.controls.forEach(item => {
        if (item.value == e.target.value) {
          checkArray.removeAt(i);
          return;
        }
        i++;
      });
    }
    //console.log(this.formrcrea.value);
  }

  Crea_Click() {
    let body = "{\"idTorneo\": \""+this.id+"\",	\r\n";

    Object.keys(this.formrcrea.controls).forEach(key => {
      //scorro tutti gli input e creo il body della chiamata
      if (this.formrcrea.controls[key].status == "VALID")
        if (key == "parametriTorneo") body += (() => {
          let params = "\"" + key + "\":[";
          this.formrcrea.controls[key].value.forEach(element => {
            params += "\"" + element + "\",";
          });
          params = params.substring(0, params.length - 1);//elimino la virgola finale
          params += "],\r\n"
          return params;
        })();
        else
          body += " \"" + key + "\": \"" + this.formrcrea.controls[key].value + "\",\r\n";
    });
    body += " \"idSocieta\": \"" + parseInt(this.cookieService.get('id')) + "\"\r\n}";
    //console.log("Body",body);
    //CREA TORNEO
    this.TorneoAPIService.ModificaTorneo(this.cookieService.get('token'), body).subscribe(
      obj => {
        this.Info = obj;
        //torno ai tornei
        this.router.navigate(['OutputTornei/L1/false']);
      },
      error => {
        console.log("error", error);
      })
  }
}
