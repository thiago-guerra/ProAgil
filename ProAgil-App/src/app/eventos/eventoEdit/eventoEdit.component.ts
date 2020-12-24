import { DatePipe } from '@angular/common';
import { DateTimeFormatPipePipe } from './../../_helps/DateTimeFormatPipe.pipe';
import { Evento } from './../../_models/Evento';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ActivatedRoute } from '@angular/router';
import { EventoService } from '../../_services/evento.service';

@Component({
  selector: 'app-evento-edit',
  templateUrl: './eventoEdit.component.html',
  styleUrls: ['./eventoEdit.component.css'],
  providers: [EventoService]
})
export class EventoEditComponent implements OnInit {
  titulo = 'Editar Evento';
  registerForm: FormGroup;
  evento: Evento = new Evento();
  dataEvento: string;
  imagemURL = 'assets/imagem/LogoTopo1.png';
  fileNameToUpdate : string;
  dataAtual : string;

  constructor(
    private fb: FormBuilder,
    private localService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService
    )
    {
      this.localService.use('pt-br');
    }

    get lotes(): FormArray{
      return <FormArray>this.registerForm.get('lotes');
    }

    get redesSociais(): FormArray{
      return <FormArray>this.registerForm.get('redesSociais');
    }

  ngOnInit() {
    this.validation();
    this.carregarEvento();
  }
  carregarEvento() {
    const idEvento = +this.router.snapshot.paramMap.get('id');
    this.eventoService.getEventoById(idEvento).subscribe(
      (evento: Evento) => {
          this.evento = Object.assign({}, evento);
         console.log(this.evento);
         this.evento.imageURL = '';
         this.registerForm.patchValue(evento);
         this.fileNameToUpdate = evento.imageURL.toString();
         this.imagemURL = `http://localhost:5000/Resources/Imagens/${this.evento.imageURL}}?_ts=${this.dataAtual}`;
         this.evento.lotes.forEach(lote => this.lotes.push(this.criaLote(lote)));
         this.evento.redesSociais.forEach(redeSocial => this.redesSociais.push(this.criaRedeSocial(redeSocial)));
      }
    );
  }
  
  
  validation() {
    this.registerForm = this.fb.group({
      id: [],
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imageURL: [''],
      telefone: ['', [Validators.required, Validators.minLength(9), Validators.maxLength(9)]],
      email: ['', [Validators.required, Validators.email]],
      lotes: this.fb.array([]) ,
      redesSociais: this.fb.array([])
    });
  }
  onFileChange(file: FileList){
    const reader = new FileReader();
    reader.onload = (event: any) => this.imagemURL = event.target.result;
    reader.readAsDataURL(file[0]);
  }

  criaLote(lote: any): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio, Validators.required],
      dataFim: [lote.dataFim, Validators.required]
    });
  }

  criaRedeSocial(redeSocial: any): FormGroup{
    return this.fb.group({
      id: [redeSocial.id],
      nome: [redeSocial.nome, Validators.required],
      url: [redeSocial.url, Validators.required]
    });
  }

  adicionarRedeSocial(){
    this.redesSociais.push(this.criaRedeSocial({id: 0}));
  }

  adicionarLote() {
    this.lotes.push(this.criaLote({id: 0}));
  }

  removeLote(index: number){
    this.lotes.removeAt(index);
  }

  removeRedeSocial(index: number){
    this.redesSociais.removeAt(index);
  }
}